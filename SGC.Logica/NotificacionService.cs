using SGC.Entidades;

namespace SGC.Logica;

public class NotificacionService
{
    private static readonly List<string> _avisos = new();

    // Vacia los avisos en memoria: se llama al cerrar sesion o iniciar una nueva,
    // para que el usuario siguiente no vea los avisos (con emails de pacientes) del anterior.
    public static void LimpiarAvisos()
    {
        _avisos.Clear();
    }

    public List<string> ObtenerAvisos()
    {
        return _avisos.ToList();
    }

    // Avisos internos dirigidos a un profesional (los registrados con
    // AvisarMedico). Los avisos a pacientes no se incluyen: llevan el email
    // del paciente y no corresponden a la vista del medico.
    public List<string> ObtenerAvisosDeMedico(Medico medico)
    {
        if (medico == null)
            return new List<string>();

        string marca = $"| Médico {medico.NombreCompleto} |";
        return _avisos.Where(a => a.Contains(marca)).ToList();
    }

    public string AvisarTurno(Paciente paciente, string tipoAviso, string detalle)
    {
        if (paciente == null)
            return "No se envió el aviso: no hay paciente asociado.";

        if (string.IsNullOrWhiteSpace(paciente.Email) || !paciente.Email.Contains('@'))
            return $"No fue posible enviar el aviso de {tipoAviso}: el email de contacto es inválido o no existe.";

        var aviso = $"{DateTime.Now:HH:mm} | {tipoAviso} | {paciente.Email} | {detalle}";
        _avisos.Add(aviso);
        return $"Aviso de {tipoAviso} registrado para {paciente.Email}. El envío real de correos aún no está habilitado: queda solo en el panel de avisos.";
    }

    public string AvisarMedico(Medico medico, string tipoAviso, string detalle)
    {
        if (medico == null)
            return "No se envió el aviso al profesional.";

        var aviso = $"{DateTime.Now:HH:mm} | {tipoAviso} | Médico {medico.NombreCompleto} | {detalle}";
        _avisos.Add(aviso);
        return $"Aviso interno al profesional {medico.Apellido} registrado en el panel de avisos.";
    }
}
