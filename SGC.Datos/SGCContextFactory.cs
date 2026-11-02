using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace SGC.Datos;

// Fabrica simple del DbContext: como la app no tiene un contenedor de
// Inyeccion de Dependencias armado (WinForms sin Host/DI builder), cada
// Service crea su propio SGCContext de corta duracion por operacion
// (patron "using var contexto = SGCContextFactory.Crear();"), en vez de
// mantener uno solo vivo durante toda la app.
public static class SGCContextFactory
{
    // Fallback si no hay appsettings.Local.json (ej: tu propia maquina, que
    // ya tenia la instancia armada antes de que existiera este archivo).
    // Cada desarrollador nuevo (Luciano, etc.) crea su propio
    // appsettings.Local.json con SU instancia - ver appsettings.example.json.
    private const string ConnectionStringPorDefecto =
        @"Server=.;Database=SGC;Trusted_Connection=True;TrustServerCertificate=True;";

    // La cadena se lee una sola vez: Crear() se llama desde decenas de lugares.
    private static string? _connectionString;
    private static readonly object _candado = new();

    public static SGCContext Crear()
    {
        var options = new DbContextOptionsBuilder<SGCContext>()
            .UseSqlServer(ObtenerConnectionString())
            .Options;

        return new SGCContext(options);
    }

    private static string ObtenerConnectionString()
    {
        if (_connectionString != null)
            return _connectionString;

        lock (_candado)
        {
            return _connectionString ??= LeerConnectionString();
        }
    }

    // Un archivo mal formado o con la cadena vacia se informa con un mensaje
    // claro en espanol (en vez del error tecnico de .NET).
    private static string LeerConnectionString()
    {
        var rutaConfig = BuscarAppsettingsLocal();
        if (rutaConfig == null)
            return ConnectionStringPorDefecto;

        try
        {
            using var stream = File.OpenRead(rutaConfig);
            using var documento = JsonDocument.Parse(stream);

            if (!documento.RootElement.TryGetProperty("ConnectionString", out var valor))
                return ConnectionStringPorDefecto;

            if (valor.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(valor.GetString()))
                throw new InvalidOperationException(
                    $"El archivo appsettings.Local.json ({rutaConfig}) tiene la propiedad \"ConnectionString\" vacía o inválida. " +
                    "Indique la cadena de conexión a SQL Server (vea appsettings.example.json).");

            return valor.GetString()!;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(
                $"El archivo appsettings.Local.json ({rutaConfig}) no tiene un formato JSON válido. Corríjalo o elimínelo. Detalle: {ex.Message}", ex);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException(
                $"No se pudo leer el archivo appsettings.Local.json ({rutaConfig}). Detalle: {ex.Message}", ex);
        }
    }

    // Primero busca appsettings.Local.json junto al ejecutable (instalacion
    // sin codigo fuente). Si no esta, sube carpetas desde donde este corriendo
    // el proceso (bin/Debug/net8.0 del .exe, o bin/Debug/net8.0 de SGC.Datos
    // si es "dotnet ef") hasta encontrar la carpeta del .sln, y ahi lo busca.
    // Funciona igual sin importar si se lanza con F5, el .exe directo, o
    // "dotnet ef" desde la consola.
    private static string? BuscarAppsettingsLocal()
    {
        var junto = Path.Combine(AppContext.BaseDirectory, "appsettings.Local.json");
        if (File.Exists(junto))
            return junto;

        var directorio = new DirectoryInfo(AppContext.BaseDirectory);

        while (directorio != null)
        {
            if (directorio.GetFiles("*.sln").Length > 0)
            {
                var candidato = Path.Combine(directorio.FullName, "appsettings.Local.json");
                return File.Exists(candidato) ? candidato : null;
            }

            directorio = directorio.Parent;
        }

        return null;
    }
}
