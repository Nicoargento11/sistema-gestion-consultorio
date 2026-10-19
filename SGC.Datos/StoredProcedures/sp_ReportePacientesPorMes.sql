CREATE PROCEDURE sp_ReportePacientesPorMes
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
