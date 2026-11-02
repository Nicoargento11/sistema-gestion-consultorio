# Reglas de negocio

Acá vas a encontrar **todas las reglas que hoy implementa el código** de `SGC.Logica` (y algunas de la UI que completan el cuadro), cada una con el archivo y el método donde viven. Sirve para responder "¿por qué no me deja hacer esto?" y "¿dónde cambio esta regla?" sin tener que leer los Services enteros.

Las reglas se verificaron leyendo el código. Cuando una regla vive en la UI o en la base y no en `SGC.Logica`, se aclara.

## Índice rápido

| # | Tema | Servicio principal |
|---|------|--------------------|
| 1 | Ciclo de vida del turno | `TurnoService` |
| 2 | Asignar un turno | `TurnoService.AsignarTurno` |
| 3 | Modificar y cancelar (los turnos atendidos no se tocan) | `TurnoService.ModificarTurno` / `CancelarTurno` |
| 4 | Confirmar asistencia | `TurnoService.ConfirmarAsistencia` |
| 5 | Regla de cobro | `TurnoService.CalcularMonto*` |
| 6 | Agenda semanal y disponibilidad | `AgendaMedicoService`, `DisponibilidadDia` |
| 7 | Ausencias (excepciones de agenda) | `ExcepcionAgendaService` |
| 8 | Usuarios, roles y alcance | `UsuarioService` + UI |
| 9 | Actividad médica (historial clínico) | `ActividadMedicaService` |
| 10 | Validaciones de pacientes, médicos, obras sociales | `PacienteService`, `MedicoService`, `ObraSocialService` |
| 11 | Diferencias entre C# y los procedimientos | — |
| 12 | Copia de seguridad y restauración | `BackupService` |
| 13 | Bajas con control | `MedicoService`, `PacienteService`, `AgendaMedicoService`, `ObraSocialService`, `UsuarioService` |
| 14 | Sesión y arranque | `Program.cs`, `FormLogin`, `FormMenuPrincipal` |

---

## 1. Ciclo de vida del turno

`EstadoTurno` (`SGC.Entidades/Turno.cs`): 1 Confirmado, 2 Cancelado, 3 Asistio, 4 Ausente.

```
            AsignarTurno
   (nada) ───────────────►  Confirmado (1) ──ConfirmarAsistencia(true)──►  Asistio (3)
                               │   │
                               │   └────ConfirmarAsistencia(false)──────►  Ausente (4)
                               │
                               ├────CancelarTurno──────────────────────►  Cancelado (2)
                               └────Agregar ausencia (ExcepcionAgenda)──►  Cancelado (2)
```

| Transición | Quién la dispara | Dónde |
|-----------|------------------|-------|
| Nace **Confirmado** | Recepcionista, pantalla Turnos | `TurnoService.AsignarTurno` (`Estado = EstadoTurno.Confirmado`) |
| Confirmado → Asistio / Ausente | Recepcionista, diálogo "Confirmar asistencia" | `TurnoService.ConfirmarAsistencia` |
| Confirmado → Cancelado | Recepcionista, botón Cancelar | `TurnoService.CancelarTurno` |
| Confirmado → Cancelado (masivo) | Administrador, al cargar una ausencia | `ExcepcionAgendaService.Agregar` → `CancelarTurnosAfectados` |

Cancelar deja `Estado = Cancelado` **y** `Activo = false` (las dos cosas, en `CancelarTurno` y en `CancelarTurnosAfectados`).

**Los turnos atendidos son inmutables.** `Asistio` y `Ausente` son estados finales: no se pueden modificar, cancelar ni volver a confirmar (ver secciones 3 y 4). La pantalla además deshabilita los botones (`FormTurnos.ActualizarAcciones`: solo un turno `Confirmado` es editable).

**Sobre el estado `Pendiente`:** se eliminó (el sistema nunca lo asignaba). "Turno por atender" es ahora simplemente un turno `Confirmado` (sin atender), en `PacienteService.ObtenerListado`, `ResumenService.ObtenerProximoPaciente` y `ExcepcionAgendaService.CancelarTurnosAfectados`.

**"Pendiente de atención" (agenda del médico):** es un turno **activo**, `Confirmado` **o `Asistio`**, sin atención clínica cargada (`AgendaDiariaService.EsPendienteDeAtencion`). Un turno con la asistencia ya confirmada por recepción sigue figurando como pendiente hasta que el médico carga la atención. `Ausente` y `Cancelado` no cuentan. El "próximo paciente" (`ElegirProximo`) usa la misma regla.

---

## 2. Asignar un turno

`SGC.Logica/TurnoService.cs`, `AsignarTurno(paciente, medico, fecha, horaInicio, duracionMinutos = 30)`. Las validaciones corren **en este orden** y la primera que falla corta con una excepción:

| # | Regla | Excepción / mensaje |
|---|-------|---------------------|
| 1 | La duración debe estar **entre 5 y 240 minutos** y el turno **no puede cruzar la medianoche**: se suma en minutos (`Horas.Minutos`), no con `TimeOnly`, porque `23:30 + 45 min` daría `00:15` y pasaría las validaciones (`ValidarDuracion`). | `ArgumentException`: "La duración del turno debe estar entre 5 y 240 minutos." / "El turno no puede terminar después de las 24:00 ..." |
| 2 | La fecha no puede ser anterior a hoy, y si es hoy la hora de inicio no puede haber pasado (`EsHorarioPasado`). | `ArgumentException`: "No se puede asignar un turno en una fecha pasada." / "... en un horario que ya pasó." |
| 3 | El médico debe atender ese día y horario: debe existir una franja **activa** de `AgendasMedico` del mismo médico y día de la semana que **contenga entero** el turno (comparado en minutos). | `InvalidOperationException`: "El profesional no atiende el {día} en el horario ..." (`AgendaMedicoService.MedicoAtiende`) |
| 4 | Paciente y médico deben estar **activos**. Se vuelven a leer de la base: los objetos que llegan de la pantalla pueden estar desactualizados (`ExigirActivos`). | `InvalidOperationException`: "El paciente está dado de baja ..." / "El médico está dado de baja ..." |
| 5 | **No puede caer dentro de una ausencia activa del médico** (día completo o rango que se superponga) (`ExigirSinAusencia` → `ExcepcionAgendaService.BuscarAusencia`). | `InvalidOperationException`: "El Dr./Dra. {apellido} tiene una ausencia cargada el {fecha} ({detalle}: {motivo}) ..." |
| 6 | No puede superponerse con otro turno **activo** del mismo médico y fecha (`HaySuperposicion`, en minutos). Los turnos que solo se tocan en el borde no se consideran superpuestos. | `InvalidOperationException`: "El Dr./Dra. {apellido} ya tiene un turno asignado ..." |
| 7 | **El paciente no puede tener dos turnos a la vez**, aunque sean con médicos distintos: se rechaza si se superpone con otro turno activo no cancelado del mismo paciente y fecha (`ExigirPacienteLibre`). | `InvalidOperationException`: "El paciente ya tiene un turno el {fecha} de {hh:mm} a {hh:mm} con el Dr./Dra. ..." |
| 8 | Si todo está bien, inserta `Turno` con `Estado = Confirmado` y `Activo = true`. | — |

