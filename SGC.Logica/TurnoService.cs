using SGC.Entidades;

namespace SGC.Logica;

public class TurnoService
{
    private readonly AgendaService _agendaService = new();

    private static readonly List<Turno> _turnos = new()
    {
        new Turno
        {
            Id = 1,
            PacienteId = 1,
            Paciente = new Paciente { Id = 1, Nombre = "Carlos", Apellido = "Fernandez", Dni = "35123456", Email = "carlos.f@email.com", Telefono = "3794123456", Activo = true },
            MedicoId = 1,
            Medico = new Medico { Id = 1, Dni = "20111222", Nombre = "Laura", Apellido = "Gomez", Matricula = "MP1234", Especialidad = "Clinica General", Activo = true },
            Fecha = DateOnly.FromDateTime(DateTime.Today),
            HoraInicio = new TimeOnly(9, 0),
            DuracionMinutos = 30,
            Estado = EstadoTurno.Confirmado,
            Activo = true
        },
        new Turno
        {
            Id = 2,
            PacienteId = 2,
            Paciente = new Paciente { Id = 2, Nombre = "Ana", Apellido = "Martinez", Dni = "38987654", Email = "ana.martinez@email.com", Telefono = "3794987654", Activo = true },
            MedicoId = 1,
            Medico = new Medico { Id = 1, Dni = "20111222", Nombre = "Laura", Apellido = "Gomez", Matricula = "MP1234", Especialidad = "Clinica General", Activo = true },
            Fecha = DateOnly.FromDateTime(DateTime.Today),
            HoraInicio = new TimeOnly(9, 30),
            DuracionMinutos = 30,
            Estado = EstadoTurno.Confirmado,
            Activo = true
        },
        new Turno
        {
            Id = 3,
            PacienteId = 3,
            Paciente = new Paciente { Id = 3, Nombre = "Luis", Apellido = "Torres", Dni = "40555666", Email = "luis.torres@email.com", Telefono = "3794555666", Activo = true },
            MedicoId = 1,
            Medico = new Medico { Id = 1, Dni = "20111222", Nombre = "Laura", Apellido = "Gomez", Matricula = "MP1234", Especialidad = "Clinica General", Activo = true },
            Fecha = DateOnly.FromDateTime(DateTime.Today),
            HoraInicio = new TimeOnly(10, 0),
            DuracionMinutos = 30,
            Estado = EstadoTurno.Confirmado,
            Activo = true
        },
        new Turno
        {
            Id = 4,
            PacienteId = 4,
            Paciente = new Paciente { Id = 4, Nombre = "Sofia", Apellido = "Herrera", Dni = "42111222", Email = "sofia.herrera@email.com", Telefono = "3794111222", Activo = true },
            MedicoId = 1,
            Medico = new Medico { Id = 1, Dni = "20111222", Nombre = "Laura", Apellido = "Gomez", Matricula = "MP1234", Especialidad = "Clinica General", Activo = true },
            Fecha = DateOnly.FromDateTime(DateTime.Today),
            HoraInicio = new TimeOnly(10, 30),
            DuracionMinutos = 30,
            Estado = EstadoTurno.Confirmado,
            Activo = true
        }
    };
    private static int _siguienteId = 5;

    public List<Turno> ObtenerTodos(bool incluirCancelados = false, int? medicoId = null, DateOnly? fecha = null, int? pacienteId = null)
    {
        IEnumerable<Turno> query = incluirCancelados ? _turnos : _turnos.Where(t => t.Activo);

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
        IEnumerable<Turno> query = incluirCancelados ? _turnos : _turnos.Where(t => t.Activo);
        query = query.Where(t => t.MedicoId == medicoId);

        if (fecha.HasValue)
            query = query.Where(t => t.Fecha == fecha.Value);

        return query.OrderBy(t => t.Fecha).ThenBy(t => t.HoraInicio).ToList();
    }

    public Turno? ObtenerPorId(int id)
    {
        return _turnos.FirstOrDefault(t => t.Id == id);
    }

