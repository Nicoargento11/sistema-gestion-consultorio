using System.Globalization;
using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

// ABM de medicos (Administrador). Izquierda: tarjeta "Nuevo medico" (pasa a
// "Editar medico" al elegir una fila) con datos personales, especialidad,
// precio de consulta particular y obras sociales que acepta. Derecha: buscador,
// contadores, grilla y barra del medico seleccionado (Eliminar).
// La estructura estatica vive en el Designer; las columnas, los chips y el
// estilo de botones se arman aca para que el disenador no los borre.
public partial class FormMedicos : Form
{
    private readonly MedicoService _service = new();
    private readonly ObraSocialService _obraSocialService = new();
    private static readonly CultureInfo CulturaAr = new("es-AR");

    // Id del medico cargado en la tarjeta (null = modo "nuevo").
    private int? _idSeleccionado = null;

    // Listado completo traido del servicio; el buscador filtra sobre esta lista.
    private List<MedicoFila> _todos = new();

    // true mientras se cargan o sincronizan controles por codigo: los eventos
    // de seleccion y busqueda se ignoran para no recargar a mitad de camino.
    private bool _cargando = true;

    private FranjaMensaje _mensajes = null!;
    private BarraContadores _contadores = null!;
    private readonly ToolTip _tooltip = new();
    private Font? _fuenteNegritaCache;
    private Font _fuenteNegrita => _fuenteNegritaCache ??= new Font(DgvMedicos.Font, FontStyle.Bold);

    private const string ContTotal = "Total";
    private const string ContHoy = "ConTurnosHoy";
    private const string ContMes = "ConTurnosMes";
    private const string ContSinObras = "SinObrasSociales";

    private MedicoFila? FilaSeleccionada =>
        DgvMedicos.SelectedRows.Count > 0 ? DgvMedicos.SelectedRows[0].DataBoundItem as MedicoFila : null;

    public FormMedicos()
    {
        InitializeComponent();
        EstiloUi.AlinearBotonesConScroll(pnlCampos, pnlBotones);

        _mensajes = new FranjaMensaje(pnlEstado, LblMensaje) { ReacomodarContenedor = true };
        _contadores = new BarraContadores(FlpContadores);

        AcceptButton = BtnGuardar;
        // Enter dentro del buscador no debe guardar la tarjeta.
        TxtBuscar.Enter += (s, e) => AcceptButton = null;
        TxtBuscar.Leave += (s, e) => AcceptButton = BtnGuardar;

        BtnNuevo.Click += BtnNuevo_Click;
        BtnGuardar.Click += BtnGuardar_Click;
        BtnEliminar.Click += BtnEliminar_Click;
        DgvMedicos.SelectionChanged += DgvMedicos_SelectionChanged;
        DgvMedicos.CellFormatting += DgvMedicos_CellFormatting;
        DgvMedicos.CellToolTipTextNeeded += DgvMedicos_CellToolTipTextNeeded;
        TxtBuscar.TextChanged += TxtBuscar_TextChanged;

        TxtNombre.KeyPress += FiltrosTexto.SoloLetras;
        TxtApellido.KeyPress += FiltrosTexto.SoloLetras;
        TxtDni.KeyPress += FiltrosTexto.SoloNumeros;
        CboEspecialidad.KeyPress += FiltrosTexto.SoloLetras;

        // Los textos y flujos con ancho limitado se recalculan cuando cambia el ancho.
        tlpAcciones.SizeChanged += (s, e) => AcotarEtiquetas();
        tlpDerecho.SizeChanged += (s, e) => AcotarEtiquetas();
        foreach (var flujo in new[] { flpFiltros, FlpContadores, flpBotonesAccion })
            flujo.SizeChanged += (s, e) => EstiloUi.AjustarFlujos(flpFiltros, FlpContadores, flpBotonesAccion);

        Load += FormMedicos_Load;
    }

    // ------------------------------------------------------------- arranque

