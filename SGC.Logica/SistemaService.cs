using SGC.Datos;
using SGC.Entidades;

namespace SGC.Logica;

// Consultas de solo lectura sobre la configuracion del sistema, para el
// tablero de inicio del Administrador. No devuelve dinero ni datos clinicos.
public class SistemaService
{
    public SaludSistema ObtenerSalud()
    {
        using var contexto = SGCContextFactory.Crear();

        var porRol = contexto.Usuarios
            .Where(u => u.Activo)
            .GroupBy(u => u.Rol)
            .Select(g => new { Rol = g.Key, Cantidad = g.Count() })
            .ToList();

        int Cantidad(RolUsuario rol) => porRol.FirstOrDefault(x => x.Rol == rol)?.Cantidad ?? 0;

        var medicosActivos = contexto.Medicos.Where(m => m.Activo);

        return new SaludSistema
        {
            AdministradoresActivos = Cantidad(RolUsuario.Administrador),
            RecepcionistasActivos = Cantidad(RolUsuario.Recepcionista),
            MedicosUsuariosActivos = Cantidad(RolUsuario.Medico),
            MedicosSinHorarios = medicosActivos
                .Count(m => !m.Agenda.Any(a => a.Activo)),
            MedicosSinRecepcionista = medicosActivos
                .Count(m => !contexto.Usuarios.Any(u => u.Activo
                                                        && u.Rol == RolUsuario.Recepcionista
                                                        && u.MedicosAsignados.Any(x => x.Id == m.Id))),
            ObrasSocialesActivas = contexto.ObrasSociales.Count(o => o.Activo)
        };
    }
}
