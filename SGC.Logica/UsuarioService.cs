using Microsoft.EntityFrameworkCore;
using SGC.Datos;
using SGC.Entidades;

namespace SGC.Logica;

public class UsuarioService
{
    public List<Usuario> ObtenerTodos(bool incluirInactivos = false)
    {
        using var contexto = SGCContextFactory.Crear();
        IQueryable<Usuario> query = contexto.Usuarios
            .Include(u => u.Medico)
            .Include(u => u.MedicosAsignados.Where(m => m.Activo));

        var usuarios = (incluirInactivos ? query : query.Where(u => u.Activo)).ToList();
        usuarios.ForEach(SincronizarIds);
        return usuarios;
    }

    public Usuario? ObtenerPorId(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        var usuario = contexto.Usuarios
            .Include(u => u.Medico)
            .Include(u => u.MedicosAsignados.Where(m => m.Activo))
            .FirstOrDefault(u => u.Id == id);

        if (usuario != null) SincronizarIds(usuario);
        return usuario;
    }

    public Usuario? BuscarPorCredenciales(string nombreUsuario, string contrasena)
    {
        using var contexto = SGCContextFactory.Crear();

        // El hash no se puede comparar en SQL (PBKDF2 no es traducible a
        // una expresion de igualdad), asi que primero se busca solo por
        // nombre de usuario y despues se verifica el hash en memoria.
        var usuario = contexto.Usuarios
            .Include(u => u.Medico)
            .Include(u => u.MedicosAsignados.Where(m => m.Activo))
            .FirstOrDefault(u => u.NombreUsuario == nombreUsuario && u.Activo);

        if (usuario == null || !PasswordHasher.Verificar(contrasena, usuario.Contrasena))
            return null;

        SincronizarIds(usuario);
        return usuario;
    }

    // Relee de la base los medicos ACTIVOS asignados a una recepcionista y los deja
    // en el mismo objeto Usuario de la sesion (que se cargo al iniciar sesion y puede
    // haber quedado viejo si el administrador cambio asignaciones o dio de baja medicos).
    // Si la base no responde conserva la lista que ya tenia.
    public void ActualizarMedicosAsignados(Usuario usuario)
    {
        if (usuario == null || usuario.Rol != RolUsuario.Recepcionista)
            return;

        try
        {
            using var contexto = SGCContextFactory.Crear();
            var medicos = contexto.Usuarios
                .Where(u => u.Id == usuario.Id)
                .SelectMany(u => u.MedicosAsignados)
                .Where(m => m.Activo)
                .OrderBy(m => m.Apellido).ThenBy(m => m.Nombre)
                .ToList();

            usuario.MedicosAsignados = medicos;
            SincronizarIds(usuario);
        }
        catch (Exception)
        {
        }
    }

    // MedicosAsignadosIds es [NotMapped] (pegamento con la UI, el
    // CheckedListBox) - mismo problema que Medico.ObrasSocialesAceptadasIds:
    // hay que sincronizarlo a mano desde la navegacion real despues de leer,
    // si no el checklist de FormUsuarios siempre aparece vacio al editar.
    private static void SincronizarIds(Usuario usuario)
    {
        usuario.MedicosAsignadosIds = usuario.MedicosAsignados.Select(m => m.Id).ToList();
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
        Persistencia.Guardar(contexto);
    }

    public void Modificar(Usuario usuario)
    {
        using var contexto = SGCContextFactory.Crear();
        Validar(usuario, contexto, esNuevo: false);

        var existente = contexto.Usuarios
            .Include(u => u.MedicosAsignados.Where(m => m.Activo))
            .FirstOrDefault(u => u.Id == usuario.Id)
            ?? throw new InvalidOperationException("El usuario que intenta modificar no existe.");

        // Un administrador activo no puede dejar de serlo si es el ultimo.
        if (existente.Activo && existente.Rol == RolUsuario.Administrador && usuario.Rol != RolUsuario.Administrador)
            ExigirOtroAdministradorActivo(contexto, existente.Id, "cambiarle el rol a");

        existente.NombreUsuario = usuario.NombreUsuario;

        // Contrasena en blanco = "no cambiar" (la UI nunca debe prellenar
        // este campo con el valor guardado, es un hash, no la clave real).
        if (!string.IsNullOrWhiteSpace(usuario.Contrasena))
            existente.Contrasena = PasswordHasher.Hashear(usuario.Contrasena);

        existente.Rol = usuario.Rol;
        existente.MedicoId = usuario.Rol == RolUsuario.Medico ? usuario.MedicoId : null;
        existente.MedicosAsignados = usuario.Rol == RolUsuario.Recepcionista
            ? contexto.Medicos.Where(m => m.Activo && usuario.MedicosAsignadosIds.Contains(m.Id)).ToList()
            : new List<Medico>();

        Persistencia.Guardar(contexto);
    }

    public void EliminarLogico(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        var usuario = contexto.Usuarios.FirstOrDefault(u => u.Id == id)
            ?? throw new InvalidOperationException("El usuario que intenta eliminar no existe.");

        if (!usuario.Activo)
            throw new InvalidOperationException("Ese usuario ya estaba eliminado.");

        if (usuario.Rol == RolUsuario.Administrador)
            ExigirOtroAdministradorActivo(contexto, usuario.Id, "eliminar a");

        usuario.Activo = false;
        contexto.SaveChanges();
    }