**Por qué la superposición se resuelve en memoria:** `HoraFin` es una propiedad calculada (no es columna), así que EF no puede traducirla a SQL. `HaySuperposicion` y `ExigirPacienteLibre` traen los turnos del día (consulta chica) y comparan los rangos en minutos con `Horas.Superpone`.

**Lo que `AsignarTurno` NO valida** (verificado leyendo el método): que el médico acepte la obra social del paciente (no hace falta: si no la acepta, se cobra como particular). Tampoco hay protección contra dos asignaciones **simultáneas** (ver Transacciones y concurrencia).

### Qué hace la pantalla para ayudar (`SGC.UI/FormTurnos.cs`, `CrearHorarios`)

La pantalla consulta las ausencias antes de ofrecer horarios; además las hace cumplir el Service (regla 5):

1. `AgendaMedicoService.ObtenerDisponibilidad(medicoId, fecha)` devuelve los bloques del día y las ausencias activas de esa fecha.
2. `DisponibilidadDia.MotivoSinAtencion` explica por qué no hay turnos (ausencia de día completo, médico sin agenda, o no atiende ese día).
3. `HorarioService.GenerarSlotsEnRango` corta cada bloque en slots de la duración elegida.
4. Cada slot se pinta ocupado si hay un turno superpuesto o `disponibilidad.HayAusenciaEn(inicio, fin)`.

---

## 3. Modificar y cancelar un turno

Los **turnos atendidos (`Asistio`, `Ausente`) y los cancelados no se pueden modificar ni cancelar**: solo un turno `Confirmado` es editable.

| Regla | Dónde | Detalle |
|-------|-------|---------|
| Duración 5 a 240 min y sin cruzar la medianoche | `ModificarTurno` → `ValidarDuracion` | Igual que al asignar |
| La nueva fecha no puede ser pasada ni un horario de hoy que ya pasó | `ModificarTurno` | `ArgumentException` |
| El turno debe existir | `ModificarTurno` | "El turno que intenta modificar no existe." |
| Solo se modifica un turno `Confirmado` | `ModificarTurno` | "No se puede modificar un turno cancelado." / "No se puede modificar un turno que ya fue atendido o marcado como ausente." |
| Paciente y médico del turno deben estar activos | `ModificarTurno` → `ExigirActivos` | `InvalidOperationException` |
| Debe caer en una franja del médico (misma regla que al asignar) | `ModificarTurno` → `MedicoAtiende` | Usa el médico **del turno**, no se puede cambiar de médico |
| No debe caer dentro de una ausencia activa del médico | `ModificarTurno` → `ExigirSinAusencia` | Igual que al asignar |
| No debe superponerse con otro turno activo del médico (excluye el propio) | `ModificarTurno` → `HaySuperposicion(..., excluirTurnoId: turnoId)` | |
| El paciente no puede quedar con dos turnos a la vez (excluye el propio) | `ModificarTurno` → `ExigirPacienteLibre` | |
| Solo cambia `Fecha`, `HoraInicio` y `DuracionMinutos` | `ModificarTurno` | El estado no se toca |
| El turno debe existir y estar `Confirmado` | `CancelarTurno` | "Ese turno ya estaba cancelado." / "No se puede cancelar un turno que ya fue atendido o marcado como ausente." |
| Cancelar = `Estado = Cancelado` + `Activo = false` | `CancelarTurno` | |

**Regla de pantalla** (refuerza la del Service): en `FormTurnos.ActualizarAcciones`, Modificar y Cancelar se habilitan solo para un turno `Confirmado`.

**Avisos al paciente:** después de asignar, reprogramar o cancelar, `FormTurnos` llama a `NotificacionService.AvisarTurno` con los títulos "Confirmación", "Reprogramación" y "Cancelación". Es una simulación: agrega un texto a una lista estática en memoria (se muestra en el Inicio); no manda ningún email, y el mensaje que ve el personal lo aclara. Si el paciente no tiene email válido, el aviso devuelve un mensaje de que no fue posible, pero el turno se guarda igual.

---

## 4. Confirmar asistencia

`TurnoService.ConfirmarAsistencia(turnoId, asistio, medioPago, monto)`:

| # | Regla | Mensaje |
|---|-------|---------|
| 1 | El turno debe existir | "El turno no existe." |
| 2 | Solo se confirma un turno en estado **Confirmado** | "Solo se puede confirmar asistencia de un turno en estado Confirmado." |
| 3 | La fecha del turno no puede ser futura (`turno.Fecha > hoy` falla; **hoy sí se puede**, aunque la hora no haya llegado: no es un requisito) | "No se puede confirmar la asistencia de un turno que todavía no llegó a su fecha." |
| 4a | Si `asistio = true`: el medio de pago es obligatorio | `ArgumentException` |
| 4b | Si `asistio = true`: el monto es obligatorio y no puede ser negativo (**cero sí es válido**) | `ArgumentException` |
| 5a | `asistio = true` → `Estado = Asistio`, guarda `MedioPago` y `Monto` | |
| 5b | `asistio = false` → `Estado = Ausente`, `MedioPago = null`, `Monto = null` | |

