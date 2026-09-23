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
            .Include(t => t.Paciente)
            .Include(t => t.Medico)
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
        IQueryable<Turno> query = contexto.Turnos
            .Include(t => t.Paciente)
            .Include(t => t.Medico)
            .Include(t => t.ActividadMedica)
            .Where(t => t.MedicoId == medicoId);

        query = incluirCancelados ? query : query.Where(t => t.Activo);

        if (fecha.HasValue)
            query = query.Where(t => t.Fecha == fecha.Value);

        return query.OrderBy(t => t.Fecha).ThenBy(t => t.HoraInicio).ToList();
    }

    public Turno? ObtenerPorId(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.Turnos
            .Include(t => t.Paciente)
            .Include(t => t.Medico)
            .Include(t => t.ActividadMedica)
            .FirstOrDefault(t => t.Id == id);
    }

    public void AsignarTurno(Paciente paciente, Medico medico, DateOnly fecha, TimeOnly horaInicio, int duracionMinutos = 30)
    {
        if (duracionMinutos <= 0) duracionMinutos = 30;
        TimeOnly horaFin = horaInicio.AddMinutes(duracionMinutos);

        if (fecha < DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("No se puede asignar un turno en una fecha pasada.");

        if (!_agendaMedicoService.MedicoAtiende(medico.Id, fecha.DayOfWeek, horaInicio, duracionMinutos))
            throw new InvalidOperationException(
                $"El profesional no atiende el {AgendaMedico.NombreDia(fecha.DayOfWeek)} en el horario {horaInicio:HH:mm} - {horaFin:HH:mm}.");

        using var contexto = SGCContextFactory.Crear();

        if (HaySuperposicion(contexto, medico.Id, fecha, horaInicio, duracionMinutos, excluirTurnoId: null))
            throw new InvalidOperationException(
                $"El Dr./Dra. {medico.Apellido} ya tiene un turno asignado el {fecha:dd/MM/yyyy} que se superpone con {horaInicio:HH:mm} - {horaFin:HH:mm}.");

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
        if (nuevaDuracionMinutos <= 0) nuevaDuracionMinutos = 30;
        TimeOnly nuevaHoraFin = nuevaHoraInicio.AddMinutes(nuevaDuracionMinutos);

        if (nuevaFecha < DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("No se puede modificar un turno a una fecha pasada.");

        using var contexto = SGCContextFactory.Crear();
        var turno = contexto.Turnos.Include(t => t.Medico).FirstOrDefault(t => t.Id == turnoId)
            ?? throw new InvalidOperationException("El turno que intenta modificar no existe.");

        if (turno.Estado == EstadoTurno.Cancelado)
            throw new InvalidOperationException("No se puede modificar un turno cancelado.");

        if (!_agendaMedicoService.MedicoAtiende(turno.MedicoId, nuevaFecha.DayOfWeek, nuevaHoraInicio, nuevaDuracionMinutos))
            throw new InvalidOperationException(
                $"El profesional no atiende el {AgendaMedico.NombreDia(nuevaFecha.DayOfWeek)} en el horario {nuevaHoraInicio:HH:mm} - {nuevaHoraFin:HH:mm}.");

        if (HaySuperposicion(contexto, turno.MedicoId, nuevaFecha, nuevaHoraInicio, nuevaDuracionMinutos, excluirTurnoId: turnoId))
            throw new InvalidOperationException(
                $"El Dr./Dra. {turno.Medico?.Apellido} ya tiene otro turno asignado el {nuevaFecha:dd/MM/yyyy} que se superpone con {nuevaHoraInicio:HH:mm} - {nuevaHoraFin:HH:mm}.");

        turno.Fecha = nuevaFecha;
        turno.HoraInicio = nuevaHoraInicio;
        turno.DuracionMinutos = nuevaDuracionMinutos;
        contexto.SaveChanges();
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
            throw new InvalidOperationException("No se puede confirmar la asistencia de un turno que todavia no llego a su fecha.");

        if (asistio)
        {
            if (string.IsNullOrWhiteSpace(medioPago))
                throw new ArgumentException("Debe indicar el medio de pago cuando el paciente asistio.");

            if (monto == null || monto < 0)
                throw new ArgumentException("Debe indicar un monto valido (no puede ser negativo) cuando el paciente asistio.");

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
        TimeOnly horaFin = horaInicio.AddMinutes(duracionMinutos);

        return contexto.Turnos
            .Where(t => t.Activo && t.MedicoId == medicoId && t.Fecha == fecha && t.Id != (excluirTurnoId ?? 0))
            .AsEnumerable()
            .Any(t => t.HoraInicio < horaFin && t.HoraFin > horaInicio);
    }
}
