using System.Globalization;
using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

// Pantalla de Vacaciones y ausencias (Administrador). Izquierda: tarjeta
// "Nueva ausencia" (pasa a "Editar ausencia" al elegir una fila) con medico,
// fecha, tipo, horario (solo para rango horario) y motivo. Derecha: buscador,
// resumen con la alerta de turnos afectados, contadores, grilla y barra de la
// ausencia seleccionada (Eliminar).
// La estructura estatica vive en el Designer; las columnas, los chips y el
// estilo de botones se arman aca para que el disenador no los borre.
public partial class FormExcepcionesAgenda : Form
{
    private readonly ExcepcionAgendaService _service = new();
    private readonly MedicoService _medicoService = new();

    // Id de la excepcion cargada en la tarjeta (null = modo "nueva").
    private int? _idSeleccionado = null;

    // Listado completo traido del servicio; el buscador filtra sobre esta lista.
    private List<ExcepcionFila> _todas = new();

    // true mientras se cargan o sincronizan controles por codigo: los eventos
    // de seleccion y busqueda se ignoran para no recargar a mitad de camino.
    private bool _cargando = true;

    private FranjaMensaje _mensajes = null!;
    private BarraContadores _contadores = null!;
    private Font? _fuenteNegritaCache;
    private Font _fuenteNegrita => _fuenteNegritaCache ??= new Font(DgvExcepciones.Font, FontStyle.Bold);

    private static readonly Color ColorResumen = Color.FromArgb(70, 90, 120);

    private const string ContTotal = "Total";
    private const string ContVigentes = "Vigentes";
    private const string ContProximas = "Proximas";
    private const string ContPasadas = "Pasadas";
    private const string ContAfectados = "Afectados";

    private ExcepcionFila? FilaSeleccionada =>
        DgvExcepciones.SelectedRows.Count > 0 ? DgvExcepciones.SelectedRows[0].DataBoundItem as ExcepcionFila : null;

    public FormExcepcionesAgenda()
    {
        InitializeComponent();

        _mensajes = new FranjaMensaje(pnlEstado, LblMensaje) { ReacomodarContenedor = true };
        _contadores = new BarraContadores(FlpContadores);

        BtnNuevo.Click += BtnNuevo_Click;
        BtnGuardar.Click += BtnGuardar_Click;
        BtnEliminar.Click += BtnEliminar_Click;
        DgvExcepciones.SelectionChanged += DgvExcepciones_SelectionChanged;
        DgvExcepciones.CellFormatting += DgvExcepciones_CellFormatting;
        DgvExcepciones.CellToolTipTextNeeded += DgvExcepciones_CellToolTipTextNeeded;
        TxtBuscar.TextChanged += TxtBuscar_TextChanged;
        CboTipo.SelectedIndexChanged += CboTipo_SelectedIndexChanged;

        // Los textos y flujos con ancho limitado se recalculan cuando cambia el ancho.
        tlpTarjeta.SizeChanged += (s, e) => AcotarEtiquetas();
        tlpToolbar.SizeChanged += (s, e) => AcotarEtiquetas();
        tlpDerecho.SizeChanged += (s, e) => AcotarEtiquetas();
        tlpAcciones.SizeChanged += (s, e) => AcotarEtiquetas();
        foreach (var flujo in new[] { flpFiltros, FlpContadores, flpBotonesAccion })
            flujo.SizeChanged += (s, e) => EstiloUi.AjustarFlujos(flpFiltros, FlpContadores, flpBotonesAccion);

        Load += FormExcepcionesAgenda_Load;
    }

    // ------------------------------------------------------------- arranque

    private void FormExcepcionesAgenda_Load(object? sender, EventArgs e)
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

