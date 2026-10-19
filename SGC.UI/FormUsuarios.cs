using System.Globalization;
using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

// Pantalla de Usuarios (Administrador). Izquierda: tarjeta "Nuevo usuario"
// (pasa a "Editar usuario" al elegir una fila) con usuario, contrasena, rol y
// un unico espacio que muestra el medico (rol Medico) o los medicos asignados
// (rol Recepcionista). Derecha: buscador, contadores, grilla y barra del
// usuario seleccionado (Eliminar o Reactivar).
// La estructura estatica vive en el Designer; las columnas, los chips y el
// estilo de botones se arman aca para que el disenador no los borre.
public partial class FormUsuarios : Form
{
    private readonly UsuarioService _service = new();
    private readonly MedicoService _medicoService = new();

    // Id del usuario cargado en la tarjeta (null = modo "nuevo").
    private int? _idSeleccionado = null;

    // Listado completo (activos e inactivos) traido del servicio; el buscador y
    // "Mostrar inactivos" filtran sobre esta lista.
    private List<Usuario> _todos = new();

    // true mientras se cargan o sincronizan controles por codigo: los eventos
    // de seleccion y busqueda se ignoran para no recargar a mitad de camino.
    private bool _cargando = true;

    private FranjaMensaje _mensajes = null!;
    private BarraContadores _contadores = null!;

    private const string ContTotal = "Total";
    private const string ContActivos = "Activos";
    private const string ContInactivos = "Inactivos";
    private const string ContAdministradores = "Administradores";
    private const string ContMedicos = "Medicos";
    private const string ContRecepcionistas = "Recepcionistas";

    private Usuario? FilaSeleccionada =>
        DgvUsuarios.SelectedRows.Count > 0 ? DgvUsuarios.SelectedRows[0].DataBoundItem as Usuario : null;

    public FormUsuarios()
    {
        InitializeComponent();

        _mensajes = new FranjaMensaje(pnlEstado, LblMensaje) { ReacomodarContenedor = true };
        _contadores = new BarraContadores(FlpContadores);

        AcceptButton = BtnGuardar;
        // Enter dentro del buscador no debe guardar la tarjeta.
        TxtBuscar.Enter += (s, e) => AcceptButton = null;
        TxtBuscar.Leave += (s, e) => AcceptButton = BtnGuardar;

        BtnNuevo.Click += BtnNuevo_Click;
        BtnGuardar.Click += BtnGuardar_Click;
        BtnEliminar.Click += BtnEliminar_Click;
        BtnReactivar.Click += BtnReactivar_Click;
        DgvUsuarios.SelectionChanged += DgvUsuarios_SelectionChanged;
        DgvUsuarios.CellFormatting += DgvUsuarios_CellFormatting;
        TxtBuscar.TextChanged += TxtBuscar_TextChanged;
        ChkMostrarInactivos.CheckedChanged += ChkMostrarInactivos_CheckedChanged;
        CboRol.SelectedIndexChanged += CboRol_SelectedIndexChanged;
        CboRol.Format += CboRol_Format;

        TxtNombreUsuario.KeyPress += FiltrosTexto.SoloUsuario;

        // Los textos y flujos con ancho limitado se recalculan cuando cambia el ancho.
        tlpTarjeta.SizeChanged += (s, e) => AcotarEtiquetas();
        tlpToolbar.SizeChanged += (s, e) => AcotarEtiquetas();
        tlpAcciones.SizeChanged += (s, e) => AcotarEtiquetas();
        foreach (var flujo in new[] { flpFiltros, FlpContadores, flpBotonesAccion })
            flujo.SizeChanged += (s, e) => EstiloUi.AjustarFlujos(flpFiltros, FlpContadores, flpBotonesAccion);

        Load += FormUsuarios_Load;
    }

    private static string TextoRol(RolUsuario rol) => rol switch
    {
        RolUsuario.Medico => "Médico",
        RolUsuario.Recepcionista => "Recepcionista",
        _ => "Administrador"
    };

    // ------------------------------------------------------------- arranque

