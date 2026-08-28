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

Reglas generales:
- `main` siempre tiene que compilar y andar. Nunca se pushea a `main` codigo roto.
- Cada modulo grande se arma en su propia rama (`feature/nombre-modulo`), no directo en `main`.
- Pull Request en GitHub antes de mergear a `main`, para que el otro revise.

### Primera vez (clonar el repo)

```bash
git clone https://github.com/Nicoargento11/sistema-gestion-consultorio.git
cd sistema-gestion-consultorio
```

### Antes de arrancar a trabajar cada dia: traer lo ultimo de main

```bash
git checkout main
git pull
```

`checkout main` te para en la rama `main`. `pull` trae los commits nuevos que el otro haya subido desde la ultima vez. Hacer esto SIEMPRE antes de crear una rama nueva, para partir de la version mas actualizada.

### Crear tu rama de trabajo

```bash
git checkout -b feature/medicos
```

Crea la rama `feature/medicos` Y te para en ella en un solo paso (el `-b` es "branch nueva"). A partir de aca, todo lo que hagas queda en ESTA rama, sin tocar `main`.

Para confirmar en que rama estas parado en cualquier momento:
```bash
git branch
```
(la que tiene un `*` al lado es la actual)

### El ciclo de trabajo normal (repetir las veces que haga falta)

1. Hacer cambios en el codigo (Visual Studio).
2. Ver que archivos cambiaron:
   ```bash
   git status
   ```
3. Agregar los archivos que querés incluir en el commit:
   ```bash
   git add NombreDelArchivo.cs
   ```
   O, si tocaste varios archivos relacionados y querés agregarlos todos:
   ```bash
   git add .
   ```
4. Confirmar el commit con un mensaje descriptivo:
   ```bash
   git commit -m "Agrega ABM de Medicos con validaciones"
   ```
5. Subir tu rama a GitHub:
   ```bash
   git push -u origin feature/medicos
   ```
   El `-u origin feature/medicos` hace falta SOLO la primera vez que pusheas esa rama (conecta tu rama local con una remota del mismo nombre). Las veces siguientes, con `git push` alcanza.

Repetir los pasos 1 a 4 (commits chicos y frecuentes son mejores que uno gigante al final), y pushear cuando quieras guardar avance en GitHub.

### Cuando tu parte esta lista: Pull Request

1. Andá a https://github.com/Nicoargento11/sistema-gestion-consultorio en el navegador.
2. Te va a aparecer un cartel amarillo "feature/medicos had recent pushes" con un boton **"Compare & pull request"** — clic ahi.
3. Ponele un titulo/descripcion breve de que hiciste, y **"Create pull request"**.
4. El otro (o vos, si es al reves) revisa los cambios en la pestaña "Files changed", y si esta todo bien, aprieta **"Merge pull request"**.

Eso mezcla `feature/medicos` adentro de `main` en GitHub. Despues, en tu compu:
```bash
git checkout main
git pull
```
para traerte esa mezcla a tu copia local tambien.

### Si mientras tanto `main` cambio y tu rama quedo desactualizada

Si el otro mergeo algo a `main` mientras vos seguias trabajando en tu rama, conviene traer esos cambios a tu rama antes de que se acumule diferencia:

```bash
git checkout main
git pull
git checkout feature/medicos
git merge main
```

Si no hay conflictos, se mezcla solo. Si hay conflictos (los dos tocaron la misma linea del mismo archivo), Git te va a marcar el archivo en conflicto — ahi hay que abrirlo, elegir que parte queda (VS marca los conflictos con `<<<<<<<`, `=======`, `>>>>>>>`), guardar, `git add` al archivo resuelto, y `git commit` para cerrar el merge.

### Comandos sueltos que van a usar seguido

| Comando | Que hace |
|---|---|
| `git status` | Que archivos cambiaste, en que rama estas |
| `git log --oneline` | Historial de commits, resumido |
| `git branch` | Lista las ramas locales |
| `git checkout nombre-rama` | Cambiar de rama |
| `git pull` | Traer cambios nuevos de GitHub a tu rama actual |
| `git push` | Subir tus commits a GitHub |
