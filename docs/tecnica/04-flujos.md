# Flujos de punta a punta

Acá vas a encontrar los 9 recorridos más importantes del sistema, **de la pantalla a la base y de vuelta**: qué archivo y qué método se ejecuta en cada paso, y por qué el código está armado así. Sirve para entender qué pasa "por debajo" cuando alguien aprieta un botón, y para saber dónde tocar si hay que cambiar algo.

Cada flujo tiene un diagrama de secuencia (las flechas van de arriba hacia abajo en el orden en que ocurren; las punteadas son respuestas), una tabla con el recorrido y un cierre con el **porqué**. Los nombres entre comillas de las franjas de mensajes son los que ve el usuario.

> Todo fue verificado leyendo el código de la rama. Lo que se dedujo sin ejecutar la aplicación se marca como "deducido". Las reglas completas de cada validación están en [`03-reglas-de-negocio.md`](03-reglas-de-negocio.md); acá solo se nombran.

| # | Flujo | Pantalla de entrada |
|---|-------|---------------------|
| a | [Arranque y login](#a-arranque-de-la-app-y-login) | `Program.cs`, `FormLogin` |
| b | [Asignar un turno](#b-asignar-un-turno) | `FormTurnos` |
| c | [Modificar y cancelar un turno](#c-modificar-y-cancelar-un-turno) | `FormTurnos` |
| d | [Confirmar asistencia y cobro](#d-confirmar-asistencia-y-cobro) | `FormTurnos` → `FormConfirmarAsistencia` |
| e | [Atención médica e historial](#e-atención-médica-e-historial-clínico) | `FormAgendaMedico` |
| f | [Cargar una ausencia](#f-cargar-una-ausencia) | `FormExcepcionesAgenda` |
| g | [Generar un reporte](#g-generar-un-reporte) | `FormReportes` |
| h | [Pantalla de Inicio](#h-pantalla-de-inicio) | `FormInicio` |
| i | [Copia de seguridad y restauración](#i-copia-de-seguridad-y-restauración) | `FormBackup` |

**Dos cosas que se repiten en todos los flujos** (explicadas en [`01-arquitectura.md`](01-arquitectura.md)): cada método de un Service abre su propio `SGCContext` con `SGCContextFactory.Crear()` y lo descarta al terminar, y las pantallas nunca tocan la base: siempre pasan por un Service.

---

## a) Arranque de la app y login

![Arranque y login](diagramas/flujo-login.png)

| Paso | Archivo y método | Qué pasa |
|------|------------------|----------|
| 0 | `SGC.UI/Program.cs` → `Main` (antes de todo) | Fija la cultura **es-AR** (`CurrentCulture`, `DefaultThreadCurrentCulture`) y registra los manejadores globales de errores (`ThreadException`, `UnhandledException`): un error no controlado muestra "Ocurrió un error inesperado..." sin cerrar la aplicación. |
| 1 | `SGC.UI/Program.cs` → `Main` | Crea un contexto con `SGCContextFactory.Crear()` (busca `appsettings.Local.json`, si no usa `Server=.`; la cadena se lee una vez y se guarda; un archivo mal formado se informa con un mensaje claro). |
| 2 | `contexto.Database.Migrate()` | Crea la base si no existe y aplica las migraciones que falten (incluidos los procedimientos almacenados). |
| 3 | `SGC.Datos/DbSeeder.cs` → `Sembrar` | Si **no hay ningún médico**, carga obras sociales, médicos, pacientes, usuarios, agenda y turnos de prueba. Si ya hay datos, no hace nada. |
| 4 | `Program.cs` (bloque `catch`) | Si 2 o 3 fallan (por ejemplo SQL Server apagado) muestra un `MessageBox` y **sale sin abrir ninguna pantalla**. |
| 5 | `Application.Run(new FormLogin())` | Abre el login. Desde acá `FormLogin` es la ventana principal del proceso. |
| 6 | `SGC.UI/FormLogin.cs` → `BtnIngresar_Click` | Valida que usuario y clave no estén vacíos. Llama a `new AutenticacionService().Autenticar(...)`. |
| 7 | `SGC.Logica/AutenticacionService.cs` → `Autenticar` | Delega en `UsuarioService.BuscarPorCredenciales` (no tiene lógica propia). |
| 8 | `SGC.Logica/UsuarioService.cs` → `BuscarPorCredenciales` | Busca por `NombreUsuario` **y** `Activo = true`, con `Include` de `Medico` y `MedicosAsignados`. |
| 9 | `SGC.Entidades/PasswordHasher.cs` → `Verificar` | Compara la clave tipeada con el hash guardado (PBKDF2, 100.000 iteraciones, SHA-256). Devuelve `null` si no coincide. |
| 10 | `FormLogin.BtnIngresar_Click` | Si el resultado es `null`, muestra "Usuario o contraseña incorrectos" en la `FranjaMensaje`. Si no, limpia la clave escrita y los avisos de la sesión anterior (`NotificacionService.LimpiarAvisos`), crea `new FormMenuPrincipal(usuario)`, lo muestra y se **oculta** (`Hide()`). |
| 11 | `SGC.UI/FormMenuPrincipal.cs` → constructor | Muestra los botones del rol (`ConfigurarBotonesPorRol`) y abre `FormInicio` (flujo [h](#h-pantalla-de-inicio)). |

**Por qué está armado así**

- **El hash no se compara en SQL.** PBKDF2 con sal aleatoria no se puede traducir a un `WHERE`, así que se busca el usuario por nombre y se verifica en memoria.
- **Mismo mensaje para "no existe" y "clave mala".** El login no revela si el usuario existe.
- **No hay sesión global.** El `Usuario` viaja por constructor: `FormLogin` → `FormMenuPrincipal` → cada pantalla hija. Por eso cada pantalla puede saber el rol sin consultar nada.
- **El cierre de la aplicación y de la sesión están separados.** El primer `FormLogin` sigue siendo la ventana principal del proceso (queda oculto). Cerrar el menú con la X ejecuta `Application.Exit()` (`FormMenuPrincipal.OnFormClosed`); "Cerrar sesión" (`CerrarSesion`) marca `_cerrandoSesion`, vacía los avisos, reutiliza el login oculto (`FormLogin.Reiniciar`) y cierra el menú sin terminar el proceso. Mientras corre un backup o una restauración no se puede cerrar la ventana.

---

## b) Asignar un turno

![Asignar un turno](diagramas/flujo-asignar-turno.png)

Se hace en dos tiempos: primero la pantalla **calcula qué horarios mostrar** y después, al apretar "Asignar", el Service **vuelve a validar todo**.

**Tiempo 1: los horarios libres como botones**

| Paso | Archivo y método | Qué pasa |
|------|------------------|----------|
| 1 | `SGC.UI/FormTurnos.cs` → `ActualizarHorarios` | Se dispara al cambiar médico, fecha o duración. Borra los botones anteriores y llama a `CrearHorarios`. |
| 2 | `SGC.Logica/AgendaMedicoService.cs` → `ObtenerDisponibilidad` | Devuelve un `DisponibilidadDia` (`SGC.Logica/DisponibilidadDia.cs`): las franjas de ese día de la semana y las ausencias activas de esa fecha. |
| 3 | `DisponibilidadDia.MotivoSinAtencion` | Si el médico no atiende ese día, no tiene agenda o tiene una ausencia de día completo, **no se crean botones** y se muestra el motivo en un aviso. |
| 4 | `SGC.Logica/HorarioService.cs` → `GenerarSlotsEnRango` | Corta cada franja en tramos de la duración elegida (15, 20, 30, 45, 60 o 90 min). Los tramos que no entran completos se descartan. |
| 5 | `SGC.Logica/TurnoService.cs` → `ObtenerPorMedicoYFecha` | Trae los turnos activos de ese médico y día con `EXEC sp_ObtenerTurnosPorMedicoYFecha` (ver [`05`](05-procedimientos-almacenados.md)). |
| 6 | `FormTurnos.CrearHorarios` / `CrearChip` | Un botón por tramo: **libre** (borde verde, clic habilitado) u **ocupado** (gris, deshabilitado) si se superpone con un turno, con una ausencia (`HayAusenciaEn`) o si la fecha ya pasó. |

**Tiempo 2: el botón "Asignar"**

| Paso | Archivo y método | Qué pasa |
|------|------------------|----------|
| 7 | `FormTurnos.BtnAsignar_Click` | Verifica que haya paciente, médico, duración y horario elegido; si falta algo lo lista en la franja de mensajes. |
| 8 | `TurnoService.AsignarTurno` | Valida en este orden: duración 5 a 240 min sin cruzar la medianoche, fecha y horario no pasados, el médico atiende (`AgendaMedicoService.MedicoAtiende`), paciente y médico **activos** (`ExigirActivos`), sin ausencia del médico (`ExigirSinAusencia`), sin superposición del médico (`HaySuperposicion`) y **sin otro turno del paciente a la vez** (`ExigirPacienteLibre`). Todo en minutos. Detalle en [`03`](03-reglas-de-negocio.md#2-asignar-un-turno). |
| 9 | `TurnoService.AsignarTurno` | Inserta `Turno` con `Estado = Confirmado` y `Activo = true` y llama a `SaveChanges` (una sola operación). |
| 10 | `FormTurnos.BtnAsignar_Click` | Si el Service lanzó una excepción, muestra el mensaje y **vuelve a dibujar los botones** (el horario pudo ocuparse desde otra pantalla). Si salió bien, recarga la grilla. |
| 11 | `SGC.Logica/NotificacionService.cs` → `AvisarTurno` | "Avisa" al paciente (título "Confirmación"): solo agrega un texto a una lista estática en memoria (no manda ningún email; el mensaje lo aclara). Si falla, el turno ya está guardado. |

**Por qué está armado así**

- **El botón libre/ocupado es una ayuda visual, no la regla.** La regla real vive en `TurnoService.AsignarTurno`; los botones solo evitan que el usuario elija algo inválido.
- **`AsignarTurno` no llama a `sp_AsignarTurno`.** Hace sus propias validaciones en C# (nadie en el código invoca ese procedimiento y hoy tiene menos reglas que el C#; ver [`05`](05-procedimientos-almacenados.md)). Las ausencias las hace cumplir el Service (`ExigirSinAusencia`), no solo la pantalla.
- **La superposición se resuelve en memoria** porque `Turno.HoraFin` es una propiedad calculada que EF no puede traducir a SQL.
- **Hay una carrera posible.** Entre las comprobaciones de superposición (médico y paciente) y el `INSERT` no hay transacción ni índice único: dos recepcionistas que asignen a la vez el mismo horario podrían crear turnos superpuestos. Detalle y arreglo propuesto en [`08-guia-para-modificar.md`](08-guia-para-modificar.md#transacciones-y-concurrencia).
- **Los horarios `Horario` no están en la base.** `HorarioService` guarda sus tramos en una lista estática en memoria; solo sirven para armar los botones.

---

## c) Modificar y cancelar un turno

![Modificar y cancelar](diagramas/flujo-modificar-cancelar.png)

| Paso | Archivo y método | Qué pasa |
|------|------------------|----------|
| 1 | `FormTurnos.DgvTurnos_SelectionChanged` | Al elegir una fila, la tarjeta de la izquierda pasa a **"REPROGRAMAR TURNO"** (`ActualizarModo`): paciente y médico quedan bloqueados, solo se cambia fecha, horario y duración. |
| 2 | `FormTurnos.ActualizarHorarios` | Recalcula los botones **ignorando el propio turno** (`idPropio`), para que el turno pueda "quedarse" en su horario actual. |
| 3 | `FormTurnos.BtnModificar_Click` | Exige un horario libre elegido y llama a `TurnoService.ModificarTurno(id, fecha, hora, duración)`. |
| 4 | `TurnoService.ModificarTurno` | Valida: duración, fecha y horario no pasados, el turno existe y está **`Confirmado`** (uno cancelado, atendido o ausente se rechaza), paciente y médico activos, el médico **del turno** atiende, sin ausencia, sin superposición del médico ni del paciente (excluyendo el propio turno). Actualiza `Fecha`, `HoraInicio` y `DuracionMinutos`. |
| 5 | `NotificacionService.AvisarTurno` | Aviso simulado de "Reprogramación". |
| 6 | `FormTurnos.BtnCancelar_Click` | Pide confirmación con un `MessageBox` y llama a `TurnoService.CancelarTurno(id)`. |
| 7 | `TurnoService.CancelarTurno` | `Estado = Cancelado` **y** `Activo = false`; falla si ya estaba cancelado o si ya fue atendido o marcado como ausente. |

**Por qué está armado así**

- **Cancelar no borra:** el turno queda en la base con `Activo = false` (baja lógica). Por eso la grilla lo oculta salvo que se marque "Mostrar cancelados", y los reportes lo pueden contar o no según el procedimiento (ver [`05`](05-procedimientos-almacenados.md)).
- **No se puede cambiar de médico ni de paciente** al reprogramar (requisito RF#05 citado en el código): para eso se cancela y se asigna uno nuevo.
- **Un turno ya atendido (`Asistio` o `Ausente`) no se puede modificar ni cancelar:** el Service lo rechaza y `FormTurnos.ActualizarAcciones` deshabilita los botones (solo un turno `Confirmado` es editable).

---

## d) Confirmar asistencia y cobro

![Confirmar asistencia](diagramas/flujo-confirmar-asistencia.png)

| Paso | Archivo y método | Qué pasa |
|------|------------------|----------|
| 1 | `FormTurnos.ActualizarAcciones` | El botón se habilita solo si el turno está en estado `Confirmado` **y** su fecha es hoy o anterior. |
| 2 | `FormTurnos.BtnConfirmarAsistencia_Click` | Abre el diálogo modal `new FormConfirmarAsistencia(turno)`. |
| 3 | `SGC.UI/FormConfirmarAsistencia.cs` → constructor | Ofrece como medio de pago "Particular" y, **solo si el médico la acepta**, la obra social del paciente (`TurnoService.MedicoAceptaObraSocialDelPaciente`). |
| 4 | `FormConfirmarAsistencia.CboMedioPago_SelectedIndexChanged` | Sugiere el monto: `CalcularMontoParticular` o `CalcularMontoConObraSocial` (editable). |
| 5 | `FormConfirmarAsistencia.BtnConfirmar_Click` | Valida en pantalla (eligió Asistió/Ausente, medio de pago, monto no negativo), **pregunta "¿Confirma ...?" con "No" como opción por defecto** (la acción no se puede deshacer) y devuelve `Asistio`, `MedioPagoSeleccionado` y `MontoSeleccionado`. `Enter` confirma el diálogo y `Esc` lo cancela. |
| 6 | `TurnoService.ConfirmarAsistencia` | Revalida (turno existe, está `Confirmado`, la fecha no es futura, medio y monto si asistió) y guarda: `Asistio` con medio y monto, o `Ausente` con ambos en `null`. `FormTurnos` muestra "Asistencia confirmada." o "Se registró la ausencia." según el caso. |

**La regla de cobro** (`TurnoService`, métodos estáticos): precio particular del médico; con obra social aceptada por **ese** médico, `precio − precio × cobertura / 100` (con piso en 0). Si el médico no acepta la obra social del paciente, el paciente paga particular. Ejemplo: 12.000 con 70 % de cobertura → 3.600.

**Por qué está armado así**

- **La regla de cobro vive en el Service y no en el diálogo** porque la usan dos pantallas: el diálogo (monto sugerido) y la grilla de turnos (columna "A cobrar (estimado)").
- **Se valida dos veces** (pantalla y Service) a propósito: la pantalla da respuesta rápida y el Service protege la regla aunque otra pantalla lo llame.
- **`Asistio` y `Ausente` son estados finales:** `ConfirmarAsistencia` exige `Confirmado`, así que no se puede confirmar dos veces.
- **`MedioPago` es texto libre:** guarda el nombre de la obra social o "Particular"; si después se renombra la obra social, los turnos viejos no cambian.

---

## e) Atención médica e historial clínico

![Atención médica](diagramas/flujo-atencion-medica.png)

Esta parte es del rol Médico (pantallas de Luciano). **Atender un turno no cambia su estado**: el turno sigue `Confirmado` (o `Asistio`, si recepción ya confirmó la asistencia) y se considera "atendido" porque tiene una `ActividadMedica` activa (`Turno.EstadoAtencion`; sin atención, la columna "Atención clínica" muestra el estado con las mismas etiquetas que el resto de la interfaz: Confirmado, Asistió, Ausente, Cancelado).

| Paso | Archivo y método | Qué pasa |
|------|------------------|----------|
| 1 | `FormMenuPrincipal` → botón "Mi agenda" | Abre `new FormAgendaMedico(usuario, AbrirFormularioHijo)`: se le pasa el método de navegación para poder abrir otras pantallas en el mismo panel. |
| 2 | `FormAgendaMedico.DeterminarMedicoActivo` | El médico sale de `Usuario.MedicoId`. Si el usuario no tiene médico vinculado, cae en el **primero** de `MedicoService.ObtenerTodos()` (comportamiento del código). |
| 3 | `SGC.Logica/AgendaDiariaService.cs` → `ObtenerAgenda` | Trae los turnos del día (con `sp_ObtenerTurnosPorMedicoYFecha`, incluyendo cancelados) y arma `TurnoAgendaItem` con edad, obra social y "Nuevo/Recurrente". |
| 4 | `FormAgendaMedico.CargarTurnos` | Cuenta, filtra (por defecto muestra los **pendientes de atender**) y calcula el próximo paciente con `AgendaDiariaService.ElegirProximo`. |
| 5 | `FormAgendaMedico.BtnAtender_Click` | Bloquea turnos cancelados, ausentes y futuros; abre `new FormRegistrarActividad(turno, usuario, navegador)`. El doble clic en una fila lo dispara, pero ignora el clic en el encabezado (`RowIndex < 0`). |
| 6 | `SGC.UI/FormRegistrarActividad.cs` → `BtnGuardar_Click` | Llama a `ActividadMedicaService.RegistrarOModificarActividad(turno, tipo, motivo, procedimiento, receta)`. |
| 7 | `SGC.Logica/ActividadMedicaService.cs` | Rechaza turno de otro médico, cancelado, ausente o de fecha futura, o motivo vacío; si ya hay una actividad del turno (incluso dada de baja) la **actualiza y reactiva**, si no la **inserta** (una por turno). Un turno admite atención si está `Confirmado` o `Asistio`. |
| 8 | `FormRegistrarActividad.BtnVerHistorial_Click` | Abre `FormHistorialClinico` con ese paciente. Desde el historial, el botón "Registrar atención" vuelve a abrir `FormRegistrarActividad`. |
| 9 | `SGC.UI/FormHistorialClinico.cs` → `CargarHistorialPaciente` | Llama a `ActividadMedicaService.ConsultarFilas(rol, paciente, médico, desde, hasta, tipo)`. |
| 10 | `SGC.Logica/AtencionFila.cs` | Devuelve motivo, procedimiento y receta **solo si el rol es Médico**; para otro rol los campos vienen vacíos. |

**Por qué está armado así**

- **Atención y asistencia son cosas distintas.** La asistencia/cobro la registra la recepción (flujo d); la atención clínica la registra el médico. No se pisan.
- **El navegador (`Action<Form>`) evita ventanas sueltas.** Las pantallas del médico se abren como hijas del menú. Cada una tiene una rama alternativa con `ShowDialog()` (y `FormHistorialPaciente`, la versión modal del historial) que **solo se usa cuando no hay navegador**; en la aplicación real el menú siempre lo pasa, así que esas ramas hoy no se ejecutan (deducido).
- **La seguridad clínica (RNF#07) está en la fila, no en la pantalla:** aunque una grilla quisiera mostrar el motivo, `AtencionFila` ya lo trae vacío para otros roles. Decisión de alcance: el Médico ve la historia clínica **completa** del paciente (incluidas las atenciones de otros médicos); solo la Recepcionista queda fuera de los datos clínicos.
- **Pendiente de atención = `Confirmado` o `Asistio` sin atención cargada.** En el combo de "Registrar actividad" cada turno lleva la marca `[Atendido]` o `[Pendiente]`: acá "Pendiente" significa "sin atención cargada", no un estado de turno (ver [`06-capa-ui.md`](06-capa-ui.md#qué-significa-pendiente-en-la-interfaz)).

---

## f) Cargar una ausencia

![Cargar una ausencia](diagramas/flujo-ausencia.png)

> La fuente (`.json`) y la versión interactiva (`.html`) de este diagrama ya reflejan el guardado único; la imagen `.png` y el `.svg` son anteriores y todavía muestran "Pendiente o Confirmado" y los "dos SaveChanges": pendiente de regenerar la imagen.

| Paso | Archivo y método | Qué pasa |
|------|------------------|----------|
| 1 | `SGC.UI/FormExcepcionesAgenda.cs` → `BtnGuardar_Click` | Arma un `ExcepcionAgenda` (médico, fecha, tipo `DiaCompleto` o `RangoHorario`, horas, motivo) y llama a `Agregar` (o a `Modificar` si había una fila elegida). |
| 2 | `SGC.Logica/ExcepcionAgendaService.cs` → `Validar` | Médico existente y activo, fecha no pasada (hoy sí), motivo obligatorio, horas coherentes si es rango y **sin otra ausencia superpuesta** del mismo médico y fecha. |
| 3 | `ExcepcionAgendaService.Agregar` | Agrega la ausencia al contexto (todavía sin guardar). |
| 4 | `ExcepcionAgendaService.CancelarTurnosAfectados` | Busca los turnos **activos** del médico en esa fecha en estado `Confirmado`; si la ausencia es de rango, solo los que se superponen. |
| 5 | `CancelarTurnosAfectados` | Los pasa a `Cancelado` con `Activo = false` en el mismo contexto y **guarda todo con un único `SaveChanges`** (la ausencia y las cancelaciones, o nada). Recién después registra un aviso simulado al paciente por cada turno cancelado (`AvisarCancelaciones`). |
| 6 | `FormExcepcionesAgenda.BtnGuardar_Click` | Muestra "Excepción guardada correctamente. Se cancelaron N turno(s)...". |

**Qué pasa con los turnos existentes**

| Turno del médico ese día | Efecto |
|--------------------------|--------|
| `Confirmado` dentro de la ausencia | Se cancela solo, y se avisa (simulado) |
| `Confirmado` fuera del rango horario | No se toca |
| `Asistio` / `Ausente` | No se toca (ya están resueltos) |
| Ya cancelado | No se toca |

**Por qué está armado así**

- **Un turno en una ausencia deja de tener sentido**, por eso se cancela en el momento y no se deja para que alguien lo note.
- **Es un solo `SaveChanges`:** si algo falla no queda una ausencia guardada con los turnos sin cancelar. Ver [`08`](08-guia-para-modificar.md#transacciones-y-concurrencia).
- **Solo `Agregar` cancela turnos.** `Modificar` y `EliminarLogico` no vuelven a revisar nada: editar una ausencia no cancela turnos nuevos que queden dentro, y eliminarla no restituye los ya cancelados.
- **Las ausencias también bloquean turnos nuevos:** la pantalla las pinta como ocupadas (`DisponibilidadDia`, flujo b) y `TurnoService.AsignarTurno` y `ModificarTurno` las rechazan aunque se llamen sin pasar por la pantalla.

---

## g) Generar un reporte

![Generar un reporte](diagramas/flujo-reporte.png)

El diagrama muestra la pestaña "Turnos"; todas siguen el mismo camino. El módulo completo está en [`07-reportes.md`](07-reportes.md).

| Paso | Archivo y método | Qué pasa |
|------|------------------|----------|
| 1 | `SGC.UI/FormReportes.cs` → `Generar(vista)` | Valida los filtros (`ValidarFiltros`: desde ≤ hasta, y máximo 366 días en Ocupación), pone el cursor de espera y deshabilita el botón. |
| 2 | `FormReportes.GenerarTurnos` (u otro `Generar*`) | Decide qué médico filtrar según el rol (`MedicoSeleccionado`; `MedicosAConsultar` expande "Todos mis médicos" a los asignados) y llama al método de `ReporteService`. El Administrador no llega acá: no tiene Reportes. |
| 3 | `SGC.Logica/ReporteService.cs` → `ObtenerTurnosPorPeriodo` | Valida el período y ejecuta `EXEC sp_ReporteTurnosPorPeriodo` con `SqlQueryRaw<ReporteTurnoFila>`. |
| 4 | `SGC.Datos/StoredProcedures/sp_ReporteTurnosPorPeriodo.sql` | Hace el `SELECT` con los `JOIN` y devuelve las filas. |
| 5 | `FormReportes.MostrarDatos` | Asigna la lista como `DataSource` de la grilla; arma el resumen de abajo. En Recaudación y Ocupación también actualiza el gráfico (`ActualizarGrafico`). |
| 6 | `FormReportes.ExportarCsv` | Con el botón "Exportar a CSV": `SaveFileDialog` (nombre sugerido con pestaña, médico y opción elegidos, p. ej. `reporte_turnos_todos_mis_medicos_confirmado_20261001_20261031.csv`) y `EscribirCsv` (UTF-8 con BOM, separador `;`, celdas que parecen fórmula con `'` delante). |

**Por qué está armado así**

- **La agregación vive en la base** (procedimientos) y la pantalla solo dibuja: así el cálculo es el mismo para cualquier pantalla y no se traen miles de filas para sumarlas en C#.
- **Se guarda `Encabezados` y `FilasExportacion` al generar:** el CSV exporta exactamente lo que se ve, sin volver a consultar.
- **El alcance por rol lo decide la pantalla** (ver [`07`](07-reportes.md)): el Service y el procedimiento solo filtran por el `medicoId` que reciben. Desde esta versión Recaudación, Obras sociales y Rendimiento también lo aceptan (`@MedicoId`, opcional). Si la Recepcionista elige "Todos mis médicos", la pantalla ejecuta el procedimiento **una vez por médico asignado** y une o suma los resultados, porque el procedimiento recibe un solo médico.
- **Los filtros invalidan el resultado:** al cambiar un filtro (fechas, médico o la opción de agrupación) de una pestaña ya generada, `MarcarDesactualizado` vacía la grilla, el resumen y el gráfico, deshabilita **Exportar** y avisa "Los filtros cambiaron. Pulse Generar para actualizar." (las pestañas por año se recalculan solas). Así el CSV nunca sale con datos de otros filtros.
- **Errores:** un `ArgumentException` (por ejemplo, fechas invertidas) se muestra tal cual; cualquier otra excepción se muestra como "No se pudo generar el reporte..." y limpia el resultado.

---

## h) Pantalla de Inicio

![Pantalla de Inicio](diagramas/flujo-inicio.png)

| Paso | Archivo y método | Qué pasa |
|------|------------------|----------|
| 1 | `FormMenuPrincipal.CargarPantallaInicialPorDefecto` | Todos los roles arrancan en Inicio: `new FormInicio(usuario)`. |
| 2 | `SGC.UI/FormInicio.cs` → `Cargar` | Según el rol llama a `CargarAdministrador`, `CargarMedico` o `CargarRecepcionista`. El Administrador ve el estado del sistema; no ve dinero. |
| 3 | `SGC.Logica/ResumenService.cs` → `ObtenerResumen` (Médico) | `EXEC sp_ResumenDelDia @Fecha, @MedicoId` y devuelve **una fila** (`ResumenDelDiaFila`) con los contadores del día, la semana y el mes. |
| 4 | `ResumenService.ObtenerResumenDeMedicos` | Para la Recepcionista: llama al procedimiento una vez por médico asignado y suma con `ResumenDelDiaFila.Sumar`. Una lista vacía da todo en cero. |
| 5 | `ResumenService.ObtenerProximoPaciente` / `ObtenerProximosTurnos` | Consultas con EF para el próximo paciente (Médico) y la lista "Próximos turnos" (Recepcionista): turnos `Confirmado` del día que todavía no empezaron (si la fecha es hoy), ordenados por hora. |
| 6 | `NotificacionService.ObtenerAvisos` / `ObtenerAvisosDeMedico` (Recepcionista y Médico; el Administrador no los ve) | Muestra los avisos simulados **de esta sesión**: están en una lista en memoria. |
| 7 | `FormInicio.AgregarEstadoDelSistema` → `SistemaService.ObtenerSalud` (solo Administrador) | Consultas EF de solo lectura: usuarios activos por rol, médicos sin horarios, médicos sin recepcionista y obras sociales activas. Los contadores en cero se pintan normal; los que indican algo por corregir, en advertencia. |
| 8 | `FormInicio.CrearTarjetaUltimoBackup` → `BackupService.ObtenerUltimoBackup` (solo Administrador) | `MAX(backup_finish_date)` de `msdb`. Advertencia si nunca hubo copia, si no se pudo consultar o si tiene más de 7 días. |

**Por qué está armado así**

- **`FormInicio` no tiene Designer:** toda la interfaz se arma en código (ver [`06-capa-ui.md`](06-capa-ui.md)). Es la excepción al patrón normal y se hizo para que el diseñador de Visual Studio no pueda borrarla.
- **Los números del día vienen de un solo procedimiento** para no hacer cinco consultas separadas. El estado del sistema del Administrador no: son contadores de configuración, y por eso salen de EF.
- **El botón "Actualizar"** vuelve a ejecutar `Cargar`; la pantalla no se refresca sola.
- **Las tarjetas del día** son Confirmados, Asistieron, Ausentes y Cancelados; el estado `Pendiente` ya no existe, así que no hay tarjeta "Pendientes".

---

## i) Copia de seguridad y restauración

![Copia de seguridad y restauración](diagramas/flujo-backup.png)

Solo el Administrador. Dos operaciones en la misma pantalla: **crear una copia** (con verificación) y **restaurar** una existente. Las reglas están en [`03`](03-reglas-de-negocio.md#12-copia-de-seguridad-y-restauración).

> Probado solo contra una base de prueba (crear, verificar y restaurar); falta la prueba desde la pantalla. El diagrama de esta sección (`flujo-backup`) es anterior a la validación del `.bak` y a la migración posterior: la tabla de abajo es la que está al día.

| Paso | Archivo y método | Qué pasa |
|------|------------------|----------|
| 1 | `SGC.UI/FormMenuPrincipal.cs` → `btnBackup.Click` | Abre `new FormBackup()` como hijo. El botón solo es visible para el Administrador. |
| 2 | `SGC.UI/FormBackup.cs` → `CargarInicialAsync` | En un hilo aparte (`Task.Run`): `BackupService.ObtenerCarpetaPorDefecto` (`SERVERPROPERTY('InstanceDefaultBackupPath')`) para sugerir la carpeta, y `ObtenerHistorial` (últimas 20 copias de `msdb`). |
| 3 | `FormBackup.CrearCopiaAsync` → `BackupService.RealizarBackup` | Arma la ruta `carpeta\SGC_yyyyMMdd_HHmmss.bak` y ejecuta `BACKUP DATABASE [base] TO DISK = @ruta WITH INIT, CHECKSUM, STATS = 10`. |
| 4 | `RealizarBackup` | Verifica con `RESTORE VERIFYONLY FROM DISK = @ruta WITH CHECKSUM`. Si falla, la copia existe pero queda `Verificado = false`. |
| 5 | `RealizarBackup` → `ObtenerTamano` | Lee el tamaño de `msdb.dbo.backupset`; si no puede, el tamaño queda en blanco. |
| 6 | `FormBackup.CrearCopiaAsync` | Muestra "Copia creada y verificada" (o el error de verificación) y recarga el historial. |
| 7 | `FormBackup.ElegirArchivo` / `UsarSeleccionada` | Para restaurar se elige un `.bak` con el diálogo o se toma una fila del historial. |
| 8 | `FormBackup.RestaurarAsync` → `BackupService.ObtenerInfoBackup` | Lee el encabezado del `.bak` (`RESTORE HEADERONLY` desde `master`, sin tocar la base). Rechaza un archivo que no sea una copia **completa** o que sea de **otra base** (`DatabaseName` distinto del actual). Muestra un `MessageBox` con la base de origen, el servidor y la fecha de la copia (por defecto "No"). Solo si el usuario confirma, sigue. |
| 9 | `BackupService.Restaurar` | `SqlConnection.ClearAllPools()`, conecta a **`master`**, `RESTORE VERIFYONLY` del archivo, `ALTER DATABASE ... SET SINGLE_USER WITH ROLLBACK IMMEDIATE`. |
| 10 | `Restaurar` | `RESTORE DATABASE [base] FROM DISK = @ruta WITH REPLACE, STATS = 10`. El `finally` siempre ejecuta `SET MULTI_USER` y vuelve a vaciar los pooles. |
| 11 | `BackupService.MigrarBase` | Ejecuta `Migrate()` sobre la base restaurada (la copia puede ser de una versión anterior). Nunca lanza: si falla, la restauración igual se hizo y el resultado lo informa. |
| 12 | `FormBackup.RestaurarAsync` | Si la migración falló, muestra el error y **no** vuelve al login (pide cerrar la aplicación). Si salió bien, avisa y llama a `FormMenuPrincipal.CerrarSesion`: la sesión termina para empezar con los datos restaurados. Mientras tanto el menú lateral y el cierre de la ventana están bloqueados (`BloquearPorOperacion`). |

**Por qué está armado así**

- **Lo hace SQL Server, no la aplicación.** El `.bak` lo escribe y lo lee la **cuenta de servicio de SQL Server**: la carpeta tiene que existir en el servidor y esa cuenta necesita permiso. Una carpeta de tu usuario de Windows puede fallar aunque exista (errores 3201/3203/3013, traducidos a un mensaje que lo explica).
- **Se restaura desde `master`**, porque no se puede reemplazar una base desde una conexión que está usando esa misma base. Por eso `SINGLE_USER ... ROLLBACK IMMEDIATE` (corta las demás conexiones) y el `finally` que garantiza volver a `MULTI_USER`.
- **Se verifica siempre,** al crear y antes de restaurar: una copia que no se puede leer no sirve cuando hace falta. Además se comprueba que el `.bak` sea de **esta** base antes de reemplazarla.
- **Las operaciones corren en `Task.Run`** y la pantalla deshabilita los botones mientras tanto, para que la ventana no se congele; el `CommandTimeout` es 0 (sin límite) porque una copia grande puede tardar minutos.
- **No hay tabla propia de copias:** el historial y la tarjeta "Última copia de seguridad" del Inicio salen de `msdb.dbo.backupset`. Si el usuario de conexión no puede leer `msdb`, la grilla muestra el motivo en vez de fallar.
- **Las copias son manuales:** no hay una tarea programada que las genere; el Inicio del Administrador avisa si la última tiene más de 7 días.

---

## Para seguir leyendo

- Las pantallas, sus servicios y el patrón visual: [`06-capa-ui.md`](06-capa-ui.md)
- El módulo de reportes por dentro: [`07-reportes.md`](07-reportes.md)
- Recetas para modificar y defectos conocidos: [`08-guia-para-modificar.md`](08-guia-para-modificar.md)
