CREATE PROCEDURE sp_ReporteRendimientoMedicos
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
