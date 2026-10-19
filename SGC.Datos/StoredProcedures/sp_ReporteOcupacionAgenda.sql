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