Lo que además hace la pantalla (`SGC.UI/FormConfirmarAsistencia.cs`):

- Ofrece como medios de pago: **"Particular"** siempre, y **el nombre de la obra social del paciente solo si el médico la acepta** (ver sección 5).
- Al elegir un medio de pago, **sugiere el monto** (editable): precio particular, o precio menos la cobertura.
- La base respalda el monto no negativo con `CK_Turno_Monto`.
- **Pide confirmación antes de la acción irreversible:** "¿Confirma que el paciente asistió? ..." o "¿Confirma que el paciente estuvo ausente? ...", con "No" como botón por defecto (`ConfirmarAccion`). `Enter` confirma el diálogo y `Esc` lo cancela (`AcceptButton`/`CancelButton`).
- `FormTurnos` informa el resultado según el caso: "Asistencia confirmada." o "Se registró la ausencia."

---

## 5. Regla de cobro

Métodos estáticos de `SGC.Logica/TurnoService.cs`. Los usan el diálogo de asistencia y la grilla de turnos. El turno debe venir con `Paciente.ObraSocial` y `Medico.ObrasSocialesAceptadas` cargados (`ObtenerTodos`/`ObtenerPorId` ya hacen el `Include`).

| Método | Qué calcula |
|--------|-------------|
| `MedicoAceptaObraSocialDelPaciente(turno)` | `true` solo si el paciente tiene obra social **y** esa obra social está en `Medico.ObrasSocialesAceptadas` de **este** médico |
| `CalcularMontoParticular(turno)` | `Medico.PrecioConsultaParticular` (0 si no hay médico) |
| `CalcularMontoConObraSocial(turno)` | `precio − precio × cobertura / 100`, con piso en 0 (`Math.Max`) |
| `CalcularMontoEstimado(turno)` | Con obra social si el médico la acepta; si no, precio particular |

Ejemplo: precio 12.000, obra social con 70 % de cobertura, médico que la acepta → 12.000 − 8.400 = **3.600**. Si el médico **no** acepta esa obra social, el paciente paga **12.000** como particular.

El paciente sin obra social (`ObraSocialId` nulo) es "Particular". La cobertura se toma de `ObraSocial.PorcentajeCobertura` (decimal, 0 a 100, respaldado por `CK_ObraSocial_Porcentaje`).

---

## 6. Agenda semanal y disponibilidad

`SGC.Logica/AgendaMedicoService.cs` (tabla `AgendasMedico`).

### Alta y modificación de franjas

| Regla | Método | Mensaje |
|-------|--------|---------|
| El médico debe existir y estar activo | `ValidarDatosBasicos` | "Debe seleccionar un médico válido." |
| `HoraFin` debe ser posterior a `HoraInicio` | `ValidarDatosBasicos` | "La hora de fin debe ser posterior a la hora de inicio." |
| No puede superponerse con otra franja **activa** del mismo médico y día de la semana | `ValidarSuperposicion` | "El rango horario seleccionado se superpone ..." |
| Si existe una franja idéntica dada de baja, **se reactiva** en vez de insertar otra | `Agregar` | Evita chocar con el índice único |
| La baja es lógica (`Activo = false`) y se rechaza si deja sin horario de atención a turnos confirmados futuros | `EliminarLogico`, `Modificar` → `ExigirSinTurnosHuerfanos` | Ver sección 13 |

La base refuerza con `CK_AgendaMedico_Rango` (`HoraFin > HoraInicio`) y un índice único sobre (médico, día, hora inicio, hora fin).

### Consultas

| Método | Para qué |
|--------|----------|
| `MedicoAtiende(medicoId, dia, horaInicio, duracion)` | Responde si el turno entra completo en una franja activa; compara en **minutos** para no equivocarse con turnos que cruzan la medianoche (lo usan `AsignarTurno` y `ModificarTurno`) |
| `ObtenerResumenSemanal` | Una fila por médico **activo** con sus franjas activas y horas semanales (`AgendaSemanalFila`) |
| `ObtenerDisponibilidad(medicoId, fecha)` | Devuelve `DisponibilidadDia` (solo lectura) |

### `DisponibilidadDia` (`SGC.Logica/DisponibilidadDia.cs`)

Objeto de solo lectura que junta, para un médico y una fecha: los **bloques** de ese día de la semana, las **ausencias** activas de esa fecha y los **días que atiende** (ordenados lunes a domingo).

| Miembro | Qué responde |
|---------|--------------|
| `AusenteTodoElDia` | ¿Hay una ausencia `DiaCompleto`? |
| `HayAusenciaEn(inicio, fin)` | ¿El tramo cae total o parcialmente en una ausencia? (día completo → siempre sí; rango → superposición) |
| `MotivoSinAtencion(nombreMedico)` | Texto que explica por qué no hay turnos: ausencia de día completo (con el motivo), médico sin horarios cargados, o "no atiende los {día}. Atiende: ..."; `null` si sí atiende |
| `DescripcionAusenciasParciales()` | Texto de las ausencias por rango del día |

Es solo ayuda para la pantalla: **no** modifica las reglas de `AsignarTurno` ni de `MedicoAtiende` (las ausencias las hace cumplir `TurnoService` por su cuenta, sección 2).

---

## 7. Ausencias (excepciones de agenda)

`SGC.Logica/ExcepcionAgendaService.cs` (tabla `ExcepcionesAgenda`). Solo el administrador accede desde el menú.

### Validaciones (`Validar`, en alta y en modificación)

| Regla | Mensaje |
|-------|---------|
| El médico debe existir y estar activo | "Debe seleccionar un médico válido." |
| La fecha no puede ser pasada (**hoy sí**). Al **modificar**, solo se exige si cambió el rango (médico, fecha, tipo u horas): así se puede corregir el motivo de una ausencia que ya pasó | "No se puede cargar una excepción en una fecha pasada." |
| El motivo es obligatorio (máximo 200 caracteres) | "Debe indicar el motivo de la ausencia." |
| Si el tipo es `RangoHorario`: hora de inicio y fin obligatorias, y `HoraFin > HoraInicio` | mensajes específicos |
| **No puede superponerse con otra ausencia activa del mismo médico y fecha** (un día completo se superpone con cualquier otra del día) | "El médico ya tiene una ausencia cargada el {fecha} (...) que se superpone con esta." |

