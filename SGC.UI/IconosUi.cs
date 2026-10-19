namespace SGC.UI;

// Glifos de la fuente de iconos de Windows (Segoe Fluent Icons en Windows 11,
// Segoe MDL2 Assets en Windows 10; comparten los mismos codigos). Si ninguna
// esta instalada se usa Segoe UI Symbol. Misma eleccion que el tablero de inicio.
internal static class IconosUi
{
    public const string Inicio = "";
    public const string Pacientes = "";
    public const string Turnos = "";
    public const string Medicos = "";
    public const string Horarios = "";
    public const string Usuarios = "";
    public const string ObrasSociales = "";
    public const string Ausencias = "";
    public const string Reportes = "";
    public const string Atencion = "";
    public const string Historial = "";
    public const string CerrarSesion = "";
    public const string Salud = "";

    public static readonly string Familia = ElegirFamilia();

    private static string ElegirFamilia()
    {
        var instaladas = new System.Drawing.Text.InstalledFontCollection().Families
            .Select(f => f.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var nombre in new[] { "Segoe Fluent Icons", "Segoe MDL2 Assets" })
            if (instaladas.Contains(nombre))
                return nombre;

        return "Segoe UI Symbol";
    }

    public static Font Fuente(float puntos) => new(Familia, puntos);
}