    private void FormUsuarios_Load(object? sender, EventArgs e)
    {
        _cargando = true;
        try
        {
            ConfigurarColumnas();
            ConfigurarBarraContadores();
            ConfigurarEstiloBotones();
            CargarCombos();
        }
        finally
        {
            _cargando = false;
        }

        CargarGrilla();
        IrAModoNuevo();

        // WinForms selecciona sola la primera fila recien cuando la grilla se
        // muestra de verdad en pantalla (despues de Load, sobre todo cuando el
        // formulario es hijo del menu). Esa seleccion automatica se ignora y se
        // limpia una vez que el formulario termino de mostrarse.
        _cargando = true;
        BeginInvoke(new Action(() =>
        {
            if (IsDisposed) return;
            _cargando = false;
            IrAModoNuevo();
        }));
    }

    private void ConfigurarEstiloBotones()
    {
        EstiloUi.EstilizarBoton(BtnGuardar, TipoBoton.Primario);
        EstiloUi.EstilizarBoton(BtnNuevo, TipoBoton.Secundario);
        EstiloUi.EstilizarBoton(BtnEliminar, TipoBoton.Peligro);
        EstiloUi.EstilizarBoton(BtnReactivar, TipoBoton.Exito);
    }

    private void ConfigurarColumnas()
    {
        // Columnas en codigo: el DataPropertyName apunta a propiedades aplanadas
        // (MedicoAsignadoNombre, MedicosAsignadosTexto) porque el DataGridView no
        // soporta rutas anidadas.
        GrillaUi.Estilizar(DgvUsuarios);

        GrillaUi.AgregarColumna(DgvUsuarios, "colNombreUsuario", "Usuario", "NombreUsuario", 130, EscalaDpi.Px(this, 70));
        GrillaUi.AgregarColumna(DgvUsuarios, "colRol", "Rol", "Rol", 100, GrillaUi.MinimoPara(DgvUsuarios, "Recepcionista"));
        GrillaUi.AgregarColumna(DgvUsuarios, "colMedico", "Médico", "MedicoAsignadoNombre", 150, EscalaDpi.Px(this, 70));
        GrillaUi.AgregarColumna(DgvUsuarios, "colMedicosAsignados", "Médicos asignados", "MedicosAsignadosTexto", 200, EscalaDpi.Px(this, 80));
        GrillaUi.AgregarColumna(DgvUsuarios, "colEstado", "Estado", "EstadoTexto", 80, GrillaUi.MinimoPara(DgvUsuarios, "Inactivo"))
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    private void ConfigurarBarraContadores()
    {
        _contadores.Agregar(ContTotal, EstiloUi.Texto, Color.White);
        _contadores.Agregar(ContActivos, PaletaEstados.Fondo(EstadoTurno.Asistio), EstiloUi.Ok);
        _contadores.Agregar(ContInactivos, PaletaEstados.Fondo(EstadoTurno.Cancelado), PaletaEstados.Acento(EstadoTurno.Cancelado));
        _contadores.Agregar(ContAdministradores, EstiloUi.BotonDeshabilitadoFondo, EstiloUi.Texto);
        _contadores.Agregar(ContMedicos, PaletaEstados.Fondo(EstadoTurno.Confirmado), PaletaEstados.Acento(EstadoTurno.Confirmado));
        _contadores.Agregar(ContRecepcionistas, PaletaEstados.Fondo(EstadoTurno.Pendiente), PaletaEstados.Acento(EstadoTurno.Pendiente));
        ActualizarContadores(new List<Usuario>(), 0);
    }

    private void CargarCombos()
    {
        CboRol.DataSource = Enum.GetValues(typeof(RolUsuario));

        // Dos listas distintas: si el combo y el checklist compartieran la misma
        // lista, WinForms sincronizaria su posicion actual.
        try
        {
            CboMedico.DataSource = _medicoService.ObtenerTodos();
            CboMedico.DisplayMember = "NombreCompleto";
            CboMedico.ValueMember = "Id";

            ClbMedicosAsignados.DataSource = _medicoService.ObtenerTodos();
            ClbMedicosAsignados.DisplayMember = "NombreCompleto";
            ClbMedicosAsignados.ValueMember = "Id";
        }
        catch (Exception ex)
        {
            MostrarMensaje("No se pudo cargar la lista de médicos: " + ex.Message, TipoMensaje.Error);
        }
    }

    private void CboRol_Format(object? sender, ListControlConvertEventArgs e)
    {
        if (e.ListItem is RolUsuario rol)
            e.Value = TextoRol(rol);
    }

    // ------------------------------------------------------- datos y buscador

    // Trae el listado completo del servicio y lo muestra con el filtro vigente.
    private void CargarGrilla()
    {
        try
        {
            _todos = _service.ObtenerTodos(true);
        }
        catch (Exception ex)
        {
            _todos = new List<Usuario>();
            MostrarMensaje("No se pudo cargar el listado de usuarios: " + ex.Message, TipoMensaje.Error);
        }
        AplicarFiltro();
    }

    private void TxtBuscar_TextChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;
        AplicarFiltro();
    }

