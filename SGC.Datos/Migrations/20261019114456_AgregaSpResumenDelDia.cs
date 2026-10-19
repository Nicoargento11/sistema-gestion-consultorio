using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGC.Datos.Migrations
{
    // El SQL del procedimiento esta copiado de
    // SGC.Datos/StoredProcedures/sp_ResumenDelDia.sql (fuente legible). Si se
    // modifica el .sql, hay que crear una migracion nueva con CREATE OR ALTER.
    /// <inheritdoc />
    public partial class AgregaSpResumenDelDia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // sp_ResumenDelDia
            migrationBuilder.Sql("""
-- Resumen para la pantalla de inicio: una sola fila con los contadores del
-- dia @Fecha (por estado), de la semana (lunes a domingo) y del mes en que
-- cae @Fecha. @MedicoId NULL = todos los medicos.
-- Estado: 0=Pendiente, 1=Confirmado, 2=Cancelado, 3=Asistio, 4=Ausente.
-- RNF#07: no devuelve datos clinicos ni datos de pacientes.
-- Los dias de la semana se calculan con DATEDIFF desde 1900-01-01 (lunes) y
-- 1900-01-07 (domingo) para no depender de @@DATEFIRST.
CREATE OR ALTER PROCEDURE sp_ResumenDelDia
    @Fecha DATE,
    @MedicoId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @InicioSemana DATE = DATEADD(DAY, -(((DATEDIFF(DAY, '19000101', @Fecha) % 7) + 7) % 7), @Fecha);
    DECLARE @FinSemana DATE = DATEADD(DAY, 6, @InicioSemana);
    DECLARE @InicioMes DATE = DATEFROMPARTS(YEAR(@Fecha), MONTH(@Fecha), 1);
    DECLARE @FinMes DATE = EOMONTH(@Fecha);
    DECLARE @DiaSemana INT = ((DATEDIFF(DAY, '19000107', @Fecha) % 7) + 7) % 7;

    -- Medicos activos con franja de agenda ese dia y sin ausencia de dia completo.
    -- Se cuenta aparte (no correlacionado con el resto) para no mezclar
    -- agregados de tablas distintas.
    DECLARE @MedicosConAgenda INT =
    (
        SELECT COUNT(DISTINCT a.MedicoId)
        FROM AgendasMedico a
        INNER JOIN Medicos m ON m.Id = a.MedicoId
        WHERE a.Activo = 1
          AND m.Activo = 1
          AND a.DiaSemana = @DiaSemana
          AND (@MedicoId IS NULL OR a.MedicoId = @MedicoId)
          AND NOT EXISTS (
              SELECT 1 FROM ExcepcionesAgenda e
              WHERE e.MedicoId = a.MedicoId
                AND e.Activo = 1
                AND e.Fecha = @Fecha
                AND e.Tipo = 0)
    );

    -- Una sola pasada sobre Turnos: se calculan las filas del periodo
    -- (semana o mes) y luego se agregan con CASE.
    SELECT
        CAST(ISNULL(SUM(CASE WHEN t.Fecha = @Fecha AND t.Estado = 0 THEN 1 ELSE 0 END), 0) AS INT) AS Pendientes,
        CAST(ISNULL(SUM(CASE WHEN t.Fecha = @Fecha AND t.Estado = 1 THEN 1 ELSE 0 END), 0) AS INT) AS Confirmados,
        CAST(ISNULL(SUM(CASE WHEN t.Fecha = @Fecha AND t.Estado = 3 THEN 1 ELSE 0 END), 0) AS INT) AS Asistieron,
        CAST(ISNULL(SUM(CASE WHEN t.Fecha = @Fecha AND t.Estado = 4 THEN 1 ELSE 0 END), 0) AS INT) AS Ausentes,
        CAST(ISNULL(SUM(CASE WHEN t.Fecha = @Fecha AND t.Estado = 2 THEN 1 ELSE 0 END), 0) AS INT) AS Cancelados,
        CAST(ISNULL(SUM(CASE WHEN t.Fecha BETWEEN @InicioSemana AND @FinSemana AND t.Estado <> 2 THEN 1 ELSE 0 END), 0) AS INT) AS TurnosSemana,
        CAST(ISNULL(SUM(CASE WHEN t.Fecha BETWEEN @InicioMes AND @FinMes AND t.Estado = 4 THEN 1 ELSE 0 END), 0) AS INT) AS AusentesMes,
        CAST(ISNULL(SUM(CASE WHEN t.Fecha BETWEEN @InicioMes AND @FinMes AND t.Estado = 3 THEN t.Monto ELSE 0 END), 0) AS DECIMAL(18, 2)) AS RecaudacionMes,
        @MedicosConAgenda AS MedicosConAgendaHoy
    FROM Turnos t
    WHERE (t.Fecha BETWEEN @InicioSemana AND @FinSemana OR t.Fecha BETWEEN @InicioMes AND @FinMes)
      AND (@MedicoId IS NULL OR t.MedicoId = @MedicoId);
END;
""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ResumenDelDia;");
        }
    }
}
