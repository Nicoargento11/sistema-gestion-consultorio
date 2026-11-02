using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGC.Datos.Migrations
{
    // Agrega el parametro opcional @MedicoId al final de sp_ReporteRecaudacion,
    // sp_ReporteTurnosPorObraSocial y sp_ReporteRendimientoMedicos, para poder
    // acotar esos reportes a un medico (rol Medico) o a los medicos asignados
    // (rol Recepcionista). El SQL esta copiado de SGC.Datos/StoredProcedures/*.sql.
    // Down restaura las versiones anteriores con CREATE OR ALTER (nunca DROP:
    // los procedimientos los crean migraciones previas).
    /// <inheritdoc />
    public partial class AgregaMedicoIdAReportes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // sp_ReporteRecaudacion
            migrationBuilder.Sql("""
-- Recaudacion de un periodo agrupada por MEDICO, MEDIO_PAGO u OBRA_SOCIAL.
-- Solo cuentan turnos con Estado = 3 (Asistio) y Monto informado.
-- Pacientes sin obra social se agrupan como 'Particular'.
CREATE OR ALTER PROCEDURE sp_ReporteRecaudacion
    @FechaDesde DATE,
    @FechaHasta DATE,
    @AgruparPor NVARCHAR(20),
    @MedicoId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @FechaDesde > @FechaHasta
    BEGIN
        RAISERROR(N'La fecha desde no puede ser posterior a la fecha hasta.', 16, 1);
        RETURN;
    END

    IF @AgruparPor IS NULL OR @AgruparPor NOT IN (N'MEDICO', N'MEDIO_PAGO', N'OBRA_SOCIAL')
    BEGIN
        RAISERROR(N'El criterio de agrupación debe ser MEDICO, MEDIO_PAGO u OBRA_SOCIAL.', 16, 1);
        RETURN;
    END

    SELECT
        CAST(
            CASE @AgruparPor
                WHEN N'MEDICO' THEN m.Apellido + N', ' + m.Nombre
                WHEN N'MEDIO_PAGO' THEN ISNULL(t.MedioPago, N'Sin especificar')
                ELSE ISNULL(os.Nombre, N'Particular')
            END AS NVARCHAR(200)) AS Grupo,
        COUNT(*) AS CantidadTurnos,
        CAST(SUM(t.Monto) AS DECIMAL(18, 2)) AS MontoTotal
    FROM Turnos t
    INNER JOIN Medicos m ON m.Id = t.MedicoId
    INNER JOIN Pacientes p ON p.Id = t.PacienteId
    LEFT JOIN ObrasSociales os ON os.Id = p.ObraSocialId
    WHERE t.Fecha BETWEEN @FechaDesde AND @FechaHasta
      AND t.Estado = 3
      AND t.Monto IS NOT NULL
      AND (@MedicoId IS NULL OR t.MedicoId = @MedicoId)
    GROUP BY
        CASE @AgruparPor
            WHEN N'MEDICO' THEN m.Apellido + N', ' + m.Nombre
            WHEN N'MEDIO_PAGO' THEN ISNULL(t.MedioPago, N'Sin especificar')
            ELSE ISNULL(os.Nombre, N'Particular')
        END
    ORDER BY MontoTotal DESC;
END;
""");

            // sp_ReporteTurnosPorObraSocial
            migrationBuilder.Sql("""
CREATE OR ALTER PROCEDURE sp_ReporteTurnosPorObraSocial
    @Anio INT,
    @MedicoId INT = NULL
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
      AND (@MedicoId IS NULL OR t.MedicoId = @MedicoId)
    GROUP BY os.Nombre
    ORDER BY CantidadTurnos DESC;
END;
""");

            // sp_ReporteRendimientoMedicos
            migrationBuilder.Sql("""
CREATE OR ALTER PROCEDURE sp_ReporteRendimientoMedicos
    @Anio INT,
    @Mes INT = NULL,
    @MedicoId INT = NULL
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
      AND (@MedicoId IS NULL OR m.Id = @MedicoId)
    GROUP BY m.Id, m.Apellido, m.Nombre, m.Especialidad
    ORDER BY TurnosAtendidos DESC, TotalTurnos DESC;
END;
""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // sp_ReporteRecaudacion
            migrationBuilder.Sql("""
-- Recaudacion de un periodo agrupada por MEDICO, MEDIO_PAGO u OBRA_SOCIAL.
-- Solo cuentan turnos con Estado = 3 (Asistio) y Monto informado.
-- Pacientes sin obra social se agrupan como 'Particular'.
CREATE OR ALTER PROCEDURE sp_ReporteRecaudacion
    @FechaDesde DATE,
    @FechaHasta DATE,
    @AgruparPor NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    IF @FechaDesde > @FechaHasta
    BEGIN
        RAISERROR(N'La fecha desde no puede ser posterior a la fecha hasta.', 16, 1);
        RETURN;
    END

    IF @AgruparPor IS NULL OR @AgruparPor NOT IN (N'MEDICO', N'MEDIO_PAGO', N'OBRA_SOCIAL')
    BEGIN
        RAISERROR(N'El criterio de agrupación debe ser MEDICO, MEDIO_PAGO u OBRA_SOCIAL.', 16, 1);
        RETURN;
    END

    SELECT
        CAST(
            CASE @AgruparPor
                WHEN N'MEDICO' THEN m.Apellido + N', ' + m.Nombre
                WHEN N'MEDIO_PAGO' THEN ISNULL(t.MedioPago, N'Sin especificar')
                ELSE ISNULL(os.Nombre, N'Particular')
            END AS NVARCHAR(200)) AS Grupo,
        COUNT(*) AS CantidadTurnos,
        CAST(SUM(t.Monto) AS DECIMAL(18, 2)) AS MontoTotal
    FROM Turnos t
    INNER JOIN Medicos m ON m.Id = t.MedicoId
    INNER JOIN Pacientes p ON p.Id = t.PacienteId
    LEFT JOIN ObrasSociales os ON os.Id = p.ObraSocialId
    WHERE t.Fecha BETWEEN @FechaDesde AND @FechaHasta
      AND t.Estado = 3
      AND t.Monto IS NOT NULL
    GROUP BY
        CASE @AgruparPor
            WHEN N'MEDICO' THEN m.Apellido + N', ' + m.Nombre
            WHEN N'MEDIO_PAGO' THEN ISNULL(t.MedioPago, N'Sin especificar')
            ELSE ISNULL(os.Nombre, N'Particular')
        END
    ORDER BY MontoTotal DESC;
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
        }
    }
}
