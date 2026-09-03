using SGC.Entidades;

namespace SGC.Logica;

public class AutenticacionService
{
    // TODO: reemplazar esta lista fija por una consulta real a SGCContext.Usuarios
    // cuando conectemos la base de datos. El formulario que use este servicio
    // no debería necesitar ningún cambio cuando eso pase.
    private static readonly List<Usuario> UsuariosDePrueba = new()
    {
        new Usuario { Id = 1, NombreUsuario = "admin", Contrasena = "admin123", Rol = RolUsuario.Administrador },
        new Usuario { Id = 2, NombreUsuario = "recepcion", Contrasena = "recepcion123", Rol = RolUsuario.Recepcionista },
        new Usuario { Id = 3, NombreUsuario = "medico", Contrasena = "medico123", Rol = RolUsuario.Medico, MedicoId = 1 }
    };

    public Usuario? Autenticar(string nombreUsuario, string contrasena)
    {
        return UsuariosDePrueba.FirstOrDefault(u =>
            u.NombreUsuario == nombreUsuario &&
            u.Contrasena == contrasena &&
            u.Activo);
    }
}
