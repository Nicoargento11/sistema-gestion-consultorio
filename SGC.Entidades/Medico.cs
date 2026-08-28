namespace SGC.Entidades;

public class Medico
{
    public int Id { get; set; }
    public string Dni { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Matricula { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public string NombreCompleto => $"{Apellido}, {Nombre} ({Especialidad})";

    public ICollection<AgendaMedico> Agenda { get; set; } = new List<AgendaMedico>();
    public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
