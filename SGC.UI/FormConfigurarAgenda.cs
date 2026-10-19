using System.Globalization;
using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

// Pantalla de Horarios (Administrador). Izquierda: tarjeta "Nuevo horario"
// (pasa a "Editar horario" al elegir una franja del Listado) con medico, dia de
// la semana y horario desde/hasta. Derecha: filtro por medico, contadores, las
// solapas "Vista semanal" (protagonista) y "Listado", y la barra de la franja
// seleccionada (Eliminar).
// La estructura estatica (incluidas las solapas) vive en el Designer; las
// columnas, los chips y el estilo de botones se arman aca.
public partial class FormConfigurarAgenda : Form
{
    private readonly AgendaMedicoService _service = new();
    private readonly MedicoService _medicoService = new();

    // Id de la franja cargada en la tarjeta (null = modo "nuevo").
    private int? _idSeleccionado = null;

    // Datos completos: las franjas del Listado y el resumen de la vista semanal.
    // El filtro por medico trabaja sobre estas listas.
    private List<AgendaMedico> _todas = new();
    private List<AgendaSemanalFila> _resumenSemanal = new();
    private Dictionary<int, int> _minutosPorMedico = new();

    // true mientras se cargan o sincronizan controles por codigo: los eventos
    // de seleccion y filtro se ignoran para no recargar a mitad de camino.
    private bool _cargando = true;

    private FranjaMensaje _mensajes = null!;
    private BarraContadores _contadores = null!;
    private readonly ToolTip _tooltip = new();
    private Font? _fuenteNegritaCache;
    private Font _fuenteNegrita => _fuenteNegritaCache ??= new Font(DgvSemanal.Font, FontStyle.Bold);

    private const string TextoSinHorario = "—";
    private const string ContMedicos = "MedicosConAgenda";
    private const string ContHoras = "HorasTotales";
    private const string ContFranjas = "Franjas";

    // Opcion del combo de filtro (Id 0 = todos los medicos).
    private sealed record ItemFiltro(int Id, string Texto);

    private AgendaMedico? FilaSeleccionada =>
        DgvAgenda.SelectedRows.Count > 0 ? DgvAgenda.SelectedRows[0].DataBoundItem as AgendaMedico : null;

    public FormConfigurarAgenda()
    {
        InitializeComponent();
        EstiloUi.AlinearBotonesConScroll(pnlCampos, pnlBotones);

        _mensajes = new FranjaMensaje(pnlEstado, LblMensaje) { ReacomodarContenedor = true };
        _contadores = new BarraContadores(FlpContadores);

        BtnNuevo.Click += BtnNuevo_Click;
        BtnGuardar.Click += BtnGuardar_Click;
        BtnEliminar.Click += BtnEliminar_Click;
        DgvAgenda.SelectionChanged += DgvAgenda_SelectionChanged;
        DgvAgenda.CellFormatting += DgvAgenda_CellFormatting;
        DgvSemanal.CellFormatting += DgvSemanal_CellFormatting;
        CboFiltroMedico.SelectedIndexChanged += CboFiltroMedico_SelectedIndexChanged;

        // Los textos y flujos con ancho limitado se recalculan cuando cambia el ancho.
        tlpAcciones.SizeChanged += (s, e) => AcotarEtiquetas();
        tlpDerecho.SizeChanged += (s, e) => AcotarEtiquetas();
        foreach (var flujo in new[] { flpFiltros, FlpContadores, flpBotonesAccion })
            flujo.SizeChanged += (s, e) => EstiloUi.AjustarFlujos(flpFiltros, FlpContadores, flpBotonesAccion);

        Load += FormConfigurarAgenda_Load;
    }

    // ------------------------------------------------------------- arranque

