using SGC.Entidades;

namespace SGC.Logica;

public class AutenticacionService
{
    private readonly UsuarioService _usuarioService = new();

    public Usuario? Autenticar(string nombreUsuario, string contrasena)
    {
        return _usuarioService.BuscarPorCredenciales(nombreUsuario, contrasena);
    }
}
