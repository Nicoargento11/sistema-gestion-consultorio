using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SGC.Datos;

namespace SGC.Logica;

public class ResultadoBackup
{
    public string Ruta { get; set; } = string.Empty;
    public long? TamanoBytes { get; set; }
    public TimeSpan Duracion { get; set; }

    // true si RESTORE VERIFYONLY ... WITH CHECKSUM confirmo que el archivo es legible.
    public bool Verificado { get; set; }
    public string? DetalleVerificacion { get; set; }
}

public class FilaHistorialBackup
{
    public DateTime Fecha { get; set; }
    public string Ruta { get; set; } = string.Empty;
    public long TamanoBytes { get; set; }

    public string Archivo
    {
        get
        {
            int corte = Ruta.LastIndexOfAny(new[] { '\\', '/' });
            return corte < 0 ? Ruta : Ruta[(corte + 1)..];
        }
    }

    public decimal TamanoMb => Math.Round(TamanoBytes / 1024m / 1024m, 2);
}

public class HistorialBackup
{
    public List<FilaHistorialBackup> Filas { get; set; } = new();

    // false si no se pudo leer msdb (sin permisos o sin acceso): en ese caso
    // Filas esta vacia y Mensaje explica el motivo.
    public bool Disponible { get; set; } = true;
    public string? Mensaje { get; set; }
}

// Copias de seguridad de la base. Todo se ejecuta en el servidor SQL: el
// archivo .bak lo escribe/lee la CUENTA DE SERVICIO de SQL Server, no la
// aplicacion, por eso la carpeta tiene que existir y ser accesible desde el
// servidor. El historial sale de msdb (no hay tabla propia).
public class BackupService
{
    private readonly string _connectionString;
    private readonly Func<DateTime> _ahora;

    // connectionString null = la misma que usa el resto de la aplicacion.
    // El reloj se inyecta para poder probar el nombre del archivo.
    public BackupService(string? connectionString = null, Func<DateTime>? ahora = null)
    {
        _connectionString = connectionString ?? ObtenerConnectionStringDeLaApp();
        _ahora = ahora ?? (() => DateTime.Now);
    }

