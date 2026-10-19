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
