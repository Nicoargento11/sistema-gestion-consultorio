using SGC.Entidades;

namespace SGC.Logica;

// Fila del historial de atenciones: la actividad medica mas el criterio de
// visibilidad de los datos clinicos. RNF#07 (secreto medico): motivo,
// procedimiento y receta SOLO se exponen cuando IncluyeDatosClinicos es true
// (rol Medico); en cualquier otro caso devuelven cadena vacia, de modo que
// ni la grilla, ni el panel de detalle, ni los tooltips pueden mostrarlos.
public class AtencionFila
{
    // Largo maximo del texto de una celda de grilla antes de truncarlo.
    public const int LargoResumen = 60;

    public required ActividadMedica Actividad { get; init; }
    public bool IncluyeDatosClinicos { get; init; }

    public int Id => Actividad.Id;
    public string FechaStr => Actividad.FechaStr;
    public string HorarioStr => Actividad.HorarioStr;
    public string MedicoNombre => Actividad.MedicoNombre;
    public string TipoActividadNombre => Actividad.TipoActividadNombre;

    public string MotivoConsulta => IncluyeDatosClinicos ? Actividad.MotivoConsulta ?? string.Empty : string.Empty;
    public string Procedimiento => IncluyeDatosClinicos ? Actividad.Procedimiento ?? string.Empty : string.Empty;
    public string RecetaMedicamentos => IncluyeDatosClinicos ? Actividad.RecetaMedicamentos ?? string.Empty : string.Empty;

    public string MotivoResumen => Resumir(MotivoConsulta);
    public string ProcedimientoResumen => Resumir(Procedimiento);
    public string RecetaResumen => Resumir(RecetaMedicamentos);

    // Una sola linea: saltos de linea y espacios repetidos se colapsan y, si
    // supera el largo maximo, se corta con puntos suspensivos.
    public static string Resumir(string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return string.Empty;

        var linea = string.Join(" ", texto.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
        return linea.Length <= LargoResumen ? linea : linea[..(LargoResumen - 3)] + "...";
    }
}
