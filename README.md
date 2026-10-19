# Sistema de Gestión de Consultorio (SGC)

Trabajo integrador de Taller de Programación II. Aplicación de escritorio en C# / .NET 8 (WinForms) con arquitectura en capas, Entity Framework Core y SQL Server.

> **¿Querés entender cómo funciona por dentro?** Empezá por [`docs/tecnica/00-indice.md`](docs/tecnica/00-indice.md): arquitectura, modelo de datos, reglas de negocio, flujos, procedimientos almacenados y guía para modificar el código.

## Cómo correr el proyecto

1. Tener **SQL Server** corriendo (Express, Developer o una instancia con nombre). La app **crea la base y aplica las migraciones sola** al arrancar, y siembra los datos iniciales si está vacía.
2. Copiar `appsettings.example.json` como `appsettings.Local.json` (mismo nivel, junto al `.sln`) y poner tu propia instancia en `ConnectionString`. Ese archivo **no se sube a git**: cada integrante tiene el suyo. Si no existe, la app prueba con `Server=.`.
3. Abrir `SistemaGestionConsultorio.sln` en Visual Studio 2022 (carga de trabajo "Desarrollo de escritorio de .NET").
4. Confirmar que **SGC.UI** sea el proyecto de inicio y correr con `Ctrl+F5`.

### Usuarios de prueba (login)

| Usuario     | Contraseña      | Rol            | Qué ve                                                        |
|-------------|-----------------|----------------|----------------------------------------------------------------|
| admin       | admin123        | Administrador  | Médicos, Horarios, Usuarios, Obras sociales, Vacaciones y ausencias, Reportes |
| recepcion   | recepcion123    | Recepcionista  | Pacientes, Turnos, Reportes (de sus médicos asignados; el de Obras sociales es global) |
| medico      | medico123       | Médico         | Mi agenda, Registrar actividad, Historial clínico, Reportes (solo los suyos) |

Todos ven además la pantalla de **Inicio** con el resumen del día según su rol.

### Datos de demostración (opcional)

Con la base vacía los reportes no dicen mucho. En `SGC.Datos/Scripts/` hay dos scripts que **no son migraciones** y se corren a mano:

- `carga_masiva_demo.sql`: carga pacientes, ~900 turnos de varios meses (con asistencias, ausencias, cancelaciones y cobros), historial clínico y ausencias de agenda.
- `descarga_masiva_demo.sql`: borra únicamente lo que cargó el anterior.

```
sqlcmd -S .\TU_INSTANCIA -E -C -d SGC -f 65001 -i SGC.Datos\Scripts\carga_masiva_demo.sql
```

## Arquitectura

```
SGC.UI          -> Windows Forms (pantallas)
SGC.Logica      -> Reglas de negocio (Services)
SGC.Datos       -> DbContext de EF Core, migraciones, procedimientos almacenados y datos iniciales
SGC.Entidades   -> Clases del dominio (Paciente, Medico, Turno, etc.) - sin dependencias de nada
```

Regla de dependencias: cada capa solo referencia a la de abajo. `SGC.Entidades` no referencia a nadie.

- **Datos:** SQL Server vía EF Core. Cada operación abre su propio contexto con `SGCContextFactory.Crear()`.
- **Procedimientos almacenados:** los reportes, el resumen del día y la agenda del médico se calculan en la base (`sp_*`). Se crean con migraciones (`CREATE OR ALTER`), por eso no hay que ejecutar nada a mano.
- **Roles:** Administrador, Recepcionista y Médico. Los datos clínicos solo los ve el rol Médico.

## Estado actual

| Módulo | Estado |
|---|---|
| Login + menú principal con navegación por rol | Hecho |
| Recepcionista: Pacientes, Turnos (asignar, modificar, cancelar, confirmar asistencia) | Hecho |
| Administrador: Médicos, Horarios, Usuarios, Obras sociales, Vacaciones y ausencias | Hecho |
| Médico: Mi agenda, Registrar actividad, Historial clínico | Hecho |
| Inicio (resumen del día) y Reportes con estadísticas, gráficos y exportación a CSV | Hecho |

Pendientes y defectos conocidos: ver `docs/tecnica/08-guia-para-modificar.md`.

## Cómo agregar una pantalla nueva

Las pantallas del rol Administrador y Recepcionista comparten un diseño: **tarjeta numerada a la izquierda** (formulario), **barra de herramientas, contadores y grilla a la derecha** y una **franja de mensajes** abajo. Mirá `FormPacientes.cs` o `FormTurnos.cs` como modelo.

1. **Entidad**: revisar si ya existe en `SGC.Entidades`. Si cambia el modelo, crear una migración (`dotnet ef migrations add`).
2. **Service** en `SGC.Logica`: copiar la forma de `PacienteService.cs` (validaciones que lanzan excepciones descriptivas, baja lógica con `Activo`). Si ya existe un Service para ese tipo, **extenderlo**.
3. **Formulario** en `SGC.UI`: reutilizar los componentes compartidos (`EstiloUi`, `FranjaMensaje`, `BarraContadores`, `GrillaUi`, `EscalaDpi`) en lugar de copiar estilos.
4. Conectar el botón en `FormMenuPrincipal.cs`.

## Reglas importantes para evitar romper cosas (leer antes de tocar Designer.cs)

### 1. NUNCA pongas métodos propios ni columnas de DataGridView dentro de `InitializeComponent()`

El diseñador visual de Visual Studio regenera `*.Designer.cs` cada vez que abrís un formulario en modo diseño, y **borra en silencio** cualquier cosa que no sea una simple asignación de propiedades (`control.Propiedad = valor;`). Esto incluye:
- Métodos propios llamados desde `InitializeComponent()`.
- Columnas de `DataGridView` (`.Columns.Add(...)`), incluso declaradas como campos separados.

**Regla**: toda columna de grilla, y cualquier lógica que no sea "crear un control y setearle propiedades", va en el archivo `.cs` normal (no en `.Designer.cs`), típicamente en un método `ConfigurarColumnas()` llamado desde el constructor. El `.Designer.cs` solo tiene la estructura estática con controles estándar.

### 2. Textos y comentarios

- Los **textos que ve el usuario** van en español **con tildes y ñ** (RNF#05).
- Los **comentarios del código** van en español **sin tildes**, para evitar problemas de codificación al guardar.

### 3. Tamaños y pantallas con escala (DPI)

Con escala de pantalla del 125 % o 150 % las fuentes crecen y un tamaño fijo en píxeles recorta el texto. Usá `Dock`, `TableLayoutPanel`, `FlowLayoutPanel`, `AutoSize` y `EscalaDpi.Px(...)` en vez de coordenadas fijas.

### 4. Orden de Tab y Enter para guardar

Cada pantalla nueva debería tener:
- `TabIndex` de los campos en orden lógico (según el orden visual real, no el orden en que se agregaron al código).
- `AcceptButton = BtnGuardar;` (o el botón principal correspondiente) en el constructor, para que Enter dispare la acción principal.

### 5. Patrón de guardado en grillas con selección automática

Al reasignar `DataSource` de una grilla (`CargarGrilla()`), WinForms selecciona sola la primera fila y dispara `SelectionChanged`. Si el formulario usa esa selección para decidir "alta vs edición" (variable tipo `_idSeleccionado`), hay que resetearla explícitamente después de un guardado/eliminado exitoso, y **desenganchar `SelectionChanged` antes de reasignar el `DataSource`**. Ver `FormPacientes.cs`.

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
