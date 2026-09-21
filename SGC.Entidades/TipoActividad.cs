namespace SGC.Entidades;

public class TipoActividad
{
    public int Id { get; set; }
    public string NombreTipo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int DuracionSugeridaMinutos { get; set; } = 30;
    public bool Activo { get; set; } = true;
}