    private void ConfigurarColumnas()
    {
        GrillaUi.Estilizar(DgvExcepciones);

        GrillaUi.AgregarColumna(DgvExcepciones, "colMedico", "Médico", "MedicoNombre", 180, EscalaDpi.Px(this, 70));
        GrillaUi.AgregarColumna(DgvExcepciones, "colFecha", "Fecha", "Fecha", 100, GrillaUi.MinimoPara(DgvExcepciones, "00/00/0000"), "dd/MM/yyyy")
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        GrillaUi.AgregarColumna(DgvExcepciones, "colDescripcion", "Detalle", "DescripcionTexto", 140, GrillaUi.MinimoPara(DgvExcepciones, "Día completo"));
        GrillaUi.AgregarColumna(DgvExcepciones, "colMotivo", "Motivo", "Motivo", 200, EscalaDpi.Px(this, 70));
        GrillaUi.AgregarColumna(DgvExcepciones, "colEstado", "Estado", "EstadoTexto", 80, GrillaUi.MinimoPara(DgvExcepciones, "Próxima"))
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        GrillaUi.AgregarColumna(DgvExcepciones, "colAfectados", "Turnos afectados", "TurnosAfectados", 90, GrillaUi.MinimoPara(DgvExcepciones, "00"))
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    private void ConfigurarBarraContadores()
    {
        _contadores.Agregar(ContTotal, EstiloUi.Texto, Color.White);
        _contadores.Agregar(ContVigentes, PaletaEstados.Fondo(EstadoExcepcion.Vigente), PaletaEstados.Acento(EstadoExcepcion.Vigente));
        _contadores.Agregar(ContProximas, PaletaEstados.Fondo(EstadoExcepcion.Proxima), PaletaEstados.Acento(EstadoExcepcion.Proxima));
        _contadores.Agregar(ContPasadas, PaletaEstados.Fondo(EstadoExcepcion.Pasada), PaletaEstados.Acento(EstadoExcepcion.Pasada));
        _contadores.Agregar(ContAfectados, PaletaEstados.Fondo(EstadoTurno.Ausente), PaletaEstados.Acento(EstadoTurno.Ausente));
        ActualizarContadores(new List<ExcepcionFila>());
    }

    private void CargarCombos()
    {
        try
        {
            CboMedico.DataSource = _medicoService.ObtenerTodos();
            CboMedico.DisplayMember = "NombreCompleto";
            CboMedico.ValueMember = "Id";
        }
        catch (Exception ex)
        {
            MostrarMensaje("No se pudo cargar la lista de médicos: " + ex.Message, TipoMensaje.Error);
        }

        // Mismo patron que CboDiaSemana en FormConfigurarAgenda: armamos una
        // lista con Valor/Texto en vez de bindear el enum crudo, para poder
        // mostrar un texto mas legible que TipoExcepcionAgenda.ToString().
        CboTipo.DataSource = new[]
        {
            new { Valor = TipoExcepcionAgenda.DiaCompleto, Texto = "Día completo" },
            new { Valor = TipoExcepcionAgenda.RangoHorario, Texto = "Rango horario" }
        };
        CboTipo.DisplayMember = "Texto";
        CboTipo.ValueMember = "Valor";

        DtpHoraInicio.Value = DateTime.Today.AddHours(8);
        DtpHoraFin.Value = DateTime.Today.AddHours(9);
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
            _todas = new List<ExcepcionFila>();
            MostrarMensaje("No se pudo cargar el listado de excepciones: " + ex.Message, TipoMensaje.Error);
        }
        AplicarFiltro();
    }

