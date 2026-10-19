CREATE PROCEDURE sp_ReporteTurnosPorObraSocial
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
