namespace SGC.Logica;

// Indicadores de estado del sistema para el Inicio del Administrador. Son solo
// contadores de configuracion: ningun dato de dinero ni clinico (RNF#07).
public class SaludSistema
{
    public int AdministradoresActivos { get; set; }
    public int RecepcionistasActivos { get; set; }
    public int MedicosUsuariosActivos { get; set; }

    public int UsuariosActivos => AdministradoresActivos + RecepcionistasActivos + MedicosUsuariosActivos;

    // Medicos activos sin ningun horario de atencion activo (AgendaMedico).
    public int MedicosSinHorarios { get; set; }

    // Medicos activos que no tiene asignado ningun usuario Recepcionista activo.
    public int MedicosSinRecepcionista { get; set; }

    public int ObrasSocialesActivas { get; set; }
}
