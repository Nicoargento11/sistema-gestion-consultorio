using Microsoft.EntityFrameworkCore;
using SGC.Datos;

namespace SGC.UI;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        // Errores no controlados: se muestran en espanol y sin stack trace.
        // Los de la interfaz (ThreadException) no cierran la aplicacion.
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (s, e) => MostrarErrorInesperado(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            MostrarErrorInesperado(e.ExceptionObject as Exception);

        // Aplica migraciones pendientes (crea la base si no existe) y
        // siembra datos de prueba la primera vez que arranca contra una
        // base vacia. Falla rapido y visible si SQL Server no esta
        // disponible, en vez de que cada pantalla tire un error distinto
        // mas adelante. Crear() va dentro del try: un appsettings.Local.json
        // mal formado tambien se informa aca.
        var cursorAnterior = Cursor.Current;
        try
        {
            Cursor.Current = Cursors.WaitCursor;
            using var contexto = SGCContextFactory.Crear();
            contexto.Database.Migrate();
            DbSeeder.Sembrar(contexto);
        }
        catch (Exception ex)
        {
            Cursor.Current = cursorAnterior;
            MessageBox.Show(
                $"No se pudo conectar/inicializar la base de datos.\n\n{ex.Message}",
                "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        finally
        {
            Cursor.Current = cursorAnterior;
        }

        Application.Run(new FormLogin());
    }

    // Mensaje corto para el usuario; el detalle tecnico no se muestra.
    private static void MostrarErrorInesperado(Exception? ex)
    {
        string detalle = string.IsNullOrWhiteSpace(ex?.Message) ? "Sin detalle disponible." : ex.Message;
        try
        {
            MessageBox.Show(
                $"Ocurrió un error inesperado. Si el problema continúa, cierre y vuelva a abrir la aplicación.\n\n{detalle}",
                "Error inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch
        {
            // Si ni siquiera se puede mostrar el cuadro, no hay mas nada que hacer.
        }
    }
}
