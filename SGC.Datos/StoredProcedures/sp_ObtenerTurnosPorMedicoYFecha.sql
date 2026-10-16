CREATE PROCEDURE sp_ObtenerTurnosPorMedicoYFecha
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