    private void TxtBuscar_TextChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;
        AplicarFiltro();
    }

    private static bool Contiene(string texto, string buscado) =>
        CultureInfo.CurrentCulture.CompareInfo.IndexOf(texto, buscado, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;

    // Filtra _todas por medico o motivo (sin distinguir mayusculas ni acentos)
    // y vuelve a armar la grilla. Si la excepcion en edicion queda fuera del
    // filtro, la tarjeta vuelve al modo "nueva".
    private void AplicarFiltro()
    {
        string texto = TxtBuscar.Text.Trim();
        var lista = string.IsNullOrEmpty(texto)
            ? _todas.ToList()
            : _todas.Where(f => Contiene(f.MedicoNombre, texto) || Contiene(f.Motivo, texto)).ToList();

        bool anterior = _cargando;
        _cargando = true;
        bool reseleccionada = false;
        try
        {
            // Se desengancha SelectionChanged mientras se reasigna el DataSource
            // para que no corra con filas a medio cargar.
            DgvExcepciones.SelectionChanged -= DgvExcepciones_SelectionChanged;
            DgvExcepciones.DataSource = lista;

            // Sin esto, WinForms selecciona sola la primera fila al asignar el
            // DataSource y precarga el formulario con ese registro.
            DgvExcepciones.ClearSelection();
            DgvExcepciones.CurrentCell = null;

            // Si se estaba editando una excepcion y sigue en la lista, queda elegida.
            if (_idSeleccionado != null)
            {
                foreach (DataGridViewRow fila in DgvExcepciones.Rows)
                {
                    if (fila.DataBoundItem is ExcepcionFila f && f.Id == _idSeleccionado)
                    {
                        fila.Selected = true;
                        DgvExcepciones.CurrentCell = fila.Cells[0];
                        reseleccionada = true;
                        break;
                    }
                }
            }
            DgvExcepciones.SelectionChanged += DgvExcepciones_SelectionChanged;
        }
        finally
        {
            _cargando = anterior;
        }

        ActualizarContadores(lista);
        ActualizarResumen(lista);

        LblVista.Text = string.IsNullOrEmpty(texto)
            ? $"Mostrando: todas las ausencias ({_todas.Count})"
            : $"Mostrando: {lista.Count} de {_todas.Count} · médico o motivo contiene «{texto}»";
        AcotarEtiquetas();

        if (_idSeleccionado != null && !reseleccionada)
            IrAModoNuevo();
        else
            ActualizarAcciones();
    }

    // Cuenta sobre lo que muestra la grilla (con la busqueda ya aplicada).
    private void ActualizarContadores(List<ExcepcionFila> filas)
    {
        _contadores.Poner(ContTotal, $"Total: {filas.Count}");
        _contadores.Poner(ContVigentes, $"Vigentes: {filas.Count(f => f.Estado == EstadoExcepcion.Vigente)}");
        _contadores.Poner(ContProximas, $"Próximas: {filas.Count(f => f.Estado == EstadoExcepcion.Proxima)}");
        _contadores.Poner(ContPasadas, $"Pasadas: {filas.Count(f => f.Estado == EstadoExcepcion.Pasada)}");
        _contadores.Poner(ContAfectados, $"Con turnos afectados: {filas.Count(f => f.Estado != EstadoExcepcion.Pasada && f.TurnosAfectados > 0)}");
    }

    private void ActualizarResumen(List<ExcepcionFila> filas)
    {
        if (filas.Count == 0)
        {
            LblResumenExcepciones.ForeColor = ColorResumen;
            LblResumenExcepciones.Text = _todas.Count == 0
                ? "No hay excepciones de agenda cargadas."
                : "Ninguna excepción coincide con la búsqueda.";
            AcotarEtiquetas();
            return;
        }

        var vigentes = filas.Count(f => f.Estado == EstadoExcepcion.Vigente);
        var proximas = filas.Count(f => f.Estado == EstadoExcepcion.Proxima);
        var pasadas = filas.Count - vigentes - proximas;
        var conAfectados = filas.Where(f => f.Estado != EstadoExcepcion.Pasada && f.TurnosAfectados > 0).ToList();

        var texto = $"{filas.Count} excepción(es): {vigentes} vigente(s), {proximas} próxima(s), {pasadas} pasada(s).";
        if (conAfectados.Count > 0)
        {
            texto += $"  Atención: {conAfectados.Sum(f => f.TurnosAfectados)} turno(s) activo(s) se superponen con {conAfectados.Count} excepción(es) vigente(s) o próxima(s).";
            LblResumenExcepciones.ForeColor = PaletaEstados.Acento(EstadoTurno.Ausente);
        }
        else
        {
            LblResumenExcepciones.ForeColor = ColorResumen;
        }
        LblResumenExcepciones.Text = texto;
        AcotarEtiquetas();
    }

    private void DgvExcepciones_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.CellStyle == null || e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (DgvExcepciones.Rows[e.RowIndex].DataBoundItem is not ExcepcionFila fila) return;

        var columna = DgvExcepciones.Columns[e.ColumnIndex].Name;
        if (columna == "colEstado")
        {
            e.CellStyle.BackColor = PaletaEstados.Fondo(fila.Estado);
            e.CellStyle.ForeColor = PaletaEstados.Acento(fila.Estado);
            e.CellStyle.Font = _fuenteNegrita;
        }
        else if (columna == "colAfectados" && fila.Estado != EstadoExcepcion.Pasada && fila.TurnosAfectados > 0)
        {
            // Alerta: una ausencia vigente o proxima que todavia tiene turnos activos encima.
            e.CellStyle.BackColor = PaletaEstados.Fondo(EstadoTurno.Ausente);
            e.CellStyle.ForeColor = PaletaEstados.Acento(EstadoTurno.Ausente);
            e.CellStyle.Font = _fuenteNegrita;
        }
        else if (columna == "colDescripcion" && fila.Tipo == TipoExcepcionAgenda.DiaCompleto)
        {
            // El texto de la entidad no lleva acento; en pantalla si.
            e.Value = "Día completo";
            e.FormattingApplied = true;
        }
    }

    private void DgvExcepciones_CellToolTipTextNeeded(object? sender, DataGridViewCellToolTipTextNeededEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (DgvExcepciones.Columns[e.ColumnIndex].Name != "colAfectados") return;
        if (DgvExcepciones.Rows[e.RowIndex].DataBoundItem is not ExcepcionFila fila) return;

        e.ToolTipText = TextoAfectados(fila);
    }

    private static string TextoAfectados(ExcepcionFila fila) => fila.TurnosAfectados == 0
        ? "No hay turnos activos del médico que se superpongan con esta excepción."
        : $"{fila.TurnosAfectados} turno(s) activo(s) no cancelado(s) del médico se superponen con esta excepción.";

    // ------------------------------------------------------- modo de la tarjeta

    // El horario (desde / hasta) solo tiene sentido para un rango horario.
    private void ActualizarVisibilidadPorTipo()
    {
        if (CboTipo.SelectedValue is TipoExcepcionAgenda tipo)
        {
            bool esRango = tipo == TipoExcepcionAgenda.RangoHorario;
            lblPasoHoras.Visible = esRango;
            DtpHoraInicio.Visible = esRango;
            lblHoraInicio.Visible = esRango;
            DtpHoraFin.Visible = esRango;
            lblHoraFin.Visible = esRango;
            lblMotivo.Text = esRango ? "5. Motivo" : "4. Motivo";
        }
    }

    // Vuelve al modo "nueva ausencia": limpia la tarjeta y deselecciona la fila.
    private void IrAModoNuevo()
    {
        bool anterior = _cargando;
        _cargando = true;
        try
        {
            DgvExcepciones.SelectionChanged -= DgvExcepciones_SelectionChanged;
            DgvExcepciones.ClearSelection();
            DgvExcepciones.CurrentCell = null;
            DgvExcepciones.SelectionChanged += DgvExcepciones_SelectionChanged;

            _idSeleccionado = null;
            CboMedico.SelectedIndex = -1;
            DtpFecha.Value = DateTime.Today;
            CboTipo.SelectedIndex = 0;
            TxtMotivo.Text = "";
            DtpHoraInicio.Value = DateTime.Today.AddHours(8);
            DtpHoraFin.Value = DateTime.Today.AddHours(9);
            ActualizarVisibilidadPorTipo();
        }
        finally
        {
            _cargando = anterior;
        }

        ActualizarModo();
        ActualizarAcciones();
    }

    // Titulo y aviso de la tarjeta segun si hay una ausencia cargada para editar.
    private void ActualizarModo()
    {
        bool editando = _idSeleccionado != null;
        lblTituloTarjeta.Text = editando ? "EDITAR AUSENCIA" : "NUEVA AUSENCIA";

        LblModo.Visible = editando;
        LblModo.Text = editando
            ? $"Editando la ausencia de «{FilaSeleccionada?.MedicoNombre}»: modifique los datos y pulse «Guardar»."
            : "";
        AcotarEtiquetas();
    }

    // Texto de la barra de seleccion, detalle y estado del boton Eliminar.
    private void ActualizarAcciones()
    {
        var fila = FilaSeleccionada;
        if (fila == null)
        {
            LblSeleccion.Text = "Ausencia seleccionada: ninguna";
            BtnEliminar.Enabled = false;
            MostrarAyudaAcciones("Elija una fila de la tabla.");
            LblDetalle.Text = "Seleccione una ausencia para ver su motivo y los turnos afectados.";
            AcotarEtiquetas();
            return;
        }

        var detalle = fila.Tipo == TipoExcepcionAgenda.DiaCompleto ? "Día completo" : fila.DescripcionTexto;
        LblSeleccion.Text = $"Ausencia seleccionada: {fila.MedicoNombre} · {fila.Fecha:dd/MM/yyyy} · {detalle}";
        BtnEliminar.Enabled = true;
        MostrarAyudaAcciones("");
        LblDetalle.Text = (string.IsNullOrWhiteSpace(fila.Motivo) ? "" : $"Motivo: {fila.Motivo}. ") + TextoAfectados(fila);
        AcotarEtiquetas();
    }

    private void MostrarAyudaAcciones(string texto)
    {
        LblAyudaAcciones.Text = texto;
        LblAyudaAcciones.Visible = !string.IsNullOrEmpty(texto);
        AcotarEtiquetas();
    }

    // ------------------------------------------------------------ eventos

    private void DgvExcepciones_SelectionChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;

        var fila = FilaSeleccionada;
        if (fila == null)
        {
            ActualizarAcciones();
            return;
        }

        var excepcion = fila.Excepcion;
        _cargando = true;
        try
        {
            _idSeleccionado = excepcion.Id;

            CboMedico.SelectedValue = excepcion.MedicoId;
            DtpFecha.Value = excepcion.Fecha.ToDateTime(TimeOnly.MinValue);
            CboTipo.SelectedValue = excepcion.Tipo;
            TxtMotivo.Text = excepcion.Motivo;

            if (excepcion.Tipo == TipoExcepcionAgenda.RangoHorario && excepcion.HoraInicio != null && excepcion.HoraFin != null)
            {
                DtpHoraInicio.Value = DateTime.Today.Add(excepcion.HoraInicio.Value.ToTimeSpan());
                DtpHoraFin.Value = DateTime.Today.Add(excepcion.HoraFin.Value.ToTimeSpan());
            }

            ActualizarVisibilidadPorTipo();
        }
        finally
        {
            _cargando = false;
        }

        LimpiarMensaje();
        ActualizarModo();
        ActualizarAcciones();
    }

    private void CboTipo_SelectedIndexChanged(object? sender, EventArgs e)
    {
        ActualizarVisibilidadPorTipo();
    }

    private void BtnNuevo_Click(object? sender, EventArgs e)
    {
        LimpiarMensaje();
        IrAModoNuevo();
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        var turnosCancelados = new List<Turno>();
        try
        {
            if (CboMedico.SelectedValue == null) throw new ArgumentException("Debe seleccionar un médico.");
            if (CboTipo.SelectedValue == null) throw new ArgumentException("Debe seleccionar un tipo de excepción.");

            var tipo = (TipoExcepcionAgenda)CboTipo.SelectedValue;

            var excepcion = new ExcepcionAgenda
            {
                Id = _idSeleccionado ?? 0,
                MedicoId = (int)CboMedico.SelectedValue,
                Fecha = DateOnly.FromDateTime(DtpFecha.Value),
                Tipo = tipo,
                HoraInicio = tipo == TipoExcepcionAgenda.RangoHorario ? TimeOnly.FromDateTime(DtpHoraInicio.Value) : null,
                HoraFin = tipo == TipoExcepcionAgenda.RangoHorario ? TimeOnly.FromDateTime(DtpHoraFin.Value) : null,
                Motivo = TxtMotivo.Text
            };

            if (_idSeleccionado == null)
            {
                turnosCancelados = _service.Agregar(excepcion);
            }
            else
            {
                _service.Modificar(excepcion);
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message, TipoMensaje.Error);
            return;
        }

        CargarGrilla();
        IrAModoNuevo();

        MostrarMensaje("Excepción guardada correctamente." + (turnosCancelados.Count > 0
            ? $" Se cancelaron {turnosCancelados.Count} turno(s) que coincidían con esta ausencia."
            : ""), TipoMensaje.Ok);
    }

    private void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var fila = FilaSeleccionada;
        if (fila == null)
        {
            MostrarMensaje("Seleccione una excepción de la lista primero.", TipoMensaje.Error);
            return;
        }
        var excepcion = fila.Excepcion;

        var respuesta = MessageBox.Show(
            $"¿Está seguro que desea eliminar la excepción del médico {excepcion.MedicoNombre} para el {excepcion.Fecha:dd/MM/yyyy}?",
            "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (respuesta != DialogResult.Yes) return;

        try
        {
            _service.EliminarLogico(excepcion.Id);
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message, TipoMensaje.Error);
            return;
        }

        CargarGrilla();
        IrAModoNuevo();
        MostrarMensaje("Excepción eliminada correctamente.", TipoMensaje.Ok);
    }

    // ----------------------------------------------------- mensajes y estilo

    private void MostrarMensaje(string texto, TipoMensaje tipo) => _mensajes.Mostrar(texto, tipo);

    private void LimpiarMensaje() => _mensajes.Limpiar();

    // Limita el ancho de los textos que pueden ocupar varias lineas al de su contenedor.
    private void AcotarEtiquetas()
    {
        EstiloUi.Acotar(LblModo, tlpTarjeta);
        EstiloUi.Acotar(LblVista, tlpToolbar);
        EstiloUi.Acotar(LblResumenExcepciones, tlpDerecho);
        EstiloUi.Acotar(LblSeleccion, tlpAcciones);
        EstiloUi.Acotar(LblAyudaAcciones, tlpAcciones);
        EstiloUi.Acotar(LblDetalle, tlpAcciones);
        EstiloUi.AjustarFlujos(flpFiltros, FlpContadores, flpBotonesAccion);
        _mensajes.Ajustar();
    }
}
