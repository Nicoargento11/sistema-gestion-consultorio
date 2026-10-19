namespace SGC.Entidades.Reportes;

// Una fila de sp_ReporteAsistencia.
public class ReporteAsistenciaFila
{
    public string Medico { get; set; } = string.Empty;
    public int TotalTurnos { get; set; }
    public int Atendidos { get; set; }
    public int Ausentes { get; set; }
    public int Cancelados { get; set; }
    public decimal PorcentajeAsistencia { get; set; }
}
