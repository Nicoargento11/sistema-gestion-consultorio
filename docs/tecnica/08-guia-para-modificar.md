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

### Cambiar una regla de negocio

1. Buscá la regla en [`03-reglas-de-negocio.md`](03-reglas-de-negocio.md): dice en qué archivo y método vive.
2. Cambiala en el Service (`SGC.Logica`). **No hay proyecto de pruebas en la solución**: probá a mano con los tres usuarios de prueba.
3. Revisá las **copias de la regla**:
   - La pantalla que ayuda al usuario (por ejemplo `FormTurnos.CrearHorarios` repite la lógica de superposición y de ausencias para pintar botones).
   - `sp_AsignarTurno`, que duplica `TurnoService.AsignarTurno` (aunque hoy nadie lo llama).
   - Las restricciones de la base (`CHECK`, índices): si la regla es más estricta que la base, no hace falta migración; si es más laxa, sí.
   - Los números de estado (`0` a `4`) que usan los procedimientos.
4. Si cambia un texto que otro código compara (por ejemplo `"Asistió"`), buscalo con una búsqueda de texto en toda la solución.

---

## Cómo depurar

| Síntoma | Qué mirar |
|---------|-----------|
| Al abrir la app aparece **"No se pudo conectar/inicializar la base de datos"** y se cierra | SQL Server está apagado o la cadena de conexión es otra. Iniciá el servicio de SQL Server (Servicios de Windows → "SQL Server (NOMBRE_INSTANCIA)") y revisá el `appsettings.Local.json`. Ese cuadro sale de `Program.cs`, antes del login. |
| Conecta a la base equivocada o no encuentra el servidor | Falta o está mal `appsettings.Local.json`. Debe estar **junto al `.sln`**, copiado de `appsettings.example.json`, con tu instancia en `ConnectionString` (por ejemplo `Server=.\\SQLEXPRESS;...`). Si no existe, la app usa `Server=.` (la instancia por defecto). El archivo está en el `.gitignore`. |
| Cada pantalla muestra "No se pudieron cargar..." con la app ya abierta | SQL Server cayó o se cortó la conexión después de arrancar. No hay manejador global: cada pantalla muestra el error de su operación en la franja. |
| Al compilar: **error de archivo bloqueado** (`MSB3027`, "being used by another process") | La aplicación sigue abierta y bloquea los `.dll` de `bin/`. Cerrala. Ojo: por cómo está armado el cierre de sesión, el proceso puede quedar vivo y **oculto** aunque no veas ninguna ventana (ver [`04`](04-flujos.md#a-arranque-de-la-app-y-login), deducido): buscá `SGC.UI.exe` en el Administrador de tareas y finalizalo. |
| No sabés qué hay en la base | `sqlcmd -S ".\NICO_SERV1" -E -C -d SGC -Q "SELECT COUNT(*) FROM Turnos"` (cambiá la instancia por la tuya; usá solo `SELECT`). |
| Un reporte "no cierra" con otro | Los procedimientos filtran distinto los cancelados (ver defecto R-2 más abajo). |
| Una pantalla de Luciano se corta con otra escala | Usa píxeles fijos en algunos lugares; reemplazar por `EscalaDpi.Px` (ver [`06`](06-capa-ui.md#6-escala-de-pantalla-dpi)). |
| Querés ver qué cambió un procedimiento | `git log -- SGC.Datos/StoredProcedures/<archivo>.sql` |

---

## Trampas conocidas

| Trampa | Qué pasa | Cómo evitarla |
|--------|----------|---------------|
| **Designer** | Visual Studio regenera `InitializeComponent()` al abrir el formulario en modo diseño y borra en silencio lo que no sea asignación de propiedades (columnas, métodos propios). | Columnas, chips y estilos van en el `.cs`. Ver [`06`](06-capa-ui.md#5-lógica-en-el-cs-el-designer-solo-para-la-estructura). |
| **Tildes** | Los textos que ve el usuario llevan tildes y ñ; los **comentarios** del código no (evita problemas de codificación). Muchos mensajes de los Services no tienen tildes; los de los `.sql` sí, porque se escriben como `N'...'`. | Textos de usuario con tildes. Para scripts SQL a mano: `sqlcmd ... -f 65001`. Un `RAISERROR` con tildes necesita el prefijo `N`. |
| **DPI** | Un tamaño fijo en píxeles recorta el texto con escala 125 %/150 %. Lo creado por código no se autoescala. | `EscalaDpi.Px(this, valor)`, `Dock`, layouts y medir el texto. |
| **`EXEC` no componible** | EF no puede agregar `Include`, `Where` ni `JOIN` sobre un `EXEC` de procedimiento. | Leer el resultado a memoria y cargar las navegaciones con una segunda consulta (ver `TurnoService.ObtenerPorMedicoYFecha`). |
| **`SelectionChanged`** | Asignar el `DataSource` selecciona la primera fila y dispara el evento: la pantalla pasa a "editar" sola. | Desenganchar, asignar, `ClearSelection`, volver a enganchar; y la bandera `_cargando`. |
| **`HoraFin` no es columna** | `Turno.HoraFin` es calculada, EF no la traduce a SQL. | Traer los turnos del día y comparar en memoria (`HaySuperposicion`). |
| **Día de la semana** | `DayOfWeek` de C# (0 = domingo) y `DATEPART(weekday)` de SQL (depende de `@@DATEFIRST`) no coinciden. | En SQL usar la fórmula con `DATEDIFF` desde `'19000107'` (ver [`05`](05-procedimientos-almacenados.md#21-el-día-de-la-semana-no-depende-del-idioma)). |
| **Parámetros opcionales** | Un `null` de C# no viaja como `NULL` de SQL. | `DBNull.Value` (`ParametroEntero`, `ParametroFecha`). |
| **Baja lógica e índices únicos** | Los índices únicos incluyen filas inactivas: no se puede volver a crear un registro dado de baja con el mismo DNI/nombre. | Ver defecto D-1. |
| **Migraciones viejas** | Una migración aplicada no se vuelve a ejecutar. | Cambios nuevos = migración nueva. |

---

## Transacciones y concurrencia

**No hay transacciones explícitas en la aplicación.** Se buscó `BeginTransaction`, `TransactionScope`, `IDbContextTransaction` y `CommitTransaction` en `SGC.Logica`, `SGC.UI`, `SGC.Entidades` y `SGC.Datos` (incluidos los `.sql`, donde también se buscó `BEGIN TRAN`, `UPDLOCK` y `HOLDLOCK`) y **no aparece ninguno** en el código de la aplicación ni en los procedimientos. La única excepción son los dos scripts de demostración que se corren a mano (`SGC.Datos/Scripts/carga_masiva_demo.sql` y `descarga_masiva_demo.sql`), que sí usan `BEGIN TRAN`.

**Lo que sí hay:** cada operación hace un solo `SaveChanges()` y EF Core lo ejecuta dentro de una **transacción implícita**, así que un guardado individual es atómico (se hace todo o nada).

### El riesgo real: "consultar y después insertar"

`TurnoService.AsignarTurno` (y `ModificarTurno`) hace dos pasos separados:

1. `HaySuperposicion(...)`: consulta si el médico ya tiene un turno que se pisa.
2. `contexto.Turnos.Add(...)` + `SaveChanges()`: inserta.

Entre el paso 1 y el 2 no hay nada que bloquee. Escenario:

| Tiempo | Recepcionista A | Recepcionista B |
|--------|-----------------|-----------------|
| t1 | `HaySuperposicion` → no hay turno a las 10:00 | |
| t2 | | `HaySuperposicion` → tampoco hay |
| t3 | Inserta el turno de las 10:00 | |
| t4 | | Inserta otro turno a las 10:00 |

Resultado: **dos turnos superpuestos** del mismo médico. La base no lo impide: la tabla `Turnos` solo tiene la clave primaria y los índices comunes `IX_Turnos_MedicoId` e `IX_Turnos_PacienteId` (consultado con `sqlcmd`; no existe un índice único por `(MedicoId, Fecha, HoraInicio)`). Y aunque existiera, solo atajaría turnos con **la misma hora de inicio**, no los que se pisan parcialmente.

`sp_AsignarTurno` tiene el mismo patrón: un `IF EXISTS` de superposición seguido de un `INSERT`, **sin transacción y sin `UPDLOCK`/`HOLDLOCK`**.

Como es una agenda de pocos usuarios, la probabilidad es baja, pero es el único lugar donde dos operaciones simultáneas pueden dejar datos inconsistentes.

### Otro caso: dos guardados encadenados

`ExcepcionAgendaService.Agregar` hace **dos** `SaveChanges`: uno para insertar la ausencia y otro (dentro de `CancelarTurnosAfectados`) para cancelar los turnos. Son dos transacciones separadas: si la segunda falla, la ausencia queda guardada y los turnos sin cancelar. Es la única operación de `SGC.Logica` que encadena dos guardados (se revisó el conteo de `SaveChanges` de cada Service).

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

Con `UPDLOCK, HOLDLOCK` el segundo que llega espera al primero y después ve su turno. En C#, `TurnoService.AsignarTurno` ejecutaría `EXEC sp_AsignarTurno` y traduciría el `SqlException` (los `RAISERROR` de severidad 16) en un mensaje. Como además el procedimiento ya valida las ausencias, esto resuelve también el defecto N-3. `ModificarTurno` necesitaría un procedimiento equivalente.

### Historia de `sp_AsignarTurno`

`git log -- SGC.Datos/StoredProcedures/sp_AsignarTurno.sql`:

| Commit | Autor | Fecha | Qué hizo |
|--------|-------|-------|----------|
| `e5bc51c` | Luciano Villordo | 16/10 | Creó el procedimiento (con `DATEPART(WEEKDAY, ...)` para el día) |
| `245e9de` | Nicolás Valdes | 19/10 | Lo corrigió: día de la semana independiente del idioma (`DATEDIFF`), validación de `ExcepcionesAgenda`, solo franjas activas, duración por defecto, mensajes `N'...'` y `CREATE OR ALTER` |

Ese segundo commit es el que agregó la validación de ausencias que `TurnoService` nunca tuvo.

---

## Defectos y deuda conocidos

Consolidado de **todos** los hallazgos de [`01`](01-arquitectura.md#hallazgos), [`02`](02-modelo-de-datos.md), [`03`](03-reglas-de-negocio.md#hallazgos) y [`05`](05-procedimientos-almacenados.md#hallazgos), más los que se encontraron al escribir esta guía. Impacto: **A** = puede dar datos incorrectos o filtrar información; **M** = confunde o complica; **B** = cosmético o de mantenimiento.

### Reglas de negocio y datos

| ID | Hallazgo | Dónde | Imp. | Sugerencia de arreglo |
|----|----------|-------|:----:|-----------------------|
| N-1 | **Carrera al asignar o modificar:** consulta de superposición y luego inserción, sin transacción ni índice único; dos asignaciones simultáneas pueden crear turnos superpuestos | `TurnoService.AsignarTurno`, `ModificarTurno`; `sp_AsignarTurno.sql` | A | Transacción con `UPDLOCK, HOLDLOCK` dentro de `sp_AsignarTurno` y que C# lo invoque (ver sección anterior) |
| N-2 | **`ExcepcionAgendaService.Agregar` usa dos `SaveChanges`:** si falla el segundo, la ausencia queda sin cancelar los turnos | `ExcepcionAgendaService.Agregar` / `CancelarTurnosAfectados` | M | Un solo `SaveChanges` (hacer la cancelación en el mismo contexto antes de guardar) o transacción explícita |
| N-3 | **`AsignarTurno` y `ModificarTurno` ignoran `ExcepcionAgenda`**; solo la pantalla pinta esos horarios como ocupados y solo `sp_AsignarTurno` las valida | `SGC.Logica/TurnoService.cs`; `FormTurnos.CrearHorarios` | A | Validar `ExcepcionesAgenda` en C# (consulta en el mismo contexto) o usar el procedimiento |
| N-4 | `AsignarTurno` no valida que paciente y médico existan y estén activos; `ModificarTurno` tampoco revisa el médico | `TurnoService` | M | Validar `Activo` antes de insertar |
| N-5 | Se puede **modificar o cancelar un turno ya atendido** (`Asistio`/`Ausente`); cancelar un `Asistio` conserva `Monto` y `MedioPago` | `TurnoService.ModificarTurno`/`CancelarTurno`; `FormTurnos.ActualizarAcciones` | M | Rechazar si el estado es `Asistio` o `Ausente`, en el Service y en la pantalla |
| N-6 | **Modificar o eliminar una ausencia no re-evalúa turnos:** modificar no cancela turnos nuevos afectados; eliminar no restituye los cancelados | `ExcepcionAgendaService.Modificar`, `EliminarLogico` | M | Reaplicar `CancelarTurnosAfectados` al modificar; avisar en pantalla al eliminar |
| N-7 | El estado `Pendiente (0)` **nunca se crea**, pero hay contadores, filtros y una lista ("sin confirmar") para él | `AsignarTurno`; `FormTurnos`, `FormInicio`, `ResumenService.ObtenerProximosSinConfirmar` | B | Quitarlo de la UI y mantener el 0 reservado en el enum (ver sección siguiente) |
| N-8 | Editar un paciente cuya obra social fue dada de baja lo guarda como **Particular sin avisar** (deducido) | `FormPacientes.CargarCombos`, `BtnGuardar_Click` | M | Cargar la obra social actual del paciente aunque esté inactiva, o avisar |
| N-9 | La **matrícula duplicada** no se valida en C#; solo la base la rechaza (mensaje técnico) | `MedicoService.Validar` | B | Validar contra médicos activos con mensaje claro |
| N-10 | **No se puede recrear** un médico, paciente, obra social, usuario ni franja dados de baja con los mismos datos únicos (índices sin filtro de `Activo`); solo `Usuario` tiene `Reactivar` | `SGCContext.cs` (índices) | M | Agregar `Reactivar` (como `UsuarioService.Reactivar`) o índices filtrados `WHERE Activo = 1` |
| N-11 | Validaciones de C# y de la base **no idénticas** (DNI 7-8 dígitos vs. 15 caracteres; nacimiento hasta 120 años vs. solo no futuro; email solo en C#) | `PacienteService.Validar`; `SGCContext.cs` | B | Alinear los `CHECK` o documentar la diferencia |
| N-12 | `Turnos.MedioPago` es **texto libre** con el nombre de la obra social; un renombre no actualiza turnos viejos | `FormConfirmarAsistencia` | B | Guardar el `ObraSocialId` o un código de medio de pago |
| N-13 | **16 turnos en estado `Pendiente`** en la base de desarrollo, que viene del script de demo, no de la app | `SGC.Datos/Scripts/carga_masiva_demo.sql` | B | Generar la demo con `Confirmado` si se quiere reflejar el uso real |
| N-14 | `FormAgendaMedico` y `FormRegistrarActividad`: si el usuario no tiene médico vinculado, toman **el primero de la lista** de médicos | `DeterminarMedicoActivo` (ambas) | M | Mostrar un error en vez de asumir un médico |

### Procedimientos y reportes

| ID | Hallazgo | Dónde | Imp. | Sugerencia de arreglo |
|----|----------|-------|:----:|-----------------------|
| R-1 | **`sp_AsignarTurno` no está conectado a nada** (ningún Service ni pantalla lo invoca) | `sp_AsignarTurno.sql`, `TurnoService.cs` | M | Conectarlo (ver N-1) o eliminarlo para no tener dos reglas |
| R-2 | **Los reportes filtran distinto los cancelados:** cuentan cancelados `sp_ReporteTurnosPorPeriodo`, `sp_ReporteAsistencia`, `sp_ReportePacientesPorMes` y `sp_ReporteRendimientoMedicos`; los excluyen `sp_ReporteOcupacionAgenda` y `sp_ReporteTurnosPorObraSocial`; Recaudación solo cuenta `Asistio` | `StoredProcedures/*.sql` | M | Definir un criterio único y aplicarlo con una migración nueva por procedimiento |
| R-3 | **Reporte "Obras sociales" para la Recepcionista:** muestra datos y recaudación de **todos** los médicos | `FormReportes.Estadisticas.cs`; `sp_ReporteTurnosPorObraSocial` | A | Agregar `@MedicoId` al procedimiento o limitar la pestaña al Administrador; ocultar "Total recaudado" a la Recepcionista |
| R-4 | Dos reportes agrupan por la **obra social actual del paciente**, no por lo cobrado | `sp_ReporteRecaudacion`, `sp_ReporteTurnosPorObraSocial` | M | Guardar la obra social usada en el turno al cobrar |
| R-5 | **Texto acoplado SQL ↔ pantalla:** `EstadoNombre = 'Asistió'` se compara por texto | `sp_ReporteTurnosPorPeriodo`; `FormReportes.GenerarTurnos` | B | Devolver también el número de estado y comparar por número |
| R-6 | Procedimientos con filtro de **un solo médico** se ejecutan una vez por médico para la Recepcionista | `ResumenService`, `FormReportes` | B | Parámetro con lista de médicos (tabla de valores) |
| R-7 | `sp_ReporteOcupacionAgenda` no filtra `Medicos.Activo` (deducido) | `sp_ReporteOcupacionAgenda.sql` | B | Agregar el filtro |
| R-8 | El CSV no escapa un `-` inicial (solo `=`, `+`, `@`); **no verificado** cómo lo trata Excel | `FormReportes.EscaparCsv` | B | Agregar `-` a la lista si corresponde |

### Arquitectura, interfaz y mantenimiento

| ID | Hallazgo | Dónde | Imp. | Sugerencia de arreglo |
|----|----------|-------|:----:|-----------------------|
| A-1 | **Cerrar sesión no termina el proceso** (deducido): el primer `FormLogin` queda oculto y manda en el ciclo de vida | `FormLogin.cs`, `FormMenuPrincipal.BtnCerrarSesion_Click` | M | Abrir el login con `ShowDialog` o cerrar/`Application.Exit` según corresponda |
| A-2 | Dos servicios con **datos en memoria**: `HorarioService` (tramos) y `NotificacionService` (avisos; los emails son simulados) | `SGC.Logica/HorarioService.cs`, `NotificacionService.cs` | M | Persistir los avisos o integrar un envío real; documentar que son simulados |
| A-3 | La cadena de conexión por defecto es `Server=.`, no `SQLEXPRESS` | `SGCContextFactory.cs` | B | Mantener el `appsettings.Local.json` por integrante (ya documentado) |
| A-4 | **Tildes inconsistentes:** especialidades y tipos de actividad sembrados sin tildes, `NombreDia` ("Miercoles"), mensajes de los Services sin tildes | `DbSeeder.cs`, `SGCContext.cs`, `AgendaMedico.cs`, `SGC.Logica/*.cs` | B | Unificar a tildes en textos de usuario |
| A-5 | Pantallas de Luciano sin los componentes compartidos y con píxeles fijos (`FormAgendaMedico`, `FormRegistrarActividad`) | `SGC.UI` | B | Migrarlas al patrón de [`06`](06-capa-ui.md) |
| A-6 | `FormHistorialPaciente` (versión modal) no se abre desde el menú: solo corre sin navegador | `FormHistorialPaciente.cs` | B | Eliminarla o usarla de verdad |
| A-7 | **No hay proyecto de pruebas automáticas** en la solución | `SistemaGestionConsultorio.sln` | M | Agregar un proyecto de pruebas para las reglas de `TurnoService` |
| A-8 | El README estaba desactualizado (decía que no había base de datos) | `README.md` | B | **Resuelto en esta rama** (el README ya describe EF Core y SQL Server) |

---

## Qué se puede mejorar para la próxima entrega

Ordenado por prioridad (primero lo que evita datos incorrectos):

1. **Conectar `sp_AsignarTurno` con transacción.** Agregar `UPDLOCK, HOLDLOCK` y `BEGIN TRANSACTION` dentro del procedimiento y hacer que `TurnoService.AsignarTurno` lo invoque. Cierra N-1, N-3 (para asignar) y R-1 de una sola vez.
2. **Validar `ExcepcionAgenda` en C#** también para `ModificarTurno` (y para `AsignarTurno` si se decide no usar el procedimiento). Cierra N-3.
3. **Quitar el estado `Pendiente` de la UI** (contador de Turnos, tarjeta de Inicio, filtro de Reportes, lista "sin confirmar") **manteniendo el valor 0 reservado en el enum `EstadoTurno`**, porque los procedimientos almacenados usan los números 0 a 4 y borrarlo correría los demás. Cierra N-7 y evita la confusión descrita en [`06`](06-capa-ui.md#las-dos-acepciones-de-pendiente).
4. **Validar la matrícula duplicada en `MedicoService.Validar`** con un mensaje claro. Cierra N-9.
5. **Poder reactivar médicos, franjas, pacientes y obras sociales** dados de baja (como `UsuarioService.Reactivar`). Cierra N-10.

Si queda tiempo, en este orden: restringir el reporte de Obras sociales para la Recepcionista (R-3), unificar el `SaveChanges` de la carga de ausencias (N-2), impedir modificar o cancelar turnos ya atendidos (N-5) y mostrar un error cuando un usuario Médico no tiene médico vinculado (N-14).

## Para seguir leyendo

- Reglas y validaciones actuales: [`03-reglas-de-negocio.md`](03-reglas-de-negocio.md)
- Procedimientos y migraciones: [`05-procedimientos-almacenados.md`](05-procedimientos-almacenados.md)
- Patrones de pantalla: [`06-capa-ui.md`](06-capa-ui.md)
