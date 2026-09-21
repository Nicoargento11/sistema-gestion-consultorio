namespace SGC.Entidades;

public enum EstadoTurno
{
    Pendiente,
    Confirmado,
    Cancelado,
    Asistio,
    Ausente
}

public class Turno
{
    public int Id { get; set; }

    public int PacienteId { get; set; }
    public Paciente? Paciente { get; set; }

    public int MedicoId { get; set; }
    public Medico? Medico { get; set; }

    public DateOnly Fecha { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public int DuracionMinutos { get; set; } = 30;

    public TimeOnly HoraFin => HoraInicio.AddMinutes(DuracionMinutos);

    public EstadoTurno Estado { get; set; } = EstadoTurno.Pendiente;
    public string? MedioPago { get; set; }
    public decimal? Monto { get; set; }
    public bool Activo { get; set; } = true;

    public string PacienteNombre => Paciente?.NombreCompleto ?? "";
    public string PacienteDni => Paciente?.Dni ?? "";
    public string MedicoNombre => Medico?.NombreCompleto ?? "";
    public string HorarioRango => $"{HoraInicio:HH:mm} - {HoraFin:HH:mm}";
    public string EstadoAtencion => ActividadMedica != null && ActividadMedica.Activo ? "Atendido" : Estado.ToString();

    public ActividadMedica? ActividadMedica { get; set; }
}