    private void ChkMostrarInactivos_CheckedChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;
        AplicarFiltro();
    }

    private static bool Contiene(string texto, string buscado) =>
        CultureInfo.CurrentCulture.CompareInfo.IndexOf(texto, buscado, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;

    // Filtra _todos por usuario, rol o medico (sin distinguir mayusculas ni
    // acentos) y oculta los inactivos salvo que "Mostrar inactivos" este
    // tildado. Si el usuario en edicion queda fuera del filtro, la tarjeta
    // vuelve al modo "nuevo".
    private void AplicarFiltro()
    {
        string texto = TxtBuscar.Text.Trim();
        var coinciden = string.IsNullOrEmpty(texto)
            ? _todos.ToList()
            : _todos.Where(u =>
                Contiene(u.NombreUsuario, texto) ||
                Contiene(TextoRol(u.Rol), texto) ||
                Contiene(u.MedicoAsignadoNombre, texto) ||
                Contiene(u.MedicosAsignadosTexto, texto)).ToList();

        bool verInactivos = ChkMostrarInactivos.Checked;
        var lista = coinciden.Where(u => u.Activo || verInactivos).ToList();

        bool anterior = _cargando;
        _cargando = true;
        bool reseleccionado = false;
        try
        {
            // Se desengancha SelectionChanged mientras se reasigna el DataSource
            // para que no corra con filas a medio cargar.
            DgvUsuarios.SelectionChanged -= DgvUsuarios_SelectionChanged;
            DgvUsuarios.DataSource = lista;

            // Sin esto, WinForms selecciona sola la primera fila al asignar el
            // DataSource y precarga el formulario con ese registro.
            DgvUsuarios.ClearSelection();
            DgvUsuarios.CurrentCell = null;

            // Si se estaba editando un usuario y sigue en la lista, queda elegido.
            if (_idSeleccionado != null)
            {
                foreach (DataGridViewRow fila in DgvUsuarios.Rows)
                {
                    if (fila.DataBoundItem is Usuario u && u.Id == _idSeleccionado)
                    {
                        fila.Selected = true;
                        DgvUsuarios.CurrentCell = fila.Cells[0];
                        reseleccionado = true;
                        break;
                    }
                }
            }
            DgvUsuarios.SelectionChanged += DgvUsuarios_SelectionChanged;
        }
        finally
        {
            _cargando = anterior;
        }

        ActualizarContadores(lista, coinciden.Count(u => !u.Activo));

        string vista = string.IsNullOrEmpty(texto)
            ? $"Mostrando: {(verInactivos ? "todos los usuarios" : "usuarios activos")} ({lista.Count})"
            : $"Mostrando: {lista.Count} de {_todos.Count} · búsqueda «{texto}»";
        LblVista.Text = vista;
        AcotarEtiquetas();

        if (_idSeleccionado != null && !reseleccionado)
            IrAModoNuevo();
        else
            ActualizarAcciones();
    }

    // Cuenta sobre lo que muestra la grilla; los inactivos se informan aunque
    // esten ocultos (inactivosTotales = los que coinciden con la busqueda).
    private void ActualizarContadores(List<Usuario> filas, int inactivosTotales)
    {
        _contadores.Poner(ContTotal, $"Total: {filas.Count}");
        _contadores.Poner(ContActivos, $"Activos: {filas.Count(u => u.Activo)}");
        string ocultos = !ChkMostrarInactivos.Checked && inactivosTotales > 0 ? " (ocultos)" : "";
        _contadores.Poner(ContInactivos, $"Inactivos: {inactivosTotales}{ocultos}");
        _contadores.Poner(ContAdministradores, $"Administradores: {filas.Count(u => u.Rol == RolUsuario.Administrador)}");
        _contadores.Poner(ContMedicos, $"Médicos: {filas.Count(u => u.Rol == RolUsuario.Medico)}");
        _contadores.Poner(ContRecepcionistas, $"Recepcionistas: {filas.Count(u => u.Rol == RolUsuario.Recepcionista)}");
    }

    private void DgvUsuarios_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.CellStyle == null) return;
        if (DgvUsuarios.Rows[e.RowIndex].DataBoundItem is not Usuario usuario) return;

        switch (DgvUsuarios.Columns[e.ColumnIndex].Name)
        {
            case "colRol":
                e.Value = TextoRol(usuario.Rol);
                e.FormattingApplied = true;
                break;
            case "colMedico":
            case "colMedicosAsignados":
                if (e.Value is string s && s.Length == 0)
                {
                    e.Value = "-";
                    e.FormattingApplied = true;
                }
                break;
            case "colEstado":
                e.CellStyle.BackColor = usuario.Activo ? PaletaEstados.Fondo(EstadoTurno.Asistio) : PaletaEstados.Fondo(EstadoTurno.Cancelado);
                e.CellStyle.ForeColor = usuario.Activo ? EstiloUi.Ok : PaletaEstados.Acento(EstadoTurno.Cancelado);
                break;
        }

        // Los usuarios inactivos se leen atenuados.
        if (!usuario.Activo && DgvUsuarios.Columns[e.ColumnIndex].Name != "colEstado")
            e.CellStyle.ForeColor = EstiloUi.Secundario;
    }

    // ------------------------------------------------------- modo de la tarjeta

    private void ActualizarVisibilidadPorRol()
    {
        // Si no hay rol seleccionado (ej: recien apretaste "Nuevo"), se
        // ocultan los dos - antes se quedaba pegado el que estuviera visible
        // del usuario anterior que tenias seleccionado.
        if (CboRol.SelectedItem is RolUsuario rol)
        {
            CboMedico.Visible = rol == RolUsuario.Medico;
            ClbMedicosAsignados.Visible = rol == RolUsuario.Recepcionista;
        }
        else
        {
            CboMedico.Visible = false;
            ClbMedicosAsignados.Visible = false;
        }

        // Los dos controles comparten el mismo espacio de la tarjeta: solo se
        // muestra el titulo del que corresponde al rol elegido.
        lblSlotRol.Visible = CboMedico.Visible || ClbMedicosAsignados.Visible;
        lblSlotRol.Text = ClbMedicosAsignados.Visible ? "4. Médicos asignados" : "4. Médico";
    }

    // Vuelve al modo "nuevo usuario": limpia la tarjeta y deselecciona la fila.
    private void IrAModoNuevo()
    {
        bool anterior = _cargando;
        _cargando = true;
        try
        {
            DgvUsuarios.SelectionChanged -= DgvUsuarios_SelectionChanged;
            DgvUsuarios.ClearSelection();
            DgvUsuarios.CurrentCell = null;
            DgvUsuarios.SelectionChanged += DgvUsuarios_SelectionChanged;

            _idSeleccionado = null;
            TxtNombreUsuario.Text = "";
            TxtContrasena.Text = "";
            CboRol.SelectedIndex = -1;
            CboMedico.SelectedIndex = -1;
            for (int i = 0; i < ClbMedicosAsignados.Items.Count; i++)
            {
                ClbMedicosAsignados.SetItemChecked(i, false);
            }
            // Sin esto la primera fila del checklist queda resaltada como "seleccionada".
            ClbMedicosAsignados.ClearSelected();
            ActualizarVisibilidadPorRol();
        }
        finally
        {
            _cargando = anterior;
        }

        ActualizarModo();
        ActualizarAcciones();
    }

    // Titulo y avisos de la tarjeta segun si hay un usuario cargado para editar.
    private void ActualizarModo()
    {
        bool editando = _idSeleccionado != null;
        lblTituloTarjeta.Text = editando ? "EDITAR USUARIO" : "NUEVO USUARIO";

        LblModo.Visible = editando;
        LblModo.Text = editando
            ? $"Editando «{FilaSeleccionada?.NombreUsuario}»: modifique los datos y pulse «Guardar»."
            : "";

        TxtContrasena.PlaceholderText = editando ? "Dejar vacía para no cambiarla" : "";
        LblAyudaContrasena.Text = editando
            ? "Dejar vacía para no cambiarla. Si la completa, mínimo 6 caracteres."
            : "Mínimo 6 caracteres.";
        AcotarEtiquetas();
    }

    // Texto de la barra de seleccion y botones Eliminar / Reactivar.
    private void ActualizarAcciones()
    {
        var usuario = FilaSeleccionada;
        if (usuario == null)
        {
            LblSeleccion.Text = "Usuario seleccionado: ninguno";
            BtnEliminar.Visible = true;
            BtnEliminar.Enabled = false;
            BtnReactivar.Visible = false;
            MostrarAyudaAcciones("Elija una fila de la tabla.");
            LblDetalle.Text = "Seleccione un usuario para ver sus médicos.";
            AcotarEtiquetas();
            return;
        }

        LblSeleccion.Text = $"Usuario seleccionado: {usuario.NombreUsuario} · {TextoRol(usuario.Rol)} · {usuario.EstadoTexto}";
        BtnEliminar.Visible = usuario.Activo;
        BtnEliminar.Enabled = usuario.Activo;
        BtnReactivar.Visible = !usuario.Activo;
        BtnReactivar.Enabled = !usuario.Activo;
        MostrarAyudaAcciones(usuario.Activo ? "" : "Este usuario está inactivo: no puede iniciar sesión hasta reactivarlo.");

        LblDetalle.Text = usuario.Rol switch
        {
            RolUsuario.Medico => $"Médico: {(usuario.MedicoAsignadoNombre.Length > 0 ? usuario.MedicoAsignadoNombre : "sin asignar")}",
            RolUsuario.Recepcionista => $"Médicos asignados: {(usuario.MedicosAsignadosTexto.Length > 0 ? usuario.MedicosAsignadosTexto : "ninguno")}",
            _ => "Administrador: no tiene médicos asociados."
        };
        AcotarEtiquetas();
    }

    private void MostrarAyudaAcciones(string texto)
    {
        LblAyudaAcciones.Text = texto;
        LblAyudaAcciones.Visible = !string.IsNullOrEmpty(texto);
        AcotarEtiquetas();
    }

    // ------------------------------------------------------------ eventos

    private void DgvUsuarios_SelectionChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;

        var usuarioSeleccionado = FilaSeleccionada;
        if (usuarioSeleccionado == null)
        {
            ActualizarAcciones();
            return;
        }

        _cargando = true;
        try
        {
            _idSeleccionado = usuarioSeleccionado.Id;
            TxtNombreUsuario.Text = usuarioSeleccionado.NombreUsuario;

            // Nunca se prellena: el valor guardado es un hash, no la contrasena
            // real. Dejarlo en blanco significa "no cambiarla" al guardar.
            TxtContrasena.Text = "";

            // Se limpian los dos controles antes de cargar el que corresponde
            // al rol, para que no quede pegado el valor del usuario anterior.
            CboMedico.SelectedIndex = -1;
            for (int i = 0; i < ClbMedicosAsignados.Items.Count; i++)
            {
                ClbMedicosAsignados.SetItemChecked(i, false);
            }

            CboRol.SelectedItem = usuarioSeleccionado.Rol;
            if (usuarioSeleccionado.Rol == RolUsuario.Medico)
            {
                CboMedico.SelectedValue = usuarioSeleccionado.MedicoId ?? -1;
            }
            else if (usuarioSeleccionado.Rol == RolUsuario.Recepcionista)
            {
                for (int i = 0; i < ClbMedicosAsignados.Items.Count; i++)
                {
                    var medico = (Medico)ClbMedicosAsignados.Items[i];
                    ClbMedicosAsignados.SetItemChecked(i, usuarioSeleccionado.MedicosAsignadosIds.Contains(medico.Id));
                }
            }
            ClbMedicosAsignados.ClearSelected();

            ActualizarVisibilidadPorRol();
        }
        finally
        {
            _cargando = false;
        }

        LimpiarMensaje();
        ActualizarModo();
        ActualizarAcciones();
    }

    private void CboRol_SelectedIndexChanged(object? sender, EventArgs e)
    {
        ActualizarVisibilidadPorRol();
    }

    private void BtnNuevo_Click(object? sender, EventArgs e)
    {
        LimpiarMensaje();
        IrAModoNuevo();
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(TxtNombreUsuario.Text))
            {
                throw new ArgumentException("El nombre de usuario es obligatorio.");
            }
            if (CboRol.SelectedItem == null)
            {
                throw new ArgumentException("Debe seleccionar un rol.");
            }

            var usuario = new Usuario
            {
                Id = _idSeleccionado ?? 0,
                NombreUsuario = TxtNombreUsuario.Text,
                Contrasena = TxtContrasena.Text,
                Rol = (RolUsuario)CboRol.SelectedItem
            };
            if (usuario.Rol == RolUsuario.Medico)
            {
                if (CboMedico.SelectedValue == null) throw new ArgumentException("Debe seleccionar un médico.");
                usuario.MedicoId = (int)CboMedico.SelectedValue;
            }
            else if (usuario.Rol == RolUsuario.Recepcionista)
            {
                usuario.MedicosAsignadosIds = ClbMedicosAsignados.CheckedItems.Cast<Medico>().Select(m => m.Id).ToList();
            }
            if (_idSeleccionado == null)
            {
                _service.Agregar(usuario);
            }
            else
            {
                _service.Modificar(usuario);
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje($"Error: {ex.Message}", TipoMensaje.Error);
            return;
        }

        CargarGrilla();
        IrAModoNuevo();
        MostrarMensaje("Usuario guardado correctamente.", TipoMensaje.Ok);
    }

    private void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var usuarioSeleccionado = FilaSeleccionada;
        if (usuarioSeleccionado == null)
        {
            MostrarMensaje("Seleccione un usuario de la lista primero.", TipoMensaje.Error);
            return;
        }

        var respuesta = MessageBox.Show(
            $"¿Está seguro que desea eliminar al usuario \"{usuarioSeleccionado.NombreUsuario}\"?",
            "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (respuesta != DialogResult.Yes) return;

        try
        {
            _service.EliminarLogico(usuarioSeleccionado.Id);
        }
        catch (Exception ex)
        {
            MostrarMensaje($"Error: {ex.Message}", TipoMensaje.Error);
            return;
        }

        CargarGrilla();
        IrAModoNuevo();
        MostrarMensaje("Usuario eliminado correctamente.", TipoMensaje.Ok);
    }

    private void BtnReactivar_Click(object? sender, EventArgs e)
    {
        var usuarioSeleccionado = FilaSeleccionada;
        if (usuarioSeleccionado == null)
        {
            MostrarMensaje("Seleccione un usuario de la lista primero.", TipoMensaje.Error);
            return;
        }

        var respuesta = MessageBox.Show(
            $"¿Reactivar al usuario \"{usuarioSeleccionado.NombreUsuario}\"?",
            "Confirmar reactivación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (respuesta != DialogResult.Yes) return;

        try
        {
            _service.Reactivar(usuarioSeleccionado.Id);
        }
        catch (Exception ex)
        {
            MostrarMensaje($"Error: {ex.Message}", TipoMensaje.Error);
            return;
        }

        CargarGrilla();
        IrAModoNuevo();
        MostrarMensaje("Usuario reactivado correctamente.", TipoMensaje.Ok);
    }

    // ----------------------------------------------------- mensajes y estilo

    private void MostrarMensaje(string texto, TipoMensaje tipo) => _mensajes.Mostrar(texto, tipo);

    private void LimpiarMensaje() => _mensajes.Limpiar();

    // Limita el ancho de los textos que pueden ocupar varias lineas al de su contenedor.
    private void AcotarEtiquetas()
    {
        EstiloUi.Acotar(LblModo, tlpTarjeta);
        EstiloUi.Acotar(LblAyudaContrasena, tlpTarjeta);
        EstiloUi.Acotar(LblVista, tlpToolbar);
        EstiloUi.Acotar(LblSeleccion, tlpAcciones);
        EstiloUi.Acotar(LblAyudaAcciones, tlpAcciones);
        EstiloUi.Acotar(LblDetalle, tlpAcciones);
        EstiloUi.AjustarFlujos(flpFiltros, FlpContadores, flpBotonesAccion);
        _mensajes.Ajustar();
    }
}
