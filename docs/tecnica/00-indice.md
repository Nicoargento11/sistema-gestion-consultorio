# Documentación técnica del SGC

Acá vas a encontrar la puerta de entrada a la documentación técnica: qué es el sistema, cómo está organizado el proyecto, **qué documento leer según lo que quieras entender**, un orden de lectura de 20 minutos y un glosario. Está pensada para que cualquiera del equipo entienda cómo funciona el código por dentro (el flujo y la lógica detrás), no solo cómo se usa.

## Qué es el SGC

- **Sistema de Gestión de Consultorio:** una aplicación de escritorio (C#, .NET 8, Windows Forms) con Entity Framework Core y SQL Server.
- Gestiona **pacientes, médicos, horarios, turnos, cobro de la consulta, atención clínica y reportes**.
- Tiene **tres roles** con pantallas distintas: Administrador (configura), Recepcionista (agenda y cobra) y Médico (atiende y ve el historial clínico; es el único que ve datos clínicos).
- Está dividido en **cuatro capas** (Entidades, Datos, Lógica, UI) y parte de los cálculos (reportes, resumen del día) viven en 10 procedimientos almacenados de la base.
- Todo lo escrito acá se verificó contra el código y la base de la rama; lo que no se pudo comprobar dice "no verificado" o "deducido".

## Mapa del proyecto

```
sistema-gestion-consultorio/
├─ SGC.Entidades/        Clases del dominio (Paciente, Medico, Turno, Usuario...), enums, PasswordHasher, DTO de reportes
├─ SGC.Datos/            SGCContext (EF Core), Migrations/, StoredProcedures/*.sql, DbSeeder, Scripts/ (datos de demo)
├─ SGC.Logica/           Un *Service por tema (TurnoService, PacienteService...) y las clases "Fila" para las grillas
├─ SGC.UI/               Pantallas (Form*.cs) y componentes compartidos de estilo
├─ docs/tecnica/         Esta documentación (y diagramas/)
├─ appsettings.example.json   Plantilla de la cadena de conexión (cada uno tiene su appsettings.Local.json, no versionado)
└─ README.md             Cómo correr el proyecto y usuarios de prueba
```

La dependencia va de arriba hacia abajo: `SGC.UI` → `SGC.Logica` → `SGC.Datos` → `SGC.Entidades` (detalle y matices en [`01-arquitectura.md`](01-arquitectura.md)).

## Si querés entender X, leé Y

| Pregunta | Documento |
|----------|-----------|
| ¿Cómo se organiza el código y quién depende de quién? | [`01-arquitectura.md`](01-arquitectura.md) |
| ¿Qué pasa desde que se abre la app hasta el login? | [`01-arquitectura.md`](01-arquitectura.md#2-cómo-arranca-la-aplicación) y [`04-flujos.md`](04-flujos.md#a-arranque-de-la-app-y-login) |
| ¿Qué tablas hay y cómo se relacionan? | [`02-modelo-de-datos.md`](02-modelo-de-datos.md) |
| ¿Por qué no me deja guardar esto? ¿Dónde vive la regla? | [`03-reglas-de-negocio.md`](03-reglas-de-negocio.md) |
| ¿Cómo llega un turno a la base? | [`04-flujos.md`](04-flujos.md#b-asignar-un-turno) |
| ¿Cómo se cobra una consulta y cómo se calcula el monto? | [`04-flujos.md`](04-flujos.md#d-confirmar-asistencia-y-cobro) y [`03`](03-reglas-de-negocio.md#5-regla-de-cobro) |
| ¿Qué pasa con los turnos cuando se carga una ausencia? | [`04-flujos.md`](04-flujos.md#f-cargar-una-ausencia) |
| ¿Cómo funciona la atención médica y el historial clínico? | [`04-flujos.md`](04-flujos.md#e-atención-médica-e-historial-clínico) |
| ¿De dónde salen los números de Inicio y de Reportes? | [`05-procedimientos-almacenados.md`](05-procedimientos-almacenados.md) y [`07-reportes.md`](07-reportes.md) |
| ¿Qué procedimientos hay y cuál se usa realmente? | [`05-procedimientos-almacenados.md`](05-procedimientos-almacenados.md) |
| ¿Qué pantallas existen y qué servicio usa cada una? | [`06-capa-ui.md`](06-capa-ui.md#1-mapa-de-pantallas) |
| ¿Por qué el Designer "me borra" cosas? ¿Cómo manejo la escala de pantalla? | [`06-capa-ui.md`](06-capa-ui.md#5-lógica-en-el-cs-el-designer-solo-para-la-estructura) |
| ¿Qué quiere decir "pendiente"? | [`06-capa-ui.md`](06-capa-ui.md#las-dos-acepciones-de-pendiente) |
| ¿Qué ve cada rol en Reportes? | [`07-reportes.md`](07-reportes.md#2-reglas-por-rol) |
| ¿Cómo agrego una pantalla, un campo, un reporte o un procedimiento? | [`08-guia-para-modificar.md`](08-guia-para-modificar.md#recetas) |
| ¿Cómo depuro "no se puede conectar" o un error de compilación por DLL bloqueada? | [`08-guia-para-modificar.md`](08-guia-para-modificar.md#cómo-depurar) |
| ¿Qué defectos conocidos hay y qué conviene arreglar primero? | [`08-guia-para-modificar.md`](08-guia-para-modificar.md#defectos-y-deuda-conocidos) |

## Orden de lectura sugerido (20 minutos)

| Minutos | Qué leer | Para qué |
|:-------:|----------|----------|
| 2 | Este índice | Ubicarte |
| 5 | [`01-arquitectura.md`](01-arquitectura.md), secciones 1 a 3 | Capas, arranque y patrón de un contexto por operación |
| 6 | [`04-flujos.md`](04-flujos.md), flujos **b** (asignar turno) y **d** (confirmar asistencia) | Ver cómo se recorre el código de punta a punta |
| 4 | [`03-reglas-de-negocio.md`](03-reglas-de-negocio.md), secciones 1 a 5 | Estados del turno y reglas de asignación y cobro |
| 3 | [`06-capa-ui.md`](06-capa-ui.md), secciones 1 a 3 y "pendiente" | Pantallas, navegación y el patrón visual |

Después, según lo que vayas a tocar: modelo de datos (02), procedimientos (05), reportes (07) o las recetas (08).

## Glosario

| Término | Qué es en este sistema |
|---------|------------------------|
| **Turno** | Una cita de un paciente con un médico, en una fecha y hora, con una duración (`Turno`). Tiene un **estado**: Pendiente (0), Confirmado (1), Cancelado (2), Asistio (3) o Ausente (4). |
| **Franja / agenda** | Un bloque semanal en que un médico atiende (por ejemplo lunes de 8 a 12): `AgendaMedico`. Los horarios de turnos se cortan de las franjas. |
| **Excepción / ausencia** | Un día u horario en que un médico **no** atiende aunque su franja diga que sí (vacaciones, congreso): `ExcepcionAgenda`. Puede ser de día completo o por rango horario. |
| **Obra social** | Cobertura médica del paciente. Cada una tiene un **porcentaje de cobertura** y cada médico elige cuáles acepta. |
| **Cobertura** | Porcentaje del precio de la consulta que paga la obra social. El paciente paga el resto; si el médico no acepta su obra social, paga particular. |
| **Particular** | Paciente sin obra social, o con una que el médico no acepta: paga el precio completo. |
| **Baja lógica** | "Borrar" sin borrar: el registro queda en la base con `Activo = false` y la aplicación deja de mostrarlo. |
| **SP (procedimiento almacenado)** | Código SQL guardado en la base (`sp_...`). Acá calculan reportes y el resumen del día. |
| **Migración** | Un archivo de `SGC.Datos/Migrations/` que describe un cambio de la base. Se aplican solas al abrir la app. |
| **Fila / DTO** | Clase que junta lo que muestra una grilla (la entidad más datos calculados): `PacienteFila`, `ReporteTurnoFila`... |
| **Service** | Clase de `SGC.Logica` que concentra las reglas de un tema (`TurnoService`...). Las pantallas solo hablan con los Services. |
| **Rol** | Tipo de usuario: Administrador, Recepcionista o Médico. Define qué menú y qué datos ve. |

## Todos los documentos

| Documento | Contenido |
|-----------|-----------|
| [`01-arquitectura.md`](01-arquitectura.md) | Capas, arranque, patrones, sesión y roles |
| [`02-modelo-de-datos.md`](02-modelo-de-datos.md) | Tablas, relaciones, restricciones, baja lógica |
| [`03-reglas-de-negocio.md`](03-reglas-de-negocio.md) | Todas las reglas y dónde viven |
| [`04-flujos.md`](04-flujos.md) | 8 recorridos de la pantalla a la base |
| [`05-procedimientos-almacenados.md`](05-procedimientos-almacenados.md) | Los 10 procedimientos y cómo se despliegan |
| [`06-capa-ui.md`](06-capa-ui.md) | Pantallas, navegación, patrón visual, componentes |
| [`07-reportes.md`](07-reportes.md) | Las 7 pestañas, roles, gráficos, CSV |
| [`08-guia-para-modificar.md`](08-guia-para-modificar.md) | Recetas, trampas, defectos y mejoras |

## Diagramas

Cada diagrama tiene una versión en imagen (`.png`), una interactiva (`.html`, se abre en el navegador) y la fuente editable (`.json`) en [`diagramas/`](diagramas/).

| Diagrama | Imagen | Interactivo |
|----------|--------|-------------|
| Capas del SGC | [`capas.png`](diagramas/capas.png) | [`capas.html`](diagramas/capas.html) |
| Modelo de datos | [`modelo-datos.png`](diagramas/modelo-datos.png) | [`modelo-datos.html`](diagramas/modelo-datos.html) |
| Arranque y login | [`flujo-login.png`](diagramas/flujo-login.png) | [`flujo-login.html`](diagramas/flujo-login.html) |
| Asignar un turno | [`flujo-asignar-turno.png`](diagramas/flujo-asignar-turno.png) | [`flujo-asignar-turno.html`](diagramas/flujo-asignar-turno.html) |
| Modificar y cancelar | [`flujo-modificar-cancelar.png`](diagramas/flujo-modificar-cancelar.png) | [`flujo-modificar-cancelar.html`](diagramas/flujo-modificar-cancelar.html) |
| Confirmar asistencia | [`flujo-confirmar-asistencia.png`](diagramas/flujo-confirmar-asistencia.png) | [`flujo-confirmar-asistencia.html`](diagramas/flujo-confirmar-asistencia.html) |
| Atención médica | [`flujo-atencion-medica.png`](diagramas/flujo-atencion-medica.png) | [`flujo-atencion-medica.html`](diagramas/flujo-atencion-medica.html) |
| Cargar una ausencia | [`flujo-ausencia.png`](diagramas/flujo-ausencia.png) | [`flujo-ausencia.html`](diagramas/flujo-ausencia.html) |
| Generar un reporte | [`flujo-reporte.png`](diagramas/flujo-reporte.png) | [`flujo-reporte.html`](diagramas/flujo-reporte.html) |
| Pantalla de Inicio | [`flujo-inicio.png`](diagramas/flujo-inicio.png) | [`flujo-inicio.html`](diagramas/flujo-inicio.html) |
| Pantallas y servicios | [`pantallas-servicios.png`](diagramas/pantallas-servicios.png) | [`pantallas-servicios.html`](diagramas/pantallas-servicios.html) |