    private static string ObtenerConnectionStringDeLaApp()
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.Database.GetConnectionString()
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión de la base de datos.");
    }

    // Nombre de la base tomado de la conexion (Initial Catalog / Database).
    public string NombreBase
    {
        get
        {
            var nombre = new SqlConnectionStringBuilder(_connectionString).InitialCatalog;
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("La cadena de conexión no indica la base de datos.");
            return nombre;
        }
    }

    // Nombre entre corchetes escapando "]" (equivale a QUOTENAME). Los
    // identificadores no admiten parametros, por eso se escapa en vez de concatenar.
    internal static string Entre(string identificador) => "[" + identificador.Replace("]", "]]") + "]";

    public string NombreArchivo() => $"SGC_{_ahora():yyyyMMdd_HHmmss}.bak";

    // ------------------------------------------------------------- carpeta

    // Carpeta de backup por defecto de la instancia (la unica donde el servicio
    // de SQL Server suele tener permiso de escritura). "" si no se pudo saber.
    public string ObtenerCarpetaPorDefecto()
    {
        try
        {
            using var conexion = new SqlConnection(_connectionString);
            conexion.Open();
            using var comando = new SqlCommand(
                "SELECT CAST(SERVERPROPERTY('InstanceDefaultBackupPath') AS nvarchar(512))", conexion);
            return comando.ExecuteScalar() as string ?? "";
        }
        catch (SqlException)
        {
            return "";
        }
    }

    // --------------------------------------------------------------- backup

    public ResultadoBackup RealizarBackup(string carpetaDestino)
    {
        if (string.IsNullOrWhiteSpace(carpetaDestino))
            throw new ArgumentException("Indicá la carpeta de destino de la copia de seguridad.");

        string ruta = carpetaDestino.Trim().TrimEnd('\\', '/') + "\\" + NombreArchivo();
        string baseDatos = NombreBase;
        var reloj = Stopwatch.StartNew();

        try
        {
            using var conexion = new SqlConnection(_connectionString);
            conexion.Open();

            Ejecutar(conexion,
                $"BACKUP DATABASE {Entre(baseDatos)} TO DISK = @ruta WITH INIT, CHECKSUM, STATS = 10",
                ruta);

            var resultado = new ResultadoBackup { Ruta = ruta };

            // Un backup que no se puede leer de vuelta no sirve: se verifica siempre.
            try
            {
                Ejecutar(conexion, "RESTORE VERIFYONLY FROM DISK = @ruta WITH CHECKSUM", ruta);
                resultado.Verificado = true;
            }
            catch (SqlException ex)
            {
                resultado.Verificado = false;
                resultado.DetalleVerificacion = ex.Message;
            }

            reloj.Stop();
            resultado.Duracion = reloj.Elapsed;
            resultado.TamanoBytes = ObtenerTamano(conexion, ruta);
            return resultado;
        }
        catch (SqlException ex)
        {
            throw new InvalidOperationException(TraducirError(ex, "crear la copia de seguridad", ruta), ex);
        }
    }

    // Tamano segun msdb; null si no se puede leer.
    private static long? ObtenerTamano(SqlConnection conexion, string ruta)
    {
        try
        {
            using var comando = new SqlCommand(
                @"SELECT TOP (1) bs.backup_size
                  FROM msdb.dbo.backupset bs
                  JOIN msdb.dbo.backupmediafamily mf ON mf.media_set_id = bs.media_set_id
                  WHERE mf.physical_device_name = @ruta
                  ORDER BY bs.backup_finish_date DESC", conexion);
            comando.Parameters.Add(new SqlParameter("@ruta", System.Data.SqlDbType.NVarChar, 512) { Value = ruta });
            var valor = comando.ExecuteScalar();
            return valor == null || valor is DBNull ? null : Convert.ToInt64(valor);
        }
        catch (SqlException)
        {
            return null;
        }
    }

    // ------------------------------------------------------------- historial

    public HistorialBackup ObtenerHistorial(int max = 20)
    {
        var historial = new HistorialBackup();
        if (max <= 0) return historial;

        try
        {
            using var conexion = new SqlConnection(_connectionString);
            conexion.Open();
            using var comando = new SqlCommand(
                @"SELECT TOP (@max) bs.backup_finish_date, mf.physical_device_name, bs.backup_size
                  FROM msdb.dbo.backupset bs
                  JOIN msdb.dbo.backupmediafamily mf ON mf.media_set_id = bs.media_set_id
                  WHERE bs.database_name = @base AND bs.type = 'D'
                  ORDER BY bs.backup_finish_date DESC", conexion);
            comando.Parameters.Add(new SqlParameter("@max", System.Data.SqlDbType.Int) { Value = max });
            comando.Parameters.Add(new SqlParameter("@base", System.Data.SqlDbType.NVarChar, 128) { Value = NombreBase });

            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                historial.Filas.Add(new FilaHistorialBackup
                {
                    Fecha = lector.GetDateTime(0),
                    Ruta = lector.GetString(1),
                    TamanoBytes = lector.IsDBNull(2) ? 0 : Convert.ToInt64(lector.GetValue(2))
                });
            }
        }
        catch (SqlException ex)
        {
            historial.Filas.Clear();
            historial.Disponible = false;
            historial.Mensaje = "No se pudo leer el historial de copias (msdb): " + ex.Message;
        }
        return historial;
    }

    // Fin de la ultima copia completa; null si no hay o no se puede consultar.
    public DateTime? ObtenerUltimoBackup()
    {
        try
        {
            using var conexion = new SqlConnection(_connectionString);
            conexion.Open();
            using var comando = new SqlCommand(
                "SELECT MAX(backup_finish_date) FROM msdb.dbo.backupset WHERE database_name = @base AND type = 'D'",
                conexion);
            comando.Parameters.Add(new SqlParameter("@base", System.Data.SqlDbType.NVarChar, 128) { Value = NombreBase });
            var valor = comando.ExecuteScalar();
            return valor == null || valor is DBNull ? null : (DateTime)valor;
        }
        catch (SqlException)
        {
            return null;
        }
    }

    // ------------------------------------------------------------- restaurar

    // Reemplaza la base actual por el contenido del .bak. Cierra todas las
    // conexiones (SINGLE_USER ... ROLLBACK IMMEDIATE) y SIEMPRE vuelve a
    // MULTI_USER, aun si la restauracion falla.
    public void Restaurar(string rutaBak)
    {
        if (string.IsNullOrWhiteSpace(rutaBak))
            throw new ArgumentException("Indicá el archivo de copia de seguridad (.bak) a restaurar.");

        rutaBak = rutaBak.Trim();
        string baseDatos = NombreBase;
        var conexionMaster = new SqlConnectionStringBuilder(_connectionString) { InitialCatalog = "master" }.ConnectionString;

        // Se cierran los pooles propios antes y despues: las conexiones
        // reutilizadas apuntarian a la base anterior.
        SqlConnection.ClearAllPools();
        try
        {
            using var conexion = new SqlConnection(conexionMaster);
            conexion.Open();

            // 1) El archivo tiene que ser legible antes de tocar la base.
            Ejecutar(conexion, "RESTORE VERIFYONLY FROM DISK = @ruta WITH CHECKSUM", rutaBak);

            // 2) Acceso exclusivo: cierra las demas conexiones.
            Ejecutar(conexion, $"ALTER DATABASE {Entre(baseDatos)} SET SINGLE_USER WITH ROLLBACK IMMEDIATE", null);
            bool fallo = true;
            try
            {
                // 3) Reemplazo de la base.
                Ejecutar(conexion, $"RESTORE DATABASE {Entre(baseDatos)} FROM DISK = @ruta WITH REPLACE, STATS = 10", rutaBak);
                fallo = false;
            }
            finally
            {
                // 4) Nunca dejar la base en SINGLE_USER. Si ya hubo un error
                // se ignora uno nuevo para no tapar el original.
                try
                {
                    Ejecutar(conexion, $"ALTER DATABASE {Entre(baseDatos)} SET MULTI_USER", null);
                }
                catch (SqlException) when (fallo)
                {
                }
            }
        }
        catch (SqlException ex)
        {
            throw new InvalidOperationException(TraducirError(ex, "restaurar la copia de seguridad", rutaBak), ex);
        }
        finally
        {
            SqlConnection.ClearAllPools();
        }
    }

    // ---------------------------------------------------------------- helpers

    // Ejecuta una sentencia sin limite de tiempo (un backup puede tardar
    // minutos). La ruta, si hay, siempre viaja como parametro.
    private static void Ejecutar(SqlConnection conexion, string sentencia, string? ruta)
    {
        using var comando = new SqlCommand(sentencia, conexion) { CommandTimeout = 0 };
        if (ruta != null)
            comando.Parameters.Add(new SqlParameter("@ruta", System.Data.SqlDbType.NVarChar, 512) { Value = ruta });
        comando.ExecuteNonQuery();
    }

    private static string TraducirError(SqlException ex, string operacion, string ruta)
    {
        string detalle = ex.Number switch
        {
            // No se puede abrir el dispositivo de backup (ruta inexistente o sin permiso del SO).
            3201 or 3203 or 3013 =>
                $"SQL Server no pudo acceder a \"{ruta}\". La ruta la usa la cuenta de servicio de SQL Server (no la aplicación): " +
                "verificá que la carpeta exista en el servidor y que esa cuenta tenga permiso de escritura (para restaurar, de lectura). " +
                $"Detalle: {ex.Message}",
            // Permisos de base de datos del usuario de conexion.
            262 or 229 or 4834 or 3110 =>
                "El usuario de conexión no tiene permisos para esta operación (se requiere un rol con permiso de backup/restore, por ejemplo db_owner o sysadmin).",
            _ => ex.Message
        };
        return $"No se pudo {operacion}. {detalle}";
    }
}
