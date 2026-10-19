namespace SGC.Entidades.Reportes;

// Una fila de sp_ReporteTurnosPorPeriodo. Los nombres de propiedad coinciden
// con las columnas del procedimiento. Sin datos clinicos (RNF#07).
public class ReporteTurnoFila
{
    public int TurnoId { get; set; }
    public DateOnly Fecha { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public int DuracionMinutos { get; set; }
    public string Paciente { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public string Medico { get; set; } = string.Empty;
    public string EstadoNombre { get; set; } = string.Empty;
    public decimal? Monto { get; set; }
    public string? MedioPago { get; set; }
}