    public void AsignarTurno(Paciente paciente, Medico medico, DateOnly fecha, TimeOnly horaInicio, int duracionMinutos = 30)
    {
        if (duracionMinutos <= 0) duracionMinutos = 30;
        TimeOnly horaFin = horaInicio.AddMinutes(duracionMinutos);

        if (fecha < DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("No se puede asignar un turno en una fecha pasada.");

        if (!_agendaService.MedicoAtiende(medico.Id, fecha.DayOfWeek, horaInicio, duracionMinutos))
            throw new InvalidOperationException(
                $"El profesional no atiende el {AgendaMedico.NombreDia(fecha.DayOfWeek)} en el horario {horaInicio:HH:mm} - {horaFin:HH:mm}.");

        if (HaySuperposicion(medico.Id, fecha, horaInicio, duracionMinutos))
            throw new InvalidOperationException(
                $"El Dr./Dra. {medico.Apellido} ya tiene un turno asignado el {fecha:dd/MM/yyyy} que se superpone con {horaInicio:HH:mm} - {horaFin:HH:mm}.");

        var turno = new Turno
        {
            Id = _siguienteId++,
            PacienteId = paciente.Id,
            Paciente = paciente,
            MedicoId = medico.Id,
            Medico = medico,
            Fecha = fecha,
            HoraInicio = horaInicio,
            DuracionMinutos = duracionMinutos,
            Estado = EstadoTurno.Confirmado,
            Activo = true
        };

        _turnos.Add(turno);
    }

    public void ModificarTurno(int turnoId, DateOnly nuevaFecha, TimeOnly nuevaHoraInicio, int nuevaDuracionMinutos = 30)
    {
        var turno = _turnos.FirstOrDefault(t => t.Id == turnoId)
            ?? throw new InvalidOperationException("El turno que intenta modificar no existe.");

        if (turno.Estado == EstadoTurno.Cancelado)
            throw new InvalidOperationException("No se puede modificar un turno cancelado.");

        if (nuevaDuracionMinutos <= 0) nuevaDuracionMinutos = 30;
        TimeOnly nuevaHoraFin = nuevaHoraInicio.AddMinutes(nuevaDuracionMinutos);

        if (nuevaFecha < DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("No se puede modificar un turno a una fecha pasada.");

        if (!_agendaService.MedicoAtiende(turno.MedicoId, nuevaFecha.DayOfWeek, nuevaHoraInicio, nuevaDuracionMinutos))
            throw new InvalidOperationException(
                $"El profesional no atiende el {AgendaMedico.NombreDia(nuevaFecha.DayOfWeek)} en el horario {nuevaHoraInicio:HH:mm} - {nuevaHoraFin:HH:mm}.");

        if (HaySuperposicion(turno.MedicoId, nuevaFecha, nuevaHoraInicio, nuevaDuracionMinutos, turnoId))
            throw new InvalidOperationException(
                $"El Dr./Dra. {turno.Medico?.Apellido} ya tiene otro turno asignado el {nuevaFecha:dd/MM/yyyy} que se superpone con {nuevaHoraInicio:HH:mm} - {nuevaHoraFin:HH:mm}.");

        turno.Fecha = nuevaFecha;
        turno.HoraInicio = nuevaHoraInicio;
        turno.DuracionMinutos = nuevaDuracionMinutos;
    }

    public bool HorarioOcupado(int medicoId, DateOnly fecha, TimeOnly horaInicio, int duracionMinutos = 30)
    {
        return HaySuperposicion(medicoId, fecha, horaInicio, duracionMinutos);
    }

    public void ConfirmarAsistencia(int turnoId, bool asistio, string? medioPago, decimal? monto)
    {
        var turno = _turnos.FirstOrDefault(t => t.Id == turnoId)
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
    }

    public void CancelarTurno(int id)
    {
        var turno = _turnos.FirstOrDefault(t => t.Id == id)
            ?? throw new InvalidOperationException("El turno que intenta cancelar no existe.");

        if (turno.Estado == EstadoTurno.Cancelado)
            throw new InvalidOperationException("Ese turno ya estaba cancelado.");

        turno.Estado = EstadoTurno.Cancelado;
        turno.Activo = false;
    }

    private bool HaySuperposicion(int medicoId, DateOnly fecha, TimeOnly horaInicio, int duracionMinutos, int? excluirTurnoId = null)
    {
        TimeOnly horaFin = horaInicio.AddMinutes(duracionMinutos);
        return _turnos.Any(t =>
            t.Activo &&
            t.MedicoId == medicoId &&
            t.Fecha == fecha &&
            t.Id != (excluirTurnoId ?? 0) &&
            t.HoraInicio < horaFin &&
            t.HoraFin > horaInicio);
    }
}
