using System.Globalization;
using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

// Pantalla de Obras Sociales (Administrador). Izquierda: tarjeta "Nueva obra
// social" (pasa a "Editar obra social" al elegir una fila) con el flujo
// nombre -> porcentaje -> Guardar. Derecha: buscador, contadores, grilla y
// barra de la obra social seleccionada (Eliminar + medicos que la aceptan).
// La estructura estatica vive en el Designer; las columnas, los chips y el
// estilo de botones se arman aca para que el disenador no los borre.
public partial class FormObrasSociales : Form
{
    private readonly ObraSocialService _service = new();

    // Id de la obra social cargada en la tarjeta (null = modo "nueva").
    private int? _idSeleccionado = null;

    // Listado completo traido del servicio; el buscador filtra sobre esta lista.
    private List<ObraSocialFila> _todas = new();

    // true mientras se cargan o sincronizan controles por codigo: los eventos
    // de seleccion y busqueda se ignoran para no recargar a mitad de camino.
    private bool _cargando = true;

    private FranjaMensaje _mensajes = null!;
    private BarraContadores _contadores = null!;

    private const string ContTotal = "Total";
    private const string ContConMedicos = "ConMedicos";
    private const string ContConAfiliados = "ConAfiliados";

    private ObraSocialFila? FilaSeleccionada =>
        DgvObrasSociales.SelectedRows.Count > 0 ? DgvObrasSociales.SelectedRows[0].DataBoundItem as ObraSocialFila : null;

    public FormObrasSociales()
    {
        InitializeComponent();

        _mensajes = new FranjaMensaje(pnlEstado, LblMensaje) { ReacomodarContenedor = true };
        _contadores = new BarraContadores(FlpContadores);

        BtnNuevo.Click += BtnNuevo_Click;
        BtnGuardar.Click += BtnGuardar_Click;
        BtnEliminar.Click += BtnEliminar_Click;
        DgvObrasSociales.SelectionChanged += DgvObrasSociales_SelectionChanged;
        TxtBuscar.TextChanged += TxtBuscar_TextChanged;

        // Los textos y flujos con ancho limitado se recalculan cuando cambia el ancho.
        tlpTarjeta.SizeChanged += (s, e) => AcotarEtiquetas();
        tlpToolbar.SizeChanged += (s, e) => AcotarEtiquetas();
        tlpAcciones.SizeChanged += (s, e) => AcotarEtiquetas();
        foreach (var flujo in new[] { flpFiltros, FlpContadores, flpBotonesAccion })
            flujo.SizeChanged += (s, e) => EstiloUi.AjustarFlujos(flpFiltros, FlpContadores, flpBotonesAccion);

        Load += FormObrasSociales_Load;
    }

    // ------------------------------------------------------------- arranque