`DiaCompleto` no usa horas; la base lo exige con `CK_ExcepcionAgenda_Tipo`.

### Las ausencias bloquean turnos

Con una ausencia activa, `TurnoService.AsignarTurno` y `ModificarTurno` **rechazan** cualquier turno que se superponga con ella (sección 2, regla 5). Antes solo la pantalla pintaba esos horarios como ocupados.

### Efecto sobre los turnos existentes (solo al **agregar**) y atomicidad

`Agregar` marca la ausencia y los turnos afectados **en el mismo contexto y los guarda con un único `SaveChanges`** (una sola transacción): o se guardan la ausencia y las cancelaciones, o no se guarda nada. Los avisos al paciente se registran **después** de guardar.

1. `CancelarTurnosAfectados` busca turnos **activos** del médico en esa fecha con estado `Confirmado`.
2. Si la ausencia es `DiaCompleto`, afecta a todos; si es `RangoHorario`, solo a los que se superponen (en minutos).
3. Los pasa a `Cancelado` + `Activo = false` (sin guardar todavía).
4. Tras el `SaveChanges`, `AvisarCancelaciones` registra un aviso por paciente ("... por ausencia del profesional") y se devuelve la lista para que la pantalla informe.

`Modificar` y `EliminarLogico` **no** vuelven a revisar turnos: modificar una ausencia no cancela turnos que queden afectados, y eliminar una ausencia no restaura los turnos que ya se cancelaron (los turnos nuevos sí vuelven a poder asignarse).

### Listado (`ObtenerListado`)

Arma `ExcepcionFila`: estado respecto de hoy (`Vigente` si es hoy, `Proxima` si es futura, `Pasada` si ya fue) y `TurnosAfectados`, la cantidad de turnos activos y no cancelados que todavía se superponen con la ausencia. Ordena vigentes y próximas primero (la más cercana arriba), después las pasadas (la más reciente arriba). Trae todos los turnos relevantes en **una** consulta.

---

## 8. Usuarios, roles y alcance

### Reglas de `UsuarioService` (`SGC.Logica/UsuarioService.cs`)

| Regla | Método | Detalle |
|-------|--------|---------|
| Nombre de usuario: mínimo 3 caracteres, solo letras, números, `_` y `.` | `Validar` | Reforzado por `CK_Usuario_NombreUsuario` |
| Contraseña obligatoria al crear; mínimo 6 caracteres si se informa | `Validar` | El largo se valida **antes** de hashear |
| No puede haber dos usuarios **activos** con el mismo nombre | `Validar` | La base lo exige además contra los inactivos |
| Rol Médico: debe tener un médico activo asignado, y ese médico no puede tener otro usuario activo | `Validar` | Base: `UQ_Usuario_MedicoUnico` y `CK_Usuario_RolMedico` |
| Rol Recepcionista: al menos un médico asignado | `Validar` | Se guardan en `UsuarioMedico` |
| Al modificar, contraseña en blanco = no cambiarla | `Modificar` | |
| Cambiar de rol limpia lo que no corresponde (`MedicoId` o la lista de médicos asignados) | `Modificar`, `ResolverAsignaciones` | |
| La contraseña se guarda hasheada (PBKDF2) | `Agregar`, `Modificar` | `PasswordHasher.Hashear` |
| Baja lógica y reactivación | `EliminarLogico`, `Reactivar` | `Reactivar` vuelve a chequear nombre duplicado y médico ya asignado |
| **Siempre debe quedar al menos un Administrador activo** | `EliminarLogico`, `Modificar` → `ExigirOtroAdministradorActivo` | No se puede dar de baja ni cambiar de rol al único administrador activo (sin administrador nadie entraría a Usuarios, Médicos ni Copia de seguridad). La pantalla además impide eliminar o cambiar el rol del propio usuario con la sesión iniciada |
| Los médicos asignados a una Recepcionista son solo los **activos** | `ResolverAsignaciones`, `ActualizarMedicosAsignados` | Al abrir Pacientes, Turnos o Reportes se releen de la base, porque el administrador pudo cambiar asignaciones o dar de baja médicos después del login |
| Solo un usuario activo puede iniciar sesión | `BuscarPorCredenciales` | `Activo = true` en la consulta |

### Qué ve cada rol

| Aspecto | Administrador | Recepcionista | Médico |
|---------|---------------|---------------|--------|
| Menú | Médicos, Horarios, Usuarios, Obras Sociales, Ausencias, Copia de seguridad, Inicio (sin Reportes) | Pacientes, Turnos, Reportes, Inicio | Agenda, Actividad, Historial, Reportes, Inicio |
| Médicos disponibles al agendar turnos | Todos | **Solo los asignados** (`MedicosAsignados`) | — |
| "Próximo turno" en el listado de pacientes | De cualquier médico | Solo de sus médicos asignados | Solo del propio médico |
| Inicio | **Estado del sistema** (usuarios activos por rol, médicos sin horarios, médicos sin recepcionista, obras sociales activas, última copia de seguridad) más turnos de hoy y de la semana; sin dinero | Resumen del día sumando sus médicos asignados (`ObtenerResumenDeMedicos`; lista vacía = todo en cero) | Su propia agenda |
| Pestañas de Reportes | **Ninguna** (no tiene Reportes ni ve dinero) | Las **7**: Turnos, Recaudación, Asistencia, Ocupación, Pacientes por mes, Rendimiento de médicos y Obras sociales, siempre de sus médicos asignados ("Todos mis médicos" consulta uno por uno y suma) | **6**: Turnos, Recaudación, Asistencia, Ocupación, Pacientes por mes y Obras sociales (sin Rendimiento); siempre filtrado por su propio `MedicoId` aunque el combo diga otra cosa |
| Copia de seguridad | **Sí (único rol)**, ver sección 12 | No | No |
| **Datos clínicos** | No | No | **Sí (único rol)** |

