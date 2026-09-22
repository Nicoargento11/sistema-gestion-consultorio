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
        // Aplica migraciones pendientes (crea la base si no existe) y
        // siembra datos de prueba la primera vez que arranca contra una
        // base vacia. Falla rapido y visible si SQL Server no esta
        // disponible, en vez de que cada pantalla tire un error distinto
        // mas adelante.
        using (var contexto = SGCContextFactory.Crear())
        {
            try
            {
                contexto.Database.Migrate();
                DbSeeder.Sembrar(contexto);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo conectar/inicializar la base de datos.\n\n{ex.Message}",
                    "Error de conexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new FormLogin());
    }
}