# Reglas de negocio

Acá vas a encontrar **todas las reglas que hoy implementa el código** de `SGC.Logica` (y algunas de la UI que completan el cuadro), cada una con el archivo y el método donde viven. Sirve para responder "¿por qué no me deja hacer esto?" y "¿dónde cambio esta regla?" sin tener que leer los Services enteros.

Las reglas se verificaron leyendo el código. Cuando una regla vive en la UI o en la base y no en `SGC.Logica`, se aclara.

## Índice rápido

| # | Tema | Servicio principal |
|---|------|--------------------|
| 1 | Ciclo de vida del turno | `TurnoService` |
| 2 | Asignar un turno | `TurnoService.AsignarTurno` |
| 3 | Modificar y cancelar | `TurnoService.ModificarTurno` / `CancelarTurno` |
| 4 | Confirmar asistencia | `TurnoService.ConfirmarAsistencia` |
| 5 | Regla de cobro | `TurnoService.CalcularMonto*` |
| 6 | Agenda semanal y disponibilidad | `AgendaMedicoService`, `DisponibilidadDia` |
| 7 | Ausencias (excepciones de agenda) | `ExcepcionAgendaService` |
| 8 | Usuarios, roles y alcance | `UsuarioService` + UI |
| 9 | Actividad médica (historial clínico) | `ActividadMedicaService` |
| 10 | Validaciones de pacientes, médicos, obras sociales | `PacienteService`, `MedicoService`, `ObraSocialService` |
| 11 | Diferencias entre C# y los procedimientos | — |

---

## 1. Ciclo de vida del turno

`EstadoTurno` (`SGC.Entidades/Turno.cs`): 0 Pendiente, 1 Confirmado, 2 Cancelado, 3 Asistio, 4 Ausente.

```
            AsignarTurno
   (nada) ───────────────►  Confirmado (1) ──ConfirmarAsistencia(true)──►  Asistio (3)
                               │   │
                               │   └────ConfirmarAsistencia(false)──────►  Ausente (4)
                               │
                               ├────CancelarTurno──────────────────────►  Cancelado (2)
                               └────Agregar ausencia (ExcepcionAgenda)──►  Cancelado (2)

   Pendiente (0): existe en el enum, ninguna pantalla ni servicio lo crea.
```

| Transición | Quién la dispara | Dónde |
|-----------|------------------|-------|
| Nace **Confirmado** | Recepcionista, pantalla Turnos | `TurnoService.AsignarTurno` (`Estado = EstadoTurno.Confirmado`) |
| Confirmado → Asistio / Ausente | Recepcionista, diálogo "Confirmar asistencia" | `TurnoService.ConfirmarAsistencia` |
| Confirmado (o Pendiente) → Cancelado | Recepcionista, botón Cancelar | `TurnoService.CancelarTurno` |
| Pendiente/Confirmado → Cancelado (masivo) | Administrador, al cargar una ausencia | `ExcepcionAgendaService.Agregar` → `CancelarTurnosAfectados` |

Cancelar deja `Estado = Cancelado` **y** `Activo = false` (las dos cosas, en `CancelarTurno` y en `CancelarTurnosAfectados`).

**Sobre `Pendiente (0)`:** aunque no se crea, el código sí lo trata en varios lugares como "turno por atender" junto con `Confirmado` (`AgendaDiariaService.ElegirProximo`, `PacienteService.ObtenerListado`, `ResumenService.ObtenerProximoPaciente`, `ExcepcionAgendaService.CancelarTurnosAfectados`). `ResumenService.ObtenerProximosSinConfirmar` busca exclusivamente `Pendiente`, así que en una base creada solo por la app siempre devuelve vacío.

---

## 2. Asignar un turno

`SGC.Logica/TurnoService.cs`, `AsignarTurno(paciente, medico, fecha, horaInicio, duracionMinutos = 30)`. Las validaciones corren **en este orden** y la primera que falla corta con una excepción:

| # | Regla | Excepción / mensaje |
|---|-------|---------------------|
| 0 | Si `duracionMinutos <= 0`, se usa 30 (no falla). | — |
| 1 | La fecha no puede ser anterior a hoy. | `ArgumentException`: "No se puede asignar un turno en una fecha pasada." |
| 2 | El médico debe atender ese día y horario: debe existir una franja **activa** de `AgendasMedico` del mismo médico y día de la semana que **contenga entero** el turno (`horaInicio >= franja.HoraInicio` y `horaFin <= franja.HoraFin`). | `InvalidOperationException`: "El profesional no atiende el {día} en el horario ..." (`AgendaMedicoService.MedicoAtiende`) |
| 3 | No puede superponerse con otro turno **activo** del mismo médico y fecha: `t.HoraInicio < horaFin && t.HoraFin > horaInicio` (`HaySuperposicion`). Los turnos que solo se tocan en el borde (uno termina cuando el otro empieza) no se consideran superpuestos. | `InvalidOperationException`: "El Dr./Dra. {apellido} ya tiene un turno asignado ..." |
| 4 | Si todo está bien, inserta `Turno` con `Estado = Confirmado` y `Activo = true`. | — |

**Por qué la superposición se resuelve en memoria:** `HoraFin` es una propiedad calculada (no es columna), así que EF no puede traducirla a SQL. `HaySuperposicion` trae los turnos del médico en esa fecha (consulta chica) y compara los rangos con LINQ.

**Lo que `AsignarTurno` NO valida** (verificado leyendo el método): que el paciente o el médico existan o estén activos, que el médico acepte la obra social del paciente, ni las ausencias cargadas (ver sección 11).

### Qué hace la pantalla para ayudar (`SGC.UI/FormTurnos.cs`, `CrearHorarios`)

La UI **sí** consulta las ausencias antes de ofrecer horarios, pero no es el Service el que las hace cumplir:

1. `AgendaMedicoService.ObtenerDisponibilidad(medicoId, fecha)` devuelve los bloques del día y las ausencias activas de esa fecha.
2. `DisponibilidadDia.MotivoSinAtencion` explica por qué no hay turnos (ausencia de día completo, médico sin agenda, o no atiende ese día).
3. `HorarioService.GenerarSlotsEnRango` corta cada bloque en slots de la duración elegida.
4. Cada slot se pinta ocupado si hay un turno superpuesto o `disponibilidad.HayAusenciaEn(inicio, fin)`.

---

## 3. Modificar y cancelar un turno

| Regla | Dónde | Detalle |
|-------|-------|---------|
| La nueva fecha no puede ser pasada | `ModificarTurno` | `ArgumentException` |
| El turno debe existir | `ModificarTurno` | "El turno que intenta modificar no existe." |
| No se modifica un turno `Cancelado` | `ModificarTurno` | `InvalidOperationException` |
| Debe caer en una franja del médico (misma regla que al asignar) | `ModificarTurno` → `MedicoAtiende` | Usa el médico **del turno**, no se puede cambiar de médico |
| No debe superponerse con otro turno activo (excluye el propio) | `ModificarTurno` → `HaySuperposicion(..., excluirTurnoId: turnoId)` | |
| Solo cambia `Fecha`, `HoraInicio` y `DuracionMinutos` | `ModificarTurno` | El estado no se toca |
| El turno debe existir y no estar ya cancelado | `CancelarTurno` | "Ese turno ya estaba cancelado." |
| Cancelar = `Estado = Cancelado` + `Activo = false` | `CancelarTurno` | |

**Regla de pantalla** (no del Service): en `FormTurnos.ActualizarAcciones`, Modificar y Cancelar se habilitan para cualquier turno no cancelado. Es decir, el servicio y la UI permiten modificar o cancelar un turno que ya figura como `Asistio` o `Ausente` (ver Hallazgos).

**Avisos al paciente:** después de asignar, reprogramar o cancelar, `FormTurnos` llama a `NotificacionService.AvisarTurno`. Es una simulación: agrega un texto a una lista estática en memoria; no manda ningún email. Si el paciente no tiene email válido (sin `@`), el aviso devuelve un mensaje de que no fue posible, pero el turno se guarda igual.

---

## 4. Confirmar asistencia

`TurnoService.ConfirmarAsistencia(turnoId, asistio, medioPago, monto)`:

| # | Regla | Mensaje |
|---|-------|---------|
| 1 | El turno debe existir | "El turno no existe." |
| 2 | Solo se confirma un turno en estado **Confirmado** | "Solo se puede confirmar asistencia de un turno en estado Confirmado." |
| 3 | La fecha del turno no puede ser futura (`turno.Fecha > hoy` falla; **hoy sí se puede**) | "No se puede confirmar la asistencia de un turno que todavia no llego a su fecha." |
| 4a | Si `asistio = true`: el medio de pago es obligatorio | `ArgumentException` |
| 4b | Si `asistio = true`: el monto es obligatorio y no puede ser negativo (**cero sí es válido**) | `ArgumentException` |
| 5a | `asistio = true` → `Estado = Asistio`, guarda `MedioPago` y `Monto` | |
| 5b | `asistio = false` → `Estado = Ausente`, `MedioPago = null`, `Monto = null` | |

