using SGC.Entidades;

namespace SGC.Logica;

public class NotificacionService
{
    private static readonly List<string> _avisos = new();

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

        string marca = $"| Medico {medico.NombreCompleto} |";
        return _avisos.Where(a => a.Contains(marca)).ToList();
    }

    public string AvisarTurno(Paciente paciente, string tipoAviso, string detalle)
    {
        if (paciente == null)
            return "No se envio aviso: no hay paciente asociado.";

        if (string.IsNullOrWhiteSpace(paciente.Email) || !paciente.Email.Contains('@'))
            return $"No fue posible enviar el aviso de {tipoAviso}: email de contacto invalido o inexistente.";

        var aviso = $"{DateTime.Now:HH:mm} | {tipoAviso} | {paciente.Email} | {detalle}";
        _avisos.Add(aviso);
        return $"Aviso de {tipoAviso} registrado para {paciente.Email} (simulado en esta etapa).";
    }

    public string AvisarMedico(Medico medico, string tipoAviso, string detalle)
    {
        if (medico == null)
            return "No se envio aviso al profesional.";

        var aviso = $"{DateTime.Now:HH:mm} | {tipoAviso} | Medico {medico.NombreCompleto} | {detalle}";
        _avisos.Add(aviso);
        return $"Aviso interno al profesional {medico.Apellido} registrado (simulado).";
    }
}
