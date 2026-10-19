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