    public void Reactivar(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        var usuario = contexto.Usuarios.FirstOrDefault(u => u.Id == id)
            ?? throw new InvalidOperationException("El usuario que intenta reactivar no existe.");

        if (usuario.Activo)
            throw new InvalidOperationException("Ese usuario ya estaba activo.");

        // Puede haber pasado tiempo desde que se elimino: si alguien creo
        // mientras tanto otro usuario activo con el mismo nombre, o un
        // usuario de Rol=Medico para el mismo medico, reactivar este
        // generaria un duplicado - mismas reglas que Agregar/Modificar.
        if (contexto.Usuarios.Any(u => u.Activo && u.NombreUsuario == usuario.NombreUsuario))
            throw new InvalidOperationException($"Ya existe un usuario activo con el nombre {usuario.NombreUsuario}. Cambie el nombre antes de reactivarlo.");

        if (usuario.Rol == RolUsuario.Medico && contexto.Usuarios.Any(u => u.Activo && u.Rol == RolUsuario.Medico && u.MedicoId == usuario.MedicoId))
            throw new InvalidOperationException("Ese médico ya tiene otro usuario activo asignado.");

        usuario.Activo = true;
        Persistencia.Guardar(contexto);
    }

    // Regla: siempre debe quedar al menos un Administrador activo, si no
    // nadie podria entrar a Usuarios, Medicos ni Copia de seguridad.
    private static void ExigirOtroAdministradorActivo(SGCContext contexto, int usuarioId, string accion)
    {
        bool hayOtro = contexto.Usuarios.Any(u => u.Activo && u.Rol == RolUsuario.Administrador && u.Id != usuarioId);
        if (!hayOtro)
            throw new InvalidOperationException($"No se puede {accion} el único administrador activo. Cree o reactive otro administrador primero.");
    }

    private void ResolverAsignaciones(Usuario usuario, SGCContext contexto)
    {
        if (usuario.Rol != RolUsuario.Medico)
            usuario.MedicoId = null;

        usuario.MedicosAsignados = usuario.Rol == RolUsuario.Recepcionista
            ? contexto.Medicos.Where(m => m.Activo && usuario.MedicosAsignadosIds.Contains(m.Id)).ToList()
            : new List<Medico>();
    }

    private void Validar(Usuario usuario, SGCContext contexto, bool esNuevo)
    {
        if (string.IsNullOrWhiteSpace(usuario.NombreUsuario) || usuario.NombreUsuario.Length < 3 ||
            !usuario.NombreUsuario.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '.'))
            throw new ArgumentException("El nombre de usuario debe tener al menos 3 caracteres: solo letras, números, '_' o '.', sin espacios.");

        // Al modificar, contrasena en blanco significa "no cambiarla" (ver
        // Modificar) - solo es obligatoria cuando se esta creando el usuario.
        if (esNuevo && string.IsNullOrWhiteSpace(usuario.Contrasena))
            throw new ArgumentException("La contraseña es obligatoria.");

        // Se valida el largo minimo ANTES de hashear (una vez hasheada,
        // el string siempre mide ~69 caracteres sin importar la clave
        // real, asi que esta validacion no se puede mover a la base).
        if (!string.IsNullOrWhiteSpace(usuario.Contrasena) && usuario.Contrasena.Length < 6)
            throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");

        // El indice unico de la base incluye a los usuarios dados de baja.
        var mismoNombre = contexto.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuario.NombreUsuario && u.Id != usuario.Id);
        if (mismoNombre != null)
            throw new InvalidOperationException(mismoNombre.Activo
                ? $"Ya existe un usuario con el nombre {usuario.NombreUsuario}."
                : $"Ya existe un usuario con el nombre {usuario.NombreUsuario} (está dado de baja). Elija otro nombre o reactive ese usuario.");

        if (usuario.Rol == RolUsuario.Medico)
        {
            if (usuario.MedicoId is null)
                throw new ArgumentException("Debe seleccionar a qué médico corresponde este usuario.");

            var medico = contexto.Medicos.FirstOrDefault(m => m.Id == usuario.MedicoId.Value);
            if (medico is null || !medico.Activo)
                throw new ArgumentException("El médico seleccionado no es válido.");

            // El indice unico de la base (UQ_Usuario_MedicoUnico) no filtra por Activo.
            var usuarioDelMedico = contexto.Usuarios.FirstOrDefault(u => u.Id != usuario.Id && u.Rol == RolUsuario.Medico && u.MedicoId == usuario.MedicoId);
            if (usuarioDelMedico != null)
                throw new InvalidOperationException(usuarioDelMedico.Activo
                    ? "Ese médico ya tiene un usuario asignado."
                    : "Ese médico ya tiene un usuario (está dado de baja). Reactive ese usuario en lugar de crear uno nuevo.");
        }

        if (usuario.Rol == RolUsuario.Recepcionista && usuario.MedicosAsignadosIds.Count == 0)
            throw new ArgumentException("Debe asignar al menos un médico a la recepcionista.");
    }
}
