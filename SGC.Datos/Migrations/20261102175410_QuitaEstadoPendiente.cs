using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGC.Datos.Migrations
{
    // Quita el estado Pendiente (0) de EstadoTurno: pasa los turnos en Estado = 0
    // a Confirmado (1) y actualiza sp_ResumenDelDia (sin columna Pendientes),
    // sp_ReportePacientesPorMes (PendientesOConfirmados -> Confirmados) y
    // sp_ReporteTurnosPorPeriodo (sin el caso 0). El SQL esta copiado de
    // SGC.Datos/StoredProcedures/*.sql. Down restaura los procedimientos con
    // CREATE OR ALTER; el UPDATE de datos no se revierte.
    /// <inheritdoc />
    public partial class QuitaEstadoPendiente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Los turnos en Estado = 0 (Pendiente) pasan a Confirmado (1). El sistema nunca
            // asigno Pendiente; ver AsignarTurno y sp_AsignarTurno.
            migrationBuilder.Sql("UPDATE Turnos SET Estado = 1 WHERE Estado = 0");

            // sp_ResumenDelDia
            migrationBuilder.Sql("""
-- Resumen para la pantalla de inicio: una sola fila con los contadores del
-- dia @Fecha (por estado), de la semana (lunes a domingo) y del mes en que
-- cae @Fecha. @MedicoId NULL = todos los medicos.
-- Estado: 1=Confirmado, 2=Cancelado, 3=Asistio, 4=Ausente.
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

            // sp_ReportePacientesPorMes
            migrationBuilder.Sql("""
CREATE OR ALTER PROCEDURE sp_ReportePacientesPorMes
    @Anio INT,
    @MedicoId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- EstadoTurno enum en C#:
    -- 1: Confirmado, 2: Cancelado, 3: Asistio, 4: Ausente

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
        COUNT(CASE WHEN t.Estado = 1 THEN 1 END) AS Confirmados
    FROM Turnos t
    WHERE YEAR(t.Fecha) = @Anio
      AND (@MedicoId IS NULL OR t.MedicoId = @MedicoId)
    GROUP BY MONTH(t.Fecha)
    ORDER BY MesNumero;
END;
""");

            // sp_ReporteTurnosPorPeriodo
            migrationBuilder.Sql("""
-- Reporte de turnos de un periodo (una fila por turno).
-- RNF#07: este reporte NUNCA devuelve datos clinicos (MotivoConsulta,
-- RecetaMedicamentos, Procedimiento), por eso es seguro para Recepcionista.
-- Estado: 1=Confirmado, 2=Cancelado, 3=Asistio, 4=Ausente.
CREATE OR ALTER PROCEDURE sp_ReporteTurnosPorPeriodo
    @FechaDesde DATE,
    @FechaHasta DATE,
    @MedicoId INT = NULL,
    @Estado INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @FechaDesde > @FechaHasta
    BEGIN
        RAISERROR(N'La fecha desde no puede ser posterior a la fecha hasta.', 16, 1);
        RETURN;
    END

    SELECT
        t.Id AS TurnoId,
        t.Fecha,
        t.HoraInicio,
        t.DuracionMinutos,
        p.Apellido + N', ' + p.Nombre AS Paciente,
        p.Dni,
        m.Apellido + N', ' + m.Nombre AS Medico,
        CASE t.Estado
            WHEN 1 THEN N'Confirmado'
            WHEN 2 THEN N'Cancelado'
            WHEN 3 THEN N'Asistió'
            WHEN 4 THEN N'Ausente'
            ELSE N'Desconocido'
        END AS EstadoNombre,
        t.Monto,
        t.MedioPago
    FROM Turnos t
    INNER JOIN Pacientes p ON p.Id = t.PacienteId
    INNER JOIN Medicos m ON m.Id = t.MedicoId
    WHERE t.Fecha BETWEEN @FechaDesde AND @FechaHasta
      AND (@MedicoId IS NULL OR t.MedicoId = @MedicoId)
      AND (@Estado IS NULL OR t.Estado = @Estado)
    ORDER BY t.Fecha, t.HoraInicio, m.Apellido;
END;
""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // El cambio de datos (Estado 0 -> 1) NO es reversible: no se sabe cuales turnos
            // eran Pendiente. Down solo restaura los textos anteriores de los procedimientos.

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

            // sp_ReporteTurnosPorPeriodo
            migrationBuilder.Sql("""
-- Reporte de turnos de un periodo (una fila por turno).
-- RNF#07: este reporte NUNCA devuelve datos clinicos (MotivoConsulta,
-- RecetaMedicamentos, Procedimiento), por eso es seguro para Recepcionista.
-- Estado: 0=Pendiente, 1=Confirmado, 2=Cancelado, 3=Asistio, 4=Ausente.
CREATE OR ALTER PROCEDURE sp_ReporteTurnosPorPeriodo
    @FechaDesde DATE,
    @FechaHasta DATE,
    @MedicoId INT = NULL,
    @Estado INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @FechaDesde > @FechaHasta
    BEGIN
        RAISERROR(N'La fecha desde no puede ser posterior a la fecha hasta.', 16, 1);
        RETURN;
    END

    SELECT
        t.Id AS TurnoId,
        t.Fecha,
        t.HoraInicio,
        t.DuracionMinutos,
        p.Apellido + N', ' + p.Nombre AS Paciente,
        p.Dni,
        m.Apellido + N', ' + m.Nombre AS Medico,
        CASE t.Estado
            WHEN 0 THEN N'Pendiente'
            WHEN 1 THEN N'Confirmado'
            WHEN 2 THEN N'Cancelado'
            WHEN 3 THEN N'Asistió'
            WHEN 4 THEN N'Ausente'
            ELSE N'Desconocido'
        END AS EstadoNombre,
        t.Monto,
        t.MedioPago
    FROM Turnos t
    INNER JOIN Pacientes p ON p.Id = t.PacienteId
    INNER JOIN Medicos m ON m.Id = t.MedicoId
    WHERE t.Fecha BETWEEN @FechaDesde AND @FechaHasta
      AND (@MedicoId IS NULL OR t.MedicoId = @MedicoId)
      AND (@Estado IS NULL OR t.Estado = @Estado)
    ORDER BY t.Fecha, t.HoraInicio, m.Apellido;
END;
""");
        }
    }
}
