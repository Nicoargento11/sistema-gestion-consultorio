namespace SGC.Entidades.Reportes;

// Una fila de sp_ReporteRecaudacion.
public class ReporteRecaudacionFila
{
    public string Grupo { get; set; } = string.Empty;
    public int CantidadTurnos { get; set; }
    public decimal MontoTotal { get; set; }
}
