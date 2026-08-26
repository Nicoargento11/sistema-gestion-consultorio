namespace SGC.Entidades;

public enum RolUsuario
{
    Administrador,
    Recepcionista,
    Medico
}

public class Usuario
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
    public RolUsuario Rol { get; set; }
    public bool Activo { get; set; } = true;

    public int? MedicoId { get; set; }
    public Medico? Medico { get; set; }
}