Lo que además hace la pantalla (`SGC.UI/FormConfirmarAsistencia.cs`):

- Ofrece como medios de pago: **"Particular"** siempre, y **el nombre de la obra social del paciente solo si el médico la acepta** (ver sección 5).
- Al elegir un medio de pago, **sugiere el monto** (editable): precio particular, o precio menos la cobertura.
- La base respalda el monto no negativo con `CK_Turno_Monto`.

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
| El médico debe existir y estar activo | `ValidarDatosBasicos` | "Debe seleccionar un medico valido." |
| `HoraFin` debe ser posterior a `HoraInicio` | `ValidarDatosBasicos` | "La hora de fin debe ser posterior a la hora de inicio." |
| No puede superponerse con otra franja **activa** del mismo médico y día de la semana | `ValidarSuperposicion` | "El rango horario seleccionado se superpone ..." |
| La baja es lógica (`Activo = false`) | `EliminarLogico` | |

La base refuerza con `CK_AgendaMedico_Rango` (`HoraFin > HoraInicio`) y un índice único sobre (médico, día, hora inicio, hora fin).

### Consultas

| Método | Para qué |
|--------|----------|
| `MedicoAtiende(medicoId, dia, horaInicio, duracion)` | Responde si el turno entra completo en una franja activa (lo usan `AsignarTurno` y `ModificarTurno`) |
| `ObtenerResumenSemanal` | Una fila por médico activo con sus franjas y horas semanales (`AgendaSemanalFila`) |
| `ObtenerDisponibilidad(medicoId, fecha)` | Devuelve `DisponibilidadDia` (solo lectura) |

### `DisponibilidadDia` (`SGC.Logica/DisponibilidadDia.cs`)

Objeto de solo lectura que junta, para un médico y una fecha: los **bloques** de ese día de la semana, las **ausencias** activas de esa fecha y los **días que atiende** (ordenados lunes a domingo).

| Miembro | Qué responde |
|---------|--------------|
| `AusenteTodoElDia` | ¿Hay una ausencia `DiaCompleto`? |
| `HayAusenciaEn(inicio, fin)` | ¿El tramo cae total o parcialmente en una ausencia? (día completo → siempre sí; rango → superposición) |
| `MotivoSinAtencion(nombreMedico)` | Texto que explica por qué no hay turnos: ausencia de día completo (con el motivo), médico sin horarios cargados, o "no atiende los {día}. Atiende: ..."; `null` si sí atiende |
| `DescripcionAusenciasParciales()` | Texto de las ausencias por rango del día |

Es solo ayuda para la pantalla: **no** modifica las reglas de `AsignarTurno` ni de `MedicoAtiende`.

---

## 7. Ausencias (excepciones de agenda)

`SGC.Logica/ExcepcionAgendaService.cs` (tabla `ExcepcionesAgenda`). Solo el administrador accede desde el menú.

### Validaciones (`Validar`, en alta y en modificación)

| Regla | Mensaje |
|-------|---------|
| El médico debe existir y estar activo | "Debe seleccionar un medico valido." |
| La fecha no puede ser pasada (**hoy sí**) | "No se puede cargar una excepcion en una fecha pasada." |
| El motivo es obligatorio | "Debe indicar el motivo de la ausencia." |
| Si el tipo es `RangoHorario`: hora de inicio y fin obligatorias, y `HoraFin > HoraInicio` | mensajes específicos |

`DiaCompleto` no usa horas; la base lo exige con `CK_ExcepcionAgenda_Tipo`.

### Efecto sobre los turnos existentes (solo al **agregar**)

`Agregar` guarda la ausencia y llama a `CancelarTurnosAfectados`:

1. Busca turnos **activos** del médico en esa fecha con estado `Pendiente` o `Confirmado`.
2. Si la ausencia es `DiaCompleto`, afecta a todos; si es `RangoHorario`, solo a los que se superponen (`turno.HoraInicio < HoraFin && HoraInicio < turno.HoraFin`).
3. Los pasa a `Cancelado` + `Activo = false` y registra un aviso al paciente con `NotificacionService.AvisarTurno` ("... por ausencia del profesional").
4. Devuelve la lista de turnos cancelados para que la pantalla informe.