Fuentes de la tabla: `FormMenuPrincipal.ConfigurarBotonesPorRol`, `FormTurnos` (carga de `CboMedico`), `FormPacientes.MedicosVisibles`, `FormInicio`, `FormReportes` (`MedicoSeleccionado`, `MedicosAConsultar`, pestañas por rol). El alcance sigue viviendo en la **UI**: los Services de reportes aceptan cualquier `medicoId` (lo aclara el comentario de `ReporteService`). Desde esta versión los procedimientos de Recaudación, Obras sociales y Rendimiento también reciben `@MedicoId` (ver `05-procedimientos-almacenados.md`); la Recepcionista con "Todos mis médicos" los llama una vez por médico asignado y suma en memoria.

### RNF#07: datos clínicos solo para el rol Médico

> **Decisión de alcance (acordada con el usuario):** el rol **Médico puede consultar la historia clínica completa de un paciente, incluidas las atenciones de otros médicos**. No se considera un defecto. La restricción de RNF#07 se aplica a la **Recepcionista**, que nunca ve datos clínicos (ni el Administrador).
>
> **Límite conocido:** RNF#07 se hace cumplir **a nivel de aplicación** (la pantalla y `AtencionFila`). Todos los roles comparten un único login de SQL Server, así que no hay una restricción a nivel de motor: quien tenga acceso directo a la base puede leer las tablas clínicas.

Se cumple en tres puntos:

1. `AtencionFila` (usada por `ActividadMedicaService.ConsultarFilas`): motivo, procedimiento y receta devuelven cadena vacía salvo `IncluyeDatosClinicos`, que es `rol == RolUsuario.Medico`.
2. Los DTO de reportes (`ReporteTurnoFila`, etc.) y los procedimientos **no incluyen** campos clínicos (lo dicen los comentarios de `ReporteService` y de `sp_ReporteTurnosPorPeriodo`).
3. `TurnoAgendaItem` y `PacienteFila`/`MedicoFila` solo traen datos administrativos.

---

## 9. Actividad médica (historial clínico)

`SGC.Logica/ActividadMedicaService.cs` (tabla `ActividadesMedicas`).

| Regla | Método | Detalle |
|-------|--------|---------|
| Un turno admite atención si está **activo**, en estado `Confirmado` **o `Asistio`**, y su fecha **no es futura** | `TurnoAdmiteActividad` (compartida con la pantalla) | El médico puede atender antes o después de que recepción confirme la asistencia. El combo de `FormRegistrarActividad` usa exactamente la misma regla |
| El turno se vuelve a leer de la base y debe pertenecer al médico | `RegistrarOModificarActividad` | "El turno seleccionado no pertenece a este médico." |
| No se registra sobre un turno `Cancelado` | `RegistrarOModificarActividad` | `InvalidOperationException` |
| No se registra sobre un turno `Ausente` | `RegistrarOModificarActividad` | "... en el que el paciente estuvo ausente." |
| No se registra sobre un turno de **fecha futura** | `RegistrarOModificarActividad` | `InvalidOperationException` |
| El motivo de consulta es obligatorio | `RegistrarOModificarActividad` | `ArgumentException`; respaldado por `CK_ActividadMedica_Motivo` |
| El tipo de actividad debe existir | `RegistrarOModificarActividad` | |
| Una actividad por turno: se crea o se actualiza la existente | `RegistrarOModificarActividad` | Busca **sin filtrar por `Activo`**: el índice único por `TurnoId` incluye las eliminadas, así que una atención dada de baja se **reactiva** y se pisa en lugar de insertar una segunda fila |
| Los textos se guardan con `Trim()` | `RegistrarOModificarActividad` | Procedimiento y receta pueden quedar vacíos |
| Baja lógica de la actividad | `EliminarLogico` | `Activo = false`; el turno vuelve a verse como no atendido |
| `desde` no puede ser posterior a `hasta` | `ConsultarFilas` | `ArgumentException` |
| Los datos clínicos solo se devuelven al rol Médico | `ConsultarFilas` → `AtencionFila` | RNF#07, ver sección 8 |
| Un turno figura "Atendido" si tiene actividad activa | `Turno.EstadoAtencion` | Si no, muestra el estado con las mismas etiquetas de la interfaz ("Confirmado", "Asistió", "Ausente", "Cancelado") |

Solo el rol Médico tiene el botón de registrar actividad y el historial clínico (menú). El alcance de médico lo da la pantalla: `FormRegistrarActividad` lista los turnos admitidos del médico logueado para la fecha elegida.

**Pendiente de atención** (botón "PENDIENTES" y "próximo paciente" de la agenda): turno activo `Confirmado` o `Asistio` sin atención cargada (sección 1).

---

## 10. Validaciones de pacientes, médicos y obras sociales

### Pacientes (`PacienteService.Validar`)

| Campo | Regla | Respaldo en la base |
|-------|-------|---------------------|
| Nombre, apellido | Obligatorios; solo letras, espacios, apóstrofes y guiones (`Texto.EsNombreValido`, acepta acentos y ñ), máximo 80 | `CK_Paciente_Nombre/Apellido`: sin dígitos |
| DNI | Obligatorio, solo dígitos, **7 u 8** dígitos; no puede ser todo ceros | `CK_Paciente_Dni_NoCero`; índice único |
| Email | Obligatorio; formato válido (`Texto.EsEmailValido`: sin espacios, dominio con punto, máximo 120) | Ninguno |
| Teléfono | Obligatorio; acepta `+`, espacios, guiones y paréntesis, y se guardan **solo los dígitos** (6 a 15) | `CK_Paciente_Telefono` (solo dígitos) |
| Fecha de nacimiento | Obligatoria; **anterior a hoy** (ni hoy ni futura); no más de 120 años atrás | `CK_Paciente_FechaNacimiento` (solo no futura) |
| DNI repetido | No puede haber otro paciente con el mismo DNI (activo o dado de baja; el mensaje lo distingue) | Índice único |
| Obra social | Opcional (nulo = Particular); si se informa, debe existir y estar activa | FK |

### Médicos (`MedicoService.Validar`)

