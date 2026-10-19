using SGC.Entidades;

namespace SGC.Logica;

// Disponibilidad de un medico en una fecha concreta: los bloques de su agenda
// semanal para ese dia de la semana mas las ausencias (ExcepcionAgenda)
// cargadas para esa fecha. Es de solo lectura: sirve para ofrecer horarios en
// la pantalla de Turnos y para explicar el motivo REAL cuando no hay ninguno.
public class DisponibilidadDia
{
    public DateOnly Fecha { get; init; }

    // Bloques activos de la agenda del medico para el dia de la semana de Fecha.
    public List<AgendaMedico> Bloques { get; init; } = new();

    // Ausencias activas del medico cargadas para exactamente esa fecha.
    public List<ExcepcionAgenda> Ausencias { get; init; } = new();

    // Todos los dias de la semana en que el medico tiene agenda (en orden lunes a domingo).
    public List<DayOfWeek> DiasQueAtiende { get; init; } = new();

    public bool AusenteTodoElDia => Ausencias.Any(a => a.Tipo == TipoExcepcionAgenda.DiaCompleto);

    // Ausencias parciales (por rango horario) de ese dia.
    public IEnumerable<ExcepcionAgenda> AusenciasParciales =>
        Ausencias.Where(a => a.Tipo == TipoExcepcionAgenda.RangoHorario && a.HoraInicio != null && a.HoraFin != null);

    // true si el tramo [inicio, fin) cae total o parcialmente en una ausencia.
    public bool HayAusenciaEn(TimeOnly inicio, TimeOnly fin)
    {
        if (AusenteTodoElDia) return true;
        return AusenciasParciales.Any(a => inicio < a.HoraFin && a.HoraInicio < fin);
    }

    // Motivo por el que NO hay turnos para ofrecer ese dia, o null si el medico
    // si atiende (aunque pueda tener ausencias parciales o todo ocupado).
    public string? MotivoSinAtencion(string nombreMedico)
    {
        var ausenciaDia = Ausencias.FirstOrDefault(a => a.Tipo == TipoExcepcionAgenda.DiaCompleto);
        if (ausenciaDia != null)
        {
            string motivo = string.IsNullOrWhiteSpace(ausenciaDia.Motivo) ? "" : $" ({ausenciaDia.Motivo.Trim()})";
            return $"{nombreMedico} está ausente el {Fecha:dd/MM/yyyy}{motivo}.";
        }

        if (Bloques.Count > 0) return null;

        if (DiasQueAtiende.Count == 0)
            return $"{nombreMedico} no tiene horarios de atención cargados.";

        return $"{nombreMedico} no atiende los {DiaPlural(Fecha.DayOfWeek)}. Atiende: {UnirDias(DiasQueAtiende)}.";
    }

    // Texto de las ausencias parciales del dia (null si no hay).
    public string? DescripcionAusenciasParciales()
    {
        var parciales = AusenciasParciales.OrderBy(a => a.HoraInicio).ToList();
        if (parciales.Count == 0) return null;

        var partes = parciales.Select(a =>
        {
            string motivo = string.IsNullOrWhiteSpace(a.Motivo) ? "" : $" ({a.Motivo.Trim()})";
            return $"{a.HoraInicio:HH:mm} a {a.HoraFin:HH:mm}{motivo}";
        });
        return "Ausencia de " + string.Join("; ", partes) + ": esos horarios no se pueden asignar.";
    }

    public static string NombreDiaMinuscula(DayOfWeek dia) => dia switch
    {
        DayOfWeek.Monday => "lunes",
        DayOfWeek.Tuesday => "martes",
        DayOfWeek.Wednesday => "miércoles",
        DayOfWeek.Thursday => "jueves",
        DayOfWeek.Friday => "viernes",
        DayOfWeek.Saturday => "sábado",
        _ => "domingo"
    };

    // "los lunes", "los sabados": lunes a viernes no cambian en plural.
    private static string DiaPlural(DayOfWeek dia) => dia switch
    {
        DayOfWeek.Saturday => "sábados",
        DayOfWeek.Sunday => "domingos",
        _ => NombreDiaMinuscula(dia)
    };

    private static string UnirDias(List<DayOfWeek> dias)
    {
        var nombres = dias.Select(NombreDiaMinuscula).ToList();
        if (nombres.Count == 1) return nombres[0];
        return string.Join(", ", nombres.Take(nombres.Count - 1)) + " y " + nombres[^1];
    }
}