`Modificar` y `EliminarLogico` **no** vuelven a revisar turnos: modificar una ausencia no cancela turnos nuevos que queden afectados, y eliminar una ausencia no restaura los turnos que ya se cancelaron.

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
| Solo un usuario activo puede iniciar sesión | `BuscarPorCredenciales` | `Activo = true` en la consulta |

### Qué ve cada rol

| Aspecto | Administrador | Recepcionista | Médico |
|---------|---------------|---------------|--------|
| Menú | Médicos, Horarios, Usuarios, Obras Sociales, Ausencias, Reportes, Inicio | Pacientes, Turnos, Reportes, Inicio | Agenda, Actividad, Historial, Reportes, Inicio |
| Médicos disponibles al agendar turnos | Todos | **Solo los asignados** (`MedicosAsignados`) | — |
| "Próximo turno" en el listado de pacientes | De cualquier médico | Solo de sus médicos asignados | Solo del propio médico |
| Inicio (resumen del día) | Resumen general | Suma de sus médicos asignados (`ObtenerResumenDeMedicos`; lista vacía = todo en cero) | Su propia agenda |
| Pestañas de Reportes | Turnos, Recaudación, Asistencia, Ocupación, Pacientes por mes, Rendimiento de médicos, Obras sociales | Turnos, Pacientes por mes, Obras sociales ("Todos mis médicos" consulta uno por uno los asignados) | Turnos, Asistencia, Ocupación, Pacientes por mes; siempre filtrado por su propio `MedicoId` aunque el combo diga otra cosa |
| **Datos clínicos** | No | No | **Sí (único rol)** |

Fuentes de la tabla: `FormMenuPrincipal.ConfigurarBotonesPorRol`, `FormTurnos` (carga de `CboMedico`), `FormPacientes.MedicosVisibles`, `FormInicio`, `FormReportes` (`MedicoSeleccionado`, pestañas por rol). Estas restricciones de alcance viven en la **UI**; los Services de reportes aceptan cualquier `medicoId` (lo aclara el comentario de `ReporteService`).

### RNF#07: datos clínicos solo para el rol Médico

Se cumple en tres puntos:

1. `AtencionFila` (usada por `ActividadMedicaService.ConsultarFilas`): motivo, procedimiento y receta devuelven cadena vacía salvo `IncluyeDatosClinicos`, que es `rol == RolUsuario.Medico`.
2. Los DTO de reportes (`ReporteTurnoFila`, etc.) y los procedimientos **no incluyen** campos clínicos (lo dicen los comentarios de `ReporteService` y de `sp_ReporteTurnosPorPeriodo`).
3. `TurnoAgendaItem` y `PacienteFila`/`MedicoFila` solo traen datos administrativos.

---

## 9. Actividad médica (historial clínico)

`SGC.Logica/ActividadMedicaService.cs` (tabla `ActividadesMedicas`).

| Regla | Método | Detalle |
|-------|--------|---------|
| Una actividad por turno (se crea o se actualiza la existente) | `RegistrarOModificarActividad` | Busca la activa del turno: si existe, la edita; si no, la crea. La base respalda con índice único en `TurnoId` |
| No se registra sobre un turno `Cancelado` | `RegistrarOModificarActividad` | `InvalidOperationException` |
| El motivo de consulta es obligatorio | `RegistrarOModificarActividad` | `ArgumentException`; respaldado por `CK_ActividadMedica_Motivo` |
| El tipo de actividad debe existir | `RegistrarOModificarActividad` | |
| Los textos se guardan con `Trim()` | `RegistrarOModificarActividad` | Procedimiento y receta pueden quedar vacíos |
| Baja lógica de la actividad | `EliminarLogico` | `Activo = false`; el turno vuelve a verse como no atendido |
| `desde` no puede ser posterior a `hasta` | `ConsultarFilas` | `ArgumentException` |
| Los datos clínicos solo se devuelven al rol Médico | `ConsultarFilas` → `AtencionFila` | RNF#07, ver sección 8 |
| Un turno figura "Atendido" si tiene actividad activa | `Turno.EstadoAtencion` | Si no, muestra el nombre del estado |

Solo el rol Médico tiene el botón de registrar actividad y el historial clínico (menú). El servicio **no** exige que el turno esté en `Asistio`; solo rechaza `Cancelado`.

El alcance de médico lo da la pantalla: `FormRegistrarActividad` lista los turnos no cancelados del médico logueado para la fecha elegida.

---

## 10. Validaciones de pacientes, médicos y obras sociales

