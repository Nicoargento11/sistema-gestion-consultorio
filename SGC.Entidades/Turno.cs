namespace SGC.Entidades;

public enum EstadoTurno
{
    Pendiente,
    Confirmado,
    Cancelado
}

public class Turno
{
    public int Id { get; set; }

    public int PacienteId { get; set; }
    public Paciente? Paciente { get; set; }

    public int MedicoId { get; set; }
    public Medico? Medico { get; set; }

    public int HorarioId { get; set; }
    public Horario? Horario { get; set; }

    public DateOnly Fecha { get; set; }
    public EstadoTurno Estado { get; set; } = EstadoTurno.Pendiente;
    public bool Activo { get; set; } = true;

    public string PacienteNombre => Paciente?.NombreCompleto ?? "";
    public string MedicoNombre => Medico?.NombreCompleto ?? "";
    public string HorarioRango => Horario?.Rango ?? "";

    public ActividadMedica? ActividadMedica { get; set; }
}
