using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGC.Datos.Migrations
{
    // Procedimientos del modulo de Reportes de Luciano Villordo (pacientes por
    // mes, rendimiento de medicos y turnos por obra social). El SQL esta
    // copiado de SGC.Datos/StoredProcedures/*.sql (fuente legible); si se
    // modifica un .sql, hay que crear una migracion nueva con CREATE OR ALTER.
    /// <inheritdoc />
    public partial class AgregaSpReportesPacientesRendimientoObraSocial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // sp_ReportePacientesPorMes
            migrationBuilder.Sql("""
CREATE OR ALTER PROCEDURE sp_ReportePacientesPorMes
    @Anio INT,
    @MedicoId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- EstadoTurno enum en C#:
    -- 0: Pendiente, 1: Confirmado, 2: Cancelado, 3: Asistio, 4: Ausente

    SELECT 
        MONTH(t.Fecha) AS MesNumero,
        CASE MONTH(t.Fecha)
            WHEN 1 THEN 'Enero'
            WHEN 2 THEN 'Febrero'
            WHEN 3 THEN 'Marzo'
            WHEN 4 THEN 'Abril'
            WHEN 5 THEN 'Mayo'
            WHEN 6 THEN 'Junio'
            WHEN 7 THEN 'Julio'
            WHEN 8 THEN 'Agosto'
            WHEN 9 THEN 'Septiembre'
            WHEN 10 THEN 'Octubre'
            WHEN 11 THEN 'Noviembre'
            WHEN 12 THEN 'Diciembre'
        END AS NombreMes,
        COUNT(t.Id) AS TotalTurnos,
        COUNT(CASE WHEN t.Estado = 3 THEN 1 END) AS PacientesAtendidos,
        COUNT(CASE WHEN t.Estado = 4 THEN 1 END) AS Ausentes,
        COUNT(CASE WHEN t.Estado = 2 THEN 1 END) AS Cancelados,
        COUNT(CASE WHEN t.Estado IN (0, 1) THEN 1 END) AS PendientesOConfirmados
    FROM Turnos t
    WHERE YEAR(t.Fecha) = @Anio
      AND (@MedicoId IS NULL OR t.MedicoId = @MedicoId)
    GROUP BY MONTH(t.Fecha)
    ORDER BY MesNumero;
END;
""");

            // sp_ReporteRendimientoMedicos
            migrationBuilder.Sql("""
CREATE OR ALTER PROCEDURE sp_ReporteRendimientoMedicos
    @Anio INT,
    @Mes INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        m.Id AS MedicoId,
        m.Apellido + ', ' + m.Nombre AS MedicoNombre,
        m.Especialidad,
        COUNT(t.Id) AS TotalTurnos,
        COUNT(CASE WHEN t.Estado = 3 THEN 1 END) AS TurnosAtendidos,
        COUNT(CASE WHEN t.Estado = 4 THEN 1 END) AS TurnosAusentes,
        COUNT(CASE WHEN t.Estado = 2 THEN 1 END) AS TurnosCancelados,
        ISNULL(SUM(t.Monto), 0) AS TotalRecaudado
    FROM Medicos m
    LEFT JOIN Turnos t ON m.Id = t.MedicoId 
                       AND YEAR(t.Fecha) = @Anio
                       AND (@Mes IS NULL OR MONTH(t.Fecha) = @Mes)
    WHERE m.Activo = 1
    GROUP BY m.Id, m.Apellido, m.Nombre, m.Especialidad
    ORDER BY TurnosAtendidos DESC, TotalTurnos DESC;
END;
""");

            // sp_ReporteTurnosPorObraSocial
            migrationBuilder.Sql("""
CREATE OR ALTER PROCEDURE sp_ReporteTurnosPorObraSocial
    @Anio INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        ISNULL(os.Nombre, 'Particular') AS ObraSocial,
        COUNT(t.Id) AS CantidadTurnos,
        ISNULL(SUM(t.Monto), 0) AS TotalRecaudado
    FROM Turnos t
    INNER JOIN Pacientes p ON t.PacienteId = p.Id
    LEFT JOIN ObrasSociales os ON p.ObraSocialId = os.Id
    WHERE YEAR(t.Fecha) = @Anio 
      AND t.Activo = 1
    GROUP BY os.Nombre
    ORDER BY CantidadTurnos DESC;
END;
""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ReportePacientesPorMes;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ReporteRendimientoMedicos;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ReporteTurnosPorObraSocial;");
        }
    }
}
