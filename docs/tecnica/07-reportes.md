# Módulo de reportes

Acá vas a encontrar cómo funciona la pantalla **Reportes**: las 7 pestañas (cuáles son nuestras y cuáles de Luciano Villordo), qué ve cada rol, cómo se arma cada reporte desde la base hasta la grilla, cómo se dibujan los gráficos, cómo se exporta a CSV y cómo agregar una pestaña nueva. Sirve para responder "¿de dónde sale este número?" y "¿cómo agrego otro reporte?".

> Verificado contra `SGC.UI/FormReportes.cs`, `SGC.UI/FormReportes.Estadisticas.cs`, `SGC.Logica/ReporteService.cs` y los `.sql`. El flujo general (pantalla → Service → procedimiento) está dibujado en [`04-flujos.md`](04-flujos.md#g-generar-un-reporte); los procedimientos, en [`05-procedimientos-almacenados.md`](05-procedimientos-almacenados.md).

## 1. Las 7 pestañas

`FormReportes` es **una sola pantalla** con un `TabControl`. Antes eran dos módulos (el nuestro y el de Luciano); se unificaron y cada pestaña conserva su procedimiento y su autor.

| # | Pestaña | De quién | Procedimiento | Filtros | Gráfico |
|---|---------|----------|---------------|---------|---------|
| 1 | Turnos | Nuestra | `sp_ReporteTurnosPorPeriodo` | Desde, Hasta, Médico, Estado | No |
| 2 | Recaudación | Nuestra | `sp_ReporteRecaudacion` | Desde, Hasta, Agrupar por (médico, medio de pago, obra social) | Barras (control `Chart`) |
| 3 | Asistencia | Nuestra | `sp_ReporteAsistencia` | Desde, Hasta, Médico | No |
| 4 | Ocupación de agenda | Nuestra | `sp_ReporteOcupacionAgenda` | Desde, Hasta (máx. 366 días), Médico | Barras (control `Chart`) |
| 5 | Pacientes por mes | Luciano | `sp_ReportePacientesPorMes` | Año, Médico | Barras por mes (GDI+ a mano) |
| 6 | Rendimiento de médicos | Luciano | `sp_ReporteRendimientoMedicos` | Año, Mes (opcional) | No |
| 7 | Obras sociales | Luciano | `sp_ReporteTurnosPorObraSocial` | Año | Barras horizontales (GDI+ a mano) |

- Las pestañas 1 a 4 se arman en `FormReportes.cs`; las 5 a 7 (las "por año") en `FormReportes.Estadisticas.cs`, que es una clase parcial del mismo formulario. El autor de cada procedimiento se ve con `git log -- SGC.Datos/StoredProcedures/<archivo>.sql`.
- Las 5 a 7 "se calculan solas" al abrir la pestaña por primera vez (`GenerarEstadisticasAlAbrir`) y se recalculan al cambiar año o mes (`RegenerarSiCorresponde`). Las 1 a 4 esperan el botón **Generar**.
- Ninguna pestaña incluye datos clínicos (RNF#07): ni los procedimientos ni los DTO tienen motivo, procedimiento ni receta.

## 2. Reglas por rol

Se verifican en `FormReportes.ConstruirPestaniasPorRol` y `ConstruirPestaniasEstadisticas`: **a cada rol solo se le agregan las pestañas permitidas** al `TabControl`.

| Pestaña | Administrador | Recepcionista | Médico |
|---------|:-:|:-:|:-:|
| Turnos | Sí (todos los médicos) | Sí (sus asignados) | Sí (solo el propio) |
| Recaudación | **Sí** | No | No |
| Asistencia | Sí | No | Sí (solo el propio) |
| Ocupación de agenda | Sí | No | Sí (solo el propio) |
| Pacientes por mes | Sí | Sí (sus asignados) | Sí (solo el propio) |
| Rendimiento de médicos | **Sí** | No | No |
| Obras sociales | Sí | **Sí (global)** | No |
| **Total de pestañas** | **7** | **3** | **4** |

Cómo se hace cumplir el alcance de médicos:

| Rol | Combo "Médico" | Qué se consulta |
|-----|----------------|-----------------|
| Administrador | "Todos" + todos los médicos | `medicoId = null` (todos) o el elegido |
| Recepcionista | "Todos mis médicos" + sus asignados | Un médico elegido; con "Todos mis médicos" se ejecuta el procedimiento **una vez por médico asignado** y se unen los resultados (`GenerarTurnos`, `GenerarMensual`) |
| Médico | Un solo ítem (él mismo), deshabilitado | `MedicoSeleccionado` devuelve siempre `_usuarioActivo.MedicoId`, **sin mirar el combo** (defensa en profundidad) |

Casos sin acceso (`_sinAcceso`): un Médico sin `MedicoId` o una Recepcionista sin médicos asignados ven un mensaje de error y todos los botones **Generar** quedan deshabilitados.

**Por qué Recaudación y Rendimiento son solo del Administrador:** `sp_ReporteRecaudacion` no admite filtrar por médico, así que no se puede limitar a "los suyos"; y el rendimiento compara a todos los profesionales.

> **Importante:** estas restricciones viven **solo en la pantalla**. Ni `ReporteService` ni los procedimientos validan el rol (el comentario de `ReporteService` lo dice: "restringir a qué médicos puede ver cada usuario es responsabilidad de la UI").

### Hallazgo: "Obras sociales" y la Recepcionista

La pestaña **Obras sociales** se le agrega a todo rol distinto de Médico (`Rol != RolUsuario.Medico`), es decir, también a la Recepcionista. Pero `sp_ReporteTurnosPorObraSocial` recibe solo `@Anio`: **no filtra por médico**. Resultado:

- La Recepcionista ve los turnos de **todos los médicos** (no solo los asignados), contradiciendo lo que dice el README ("Reportes de sus médicos asignados").
- Ve también la columna **"Total recaudado"** de la grilla (`AgregarColumnasObrasSociales` la agrega siempre). Solo el contador "Facturación anual" está limitado al Administrador.

Cómo arreglarlo (cualquiera de las dos): ocultar la columna y los datos de plata para la Recepcionista; o, mejor, agregar `@MedicoId` al procedimiento y reutilizar el mismo mecanismo que "Pacientes por mes" (una consulta por médico asignado). Está en la lista de [`08-guia-para-modificar.md`](08-guia-para-modificar.md#defectos-y-deuda-conocidos).

## 3. Cómo se arma un reporte

### La clase `VistaReporte`

Para tratar las 7 pestañas igual, `FormReportes` define una clase privada `VistaReporte` que agrupa todo lo de una pestaña: la página (`TabPage`), los filtros (`Desde`, `Hasta`, `Anio`, `CboMes`, `CboMedico`, `CboOpcion`), los botones (`BtnGenerar`, `BtnExportar`), la grilla, el resumen, el gráfico y los datos de exportación (`Encabezados`, `FilasExportacion`).

`CrearVista(...)` construye una pestaña completa **por código** (filtros, grilla, panel de resumen, botones) y la devuelve. Por eso agregar una pestaña es agregar una llamada y no diseñar controles (ver sección 6). Todo se arma en código, nunca en el Designer, para que Visual Studio no lo borre.

### De "Generar" a la grilla

1. `Generar(vista)` valida (`ValidarFiltros`: desde ≤ hasta; en Ocupación, máximo 366 días), deshabilita el botón y pone el cursor de espera.
2. Despacha según la vista a `GenerarTurnos`, `GenerarRecaudacion`, `GenerarAsistencia`, `GenerarOcupacion` o `GenerarEstadisticas` (que a su vez elige entre Pacientes por mes, Obras sociales y Rendimiento).
3. Cada `Generar*` llama a un método de `ReporteService` (que ejecuta el procedimiento y devuelve una lista de DTO), la asigna a la grilla con `MostrarDatos`, calcula el texto del resumen y guarda lo que va a exportar.
4. Devuelve la cantidad de filas: con 0 filas se muestra "No hay datos para el periodo seleccionado." y el botón **Exportar** queda deshabilitado.
5. Los errores se separan: `ArgumentException` (por ejemplo fechas invertidas) se muestra tal cual; cualquier otra excepción se muestra como "No se pudo generar el reporte..." y `LimpiarResultado` vacía la pestaña.

### Cómo llega el dato desde SQL

`ReporteService` usa dos formas de ejecutar (detalle en [`05`](05-procedimientos-almacenados.md#24-tres-formas-de-invocar-un-procedimiento-desde-c)):

| Pestañas | Forma | Detalle |
|----------|-------|---------|
| 1 a 4 | `Database.SqlQueryRaw<T>("EXEC sp_...", parámetros)` | EF mapea las columnas a las propiedades del DTO por **nombre** |
| 5 a 7 | `DbCommand` con `CommandType.StoredProcedure` y `SqlDataReader` | Se lee columna por columna (`reader.GetInt32(reader.GetOrdinal("..."))`) |

Un parámetro opcional sin valor tiene que viajar como `DBNull.Value`, no como `null`: para eso existen `ParametroEntero` y `ParametroFecha`.

### Qué muestra cada pestaña

| Pestaña | Resumen / contadores | Detalle a tener en cuenta |
|---------|----------------------|---------------------------|
| Turnos | Total, asistieron, ausentes, cancelados y monto total | Cuenta por **texto**: compara `EstadoNombre == "Asistió"` (con tilde). Si se cambia el texto en el `.sql` hay que cambiarlo acá. |
| Recaudación | Monto total, turnos cobrados | Solo cuenta turnos `Asistio` con monto. El encabezado de la primera columna cambia según el agrupamiento. |
| Asistencia | Total, atendidos, ausentes, % de asistencia | El % es `atendidos / (atendidos + ausentes)`: los cancelados no entran. |
| Ocupación | Horas configuradas, ocupadas y % global | Descuenta de la agenda las ausencias cargadas. |
| Pacientes por mes | Contadores de chips (total, atendidos, ausentes, cancelados, "pendientes o confirmados") y, solo Admin, "Facturación anual" | "Pendientes o confirmados" suma los estados 0 y 1 juntos (columna `PendientesOConfirmados`). La facturación sale de `ObtenerTurnosPorObraSocial(anio)`. |
| Rendimiento de médicos | Cantidad de médicos, turnos, atendidos, ausentes, recaudado | Muestra todos los médicos activos, incluso con 0 turnos. |
| Obras sociales | Cantidad de obras sociales, turnos y (Admin) facturación | Agrupa por la obra social **actual** del paciente, no por lo cobrado (ver hallazgo 3 de [`05`](05-procedimientos-almacenados.md)). |

## 4. Los gráficos

Hay **dos tecnologías** distintas en la misma pantalla:

| | Control `Chart` | Dibujo a mano con GDI+ |
|---|-----------------|------------------------|
| Pestañas | Recaudación, Ocupación de agenda | Pacientes por mes, Obras sociales |
| Autor | Nuestro | Luciano |
| Paquete | `WinForms.DataVisualization` 1.10.2 (NuGet, referenciado en `SGC.UI.csproj`) | Solo `System.Drawing` |
| Dónde | `FormReportes.CrearGrafico`, `ActualizarGrafico` | `PanelGrafico` + `DibujarGraficoBarrasMensual` / `DibujarGraficoBarrasObrasSociales` |

### Control `Chart`

`ActualizarGrafico` borra los puntos de la serie y agrega uno por fila con `serie.Points.AddY(valor)` y le pone el texto con `AxisLabel`. Se hace así a propósito: con `AddXY` y texto como X, todas las barras caían en la misma posición y se superponían (lo aclara el comentario del código). El ancho del gráfico se recalcula con el de la pestaña (36 %, mínimo `EscalaDpi.Px(this, 280)`).

### GDI+ a mano (`PanelGrafico`)

`PanelGrafico` es un `Panel` con `DoubleBuffered = true` y `ResizeRedraw = true` (no parpadea y se redibuja al cambiar de tamaño). No dibuja nada por sí solo: en `OnPaint` llama a un `Action<Graphics, Rectangle>` (`Dibujar`) que le pone el formulario. Para actualizar un gráfico basta `panel.Invalidate()` después de cambiar los datos.

Cómo se respeta la **escala de pantalla (DPI)**:

- Cada método de dibujo define `int px(int valor) => EscalaDpi.Px(panel, valor)` y mide todo con él (márgenes, ancho máximo de barra, alto mínimo).
- Los textos se miden con `TextRenderer.MeasureText` para ubicar etiquetas y valores, no con tamaños fijos.
- Si el panel es bajo (menos de `px(260)`), se omite el título para que el plano conserve altura (`compacto`).
- El eje Y usa un paso "redondo" (1, 2 o 5 × 10^k) con a lo sumo 5 divisiones (`PasoEje`).
- "Obras sociales" dibuja solo las 7 con más turnos (`MaxObrasSocialesEnGrafico`) y lo aclara en el título.

Cómo se **liberan los recursos**: `OnPaint` se ejecuta muchas veces (cada vez que se mueve o redimensiona la ventana), y cada `Font`, `Pen` y `SolidBrush` ocupa un recurso de Windows (GDI). Por eso los métodos de dibujo crean todo con `using var` (por ejemplo `using var fuenteEtiqueta = new Font(...)`, `using var lapizGrilla = new Pen(...)`), así se liberan al terminar cada pintado. Si se agrega un gráfico nuevo, hay que seguir esa regla; un `new Font(...)` sin `using` dentro de `Dibujar` pierde un handle por cada repintado.

Si no hay datos, el panel muestra un mensaje centrado ("Genere el reporte para ver el gráfico." o "No hay registros...").

## 5. Exportación a CSV

El botón **Exportar a CSV** (a la derecha del resumen) se habilita cuando el reporte tiene filas.

| Paso | Dónde | Qué hace |
|------|-------|----------|
| 1 | `FormReportes.ExportarCsv` | Abre un `SaveFileDialog` con nombre sugerido `reporte_<pestaña>_<desde>_<hasta>.csv`. |
| 2 | `FormReportes.EscribirCsv` | Escribe `Encabezados` + `FilasExportacion` con **separador `;`** y **UTF-8 con BOM** (Excel configurado en es-AR lo abre bien y muestra las tildes). |
| 3 | `FormReportes.EscaparCsv` | Si el valor tiene `;`, comillas o saltos de línea, lo entrecomilla; y si empieza con `=`, `+` o `@` le antepone `'` para que Excel no lo interprete como fórmula (CSV injection). |
| 4 | Manejo de errores | Distingue falta de permisos (`UnauthorizedAccessException`), archivo abierto en otro programa (`IOException`) y error inesperado. |

Los números se exportan con `ToString("0.00", CulturaAr)` (con coma decimal). **No verificado:** el `-` inicial no está en la lista de caracteres que se escapan; no se probó cómo lo trata Excel.

El CSV exporta lo que se **generó**, no lo que muestra la grilla ahora: `FilasExportacion` se guarda en `Generar*`. Por eso, si se cambian los filtros sin volver a generar, el archivo sigue siendo el del último "Generar".

## 6. Cómo agregar una pestaña nueva

Ejemplo: un reporte "Turnos por especialidad". Los pasos, en orden:

| # | Paso | Dónde |
|---|------|-------|
| 1 | Escribir el procedimiento y crear su migración (`CREATE OR ALTER`) | [`05`](05-procedimientos-almacenados.md#4-cómo-agregar-o-cambiar-un-procedimiento) |
| 2 | Crear el DTO con propiedades **con el mismo nombre** que las columnas | `SGC.Entidades/Reportes/ReporteXxxFila.cs` |
| 3 | Agregar el método en `ReporteService` (con `SqlQueryRaw<T>` y `ParametroEntero`/`ParametroFecha`) | `SGC.Logica/ReporteService.cs` |
| 4 | Declarar el campo `private VistaReporte? _vistaXxx;` | `FormReportes.cs` |
| 5 | Crearla con `CrearVista("Clave", "Título", conMedico: ..., etiquetaOpcion: ..., conGrafico: ..., limitaUnAnio: ...)`, **dentro de la condición de rol** que corresponda, y agregar su `TabPage` a `TabReportes.TabPages` | `ConstruirPestaniasPorRol` |
| 6 | Escribir `AgregarColumnasXxx(grilla)` con `Col(...)` (nombre, encabezado, propiedad del DTO, peso, formato) | `FormReportes.cs` |
| 7 | Agregarla al `yield return` de `TodasLasVistas()` | `FormReportes.cs` |
| 8 | Agregar la rama en `Generar` y escribir `GenerarXxx(vista, desde, hasta)`: llamar al Service, `MostrarDatos(vista, datos)`, armar `LblResumen`, y **completar `vista.Encabezados` y `vista.FilasExportacion`** (si no, no exporta) | `FormReportes.cs` |
| 9 | Si lleva un combo propio, cargarlo en `CargarCombos` | `FormReportes.cs` |
| 10 | Probar con **los tres roles**: la pestaña no debe aparecer donde no corresponde y los datos deben respetar el alcance | — |

Para un reporte "por año" como los de Luciano, en el paso 5 se pasa `porAnio: new OpcionesVistaAnio(ConMes: ..., ConGrafico: ...)` y la lógica va en `FormReportes.Estadisticas.cs` (rama nueva en `GenerarEstadisticas`).

**Regla de oro:** si el reporte tiene que respetar el alcance por rol, hacelo en la pantalla (como el resto) **y pensá qué pasa con la Recepcionista**: si el procedimiento no recibe médico, va a mostrar datos de todos (el problema de "Obras sociales").

## Para seguir leyendo

- Cada procedimiento por dentro: [`05-procedimientos-almacenados.md`](05-procedimientos-almacenados.md)
- Cómo escalan los controles en pantalla: [`06-capa-ui.md`](06-capa-ui.md#6-escala-de-pantalla-dpi)
- Recetas completas: [`08-guia-para-modificar.md`](08-guia-para-modificar.md)
