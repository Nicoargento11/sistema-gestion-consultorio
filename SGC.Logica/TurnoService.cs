using SGC.Entidades;

namespace SGC.Logica;

public class TurnoService
{
    // Datos iniciales de prueba para desarrollo y demostracion
    private static readonly List<Turno> _turnos = new()
    {
        new Turno
        {
            Id = 1,
            PacienteId = 1,
            Paciente = new Paciente { Id = 1, Nombre = "Carlos", Apellido = "Fernandez", Dni = "35123456", Email = "carlos.f@email.com", Telefono = "3794123456", Activo = true },
            MedicoId = 1,
            Medico = new Medico { Id = 1, Dni = "20111222", Nombre = "Laura", Apellido = "Gomez", Matricula = "MP1234", Especialidad = "Clinica General", Activo = true },
            HorarioId = 1,
            Horario = new Horario { Id = 1, HoraInicio = new TimeOnly(8, 0), HoraFin = new TimeOnly(8, 30), Activo = true },
            Fecha = DateOnly.FromDateTime(DateTime.Today),
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
            HorarioId = 2,
            Horario = new Horario { Id = 2, HoraInicio = new TimeOnly(8, 30), HoraFin = new TimeOnly(9, 0), Activo = true },
            Fecha = DateOnly.FromDateTime(DateTime.Today),
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
            HorarioId = 3,
            Horario = new Horario { Id = 3, HoraInicio = new TimeOnly(9, 0), HoraFin = new TimeOnly(9, 30), Activo = true },
            Fecha = DateOnly.FromDateTime(DateTime.Today),
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
            HorarioId = 4,
            Horario = new Horario { Id = 4, HoraInicio = new TimeOnly(9, 30), HoraFin = new TimeOnly(10, 0), Activo = true },
            Fecha = DateOnly.FromDateTime(DateTime.Today),
            Estado = EstadoTurno.Confirmado,
            Activo = true
        }
    };
    private static int _siguienteId = 5;

    public List<Turno> ObtenerTodos(bool incluirCancelados = false, int? medicoId = null, DateOnly? fecha = null)
    {
        // Por default, mismo criterio que PacienteService: al cancelar (baja logica),
        // desaparece de la vista normal. RF#06/RF#09 piden filtros de consulta
        // (por medico o por fecha), asi que se puede acotar por cualquiera de los dos.
        IEnumerable<Turno> query = incluirCancelados ? _turnos : _turnos.Where(t => t.Activo);

        if (medicoId != null)
            query = query.Where(t => t.MedicoId == medicoId);

        if (fecha != null)
            query = query.Where(t => t.Fecha == fecha);

        return query.OrderBy(t => t.Fecha).ToList();
    }

    public List<Turno> ObtenerPorMedicoYFecha(int medicoId, DateOnly? fecha = null, bool incluirCancelados = false)
    {
        IEnumerable<Turno> query = incluirCancelados ? _turnos : _turnos.Where(t => t.Activo);
        query = query.Where(t => t.MedicoId == medicoId);

        if (fecha.HasValue)
            query = query.Where(t => t.Fecha == fecha.Value);

        return query.OrderBy(t => t.Fecha).ThenBy(t => t.Horario != null ? t.Horario.HoraInicio : TimeOnly.MinValue).ToList();
    }

    public Turno? ObtenerPorId(int id)
    {
        return _turnos.FirstOrDefault(t => t.Id == id);
    }


    public void AsignarTurno(Paciente paciente, Medico medico, Horario horario, DateOnly fecha)
    {
        if (fecha < DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("No se puede asignar un turno en una fecha pasada.");

        // Esta es LA regla de negocio central del sistema (RF#04 del ERS):
        // no puede haber dos turnos activos para el mismo medico, mismo horario y misma fecha.
        bool sobreturno = _turnos.Any(t =>
            t.Activo &&
            t.MedicoId == medico.Id &&
            t.HorarioId == horario.Id &&
            t.Fecha == fecha);

        if (sobreturno)
            throw new InvalidOperationException(
                $"El Dr./Dra. {medico.Apellido} ya tiene un turno asignado el {fecha:dd/MM/yyyy} en el horario {horario.HoraInicio:HH:mm} - {horario.HoraFin:HH:mm}.");

        var turno = new Turno
        {
            Id = _siguienteId++,
            PacienteId = paciente.Id,
            Paciente = paciente,
            MedicoId = medico.Id,
            Medico = medico,
            HorarioId = horario.Id,
            Horario = horario,
            Fecha = fecha,
            Estado = EstadoTurno.Confirmado,
            Activo = true
        };

        _turnos.Add(turno);
    }

    public void ModificarTurno(int turnoId, Horario nuevoHorario, DateOnly nuevaFecha)
    {
        var turno = _turnos.FirstOrDefault(t => t.Id == turnoId)
            ?? throw new InvalidOperationException("El turno que intenta modificar no existe.");

        if (turno.Estado == EstadoTurno.Cancelado)
            throw new InvalidOperationException("No se puede modificar un turno cancelado.");

        if (nuevaFecha < DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("No se puede modificar un turno a una fecha pasada.");

        // Misma validacion de sobreturno que en AsignarTurno, pero excluyendo
        // al propio turno (si no, siempre "chocaria" contra si mismo).
        bool sobreturno = _turnos.Any(t =>
            t.Activo &&
            t.Id != turno.Id &&
            t.MedicoId == turno.MedicoId &&
            t.HorarioId == nuevoHorario.Id &&
            t.Fecha == nuevaFecha);

        if (sobreturno)
            throw new InvalidOperationException(
                $"El Dr./Dra. {turno.Medico?.Apellido} ya tiene otro turno asignado el {nuevaFecha:dd/MM/yyyy} en el horario {nuevoHorario.HoraInicio:HH:mm} - {nuevoHorario.HoraFin:HH:mm}.");

        turno.HorarioId = nuevoHorario.Id;
        turno.Horario = nuevoHorario;
        turno.Fecha = nuevaFecha;
    }

    public bool HorarioOcupado(int medicoId, int horarioId, DateOnly fecha)
    {
        return _turnos.Any(t =>
            t.Activo &&
            t.MedicoId == medicoId &&
            t.HorarioId == horarioId &&
            t.Fecha == fecha);
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

        // Baja logica: no se borra, se marca cancelado. Libera el horario
        // para que AsignarTurno vuelva a permitirlo en esa fecha.
        turno.Estado = EstadoTurno.Cancelado;
        turno.Activo = false;
    }
}
