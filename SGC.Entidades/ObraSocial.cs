namespace SGC.Entidades;

public class ObraSocial
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal PorcentajeCobertura { get; set; }
    public bool Activo { get; set; } = true;

    public string NombreConCobertura => $"{Nombre} ({PorcentajeCobertura}% cobertura)";
}