| Campo | Regla | Respaldo en la base |
|-------|-------|---------------------|
| Nombre, apellido | Obligatorios; letras, espacios, apóstrofes y guiones, máximo 80 | `CK_Medico_*`: sin dígitos |
| Especialidad | Obligatoria; solo letras y espacios, máximo 80 | `CK_Medico_*`: sin dígitos |
| DNI | 7 u 8 dígitos; no todo ceros | `CK_Medico_Dni_NoCero`; índice único |
| Matrícula | Obligatoria, máximo 30; **no puede repetirse** (el mensaje aclara si el otro médico está dado de baja) | Índice único `IX_Medicos_Matricula` |
| Precio particular | **Mayor que cero** (los datos existentes no se modifican: un médico con precio 0 debe cargar uno al editarlo) | `CK_Medico_Precio` (no negativo) |
| DNI repetido | No puede haber otro médico con el mismo DNI (activo o dado de baja; el mensaje lo distingue) | Índice único |
| Obras sociales aceptadas | Cada una debe existir y estar activa | Tabla `MedicoObraSocial` |

### Obras sociales (`ObraSocialService.Validar`)

| Campo | Regla | Respaldo en la base |
|-------|-------|---------------------|
| Nombre | Obligatorio, máximo 100; no puede repetirse (activa o dada de baja; el mensaje lo distingue) | Índice único (incluye inactivas) |
| Porcentaje de cobertura | Entre 0 y 100 | `CK_ObraSocial_Porcentaje` |

**Recrear un registro dado de baja:** como los índices únicos incluyen a las filas inactivas, recrear un médico, paciente u obra social con los mismos datos únicos sigue sin ser posible, pero ahora falla con un mensaje claro ("... (está dado de baja)") que arman `Validar` y `Persistencia.Guardar`, en vez del error técnico de SQL Server. Solo el usuario tiene `Reactivar`, y la franja de agenda se reactiva sola al volver a cargarla.

Los listados de la UI se arman con `ObtenerListado` de cada Service (las clases `*Fila`; ver `01-arquitectura.md`). `ObraSocialService.ObtenerListado` cuenta solo médicos y pacientes **activos**.

---

## 11. Diferencias entre lo que hace C# y lo que hace el procedimiento

`sp_AsignarTurno` es una versión en T-SQL de `TurnoService.AsignarTurno` (los comentarios del `.sql` lo dicen), pero **ninguna pantalla ni Service lo invoca** y quedó atrás: el C# validó más cosas después.

