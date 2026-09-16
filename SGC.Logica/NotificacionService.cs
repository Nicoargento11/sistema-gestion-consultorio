using SGC.Entidades;

namespace SGC.Logica;

public class NotificacionService
{
    private static readonly List<string> _avisos = new();

    public List<string> ObtenerAvisos()
    {
        return _avisos.ToList();
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
