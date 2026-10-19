-- Deshace carga_masiva_demo.sql: borra solo los datos demo (pacientes *@demo.sgc,
-- sus turnos y actividades, y las ausencias marcadas "(demo)").
-- Uso:  sqlcmd -S .\NICO_SERV1 -E -C -d SGC -f 65001 -i descarga_masiva_demo.sql

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRAN;

DELETE FROM ActividadesMedicas
WHERE TurnoId IN (SELECT t.Id FROM Turnos t
                  JOIN Pacientes p ON p.Id = t.PacienteId
                  WHERE p.Email LIKE '%@demo.sgc');

DELETE FROM Turnos
WHERE PacienteId IN (SELECT Id FROM Pacientes WHERE Email LIKE '%@demo.sgc');

DELETE FROM Pacientes WHERE Email LIKE '%@demo.sgc';

DELETE FROM ExcepcionesAgenda WHERE Motivo LIKE N'%(demo)%';

COMMIT TRAN;

PRINT 'Datos demo eliminados.';
