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
        @"Server=.\NICO_SERV1;Database=SGC;Trusted_Connection=True;TrustServerCertificate=True;";

    public static SGCContext Crear()
    {
        var options = new DbContextOptionsBuilder<SGCContext>()
            .UseSqlServer(ObtenerConnectionString())
            .Options;

        return new SGCContext(options);
    }

    private static string ObtenerConnectionString()
    {
        var rutaConfig = BuscarAppsettingsLocal();
        if (rutaConfig == null)
            return ConnectionStringPorDefecto;

        using var stream = File.OpenRead(rutaConfig);
        using var documento = JsonDocument.Parse(stream);

        return documento.RootElement.TryGetProperty("ConnectionString", out var valor) && valor.GetString() is string cadena
            ? cadena
            : ConnectionStringPorDefecto;
    }

    // Sube carpetas desde donde este corriendo el proceso (bin/Debug/net8.0
    // del .exe, o bin/Debug/net8.0 de SGC.Datos si es "dotnet ef") hasta
    // encontrar la carpeta del .sln, y ahi busca appsettings.Local.json.
    // Funciona igual sin importar si se lanza con F5, el .exe directo, o
    // "dotnet ef" desde la consola.
    private static string? BuscarAppsettingsLocal()
    {
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