### Pacientes (`PacienteService.Validar`)

| Campo | Regla | Respaldo en la base |
|-------|-------|---------------------|
| Nombre, apellido | Obligatorios; solo letras y espacios (`char.IsLetter`, acepta acentos y ñ) | `CK_Paciente_Nombre/Apellido`: sin dígitos |
| DNI | Obligatorio, solo dígitos, **7 u 8** dígitos; no puede ser todo ceros | `CK_Paciente_Dni_NoCero`; índice único |
| Email | Obligatorio; debe contener `@` y `.` | Ninguno |
| Teléfono | Obligatorio, solo dígitos, **6 a 15** | `CK_Paciente_Telefono` (igual) |
| Fecha de nacimiento | Obligatoria; no futura; no más de 120 años atrás | `CK_Paciente_FechaNacimiento` (solo no futura) |
| DNI repetido | No puede haber otro paciente **activo** con el mismo DNI | Índice único (también contra inactivos) |
| Obra social | Opcional (nulo = Particular); si se informa, debe existir y estar activa | FK |

### Médicos (`MedicoService.Validar`)

| Campo | Regla | Respaldo en la base |
|-------|-------|---------------------|
| Nombre, apellido, especialidad | Obligatorios; solo letras y espacios | `CK_Medico_*`: sin dígitos |
| DNI | 7 u 8 dígitos; no todo ceros | `CK_Medico_Dni_NoCero`; índice único |
| Matrícula | Obligatoria (no se controla duplicado en C#) | Índice único `IX_Medicos_Matricula` |
| Precio particular | No negativo | `CK_Medico_Precio` |
| DNI repetido | Otro médico **activo** con el mismo DNI | Índice único |
| Obras sociales aceptadas | Cada una debe existir y estar activa | Tabla `MedicoObraSocial` |

### Obras sociales (`ObraSocialService.Validar`)

| Campo | Regla | Respaldo en la base |
|-------|-------|---------------------|
| Nombre | Obligatorio; no puede repetirse entre obras **activas** | Índice único (incluye inactivas) |
| Porcentaje de cobertura | Entre 0 y 100 | `CK_ObraSocial_Porcentaje` |

Los listados de la UI se arman con `ObtenerListado` de cada Service (las clases `*Fila`; ver `01-arquitectura.md`). `ObraSocialService.ObtenerListado` cuenta solo médicos y pacientes **activos**.

---

## 11. Diferencias entre lo que hace C# y lo que hace el procedimiento

`sp_AsignarTurno` es una versión en T-SQL de `TurnoService.AsignarTurno` (los comentarios del `.sql` lo dicen). Comparación verificada leyendo ambos:

| Validación | `TurnoService.AsignarTurno` (C#) | `sp_AsignarTurno` (SQL) |
|-----------|----------------------------------|--------------------------|
| Fecha no pasada | Sí | Sí (`@Fecha < CAST(GETDATE() AS DATE)`) |
| El médico atiende en ese horario | Sí (`MedicoAtiende`) | Sí (`EXISTS` en `AgendasMedico`) |
| **Ausencias (`ExcepcionesAgenda`)** | **No las consulta** | **Sí**: día completo (`Tipo = 0`) o rango que se superponga |
| Superposición con otro turno activo | Sí | Sí |
| Duración `<= 0` pasa a 30 | Sí | Sí |
| Estado inicial | `Confirmado` (1) | `Confirmado` (1) |
| Existencia/actividad de paciente y médico | No | No (solo la FK del `INSERT`) |
| Mensajes | Con día y horario en el texto | Fijos y más cortos ("El profesional no atiende en ese horario.") |

**Hallazgo central:** `ExcepcionAgendaService` y la UI manejan las ausencias, pero **ni `AsignarTurno` ni `ModificarTurno` (en `TurnoService`) las consultan**. Hoy lo único que impide agendar durante una ausencia es que `FormTurnos` pinta esos horarios como ocupados. Un llamado directo al Service (otra pantalla futura, una prueba) sí permitiría crear el turno. El procedimiento sí rechaza el caso, pero **ninguna pantalla lo llama hoy** (ver `05-procedimientos-almacenados.md`).

Los procedimientos de reporte además no filtran todos igual los turnos cancelados o inactivos (por ejemplo `sp_ReporteAsistencia` no mira `Activo`, `sp_ReporteOcupacionAgenda` exige `Activo = 1` y `Estado <> 2`, `sp_ReporteTurnosPorObraSocial` filtra solo `Activo = 1`). Los criterios de cada procedimiento están en `05-procedimientos-almacenados.md`.

---

## Hallazgos

Hechos observados en el código, sin juicio.

| # | Hallazgo | Dónde |
|---|----------|-------|
| 1 | **`AsignarTurno` y `ModificarTurno` ignoran `ExcepcionAgenda`.** Solo `sp_AsignarTurno` las considera (y nadie lo llama). La UI evita ofrecer esos horarios, pero el Service no los rechaza. | `SGC.Logica/TurnoService.cs`; `SGC.UI/FormTurnos.cs` (`CrearHorarios`) |
| 2 | **`AsignarTurno` no valida que paciente y médico estén activos**; `ModificarTurno` tampoco revisa el estado del médico. | `SGC.Logica/TurnoService.cs` |
| 3 | **Se puede modificar o cancelar un turno ya atendido** (`Asistio` o `Ausente`): los servicios solo rechazan los cancelados y la UI habilita los botones para cualquier turno no cancelado. Cancelar un `Asistio` lo deja `Cancelado` conservando `Monto` y `MedioPago`. | `TurnoService.ModificarTurno`, `CancelarTurno`; `FormTurnos.ActualizarAcciones` |
| 4 | **Cargar o editar una ausencia no afecta turnos de la misma forma:** solo `Agregar` cancela turnos; `Modificar` no los revisa y `EliminarLogico` no los restituye. | `SGC.Logica/ExcepcionAgendaService.cs` |
| 5 | **`Pendiente (0)` nunca se crea**, pero hay lógica que lo trata (y `ObtenerProximosSinConfirmar` solo busca `Pendiente`). | Varios (sección 1) |
| 6 | **Editar un paciente cuya obra social fue dada de baja lo guarda como Particular sin avisar.** El combo de `FormPacientes` solo carga obras sociales activas; al editar, `CboObraSocial.SelectedValue` queda sin coincidencia y `BtnGuardar_Click` lo interpreta como 0 = Particular. Deducido del código, no ejecutado. | `SGC.UI/FormPacientes.cs` (`CargarCombos`, `BtnGuardar_Click`) |
| 7 | **Mensajes sin tildes en los Services** ("Debe seleccionar un medico valido.", "El turno no existe.") mientras que `DisponibilidadDia`, `ReporteService`, `PacienteService` (DNI, email, teléfono) sí las llevan. | `SGC.Logica/*.cs` |
| 8 | **Reporte "Obras sociales" visible para Recepcionista con datos de todos los médicos y la columna "Total recaudado":** el procedimiento no recibe médico, y solo el contador de facturación está restringido al administrador; la columna de la grilla no se oculta. Deducido del código. | `SGC.UI/FormReportes.Estadisticas.cs`, `sp_ReporteTurnosPorObraSocial` |
| 9 | **La unicidad de matrícula de médico no se valida en C#**; solo la base la rechaza. | `SGC.Logica/MedicoService.cs` |
| 10 | **Altas de entidades dadas de baja con los mismos datos únicos fallan en la base** (índices sin filtro de `Activo`), aunque la validación de C# las deje pasar. | Ver `02-modelo-de-datos.md`, sección 6 |

## Para seguir leyendo

- Estructura de capas y patrones: [`01-arquitectura.md`](01-arquitectura.md)
- Tablas, restricciones e índices: [`02-modelo-de-datos.md`](02-modelo-de-datos.md)
- Procedimientos almacenados: [`05-procedimientos-almacenados.md`](05-procedimientos-almacenados.md)

## Transacciones y concurrencia

Resumen corto (el detalle, el escenario y la propuesta de arreglo están en [`08-guia-para-modificar.md`](08-guia-para-modificar.md#transacciones-y-concurrencia)):

- La aplicación **no usa transacciones explícitas** (ni `BeginTransaction` ni `TransactionScope`): cada operación hace un solo `SaveChanges`, que EF Core ejecuta en una transacción implícita (atómica).
- El riesgo real es la carrera "consultar superposición y después insertar" de `TurnoService.AsignarTurno` y `ModificarTurno` (y de `sp_AsignarTurno`): sin transacción, sin `UPDLOCK`/`HOLDLOCK` y sin índice único por médico, fecha y hora de inicio, dos asignaciones simultáneas podrían crear turnos superpuestos.
- `ExcepcionAgendaService.Agregar` encadena dos `SaveChanges` (la ausencia y la cancelación de turnos): son dos transacciones separadas.
