using System.Globalization;

namespace SGC.Entidades;

public class AgendaMedico
{
    public int Id { get; set; }

    public int MedicoId { get; set; }
    public Medico? Medico { get; set; }

    public DayOfWeek DiaSemana { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }
    public bool Activo { get; set; } = true;

    public string MedicoNombre => Medico?.NombreCompleto ?? "";
    public string Rango => $"{HoraInicio:HH:mm} - {HoraFin:HH:mm}";

    // DayOfWeek.ToString() siempre da el nombre en ingles (no depende de la
    // cultura de Windows) - GetDayName si respeta la cultura, por eso lo usamos
    // para mostrar el dia en espanol en UI.
    public string DiaSemanaTexto => CultureInfo.GetCultureInfo("es-AR").DateTimeFormat.GetDayName(DiaSemana);
}
