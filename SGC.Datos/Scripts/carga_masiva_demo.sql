-- Carga masiva de datos de demostracion (NO es una migracion: se corre a mano).
-- Uso:  sqlcmd -S .\NICO_SERV1 -E -C -d SGC -f 65001 -i carga_masiva_demo.sql
-- Deshacer: descarga_masiva_demo.sql (borra solo lo que crea este script).
--
-- Que genera (relativo a la fecha de hoy):
--   * 24 pacientes demo (Email *@demo.sgc), con y sin obra social.
--   * Turnos de los medicos 1, 2 y 3 sobre sus franjas de AgendasMedico, desde 147 dias
--     atras hasta 25 dias adelante, con mas ocupacion en los meses recientes.
--       - Pasados: Asistio (~68%), Ausente (~14%), Cancelado (~18%). Ayer queda algo
--         Confirmado para probar "Confirmar asistencia".
--       - Futuros: Confirmado, algunos Pendiente y algunos Cancelado.
--   * Monto y medio de pago de los Asistio segun la regla de cobro real: obra social
--     si el medico la acepta (precio - cobertura), si no Particular.
--   * Actividad medica (historial clinico) para ~75% de los Asistio.
--   * 4 excepciones de agenda (ausencias) marcadas con "(demo)" en el motivo.
-- No pisa turnos existentes ni cae en dias/horas con ausencia.

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF EXISTS (SELECT 1 FROM Pacientes WHERE Email LIKE '%@demo.sgc')
BEGIN
    PRINT 'La carga demo ya existe. Corre descarga_masiva_demo.sql antes de volver a cargarla.';
    RETURN;
END

DECLARE @Hoy DATE = CAST(GETDATE() AS DATE);
DECLARE @Ini DATE = DATEADD(DAY, -147, @Hoy);
DECLARE @Fin DATE = DATEADD(DAY, 25, @Hoy);
DECLARE @Rango INT = DATEDIFF(DAY, @Ini, @Fin);

BEGIN TRAN;

-- 1) Ausencias de agenda (se insertan primero: los turnos las respetan).
INSERT INTO ExcepcionesAgenda (MedicoId, Fecha, Tipo, HoraInicio, HoraFin, Motivo, Activo) VALUES
    (1, DATEADD(DAY, -60, @Hoy), 0, NULL, NULL, N'Congreso médico (demo)', 1),
    (2, DATEADD(DAY, -40, @Hoy), 0, NULL, NULL, N'Licencia por capacitación (demo)', 1),
    (1, DATEADD(DAY,   9, @Hoy), 1, '09:00', '11:00', N'Trámite personal (demo)', 1),
    (3, DATEADD(DAY,  14, @Hoy), 0, NULL, NULL, N'Vacaciones (demo)', 1);

-- 2) Pacientes demo.
;WITH P (n, nombre, apellido, os) AS (
    SELECT * FROM (VALUES
        (1,  N'Sofía',     N'Ramírez',   1),    (2,  N'Mateo',    N'González',  2),
        (3,  N'Valentina', N'López',     3),    (4,  N'Santiago', N'Rodríguez', NULL),
        (5,  N'Camila',    N'Sánchez',   2),    (6,  N'Joaquín',  N'Díaz',      1),
        (7,  N'Martina',   N'Romero',    NULL), (8,  N'Lucas',    N'Álvarez',   3),
        (9,  N'Julieta',   N'Torres',    2),    (10, N'Tomás',    N'Benítez',   1),
        (11, N'Lucía',     N'Herrera',   NULL), (12, N'Facundo',  N'Medina',    2),
        (13, N'Agustina',  N'Castro',    3),    (14, N'Nicolás',  N'Ruiz',      4),
        (15, N'Florencia', N'Giménez',   1),    (16, N'Bruno',    N'Acosta',    NULL),
        (17, N'Micaela',   N'Silva',     2),    (18, N'Franco',   N'Ortiz',     3),
        (19, N'Carolina',  N'Peralta',   1),    (20, N'Ezequiel', N'Vega',      NULL),
        (21, N'Paula',     N'Domínguez', 2),    (22, N'Gonzalo',  N'Ríos',      3),
        (23, N'Daniela',   N'Moreno',    1),    (24, N'Ignacio',  N'Cabrera',   NULL)
    ) v (n, nombre, apellido, os)
)
INSERT INTO Pacientes (Dni, Nombre, Apellido, Email, Telefono, FechaNacimiento, Activo, ObraSocialId)
SELECT CAST(25000000 + n * 977777 AS NVARCHAR(30)),
       nombre,
       apellido,
       N'paciente.demo' + RIGHT('0' + CAST(n AS NVARCHAR(2)), 2) + N'@demo.sgc',
       N'3794' + RIGHT('000000' + CAST(100000 + n * 37123 AS NVARCHAR(10)), 6),
       DATEADD(DAY, -(7000 + n * 613), @Hoy),
       1,
       os
