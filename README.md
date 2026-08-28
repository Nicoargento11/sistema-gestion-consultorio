# Sistema de Gestion de Consultorio (SGC)

Trabajo integrador de Taller de Programacion II. Aplicacion de escritorio en C# / .NET 8 (WinForms) con arquitectura en capas.

## Como correr el proyecto

1. Abrir `SistemaGestionConsultorio.sln` en Visual Studio 2022 (con la carga de trabajo "Desarrollo de escritorio de .NET" instalada).
2. Confirmar que **SGC.UI** sea el proyecto de inicio.
3. `Ctrl+F5` para correr.

### Usuarios de prueba (login)

| Usuario     | Contrasena      | Rol            |
|-------------|-----------------|----------------|
| admin       | admin123        | Administrador  |
| recepcion   | recepcion123    | Recepcionista  |
| medico      | medico123       | Medico         |

Todavia no hay base de datos conectada: los datos (usuarios, pacientes, medicos, turnos) viven en listas en memoria dentro de cada Service de `SGC.Logica`, y se pierden al cerrar la app. Es intencional para esta etapa (prioridad: pantallas funcionando).

## Arquitectura

```
SGC.UI          -> Windows Forms (pantallas)
SGC.Logica      -> Reglas de negocio (Services)
SGC.Datos       -> DbContext de EF Core (todavia sin conectar a SQL Server real)
SGC.Entidades   -> Clases del dominio (Paciente, Medico, Turno, etc.) - sin dependencias de nada
```

Regla de dependencias: cada capa solo referencia a la de abajo. `SGC.Entidades` no referencia a nadie.

## Estado actual (quien hizo que)

- **Login + Menu principal con navegacion por rol**: hecho.
- **Modulo Recepcionista** (Pacientes ABM + Turnos): hecho.
- **Modulo Administrador** (Medicos ABM + Horarios): pendiente.
- **Modulo Medico** (Agenda, Registrar Actividad, Historial): pendiente.

## Como agregar una pantalla nueva (ABM)

El patron ya esta armado y probado en Pacientes. Para copiarlo (ej: Medicos):

1. **Entidad**: ya deberia existir en `SGC.Entidades` (revisar antes de crear una nueva).
2. **Service** en `SGC.Logica`: copiar la forma de `PacienteService.cs` (lista estatica en memoria + `Agregar`/`Modificar`/`EliminarLogico` con validaciones que tiran excepciones descriptivas). Si ya existe un Service de solo lectura para ese tipo (ej: `MedicoService` ya existe, usado por Turnos), **extenderlo**, no crear uno nuevo.
3. **Formulario** en `SGC.UI`: copiar la estructura de `FormPacientes.Designer.cs` (layout: panel de formulario arriba + grilla abajo) y `FormPacientes.cs` (constructor -> `ConfigurarColumnas()` -> `CargarGrilla()`, mas los handlers de los botones con `try/catch`).
4. Conectar el boton correspondiente en `FormMenuPrincipal.cs` (reemplazar el `AbrirPantallaPendiente(...)` de ese boton por `new FormNuevo().ShowDialog()`).

## Reglas importantes para evitar romper cosas (leer antes de tocar Designer.cs)

### 1. NUNCA pongas metodos propios ni columnas de DataGridView dentro de `InitializeComponent()`

El diseñador visual de Visual Studio regenera `*.Designer.cs` cada vez que abris un formulario en modo diseño, y **borra en silencio** cualquier cosa que no sea una simple asignacion de propiedades (`control.Propiedad = valor;`). Esto incluye:
- Metodos propios llamados desde `InitializeComponent()`.
- Columnas de `DataGridView` (`.Columns.Add(...)`), incluso declaradas como campos separados.

**Regla**: toda columna de grilla, y cualquier logica que no sea "crear un control y setearle propiedades", va en el archivo `.cs` normal (no en `.Designer.cs`), tipicamente en un metodo `ConfigurarColumnas()` llamado desde el constructor. Mirar `FormPacientes.cs` o `FormTurnos.cs` como ejemplo.

### 2. Evitar tildes y "ñ" en strings del codigo

Hubo problemas recurrentes de codificacion (los caracteres se corrompian al guardar). Hasta que se resuelva de raiz, escribir mensajes al usuario sin tildes ni ñ (ej: "Contrasena incorrecta" en vez de "Contraseña incorrecta"). No afecta el funcionamiento, es solo prolijidad del texto.

### 3. Orden de Tab y Enter para guardar

Cada pantalla nueva deberia tener:
- `TabIndex` de los campos en orden logico (0, 1, 2... segun el orden visual real, no el orden en que se agregaron al codigo).
- `AcceptButton = BtnGuardar;` (o el boton principal correspondiente) en el constructor, para que Enter dispare la accion principal sin tener que tocar el mouse.

### 4. Patron de guardado en grillas con seleccion automatica

Al reasignar `DataSource` de una grilla (`CargarGrilla()`), WinForms selecciona sola la primera fila y dispara `SelectionChanged`. Si el formulario usa esa seleccion para decidir "alta vs edicion" (variable tipo `_idSeleccionado`), hay que resetearla explicitamente despues de un guardado/eliminado exitoso, sino el siguiente alta se confunde con una edicion. Ver `BtnGuardar_Click` en `FormPacientes.cs`.

## Git: como nos vamos a manejar

- `main` siempre tiene que compilar y andar.
- Cada modulo grande se arma en su propia rama (`feature/nombre-modulo`), no directo en `main`.
- Pull Request en GitHub antes de mergear a `main`, para que el otro revise.
