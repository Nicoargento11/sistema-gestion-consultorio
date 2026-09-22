using System.ComponentModel.DataAnnotations.Schema;

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

    // Para Rol = Medico: a que profesional corresponde este login.
    public int? MedicoId { get; set; }
    public Medico? Medico { get; set; }

    // Para Rol = Recepcionista: a que medicos tiene acceso (una recepcionista
    // puede atender a varios medicos, y un medico puede ser atendido por
    // varias recepcionistas en distintos turnos de trabajo).
    // MedicosAsignadosIds es solo pegamento con la UI (CheckedListBox);
    // la relacion real es MedicosAsignados, mapeada M:N por EF Core.
    [NotMapped]
    public List<int> MedicosAsignadosIds { get; set; } = new();
    public List<Medico> MedicosAsignados { get; set; } = new();

    public string MedicoAsignadoNombre => Medico?.NombreCompleto ?? "";
    public string MedicosAsignadosTexto => string.Join(", ", MedicosAsignados.Select(m => m.NombreCompleto));
}
