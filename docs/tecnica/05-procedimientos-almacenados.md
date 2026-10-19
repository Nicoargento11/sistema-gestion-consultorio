# Procedimientos almacenados

Acá vas a encontrar los 10 procedimientos almacenados (SP) que usa el sistema: qué hace cada uno en lenguaje simple, con qué parámetros se llama, qué columnas devuelve y **quién lo llama** (servicio y pantalla). También cómo llegan a la base (migraciones) y cómo agregar uno nuevo. Sirve para entender de dónde salen los números de los reportes y qué hacer si hay que cambiar uno.

Todo se verificó contra los archivos `.sql`, las migraciones, el código C# y la base real (los 10 procedimientos existen en la base de desarrollo; `sp_ResumenDelDia` y `sp_ReporteAsistencia` se ejecutaron con `sqlcmd` y respondieron).

## Tabla resumen

| Procedimiento | Qué hace (en una línea) | Quién lo llama | Pantalla |
|---------------|--------------------------|----------------|----------|
| `sp_AsignarTurno` | Crea un turno validando fecha, agenda, ausencias y superposición | **Nadie** | **Ninguna** |
| `sp_ObtenerTurnosPorMedicoYFecha` | Lista los turnos de un médico (y opcionalmente de un día) | `TurnoService.ObtenerPorMedicoYFecha` | Turnos, Registrar actividad, Mi agenda (vía `AgendaDiariaService`) |
| `sp_ReporteTurnosPorPeriodo` | Una fila por turno de un período | `ReporteService.ObtenerTurnosPorPeriodo` | Reportes → Turnos |
| `sp_ReporteRecaudacion` | Plata cobrada, agrupada por médico, medio de pago u obra social | `ReporteService.ObtenerRecaudacion` | Reportes → Recaudación (solo Administrador) |
| `sp_ReporteAsistencia` | Atendidos, ausentes y % de asistencia por médico | `ReporteService.ObtenerAsistencia` | Reportes → Asistencia |
| `sp_ReporteOcupacionAgenda` | Horas de agenda vs. horas ocupadas por médico | `ReporteService.ObtenerOcupacionAgenda` | Reportes → Ocupación de agenda |
| `sp_ResumenDelDia` | Contadores del día, semana y mes (una sola fila) | `ResumenService.ObtenerResumen` / `ObtenerResumenDeMedicos` | Inicio |
| `sp_ReportePacientesPorMes` | Turnos por mes de un año | `ReporteService.ObtenerPacientesPorMes` | Reportes → Pacientes por mes |
| `sp_ReporteRendimientoMedicos` | Turnos, atendidos, ausentes y recaudado por médico | `ReporteService.ObtenerRendimientoMedicos` | Reportes → Rendimiento de médicos (Administrador) |
| `sp_ReporteTurnosPorObraSocial` | Turnos y recaudado por obra social del paciente | `ReporteService.ObtenerTurnosPorObraSocial` | Reportes → Obras sociales |

Los llamadores se confirmaron con búsquedas en `SGC.Logica` y `SGC.UI` (`EXEC sp_`, `CommandType.StoredProcedure` y los nombres de método).

> **Dato clave:** `sp_AsignarTurno` está creado en la base pero **no está conectado**: `TurnoService.AsignarTurno` hace sus propias validaciones en C# y nunca lo invoca.

---

## 1. Los procedimientos uno por uno

