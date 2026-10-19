using System.Globalization;
using SGC.Entidades;

namespace SGC.Logica;

// Una fila de la vista semanal de horarios: un medico con sus franjas
// activas de lunes a domingo y el total de horas semanales.
public class AgendaSemanalFila
{
    private static readonly CultureInfo CulturaAr = new("es-AR");

    // Lunes a domingo, en el orden en que se muestran las columnas.
    public static readonly DayOfWeek[] DiasEnOrden =
    {
        DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
        DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
    };

    public required Medico Medico { get; init; }
    public IReadOnlyList<AgendaMedico> Franjas { get; init; } = Array.Empty<AgendaMedico>();

    public int MedicoId => Medico.Id;
    public string MedicoNombre => Medico.NombreCompleto;

    public int MinutosSemanales => Franjas.Sum(f => (int)(f.HoraFin - f.HoraInicio).TotalMinutes);
    public string HorasSemanalesTexto => FormatearMinutos(MinutosSemanales);

    // Franjas del dia ordenadas, una por linea ("08:00 - 13:00"); vacio si no atiende.
    public string TextoDia(DayOfWeek dia)
    {
        return string.Join("\n", Franjas
            .Where(f => f.DiaSemana == dia)
            .OrderBy(f => f.HoraInicio)
            .Select(f => f.HorarioRango));
    }

    // Nombre del dia en es-AR con la primera letra en mayuscula (Miercoles, Sabado).
    public static string NombreDia(DayOfWeek dia)
    {
        var nombre = CulturaAr.DateTimeFormat.GetDayName(dia);
        return char.ToUpper(nombre[0], CulturaAr) + nombre[1..];
    }

    public static string FormatearMinutos(int minutos)
    {
        if (minutos <= 0) return "0 h";
        var horas = minutos / 60;
        var resto = minutos % 60;
        return resto == 0 ? $"{horas} h" : $"{horas} h {resto} min";
    }
}
