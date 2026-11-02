# Guía para modificar el sistema

Acá vas a encontrar recetas cortas y exactas para los cambios más comunes (pantalla nueva, campo nuevo, procedimiento, reporte, regla de negocio), cómo depurar los problemas típicos, las trampas conocidas y una **tabla con todos los defectos y deudas** que se encontraron al documentar. Sirve para cambiar algo sin romper otra cosa y para decidir qué mejorar en la próxima entrega.

> Los comandos de esta guía se verificaron en este repo: `dotnet ef migrations list` y `dotnet ef migrations has-pending-model-changes` se ejecutaron con éxito; **no se creó ninguna migración**. Donde algo no se ejecutó se aclara.

## Índice

1. [Recetas](#recetas)
2. [Cómo depurar](#cómo-depurar)
3. [Trampas conocidas](#trampas-conocidas)
4. [Transacciones y concurrencia](#transacciones-y-concurrencia)
5. [Defectos y deuda conocidos](#defectos-y-deuda-conocidos)
6. [Qué se puede mejorar para la próxima entrega](#qué-se-puede-mejorar-para-la-próxima-entrega)

---

## Recetas

Todos los comandos se corren desde la carpeta del `.sln` (la raíz del repo).

### Agregar una pantalla ABM

Modelo a copiar: `FormPacientes` (formulario) y `PacienteService` (servicio). El patrón visual está en [`06-capa-ui.md`](06-capa-ui.md).

1. **Entidad:** ver si ya existe en `SGC.Entidades`. Si hay que crearla o cambiarla, ver "Agregar un campo" (abajo).
2. **Service** en `SGC.Logica/XxxService.cs`: métodos `ObtenerListado`, `Agregar`, `Modificar`, `EliminarLogico`; un `Validar` que lance excepciones con mensajes claros; cada método con `using var contexto = SGCContextFactory.Crear();`. Si necesitás datos calculados para la grilla, creá una clase `XxxFila` (ver [`01`](01-arquitectura.md#4-patrón-dto-fila)).
3. **Formulario** (`Form` nuevo en `SGC.UI`): armá en el Designer solo la estructura (tarjeta izquierda, barra de herramientas, `FlowLayoutPanel` de contadores, `DataGridView`, panel y label de la franja de mensajes). Todo lo demás en el `.cs`:
   - Constructor: `FranjaMensaje`, `BarraContadores`, suscripción de eventos, `AcceptButton = BtnGuardar`.
   - `Load`: `GrillaUi.Estilizar` y `GrillaUi.AgregarColumna(...)` por cada columna, chips de contadores, `EstiloUi.EstilizarBoton(...)`.
   - Reglas de selección: desenganchar `SelectionChanged` antes de asignar el `DataSource` y usar la bandera `_cargando` (ver [`06`](06-capa-ui.md#8-enganchar-y-desenganchar-selectionchanged)).
4. **Menú** (`FormMenuPrincipal`): agregá el botón en el Designer y completá cuatro lugares del `.cs`: `ConfigurarBotonesPorRol` (para qué rol se ve), `_iconos[btnXxx] = IconosUi.Xxx` en `ConfigurarBotonesDelMenu`, el `yield return` en `BotonesDelMenu()` y el `Click` en `AsignarEventosNavegacion` con `AbrirFormularioHijo(new FormXxx(...))`. Si falta el icono, el botón no se dibuja bien (`BotonMenu_Paint` sale sin pintar).
5. **Probar** a 100 %, 125 % y 150 % de escala y con los tres roles.

### Agregar un campo a una entidad

1. Agregá la propiedad en la clase de `SGC.Entidades` (por ejemplo `Paciente.Direccion`). Si necesita restricción (longitud, `CHECK`, índice), configurala en `SGC.Datos/SGCContext.cs` (`OnModelCreating`).
2. Creá la migración (**este comando no se ejecutó para esta guía**, para no generar archivos; el mismo `--project` sí se verificó con `migrations list`):

   ```
   dotnet ef migrations add AgregaDireccionPaciente --project SGC.Datos
   ```

   - **No uses `--startup-project SGC.UI`:** `SGC.UI` no referencia `Microsoft.EntityFrameworkCore.Design` y la herramienta falla con ese error (verificado). Alcanza con `--project SGC.Datos`: ese proyecto tiene el paquete y `SGCContextDesignTimeFactory`, que encuentra la cadena de conexión igual que la app.
   - La herramienta instalada en esta máquina es `dotnet-ef` 10.0.12; los paquetes del proyecto son EF Core 8.0.11.
3. Revisá el archivo generado en `SGC.Datos/Migrations/`: si es una columna obligatoria en una tabla con datos, necesita un valor por defecto.
4. Aplicala: al abrir la app (`Program.cs` ejecuta `Migrate()`) o con `dotnet ef database update --project SGC.Datos`.
5. Completá el resto: el `Validar` del Service, el control en el formulario, el `DbSeeder` si siembra esa entidad y los procedimientos que listen columnas a mano.
6. **Nunca edites una migración ya aplicada** (otras bases no la vuelven a ejecutar): hacé una nueva.

> Chequeo útil antes de crear la migración: `dotnet ef migrations has-pending-model-changes --project SGC.Datos` dice si el modelo cambió respecto de la última migración. Hoy responde "No changes have been made to the model since the last migration."

### Agregar un procedimiento almacenado

Está completo en [`05-procedimientos-almacenados.md`](05-procedimientos-almacenados.md#4-cómo-agregar-o-cambiar-un-procedimiento). Resumen: `.sql` con `CREATE OR ALTER PROCEDURE` → migración con `migrationBuilder.Sql("""...""")` (un `Sql` por procedimiento) → DTO con propiedades de igual nombre que las columnas → método en el Service con `SqlQueryRaw<T>`. Para **cambiar** uno existente: editá el `.sql` y creá una migración **nueva**.

### Agregar un reporte

Pasos 1 a 10 en [`07-reportes.md`](07-reportes.md#6-cómo-agregar-una-pestaña-nueva). Lo que más se olvida: completar `vista.Encabezados` y `vista.FilasExportacion` (si no, no exporta) y decidir qué roles la ven.

### Copia de seguridad: qué revisar antes de tocarla

Código en `SGC.Logica/BackupService.cs` y `SGC.UI/FormBackup.cs`; reglas en [`03`](03-reglas-de-negocio.md#12-copia-de-seguridad-y-restauración), recorrido en [`04`](04-flujos.md#i-copia-de-seguridad-y-restauración). *Probado solo contra una base de prueba (crear, verificar, restaurar); falta la prueba desde la pantalla.*

1. **La ruta la usa SQL Server, no la app.** La carpeta tiene que existir **en el servidor** y la **cuenta de servicio de SQL Server** necesita permiso de escritura (crear) o lectura (restaurar). Si falla con "no pudo acceder", casi siempre es eso (errores 3201, 3203, 3013). La carpeta que sugiere la pantalla (`InstanceDefaultBackupPath`) es la que ya tiene ese permiso.
2. **Restaurar se hace desde `master`** con `SINGLE_USER WITH ROLLBACK IMMEDIATE` y vuelve siempre a `MULTI_USER` en un `finally`. Si cambiás ese método, no saques el `finally`: una base que queda en `SINGLE_USER` rechaza a todos.
3. **Después de restaurar hay que vaciar los pooles** (`SqlConnection.ClearAllPools()`) y volver al login: las conexiones abiertas apuntan a la base anterior.
4. **El usuario de conexión necesita permisos** de backup/restore (por ejemplo `db_owner` o `sysadmin`) y lectura de `msdb` para el historial. Sin lectura de `msdb`, la pantalla funciona pero el historial y la tarjeta "Última copia" del Inicio quedan vacíos o en advertencia.
5. **El nombre de la base no se escribe a mano:** sale de la cadena de conexión (`NombreBase`) y se escapa con corchetes (`Entre`); la ruta viaja como parámetro `@ruta`. Mantené ese criterio si agregás sentencias.
6. **Un backup es de una base y una versión:** restaurar una copia vieja devuelve también el esquema viejo (`__EFMigrationsHistory` incluido). `Restaurar` ya ejecuta `Migrate()` al terminar (`MigrarBase`) y, si falla, la pantalla pide cerrar la aplicación en vez de volver al login.
7. **Antes de tocar nada se valida el archivo:** `ObtenerInfoBackup` hace `RESTORE HEADERONLY` y rechaza un `.bak` que no sea una copia completa de **esta** base (`DatabaseName`). No lo saques: restaurar la copia de otra base la reemplazaría con `WITH REPLACE`.
8. **Mientras corre una operación la ventana está bloqueada** (`FormMenuPrincipal.BloquearPorOperacion` y `OnFormClosing`): si agregás una operación larga, usá el mismo mecanismo.
9. **No hay copias programadas:** todo es manual; el `.bak` queda en el servidor.

### Cambiar una regla de negocio

1. Buscá la regla en [`03-reglas-de-negocio.md`](03-reglas-de-negocio.md): dice en qué archivo y método vive.
2. Cambiala en el Service (`SGC.Logica`). **No hay proyecto de pruebas en la solución**: probá a mano con los tres usuarios de prueba.
3. Revisá las **copias de la regla**:
   - La pantalla que ayuda al usuario (por ejemplo `FormTurnos.CrearHorarios` repite la lógica de superposición y de ausencias para pintar botones).
   - `sp_AsignarTurno`, que duplicaba `TurnoService.AsignarTurno` (hoy nadie lo llama y tiene menos reglas que el C#: sin paciente activo, sin duración 5-240, sin paciente libre).
   - Las restricciones de la base (`CHECK`, índices): si la regla es más estricta que la base, no hace falta migración; si es más laxa, sí.
   - Los números de estado (`1` a `4`) que usan los procedimientos.
4. Si cambia un texto que otro código compara (por ejemplo `"Asistió"`), buscalo con una búsqueda de texto en toda la solución.

---

## Cómo depurar

| Síntoma | Qué mirar |
|---------|-----------|
| Al abrir la app aparece **"No se pudo conectar/inicializar la base de datos"** y se cierra | SQL Server está apagado o la cadena de conexión es otra. Iniciá el servicio de SQL Server (Servicios de Windows → "SQL Server (NOMBRE_INSTANCIA)") y revisá el `appsettings.Local.json`. Ese cuadro sale de `Program.cs`, antes del login. |
| Conecta a la base equivocada o no encuentra el servidor | Falta o está mal `appsettings.Local.json`. Debe estar **junto al `.sln`**, copiado de `appsettings.example.json`, con tu instancia en `ConnectionString` (por ejemplo `Server=.\\SQLEXPRESS;...`). Si no existe, la app usa `Server=.` (la instancia por defecto). El archivo está en el `.gitignore`. |
| Cada pantalla muestra "No se pudieron cargar..." con la app ya abierta | SQL Server cayó o se cortó la conexión después de arrancar. Cada pantalla muestra el error de su operación en la franja; lo que no se atrapa llega al manejador global de `Program.cs` ("Ocurrió un error inesperado...") y la aplicación sigue abierta. |
| Al compilar: **error de archivo bloqueado** (`MSB3027`, "being used by another process") | La aplicación sigue abierta y bloquea los `.dll` de `bin/`. Cerrala (cerrar el menú con la X termina el proceso). Si igual queda algo, buscá `SGC.UI.exe` en el Administrador de tareas y finalizalo; para compilar sin cerrarla, usá `-p:OutDir=<otra carpeta>`. |
| No sabés qué hay en la base | `sqlcmd -S ".\NICO_SERV1" -E -C -d SGC -Q "SELECT COUNT(*) FROM Turnos"` (cambiá la instancia por la tuya; usá solo `SELECT`). |
| Un reporte "no cierra" con otro | Los procedimientos filtran distinto los cancelados (ver defecto R-2 más abajo). |
| Una pantalla de Luciano se corta con otra escala | Usa píxeles fijos en algunos lugares; reemplazar por `EscalaDpi.Px` (ver [`06`](06-capa-ui.md#6-escala-de-pantalla-dpi)). |
| Querés ver qué cambió un procedimiento | `git log -- SGC.Datos/StoredProcedures/<archivo>.sql` |
| "SQL Server no pudo acceder a ..." al crear o restaurar una copia | La ruta la usa la cuenta de servicio de SQL Server: la carpeta debe existir en el servidor y esa cuenta debe tener permiso (ver la receta de copia de seguridad). |
| Tras una restauración fallida la base rechaza conexiones | Quedó en `SINGLE_USER`: `ALTER DATABASE [SGC] SET MULTI_USER` desde `master`. El código lo evita con un `finally`, pero un corte del servidor en medio puede dejarla así (deducido). |

---

## Trampas conocidas

| Trampa | Qué pasa | Cómo evitarla |
|--------|----------|---------------|
| **Designer** | Visual Studio regenera `InitializeComponent()` al abrir el formulario en modo diseño y borra en silencio lo que no sea asignación de propiedades (columnas, métodos propios). | Columnas, chips y estilos van en el `.cs`. Ver [`06`](06-capa-ui.md#5-lógica-en-el-cs-el-designer-solo-para-la-estructura). |
| **Tildes y registro** | Los textos que ve el usuario van en español neutro (sin voseo: "Seleccione", "Indique"), con tildes, ñ y signos de apertura (¿ ¡); los **comentarios** del código no llevan tildes (evita problemas de codificación). Los datos sembrados (`DbSeeder`, `HasData`) todavía tienen especialidades y tipos de actividad sin tildes: cambiarlos exige una migración. Los mensajes de los `.sql` llevan tildes porque se escriben como `N'...'`. | Textos de usuario con tildes. Para scripts SQL a mano: `sqlcmd ... -f 65001`. Un `RAISERROR` con tildes necesita el prefijo `N`. La cultura de la aplicación es es-AR (`Program.Main`). |
| **DPI** | Un tamaño fijo en píxeles recorta el texto con escala 125 %/150 %. Lo creado por código no se autoescala. | `EscalaDpi.Px(this, valor)`, `Dock`, layouts y medir el texto. |
| **`EXEC` no componible** | EF no puede agregar `Include`, `Where` ni `JOIN` sobre un `EXEC` de procedimiento. | Leer el resultado a memoria y cargar las navegaciones con una segunda consulta (ver `TurnoService.ObtenerPorMedicoYFecha`). |
| **`SelectionChanged`** | Asignar el `DataSource` selecciona la primera fila y dispara el evento: la pantalla pasa a "editar" sola. | Desenganchar, asignar, `ClearSelection`, volver a enganchar; y la bandera `_cargando`. |
| **`HoraFin` no es columna** | `Turno.HoraFin` es calculada, EF no la traduce a SQL. | Traer los turnos del día y comparar en memoria (`HaySuperposicion`). |
| **Día de la semana** | `DayOfWeek` de C# (0 = domingo) y `DATEPART(weekday)` de SQL (depende de `@@DATEFIRST`) no coinciden. | En SQL usar la fórmula con `DATEDIFF` desde `'19000107'` (ver [`05`](05-procedimientos-almacenados.md#21-el-día-de-la-semana-no-depende-del-idioma)). |
| **Parámetros opcionales** | Un `null` de C# no viaja como `NULL` de SQL. | `DBNull.Value` (`ParametroEntero`, `ParametroFecha`). |
| **Baja lógica e índices únicos** | Los índices únicos incluyen filas inactivas: no se puede volver a crear un registro dado de baja con el mismo DNI/nombre (ahora con un mensaje claro). | Ver defecto N-10. |
| **Migraciones viejas** | Una migración aplicada no se vuelve a ejecutar. | Cambios nuevos = migración nueva. |
| **Ruta del backup** | El `.bak` lo escribe y lee la cuenta de servicio de SQL Server, no tu usuario de Windows: una carpeta tuya puede fallar aunque exista. | Usar la carpeta por defecto de la instancia o dar permiso a esa cuenta. |
| **Restaurar y `SINGLE_USER`** | Reemplazar la base exige `master` y acceso exclusivo; si queda en `SINGLE_USER` nadie entra. | No quitar el `finally` de `BackupService.Restaurar`; vaciar los pooles. |

---

## Transacciones y concurrencia

**No hay transacciones explícitas en la aplicación.** Se buscó `BeginTransaction`, `TransactionScope`, `IDbContextTransaction` y `CommitTransaction` en `SGC.Logica`, `SGC.UI`, `SGC.Entidades` y `SGC.Datos` (incluidos los `.sql`, donde también se buscó `BEGIN TRAN`, `UPDLOCK` y `HOLDLOCK`) y **no aparece ninguno** en el código de la aplicación ni en los procedimientos. La única excepción son los dos scripts de demostración que se corren a mano (`SGC.Datos/Scripts/carga_masiva_demo.sql` y `descarga_masiva_demo.sql`), que sí usan `BEGIN TRAN`.

**Lo que sí hay:** cada operación hace un solo `SaveChanges()` y EF Core lo ejecuta dentro de una **transacción implícita**, así que un guardado individual es atómico (se hace todo o nada).

### El riesgo real: "consultar y después insertar"

`TurnoService.AsignarTurno` (y `ModificarTurno`) hace dos pasos separados:

1. `HaySuperposicion(...)`: consulta si el médico ya tiene un turno que se pisa.
2. `contexto.Turnos.Add(...)` + `SaveChanges()`: inserta.

Entre el paso 1 y el 2 no hay nada que bloquee (lo mismo vale para la comprobación de que el paciente no tenga otro turno a la vez, `ExigirPacienteLibre`). Escenario:

| Tiempo | Recepcionista A | Recepcionista B |
|--------|-----------------|-----------------|
| t1 | `HaySuperposicion` → no hay turno a las 10:00 | |
| t2 | | `HaySuperposicion` → tampoco hay |
| t3 | Inserta el turno de las 10:00 | |
| t4 | | Inserta otro turno a las 10:00 |

Resultado: **dos turnos superpuestos** del mismo médico. La base no lo impide: la tabla `Turnos` solo tiene la clave primaria y los índices comunes `IX_Turnos_MedicoId` e `IX_Turnos_PacienteId` (consultado con `sqlcmd`; no existe un índice único por `(MedicoId, Fecha, HoraInicio)`). Y aunque existiera, solo atajaría turnos con **la misma hora de inicio**, no los que se pisan parcialmente.

`sp_AsignarTurno` tiene el mismo patrón: un `IF EXISTS` de superposición seguido de un `INSERT`, **sin transacción y sin `UPDLOCK`/`HOLDLOCK`**.

Como es una agenda de pocos usuarios, la probabilidad es baja, pero es el único lugar donde dos operaciones simultáneas pueden dejar datos inconsistentes.

### Antes había dos guardados encadenados (resuelto)

`ExcepcionAgendaService.Agregar` hacía dos `SaveChanges` (la ausencia y la cancelación de turnos) y podía dejar la ausencia guardada con los turnos sin cancelar. Ahora marca todo en el mismo contexto y llama **una sola vez** a `SaveChanges` (una transacción implícita); los avisos se registran después. Ver N-2.

### Cómo arreglarlo (propuesta, no implementada)

Mover la validación y el `INSERT` **adentro de `sp_AsignarTurno`**, en una transacción que bloquee el rango leído, y que C# llame a ese procedimiento:

```sql
SET XACT_ABORT ON;
BEGIN TRANSACTION;
    IF EXISTS (SELECT 1 FROM Turnos t WITH (UPDLOCK, HOLDLOCK)
               WHERE t.MedicoId = @MedicoId AND t.Fecha = @Fecha AND t.Activo = 1
                 AND t.HoraInicio < @HoraFin
                 AND DATEADD(MINUTE, t.DuracionMinutos, t.HoraInicio) > @HoraInicio)
    BEGIN
        RAISERROR(N'Existe un turno superpuesto para el médico.', 16, 1);
        ROLLBACK TRANSACTION; RETURN;
    END
    INSERT INTO Turnos (...) VALUES (...);
COMMIT TRANSACTION;
```

Con `UPDLOCK, HOLDLOCK` el segundo que llega espera al primero y después ve su turno. En C#, `TurnoService.AsignarTurno` ejecutaría `EXEC sp_AsignarTurno` y traduciría el `SqlException` (los `RAISERROR` de severidad 16) en un mensaje. Antes habría que **actualizar el procedimiento** con las reglas que hoy solo tiene el C# (paciente y médico activos, duración 5-240 sin cruzar la medianoche, paciente sin turnos simultáneos). `ModificarTurno` necesitaría un procedimiento equivalente.

### Historia de `sp_AsignarTurno`

`git log -- SGC.Datos/StoredProcedures/sp_AsignarTurno.sql`:

| Commit | Autor | Fecha | Qué hizo |
|--------|-------|-------|----------|
| `e5bc51c` | Luciano Villordo | 16/10 | Creó el procedimiento (con `DATEPART(WEEKDAY, ...)` para el día) |
| `245e9de` | Nicolás Valdes | 19/10 | Lo corrigió: día de la semana independiente del idioma (`DATEDIFF`), validación de `ExcepcionesAgenda`, solo franjas activas, duración por defecto, mensajes `N'...'` y `CREATE OR ALTER` |

Ese segundo commit es el que agregó la validación de ausencias que `TurnoService` nunca tuvo.

---

## Defectos y deuda conocidos

Consolidado de **todos** los hallazgos de [`01`](01-arquitectura.md#hallazgos), [`02`](02-modelo-de-datos.md), [`03`](03-reglas-de-negocio.md#hallazgos) y [`05`](05-procedimientos-almacenados.md#hallazgos), más los que se encontraron al escribir esta guía y los de la auditoría posterior. **Estado:** *vigente* = sigue pasando con el código actual; *resuelto* = corregido (se indica el commit o el cambio). Impacto: **A** = puede dar datos incorrectos o filtrar información; **M** = confunde o complica; **B** = cosmético o de mantenimiento.

### Reglas de negocio y datos

| ID | Estado | Hallazgo | Dónde | Imp. | Sugerencia de arreglo |
|----|:------:|----------|-------|:----:|-----------------------|
| N-1 | **Vigente** | **Carrera al asignar o modificar:** consulta de superposición (médico y paciente) y luego inserción, sin transacción ni índice único; dos asignaciones simultáneas pueden crear turnos superpuestos | `TurnoService.AsignarTurno`, `ModificarTurno`; `sp_AsignarTurno.sql` | A | Transacción con `UPDLOCK, HOLDLOCK` dentro de `sp_AsignarTurno` y que C# lo invoque (ver sección anterior) |
| N-2 | Resuelto | ~~`ExcepcionAgendaService.Agregar` usaba dos `SaveChanges`~~ Ahora la ausencia y la cancelación de turnos se guardan con **un solo** `SaveChanges` (commit `c3a9c56`) | `ExcepcionAgendaService.Agregar` | — | — |
| N-3 | Resuelto | ~~`AsignarTurno` y `ModificarTurno` ignoraban las ausencias~~ Ahora rechazan cualquier turno dentro de una ausencia (`ExigirSinAusencia`, commit `c3a9c56`); además las ausencias activas no pueden superponerse entre sí | `TurnoService`, `ExcepcionAgendaService` | — | — |
| N-4 | Resuelto | ~~`AsignarTurno` no validaba que paciente y médico existieran y estuvieran activos~~ Ahora ambos métodos releen y exigen paciente y médico activos (`ExigirActivos`, commit `813c20a`); las bajas se rechazan si dejan turnos futuros (sección 13 de `03`) | `TurnoService` | — | — |
| N-5 | Resuelto | ~~Se podía modificar o cancelar un turno ya atendido~~ Solo se modifica o cancela un turno `Confirmado`, en el Service y en la pantalla (commit `4472d8e`) | `TurnoService`, `FormTurnos.ActualizarAcciones` | — | — |
| N-6 | **Vigente** | **Modificar o eliminar una ausencia no re-evalúa turnos:** modificar no cancela los turnos que queden afectados; eliminar no restituye los cancelados | `ExcepcionAgendaService.Modificar`, `EliminarLogico` | M | Reaplicar `CancelarTurnosAfectados` al modificar; avisar en pantalla al eliminar |
| N-7 | Resuelto | ~~El estado `Pendiente (0)` nunca se crea~~ Se eliminó del enum, la UI y los procedimientos (los demás valores conservan su número) | `QuitaEstadoPendiente` | — | — |
| N-8 | **Vigente** | Editar un paciente cuya obra social fue dada de baja lo guarda como **Particular sin avisar** (deducido) | `FormPacientes.CargarCombos`, `BtnGuardar_Click` | M | Cargar la obra social actual del paciente aunque esté inactiva, o avisar |
| N-9 | Resuelto | ~~La matrícula duplicada solo la rechazaba la base~~ `MedicoService.Validar` la controla con mensaje claro (commit `eb6250f`) | `MedicoService.Validar` | — | — |
| N-10 | **Parcial** | Los índices únicos incluyen filas dadas de baja, así que **no se puede recrear** un médico, paciente u obra social con los mismos datos únicos. Mejoró: ahora el mensaje es claro ("... está dado de baja", commit `eb6250f`), el usuario tiene `Reactivar` y la franja de agenda se reactiva sola | `SGCContext.cs` (índices), `Persistencia.Guardar` | M | Agregar `Reactivar` a médico, paciente y obra social, o índices filtrados `WHERE Activo = 1` |
| N-11 | **Vigente** | Validaciones de C# y de la base **no idénticas** (DNI 7-8 dígitos vs. 15 caracteres; nacimiento hasta 120 años y no hoy vs. solo no futuro; email solo en C#; precio `> 0` en C# vs. `>= 0` en la base) | `PacienteService.Validar`; `MedicoService.Validar`; `SGCContext.cs` | B | Alinear los `CHECK` o documentar la diferencia |
| N-12 | **Vigente** | `Turnos.MedioPago` es **texto libre** con el nombre de la obra social; un renombre no actualiza turnos viejos | `FormConfirmarAsistencia` | B | Guardar el `ObraSocialId` o un código de medio de pago |
| N-13 | Resuelto | ~~El script de demo podía generar `Estado = 0`~~ Corregido (commits `0027ddb` y `ac4b935`: no genera turnos fuera de la agenda ni depende de ids fijos) | `SGC.Datos/Scripts/carga_masiva_demo.sql` | — | — |
| N-14 | **Vigente** | `FormAgendaMedico` y `FormRegistrarActividad`: si el usuario no tiene médico vinculado, toman **el primero de la lista** de médicos | `DeterminarMedicoActivo` (ambas) | M | Mostrar un error en vez de asumir un médico |
| N-15 | **Vigente** | **Los turnos `Confirmado` de fechas pasadas no se cierran solos** (nadie los pasa a `Asistio` o `Ausente`): siguen figurando como pendientes y los reportes los cuentan como confirmados. Hay 89 al 02/11/2026 en la base de desarrollo (`SELECT COUNT(*) FROM Turnos WHERE Estado = 1 AND Fecha < CAST(GETDATE() AS date)`) | `TurnoService`, reportes | M | Cierre manual por recepción o un proceso que los marque `Ausente` |
| N-16 | **Vigente** | **RNF#07 solo a nivel de aplicación:** hay un único login de SQL Server para todos los roles; la separación de datos clínicos vive en la interfaz y en `AtencionFila`, no en el motor. **Decisión explícita:** el Médico puede leer la historia clínica completa de un paciente (incluidas las atenciones de otros médicos); la Recepcionista nunca ve datos clínicos | `AtencionFila`, `FormHistorialClinico`, `03` sección 8 | M | Usuarios de base por rol o vistas/permisos si se exigiera a nivel de motor |
| N-17 | **Vigente** | **No hay copias de seguridad programadas:** solo manuales desde la pantalla; el `.bak` lo escribe la cuenta de servicio de SQL Server en el servidor | `BackupService` | M | Un trabajo de SQL Server Agent o una tarea programada |
| N-18 | **Vigente** | **Datos de demostración en la base de desarrollo:** el paciente "prueba aprueba" y la obra social "nuevoobra" siguen cargados (la usa una paciente), y los pacientes de demo pueden tener turnos que se superponen entre sí por corridas viejas del script | Base de desarrollo | B | Limpiar antes de una demo o de una entrega |

### Procedimientos y reportes

| ID | Estado | Hallazgo | Dónde | Imp. | Sugerencia de arreglo |
|----|:------:|----------|-------|:----:|-----------------------|
| R-1 | **Vigente** | **`sp_AsignarTurno` no está conectado a nada** (ningún Service ni pantalla lo invoca) y tiene menos reglas que el C# | `sp_AsignarTurno.sql`, `TurnoService.cs` | M | Conectarlo (ver N-1) actualizándolo antes, o eliminarlo para no tener dos reglas |
| R-2 | **Vigente** | **Los reportes filtran distinto los cancelados:** cuentan cancelados `sp_ReporteTurnosPorPeriodo`, `sp_ReporteAsistencia`, `sp_ReportePacientesPorMes` y `sp_ReporteRendimientoMedicos`; los excluyen `sp_ReporteOcupacionAgenda` y `sp_ReporteTurnosPorObraSocial`; Recaudación solo cuenta `Asistio` | `StoredProcedures/*.sql` | M | Definir un criterio único y aplicarlo con una migración nueva por procedimiento |
| R-3 | Resuelto | ~~El reporte "Obras sociales" mostraba a la Recepcionista los datos de todos los médicos~~ `sp_ReporteTurnosPorObraSocial` recibe `@MedicoId` y la pantalla acota a los médicos del usuario (migración `AgregaMedicoIdAReportes`, aplicada; los tres procedimientos se ejecutaron con y sin `@MedicoId`) | `FormReportes.Estadisticas.cs`; `sp_ReporteTurnosPorObraSocial` | — | — |
| R-4 | **Vigente** | Dos reportes agrupan por la **obra social actual del paciente**, no por lo cobrado (recaudación por obra social vs. medio de pago) | `sp_ReporteRecaudacion`, `sp_ReporteTurnosPorObraSocial` | M | Guardar la obra social usada en el turno al cobrar |
| R-5 | **Vigente** | **Texto acoplado SQL ↔ pantalla:** `EstadoNombre = 'Asistió'` se compara por texto | `sp_ReporteTurnosPorPeriodo`; `FormReportes.GenerarTurnos` | B | Devolver también el número de estado y comparar por número |
| R-6 | **Vigente** | Los procedimientos con filtro de **un solo médico** se ejecutan una vez por médico para la Recepcionista (Turnos, Asistencia, Ocupación, Pacientes por mes, Recaudación, Obras sociales, Rendimiento y el resumen de Inicio): "Todos mis médicos" hace N consultas por pestaña | `ResumenService`, `FormReportes` | B | Parámetro con lista de médicos (tabla de valores) |
| R-7 | **Vigente** | `sp_ReporteOcupacionAgenda` no filtra `Medicos.Activo` (deducido del SQL) | `sp_ReporteOcupacionAgenda.sql` | B | Agregar el filtro |
| R-8 | Resuelto | ~~El CSV no escapaba un `-` inicial~~ `EscaparCsv` antepone `'` a `=`, `+`, `@` y a un `-` que no sea un número (commit `3ac3fb0`); los números negativos se dejan como están | `FormReportes.EscaparCsv` | — | — |

### Arquitectura, interfaz y mantenimiento

| ID | Estado | Hallazgo | Dónde | Imp. | Sugerencia de arreglo |
|----|:------:|----------|-------|:----:|-----------------------|
| A-1 | Resuelto | ~~Cerrar sesión no terminaba el proceso~~ Cerrar el menú con la X ejecuta `Application.Exit` salvo que se esté cerrando sesión, y cerrar sesión reutiliza el login oculto (commit `95ec2a6`) | `FormMenuPrincipal.OnFormClosed`, `CerrarSesion`, `FormLogin.Reiniciar` | — | — |
| A-2 | **Vigente** | Dos servicios con **datos en memoria**: `HorarioService` (tramos) y `NotificacionService` (avisos; los emails son simulados y los avisos se vacían al cerrar sesión) | `SGC.Logica/HorarioService.cs`, `NotificacionService.cs` | M | Persistir los avisos o integrar un envío real |
| A-3 | **Vigente** | La cadena de conexión por defecto es `Server=.`, no `SQLEXPRESS` (ahora se lee una sola vez y un `appsettings.Local.json` mal formado se informa con un mensaje claro) | `SGCContextFactory.cs` | B | Mantener el `appsettings.Local.json` por integrante (ya documentado) |
| A-4 | **Parcial** | **Tildes:** los textos visibles para el usuario (pantallas, mensajes de los Services, `NombreDia`, etiquetas de estado, avisos) ya llevan tildes y signos de apertura. Quedan **sin tildes** las especialidades y tipos de actividad sembrados, y los comentarios del código | `DbSeeder.cs`, `SGCContext.cs` (`HasData`) | B | Corregir los datos sembrados con una migración nueva |
| A-5 | **Vigente** | Pantallas de Luciano sin todos los componentes compartidos y con píxeles fijos en algunos lugares (`FormAgendaMedico`, `FormRegistrarActividad`) | `SGC.UI` | B | Migrarlas al patrón de [`06`](06-capa-ui.md) |
| A-6 | **Vigente** | `FormHistorialPaciente` (versión modal) no se abre desde el menú: solo corre cuando se abre sin navegador | `FormHistorialPaciente.cs` | B | Eliminarla o usarla de verdad |
| A-7 | **Vigente** | **No hay proyecto de pruebas automáticas** en la solución | `SistemaGestionConsultorio.sln` | M | Agregar un proyecto de pruebas para las reglas de `TurnoService` |
| A-8 | Resuelto | ~~El README estaba desactualizado~~ El README describe EF Core, SQL Server, roles y copia de seguridad | `README.md` | — | — |
| A-9 | **Parcial** | La **copia de seguridad/restauración** se probó solo contra una base de prueba: falta probarla desde la pantalla. La migración `AgregaMedicoIdAReportes` está aplicada y los tres procedimientos se ejecutaron con y sin `@MedicoId` | `BackupService.cs`, `FormBackup.cs` | M | Probar desde la pantalla: crear, verificar y restaurar una copia con el usuario administrador |
| A-10 | **Vigente** | **`script-base-datos.sql`** (raíz del repositorio, sin versionar) fue generado antes de las migraciones nuevas y **no forma parte de la entrega**: no usarlo; la base se crea con `dotnet ef database update --project SGC.Datos` o abriendo la aplicación | `script-base-datos.sql` | B | Regenerarlo desde las migraciones o no entregarlo |

---

## Qué se puede mejorar para la próxima entrega

Ya están hechos (ver la tabla anterior): quitar el estado `Pendiente` (N-7), las ausencias que bloquean turnos y se guardan en una sola transacción (N-2, N-3), turnos atendidos inmutables (N-5), matrícula duplicada (N-9), cierre de proceso (A-1) y las bajas con control.

Lo que sigue, ordenado por prioridad (primero lo que evita datos incorrectos):

1. **Probar la copia de seguridad desde la pantalla** (A-9) y decidir si hace falta programarla (N-17).
2. **Cerrar la carrera de asignación** (N-1): `UPDLOCK, HOLDLOCK` y `BEGIN TRANSACTION` dentro de `sp_AsignarTurno`, actualizándolo antes con las reglas que hoy solo tiene el C#, y que `TurnoService` lo invoque (cierra también R-1).
3. **Definir qué hacer con los turnos `Confirmado` vencidos** (N-15).
4. **Poder reactivar médicos, pacientes y obras sociales** dados de baja (como `UsuarioService.Reactivar`). Cierra N-10.
5. **Re-evaluar turnos al modificar o eliminar una ausencia** (N-6) y mostrar un error cuando un usuario Médico no tiene médico vinculado (N-14).
6. **Limpiar los datos de demostración** (N-18) y unificar el criterio de cancelados en los reportes (R-2).

## Para seguir leyendo

- Reglas y validaciones actuales: [`03-reglas-de-negocio.md`](03-reglas-de-negocio.md)
- Procedimientos y migraciones: [`05-procedimientos-almacenados.md`](05-procedimientos-almacenados.md)
- Patrones de pantalla: [`06-capa-ui.md`](06-capa-ui.md)
