namespace SGC.Entidades;

public class ActividadMedica
{
    public int Id { get; set; }

    public int TurnoId { get; set; }
    public Turno? Turno { get; set; }

    public int TipoActividadId { get; set; }
    public TipoActividad? TipoActividad { get; set; }

    public string MotivoConsulta { get; set; } = string.Empty;
    public string RecetaMedicamentos { get; set; } = string.Empty;
    public string Procedimiento { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public string TipoActividadNombre => TipoActividad?.NombreTipo ?? "";
    public string FechaStr => Turno != null ? Turno.Fecha.ToString("dd/MM/yyyy") : "";
    public string HorarioStr => Turno?.HorarioRango ?? "";
    public string PacienteNombre => Turno?.PacienteNombre ?? "";
    public string MedicoNombre => Turno?.MedicoNombre ?? "";
}

