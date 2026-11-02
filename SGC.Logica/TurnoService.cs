using Microsoft.EntityFrameworkCore;
using SGC.Datos;
using SGC.Entidades;

namespace SGC.Logica;

public class TurnoService
{
    private readonly AgendaMedicoService _agendaMedicoService = new();

    public List<Turno> ObtenerTodos(bool incluirCancelados = false, int? medicoId = null, DateOnly? fecha = null, int? pacienteId = null)
    {
        using var contexto = SGCContextFactory.Crear();
        IQueryable<Turno> query = contexto.Turnos
            .Include(t => t.Paciente).ThenInclude(p => p!.ObraSocial)
            .Include(t => t.Medico).ThenInclude(m => m!.ObrasSocialesAceptadas)
            .Include(t => t.ActividadMedica);

        query = incluirCancelados ? query : query.Where(t => t.Activo);

        if (medicoId != null)
            query = query.Where(t => t.MedicoId == medicoId);

        if (fecha != null)
            query = query.Where(t => t.Fecha == fecha);

        if (pacienteId != null)
            query = query.Where(t => t.PacienteId == pacienteId);

        return query.OrderBy(t => t.Fecha).ThenBy(t => t.HoraInicio).ToList();
    }

    public List<Turno> ObtenerPorMedicoYFecha(int medicoId, DateOnly? fecha = null, bool incluirCancelados = false)
    {
        using var contexto = SGCContextFactory.Crear();

        var pMedicoId = new Microsoft.Data.SqlClient.SqlParameter("@MedicoId", medicoId);
        var pFecha = new Microsoft.Data.SqlClient.SqlParameter("@Fecha", (object?)fecha?.ToString("yyyy-MM-dd") ?? DBNull.Value);
        var pIncluirCancelados = new Microsoft.Data.SqlClient.SqlParameter("@IncluirCancelados", incluirCancelados ? 1 : 0);

        // En SQL Server, un EXEC a un Stored Procedure no es 'composable' (no permite JOIN / Include directo en el servidor).
        // Traemos los turnos devueltos por el SP a memoria (AsEnumerable) e hidratamos las relaciones.
        var turnos = contexto.Turnos
            .FromSqlRaw("EXEC sp_ObtenerTurnosPorMedicoYFecha @MedicoId, @Fecha, @IncluirCancelados",
                        pMedicoId, pFecha, pIncluirCancelados)
            .AsEnumerable()
            .OrderBy(t => t.Fecha)
            .ThenBy(t => t.HoraInicio)
            .ToList();

        // Cargar navegaciones explicitamente para los turnos obtenidos
        if (turnos.Count > 0)
        {
            var turnosIds = turnos.Select(t => t.Id).ToList();
            contexto.Turnos
                .Where(t => turnosIds.Contains(t.Id))
                .Include(t => t.Paciente).ThenInclude(p => p!.ObraSocial)
                .Include(t => t.Medico).ThenInclude(m => m!.ObrasSocialesAceptadas)
                .Include(t => t.ActividadMedica)
                .Load();
        }

        return turnos;
    }

    public Turno? ObtenerPorId(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.Turnos
            .Include(t => t.Paciente).ThenInclude(p => p!.ObraSocial)
            .Include(t => t.Medico).ThenInclude(m => m!.ObrasSocialesAceptadas)
            .Include(t => t.ActividadMedica)
            .FirstOrDefault(t => t.Id == id);
    }

    // Regla de cobro (la usan FormConfirmarAsistencia y la grilla de FormTurnos):
    // la obra social del paciente solo cuenta si ESTE medico la acepta
    // (Medico.ObrasSocialesAceptadas); si no, el paciente paga particular.
    // El turno debe venir con Paciente.ObraSocial y Medico.ObrasSocialesAceptadas
    // cargados (ObtenerTodos/ObtenerPorId ya los incluyen).
    public static bool MedicoAceptaObraSocialDelPaciente(Turno turno)
    {
        var obraSocial = turno.Paciente?.ObraSocial;
        // Una obra social dada de baja no aplica descuento: paga particular.
        return obraSocial != null && obraSocial.Activo &&
               (turno.Medico?.ObrasSocialesAceptadas.Any(o => o.Id == obraSocial.Id) ?? false);
    }