Convención de los `.sql`: `SET NOCOUNT ON`, parámetros opcionales con valor `NULL` (significa "todos"), y los errores de validación con `RAISERROR(..., 16, 1)` (que en C# llegan como `SqlException`).

### `sp_AsignarTurno` — `SGC.Datos/StoredProcedures/sp_AsignarTurno.sql`

**Qué hace:** inserta un turno `Confirmado` si pasa las validaciones. No devuelve filas.

| Parámetro | Tipo | Nota |
|-----------|------|------|
| `@PacienteId`, `@MedicoId` | INT | |
| `@Fecha` | DATE | |
| `@HoraInicio` | TIME | |
| `@DuracionMinutos` | INT = 30 | Si es `NULL` o `<= 0`, usa 30 |

Validaciones, en orden (todas cortan con `RAISERROR`): fecha no pasada; existe una franja activa de `AgendasMedico` que contiene el turno; **no hay una ausencia** (`ExcepcionesAgenda` activa, día completo o rango superpuesto); no hay otro turno activo superpuesto. Recién ahí hace el `INSERT` con `Estado = 1`.

**Quién lo llama:** nadie. Es la versión en SQL de `TurnoService.AsignarTurno`, con una validación de más (las ausencias). Comparación completa en `03-reglas-de-negocio.md`, sección 11.

### `sp_ObtenerTurnosPorMedicoYFecha` — `.sql` homónimo

**Qué hace:** devuelve los turnos de un médico.

| Parámetro | Tipo | Nota |
|-----------|------|------|
| `@MedicoId` | INT | Obligatorio |
| `@Fecha` | DATE = NULL | `NULL` = todos los días |
| `@IncluirCancelados` | BIT = 0 | Con 0 filtra `Activo = 1` |

**Devuelve:** `SELECT t.*` de `Turnos`, o sea todas las columnas de la tabla (`Id`, `PacienteId`, `MedicoId`, `Fecha`, `HoraInicio`, `DuracionMinutos`, `Estado`, `MedioPago`, `Monto`, `Activo`). Esa forma "tabla entera" es lo que permite que EF la materialice como `Turno`.

**Quién lo llama:** `TurnoService.ObtenerPorMedicoYFecha`, que usan:
- `FormTurnos` (`CrearHorarios`: para marcar horarios ocupados),
- `FormRegistrarActividad` (`CargarComboTurnos`: turnos del día del médico),
- `AgendaDiariaService.ObtenerAgenda` (con cancelados incluidos), que alimenta `FormAgendaMedico`.

### `sp_ReporteTurnosPorPeriodo`

**Qué hace:** una fila por turno entre dos fechas, sin datos clínicos (RNF#07, por eso es seguro para el rol Recepcionista).

| Parámetro | Tipo | Nota |
|-----------|------|------|
| `@FechaDesde`, `@FechaHasta` | DATE | Si `desde > hasta` → `RAISERROR` |
| `@MedicoId` | INT = NULL | |
| `@Estado` | INT = NULL | Número de `EstadoTurno` |

**Devuelve:** `TurnoId`, `Fecha`, `HoraInicio`, `DuracionMinutos`, `Paciente` (`Apellido, Nombre`), `Dni`, `Medico`, `EstadoNombre` (texto), `Monto`, `MedioPago`. Orden: fecha, hora, apellido del médico.

**Detalle:** no filtra `Activo`, así que incluye los turnos cancelados (que tienen `Activo = 0`). `EstadoNombre` usa "Asistió" **con tilde**; `FormReportes` lo compara por texto (`EstadoNombre == "Asistió"`), así que si se cambia el texto en el SQL hay que cambiarlo también en la pantalla.

**Quién lo llama:** `ReporteService.ObtenerTurnosPorPeriodo`, desde `FormReportes` (`GenerarTurnos`). Para "Todos mis médicos" (Recepcionista) la pantalla lo ejecuta una vez por médico asignado y une los resultados.

### `sp_ReporteRecaudacion`

**Qué hace:** suma lo cobrado en un período, agrupado según `@AgruparPor`.

| Parámetro | Tipo | Nota |
|-----------|------|------|
| `@FechaDesde`, `@FechaHasta` | DATE | |
| `@AgruparPor` | NVARCHAR(20) | `'MEDICO'`, `'MEDIO_PAGO'` u `'OBRA_SOCIAL'`; otro valor → `RAISERROR` |

**Devuelve:** `Grupo`, `CantidadTurnos`, `MontoTotal`, ordenado por monto descendente.

**Reglas:** solo cuenta turnos con `Estado = 3` (Asistió) y `Monto` informado. Con `OBRA_SOCIAL` agrupa por la obra social **actual del paciente** y los pacientes sin obra social van a "Particular". En `MEDIO_PAGO`, un valor nulo se muestra como "Sin especificar".

**Quién lo llama:** `ReporteService.ObtenerRecaudacion` (traduce el enum `AgrupacionRecaudacion` al texto) desde la pestaña Recaudación de `FormReportes`, visible solo para el Administrador.

### `sp_ReporteAsistencia`

**Qué hace:** por médico, cuántos turnos hubo en el período y cuántos fueron atendidos, ausentes y cancelados.

| Parámetro | Tipo |
|-----------|------|
| `@FechaDesde`, `@FechaHasta` | DATE |
| `@MedicoId` | INT = NULL |

**Devuelve:** `Medico`, `TotalTurnos`, `Atendidos` (Estado 3), `Ausentes` (4), `Cancelados` (2), `PorcentajeAsistencia`. El porcentaje es `Atendidos / (Atendidos + Ausentes)`: **los cancelados y los turnos sin resolver no entran**; si no hay atendidos ni ausentes, da 0. `TotalTurnos` sí cuenta todos los estados, así que no es igual a la suma de las tres columnas. No filtra `Activo`.

**Quién lo llama:** `ReporteService.ObtenerAsistencia` desde la pestaña Asistencia de `FormReportes` (Administrador y Médico).

### `sp_ReporteOcupacionAgenda`

**Qué hace:** compara las horas que cada médico tiene disponibles en su agenda con las horas que ya tiene ocupadas.

| Parámetro | Tipo | Nota |
|-----------|------|------|
| `@FechaDesde`, `@FechaHasta` | DATE | Máximo 366 días |
| `@MedicoId` | INT = NULL | |

**Devuelve:** `Medico`, `HorasConfiguradas`, `HorasOcupadas`, `PorcentajeOcupacion`.

**Cómo calcula** (es el más complejo, en cadena de CTE):

| CTE | Qué hace |
|-----|----------|
| `Fechas` | Genera **una fila por día** del rango con recursión (`UNION ALL`) |
| `FranjasBase` | Cruza cada fecha con las franjas activas de `AgendasMedico` de ese día de la semana; marca si el día completo está bloqueado por una ausencia |
| `Solapamientos` | Para cada ausencia por rango, minutos que se pisan con la franja |
| `Descuentos` | Suma esos minutos por franja y fecha |
| `Franjas` | Minutos netos de cada franja (0 si el día está bloqueado) |
| `Configuradas` | Total de horas configuradas por médico |
| `Ocupadas` | Horas de turnos **activos y no cancelados** (`Activo = 1`, `Estado <> 2`) |

Cierra con `OPTION (MAXRECURSION 400)` (el default de SQL Server es 100, no alcanza para 366 días). No filtra `Medicos.Activo`; lista a los médicos que tengan agenda configurada o turnos en el período.

**Quién lo llama:** `ReporteService.ObtenerOcupacionAgenda` desde la pestaña Ocupación de agenda de `FormReportes` (Administrador y Médico). El Service valida el límite de 366 días antes de llamar para dar un mensaje claro; el procedimiento lo valida también.

### `sp_ResumenDelDia`

**Qué hace:** devuelve **una sola fila** con los números de la pantalla de Inicio.

| Parámetro | Tipo | Nota |
|-----------|------|------|
| `@Fecha` | DATE | |
| `@MedicoId` | INT = NULL | `NULL` = todos |

**Devuelve:** `Pendientes`, `Confirmados`, `Asistieron`, `Ausentes`, `Cancelados` (los del día `@Fecha`), `TurnosSemana` (semana lunes a domingo, sin cancelados), `AusentesMes`, `RecaudacionMes` (suma de `Monto` de los `Asistio` del mes) y `MedicosConAgendaHoy` (médicos activos con franja ese día de la semana y sin ausencia de día completo).

**Quién lo llama:** `ResumenService.ObtenerResumen` y `ObtenerResumenDeMedicos`, desde `FormInicio`. Como el procedimiento recibe un solo médico, para la Recepcionista el Service lo ejecuta **una vez por médico asignado** y suma los resultados con `ResumenDelDiaFila.Sumar`. Una lista vacía da todo en cero (nunca "todos los médicos").

### `sp_ReportePacientesPorMes`, `sp_ReporteRendimientoMedicos`, `sp_ReporteTurnosPorObraSocial`

Los tres vienen del módulo de reportes de Luciano (ver comentario de la migración `AgregaSpReportesPacientesRendimientoObraSocial`). Se ejecutan con `CommandType.StoredProcedure` y se leen a mano con un `SqlDataReader`.

| Procedimiento | Parámetros | Devuelve | Notas |
|---------------|------------|----------|-------|
| `sp_ReportePacientesPorMes` | `@Anio INT`, `@MedicoId INT = NULL` | `MesNumero`, `NombreMes`, `TotalTurnos`, `PacientesAtendidos`, `Ausentes`, `Cancelados`, `PendientesOConfirmados` | Agrupa por mes; **solo aparecen los meses con turnos** (no rellena los vacíos). Los nombres de mes están escritos en el CASE. No filtra `Activo`. |
| `sp_ReporteRendimientoMedicos` | `@Anio INT`, `@Mes INT = NULL` | `MedicoId`, `MedicoNombre`, `Especialidad`, `TotalTurnos`, `TurnosAtendidos`, `TurnosAusentes`, `TurnosCancelados`, `TotalRecaudado` | `LEFT JOIN` con la condición del año/mes **en el `ON`**: así un médico activo sin turnos igual aparece con ceros. Solo médicos activos. Ordena por atendidos y total. |
| `sp_ReporteTurnosPorObraSocial` | `@Anio INT` | `ObraSocial`, `CantidadTurnos`, `TotalRecaudado` | Agrupa por la obra social **actual** del paciente ("Particular" si no tiene). Filtra `Activo = 1` (excluye cancelados). No recibe médico. |

**Quién los llama:** `ReporteService.ObtenerPacientesPorMes`, `ObtenerRendimientoMedicos` y `ObtenerTurnosPorObraSocial`, todos desde `FormReportes.Estadisticas.cs` (pestañas Pacientes por mes, Rendimiento de médicos —solo Administrador— y Obras sociales). `ObtenerTurnosPorObraSocial` también alimenta el indicador "Facturación anual" del Administrador.

---

## 2. Decisiones técnicas que no son obvias

### 2.1 El día de la semana no depende del idioma

Aparece en `sp_AsignarTurno`, `sp_ReporteOcupacionAgenda` y `sp_ResumenDelDia`:

```sql
DECLARE @DiaSemana INT = ((DATEDIFF(DAY, '19000107', @Fecha) % 7) + 7) % 7;
```

| Parte | Por qué |
|-------|---------|
| `DATEDIFF(DAY, '19000107', @Fecha)` | El 7/1/1900 fue **domingo**. La diferencia de días módulo 7 da 0 para domingo, 1 para lunes... 6 para sábado |
| `% 7` | Resto de la división |
| `+ 7) % 7` | En T-SQL el resto de un número negativo es negativo; así fechas anteriores a 1900 también dan 0 a 6 |

**Por qué no `DATEPART(weekday, ...)`:** esa función depende de `@@DATEFIRST` (qué día se considera el primero), que cambia con el idioma del login de SQL Server. El resultado variaría según la máquina. La fórmula da siempre **0 = domingo ... 6 = sábado**, la misma numeración que `DayOfWeek` de C#, que es lo que se guarda en `AgendasMedico.DiaSemana`. Verificado en la base: `2026-10-19` (lunes) da 1 y `2026-10-18` (domingo) da 0, con `@@DATEFIRST = 7`.

`sp_ResumenDelDia` calcula el inicio de semana de forma análoga, pero desde `'19000101'` (lunes) para que la semana vaya de lunes a domingo.

### 2.2 Por qué `sp_ReporteOcupacionAgenda` usa tantos CTE (error 8124)

El comentario del `.sql` lo explica: calcular el solapamiento de las ausencias dentro de un único `SUM` que mezcla columnas de la consulta externa produce el error de SQL Server **8124** ("Multiple columns are specified in an aggregated expression containing an outer reference"). La solución fue calcular el solapamiento **fila a fila sin agregar** (CTE `Solapamientos`) y recién después sumar (`Descuentos`). Por eso son varios pasos en vez de una consulta grande.

### 2.3 Por qué `ObtenerPorMedicoYFecha` lee y después carga navegaciones

Un `EXEC` a un procedimiento almacenado **no es componible** en EF Core: no se le puede agregar `Include`, `JOIN` ni `Where` del lado del servidor. Entonces:

1. `FromSqlRaw("EXEC sp_ObtenerTurnosPorMedicoYFecha ...")` + `AsEnumerable()` trae los turnos a memoria.
2. Se hace una **segunda consulta** normal (`contexto.Turnos.Where(t => ids.Contains(t.Id)).Include(...).Load()`) para cargar `Paciente`, `Medico` y `ActividadMedica`. Como el mismo contexto ya tiene esos `Turno` rastreados, EF completa las navegaciones sobre los objetos de la lista.

(Esa carga posterior pide `Include(t => t.Medico).ThenInclude(m => m.ObrasSocialesAceptadas)` porque la regla de cobro la necesita; ver `03-reglas-de-negocio.md`.)

También se pasa la fecha como texto `yyyy-MM-dd`; ese formato ISO lo interpreta SQL Server igual para el tipo `DATE` sin importar el idioma (explicación deducida; el código no la documenta).

### 2.4 Tres formas de invocar un procedimiento desde C#

| Forma | Código | Se usa para | Por qué |
|-------|--------|-------------|---------|
| **`FromSqlRaw` sobre un `DbSet`** | `contexto.Turnos.FromSqlRaw("EXEC ...")` | `sp_ObtenerTurnosPorMedicoYFecha` | El resultado tiene la forma exacta de la entidad `Turno` |
| **`Database.SqlQueryRaw<T>`** | `contexto.Database.SqlQueryRaw<ReporteTurnoFila>("EXEC ...", parámetros)` | `sp_ReporteTurnosPorPeriodo`, `sp_ReporteRecaudacion`, `sp_ReporteAsistencia`, `sp_ReporteOcupacionAgenda`, `sp_ResumenDelDia` | Devuelve DTO que **no** son entidades. Los nombres de propiedad deben coincidir con las columnas del procedimiento |
| **`DbCommand` con `CommandType.StoredProcedure`** | `comando.CommandText = "sp_..."; comando.CommandType = CommandType.StoredProcedure; ... ExecuteReader()` | Los 3 procedimientos de Luciano (`ObtenerPacientesPorMes`, `ObtenerRendimientoMedicos`, `ObtenerTurnosPorObraSocial`) | ADO.NET clásico: se arma el `SqlParameter` y se lee cada columna con `reader.GetInt32(reader.GetOrdinal("..."))` |

Las tres funcionan; la diferencia es de estilo: la tercera es más larga porque lee columna por columna en vez de dejar que EF mapee el resultado. Nota: en la tercera forma se hace `using var conexion = contexto.Database.GetDbConnection()`, lo que obliga a abrir la conexión a mano (`if (conexion.State != Open) conexion.Open()`).

**Regla para los parámetros opcionales** (comentario de `ReporteService`): un parámetro `NULL` debe viajar como `DBNull.Value`, **no** como `null` de C#. Por eso existen `ParametroEntero` y `ParametroFecha`, que además fijan el tipo SQL (`Int`, `Date`).

---

## 3. Cómo se despliegan

Cada procedimiento existe en **dos lugares con el mismo texto** (verificado comparando los 10 archivos contra las 3 migraciones, ignorando espacios):

| Lugar | Rol |
|-------|-----|
| `SGC.Datos/StoredProcedures/<nombre>.sql` | Fuente legible. **No se ejecuta**: ningún código lee estos archivos. |
| Una migración de `SGC.Datos/Migrations/` | Copia dentro de `migrationBuilder.Sql("""...""")`. Esto es lo que **realmente se aplica**. |

| Migración | Procedimientos que crea |
|-----------|--------------------------|
| `20261019103826_AgregaStoredProcedures` | `sp_AsignarTurno`, `sp_ObtenerTurnosPorMedicoYFecha`, `sp_ReporteTurnosPorPeriodo`, `sp_ReporteRecaudacion`, `sp_ReporteAsistencia`, `sp_ReporteOcupacionAgenda` |
| `20261019114456_AgregaSpResumenDelDia` | `sp_ResumenDelDia` |
| `20261019153000_AgregaSpReportesPacientesRendimientoObraSocial` | `sp_ReportePacientesPorMes`, `sp_ReporteRendimientoMedicos`, `sp_ReporteTurnosPorObraSocial` |

**Cómo llegan a la base:**

1. Al arrancar, `Program.cs` ejecuta `Database.Migrate()` (ver `01-arquitectura.md`), que aplica las migraciones que falten y las anota en `__EFMigrationsHistory`.
2. Cada `Sql(...)` tiene `CREATE OR ALTER PROCEDURE`: crea el procedimiento si no existe y lo reemplaza si ya existe, así que es seguro volver a correrlo.
3. `Down()` de cada migración hace `DROP PROCEDURE IF EXISTS`, para poder deshacerla.
4. Hay **un `migrationBuilder.Sql` por procedimiento** a propósito: `CREATE PROCEDURE` tiene que ser la primera instrucción de su lote, y no se puede usar `GO` dentro de `Sql()`.

---

## 4. Cómo agregar o cambiar un procedimiento

### Agregar uno nuevo

1. Escribí `SGC.Datos/StoredProcedures/sp_MiProcedimiento.sql` con `CREATE OR ALTER PROCEDURE`, `SET NOCOUNT ON` y parámetros opcionales en `NULL`.
2. Creá una migración vacía desde la carpeta del `.sln`: `dotnet ef migrations add AgregaSpMiProcedimiento --project SGC.Datos` (usa `SGCContextDesignTimeFactory`, y encuentra la cadena de conexión como se explicó en `01-arquitectura.md`). *No se ejecutó para este documento.*
3. En el método `Up` de la migración nueva, pegá el SQL dentro de `migrationBuilder.Sql("""...""")`. En `Down`, `migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_MiProcedimiento;");`.
4. Si devuelve filas: agregá el DTO en `SGC.Entidades/Reportes/` (propiedades con **el mismo nombre** que las columnas).
5. Agregá el método en el Service que corresponda (`ReporteService` o `ResumenService`), con `SqlQueryRaw<T>` y los parámetros con `ParametroEntero`/`ParametroFecha`.
6. Ejecutá la app (o `dotnet ef database update`): `Migrate()` la aplica.

### Cambiar uno existente

**No edites la migración vieja** (ya está aplicada en otras bases y no se vuelve a ejecutar). Hacé lo mismo que arriba: editá el `.sql` y creá una **migración nueva** con el `CREATE OR ALTER PROCEDURE` modificado. Los comentarios de las migraciones lo piden expresamente: "si se modifica el .sql, hay que crear una migración nueva con CREATE OR ALTER".

### Checklist

- [ ] El `.sql` y el SQL de la migración nueva tienen el mismo texto.
- [ ] Las columnas del procedimiento coinciden con las propiedades del DTO.
- [ ] Los números de estado (`0` a `4`) coinciden con `EstadoTurno` (ver `02-modelo-de-datos.md`).
- [ ] Si el cambio afecta una pantalla, probada la grilla/gráfico que lo usa.

---

## Hallazgos

Hechos observados, sin juicio.

| # | Hallazgo | Dónde |
|---|----------|-------|
| 1 | **`sp_AsignarTurno` no está conectado a nada.** Ningún Service ni pantalla lo invoca; `TurnoService.AsignarTurno` reimplementa la validación en C#, sin la verificación de ausencias que el procedimiento sí hace. | `SGC.Logica/TurnoService.cs`, `SGC.Datos/StoredProcedures/sp_AsignarTurno.sql` |
| 2 | **Los reportes no filtran los turnos cancelados con el mismo criterio.** `sp_ReporteTurnosPorPeriodo`, `sp_ReporteAsistencia`, `sp_ReportePacientesPorMes` y `sp_ReporteRendimientoMedicos` no miran `Activo` (cuentan los cancelados, que están en `Activo = 0`); `sp_ReporteOcupacionAgenda` excluye cancelados y exige `Activo = 1`; `sp_ReporteTurnosPorObraSocial` solo filtra `Activo = 1` (excluye cancelados) y `sp_ReporteRecaudacion` solo cuenta `Estado = 3`. Por eso "turnos" del reporte por obra social puede no coincidir con "turnos" del reporte por período. | `SGC.Datos/StoredProcedures/*.sql` |
| 3 | **Dos reportes agrupan por la obra social actual del paciente, no por lo que se cobró.** Un turno cobrado como Particular (porque el médico no acepta la obra social del paciente) aparece en `sp_ReporteRecaudacion` y `sp_ReporteTurnosPorObraSocial` bajo la obra social del paciente. Lo que sí queda registrado como cobrado es `Turnos.MedioPago`. | `sp_ReporteRecaudacion.sql`, `sp_ReporteTurnosPorObraSocial.sql` |
| 4 | **Texto acoplado entre SQL y pantalla:** `sp_ReporteTurnosPorPeriodo` devuelve `EstadoNombre = 'Asistió'` y `FormReportes` compara contra esa cadena exacta. | `SGC.UI/FormReportes.cs` |
| 5 | **Los procedimientos con filtro de médico se ejecutan una vez por médico** cuando hay que sumar varios (Recepcionista): `sp_ResumenDelDia` en el Inicio y `sp_ReporteTurnosPorPeriodo` en Reportes. No hay un parámetro de lista de médicos. | `SGC.Logica/ResumenService.cs`, `SGC.UI/FormReportes.cs` |
| 6 | **`sp_ReporteOcupacionAgenda` no filtra `Medicos.Activo`:** si un médico dado de baja conserva franjas activas, aparece en el reporte. Deducido del SQL; no se ejecutó con ese caso. | `sp_ReporteOcupacionAgenda.sql` |
| 7 | **Los mensajes de error de los `.sql` llevan tildes** ("médico", "días", "agrupación") porque se escriben como `N'...'` (Unicode); en cambio muchos mensajes de los Services en C# no las llevan (ver `03-reglas-de-negocio.md`). | `SGC.Datos/StoredProcedures/*.sql` |

## Para seguir leyendo

- Reglas de negocio equivalentes en C#: [`03-reglas-de-negocio.md`](03-reglas-de-negocio.md)
- Tablas que leen estos procedimientos: [`02-modelo-de-datos.md`](02-modelo-de-datos.md)
- Cómo arranca la app y aplica las migraciones: [`01-arquitectura.md`](01-arquitectura.md)
