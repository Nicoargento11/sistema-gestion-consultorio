namespace SGC.Entidades.Reportes;

// Una fila de sp_ReporteOcupacionAgenda.
public class ReporteOcupacionFila
{
    public string Medico { get; set; } = string.Empty;
    public decimal HorasConfiguradas { get; set; }
    public decimal HorasOcupadas { get; set; }
    public decimal PorcentajeOcupacion { get; set; }
}
