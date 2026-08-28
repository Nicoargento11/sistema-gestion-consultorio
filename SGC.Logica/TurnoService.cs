using SGC.Entidades;

namespace SGC.Logica;

public class TurnoService
{
    // TODO: reemplazar por SGCContext.Turnos cuando conectemos la base de datos real.
    private static readonly List<Turno> _turnos = new();
    private static int _siguienteId = 1;

    public List<Turno> ObtenerTodos()
    {
        // Mismo criterio que PacienteService: al cancelar (baja logica), desaparece
        // de la vista normal. Consistente con como se comporta "Eliminar" en Pacientes.
        return _turnos.Where(t => t.Activo).OrderBy(t => t.Fecha).ToList();
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
