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
