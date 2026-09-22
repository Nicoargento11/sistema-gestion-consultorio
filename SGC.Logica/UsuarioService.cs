using Microsoft.EntityFrameworkCore;
using SGC.Datos;
using SGC.Entidades;

namespace SGC.Logica;

public class UsuarioService
{
    public List<Usuario> ObtenerTodos()
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.Usuarios
            .Where(u => u.Activo)
            .Include(u => u.Medico)
            .Include(u => u.MedicosAsignados)
            .ToList();
    }

    public Usuario? ObtenerPorId(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.Usuarios
            .Include(u => u.Medico)
            .Include(u => u.MedicosAsignados)
            .FirstOrDefault(u => u.Id == id);
    }

    public Usuario? BuscarPorCredenciales(string nombreUsuario, string contrasena)
    {
        using var contexto = SGCContextFactory.Crear();

        // El hash no se puede comparar en SQL (PBKDF2 no es traducible a
        // una expresion de igualdad), asi que primero se busca solo por
        // nombre de usuario y despues se verifica el hash en memoria.
        var usuario = contexto.Usuarios
            .Include(u => u.Medico)
            .Include(u => u.MedicosAsignados)
            .FirstOrDefault(u => u.NombreUsuario == nombreUsuario && u.Activo);

        if (usuario == null || !PasswordHasher.Verificar(contrasena, usuario.Contrasena))
            return null;

        return usuario;
    }

    public void Agregar(Usuario usuario)
    {
        using var contexto = SGCContextFactory.Crear();
        Validar(usuario, contexto, esNuevo: true);

        usuario.Id = 0;
        usuario.Activo = true;
        usuario.Contrasena = PasswordHasher.Hashear(usuario.Contrasena);
        ResolverAsignaciones(usuario, contexto);

        contexto.Usuarios.Add(usuario);
        contexto.SaveChanges();
    }

    public void Modificar(Usuario usuario)
    {
        using var contexto = SGCContextFactory.Crear();
        Validar(usuario, contexto, esNuevo: false);

        var existente = contexto.Usuarios
            .Include(u => u.MedicosAsignados)
            .FirstOrDefault(u => u.Id == usuario.Id)
            ?? throw new InvalidOperationException("El usuario que intenta modificar no existe.");

        existente.NombreUsuario = usuario.NombreUsuario;

        // Contrasena en blanco = "no cambiar" (la UI nunca debe prellenar
        // este campo con el valor guardado, es un hash, no la clave real).
        if (!string.IsNullOrWhiteSpace(usuario.Contrasena))
            existente.Contrasena = PasswordHasher.Hashear(usuario.Contrasena);

        existente.Rol = usuario.Rol;
        existente.MedicoId = usuario.Rol == RolUsuario.Medico ? usuario.MedicoId : null;
        existente.MedicosAsignados = usuario.Rol == RolUsuario.Recepcionista
            ? contexto.Medicos.Where(m => usuario.MedicosAsignadosIds.Contains(m.Id)).ToList()
            : new List<Medico>();

        contexto.SaveChanges();
    }

    public void EliminarLogico(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        var usuario = contexto.Usuarios.FirstOrDefault(u => u.Id == id)
            ?? throw new InvalidOperationException("El usuario que intenta eliminar no existe.");

        usuario.Activo = false;
        contexto.SaveChanges();
    }

    private void ResolverAsignaciones(Usuario usuario, SGCContext contexto)
    {
        if (usuario.Rol != RolUsuario.Medico)
            usuario.MedicoId = null;

        usuario.MedicosAsignados = usuario.Rol == RolUsuario.Recepcionista
            ? contexto.Medicos.Where(m => usuario.MedicosAsignadosIds.Contains(m.Id)).ToList()
            : new List<Medico>();
    }

    private void Validar(Usuario usuario, SGCContext contexto, bool esNuevo)
    {
        if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
            throw new ArgumentException("El nombre de usuario es obligatorio.");

        // Al modificar, contrasena en blanco significa "no cambiarla" (ver
        // Modificar) - solo es obligatoria cuando se esta creando el usuario.
        if (esNuevo && string.IsNullOrWhiteSpace(usuario.Contrasena))
            throw new ArgumentException("La contrasena es obligatoria.");

        // Se valida el largo minimo ANTES de hashear (una vez hasheada,
        // el string siempre mide ~69 caracteres sin importar la clave
        // real, asi que esta validacion no se puede mover a la base).
        if (!string.IsNullOrWhiteSpace(usuario.Contrasena) && usuario.Contrasena.Length < 6)
            throw new ArgumentException("La contrasena debe tener al menos 6 caracteres.");

        if (contexto.Usuarios.Any(u => u.Activo && u.NombreUsuario == usuario.NombreUsuario && u.Id != usuario.Id))
            throw new InvalidOperationException($"Ya existe un usuario activo con el nombre {usuario.NombreUsuario}.");

        if (usuario.Rol == RolUsuario.Medico)
        {
            if (usuario.MedicoId is null)
                throw new ArgumentException("Debe seleccionar a que medico corresponde este usuario.");

            var medico = contexto.Medicos.FirstOrDefault(m => m.Id == usuario.MedicoId.Value);
            if (medico is null || !medico.Activo)
                throw new ArgumentException("El medico seleccionado no es valido.");

            if (contexto.Usuarios.Any(u => u.Activo && u.Id != usuario.Id && u.Rol == RolUsuario.Medico && u.MedicoId == usuario.MedicoId))
                throw new InvalidOperationException("Ese medico ya tiene un usuario asignado.");
        }

        if (usuario.Rol == RolUsuario.Recepcionista && usuario.MedicosAsignadosIds.Count == 0)
            throw new ArgumentException("Debe asignar al menos un medico a la recepcionista.");
    }
}
