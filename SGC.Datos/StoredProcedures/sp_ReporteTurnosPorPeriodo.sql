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