FROM P;

SELECT Id, ROW_NUMBER() OVER (ORDER BY Id) AS rn, ObraSocialId
INTO #Pac
FROM Pacientes
WHERE Email LIKE '%@demo.sgc';

-- 3) Slots candidatos: franjas activas de los medicos 1, 2 y 3, de a 30 minutos.
SELECT a.MedicoId,
       d.Fecha,
       DATEADD(MINUTE, s.k * 30, a.HoraInicio) AS HoraInicio
INTO #Slots
FROM AgendasMedico a
CROSS JOIN (SELECT DATEADD(DAY, number, @Ini) AS Fecha
            FROM master..spt_values
            WHERE type = 'P' AND number <= DATEDIFF(DAY, @Ini, @Fin)) d
CROSS JOIN (SELECT number AS k FROM master..spt_values WHERE type = 'P' AND number < 20) s
WHERE a.Activo = 1
  AND a.MedicoId IN (1, 2, 3)
  AND a.DiaSemana = ((DATEDIFF(DAY, '19000107', d.Fecha) % 7) + 7) % 7
  AND DATEADD(MINUTE, (s.k + 1) * 30, a.HoraInicio) <= a.HoraFin;

-- Fuera los slots con ausencia vigente o que pisan un turno activo existente.
DELETE sl
FROM #Slots sl
WHERE EXISTS (SELECT 1 FROM ExcepcionesAgenda e
              WHERE e.Activo = 1 AND e.MedicoId = sl.MedicoId AND e.Fecha = sl.Fecha
                AND (e.Tipo = 0
                     OR (e.HoraInicio < DATEADD(MINUTE, 30, sl.HoraInicio) AND e.HoraFin > sl.HoraInicio)))
   OR EXISTS (SELECT 1 FROM Turnos t
              WHERE t.Activo = 1 AND t.MedicoId = sl.MedicoId AND t.Fecha = sl.Fecha
                AND t.HoraInicio < DATEADD(MINUTE, 30, sl.HoraInicio)
                AND DATEADD(MINUTE, t.DuracionMinutos, t.HoraInicio) > sl.HoraInicio);

-- 4) Seleccion aleatoria con mas ocupacion hacia el presente. Se usa NEWID() porque
-- CHECKSUM sobre entradas parecidas reparte mal (casi todo caia en el mismo estado).
SELECT MedicoId, Fecha, HoraInicio, hPac, hEst
INTO #Elegidos
FROM (SELECT sl.MedicoId, sl.Fecha, sl.HoraInicio,
             ABS(CAST(CHECKSUM(NEWID()) AS BIGINT)) % 100 AS hSel,
             ABS(CAST(CHECKSUM(NEWID()) AS BIGINT)) % 24  AS hPac,
             ABS(CAST(CHECKSUM(NEWID()) AS BIGINT)) % 100 AS hEst
      FROM #Slots sl) r
WHERE hSel < 35 + 35 * DATEDIFF(DAY, @Ini, Fecha) / @Rango;

-- 5) Turnos.
INSERT INTO Turnos (PacienteId, MedicoId, Fecha, HoraInicio, DuracionMinutos, Estado, MedioPago, Monto, Activo)
SELECT p.Id,
       e.MedicoId,
       e.Fecha,
       e.HoraInicio,
       30,
       x.Estado,
       CASE WHEN x.Estado = 3 THEN CASE WHEN acepta.Ok = 1 THEN os.Nombre ELSE N'Particular' END END,
       CASE WHEN x.Estado = 3
            THEN CAST(ROUND(m.PrecioConsultaParticular
                            * CASE WHEN acepta.Ok = 1 THEN 1 - os.PorcentajeCobertura / 100.0 ELSE 1 END, 2) AS DECIMAL(18, 2))
       END,
       CASE WHEN x.Estado = 2 THEN 0 ELSE 1 END
