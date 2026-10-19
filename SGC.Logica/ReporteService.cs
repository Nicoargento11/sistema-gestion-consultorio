using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SGC.Datos;
using SGC.Entidades;
using SGC.Entidades.Reportes;

namespace SGC.Logica;

// Reportes y estadisticas: la agregacion vive en stored procedures de SQL
// Server (migracion AgregaStoredProcedures); este servicio solo valida los
// parametros y los ejecuta con SqlQueryRaw.
//
// RNF#07: ningun procedimiento ni DTO de reportes incluye datos clinicos
// (MotivoConsulta, RecetaMedicamentos, Procedimiento), asi que estos metodos
// son seguros para cualquier rol (incluida Recepcionista). Restringir a que
// medicos puede ver cada usuario es responsabilidad de la UI (medicoId).
public class ReporteService
{
    public List<ReporteTurnoFila> ObtenerTurnosPorPeriodo(DateOnly desde, DateOnly hasta, int? medicoId = null, EstadoTurno? estado = null)
    {
        ValidarPeriodo(desde, hasta);

        using var contexto = SGCContextFactory.Crear();
        return contexto.Database.SqlQueryRaw<ReporteTurnoFila>(
                "EXEC sp_ReporteTurnosPorPeriodo @FechaDesde, @FechaHasta, @MedicoId, @Estado",
                ParametroFecha("@FechaDesde", desde),
                ParametroFecha("@FechaHasta", hasta),
                ParametroEntero("@MedicoId", medicoId),
                ParametroEntero("@Estado", estado.HasValue ? (int)estado.Value : null))
            .ToList();
    }

    public List<ReporteRecaudacionFila> ObtenerRecaudacion(DateOnly desde, DateOnly hasta, AgrupacionRecaudacion agruparPor)
    {
        ValidarPeriodo(desde, hasta);

        string criterio = agruparPor switch
        {
            AgrupacionRecaudacion.Medico => "MEDICO",
            AgrupacionRecaudacion.MedioPago => "MEDIO_PAGO",
            AgrupacionRecaudacion.ObraSocial => "OBRA_SOCIAL",
            _ => throw new ArgumentException("El criterio de agrupación no es válido.")
        };

        using var contexto = SGCContextFactory.Crear();
        return contexto.Database.SqlQueryRaw<ReporteRecaudacionFila>(
                "EXEC sp_ReporteRecaudacion @FechaDesde, @FechaHasta, @AgruparPor",
                ParametroFecha("@FechaDesde", desde),
                ParametroFecha("@FechaHasta", hasta),
                new SqlParameter("@AgruparPor", System.Data.SqlDbType.NVarChar, 20) { Value = criterio })
            .ToList();
    }

    public List<ReporteAsistenciaFila> ObtenerAsistencia(DateOnly desde, DateOnly hasta, int? medicoId = null)
    {
        ValidarPeriodo(desde, hasta);

        using var contexto = SGCContextFactory.Crear();
        return contexto.Database.SqlQueryRaw<ReporteAsistenciaFila>(
                "EXEC sp_ReporteAsistencia @FechaDesde, @FechaHasta, @MedicoId",
                ParametroFecha("@FechaDesde", desde),
                ParametroFecha("@FechaHasta", hasta),
                ParametroEntero("@MedicoId", medicoId))
            .ToList();
    }

    public List<ReporteOcupacionFila> ObtenerOcupacionAgenda(DateOnly desde, DateOnly hasta, int? medicoId = null)
    {
        ValidarPeriodo(desde, hasta);

        // El procedimiento genera una fila por dia con un CTE recursivo y lo
        // limita a 366 dias; se valida aca tambien para dar un mensaje claro.
        if (hasta.DayNumber - desde.DayNumber > 365)
            throw new ArgumentException("El rango de fechas no puede superar los 366 días.");

        using var contexto = SGCContextFactory.Crear();
        return contexto.Database.SqlQueryRaw<ReporteOcupacionFila>(
                "EXEC sp_ReporteOcupacionAgenda @FechaDesde, @FechaHasta, @MedicoId",
                ParametroFecha("@FechaDesde", desde),
                ParametroFecha("@FechaHasta", hasta),
                ParametroEntero("@MedicoId", medicoId))
            .ToList();
    }

    // ---- Reportes del modulo de Luciano Villordo (pacientes por mes, rendimiento
    // de medicos y turnos por obra social). Usan sus stored procedures
    // (migracion AgregaSpReportesPacientesRendimientoObraSocial). ----