    private void FormObrasSociales_Load(object? sender, EventArgs e)
    {
        _cargando = true;
        try
        {
            ConfigurarColumnas();
            ConfigurarBarraContadores();
            ConfigurarEstiloBotones();
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

    private void ConfigurarColumnas()
    {
        GrillaUi.Estilizar(DgvObrasSociales);

        GrillaUi.AgregarColumna(DgvObrasSociales, "colNombre", "Nombre", "Nombre", 200, EscalaDpi.Px(this, 80));

        var porcentaje = GrillaUi.AgregarColumna(DgvObrasSociales, "colPorcentaje", "% Cobertura", "PorcentajeCobertura", 100,
            GrillaUi.MinimoPara(DgvObrasSociales, "100%"), "0.##");
        porcentaje.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        var medicos = GrillaUi.AgregarColumna(DgvObrasSociales, "colMedicos", "Médicos que la aceptan", "MedicosQueAceptan", 130,
            EscalaDpi.Px(this, 60));
        medicos.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        var pacientes = GrillaUi.AgregarColumna(DgvObrasSociales, "colPacientes", "Pacientes afiliados", "PacientesAfiliados", 110,
            EscalaDpi.Px(this, 60));
        pacientes.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    private void ConfigurarBarraContadores()
    {
        _contadores.Agregar(ContTotal, EstiloUi.Texto, Color.White);
        _contadores.Agregar(ContConMedicos, PaletaEstados.Fondo(EstadoTurno.Asistio), EstiloUi.Ok);
        _contadores.Agregar(ContConAfiliados, PaletaEstados.Fondo(EstadoTurno.Confirmado), PaletaEstados.Acento(EstadoTurno.Confirmado));
        ActualizarContadores(new List<ObraSocialFila>());
    }

    // ------------------------------------------------------- datos y buscador

    // Trae el listado del servicio y lo muestra con el filtro de busqueda vigente.
    private void CargarGrilla()
    {
        try
        {
            _todas = _service.ObtenerListado();
        }
        catch (Exception ex)
        {
            _todas = new List<ObraSocialFila>();
            MostrarMensaje("No se pudo cargar el listado de obras sociales: " + ex.Message, TipoMensaje.Error);
        }
        AplicarFiltro();
    }

    private void TxtBuscar_TextChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;
        AplicarFiltro();
    }

    // Filtra _todas por nombre (sin distinguir mayusculas ni acentos) y vuelve a
    // armar la grilla. Si la obra social en edicion queda fuera del filtro, la
    // tarjeta vuelve al modo "nueva".
    private void AplicarFiltro()
    {
        string texto = TxtBuscar.Text.Trim();
        var lista = string.IsNullOrEmpty(texto)
            ? _todas.ToList()
            : _todas.Where(f => CultureInfo.CurrentCulture.CompareInfo.IndexOf(
                f.Nombre, texto, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0).ToList();

        bool anterior = _cargando;
        _cargando = true;
        bool reseleccionada = false;
        try
        {
            // Se desengancha SelectionChanged mientras se reasigna el DataSource
            // para que no corra con filas a medio cargar.
            DgvObrasSociales.SelectionChanged -= DgvObrasSociales_SelectionChanged;
            DgvObrasSociales.DataSource = lista;

            // Sin esto, WinForms selecciona sola la primera fila al asignar el
            // DataSource y precarga el formulario con ese registro.
            DgvObrasSociales.ClearSelection();
            DgvObrasSociales.CurrentCell = null;

            // Si se estaba editando una obra social y sigue en la lista, queda elegida.
            if (_idSeleccionado != null)
            {
                foreach (DataGridViewRow fila in DgvObrasSociales.Rows)
                {
                    if (fila.DataBoundItem is ObraSocialFila f && f.Id == _idSeleccionado)
                    {
                        fila.Selected = true;
                        DgvObrasSociales.CurrentCell = fila.Cells[0];
                        reseleccionada = true;
                        break;
                    }
                }
            }
            DgvObrasSociales.SelectionChanged += DgvObrasSociales_SelectionChanged;
        }
        finally
        {
            _cargando = anterior;
        }

        ActualizarContadores(lista);

        LblVista.Text = string.IsNullOrEmpty(texto)
            ? $"Mostrando: todas las obras sociales ({_todas.Count})"
            : $"Mostrando: {lista.Count} de {_todas.Count} · nombre contiene «{texto}»";
        AcotarEtiquetas();

        if (_idSeleccionado != null && !reseleccionada)
            IrAModoNuevo();
        else
            ActualizarAcciones();
    }

    // Cuenta sobre lo que muestra la grilla (con la busqueda ya aplicada).
    private void ActualizarContadores(List<ObraSocialFila> filas)
    {
        _contadores.Poner(ContTotal, $"Total: {filas.Count}");
        _contadores.Poner(ContConMedicos, $"Con médicos: {filas.Count(f => f.MedicosQueAceptan > 0)}");
        _contadores.Poner(ContConAfiliados, $"Con afiliados: {filas.Count(f => f.PacientesAfiliados > 0)}");
    }

    // ------------------------------------------------------- modo de la tarjeta

    // Vuelve al modo "nueva obra social": limpia la tarjeta y deselecciona la fila.
    private void IrAModoNuevo()
    {
        bool anterior = _cargando;
        _cargando = true;
        try
        {
            DgvObrasSociales.SelectionChanged -= DgvObrasSociales_SelectionChanged;
            DgvObrasSociales.ClearSelection();
            DgvObrasSociales.CurrentCell = null;
            DgvObrasSociales.SelectionChanged += DgvObrasSociales_SelectionChanged;

            _idSeleccionado = null;
            TxtNombre.Text = "";
            NudPorcentaje.Value = 0;
        }
        finally
        {
            _cargando = anterior;
        }

        ActualizarModo();
        ActualizarAcciones();
    }

    // Titulo y aviso de la tarjeta segun si hay una obra social cargada para editar.
    private void ActualizarModo()
    {
        bool editando = _idSeleccionado != null;
        lblTituloTarjeta.Text = editando ? "EDITAR OBRA SOCIAL" : "NUEVA OBRA SOCIAL";

        LblModo.Visible = editando;
        LblModo.Text = editando
            ? $"Editando «{FilaSeleccionada?.Nombre}»: modifique los datos y pulse «Guardar»."
            : "";
        AcotarEtiquetas();
    }

    // Texto de la barra de seleccion, detalle de medicos y estado del boton Eliminar.
    private void ActualizarAcciones()
    {
        var fila = FilaSeleccionada;
        if (fila == null)
        {
            LblSeleccion.Text = "Obra social seleccionada: ninguna";
            BtnEliminar.Enabled = false;
            MostrarAyudaAcciones("Elija una fila de la tabla.");
            MostrarDetalle(null);
            return;
        }

        LblSeleccion.Text = $"Obra social seleccionada: {fila.Nombre} · {fila.PorcentajeCobertura:0.##}% de cobertura";
        BtnEliminar.Enabled = true;
        MostrarAyudaAcciones("");
        MostrarDetalle(fila);
    }

    private void MostrarAyudaAcciones(string texto)
    {
        LblAyudaAcciones.Text = texto;
        LblAyudaAcciones.Visible = !string.IsNullOrEmpty(texto);
        AcotarEtiquetas();
    }

    // Muestra los medicos que aceptan la obra social elegida.
    private void MostrarDetalle(ObraSocialFila? fila)
    {
        if (fila == null)
            LblDetalle.Text = "Seleccione una obra social para ver los médicos que la aceptan.";
        else if (fila.Medicos.Count == 0)
            LblDetalle.Text = $"{fila.Nombre}: ningún médico activo la acepta por el momento.";
        else
            LblDetalle.Text = $"Médicos que aceptan {fila.Nombre} ({fila.Medicos.Count}): {string.Join("; ", fila.Medicos)}";
        AcotarEtiquetas();
    }

    // ------------------------------------------------------------ eventos

    private void DgvObrasSociales_SelectionChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;

        var fila = FilaSeleccionada;
        if (fila == null)
        {
            ActualizarAcciones();
            return;
        }

        _cargando = true;
        try
        {
            _idSeleccionado = fila.Id;
            TxtNombre.Text = fila.Nombre;
            NudPorcentaje.Value = Math.Clamp(fila.PorcentajeCobertura, NudPorcentaje.Minimum, NudPorcentaje.Maximum);
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
            var obraSocial = new ObraSocial
            {
                Id = _idSeleccionado ?? 0,
                Nombre = TxtNombre.Text,
                PorcentajeCobertura = NudPorcentaje.Value
            };
            if (_idSeleccionado == null)
            {
                _service.Agregar(obraSocial);
            }
            else
            {
                _service.Modificar(obraSocial);
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje($"Error: {ex.Message}", TipoMensaje.Error);
            return;
        }

        CargarGrilla();
        IrAModoNuevo();
        MostrarMensaje("Obra social guardada correctamente.", TipoMensaje.Ok);
    }

    private void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var fila = FilaSeleccionada;
        if (fila == null)
        {
            MostrarMensaje("Debe seleccionar una obra social para eliminar.", TipoMensaje.Error);
            return;
        }
        var obraSocialSeleccionada = fila.ObraSocial;

        var respuesta = MessageBox.Show(
            $"¿Está seguro que desea eliminar la obra social {obraSocialSeleccionada.Nombre}?",
            "Confirmar eliminación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (respuesta != DialogResult.Yes) return;

        try
        {
            _service.EliminarLogico(obraSocialSeleccionada.Id);
        }
        catch (Exception ex)
        {
            MostrarMensaje($"Error: {ex.Message}", TipoMensaje.Error);
            return;
        }

        CargarGrilla();
        IrAModoNuevo();
        MostrarMensaje("Obra social eliminada correctamente.", TipoMensaje.Ok);
    }

    // ----------------------------------------------------- mensajes y estilo

    private void MostrarMensaje(string texto, TipoMensaje tipo) => _mensajes.Mostrar(texto, tipo);

    private void LimpiarMensaje() => _mensajes.Limpiar();

    // Limita el ancho de los textos que pueden ocupar varias lineas al de su contenedor.
    private void AcotarEtiquetas()
    {
        EstiloUi.Acotar(LblModo, tlpTarjeta);
        EstiloUi.Acotar(LblVista, tlpToolbar);
        EstiloUi.Acotar(LblSeleccion, tlpAcciones);
        EstiloUi.Acotar(LblAyudaAcciones, tlpAcciones);
        EstiloUi.Acotar(LblDetalle, tlpAcciones);
        EstiloUi.AjustarFlujos(flpFiltros, FlpContadores, flpBotonesAccion);
        _mensajes.Ajustar();
    }
}
