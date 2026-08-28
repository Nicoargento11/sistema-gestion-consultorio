using SGC.Entidades;

namespace SGC.Logica;

public class TurnoService
{
    // TODO: reemplazar por SGCContext.Turnos cuando conectemos la base de datos real.
    private static readonly List<Turno> _turnos = new();
    private static int _siguienteId = 1;

    public List<Turno> ObtenerTodos(bool incluirCancelados = false, int? medicoId = null)
    {
        // Por default, mismo criterio que PacienteService: al cancelar (baja logica),
        // desaparece de la vista normal. RF#06/RF#09 piden filtros de consulta
        // (por medico, entre otros), asi que se puede acotar por medicoId.
        IEnumerable<Turno> query = incluirCancelados ? _turnos : _turnos.Where(t => t.Activo);

        if (medicoId != null)
            query = query.Where(t => t.MedicoId == medicoId);

        return query.OrderBy(t => t.Fecha).ToList();
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
