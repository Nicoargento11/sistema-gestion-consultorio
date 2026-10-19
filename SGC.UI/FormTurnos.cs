using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

// Pantalla de Turnos (Recepcionista). Izquierda: tarjeta "Nuevo turno" con el
// flujo medico -> fecha -> horarios libres (botones) -> paciente/duracion.
// Derecha: filtros, contadores por estado, grilla de turnos y acciones sobre el
// turno seleccionado. La estructura estatica vive en el Designer; todo lo que
// depende de datos o de pixeles (columnas, botones de horario, leyenda,
// contadores) se arma aca, en Load, para que el disenador de Visual Studio no
// lo borre y para medir con la escala de pantalla ya aplicada.
public partial class FormTurnos : Form
{
    private readonly PacienteService _pacienteService = new();
    private readonly MedicoService _medicoService = new();
    private readonly HorarioService _horarioService = new();
    private readonly AgendaMedicoService _agendaMedicoService = new();
    private readonly TurnoService _turnoService = new();
    private readonly NotificacionService _notificacionService = new();
    private readonly Usuario? _usuarioActivo;

    // Turno de la grilla cargado en la tarjeta de la izquierda (modo reprogramar).
    private int? _idTurnoSeleccionado = null;

    // Horario (boton) elegido para Asignar / Modificar.
    private Horario? _horarioElegido = null;

    // true mientras se cargan o sincronizan controles por codigo: los eventos
    // de combos/fecha/filtros se ignoran para no recargar a mitad de camino.
    private bool _cargando = true;

    private ToolTip _tip = null!;
    private Label _lblLeyendaDuracion = null!;

    private const string ColACobrar = "colACobrar";
    private static readonly System.Globalization.CultureInfo CulturaAr = new("es-AR");

    // Colores: misma paleta que el resto de la app (FormInicio / FormMedicos).
    private static readonly Color ColorTexto = Color.FromArgb(27, 42, 74);
    private static readonly Color ColorSecundario = Color.FromArgb(110, 120, 135);
    private static readonly Color ColorAzul = Color.FromArgb(46, 134, 222);
    private static readonly Color ColorBorde = Color.FromArgb(214, 220, 230);
    private static readonly Color ColorOk = Color.FromArgb(28, 120, 66);
    private static readonly Color ColorError = Color.FromArgb(192, 57, 43);
    private static readonly Color ColorAdvertencia = Color.FromArgb(176, 96, 0);
    private static readonly Color ChipLibreBorde = Color.FromArgb(39, 174, 96);
    private static readonly Color ChipLibreHover = Color.FromArgb(232, 246, 238);
    private static readonly Color ChipOcupadoFondo = Color.FromArgb(236, 238, 242);
    private static readonly Color ChipOcupadoBorde = Color.FromArgb(214, 218, 226);
    private static readonly Color ChipOcupadoTexto = Color.FromArgb(130, 138, 150);
    private static readonly Font FuenteChip = new("Segoe UI", 9.5F);
    private static readonly Font FuenteChica = new("Segoe UI", 8.5F);

    // Barra de contadores (se arma en ConfigurarBarraContadores, no en el disenador).
    private readonly Dictionary<string, Label> _contadores = new();
    private const string ContTotal = "Total";
    private const string ContPendientes = "Pendientes";
    private const string ContConfirmados = "Confirmados";
    private const string ContAtendidos = "Atendidos";
    private const string ContAusentes = "Ausentes";
    private const string ContCancelados = "Cancelados";

    // Duraciones habituales para elegir en CboDuracion. Si un turno ya
    // guardado tiene una duracion distinta (por ejemplo, cargada por otra
    // pantalla), se agrega dinamicamente al seleccionarlo en la grilla.
    private readonly List<int> _duracionesDisponibles = new() { 15, 20, 30, 45, 60, 90 };

    // Item del filtro "Paciente" de la barra derecha (Id 0 = todos).
    private sealed record ItemPaciente(int Id, string Nombre);

    private enum EstadoChip { Libre, Ocupado }

    private bool EsRecepcionista => _usuarioActivo != null && _usuarioActivo.Rol == RolUsuario.Recepcionista;

    private Turno? TurnoSeleccionado =>
        DgvTurnos.SelectedRows.Count > 0 ? DgvTurnos.SelectedRows[0].DataBoundItem as Turno : null;

    public FormTurnos(Usuario? usuarioActivo = null)
    {
        _usuarioActivo = usuarioActivo;
        InitializeComponent();

        components ??= new System.ComponentModel.Container();
        _tip = new ToolTip(components);

        DgvTurnos.SelectionChanged += DgvTurnos_SelectionChanged;
        DgvTurnos.CellFormatting += DgvTurnos_CellFormatting;
        BtnAsignar.Click += BtnAsignar_Click;
        BtnLimpiar.Click += BtnLimpiar_Click;
        BtnModificar.Click += BtnModificar_Click;
        BtnCancelar.Click += BtnCancelar_Click;
        BtnConfirmarAsistencia.Click += BtnConfirmarAsistencia_Click;

        ChkMostrarCancelados.CheckedChanged += (s, e) => { if (!_cargando) CargarGrilla(); };
        ChkTodosMedicos.CheckedChanged += (s, e) => { if (!_cargando) CargarGrilla(); };
        ChkFiltrarFecha.CheckedChanged += (s, e) => { if (!_cargando) CargarGrilla(); };
        CboFiltroPaciente.SelectedIndexChanged += (s, e) => { if (!_cargando) CargarGrilla(); };

        CboMedico.SelectedIndexChanged += CboMedico_SelectedIndexChanged;
        CboDuracion.SelectedIndexChanged += (s, e) =>
        {
            if (_cargando) return;
            LimpiarMensaje();
            ActualizarHorarios();
        };
        DtpFecha.ValueChanged += DtpFecha_ValueChanged;

        // Los textos con ancho limitado se recalculan cuando cambia el ancho.
        pnlNuevoTurno.SizeChanged += (s, e) => AcotarEtiquetas();
        tlpHorarios.SizeChanged += (s, e) => AcotarEtiquetas();
        tlpToolbar.SizeChanged += (s, e) => AcotarEtiquetas();
        tlpAcciones.SizeChanged += (s, e) => AcotarEtiquetas();
        foreach (var flujo in new[] { flpFiltros, FlpContadores, flpBotonesAccion, FlpLeyenda })
            flujo.SizeChanged += (s, e) => AjustarFlujos();
        pnlEstado.SizeChanged += (s, e) => AjustarMensaje();

        Load += FormTurnos_Load;
    }