FROM #Elegidos e
JOIN #Pac p ON p.rn = (e.hPac * e.hPac) / 23 + 1
JOIN Medicos m ON m.Id = e.MedicoId
LEFT JOIN ObrasSociales os ON os.Id = p.ObraSocialId
CROSS APPLY (SELECT CASE WHEN os.Id IS NOT NULL
                          AND EXISTS (SELECT 1 FROM MedicoObraSocial mo
                                      WHERE mo.MedicoId = e.MedicoId AND mo.ObrasSocialesAceptadasId = os.Id)
                         THEN 1 ELSE 0 END AS Ok) acepta
CROSS APPLY (SELECT CASE
                 WHEN e.Fecha < DATEADD(DAY, -1, @Hoy)
                      THEN CASE WHEN e.hEst < 68 THEN 3 WHEN e.hEst < 82 THEN 4 ELSE 2 END
                 WHEN e.Fecha = DATEADD(DAY, -1, @Hoy)
                      THEN CASE WHEN e.hEst < 40 THEN 1 WHEN e.hEst < 75 THEN 3 WHEN e.hEst < 90 THEN 4 ELSE 2 END
                 WHEN e.Fecha = @Hoy
                      THEN CASE WHEN e.hEst < 92 THEN 1 ELSE 2 END
                 ELSE CASE WHEN e.hEst < 86 THEN 1 WHEN e.hEst < 94 THEN 0 ELSE 2 END
             END AS Estado) x;

-- 6) Historial clinico para ~75% de los turnos atendidos.
INSERT INTO ActividadesMedicas (TurnoId, TipoActividadId, MotivoConsulta, RecetaMedicamentos, Procedimiento, Activo)
SELECT t.Id,
       h.k % 5 + 1,
       CHOOSE(h.k % 8 + 1, N'Control de presión arterial', N'Dolor lumbar persistente',
              N'Cefalea recurrente', N'Chequeo anual de rutina', N'Tos y congestión de varios días',
              N'Seguimiento de tratamiento', N'Dolor abdominal', N'Renovación de medicación crónica'),
       CHOOSE(h.k % 6 + 1, N'Ibuprofeno 400 mg cada 8 horas por 5 días', N'Enalapril 10 mg cada 24 horas',
              N'Paracetamol 500 mg cada 6 horas si hay fiebre', N'Omeprazol 20 mg en ayunas',
              N'Sin medicación', N'Amoxicilina 500 mg cada 8 horas por 7 días'),
       CHOOSE(h.k % 5 + 1, N'Examen físico general', N'Toma de presión y auscultación',
              N'Indicación de reposo e hidratación', N'Solicitud de análisis de laboratorio',
              N'Derivación a especialista'),
       1
FROM Turnos t
JOIN #Pac p ON p.Id = t.PacienteId
CROSS APPLY (SELECT ABS(CAST(CHECKSUM(NEWID()) AS BIGINT)) AS k) h
WHERE t.Estado = 3
  AND h.k % 100 < 75
  AND NOT EXISTS (SELECT 1 FROM ActividadesMedicas am WHERE am.TurnoId = t.Id);

COMMIT TRAN;

SELECT 'Pacientes demo' AS Cargado, COUNT(*) AS Filas FROM #Pac
UNION ALL SELECT 'Turnos demo', COUNT(*) FROM Turnos WHERE PacienteId IN (SELECT Id FROM #Pac)
UNION ALL SELECT 'Actividades medicas demo', COUNT(*) FROM ActividadesMedicas WHERE TurnoId IN (SELECT Id FROM Turnos WHERE PacienteId IN (SELECT Id FROM #Pac))
UNION ALL SELECT 'Ausencias demo', COUNT(*) FROM ExcepcionesAgenda WHERE Motivo LIKE N'%(demo)%';

DROP TABLE #Pac, #Slots, #Elegidos;
