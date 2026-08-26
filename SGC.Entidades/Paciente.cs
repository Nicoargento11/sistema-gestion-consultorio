namespace SGC.Entidades;

public class Paciente
{
    public int Id { get; set; }
    public string Dni { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
