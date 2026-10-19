using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SGC.Datos;
using SGC.Entidades;

namespace SGC.Logica;

public class ReporteService
{
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
}
