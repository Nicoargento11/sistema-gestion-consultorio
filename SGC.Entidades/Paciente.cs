namespace SGC.Entidades;

public class Paciente
{
    public int Id { get; set; }
    public string Dni { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public DateOnly FechaNacimiento { get; set; }
    public bool Activo { get; set; } = true;

    // null = Particular (sin obra social).
    public int? ObraSocialId { get; set; }
    public ObraSocial? ObraSocial { get; set; }

    public string NombreCompleto => $"{Apellido}, {Nombre}";
    public string ObraSocialNombre => ObraSocial?.Nombre ?? "Particular";

    public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