    public static decimal CalcularMontoParticular(Turno turno)
    {
        return turno.Medico?.PrecioConsultaParticular ?? 0;
    }

    // Precio particular menos el % de cobertura de la obra social del paciente.
    public static decimal CalcularMontoConObraSocial(Turno turno)
    {
        decimal precioBase = CalcularMontoParticular(turno);
        var obraSocial = turno.Paciente?.ObraSocial;
        decimal cobertura = obraSocial != null && obraSocial.Activo ? obraSocial.PorcentajeCobertura : 0;
        return Math.Max(precioBase - (precioBase * cobertura / 100m), 0);
    }

    // Monto estimado a cobrar: con obra social si el medico la acepta, o el
    // precio particular en caso contrario.
    public static decimal CalcularMontoEstimado(Turno turno)
    {
        return MedicoAceptaObraSocialDelPaciente(turno)
            ? CalcularMontoConObraSocial(turno)
            : CalcularMontoParticular(turno);
    }

    public void AsignarTurno(Paciente paciente, Medico medico, DateOnly fecha, TimeOnly horaInicio, int duracionMinutos = 30)
    {
        ValidarDuracion(horaInicio, duracionMinutos);
        TimeOnly horaFin = horaInicio.AddMinutes(duracionMinutos);

        if (fecha < DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("No se puede asignar un turno en una fecha pasada.");

        if (EsHorarioPasado(fecha, horaInicio))
            throw new ArgumentException("No se puede asignar un turno en un horario que ya pasó.");

        if (!_agendaMedicoService.MedicoAtiende(medico.Id, fecha.DayOfWeek, horaInicio, duracionMinutos))
            throw new InvalidOperationException(
                $"El profesional no atiende el {AgendaMedico.NombreDia(fecha.DayOfWeek)} en el horario {horaInicio:HH:mm} - {horaFin:HH:mm}.");

        using var contexto = SGCContextFactory.Crear();

        // Se vuelve a leer de la base: los objetos que llegan de la UI pueden
        // estar desactualizados (alguien pudo dar de baja al paciente o medico).
        ExigirActivos(contexto, paciente.Id, medico.Id);
        ExigirSinAusencia(contexto, medico.Id, medico.Apellido, fecha, horaInicio, duracionMinutos);

        if (HaySuperposicion(contexto, medico.Id, fecha, horaInicio, duracionMinutos, excluirTurnoId: null))
            throw new InvalidOperationException(
                $"El Dr./Dra. {medico.Apellido} ya tiene un turno asignado el {fecha:dd/MM/yyyy} que se superpone con {horaInicio:HH:mm} - {horaFin:HH:mm}.");

        ExigirPacienteLibre(contexto, paciente.Id, fecha, horaInicio, duracionMinutos, excluirTurnoId: null);

        var turno = new Turno
        {
            PacienteId = paciente.Id,
            MedicoId = medico.Id,
            Fecha = fecha,
            HoraInicio = horaInicio,
            DuracionMinutos = duracionMinutos,
            Estado = EstadoTurno.Confirmado,
            Activo = true
        };

        contexto.Turnos.Add(turno);
        contexto.SaveChanges();
    }

    public void ModificarTurno(int turnoId, DateOnly nuevaFecha, TimeOnly nuevaHoraInicio, int nuevaDuracionMinutos = 30)
    {
        ValidarDuracion(nuevaHoraInicio, nuevaDuracionMinutos);
        TimeOnly nuevaHoraFin = nuevaHoraInicio.AddMinutes(nuevaDuracionMinutos);

        if (nuevaFecha < DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("No se puede modificar un turno a una fecha pasada.");

        if (EsHorarioPasado(nuevaFecha, nuevaHoraInicio))
            throw new ArgumentException("No se puede mover un turno a un horario que ya pasó.");

        using var contexto = SGCContextFactory.Crear();
        var turno = contexto.Turnos.Include(t => t.Medico).FirstOrDefault(t => t.Id == turnoId)
            ?? throw new InvalidOperationException("El turno que intenta modificar no existe.");

        if (turno.Estado != EstadoTurno.Confirmado)
            throw new InvalidOperationException(turno.Estado == EstadoTurno.Cancelado
                ? "No se puede modificar un turno cancelado."
                : "No se puede modificar un turno que ya fue atendido o marcado como ausente.");

        ExigirActivos(contexto, turno.PacienteId, turno.MedicoId);

        if (!_agendaMedicoService.MedicoAtiende(turno.MedicoId, nuevaFecha.DayOfWeek, nuevaHoraInicio, nuevaDuracionMinutos))
            throw new InvalidOperationException(
                $"El profesional no atiende el {AgendaMedico.NombreDia(nuevaFecha.DayOfWeek)} en el horario {nuevaHoraInicio:HH:mm} - {nuevaHoraFin:HH:mm}.");

        ExigirSinAusencia(contexto, turno.MedicoId, turno.Medico?.Apellido, nuevaFecha, nuevaHoraInicio, nuevaDuracionMinutos);

        if (HaySuperposicion(contexto, turno.MedicoId, nuevaFecha, nuevaHoraInicio, nuevaDuracionMinutos, excluirTurnoId: turnoId))
            throw new InvalidOperationException(
                $"El Dr./Dra. {turno.Medico?.Apellido} ya tiene otro turno asignado el {nuevaFecha:dd/MM/yyyy} que se superpone con {nuevaHoraInicio:HH:mm} - {nuevaHoraFin:HH:mm}.");

        ExigirPacienteLibre(contexto, turno.PacienteId, nuevaFecha, nuevaHoraInicio, nuevaDuracionMinutos, excluirTurnoId: turnoId);

        turno.Fecha = nuevaFecha;
        turno.HoraInicio = nuevaHoraInicio;
        turno.DuracionMinutos = nuevaDuracionMinutos;
        contexto.SaveChanges();
    }

    // Duracion entre 5 y 240 minutos y turno que termina dentro del mismo dia
    // (la cuenta es en minutos: con TimeOnly un turno de 23:30 + 45 min
    // quedaria "terminando" a las 00:15 y pasaria las validaciones).
    private static void ValidarDuracion(TimeOnly horaInicio, int duracionMinutos)
    {
        if (duracionMinutos < 5 || duracionMinutos > 240)
            throw new ArgumentException("La duración del turno debe estar entre 5 y 240 minutos.");

        if (Horas.Minutos(horaInicio) + duracionMinutos > 24 * 60)
            throw new ArgumentException("El turno no puede terminar después de las 24:00 (pasaría al día siguiente).");
    }

    // Un turno no puede caer dentro de una ausencia activa del medico.
    private static void ExigirSinAusencia(SGCContext contexto, int medicoId, string? apellido, DateOnly fecha, TimeOnly horaInicio, int duracionMinutos)
    {
        var ausencia = ExcepcionAgendaService.BuscarAusencia(contexto, medicoId, fecha, horaInicio, duracionMinutos);
        if (ausencia != null)
            throw new InvalidOperationException(
                $"El Dr./Dra. {apellido} tiene una ausencia cargada el {fecha:dd/MM/yyyy} ({ExcepcionAgendaService.Describir(ausencia)}: {ausencia.Motivo}) que se superpone con ese horario.");
    }

    // Un paciente no puede estar en dos turnos a la vez, aunque sean con
    // medicos distintos.
    private static void ExigirPacienteLibre(SGCContext contexto, int pacienteId, DateOnly fecha, TimeOnly horaInicio, int duracionMinutos, int? excluirTurnoId)
    {
        int inicio = Horas.Minutos(horaInicio), fin = inicio + duracionMinutos;

        var choque = contexto.Turnos
            .Include(t => t.Medico)
            .Where(t => t.Activo && t.Estado != EstadoTurno.Cancelado
                        && t.PacienteId == pacienteId && t.Fecha == fecha && t.Id != (excluirTurnoId ?? 0))
            .AsEnumerable()
            .FirstOrDefault(t => Horas.Superpone(inicio, fin, Horas.Minutos(t.HoraInicio), Horas.Minutos(t.HoraInicio) + t.DuracionMinutos));

        if (choque != null)
            throw new InvalidOperationException(
                $"El paciente ya tiene un turno el {fecha:dd/MM/yyyy} de {choque.HoraInicio:HH:mm} a {choque.HoraFin:HH:mm} con el Dr./Dra. {choque.Medico?.Apellido} que se superpone con este horario.");
    }

    private static void ExigirActivos(SGCContext contexto, int pacienteId, int medicoId)
    {
        var paciente = contexto.Pacientes.FirstOrDefault(p => p.Id == pacienteId);
        if (paciente is null || !paciente.Activo)
            throw new InvalidOperationException("El paciente está dado de baja: no se pueden asignar ni modificar sus turnos.");

        var medico = contexto.Medicos.FirstOrDefault(m => m.Id == medicoId);
        if (medico is null || !medico.Activo)
            throw new InvalidOperationException("El médico está dado de baja: no se pueden asignar ni modificar sus turnos.");
    }

    // true si la fecha es hoy y la hora de inicio ya paso (fechas anteriores
    // se validan aparte). Tambien lo usa la UI para marcar horarios no disponibles.
    public static bool EsHorarioPasado(DateOnly fecha, TimeOnly horaInicio)
    {
        return fecha.ToDateTime(horaInicio) < DateTime.Now;
    }

    public bool HorarioOcupado(int medicoId, DateOnly fecha, TimeOnly horaInicio, int duracionMinutos = 30)
    {
        using var contexto = SGCContextFactory.Crear();
        return HaySuperposicion(contexto, medicoId, fecha, horaInicio, duracionMinutos, excluirTurnoId: null);
    }

    public void ConfirmarAsistencia(int turnoId, bool asistio, string? medioPago, decimal? monto)
    {
        using var contexto = SGCContextFactory.Crear();
        var turno = contexto.Turnos.FirstOrDefault(t => t.Id == turnoId)
            ?? throw new InvalidOperationException("El turno no existe.");

        if (turno.Estado != EstadoTurno.Confirmado)
            throw new InvalidOperationException("Solo se puede confirmar asistencia de un turno en estado Confirmado.");

        if (turno.Fecha > DateOnly.FromDateTime(DateTime.Today))
            throw new InvalidOperationException("No se puede confirmar la asistencia de un turno que todavía no llegó a su fecha.");

        if (asistio)
        {
            if (string.IsNullOrWhiteSpace(medioPago))
                throw new ArgumentException("Debe indicar el medio de pago cuando el paciente asistió.");

            if (monto == null || monto < 0)
                throw new ArgumentException("Debe indicar un monto válido (no puede ser negativo) cuando el paciente asistió.");

            turno.Estado = EstadoTurno.Asistio;
            turno.MedioPago = medioPago;
            turno.Monto = monto;
        }
        else
        {
            turno.Estado = EstadoTurno.Ausente;
            turno.MedioPago = null;
            turno.Monto = null;
        }

        contexto.SaveChanges();
    }

    public void CancelarTurno(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        var turno = contexto.Turnos.FirstOrDefault(t => t.Id == id)
            ?? throw new InvalidOperationException("El turno que intenta cancelar no existe.");

        if (turno.Estado == EstadoTurno.Cancelado)
            throw new InvalidOperationException("Ese turno ya estaba cancelado.");

        if (turno.Estado != EstadoTurno.Confirmado)
            throw new InvalidOperationException("No se puede cancelar un turno que ya fue atendido o marcado como ausente.");

        turno.Estado = EstadoTurno.Cancelado;
        turno.Activo = false;
        contexto.SaveChanges();
    }

    // HoraFin es una propiedad calculada (no existe como columna), asi que
    // la superposicion no se puede traducir a SQL directo: se trae el dia
    // completo del medico (consulta chica, se filtra bien por indice) y se
    // resuelve el solapamiento de rangos en memoria.
    private static bool HaySuperposicion(SGCContext contexto, int medicoId, DateOnly fecha, TimeOnly horaInicio, int duracionMinutos, int? excluirTurnoId)
    {
        // En minutos para que un turno que cruza la medianoche no se compare mal.
        int inicio = Horas.Minutos(horaInicio), fin = inicio + duracionMinutos;

        return contexto.Turnos
            .Where(t => t.Activo && t.MedicoId == medicoId && t.Fecha == fecha && t.Id != (excluirTurnoId ?? 0))
            .AsEnumerable()
            .Any(t => Horas.Superpone(inicio, fin, Horas.Minutos(t.HoraInicio), Horas.Minutos(t.HoraInicio) + t.DuracionMinutos));
    }
}