    // ------------------------------------------------------------- arranque

    private void FormTurnos_Load(object? sender, EventArgs e)
    {
        _cargando = true;
        try
        {
            ConfigurarColumnas();
            ConfigurarBarraContadores();
            ConfigurarLeyenda();
            ConfigurarEstiloBotones();
            ConfigurarAyudas();
            CargarCombos();

            // Al entrar (por ejemplo la Recepcionista al iniciar sesion), la
            // grilla arranca mostrando los turnos de HOY, no el historial
            // completo - el filtro de fecha ya viene activado.
            DtpFecha.Value = DateTime.Today;
            ChkFiltrarFecha.Checked = true;
            pnlEstado.Visible = false;
        }
        finally
        {
            _cargando = false;
        }

        CargarGrilla();
        ActualizarHorarios();
        ActualizarModo();
        ActualizarAcciones();
        AcotarEtiquetas();
    }

    // Los paneles de flujo que parten sus controles en varias filas (filtros,
    // contadores, botones, leyenda) no usan AutoSize: dentro de contenedores
    // AutoSize anidados se calcula su alto con un ancho equivocado. En su lugar
    // el alto se fija aca con el ancho real, cada vez que cambia el ancho o el contenido.
    private void AjustarFlujos()
    {
        foreach (var flujo in new[] { flpFiltros, FlpContadores, flpBotonesAccion, FlpLeyenda })
        {
            if (flujo.Width <= 0) continue;
            int alto = flujo.GetPreferredSize(new Size(flujo.Width, 0)).Height;
            if (flujo.Height != alto) flujo.Height = alto;
        }
    }

    private void ConfigurarAyudas()
    {
        _tip.SetToolTip(ChkTodosMedicos, EsRecepcionista
            ? "Marcado: la tabla muestra los turnos de todos sus médicos asignados.\nSin marcar: solo los del médico elegido en «Nuevo turno»."
            : "Marcado: la tabla muestra los turnos de todos los médicos.\nSin marcar: solo los del médico elegido en «Nuevo turno».");
        _tip.SetToolTip(ChkFiltrarFecha, "Marcado: la tabla muestra solo los turnos de la fecha elegida en «Nuevo turno».\nSin marcar: muestra todas las fechas.");
        _tip.SetToolTip(ChkMostrarCancelados, "Incluye en la tabla los turnos cancelados.");
        _tip.SetToolTip(CboFiltroPaciente, "Filtra la tabla por paciente (no afecta al formulario «Nuevo turno»).");
    }

    // ------------------------------------------------------- medico / fecha

