CREATE PROCEDURE sp_AsignarTurno
    @PacienteId INT,
    @MedicoId INT,
    @Fecha DATE,
    @HoraInicio TIME,
    @DuracionMinutos INT = 30
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @HoraFin TIME = DATEADD(MINUTE, @DuracionMinutos, @HoraInicio);

    -- Validar que la fecha no sea pasada
    IF @Fecha < CAST(GETDATE() AS DATE)
    BEGIN
        RAISERROR('No se puede asignar un turno en una fecha pasada.', 16, 1);
        RETURN;
    END

    -- Verificar disponibilidad del médico (simplificado: existe agenda sin conflicto)
    IF NOT EXISTS (
        SELECT 1 FROM AgendasMedico a
        WHERE a.MedicoId = @MedicoId
          AND a.DiaSemana = DATEPART(WEEKDAY, @Fecha)
          AND a.HoraInicio <= @HoraInicio
          AND a.HoraFin >= @HoraFin
    )
    BEGIN
        RAISERROR('El profesional no atiende en ese horario.', 16, 1);
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
        RAISERROR('Existe un turno superpuesto para el médico.', 16, 1);
        RETURN;
    END

    INSERT INTO Turnos (PacienteId, MedicoId, Fecha, HoraInicio, DuracionMinutos, Estado, Activo)
    VALUES (@PacienteId, @MedicoId, @Fecha, @HoraInicio, @DuracionMinutos, 1, 1); -- Estado 1 = Confirmado
END;
