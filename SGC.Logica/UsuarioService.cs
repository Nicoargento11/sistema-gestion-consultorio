using SGC.Entidades;

namespace SGC.Logica;

public class UsuarioService
{
    // TODO (companero): reemplazar por ABM real cuando este listo SGC.Datos.
    private static readonly List<Usuario> _usuarios = new()
    {
        new Usuario { Id = 1, NombreUsuario = "admin", Contrasena = "admin123", Rol = RolUsuario.Administrador },
        new Usuario { Id = 2, NombreUsuario = "recepcion", Contrasena = "recepcion123", Rol = RolUsuario.Recepcionista },
        new Usuario { Id = 3, NombreUsuario = "medico", Contrasena = "medico123", Rol = RolUsuario.Medico, MedicoId = 1 }
    };
    private static int _siguienteId = 4;

    private readonly MedicoService _medicoService = new();

    public List<Usuario> ObtenerTodos()
    {
        return _usuarios.Where(u => u.Activo).Select(ResolverNavegacion).ToList();
    }

    public Usuario? ObtenerPorId(int id)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
        return usuario is null ? null : ResolverNavegacion(usuario);
    }

    public Usuario? BuscarPorCredenciales(string nombreUsuario, string contrasena)
    {
        var usuario = _usuarios.FirstOrDefault(u =>
            u.NombreUsuario == nombreUsuario &&
            u.Contrasena == contrasena &&
            u.Activo);
        return usuario is null ? null : ResolverNavegacion(usuario);
    }

    public void Agregar(Usuario usuario)
    {
        Validar(usuario);

        if (_usuarios.Any(u => u.Activo && u.NombreUsuario == usuario.NombreUsuario))
            throw new InvalidOperationException($"Ya existe un usuario activo con el nombre {usuario.NombreUsuario}.");

        usuario.Id = _siguienteId++;
        usuario.Activo = true;
        _usuarios.Add(usuario);
    }

    public void Modificar(Usuario usuario)
    {
        Validar(usuario);

        var existente = _usuarios.FirstOrDefault(u => u.Id == usuario.Id)
            ?? throw new InvalidOperationException("El usuario que intenta modificar no existe.");

        if (_usuarios.Any(u => u.Activo && u.NombreUsuario == usuario.NombreUsuario && u.Id != usuario.Id))
            throw new InvalidOperationException($"Ya existe otro usuario activo con el nombre {usuario.NombreUsuario}.");

        existente.NombreUsuario = usuario.NombreUsuario;
        existente.Contrasena = usuario.Contrasena;
        existente.Rol = usuario.Rol;
        existente.MedicoId = usuario.MedicoId;
        existente.MedicosAsignadosIds = usuario.MedicosAsignadosIds;
    }

    public void EliminarLogico(int id)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Id == id)
            ?? throw new InvalidOperationException("El usuario que intenta eliminar no existe.");

        usuario.Activo = false;
    }

    private Usuario ResolverNavegacion(Usuario usuario)
    {
        usuario.Medico = usuario.MedicoId is int medicoId ? _medicoService.ObtenerPorId(medicoId) : null;
        usuario.MedicosAsignados = usuario.MedicosAsignadosIds
            .Select(id => _medicoService.ObtenerPorId(id))
            .Where(m => m is not null)
            .Cast<Medico>()
            .ToList();
        return usuario;
    }

    private void Validar(Usuario usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
            throw new ArgumentException("El nombre de usuario es obligatorio.");

        if (string.IsNullOrWhiteSpace(usuario.Contrasena))
            throw new ArgumentException("La contrasena es obligatoria.");

        if (usuario.Rol == RolUsuario.Medico)
        {
            if (usuario.MedicoId is null)
                throw new ArgumentException("Debe seleccionar a que medico corresponde este usuario.");

            var medico = _medicoService.ObtenerPorId(usuario.MedicoId.Value);
            if (medico is null || !medico.Activo)
                throw new ArgumentException("El medico seleccionado no es valido.");

            if (_usuarios.Any(u => u.Activo && u.Id != usuario.Id && u.Rol == RolUsuario.Medico && u.MedicoId == usuario.MedicoId))
                throw new InvalidOperationException("Ese medico ya tiene un usuario asignado.");
        }

        if (usuario.Rol == RolUsuario.Recepcionista && usuario.MedicosAsignadosIds.Count == 0)
            throw new ArgumentException("Debe asignar al menos un medico a la recepcionista.");
    }
}