    private void CboMedico_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;
        LimpiarMensaje();
        ActualizarHorarios();
        if (_idTurnoSeleccionado == null && !ChkTodosMedicos.Checked) CargarGrilla();
    }

    private void DtpFecha_ValueChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;
        LimpiarMensaje();
        ActualizarHorarios();
        if (_idTurnoSeleccionado == null && ChkFiltrarFecha.Checked) CargarGrilla();
    }

    // ------------------------------------------------- horarios (botones)

    // Arma un boton por cada horario del medico y la fecha elegidos. Los
    // horarios salen de los bloques de AgendaMedico del dia de la semana,
    // cortados en turnos de la duracion elegida (HorarioService). Un horario
    // esta ocupado si se superpone con otro turno activo del medico (se ignora
    // el turno que se esta reprogramando) o con una ausencia cargada. Recalcula
    // tambien el aviso con el motivo real cada vez que cambia algo.
    private void ActualizarHorarios(TimeOnly? preseleccion = null)
    {
        var previa = preseleccion ?? _horarioElegido?.HoraInicio;
        _horarioElegido = null;

        FlpHorarios.SuspendLayout();
        foreach (Control c in FlpHorarios.Controls.Cast<Control>().ToList())
        {
            FlpHorarios.Controls.Remove(c);
            c.Dispose();
        }

        string? aviso;
        Color colorAviso = ColorAdvertencia;

        if (CboMedico.SelectedItem is not Medico medico)
        {
            aviso = "No hay un médico seleccionado.";
        }
        else if (CboDuracion.SelectedItem is not int duracion)
        {
            aviso = "Seleccione la duración del turno.";
        }
        else
        {
            _lblLeyendaDuracion.Text = $"· turnos de {duracion} min";
            try
            {
                aviso = CrearHorarios(medico, DateOnly.FromDateTime(DtpFecha.Value), duracion, previa);
            }
            catch (Exception ex)
            {
                aviso = "No se pudo consultar la agenda del médico: " + ex.Message;
                colorAviso = ColorError;
            }
        }

        FlpHorarios.ResumeLayout(true);
        LblAviso.ForeColor = colorAviso;
        LblAviso.Text = aviso ?? "";
        LblAviso.Visible = !string.IsNullOrEmpty(aviso);

        PintarHorarios();
        ActualizarHorarioElegido();
        AcotarEtiquetas();
    }

    // Crea los botones y devuelve el aviso a mostrar (null si no hace falta).
    private string? CrearHorarios(Medico medico, DateOnly fecha, int duracion, TimeOnly? previa)
    {
        var disponibilidad = _agendaMedicoService.ObtenerDisponibilidad(medico.Id, fecha);

        // Motivo real: ausencia de todo el dia, dia sin agenda o sin agenda cargada.
        string? motivo = disponibilidad.MotivoSinAtencion(medico.NombreCompleto);
        if (motivo != null) return motivo;

        var slots = new List<Horario>();
        foreach (var bloque in disponibilidad.Bloques)
            slots.AddRange(_horarioService.GenerarSlotsEnRango(bloque.HoraInicio, bloque.HoraFin, duracion, duracion));
        slots = slots.OrderBy(h => h.HoraInicio).ToList();

        var avisos = new List<string>();
        bool fechaPasada = fecha < DateOnly.FromDateTime(DateTime.Today);
        if (fechaPasada) avisos.Add("La fecha elegida ya pasó: no se pueden asignar turnos.");

        if (slots.Count == 0)
        {
            avisos.Add($"La agenda de ese día no tiene lugar para turnos de {duracion} min.");
            return string.Join(" ", avisos);
        }

        int idPropio = _idTurnoSeleccionado ?? 0;
        var turnos = _turnoService.ObtenerPorMedicoYFecha(medico.Id, fecha, false)
            .Where(t => t.Id != idPropio)
            .ToList();

        int libres = 0;
        foreach (var slot in slots)
        {
            bool ocupado = fechaPasada
                || turnos.Any(t => t.HoraInicio < slot.HoraFin && t.HoraFin > slot.HoraInicio)
                || disponibilidad.HayAusenciaEn(slot.HoraInicio, slot.HoraFin);

            var chip = CrearChip(slot, ocupado ? EstadoChip.Ocupado : EstadoChip.Libre);
            FlpHorarios.Controls.Add(chip);

            if (!ocupado)
            {
                libres++;
                if (previa == slot.HoraInicio) _horarioElegido = slot;
            }
        }

        if (libres == 0 && !fechaPasada) avisos.Add("No quedan horarios libres ese día.");

        string? parciales = disponibilidad.DescripcionAusenciasParciales();
        if (parciales != null) avisos.Add(parciales);

        return avisos.Count == 0 ? null : string.Join(" ", avisos);
    }

    private Button CrearChip(Horario slot, EstadoChip estado)
    {
        var chip = new Button
        {
            Text = slot.HoraInicio.ToString("HH:mm"),
            Tag = slot,
            FlatStyle = FlatStyle.Flat,
            Font = FuenteChip,
            UseVisualStyleBackColor = false,
            Margin = new Padding(0, 0, EscalaDpi.Px(this, 5), EscalaDpi.Px(this, 5)),
            Enabled = estado == EstadoChip.Libre,
            TabStop = estado == EstadoChip.Libre
        };
        chip.FlatAppearance.BorderSize = 1;

        // Ancho segun el texto medido (la fuente escala con el DPI) mas holgura.
        int ancho = TextRenderer.MeasureText(chip.Text, chip.Font).Width + EscalaDpi.Px(this, 8);
        chip.Size = new Size(Math.Max(ancho, EscalaDpi.Px(this, 58)), EscalaDpi.Px(this, 30));

        if (estado == EstadoChip.Libre)
        {
            _tip.SetToolTip(chip, slot.Rango);
            chip.Click += Chip_Click;
        }
        return chip;
    }

    private void Chip_Click(object? sender, EventArgs e)
    {
        if (sender is not Button { Tag: Horario horario }) return;
        _horarioElegido = horario;
        PintarHorarios();
        ActualizarHorarioElegido();
    }

    // Libre = blanco con borde verde; ocupado = gris deshabilitado; elegido = azul solido.
    private void PintarHorarios()
    {
        foreach (var chip in FlpHorarios.Controls.OfType<Button>())
        {
            var horario = chip.Tag as Horario;
            if (!chip.Enabled)
            {
                chip.BackColor = ChipOcupadoFondo;
                chip.ForeColor = ChipOcupadoTexto;
                chip.FlatAppearance.BorderColor = ChipOcupadoBorde;
                chip.FlatAppearance.MouseOverBackColor = ChipOcupadoFondo;
                chip.FlatAppearance.MouseDownBackColor = ChipOcupadoFondo;
            }
            else if (_horarioElegido != null && horario != null && horario.HoraInicio == _horarioElegido.HoraInicio)
            {
                chip.BackColor = ColorAzul;
                chip.ForeColor = Color.White;
                chip.FlatAppearance.BorderColor = ColorAzul;
                chip.FlatAppearance.MouseOverBackColor = ColorAzul;
                chip.FlatAppearance.MouseDownBackColor = ColorAzul;
            }
            else
            {
                chip.BackColor = Color.White;
                chip.ForeColor = ColorOk;
                chip.FlatAppearance.BorderColor = ChipLibreBorde;
                chip.FlatAppearance.MouseOverBackColor = ChipLibreHover;
                chip.FlatAppearance.MouseDownBackColor = ChipLibreHover;
            }
        }
    }

    private void ActualizarHorarioElegido()
    {
        lblHorarios.Text = _horarioElegido == null
            ? "3. Horarios libres"
            : $"3. Horarios libres · {_horarioElegido.Rango}";
    }

    private void ConfigurarLeyenda()
    {
        FlpLeyenda.Controls.Add(CrearItemLeyenda("Libre", Color.White, ChipLibreBorde));
        FlpLeyenda.Controls.Add(CrearItemLeyenda("Ocupado", ChipOcupadoFondo, ChipOcupadoBorde));
        FlpLeyenda.Controls.Add(CrearItemLeyenda("Elegido", ColorAzul, ColorAzul));

        _lblLeyendaDuracion = new Label
        {
            AutoSize = true,
            Font = FuenteChica,
            ForeColor = ColorSecundario,
            Margin = new Padding(0)
        };
        FlpLeyenda.Controls.Add(_lblLeyendaDuracion);
    }

    private Control CrearItemLeyenda(string texto, Color relleno, Color borde)
    {
        var item = new FlowLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            WrapContents = false,
            Margin = new Padding(0, 0, EscalaDpi.Px(this, 12), 0)
        };

        int lado = EscalaDpi.Px(this, 12);
        var muestra = new Panel
        {
            Size = new Size(lado, lado),
            BackColor = borde,
            Padding = new Padding(1),
            Margin = new Padding(0, EscalaDpi.Px(this, 3), EscalaDpi.Px(this, 5), 0)
        };
        muestra.Controls.Add(new Panel { Dock = DockStyle.Fill, BackColor = relleno });

        var etiqueta = new Label
        {
            AutoSize = true,
            Font = FuenteChica,
            ForeColor = ColorSecundario,
            Margin = new Padding(0),
            Text = texto
        };

        item.Controls.Add(muestra);
        item.Controls.Add(etiqueta);
        return item;
    }

    // ------------------------------------------------ seleccion en la grilla

    private void DgvTurnos_SelectionChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;

        var turno = TurnoSeleccionado;
        if (turno == null)
        {
            if (_idTurnoSeleccionado != null)
            {
                SalirDeEdicion(false);
                ActualizarHorarios();
            }
            ActualizarAcciones();
            return;
        }

        _cargando = true;
        try
        {
            _idTurnoSeleccionado = turno.Id;
            CboPaciente.SelectedValue = turno.PacienteId;
            CboMedico.SelectedValue = turno.MedicoId;
            DtpFecha.Value = turno.Fecha.ToDateTime(TimeOnly.MinValue);

            // Si el turno tiene una duracion que no esta en la lista habitual
            // (por ejemplo, cargada antes de que existiera este combo), la
            // sumamos para poder mostrarla seleccionada.
            if (!_duracionesDisponibles.Contains(turno.DuracionMinutos))
            {
                _duracionesDisponibles.Add(turno.DuracionMinutos);
                _duracionesDisponibles.Sort();
                CboDuracion.DataSource = null;
                CboDuracion.DataSource = _duracionesDisponibles;
            }
            CboDuracion.SelectedItem = turno.DuracionMinutos;
        }
        finally
        {
            _cargando = false;
        }

        // Paciente y medico de un turno ya asignado no se cambian aca (RF#05
        // solo permite modificar fecha/horario): se bloquean mientras hay fila elegida.
        LimpiarMensaje();
        ActualizarModo();
        ActualizarHorarios(turno.HoraInicio);
        ActualizarAcciones();
    }

    // Vuelve al modo "Nuevo turno": deselecciona la fila y desbloquea la tarjeta.
    private void SalirDeEdicion(bool limpiarPaciente)
    {
        bool anterior = _cargando;
        _cargando = true;
        try
        {
            DgvTurnos.SelectionChanged -= DgvTurnos_SelectionChanged;
            DgvTurnos.ClearSelection();
            DgvTurnos.SelectionChanged += DgvTurnos_SelectionChanged;

            _idTurnoSeleccionado = null;
            if (limpiarPaciente) CboPaciente.SelectedIndex = -1;
        }
        finally
        {
            _cargando = anterior;
        }

        ActualizarModo();
        ActualizarAcciones();
    }

    // Titulo y bloqueos de la tarjeta segun si hay un turno cargado para reprogramar.
    private void ActualizarModo()
    {
        bool editando = _idTurnoSeleccionado != null;
        var turno = TurnoSeleccionado;

        lblTituloNuevo.Text = editando ? "REPROGRAMAR TURNO" : "NUEVO TURNO";
        CboPaciente.Enabled = !editando;
        CboMedico.Enabled = !editando;
        BtnAsignar.Enabled = !editando;

        LblModo.Visible = editando;
        LblModo.Text = editando
            ? $"Turno de {turno?.PacienteNombre}: elija otra fecha u horario y pulse «Modificar»."
            : "";
        AcotarEtiquetas();
    }

    // ------------------------------------------------------ acciones del turno

    // Habilita los botones segun el turno seleccionado y las reglas de TurnoService.
    private void ActualizarAcciones()
    {
        var turno = TurnoSeleccionado;

        if (turno == null)
        {
            LblSeleccion.Text = "Turno seleccionado: ninguno";
            BtnModificar.Enabled = false;
            BtnCancelar.Enabled = false;
            BtnConfirmarAsistencia.Enabled = false;
            MostrarAyudaAcciones("Elija una fila de la tabla.");
            return;
        }

        LblSeleccion.Text = $"Turno seleccionado: {turno.PacienteNombre} · {turno.Fecha:dd/MM/yyyy} {turno.HoraInicio:HH:mm} · {PaletaEstados.Texto(turno.Estado)}";

        bool cancelado = turno.Estado == EstadoTurno.Cancelado;
        BtnModificar.Enabled = !cancelado;
        BtnCancelar.Enabled = !cancelado;

        bool llegoLaFecha = turno.Fecha <= DateOnly.FromDateTime(DateTime.Today);
        bool puedeConfirmar = turno.Estado == EstadoTurno.Confirmado && llegoLaFecha;
        BtnConfirmarAsistencia.Enabled = puedeConfirmar;

        string ayuda = "";
        if (!puedeConfirmar)
        {
            ayuda = turno.Estado switch
            {
                EstadoTurno.Cancelado => "Este turno está cancelado.",
                EstadoTurno.Asistio or EstadoTurno.Ausente => "La asistencia ya fue registrada.",
                EstadoTurno.Pendiente => "Solo se confirma asistencia de turnos Confirmados.",
                _ => "Se confirma a partir de la fecha del turno."
            };
        }
        MostrarAyudaAcciones(ayuda);
    }

    private void MostrarAyudaAcciones(string texto)
    {
        LblAyudaAcciones.Text = texto;
        LblAyudaAcciones.Visible = !string.IsNullOrEmpty(texto);
        AcotarEtiquetas();
    }

    private void BtnLimpiar_Click(object? sender, EventArgs e)
    {
        SalirDeEdicion(true);
        _horarioElegido = null;
        LimpiarMensaje();
        ActualizarHorarios();
    }

    private void BtnAsignar_Click(object? sender, EventArgs e)
    {
        var faltantes = new List<string>();
        if (CboPaciente.SelectedItem is not Paciente) faltantes.Add("paciente");
        if (CboMedico.SelectedItem is not Medico) faltantes.Add("médico");
        if (CboDuracion.SelectedItem is not int) faltantes.Add("duración");
        if (_horarioElegido == null) faltantes.Add("horario");

        if (faltantes.Count > 0)
        {
            MostrarMensaje("Debe seleccionar " + string.Join(", ", faltantes) + ".", ColorError);
            return;
        }

        var paciente = (Paciente)CboPaciente.SelectedItem!;
        var medico = (Medico)CboMedico.SelectedItem!;
        var horario = _horarioElegido!;
        var fecha = DateOnly.FromDateTime(DtpFecha.Value);
        int duracion = (int)CboDuracion.SelectedItem!;

        try
        {
            _turnoService.AsignarTurno(paciente, medico, fecha, horario.HoraInicio, duracion);
        }
        catch (Exception ex)
        {
            // El horario pudo ocuparse desde otra pantalla: se refrescan los botones.
            MostrarMensaje(ex.Message, ColorError);
            ActualizarHorarios();
            return;
        }

        SalirDeEdicion(true);
        _horarioElegido = null;
        CargarGrilla();
        ActualizarHorarios();

        string aviso;
        try
        {
            aviso = _notificacionService.AvisarTurno(
                paciente, "Confirmacion", $"{fecha:dd/MM/yyyy} {horario.Rango} con {medico.NombreCompleto}");
        }
        catch (Exception ex)
        {
            aviso = "No se pudo avisar al paciente: " + ex.Message;
        }
        MostrarMensaje("Turno agendado. " + aviso, ColorOk);
    }

    private void BtnModificar_Click(object? sender, EventArgs e)
    {
        var seleccionado = TurnoSeleccionado;
        if (seleccionado == null || _idTurnoSeleccionado == null)
        {
            MostrarMensaje("Seleccione un turno de la lista para modificar.", ColorError);
            return;
        }

        if (_horarioElegido == null || CboDuracion.SelectedItem is not int duracion)
        {
            MostrarMensaje("Seleccione un horario libre y una duración para reprogramar el turno.", ColorError);
            return;
        }

        var horario = _horarioElegido;
        var fecha = DateOnly.FromDateTime(DtpFecha.Value);
        int idTurno = _idTurnoSeleccionado.Value;

        try
        {
            _turnoService.ModificarTurno(idTurno, fecha, horario.HoraInicio, duracion);
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message, ColorError);
            ActualizarHorarios();
            return;
        }

        string aviso = "";
        try
        {
            var turno = _turnoService.ObtenerPorId(idTurno);
            if (turno?.Paciente != null)
                aviso = _notificacionService.AvisarTurno(turno.Paciente, "Reprogramacion", $"{fecha:dd/MM/yyyy} {horario.Rango}");
        }
        catch (Exception ex)
        {
            aviso = "No se pudo avisar al paciente: " + ex.Message;
        }

        SalirDeEdicion(true);
        _horarioElegido = null;
        CargarGrilla();
        ActualizarHorarios();
        MostrarMensaje("Turno modificado. " + aviso, ColorOk);
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        var turnoSeleccionado = TurnoSeleccionado;
        if (turnoSeleccionado == null)
        {
            MostrarMensaje("Seleccione un turno de la lista primero.", ColorError);
            return;
        }

        var respuesta = MessageBox.Show(
            $"¿Está seguro que desea cancelar el turno de {turnoSeleccionado.PacienteNombre} del {turnoSeleccionado.Fecha:dd/MM/yyyy}?",
            "Confirmar cancelación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (respuesta != DialogResult.Yes)
            return;

        try
        {
            _turnoService.CancelarTurno(turnoSeleccionado.Id);
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message, ColorError);
            return;
        }

        string aviso = "";
        try
        {
            if (turnoSeleccionado.Paciente != null)
                aviso = _notificacionService.AvisarTurno(turnoSeleccionado.Paciente, "Cancelacion", $"{turnoSeleccionado.Fecha:dd/MM/yyyy} {turnoSeleccionado.HorarioRango}");
        }
        catch (Exception ex)
        {
            aviso = "No se pudo avisar al paciente: " + ex.Message;
        }

        SalirDeEdicion(true);
        _horarioElegido = null;
        CargarGrilla();
        ActualizarHorarios();
        MostrarMensaje("Turno cancelado. " + aviso, ColorOk);
    }

    private void BtnConfirmarAsistencia_Click(object? sender, EventArgs e)
    {
        var turno = TurnoSeleccionado;
        if (turno == null)
        {
            MostrarMensaje("Seleccione un turno de la lista primero.", ColorError);
            return;
        }

        using (var dialogo = new FormConfirmarAsistencia(turno))
        {
            if (dialogo.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                _turnoService.ConfirmarAsistencia(turno.Id, dialogo.Asistio, dialogo.MedioPagoSeleccionado, dialogo.MontoSeleccionado);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, ColorError);
                return;
            }
        }

        SalirDeEdicion(true);
        _horarioElegido = null;
        CargarGrilla();
        ActualizarHorarios();
        MostrarMensaje("Asistencia confirmada.", ColorOk);
    }

    // ----------------------------------------------------------- grilla y datos

    private void ConfigurarColumnas()
    {
        DgvTurnos.AutoGenerateColumns = false;
        DgvTurnos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvTurnos.ColumnHeadersHeight = EscalaDpi.Px(this, 46);
        DgvTurnos.RowTemplate.Height = EscalaDpi.Px(this, 28);

        // Encabezado claro, acorde al resto de la app.
        DgvTurnos.EnableHeadersVisualStyles = false;
        DgvTurnos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 250);
        DgvTurnos.ColumnHeadersDefaultCellStyle.ForeColor = ColorTexto;
        DgvTurnos.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 246, 250);
        DgvTurnos.ColumnHeadersDefaultCellStyle.SelectionForeColor = ColorTexto;
        DgvTurnos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        DgvTurnos.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
        DgvTurnos.ColumnHeadersDefaultCellStyle.Padding = new Padding(EscalaDpi.Px(this, 4), 0, 0, 0);
        DgvTurnos.GridColor = Color.FromArgb(228, 232, 240);

        // MinimumWidth hace que, en una ventana angosta, la tabla scrollee
        // horizontalmente dentro de si misma en vez de apretar las columnas.
        // Las columnas de contenido corto (fecha, horario, estado, importes)
        // nunca bajan del ancho de su texto; medico, paciente y medio de pago
        // se reparten el resto y se recortan si falta lugar.
        AgregarColumna("colFecha", "Fecha", "Fecha", 80, MinimoPara("00/00/0000"), "dd/MM/yyyy");
        AgregarColumna("colMedico", "Médico", "MedicoNombre", 130, EscalaDpi.Px(this, 56));
        AgregarColumna("colPaciente", "Paciente", "PacienteNombre", 150, EscalaDpi.Px(this, 60));
        AgregarColumna("colHorario", "Horario", "HorarioRango", 100, MinimoPara("00:00 - 00:00"));
        AgregarColumna("colEstado", "Estado", "Estado", 85, MinimoPara("Confirmado"));
        AgregarColumna("colMedioPago", "Medio de pago", "MedioPago", 100, EscalaDpi.Px(this, 56));
        var monto = AgregarColumna("colMonto", "Monto", "Monto", 85, MinimoPara("$ 00.000,00"), "C2");
        monto.DefaultCellStyle.FormatProvider = CulturaAr;
        monto.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        // Columna calculada (sin DataPropertyName): su valor se arma en
        // DgvTurnos_CellFormatting con la regla de cobro de TurnoService.
        var acobrar = AgregarColumna(ColACobrar, "A cobrar (estimado)", null, 110, MinimoPara("$ 00.000,00"));
        acobrar.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        // Fila seleccionada: mismo azul que el resto de la app, texto blanco.
        DgvTurnos.DefaultCellStyle.SelectionBackColor = ColorAzul;
        DgvTurnos.DefaultCellStyle.SelectionForeColor = Color.White;
    }

    // Ancho en pixeles reales del texto de muestra con la fuente de la grilla, mas holgura.
    private int MinimoPara(string muestra) =>
        TextRenderer.MeasureText(muestra, DgvTurnos.Font).Width + EscalaDpi.Px(this, 6);

    // El encabezado puede partirse en varias lineas, pero no una palabra: el
    // minimo de la columna alcanza para la palabra mas larga del titulo.
    private int MinimoEncabezado(string titulo)
    {
        var fuente = DgvTurnos.ColumnHeadersDefaultCellStyle.Font ?? DgvTurnos.Font;
        int palabra = titulo.Split(' ').Max(w => TextRenderer.MeasureText(w, fuente).Width);
        return palabra + EscalaDpi.Px(this, 14);
    }

    private DataGridViewTextBoxColumn AgregarColumna(string nombre, string titulo, string? propiedad, float peso, int anchoMinimo, string? formato = null)
    {
        var columna = new DataGridViewTextBoxColumn
        {
            Name = nombre,
            HeaderText = titulo,
            FillWeight = peso,
            MinimumWidth = Math.Max(anchoMinimo, MinimoEncabezado(titulo)),
            SortMode = DataGridViewColumnSortMode.NotSortable
        };
        if (propiedad != null) columna.DataPropertyName = propiedad;
        if (formato != null) columna.DefaultCellStyle.Format = formato;
        columna.DefaultCellStyle.NullValue = "-";
        DgvTurnos.Columns.Add(columna);
        return columna;
    }

    private void CargarCombos()
    {
        var pacientes = new List<Paciente>();
        try
        {
            pacientes = _pacienteService.ObtenerTodos();
        }
        catch (Exception ex)
        {
            MostrarMensaje("No se pudieron cargar los pacientes: " + ex.Message, ColorError);
        }

        CboPaciente.DataSource = pacientes;
        CboPaciente.DisplayMember = "NombreCompleto";
        CboPaciente.ValueMember = "Id";
        // Sin paciente elegido por defecto: evita asignar al primero por descuido.
        CboPaciente.SelectedIndex = -1;

        var itemsFiltro = new List<ItemPaciente> { new(0, "Todos los pacientes") };
        itemsFiltro.AddRange(pacientes.Select(p => new ItemPaciente(p.Id, p.NombreCompleto)));
        CboFiltroPaciente.DataSource = itemsFiltro;
        CboFiltroPaciente.DisplayMember = nameof(ItemPaciente.Nombre);
        CboFiltroPaciente.ValueMember = nameof(ItemPaciente.Id);
        CboFiltroPaciente.SelectedIndex = 0;

        // Una recepcionista solo ve los medicos que tiene asignados.
        try
        {
            if (EsRecepcionista)
                CboMedico.DataSource = (_usuarioActivo!.MedicosAsignados ?? new List<Medico>()).ToList();
            else
                CboMedico.DataSource = _medicoService.ObtenerTodos();
        }
        catch (Exception ex)
        {
            CboMedico.DataSource = new List<Medico>();
            MostrarMensaje("No se pudieron cargar los médicos: " + ex.Message, ColorError);
        }

        CboMedico.DisplayMember = "NombreCompleto";
        CboMedico.ValueMember = "Id";

        CboDuracion.DataSource = _duracionesDisponibles;
        CboDuracion.SelectedItem = 30;
    }

    private void CargarGrilla()
    {
        int? medicoFiltro = null;
        if (!ChkTodosMedicos.Checked && CboMedico.SelectedItem is Medico medicoElegido)
            medicoFiltro = medicoElegido.Id;

        DateOnly? fechaFiltro = ChkFiltrarFecha.Checked
            ? DateOnly.FromDateTime(DtpFecha.Value)
            : null;

        // El filtro por paciente es el de la barra derecha (independiente del
        // paciente del formulario "Nuevo turno").
        int? pacienteFiltro = null;
        string textoPaciente = "todos los pacientes";
        if (CboFiltroPaciente.SelectedItem is ItemPaciente { Id: > 0 } itemPaciente)
        {
            pacienteFiltro = itemPaciente.Id;
            textoPaciente = itemPaciente.Nombre;
        }

        int? idPrevio = _idTurnoSeleccionado;
        List<Turno> lista;
        string? errorCarga = null;

        try
        {
            lista = _turnoService.ObtenerTodos(ChkMostrarCancelados.Checked, medicoFiltro, fechaFiltro, pacienteFiltro);

            if (EsRecepcionista)
            {
                var medicosAsignadosIds = (_usuarioActivo!.MedicosAsignados ?? new List<Medico>()).Select(m => m.Id).ToList();
                lista = lista.Where(t => medicosAsignadosIds.Contains(t.MedicoId)).ToList();
            }
        }
        catch (Exception ex)
        {
            errorCarga = "No se pudieron cargar los turnos: " + ex.Message;
            lista = new List<Turno>();
        }

        bool anterior = _cargando;
        _cargando = true;
        bool reseleccionado = false;
        try
        {
            DgvTurnos.SelectionChanged -= DgvTurnos_SelectionChanged;
            DgvTurnos.DataSource = lista;

            // Medico y fecha solo son columna cuando el filtro no los fija (si no,
            // serian iguales en todas las filas y ya figuran en el resumen de arriba).
            DgvTurnos.Columns["colMedico"]!.Visible = ChkTodosMedicos.Checked;
            DgvTurnos.Columns["colFecha"]!.Visible = !ChkFiltrarFecha.Checked;
            DgvTurnos.ClearSelection();

            // Si se estaba reprogramando un turno y sigue en la lista, queda elegido.
            if (idPrevio != null)
            {
                foreach (DataGridViewRow fila in DgvTurnos.Rows)
                {
                    if (fila.DataBoundItem is Turno t && t.Id == idPrevio)
                    {
                        fila.Selected = true;
                        DgvTurnos.CurrentCell = fila.Cells.Cast<DataGridViewCell>().First(c => c.Visible);
                        reseleccionado = true;
                        break;
                    }
                }
            }
            DgvTurnos.SelectionChanged += DgvTurnos_SelectionChanged;
        }
        finally
        {
            _cargando = anterior;
        }

        ActualizarContadores(lista);

        // Resumen de lo que muestra la tabla ahora (los filtros aplicados).
        if (errorCarga != null)
        {
            LblVista.ForeColor = ColorError;
            LblVista.Text = errorCarga;
        }
        else
        {
            string textoMedico = medicoFiltro != null
                ? ((Medico)CboMedico.SelectedItem!).NombreCompleto
                : (EsRecepcionista ? "todos sus médicos" : "todos los médicos");
            string textoFecha = fechaFiltro != null ? fechaFiltro.Value.ToString("dd/MM/yyyy") : "todas las fechas";
            string cancelados = ChkMostrarCancelados.Checked ? " · incluye cancelados" : "";
            LblVista.ForeColor = ColorSecundario;
            LblVista.Text = $"Mostrando: {textoMedico} · {textoFecha} · {textoPaciente}{cancelados}";
        }
        AcotarEtiquetas();

        if (idPrevio != null && !reseleccionado)
        {
            SalirDeEdicion(false);
            ActualizarHorarios();
        }
        else
        {
            ActualizarModo();
            ActualizarAcciones();
        }
    }

    // Arma la franja de contadores. Se crea aca (y no en InitializeComponent)
    // para que el disenador no la borre.
    private void ConfigurarBarraContadores()
    {
        AgregarContador(ContTotal, Color.FromArgb(27, 42, 74), Color.White);
        AgregarContador(ContPendientes, PaletaEstados.Fondo(EstadoTurno.Pendiente), PaletaEstados.Acento(EstadoTurno.Pendiente));
        AgregarContador(ContConfirmados, PaletaEstados.Fondo(EstadoTurno.Confirmado), PaletaEstados.Acento(EstadoTurno.Confirmado));
        AgregarContador(ContAtendidos, PaletaEstados.Fondo(EstadoTurno.Asistio), Color.FromArgb(28, 120, 66));
        AgregarContador(ContAusentes, PaletaEstados.Fondo(EstadoTurno.Ausente), Color.FromArgb(180, 50, 38));
        AgregarContador(ContCancelados, PaletaEstados.Fondo(EstadoTurno.Cancelado), Color.FromArgb(90, 98, 112));
        ActualizarContadores(new List<Turno>());
    }

    private void AgregarContador(string clave, Color fondo, Color texto)
    {
        var etiqueta = new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            BackColor = fondo,
            ForeColor = texto,
            Margin = new Padding(0, 0, EscalaDpi.Px(this, 6), EscalaDpi.Px(this, 4)),
            Padding = new Padding(EscalaDpi.Px(this, 8), EscalaDpi.Px(this, 4), EscalaDpi.Px(this, 8), EscalaDpi.Px(this, 4)),
            Name = "lblContador" + clave
        };
        _contadores[clave] = etiqueta;
        FlpContadores.Controls.Add(etiqueta);
    }

    // Cuenta sobre lo que muestra la grilla (con los filtros ya aplicados).
    private void ActualizarContadores(List<Turno> turnos)
    {
        int Contar(EstadoTurno estado) => turnos.Count(t => t.Estado == estado);

        _contadores[ContTotal].Text = $"Total: {turnos.Count}";
        _contadores[ContPendientes].Text = $"Pendientes: {Contar(EstadoTurno.Pendiente)}";
        _contadores[ContConfirmados].Text = $"Confirmados: {Contar(EstadoTurno.Confirmado)}";
        _contadores[ContAtendidos].Text = $"Atendidos: {Contar(EstadoTurno.Asistio)}";
        _contadores[ContAusentes].Text = $"Ausentes: {Contar(EstadoTurno.Ausente)}";
        _contadores[ContCancelados].Text = $"Cancelados: {Contar(EstadoTurno.Cancelado)}";
    }

    // Color de fila por estado, texto de estado con acento y monto estimado.
    private void DgvTurnos_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= DgvTurnos.Rows.Count) return;
        if (DgvTurnos.Rows[e.RowIndex].DataBoundItem is not Turno turno) return;

        if (e.CellStyle == null) return;

        e.CellStyle.BackColor = PaletaEstados.Fondo(turno.Estado);
        e.CellStyle.ForeColor = turno.Estado == EstadoTurno.Cancelado
            ? Color.FromArgb(110, 120, 135)
            : Color.FromArgb(30, 35, 45);

        string columna = DgvTurnos.Columns[e.ColumnIndex].Name;

        if (columna == "colMedico")
        {
            // Sin la especialidad para ahorrar ancho; el nombre completo queda en el tooltip.
            if (turno.Medico != null)
            {
                e.Value = $"{turno.Medico.Apellido}, {turno.Medico.Nombre}";
                e.FormattingApplied = true;
                DgvTurnos.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = turno.MedicoNombre;
            }
        }
        else if (columna == "colEstado")
        {
            e.Value = PaletaEstados.Texto(turno.Estado);
            e.FormattingApplied = true;
        }
        else if (columna == ColACobrar)
        {
            // Solo tiene sentido estimar lo que todavia no se cobro: para un
            // turno ya atendido manda el Monto real, y ausente/cancelado no cobra.
            bool pendienteDeCobro = turno.Estado == EstadoTurno.Pendiente || turno.Estado == EstadoTurno.Confirmado;
            e.Value = pendienteDeCobro
                ? TurnoService.CalcularMontoEstimado(turno).ToString("C2", CulturaAr)
                : "-";
            e.FormattingApplied = true;
        }
    }

    // ----------------------------------------------------- mensajes y estilo

    private void MostrarMensaje(string texto, Color color)
    {
        LblMensaje.ForeColor = color;
        LblMensaje.Text = texto;
        pnlEstado.Visible = !string.IsNullOrEmpty(texto);
        AjustarMensaje();
    }

    private void LimpiarMensaje() => MostrarMensaje("", ColorOk);

    // La franja de mensajes crece con el texto (varias lineas si hace falta).
    private void AjustarMensaje()
    {
        if (!pnlEstado.Visible || string.IsNullOrEmpty(LblMensaje.Text)) return;

        int ancho = pnlEstado.ClientSize.Width - pnlEstado.Padding.Horizontal;
        if (ancho <= 0) return;

        int alto = TextRenderer.MeasureText(LblMensaje.Text, LblMensaje.Font,
            new Size(ancho, int.MaxValue), TextFormatFlags.WordBreak).Height;
        int deseado = alto + pnlEstado.Padding.Vertical + EscalaDpi.Px(this, 4);
        if (pnlEstado.Height != deseado) pnlEstado.Height = deseado;
    }

    // Limita el ancho de los textos que pueden ocupar varias lineas al de su contenedor.
    private void AcotarEtiquetas()
    {
        Acotar(LblModo, tlpNuevoTurno);
        Acotar(LblAviso, tlpHorarios);
        Acotar(LblVista, tlpToolbar);
        Acotar(LblSeleccion, tlpAcciones);
        Acotar(LblAyudaAcciones, tlpAcciones);
        AjustarFlujos();
        AjustarMensaje();
    }

    private static void Acotar(Label etiqueta, Control contenedor)
    {
        int ancho = contenedor.ClientSize.Width;
        if (ancho <= 0) return;

        var maximo = new Size(ancho, 0);
        if (etiqueta.MaximumSize != maximo) etiqueta.MaximumSize = maximo;
    }

    // Botones planos de color: cuando se deshabilitan pasan a un gris uniforme.
    private void ConfigurarEstiloBotones()
    {
        EstilizarBoton(BtnAsignar);
        EstilizarBoton(BtnModificar);
        EstilizarBoton(BtnCancelar);
        EstilizarBoton(BtnConfirmarAsistencia);
        BtnLimpiar.Cursor = Cursors.Hand;
    }

    private static void EstilizarBoton(Button boton)
    {
        Color colorActivo = boton.BackColor;
        void Aplicar()
        {
            boton.BackColor = boton.Enabled ? colorActivo : Color.FromArgb(226, 230, 237);
            boton.ForeColor = boton.Enabled ? Color.White : Color.FromArgb(140, 148, 160);
            boton.Cursor = boton.Enabled ? Cursors.Hand : Cursors.Default;
        }
        boton.EnabledChanged += (s, e) => Aplicar();
        Aplicar();
    }
}