    public List<ReporteMensualItem> ObtenerPacientesPorMes(int anio, int? medicoId = null)
    {
        var lista = new List<ReporteMensualItem>();
        using var contexto = SGCContextFactory.Crear();
        using var conexion = contexto.Database.GetDbConnection();

        using var comando = conexion.CreateCommand();
        comando.CommandText = "sp_ReportePacientesPorMes";
        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add(new SqlParameter("@Anio", anio));
        comando.Parameters.Add(new SqlParameter("@MedicoId", (object?)medicoId ?? DBNull.Value));

        if (conexion.State != ConnectionState.Open)
            conexion.Open();

        using var reader = comando.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new ReporteMensualItem
            {
                MesNumero = reader.GetInt32(reader.GetOrdinal("MesNumero")),
                NombreMes = reader.GetString(reader.GetOrdinal("NombreMes")),
                TotalTurnos = reader.GetInt32(reader.GetOrdinal("TotalTurnos")),
                PacientesAtendidos = reader.GetInt32(reader.GetOrdinal("PacientesAtendidos")),
                Ausentes = reader.GetInt32(reader.GetOrdinal("Ausentes")),
                Cancelados = reader.GetInt32(reader.GetOrdinal("Cancelados")),
                PendientesOConfirmados = reader.GetInt32(reader.GetOrdinal("PendientesOConfirmados"))
            });
        }

        return lista;
    }

    public List<ReporteObraSocialItem> ObtenerTurnosPorObraSocial(int anio)
    {
        var lista = new List<ReporteObraSocialItem>();
        using var contexto = SGCContextFactory.Crear();
        using var conexion = contexto.Database.GetDbConnection();

        using var comando = conexion.CreateCommand();
        comando.CommandText = "sp_ReporteTurnosPorObraSocial";
        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add(new SqlParameter("@Anio", anio));

        if (conexion.State != ConnectionState.Open)
            conexion.Open();

        using var reader = comando.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new ReporteObraSocialItem
            {
                ObraSocial = reader.GetString(reader.GetOrdinal("ObraSocial")),
                CantidadTurnos = reader.GetInt32(reader.GetOrdinal("CantidadTurnos")),
                TotalRecaudado = reader.GetDecimal(reader.GetOrdinal("TotalRecaudado"))
            });
        }

        return lista;
    }

    public List<ReporteMedicoRendimientoItem> ObtenerRendimientoMedicos(int anio, int? mes = null)
    {
        var lista = new List<ReporteMedicoRendimientoItem>();
        using var contexto = SGCContextFactory.Crear();
        using var conexion = contexto.Database.GetDbConnection();

        using var comando = conexion.CreateCommand();
        comando.CommandText = "sp_ReporteRendimientoMedicos";
        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add(new SqlParameter("@Anio", anio));
        comando.Parameters.Add(new SqlParameter("@Mes", (object?)mes ?? DBNull.Value));

        if (conexion.State != ConnectionState.Open)
            conexion.Open();

        using var reader = comando.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new ReporteMedicoRendimientoItem
            {
                MedicoId = reader.GetInt32(reader.GetOrdinal("MedicoId")),
                MedicoNombre = reader.GetString(reader.GetOrdinal("MedicoNombre")),
                Especialidad = reader.GetString(reader.GetOrdinal("Especialidad")),
                TotalTurnos = reader.GetInt32(reader.GetOrdinal("TotalTurnos")),
                TurnosAtendidos = reader.GetInt32(reader.GetOrdinal("TurnosAtendidos")),
                TurnosAusentes = reader.GetInt32(reader.GetOrdinal("TurnosAusentes")),
                TurnosCancelados = reader.GetInt32(reader.GetOrdinal("TurnosCancelados")),
                TotalRecaudado = reader.GetDecimal(reader.GetOrdinal("TotalRecaudado"))
            });
        }

        return lista;
    }

    private static void ValidarPeriodo(DateOnly desde, DateOnly hasta)
    {
        if (desde > hasta)
            throw new ArgumentException("La fecha desde no puede ser posterior a la fecha hasta.");
    }

    private static SqlParameter ParametroFecha(string nombre, DateOnly fecha)
    {
        return new SqlParameter(nombre, System.Data.SqlDbType.Date) { Value = fecha.ToDateTime(TimeOnly.MinValue) };
    }

    // Los parametros opcionales deben viajar como DBNull.Value, no como null.
    private static SqlParameter ParametroEntero(string nombre, int? valor)
    {
        return new SqlParameter(nombre, System.Data.SqlDbType.Int) { Value = valor.HasValue ? valor.Value : DBNull.Value };
    }
}