| Validación | `TurnoService.AsignarTurno` (C#) | `sp_AsignarTurno` (SQL) |
|-----------|----------------------------------|--------------------------|
| Fecha no pasada / horario de hoy ya pasado | Sí (ambos) | Solo fecha (`@Fecha < CAST(GETDATE() AS DATE)`) |
| Duración 5 a 240 y sin cruzar la medianoche | Sí | No (duración `<= 0` pasa a 30) |
| El médico atiende en ese horario | Sí (`MedicoAtiende`, en minutos) | Sí (`EXISTS` en `AgendasMedico`) |
| Paciente y médico activos | Sí (`ExigirActivos`) | No (solo la FK del `INSERT`) |
| Ausencias (`ExcepcionesAgenda`) | Sí (`ExigirSinAusencia`) | Sí: día completo (`Tipo = 0`) o rango que se superponga |
| Superposición con otro turno activo del médico | Sí | Sí |
| Paciente sin turnos simultáneos | Sí (`ExigirPacienteLibre`) | No |
| Estado inicial | `Confirmado` (1) | `Confirmado` (1) |
| Mensajes | Con día y horario en el texto | Fijos y más cortos ("El profesional no atiende en ese horario.") |

Antes el procedimiento era el único que validaba las ausencias; hoy lo hace también `TurnoService`, así que la diferencia relevante es la inversa: **el procedimiento tiene menos reglas que el código C#** (ver R-1 en [`08`](08-guia-para-modificar.md#defectos-y-deuda-conocidos)). Si algún día se conecta, hay que actualizarlo antes.

Los procedimientos de reporte además no filtran todos igual los turnos cancelados o inactivos (por ejemplo `sp_ReporteAsistencia` no mira `Activo`, `sp_ReporteOcupacionAgenda` exige `Activo = 1` y `Estado <> 2`, `sp_ReporteTurnosPorObraSocial` filtra solo `Activo = 1`). Los criterios de cada procedimiento están en `05-procedimientos-almacenados.md`.

---

## 12. Copia de seguridad y restauración

`SGC.Logica/BackupService.cs` (pantalla `SGC.UI/FormBackup.cs`). Solo el Administrador ve el botón; el control está en el menú, no en el Service. **No hay tabla propia**: el historial se lee de `msdb`.

> Nota: probado solo contra una base de prueba (no contra la base de desarrollo); falta la prueba desde la pantalla.

| Regla | Método | Detalle |
|-------|--------|---------|
| La carpeta de destino es obligatoria | `RealizarBackup` | `ArgumentException` si está vacía. Se sugiere la carpeta por defecto de la instancia (`ObtenerCarpetaPorDefecto`, `InstanceDefaultBackupPath`) |
| El archivo se llama `SGC_yyyyMMdd_HHmmss.bak` y se escribe con `INIT, CHECKSUM` | `NombreArchivo`, `RealizarBackup` | `INIT` sobrescribe un archivo del mismo nombre |
| Toda copia se verifica al terminar | `RealizarBackup` | `RESTORE VERIFYONLY ... WITH CHECKSUM`. Si falla, la copia queda creada pero `Verificado = false` y la pantalla avisa que no hay que confiar en ella |
| El `.bak` lo escribe y lo lee la **cuenta de servicio de SQL Server**, no la aplicación | `TraducirError` | La carpeta tiene que existir en el servidor y esa cuenta necesita permiso (escritura al crear, lectura al restaurar). Errores 3201, 3203 y 3013 se traducen a ese mensaje |
| El historial sale de `msdb.dbo.backupset` (tipo `D`, últimas 20) | `ObtenerHistorial` | Si no se puede leer `msdb`, `Disponible = false` y la grilla muestra el motivo |
| "Última copia" del Inicio del Administrador | `ObtenerUltimoBackup` | `MAX(backup_finish_date)`; la tarjeta se pone en advertencia si nunca hubo copia, no se pudo consultar o tiene más de 7 días |
| Restaurar **valida que el `.bak` sea de esta base** antes de tocar nada | `ObtenerInfoBackup` (la usa `Restaurar`) | `RESTORE HEADERONLY` desde `master`: debe ser una copia **completa** (`BackupType = 1`) y su `DatabaseName` debe coincidir con la base actual; si no, "El archivo es una copia de la base ..., no de ... No se restauró nada." |
| La pantalla muestra los datos de la copia y pide confirmación | `FormBackup.RestaurarAsync` | Base de origen, servidor y fecha del `.bak`; botón por defecto "No" |
| Restaurar verifica la integridad del archivo antes de tocar la base | `Restaurar` | `RESTORE VERIFYONLY ... WITH CHECKSUM` desde `master` |
| Restaurar usa `master` y acceso exclusivo | `Restaurar` | `ALTER DATABASE ... SET SINGLE_USER WITH ROLLBACK IMMEDIATE`, luego `RESTORE DATABASE ... WITH REPLACE` |
| La base nunca queda en `SINGLE_USER` | `Restaurar` | El `finally` vuelve a `MULTI_USER`, aun si la restauración falló |
| Los pooles de conexiones se vacían antes y después | `Restaurar` | `SqlConnection.ClearAllPools()`: las conexiones reutilizadas apuntarían a la base anterior |
| **Después de restaurar se migra la base** | `Restaurar` → `MigrarBase` | La copia puede ser de una versión anterior del programa: se ejecuta `Migrate()` sobre la base restaurada. No lanza: si falla, la restauración igual se hizo y el resultado lo informa (`ResultadoRestauracion.MigracionOk`) |
| Si la migración falla no se vuelve al login | `FormBackup.RestaurarAsync` | Muestra el error y pide cerrar la aplicación y consultar a soporte |
| Al terminar bien se vuelve al login | `FormBackup.RestaurarAsync` | `FormMenuPrincipal.CerrarSesion`: reutiliza el login oculto para entrar con los datos restaurados |
| **Mientras crea o restaura no se puede navegar ni cerrar** | `FormMenuPrincipal.BloquearPorOperacion`, `OnFormClosing` | Se deshabilitan el menú lateral y el pie, y cerrar la ventana muestra "Hay una copia de seguridad en curso" |
| **No hay copias programadas**: backup y restauración son **manuales** | — | Nadie las dispara solas; el Inicio del Administrador avisa si la última tiene más de 7 días |

Límite conocido: el `.bak` lo escribe la cuenta de servicio de SQL Server en el servidor; para tener una copia fuera de ese equipo hay que copiarla por fuera de la aplicación.

Detalles de seguridad en el código: la ruta del `.bak` siempre viaja como parámetro `@ruta` y el nombre de la base se escapa con corchetes (`Entre`), porque un identificador no admite parámetros. La operación corre sin límite de tiempo (`CommandTimeout = 0`).

Flujo paso a paso: [`04-flujos.md`](04-flujos.md#i-copia-de-seguridad-y-restauración).

---

## 13. Bajas con control

Las bajas son lógicas (`Activo = false`). Varias se **rechazan** si dejarían turnos huérfanos. "Turnos confirmados futuros" son los activos en estado `Confirmado` desde hoy en adelante (hoy solo si la hora de inicio no pasó): `TurnosFuturos.Confirmados`.

| Baja | Regla | Mensaje / efecto |
|------|-------|------------------|
| **Médico** (`MedicoService.EliminarLogico`) | Se rechaza si tiene turnos confirmados futuros | "No se puede dar de baja al médico: tiene N turno(s) confirmado(s) pendiente(s). Cancele o reasigne esos turnos primero." Si se da de baja, su usuario de acceso (Rol Médico) queda inactivo en la misma operación |
| **Paciente** (`PacienteService.EliminarLogico`) | Se rechaza si tiene turnos confirmados futuros | "No se puede dar de baja al paciente: ... Cancele esos turnos primero." |
| **Franja de agenda** (`AgendaMedicoService.EliminarLogico` y `Modificar`) | Se rechaza si dejaría fuera de todas las franjas activas del médico (y día) a turnos confirmados futuros que hoy caen dentro de la franja | "No se puede modificar o eliminar esta franja: dejaría sin horario de atención a N turno(s) confirmado(s) futuro(s) ..." |
| **Obra social** (`ObraSocialService.EliminarLogico`) | **No se bloquea**: la pantalla muestra antes cuántos pacientes afiliados y médicos la aceptan y pide confirmación (`FormObrasSociales`). Una obra social inactiva deja de aplicar descuento (`CalcularMontoConObraSocial` exige `Activo`) | Pueden quedar pacientes con una obra social inactiva (ver Hallazgos) |
| **Usuario** (`UsuarioService.EliminarLogico`) | Se rechaza si es el único Administrador activo (sección 8) | "No se puede eliminar al único administrador activo ..." |
| **Ausencia** (`ExcepcionAgendaService.EliminarLogico`) | Sin control; no restituye turnos cancelados (sección 7) | — |

Los médicos dados de baja además quedan fuera de las asignaciones de usuarios y de los listados de agenda (`AgendaMedicoService` filtra `Medico.Activo`).

---

## 14. Sesión y arranque

| Regla | Dónde | Detalle |
|-------|-------|---------|
| La app **termina** al cerrar el menú principal con la X; solo "Cerrar sesión" vuelve al login | `FormMenuPrincipal.OnFormClosed` / `CerrarSesion` (`_cerrandoSesion`) | Antes el login oculto mantenía vivo el proceso sin ninguna ventana |
| Al cerrar sesión o entrar un usuario nuevo se **vacían los avisos** y la clave escrita | `NotificacionService.LimpiarAvisos`, `FormLogin` | Los avisos llevan emails de pacientes: el usuario siguiente no los hereda |
| Los médicos asignados a la Recepcionista se **releen** al abrir Pacientes, Turnos, Inicio y Reportes | `UsuarioService.ActualizarMedicosAsignados` | El objeto `Usuario` se cargó al iniciar sesión y pudo quedar viejo |
| Una `appsettings.Local.json` mal formada o con la cadena vacía se informa con un mensaje claro | `SGCContextFactory.LeerConnectionString` | La cadena se lee **una sola vez** y se guarda en memoria; el cuadro "Error de conexión" sale de `Program.cs` antes del login |
| Errores no controlados: se muestran en español sin cerrar la aplicación | `Program.Main` (`ThreadException`, `UnhandledException`) | "Ocurrió un error inesperado. Si el problema continúa, cierre y vuelva a abrir la aplicación." |
| Toda la interfaz usa la cultura **es-AR** | `Program.Main` | `CurrentCulture` y los valores por defecto de los hilos: nombres de días y meses, formatos de fecha y números |
| Mientras corre un backup o restore no se puede cerrar la ventana | `FormMenuPrincipal.OnFormClosing` | Ver sección 12 |

---

## Hallazgos

Hechos observados en el código, sin juicio. Lo ya resuelto está tachado con su motivo; el resto sigue vigente.

| # | Hallazgo | Dónde |
|---|----------|-------|
| 1 | ~~`AsignarTurno` y `ModificarTurno` ignoran `ExcepcionAgenda`~~ **Resuelto:** ambos rechazan turnos dentro de una ausencia activa. `sp_AsignarTurno` sigue sin conectarse a nada y con menos reglas que el C#. | `SGC.Logica/TurnoService.cs` |
| 2 | ~~`AsignarTurno` no valida que paciente y médico estén activos~~ **Resuelto** (`ExigirActivos`, también en `ModificarTurno`). | `SGC.Logica/TurnoService.cs` |
| 3 | ~~Se puede modificar o cancelar un turno ya atendido~~ **Resuelto:** solo se modifica o cancela un turno `Confirmado`, en el Service y en la pantalla. | `TurnoService`, `FormTurnos.ActualizarAcciones` |
| 4 | **Modificar una ausencia no cancela turnos que queden afectados y eliminarla no restituye los ya cancelados.** Cargar (`Agregar`) sí es atómico y cancela. Los turnos nuevos igual quedan bloqueados por la ausencia. | `SGC.Logica/ExcepcionAgendaService.cs` |
| 5 | ~~`Pendiente (0)` nunca se crea~~ **Resuelto:** el estado se eliminó del enum y de la UI (migración `QuitaEstadoPendiente`). | Varios (sección 1) |
| 6 | **Editar un paciente cuya obra social fue dada de baja lo guarda como Particular sin avisar.** El combo de `FormPacientes` solo carga obras sociales activas; al editar, `CboObraSocial.SelectedValue` queda sin coincidencia y `BtnGuardar_Click` lo interpreta como 0 = Particular. Deducido del código, no ejecutado. | `SGC.UI/FormPacientes.cs` (`CargarCombos`, `BtnGuardar_Click`) |
| 7 | ~~Mensajes sin tildes en los Services~~ **Resuelto en lo visible para el usuario** (textos con tildes y signos de apertura). Quedan sin tildes los comentarios del código y parte de los datos sembrados (`DbSeeder`). | `SGC.Logica/*.cs` |
| 8 | **Resuelto.** El reporte "Obras sociales" de la Recepcionista mostraba datos de todos los médicos; ahora `sp_ReporteTurnosPorObraSocial` recibe `@MedicoId` y la pantalla acota a los médicos activos del usuario. | `SGC.UI/FormReportes.Estadisticas.cs`, `sp_ReporteTurnosPorObraSocial` |
| 9 | ~~La unicidad de matrícula no se valida en C#~~ **Resuelto** con mensaje claro (`MedicoService.Validar`). | `SGC.Logica/MedicoService.cs` |
| 10 | **Recrear entidades dadas de baja con los mismos datos únicos sigue fallando** (índices sin filtro de `Activo`), ahora con un mensaje claro. Solo `Usuario` tiene `Reactivar`; la franja de agenda se reactiva sola. | Ver `02-modelo-de-datos.md`, sección 6 |
| 11 | **RNF#07 solo a nivel de aplicación:** hay un único login de SQL Server para todos los roles; la restricción de datos clínicos vive en la interfaz y en `AtencionFila`, no en el motor. El Médico lee la historia completa de un paciente por decisión de alcance. | Sección 8 |
| 12 | **Los turnos `Confirmado` de fechas pasadas no se cierran solos:** nadie los pasa a `Asistio`/`Ausente`, así que siguen "pendientes" (89 al 02/11/2026 en la base de desarrollo; consulta: `SELECT COUNT(*) FROM Turnos WHERE Estado = 1 AND Fecha < CAST(GETDATE() AS date)`). | `TurnoService`, reportes |
| 13 | **No hay copias de seguridad programadas**: solo manuales desde la pantalla. | `BackupService` |

## Para seguir leyendo

- Estructura de capas y patrones: [`01-arquitectura.md`](01-arquitectura.md)
- Tablas, restricciones e índices: [`02-modelo-de-datos.md`](02-modelo-de-datos.md)
- Procedimientos almacenados: [`05-procedimientos-almacenados.md`](05-procedimientos-almacenados.md)

## Transacciones y concurrencia

Resumen corto (el detalle, el escenario y la propuesta de arreglo están en [`08-guia-para-modificar.md`](08-guia-para-modificar.md#transacciones-y-concurrencia)):

- La aplicación **no usa transacciones explícitas** (ni `BeginTransaction` ni `TransactionScope`): cada operación hace un solo `SaveChanges`, que EF Core ejecuta en una transacción implícita (atómica). Cargar una ausencia junto con la cancelación de sus turnos también es un único `SaveChanges`.
- El riesgo real es la carrera "consultar superposición y después insertar" de `TurnoService.AsignarTurno` y `ModificarTurno` (médico y paciente): sin transacción, sin `UPDLOCK`/`HOLDLOCK` y sin índice único por médico, fecha y hora de inicio, dos asignaciones simultáneas podrían crear turnos superpuestos.