    private void FormConfigurarAgenda_Load(object? sender, EventArgs e)
    {
        _cargando = true;
        try
        {
            ConfigurarColumnasListado();
            ConfigurarColumnasSemanal();
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

    // Listado: una fila por franja. Las columnas se arman aca (no en el
    // Designer) porque el disenador visual las borra al abrir el formulario.
    private void ConfigurarColumnasListado()
    {
        GrillaUi.Estilizar(DgvAgenda, 46, 28);

        GrillaUi.AgregarColumna(DgvAgenda, "colMedico", "Médico", "MedicoNombre", 160, EscalaDpi.Px(this, 90));
        GrillaUi.AgregarColumna(DgvAgenda, "colDia", "Día", "DiaNombre", 80, GrillaUi.MinimoPara(DgvAgenda, "Miércoles"));
        GrillaUi.AgregarColumna(DgvAgenda, "colRango", "Horario", "HorarioRango", 90, GrillaUi.MinimoPara(DgvAgenda, "00:00 - 00:00"))
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        // Columna sin enlace a datos: el valor lo completa CellFormatting con
        // las horas semanales del medico de la fila (suma de todas sus franjas).
        GrillaUi.AgregarColumna(DgvAgenda, "colHorasMedico", "Horas semanales por médico", null, 110, GrillaUi.MinimoPara(DgvAgenda, "00 h 00 min"))
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    // Vista semanal: una fila por medico con sus franjas de lunes a domingo.
    private void ConfigurarColumnasSemanal()
    {
        GrillaUi.Estilizar(DgvSemanal, 40, 30);
        // Una fila por medico; crece solo si un dia tiene mas de una franja.
        DgvSemanal.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        DgvSemanal.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        DgvSemanal.DefaultCellStyle.Padding = new Padding(EscalaDpi.Px(this, 2), EscalaDpi.Px(this, 2), EscalaDpi.Px(this, 2), EscalaDpi.Px(this, 2));

        // Ancho del texto sin el relleno extra de MeasureText: asi los cinco dias habituales entran sin scroll horizontal.
        // Los horarios van en letra chica (8,5) por la misma razon.
        DgvSemanal.DefaultCellStyle.Font = EstiloUi.FuenteChica;
        int Ancho(string muestra) => TextRenderer.MeasureText(muestra, EstiloUi.FuenteChica, Size.Empty, TextFormatFlags.NoPadding).Width + EscalaDpi.Px(this, 10);

        GrillaUi.AgregarColumna(DgvSemanal, "colSemMedico", "Médico", null, 130, EscalaDpi.Px(this, 70));
        foreach (var dia in AgendaSemanalFila.DiasEnOrden)
        {
            GrillaUi.AgregarColumna(DgvSemanal, "colSem" + dia, AgendaSemanalFila.NombreDia(dia), null, 75, Ancho("00:00–00:00"))
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        GrillaUi.AgregarColumna(DgvSemanal, "colSemHoras", "Horas semanales", null, 100, Ancho("00 h"))
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    private void ConfigurarBarraContadores()
    {
        _contadores.Agregar(ContMedicos, EstiloUi.Texto, Color.White);
        _contadores.Agregar(ContHoras, PaletaEstados.Fondo(EstadoTurno.Confirmado), PaletaEstados.Acento(EstadoTurno.Confirmado));
        _contadores.Agregar(ContFranjas, PaletaEstados.Fondo(EstadoTurno.Asistio), PaletaEstados.Acento(EstadoTurno.Asistio));
        ActualizarContadores(new List<AgendaSemanalFila>(), 0);

        _tooltip.SetToolTip(FlpContadores.Controls.Find("lblContador" + ContMedicos, false)[0], "Médicos que tienen al menos una franja cargada");
        _tooltip.SetToolTip(FlpContadores.Controls.Find("lblContador" + ContHoras, false)[0], "Horas semanales totales: suma de todas las franjas");
        _tooltip.SetToolTip(FlpContadores.Controls.Find("lblContador" + ContFranjas, false)[0], "Franjas horarias cargadas");
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

        // Lunes a domingo, con el nombre del dia en es-AR.
        CboDiaSemana.DataSource = AgendaSemanalFila.DiasEnOrden
            .Select(dia => new { Valor = dia, Texto = AgendaSemanalFila.NombreDia(dia) })
            .ToList();
        CboDiaSemana.DisplayMember = "Texto";
        CboDiaSemana.ValueMember = "Valor";

        DtpHoraInicio.Value = DateTime.Today.AddHours(8);
        DtpHoraFin.Value = DateTime.Today.AddHours(9);
    }

    // ------------------------------------------------------- datos y filtro

    // Trae franjas y resumen semanal de la base y los muestra con el filtro vigente.
    // Se llama al abrir y despues de Guardar y Eliminar (asi la vista semanal
    // siempre queda al dia).
    private void CargarGrilla()
    {
        try
        {
            _todas = _service.ObtenerTodos()
                .OrderBy(a => a.MedicoNombre, StringComparer.CurrentCultureIgnoreCase)
                .ThenBy(a => Array.IndexOf(AgendaSemanalFila.DiasEnOrden, a.DiaSemana))
                .ThenBy(a => a.HoraInicio)
                .ToList();
        }
        catch (Exception ex)
        {
            _todas = new List<AgendaMedico>();
            MostrarMensaje("No se pudo cargar el listado de horarios: " + ex.Message, TipoMensaje.Error);
        }

        try
        {
            _resumenSemanal = _service.ObtenerResumenSemanal();
        }
        catch (Exception ex)
        {
            _resumenSemanal = new List<AgendaSemanalFila>();
            MostrarMensaje("No se pudo cargar la vista semanal de horarios: " + ex.Message, TipoMensaje.Error);
        }
        _minutosPorMedico = _resumenSemanal.ToDictionary(f => f.MedicoId, f => f.MinutosSemanales);

        CargarFiltroMedicos();
        AplicarFiltro();
    }

    // "Todos los medicos" + los medicos activos; conserva el elegido entre recargas.
    private void CargarFiltroMedicos()
    {
        int idElegido = (CboFiltroMedico.SelectedItem as ItemFiltro)?.Id ?? 0;
        var items = new List<ItemFiltro> { new(0, "Todos los médicos") };
        items.AddRange(_resumenSemanal.Select(f => new ItemFiltro(f.MedicoId, f.MedicoNombre)));

        bool anterior = _cargando;
        _cargando = true;
        try
        {
            CboFiltroMedico.DataSource = items;
            CboFiltroMedico.DisplayMember = "Texto";
            CboFiltroMedico.ValueMember = "Id";
            CboFiltroMedico.SelectedIndex = Math.Max(0, items.FindIndex(i => i.Id == idElegido));
        }
        finally
        {
            _cargando = anterior;
        }
    }

    private void CboFiltroMedico_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;
        AplicarFiltro();
    }

    // Filtra ambas vistas por el medico elegido y las vuelve a armar. Si la
    // franja en edicion queda fuera del filtro, la tarjeta vuelve al modo "nuevo".
    private void AplicarFiltro()
    {
        int idMedico = (CboFiltroMedico.SelectedItem as ItemFiltro)?.Id ?? 0;
        var resumen = _resumenSemanal.Where(f => idMedico == 0 || f.MedicoId == idMedico).ToList();
        var franjas = _todas.Where(a => idMedico == 0 || a.MedicoId == idMedico).ToList();

        PintarVistaSemanal(resumen);

        bool anterior = _cargando;
        _cargando = true;
        bool reseleccionada = false;
        try
        {
            // Se desengancha SelectionChanged mientras se reasigna el DataSource
            // para que no corra con filas a medio cargar.
            DgvAgenda.SelectionChanged -= DgvAgenda_SelectionChanged;
            DgvAgenda.DataSource = franjas;

            // Sin esto, WinForms selecciona sola la primera fila al asignar el
            // DataSource y precarga el formulario con ese registro.
            DgvAgenda.ClearSelection();
            DgvAgenda.CurrentCell = null;

            // Si se estaba editando una franja y sigue en la lista, queda elegida.
            if (_idSeleccionado != null)
            {
                foreach (DataGridViewRow fila in DgvAgenda.Rows)
                {
                    if (fila.DataBoundItem is AgendaMedico a && a.Id == _idSeleccionado)
                    {
                        fila.Selected = true;
                        DgvAgenda.CurrentCell = fila.Cells[0];
                        reseleccionada = true;
                        break;
                    }
                }
            }
            DgvAgenda.SelectionChanged += DgvAgenda_SelectionChanged;
        }
        finally
        {
            _cargando = anterior;
        }

        ActualizarContadores(resumen, franjas.Count);
        DgvAgenda.Invalidate();
        AcotarEtiquetas();

        if (_idSeleccionado != null && !reseleccionada)
            IrAModoNuevo();
        else
            ActualizarAcciones();
    }

    // Cuenta sobre lo que muestran las grillas (con el filtro ya aplicado).
    private void ActualizarContadores(List<AgendaSemanalFila> resumen, int franjas)
    {
        int conAgenda = resumen.Count(f => f.MinutosSemanales > 0);
        _contadores.Poner(ContMedicos, $"Médicos con agenda: {conAgenda} de {resumen.Count}");
        _contadores.Poner(ContHoras, $"Horas semanales: {AgendaSemanalFila.FormatearMinutos(resumen.Sum(f => f.MinutosSemanales))}");
        _contadores.Poner(ContFranjas, $"Franjas cargadas: {franjas}");
    }

    private void PintarVistaSemanal(List<AgendaSemanalFila> lista)
    {
        DgvSemanal.Rows.Clear();
        foreach (var fila in lista)
        {
            var valores = new object[AgendaSemanalFila.DiasEnOrden.Length + 2];
            // Solo apellido y nombre: la especialidad no entra en la columna (esta en el filtro y el Listado).
            valores[0] = $"{fila.Medico.Apellido}, {fila.Medico.Nombre}";
            for (var i = 0; i < AgendaSemanalFila.DiasEnOrden.Length; i++)
            {
                // El rango se muestra sin espacios alrededor del guion (para que los dias entren en pantallas angostas).
                var texto = fila.TextoDia(AgendaSemanalFila.DiasEnOrden[i]).Replace(" - ", "–");
                valores[i + 1] = texto.Length == 0 ? TextoSinHorario : texto;
            }
            valores[^1] = fila.HorasSemanalesTexto;
            var indice = DgvSemanal.Rows.Add(valores);
            DgvSemanal.Rows[indice].Tag = fila;
        }
        DgvSemanal.ClearSelection();
        DgvSemanal.CurrentCell = null;

        // Sabado y domingo solo se muestran si algun medico atiende esos dias:
        // sin datos son dos columnas vacias que obligan a scrollear en pantallas angostas.
        foreach (var dia in new[] { DayOfWeek.Saturday, DayOfWeek.Sunday })
            DgvSemanal.Columns["colSem" + dia].Visible = lista.Any(f => f.TextoDia(dia).Length > 0);
    }

    private void DgvAgenda_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (DgvAgenda.Rows[e.RowIndex].DataBoundItem is not AgendaMedico agenda) return;

        var columna = DgvAgenda.Columns[e.ColumnIndex].Name;
        if (columna == "colHorasMedico")
        {
            e.Value = _minutosPorMedico.TryGetValue(agenda.MedicoId, out var minutos)
                ? AgendaSemanalFila.FormatearMinutos(minutos)
                : "-";
            e.FormattingApplied = true;
        }
        else if (columna == "colDia")
        {
            // Nombre del dia en es-AR (con tilde), igual que la vista semanal.
            e.Value = AgendaSemanalFila.NombreDia(agenda.DiaSemana);
            e.FormattingApplied = true;
        }
    }

    private void DgvSemanal_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.CellStyle == null || e.RowIndex < 0 || e.ColumnIndex < 0) return;

        var columna = DgvSemanal.Columns[e.ColumnIndex].Name;
        if (columna == "colSemHoras")
        {
            e.CellStyle.Font = _fuenteNegrita;
        }
        else if (e.ColumnIndex >= 1)
        {
            // Dia con franjas: bloque azul suave (mismo tono que "Confirmado");
            // dia sin atencion: guion gris.
            if (e.Value is string texto && texto != TextoSinHorario && texto.Length > 0)
            {
                e.CellStyle.BackColor = PaletaEstados.Fondo(EstadoTurno.Confirmado);
                e.CellStyle.ForeColor = EstiloUi.Texto;
            }
            else
            {
                e.CellStyle.ForeColor = Color.FromArgb(150, 158, 170);
            }
        }

        // La grilla es solo de lectura: la seleccion no se destaca para que la
        // fila que WinForms marca sola al mostrarse no parezca una eleccion del usuario.
        e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
        e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
    }

    // ------------------------------------------------------- modo de la tarjeta

    // Vuelve al modo "nuevo horario": limpia la tarjeta y deselecciona la fila.
    private void IrAModoNuevo()
    {
        bool anterior = _cargando;
        _cargando = true;
        try
        {
            DgvAgenda.SelectionChanged -= DgvAgenda_SelectionChanged;
            DgvAgenda.ClearSelection();
            DgvAgenda.CurrentCell = null;
            DgvAgenda.SelectionChanged += DgvAgenda_SelectionChanged;

            _idSeleccionado = null;
            CboMedico.SelectedIndex = -1;
            CboDiaSemana.SelectedIndex = -1;
            DtpHoraInicio.Value = DateTime.Today.AddHours(8);
            DtpHoraFin.Value = DateTime.Today.AddHours(9);
        }
        finally
        {
            _cargando = anterior;
        }

        ActualizarModo();
        ActualizarAcciones();
    }

    // Titulo y aviso de la tarjeta segun si hay una franja cargada para editar.
    private void ActualizarModo()
    {
        bool editando = _idSeleccionado != null;
        lblTituloTarjeta.Text = editando ? "EDITAR HORARIO" : "NUEVO HORARIO";

        var fila = FilaSeleccionada;
        string quien = fila == null ? "" : $"{fila.Medico?.Apellido} · {AgendaSemanalFila.NombreDia(fila.DiaSemana)}";
        EstiloUi.MostrarAviso(LblModo, _tooltip,
            editando ? $"Editando: {quien}" : "",
            editando ? $"Editando el horario de «{quien}»: modifique los datos y pulse «Guardar»." : "");
        AcotarEtiquetas();
    }

    // Texto de la barra de seleccion, detalle y estado del boton Eliminar.
    private void ActualizarAcciones()
    {
        var fila = FilaSeleccionada;
        if (fila == null)
        {
            EstiloUi.PonerTexto(LblSeleccion, _tooltip, "Franja seleccionada: ninguna");
            BtnEliminar.Enabled = false;
            MostrarAyudaAcciones("");
            LblDetalle.Text = "Para editar o eliminar una franja, elíjala en la solapa «Listado».";
            AcotarEtiquetas();
            return;
        }

        EstiloUi.PonerTexto(LblSeleccion, _tooltip, $"Franja seleccionada: {fila.MedicoNombre.Split('(')[0].Trim()} · {AgendaSemanalFila.NombreDia(fila.DiaSemana)} · {fila.HorarioRango}");
        BtnEliminar.Enabled = true;
        MostrarAyudaAcciones("");
        LblDetalle.Text = _minutosPorMedico.TryGetValue(fila.MedicoId, out var minutos)
            ? $"Horas semanales del médico: {AgendaSemanalFila.FormatearMinutos(minutos)}"
            : "";
        AcotarEtiquetas();
    }

    private void MostrarAyudaAcciones(string texto)
    {
        LblAyudaAcciones.Text = texto;
        LblAyudaAcciones.Visible = !string.IsNullOrEmpty(texto);
        AcotarEtiquetas();
    }

    // ------------------------------------------------------------ eventos

    private void DgvAgenda_SelectionChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;

        var agenda = FilaSeleccionada;
        if (agenda == null)
        {
            ActualizarAcciones();
            return;
        }

        _cargando = true;
        try
        {
            _idSeleccionado = agenda.Id;

            // AgendaMedico usa HoraInicio y HoraFin propios por dia y medico.
            CboMedico.SelectedValue = agenda.MedicoId;
            CboDiaSemana.SelectedValue = agenda.DiaSemana;
            DtpHoraInicio.Value = DateTime.Today.Add(agenda.HoraInicio.ToTimeSpan());
            DtpHoraFin.Value = DateTime.Today.Add(agenda.HoraFin.ToTimeSpan());
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
            if (CboMedico.SelectedValue == null) throw new ArgumentException("Debe seleccionar un médico.");
            if (CboDiaSemana.SelectedValue == null) throw new ArgumentException("Debe seleccionar un día de la semana.");

            var agenda = new AgendaMedico
            {
                Id = _idSeleccionado ?? 0,
                MedicoId = (int)CboMedico.SelectedValue,
                DiaSemana = (DayOfWeek)CboDiaSemana.SelectedValue,
                HoraInicio = TimeOnly.FromDateTime(DtpHoraInicio.Value),
                HoraFin = TimeOnly.FromDateTime(DtpHoraFin.Value)
            };

            if (_idSeleccionado == null)
            {
                _service.Agregar(agenda);
            }
            else
            {
                _service.Modificar(agenda);
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message, TipoMensaje.Error);
            return;
        }

        CargarGrilla();
        IrAModoNuevo();
        MostrarMensaje("Registro de agenda guardado correctamente", TipoMensaje.Ok);
    }

    private void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var agenda = FilaSeleccionada;
        if (agenda == null)
        {
            MostrarMensaje("Seleccione un registro de agenda de la lista primero.", TipoMensaje.Error);
            return;
        }

        var respuesta = MessageBox.Show(
            $"¿Está seguro que desea eliminar el registro de agenda del médico {agenda.MedicoNombre} para el día {AgendaSemanalFila.NombreDia(agenda.DiaSemana)} en el horario {agenda.HorarioRango}?",
            "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (respuesta != DialogResult.Yes) return;

        try
        {
            _service.EliminarLogico(agenda.Id);
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message, TipoMensaje.Error);
            return;
        }

        CargarGrilla();
        IrAModoNuevo();
        MostrarMensaje("Registro de agenda eliminado correctamente", TipoMensaje.Ok);
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
