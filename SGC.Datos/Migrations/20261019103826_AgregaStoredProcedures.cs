using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGC.Datos.Migrations
{
    // El SQL de cada procedimiento esta copiado de SGC.Datos/StoredProcedures/*.sql
    // (fuente legible). Si se modifica un .sql, hay que actualizar tambien el
    // texto de esta migracion (o crear una migracion nueva con CREATE OR ALTER).
    /// <inheritdoc />
    public partial class AgregaStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // sp_AsignarTurno
            migrationBuilder.Sql("""
-- Asigna un turno validando fecha, agenda del medico, excepciones y
-- superposicion. Inserta con Estado = 1 (Confirmado), igual que TurnoService.
-- El dia de la semana se calcula con DATEDIFF desde 1900-01-07 (domingo) para
-- no depender de @@DATEFIRST: da 0=Domingo..6=Sabado, igual que DayOfWeek de
-- C# (que es lo que se guarda en AgendasMedico.DiaSemana).
CREATE OR ALTER PROCEDURE sp_AsignarTurno
    @PacienteId INT,
    @MedicoId INT,
    @Fecha DATE,
    @HoraInicio TIME,
    @DuracionMinutos INT = 30
AS
BEGIN
    SET NOCOUNT ON;

    IF @DuracionMinutos IS NULL OR @DuracionMinutos <= 0
        SET @DuracionMinutos = 30;

    DECLARE @HoraFin TIME = DATEADD(MINUTE, @DuracionMinutos, @HoraInicio);
    DECLARE @DiaSemana INT = ((DATEDIFF(DAY, '19000107', @Fecha) % 7) + 7) % 7;

    -- Validar que la fecha no sea pasada
    IF @Fecha < CAST(GETDATE() AS DATE)
    BEGIN
        RAISERROR(N'No se puede asignar un turno en una fecha pasada.', 16, 1);
        RETURN;
    END

    -- Verificar disponibilidad del médico (existe franja de agenda activa que contenga el turno)
    IF NOT EXISTS (
        SELECT 1 FROM AgendasMedico a
        WHERE a.MedicoId = @MedicoId
          AND a.Activo = 1
          AND a.DiaSemana = @DiaSemana
          AND a.HoraInicio <= @HoraInicio
          AND a.HoraFin >= @HoraFin
    )
    BEGIN
        RAISERROR(N'El profesional no atiende en ese horario.', 16, 1);
        RETURN;
    END

    -- Verificar excepciones de agenda (ausencias): dia completo o rango que se superponga
    IF EXISTS (
        SELECT 1 FROM ExcepcionesAgenda e
        WHERE e.MedicoId = @MedicoId
          AND e.Activo = 1
          AND e.Fecha = @Fecha
          AND (e.Tipo = 0 OR (e.HoraInicio < @HoraFin AND e.HoraFin > @HoraInicio))
    )
    BEGIN
        RAISERROR(N'El profesional tiene una ausencia cargada en esa fecha y horario.', 16, 1);
        RETURN;
    END

    -- Verificar superposición de turnos existentes
    IF EXISTS (
        SELECT 1 FROM Turnos t
        WHERE t.MedicoId = @MedicoId
          AND t.Fecha = @Fecha
          AND t.Activo = 1
          AND (t.HoraInicio < @HoraFin AND DATEADD(MINUTE, t.DuracionMinutos, t.HoraInicio) > @HoraInicio)
    )
    BEGIN
        RAISERROR(N'Existe un turno superpuesto para el médico.', 16, 1);
        RETURN;
    END

    INSERT INTO Turnos (PacienteId, MedicoId, Fecha, HoraInicio, DuracionMinutos, Estado, Activo)
    VALUES (@PacienteId, @MedicoId, @Fecha, @HoraInicio, @DuracionMinutos, 1, 1); -- Estado 1 = Confirmado
END;
""");

            // sp_ObtenerTurnosPorMedicoYFecha
            migrationBuilder.Sql("""
CREATE OR ALTER PROCEDURE sp_ObtenerTurnosPorMedicoYFecha
    @MedicoId INT,
    @Fecha DATE = NULL,
    @IncluirCancelados BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    SELECT t.*
    FROM Turnos t
    WHERE t.MedicoId = @MedicoId
      AND (@IncluirCancelados = 1 OR t.Activo = 1)
      AND (@Fecha IS NULL OR t.Fecha = @Fecha);
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

            // sp_ReporteAsistencia
            migrationBuilder.Sql("""
-- Asistencia por medico: turnos totales, atendidos (3), ausentes (4) y
-- cancelados (2). PorcentajeAsistencia = Atendidos / (Atendidos + Ausentes).
CREATE OR ALTER PROCEDURE sp_ReporteAsistencia
    @FechaDesde DATE,
    @FechaHasta DATE,
    @MedicoId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @FechaDesde > @FechaHasta
    BEGIN
        RAISERROR(N'La fecha desde no puede ser posterior a la fecha hasta.', 16, 1);
        RETURN;
    END

    SELECT
        m.Apellido + N', ' + m.Nombre AS Medico,
        COUNT(*) AS TotalTurnos,
        SUM(CASE WHEN t.Estado = 3 THEN 1 ELSE 0 END) AS Atendidos,
        SUM(CASE WHEN t.Estado = 4 THEN 1 ELSE 0 END) AS Ausentes,
        SUM(CASE WHEN t.Estado = 2 THEN 1 ELSE 0 END) AS Cancelados,
        CAST(
            CASE
                WHEN SUM(CASE WHEN t.Estado IN (3, 4) THEN 1 ELSE 0 END) = 0 THEN 0
                ELSE 100.0 * SUM(CASE WHEN t.Estado = 3 THEN 1 ELSE 0 END)
                           / SUM(CASE WHEN t.Estado IN (3, 4) THEN 1 ELSE 0 END)
            END AS DECIMAL(5, 2)) AS PorcentajeAsistencia
    FROM Turnos t
    INNER JOIN Medicos m ON m.Id = t.MedicoId
    WHERE t.Fecha BETWEEN @FechaDesde AND @FechaHasta
      AND (@MedicoId IS NULL OR t.MedicoId = @MedicoId)
    GROUP BY m.Id, m.Apellido, m.Nombre
    ORDER BY m.Apellido, m.Nombre;
END;
""");

            // sp_ReporteOcupacionAgenda
            migrationBuilder.Sql("""
-- Ocupacion de agenda por medico en un periodo.
-- HorasConfiguradas: suma, por cada fecha del rango, de las franjas activas de
-- AgendasMedico cuyo DiaSemana coincide (0=Domingo..6=Sabado, igual que
-- DayOfWeek de C#), descontando ExcepcionesAgenda activas (DiaCompleto anula
-- el dia; RangoHorario descuenta el solapamiento con la franja).
-- HorasOcupadas: turnos activos y no cancelados.
-- El dia de la semana se calcula con DATEDIFF desde 1900-01-07 (domingo) para
-- no depender de @@DATEFIRST.
CREATE OR ALTER PROCEDURE sp_ReporteOcupacionAgenda
    @FechaDesde DATE,
    @FechaHasta DATE,
    @MedicoId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @FechaDesde > @FechaHasta
    BEGIN
        RAISERROR(N'La fecha desde no puede ser posterior a la fecha hasta.', 16, 1);
        RETURN;
    END

    IF DATEDIFF(DAY, @FechaDesde, @FechaHasta) > 365
    BEGIN
        RAISERROR(N'El rango de fechas no puede superar los 366 días.', 16, 1);
        RETURN;
    END

    ;WITH Fechas AS (
        SELECT @FechaDesde AS Fecha
        UNION ALL
        SELECT DATEADD(DAY, 1, Fecha) FROM Fechas WHERE Fecha < @FechaHasta
    ),
    -- Una fila por (fecha, franja de agenda); marca si el dia completo esta bloqueado.
    FranjasBase AS (
        SELECT
            a.Id AS AgendaId,
            a.MedicoId,
            f.Fecha,
            a.HoraInicio,
            a.HoraFin,
            CASE WHEN EXISTS (SELECT 1 FROM ExcepcionesAgenda e
                              WHERE e.MedicoId = a.MedicoId AND e.Activo = 1
                                AND e.Fecha = f.Fecha AND e.Tipo = 0)
                 THEN 1 ELSE 0 END AS DiaBloqueado
        FROM Fechas f
        INNER JOIN AgendasMedico a
            ON a.Activo = 1
           AND a.DiaSemana = ((DATEDIFF(DAY, '19000107', f.Fecha) % 7) + 7) % 7
        WHERE (@MedicoId IS NULL OR a.MedicoId = @MedicoId)
    ),
    -- Solapamiento fila a fila entre cada excepcion de rango horario y su franja.
    -- Se calcula sin agregar: un SUM con varias columnas externas daria el error 8124.
    Solapamientos AS (
        SELECT
            b.AgendaId,
            b.Fecha,
            CASE
                WHEN CASE WHEN e.HoraFin < b.HoraFin THEN e.HoraFin ELSE b.HoraFin END
                   > CASE WHEN e.HoraInicio > b.HoraInicio THEN e.HoraInicio ELSE b.HoraInicio END
                THEN DATEDIFF(MINUTE,
                        CASE WHEN e.HoraInicio > b.HoraInicio THEN e.HoraInicio ELSE b.HoraInicio END,
                        CASE WHEN e.HoraFin < b.HoraFin THEN e.HoraFin ELSE b.HoraFin END)
                ELSE 0
            END AS Minutos
        FROM FranjasBase b
        INNER JOIN ExcepcionesAgenda e
            ON e.MedicoId = b.MedicoId AND e.Activo = 1
           AND e.Fecha = b.Fecha AND e.Tipo = 1
        WHERE b.DiaBloqueado = 0
    ),
    Descuentos AS (
        SELECT AgendaId, Fecha, SUM(Minutos) AS MinutosDescontados
        FROM Solapamientos
        GROUP BY AgendaId, Fecha
    ),
    Franjas AS (
        SELECT
            b.MedicoId,
            CAST(
                CASE
                    WHEN b.DiaBloqueado = 1 THEN 0.0
                    ELSE DATEDIFF(MINUTE, b.HoraInicio, b.HoraFin) - ISNULL(d.MinutosDescontados, 0)
                END AS DECIMAL(10, 2)) AS Minutos
        FROM FranjasBase b
        LEFT JOIN Descuentos d ON d.AgendaId = b.AgendaId AND d.Fecha = b.Fecha
    ),
    Configuradas AS (
        SELECT MedicoId, SUM(CASE WHEN Minutos < 0 THEN 0 ELSE Minutos END) / 60.0 AS HorasConfiguradas
        FROM Franjas
        GROUP BY MedicoId
    ),
    Ocupadas AS (
        SELECT t.MedicoId, SUM(t.DuracionMinutos) / 60.0 AS HorasOcupadas
        FROM Turnos t
        WHERE t.Fecha BETWEEN @FechaDesde AND @FechaHasta
          AND t.Activo = 1
          AND t.Estado <> 2
          AND (@MedicoId IS NULL OR t.MedicoId = @MedicoId)
        GROUP BY t.MedicoId
    )
    SELECT
        m.Apellido + N', ' + m.Nombre AS Medico,
        CAST(ISNULL(c.HorasConfiguradas, 0) AS DECIMAL(10, 2)) AS HorasConfiguradas,
        CAST(ISNULL(o.HorasOcupadas, 0) AS DECIMAL(10, 2)) AS HorasOcupadas,
        CAST(
            CASE
                WHEN ISNULL(c.HorasConfiguradas, 0) = 0 THEN 0
                ELSE 100.0 * ISNULL(o.HorasOcupadas, 0) / c.HorasConfiguradas
            END AS DECIMAL(7, 2)) AS PorcentajeOcupacion
    FROM Medicos m
    LEFT JOIN Configuradas c ON c.MedicoId = m.Id
    LEFT JOIN Ocupadas o ON o.MedicoId = m.Id
    WHERE (@MedicoId IS NULL OR m.Id = @MedicoId)
      AND (c.MedicoId IS NOT NULL OR o.MedicoId IS NOT NULL)
    ORDER BY m.Apellido, m.Nombre
    OPTION (MAXRECURSION 400);
END;
""");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ReporteOcupacionAgenda;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ReporteAsistencia;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ReporteRecaudacion;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ReporteTurnosPorPeriodo;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ObtenerTurnosPorMedicoYFecha;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_AsignarTurno;");
        }
    }
}