    private void FormMedicos_Load(object? sender, EventArgs e)
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
    }

    private void CargarCombos()
    {
        try
        {
            ClbObrasSociales.DataSource = _obraSocialService.ObtenerTodos();
            ClbObrasSociales.DisplayMember = "Nombre";
            ClbObrasSociales.ValueMember = "Id";
        }
        catch (Exception ex)
        {
            MostrarMensaje("No se pudo cargar la lista de obras sociales: " + ex.Message, TipoMensaje.Error);
        }
    }

    private void ConfigurarColumnas()
    {
        // Se configura aca, en codigo, y no en el Designer, porque el disenador
        // visual de Visual Studio borra las columnas de un DataGridView cada vez
        // que se abre el formulario. Aca es inmune a eso. El Id no se muestra.
        GrillaUi.Estilizar(DgvMedicos);

        int texto = EscalaDpi.Px(this, 50);
        DataGridViewTextBoxColumn Columna(string nombre, string titulo, string propiedad, float peso, int minimo, string? formato = null)
        {
            var columna = GrillaUi.AgregarColumna(DgvMedicos, nombre, titulo, propiedad, peso, minimo, formato);
            // Con 9 columnas se recorta el margen extra del encabezado para que entren mas.
            columna.MinimumWidth = Math.Max(minimo, GrillaUi.MinimoEncabezado(DgvMedicos, titulo) - EscalaDpi.Px(this, 8));
            return columna;
        }

        Columna("colNombre", "Nombre", "Nombre", 80, texto);
        Columna("colApellido", "Apellido", "Apellido", 80, texto);
        Columna("colDni", "DNI", "Dni", 75, GrillaUi.MinimoPara(DgvMedicos, "00000000"));
        Columna("colMatricula", "Matrícula", "Matricula", 70, GrillaUi.MinimoPara(DgvMedicos, "MP00000"));
        Columna("colEspecialidad", "Especialidad", "Especialidad", 95, texto);
        var precio = Columna("colPrecio", "Precio particular", "PrecioConsultaParticular", 85, GrillaUi.MinimoPara(DgvMedicos, "$ 000.000,00"), "C2");
        precio.DefaultCellStyle.FormatProvider = CulturaAr;
        precio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        // El texto largo se corta con puntos suspensivos; la lista completa
        // sale en el tooltip (CellToolTipTextNeeded).
        Columna("colObrasSociales", "Obras sociales aceptadas", "ObrasSocialesTexto", 160, EscalaDpi.Px(this, 90));
        Columna("colTurnosMes", "Turnos del mes", "TurnosDelMes", 55, GrillaUi.MinimoPara(DgvMedicos, "00"))
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        Columna("colTurnosHoy", "Turnos de hoy", "TurnosDeHoy", 55, GrillaUi.MinimoPara(DgvMedicos, "00"))
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    private void ConfigurarBarraContadores()
    {
        _contadores.Agregar(ContTotal, EstiloUi.Texto, Color.White);
        _contadores.Agregar(ContHoy, PaletaEstados.Fondo(EstadoTurno.Confirmado), PaletaEstados.Acento(EstadoTurno.Confirmado));
        _contadores.Agregar(ContMes, PaletaEstados.Fondo(EstadoTurno.Asistio), PaletaEstados.Acento(EstadoTurno.Asistio));
        _contadores.Agregar(ContSinObras, PaletaEstados.Fondo(EstadoTurno.Pendiente), PaletaEstados.Acento(EstadoTurno.Pendiente));
        ActualizarContadores(new List<MedicoFila>());

        // Los contadores cuentan medicos; el texto corto se aclara en el tooltip.
        _tooltip.SetToolTip(FlpContadores.Controls.Find("lblContador" + ContTotal, false)[0], "Médicos que muestra la tabla");
        _tooltip.SetToolTip(FlpContadores.Controls.Find("lblContador" + ContHoy, false)[0], "Médicos con turnos hoy");
        _tooltip.SetToolTip(FlpContadores.Controls.Find("lblContador" + ContMes, false)[0], "Médicos con turnos en el mes actual");
        _tooltip.SetToolTip(FlpContadores.Controls.Find("lblContador" + ContSinObras, false)[0], "Médicos sin ninguna obra social aceptada");
    }

    // ------------------------------------------------------- datos y buscador

    // Trae el listado del servicio y lo muestra con el filtro de busqueda vigente.
    private void CargarGrilla()
    {
        try
        {
            _todos = _service.ObtenerListado();
        }
        catch (Exception ex)
        {
            _todos = new List<MedicoFila>();
            MostrarMensaje("No se pudo cargar el listado de médicos: " + ex.Message, TipoMensaje.Error);
        }
        AplicarFiltro();
    }

    private void TxtBuscar_TextChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;
        AplicarFiltro();
    }

    // Filtra _todos por nombre, apellido, DNI, matricula o especialidad (sin
    // distinguir mayusculas ni acentos) y vuelve a armar la grilla. Si el medico
    // en edicion queda fuera del filtro, la tarjeta vuelve al modo "nuevo".
    private void AplicarFiltro()
    {
        string texto = TxtBuscar.Text.Trim();
        var lista = string.IsNullOrEmpty(texto)
            ? _todos.ToList()
            : _todos.Where(m =>
                FiltrosTexto.Contiene($"{m.Nombre} {m.Apellido}", texto) ||
                FiltrosTexto.Contiene($"{m.Apellido} {m.Nombre}", texto) ||
                FiltrosTexto.Contiene(m.Dni, texto) ||
                FiltrosTexto.Contiene(m.Matricula, texto) ||
                FiltrosTexto.Contiene(m.Especialidad, texto)).ToList();

        bool anterior = _cargando;
        _cargando = true;
        bool reseleccionado = false;
        try
        {
            // Se desengancha SelectionChanged mientras se reasigna el DataSource
            // para que no corra con filas a medio cargar.
            DgvMedicos.SelectionChanged -= DgvMedicos_SelectionChanged;
            DgvMedicos.DataSource = lista;

            // Sin esto, WinForms selecciona sola la primera fila al asignar el
            // DataSource y precarga el formulario con ese registro.
            DgvMedicos.ClearSelection();
            DgvMedicos.CurrentCell = null;

            // Si se estaba editando un medico y sigue en la lista, queda elegido.
            if (_idSeleccionado != null)
            {
                foreach (DataGridViewRow fila in DgvMedicos.Rows)
                {
                    if (fila.DataBoundItem is MedicoFila f && f.Id == _idSeleccionado)
                    {
                        fila.Selected = true;
                        DgvMedicos.CurrentCell = fila.Cells[0];
                        reseleccionado = true;
                        break;
                    }
                }
            }
            DgvMedicos.SelectionChanged += DgvMedicos_SelectionChanged;
        }
        finally
        {
            _cargando = anterior;
        }

        ActualizarContadores(lista);
        AcotarEtiquetas();

        if (_idSeleccionado != null && !reseleccionado)
            IrAModoNuevo();
        else
            ActualizarAcciones();
    }

    // Cuenta sobre lo que muestra la grilla (con la busqueda ya aplicada).
    private void ActualizarContadores(List<MedicoFila> filas)
    {
        _contadores.Poner(ContTotal, $"Total: {filas.Count}");
        _contadores.Poner(ContHoy, $"Con turnos hoy: {filas.Count(f => f.TurnosDeHoy > 0)}");
        _contadores.Poner(ContMes, $"Con turnos este mes: {filas.Count(f => f.TurnosDelMes > 0)}");
        _contadores.Poner(ContSinObras, $"Sin obras: {filas.Count(f => f.ObrasSociales.Count == 0)}");
    }

    private void DgvMedicos_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.CellStyle == null || e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (DgvMedicos.Rows[e.RowIndex].DataBoundItem is not MedicoFila fila) return;

        // Turnos de hoy: resaltado azul (mismo tono que "Confirmado") si hay alguno.
        if (DgvMedicos.Columns[e.ColumnIndex].Name == "colTurnosHoy" && fila.TurnosDeHoy > 0)
        {
            e.CellStyle.BackColor = PaletaEstados.Fondo(EstadoTurno.Confirmado);
            e.CellStyle.ForeColor = PaletaEstados.Acento(EstadoTurno.Confirmado);
            e.CellStyle.Font = _fuenteNegrita;
        }
    }

    private void DgvMedicos_CellToolTipTextNeeded(object? sender, DataGridViewCellToolTipTextNeededEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (DgvMedicos.Columns[e.ColumnIndex].Name != "colObrasSociales") return;
        if (DgvMedicos.Rows[e.RowIndex].DataBoundItem is MedicoFila fila)
            e.ToolTipText = fila.ObrasSocialesTexto;
    }

    // ------------------------------------------------------- modo de la tarjeta

    private void LimpiarChecksObrasSociales()
    {
        for (int i = 0; i < ClbObrasSociales.Items.Count; i++)
            ClbObrasSociales.SetItemChecked(i, false);
    }

    // Vuelve al modo "nuevo medico": limpia la tarjeta y deselecciona la fila.
    private void IrAModoNuevo()
    {
        bool anterior = _cargando;
        _cargando = true;
        try
        {
            DgvMedicos.SelectionChanged -= DgvMedicos_SelectionChanged;
            DgvMedicos.ClearSelection();
            DgvMedicos.CurrentCell = null;
            DgvMedicos.SelectionChanged += DgvMedicos_SelectionChanged;

            _idSeleccionado = null;
            TxtNombre.Text = "";
            TxtApellido.Text = "";
            TxtDni.Text = "";
            CboEspecialidad.Text = "";
            TxtMatricula.Text = "";
            NudPrecioConsultaParticular.Value = 0;
            LimpiarChecksObrasSociales();
        }
        finally
        {
            _cargando = anterior;
        }

        ActualizarModo();
        ActualizarAcciones();
    }

    // Titulo y aviso de la tarjeta segun si hay un medico cargado para editar.
    private void ActualizarModo()
    {
        bool editando = _idSeleccionado != null;
        lblTituloTarjeta.Text = editando ? "EDITAR MÉDICO" : "NUEVO MÉDICO";

        var fila = FilaSeleccionada;
        EstiloUi.MostrarAviso(LblModo, _tooltip,
            editando ? $"Editando a «{fila?.Nombre} {fila?.Apellido}»" : "",
            editando ? $"Editando a «{fila?.Nombre} {fila?.Apellido}»: modifique los datos y pulse «Guardar»." : "");
        AcotarEtiquetas();
    }

    // Texto de la barra de seleccion, detalle y estado del boton Eliminar.
    private void ActualizarAcciones()
    {
        var fila = FilaSeleccionada;
        if (fila == null)
        {
            EstiloUi.PonerTexto(LblSeleccion, _tooltip, "Médico seleccionado: ninguno");
            BtnEliminar.Enabled = false;
            MostrarAyudaAcciones("");
            LblDetalle.Text = "Seleccione un médico para ver sus obras sociales y sus turnos.";
            AcotarEtiquetas();
            return;
        }

        EstiloUi.PonerTexto(LblSeleccion, _tooltip, $"Médico seleccionado: {fila.Apellido}, {fila.Nombre} · {fila.Especialidad}");
        BtnEliminar.Enabled = true;
        MostrarAyudaAcciones("");
        LblDetalle.Text = $"Obras sociales: {fila.ObrasSocialesTexto} · Turnos de hoy: {fila.TurnosDeHoy} · Turnos del mes: {fila.TurnosDelMes}";
        AcotarEtiquetas();
    }

    private void MostrarAyudaAcciones(string texto)
    {
        LblAyudaAcciones.Text = texto;
        LblAyudaAcciones.Visible = !string.IsNullOrEmpty(texto);
        AcotarEtiquetas();
    }

    // ------------------------------------------------------------ eventos

    private void DgvMedicos_SelectionChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;

        var fila = FilaSeleccionada;
        if (fila == null)
        {
            ActualizarAcciones();
            return;
        }

        var medico = fila.Medico;
        _cargando = true;
        try
        {
            _idSeleccionado = medico.Id;
            TxtNombre.Text = medico.Nombre;
            TxtApellido.Text = medico.Apellido;
            TxtDni.Text = medico.Dni;
            CboEspecialidad.Text = medico.Especialidad;
            TxtMatricula.Text = medico.Matricula;
            NudPrecioConsultaParticular.Value = medico.PrecioConsultaParticular;

            for (int i = 0; i < ClbObrasSociales.Items.Count; i++)
            {
                var obraSocial = (ObraSocial)ClbObrasSociales.Items[i];
                ClbObrasSociales.SetItemChecked(i, medico.ObrasSocialesAceptadasIds.Contains(obraSocial.Id));
            }
        }
        finally
        {
            _cargando = false;
        }

        LimpiarMensaje();
        ActualizarModo();
        ActualizarAcciones();
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
            var medico = new Medico
            {
                Id = _idSeleccionado ?? 0,
                Nombre = TxtNombre.Text,
                Apellido = TxtApellido.Text,
                Dni = TxtDni.Text,
                Especialidad = CboEspecialidad.Text,
                Matricula = TxtMatricula.Text,
                PrecioConsultaParticular = NudPrecioConsultaParticular.Value,
                ObrasSocialesAceptadasIds = ClbObrasSociales.CheckedItems.Cast<ObraSocial>().Select(o => o.Id).ToList()
            };
            if (_idSeleccionado == null)
            {
                _service.Agregar(medico);
            }
            else
            {
                _service.Modificar(medico);
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message, TipoMensaje.Error);
            return;
        }

        CargarGrilla();
        IrAModoNuevo();
        MostrarMensaje("Médico guardado correctamente", TipoMensaje.Ok);
    }

    private void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var fila = FilaSeleccionada;
        if (fila == null)
        {
            MostrarMensaje("Seleccione un médico de la lista primero.", TipoMensaje.Error);
            return;
        }
        var medicoSeleccionado = fila.Medico;

        var respuesta = MessageBox.Show(
            $"¿Está seguro que desea eliminar a {medicoSeleccionado.Nombre} {medicoSeleccionado.Apellido}?",
            "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (respuesta != DialogResult.Yes)
            return;

        try
        {
            _service.EliminarLogico(medicoSeleccionado.Id);
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message, TipoMensaje.Error);
            return;
        }

        CargarGrilla();
        IrAModoNuevo();
        MostrarMensaje("Médico eliminado correctamente.", TipoMensaje.Ok);
    }

    // ----------------------------------------------------- mensajes y estilo

    private void MostrarMensaje(string texto, TipoMensaje tipo) => _mensajes.Mostrar(texto, tipo);

    private void LimpiarMensaje() => _mensajes.Limpiar();

    // Limita el ancho de los textos que pueden ocupar varias lineas al de su contenedor.
    private void AcotarEtiquetas()
    {
        EstiloUi.Acotar(LblAyudaAcciones, tlpAcciones);
        EstiloUi.Acotar(LblDetalle, tlpAcciones);
        EstiloUi.AjustarFlujos(flpFiltros, FlpContadores, flpBotonesAccion);
        _mensajes.Ajustar();
    }
}
