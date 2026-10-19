﻿using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormAgendaMedico : Form
{
    private enum FiltroTurno
    {
        Todos,
        Pendientes,
        Atendidos,
        Cancelados
    }

    private readonly Usuario? _usuarioActivo;
    private readonly Action<Form>? _navegador;
    private readonly MedicoService _medicoService = new();
    private readonly AgendaDiariaService _agendaService = new();
    private readonly ActividadMedicaService _actividadService = new();

    private Medico? _medicoActual;
    private Turno? _turnoSeleccionado;
    private FiltroTurno _filtroActual = FiltroTurno.Pendientes;
    private List<TurnoAgendaItem> _turnosDelDia = new();
    private TurnoAgendaItem? _proximoPaciente;

    // Panel "Proximo paciente" y resumen del dia: se arman en codigo (no en el
    // Designer) para que el disenador visual no los borre.
    private Label _lblProximoTitulo = null!;
    private Label _lblProximoDetalle = null!;
    private Label _lblResumenDia = null!;

    public FormAgendaMedico(Usuario? usuarioActivo = null, Action<Form>? navegador = null)
    {
        InitializeComponent();
        _usuarioActivo = usuarioActivo;
        _navegador = navegador;

        DeterminarMedicoActivo();
        ConfigurarColumnas();
        CrearPanelProximoPaciente();

        DtpFecha.Value = DateTime.Today;
        DtpFecha.ValueChanged += (s, e) => CargarTurnos();
        BtnHoy.Click += (s, e) => DtpFecha.Value = DateTime.Today;

        BtnFiltroTotal.Click += (s, e) => { _filtroActual = FiltroTurno.Todos; CargarTurnos(); };
        BtnFiltroPendientes.Click += (s, e) => { _filtroActual = FiltroTurno.Pendientes; CargarTurnos(); };
        BtnFiltroAtendidos.Click += (s, e) => { _filtroActual = FiltroTurno.Atendidos; CargarTurnos(); };
        BtnFiltroCancelados.Click += (s, e) => { _filtroActual = FiltroTurno.Cancelados; CargarTurnos(); };

        DgvTurnos.SelectionChanged += DgvTurnos_SelectionChanged;
        DgvTurnos.CellDoubleClick += (s, e) => BtnAtender_Click(s, e);
        DgvTurnos.CellFormatting += DgvTurnos_CellFormatting;
        BtnAtender.Click += BtnAtender_Click;
        BtnHistorialRapido.Click += BtnHistorialRapido_Click;
        TxtBuscarPaciente.TextChanged += (s, e) => PintarGrilla();

        CargarTurnos();
    }

    private void DeterminarMedicoActivo()
    {
        try
        {
            if (_usuarioActivo?.MedicoId != null)
            {
                _medicoActual = _medicoService.ObtenerPorId(_usuarioActivo.MedicoId.Value);
            }

            if (_medicoActual == null)
            {
                _medicoActual = _medicoService.ObtenerTodos().FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            _medicoActual = null;
            MessageBox.Show("No se pudo cargar el perfil del profesional: " + ex.Message,
                "Error al cargar", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        if (_medicoActual != null)
        {
            lblMedicoInfo.Text = $"Profesional: {_medicoActual.NombreCompleto} | Matrícula: {_medicoActual.Matricula}";
        }
        else
        {
            lblMedicoInfo.Text = "Profesional: no se encontró un perfil médico asociado.";
        }
    }

    private void ConfigurarColumnas()
    {
        DgvTurnos.AutoGenerateColumns = false;
        DgvTurnos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvTurnos.EnableHeadersVisualStyles = false;
        DgvTurnos.ColumnHeadersHeight = 34;
        DgvTurnos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(27, 42, 74);
        DgvTurnos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        DgvTurnos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        DgvTurnos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 134, 222);
        DgvTurnos.DefaultCellStyle.SelectionForeColor = Color.White;
        DgvTurnos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 252);
        DgvTurnos.RowTemplate.Height = 30;
        DgvTurnos.GridColor = Color.FromArgb(225, 230, 238);

        var estiloCentrado = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, NullValue = "-" };

        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHorario", HeaderText = "Horario", DataPropertyName = "HorarioRango", FillWeight = 14 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPaciente", HeaderText = "Paciente", DataPropertyName = "PacienteNombre", FillWeight = 24 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDni", HeaderText = "DNI Paciente", DataPropertyName = "PacienteDni", FillWeight = 13 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEdad", HeaderText = "Edad", DataPropertyName = "Edad", FillWeight = 7, DefaultCellStyle = estiloCentrado });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colObraSocial", HeaderText = "Obra Social", DataPropertyName = "ObraSocial", FillWeight = 18 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPerfil", HeaderText = "Tipo de paciente", DataPropertyName = "Perfil", FillWeight = 12, DefaultCellStyle = estiloCentrado });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstadoAtencion", HeaderText = "Atención Clínica", DataPropertyName = "EstadoAtencion", FillWeight = 14 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstado", HeaderText = "Estado Turno", DataPropertyName = "Estado", FillWeight = 14 });

        foreach (DataGridViewColumn columna in DgvTurnos.Columns)
            columna.MinimumWidth = 50;
    }

    // Tarjeta destacada bajo los contadores: proximo paciente a la izquierda y
    // resumen del dia (total / atendidos / pendientes) a la derecha.
    private void CrearPanelProximoPaciente()
    {
        var contenedor = new Panel
        {
            Name = "pnlProximoContenedor",
            Dock = DockStyle.Top,
            Height = 74,
            Padding = new Padding(20, 0, 20, 10),
            BackColor = Color.FromArgb(245, 246, 250)
        };

        var tarjeta = new Panel
        {
            Name = "pnlProximoPaciente",
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(222, 236, 252)
        };

        _lblProximoDetalle = new Label
        {
            Name = "lblProximoDetalle",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = Color.FromArgb(27, 42, 74),
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true,
            Padding = new Padding(14, 0, 0, 4)
        };
        _lblProximoTitulo = new Label
        {
            Name = "lblProximoTitulo",
            Dock = DockStyle.Top,
            Height = 24,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(41, 128, 185),
            TextAlign = ContentAlignment.BottomLeft,
            Padding = new Padding(14, 0, 0, 0),
            Text = "PRÓXIMO PACIENTE"
        };
        _lblResumenDia = new Label
        {
            Name = "lblResumenDia",
            Dock = DockStyle.Right,
            Width = 360,
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(27, 42, 74),
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(0, 0, 14, 0),
            AutoEllipsis = true
        };
        var barra = new Panel
        {
            Name = "pnlProximoBarra",
            Dock = DockStyle.Left,
            Width = 6,
            BackColor = Color.FromArgb(46, 134, 222)
        };

        // El orden importa para el docking: el control de relleno primero y
        // los de borde despues (el ultimo agregado se acopla primero).
        tarjeta.Controls.Add(_lblProximoDetalle);
        tarjeta.Controls.Add(_lblProximoTitulo);
        tarjeta.Controls.Add(_lblResumenDia);
        tarjeta.Controls.Add(barra);
        contenedor.Controls.Add(tarjeta);

        // Debe quedar entre pnlResumen y pnlGrilla (que rellena el resto).
        Controls.Add(contenedor);
        Controls.SetChildIndex(contenedor, Controls.GetChildIndex(pnlResumen));
    }

    private void ActualizarEstiloBotonesFiltro()
    {
        // Resaltamos con borde grueso y color al boton activo
        BtnFiltroTotal.FlatAppearance.BorderSize = _filtroActual == FiltroTurno.Todos ? 3 : 1;
        BtnFiltroPendientes.FlatAppearance.BorderSize = _filtroActual == FiltroTurno.Pendientes ? 3 : 1;
        BtnFiltroAtendidos.FlatAppearance.BorderSize = _filtroActual == FiltroTurno.Atendidos ? 3 : 1;
        BtnFiltroCancelados.FlatAppearance.BorderSize = _filtroActual == FiltroTurno.Cancelados ? 3 : 1;
    }

    // Pendiente de atencion = turno Pendiente o Confirmado que todavia no se atendio.
    // Asistio / Ausente (ya resueltos en recepcion) y Cancelado no cuentan.
    private static bool EsPendienteDeAtencion(TurnoAgendaItem t)
        => (t.Estado == EstadoTurno.Pendiente || t.Estado == EstadoTurno.Confirmado) && !t.Atendido;

    private static string Plural(int cantidad, string singular, string plural)
        => $"{cantidad} {(cantidad == 1 ? singular : plural)}";

    private void CargarTurnos()
    {
        if (_medicoActual == null) return;

        ActualizarEstiloBotonesFiltro();

        var fechaSeleccionada = DateOnly.FromDateTime(DtpFecha.Value);

        // Siempre traemos todos los turnos del dia para calcular los totales reales
        List<TurnoAgendaItem> todosDelDia;
        try
        {
            todosDelDia = _agendaService.ObtenerAgenda(_medicoActual.Id, fechaSeleccionada);
        }
        catch (Exception ex)
        {
            todosDelDia = new List<TurnoAgendaItem>();
            MessageBox.Show("No se pudo cargar la agenda del día: " + ex.Message,
                "Error al cargar", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        int total = todosDelDia.Count;
        int cancelados = todosDelDia.Count(t => t.Estado == EstadoTurno.Cancelado);
        int atendidos = todosDelDia.Count(t => t.Atendido);
        int pendientes = todosDelDia.Count(EsPendienteDeAtencion);

        BtnFiltroTotal.Text = $"TOTAL TURNOS ({total})";
        BtnFiltroPendientes.Text = $"PENDIENTES ({pendientes})";
        BtnFiltroAtendidos.Text = $"ATENDIDOS ({atendidos})";
        BtnFiltroCancelados.Text = $"CANCELADOS ({cancelados})";

        ActualizarProximoPaciente(todosDelDia, fechaSeleccionada, total);
        _lblResumenDia.Text = $"Resumen del día: {Plural(total, "turno", "turnos")} · " +
                              $"{Plural(atendidos, "atendido", "atendidos")} · {Plural(pendientes, "pendiente", "pendientes")}";

        // Filtramos segun el boton presionado
        IEnumerable<TurnoAgendaItem> filtrados = todosDelDia;
        switch (_filtroActual)
        {
            case FiltroTurno.Pendientes:
                filtrados = todosDelDia.Where(EsPendienteDeAtencion);
                lblGrillaTitulo.Text = $"Turnos pendientes de atención ({pendientes})";
                break;
            case FiltroTurno.Atendidos:
                filtrados = todosDelDia.Where(t => t.Atendido);
                lblGrillaTitulo.Text = $"Pacientes ya atendidos ({atendidos})";
                break;
            case FiltroTurno.Cancelados:
                filtrados = todosDelDia.Where(t => t.Estado == EstadoTurno.Cancelado);
                lblGrillaTitulo.Text = $"Turnos cancelados ({cancelados})";
                break;
            default:
                filtrados = todosDelDia;
                lblGrillaTitulo.Text = $"Lista completa de turnos del día ({total})";
                break;
        }

        _turnosDelDia = filtrados.ToList();
        PintarGrilla();
    }

    // Calcula y muestra el proximo paciente del dia consultado: hoy, el primer
    // turno pendiente desde la hora actual; otro dia, el primero del dia.
    private void ActualizarProximoPaciente(List<TurnoAgendaItem> todosDelDia, DateOnly fecha, int totalTurnos)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        _proximoPaciente = AgendaDiariaService.ElegirProximo(todosDelDia, fecha, hoy, TimeOnly.FromDateTime(DateTime.Now));

        _lblProximoTitulo.Text = fecha == hoy
            ? "PRÓXIMO PACIENTE"
            : $"PRIMER PACIENTE DEL {fecha:dd/MM/yyyy}";

        if (_proximoPaciente == null)
        {
            _lblProximoDetalle.Text = totalTurnos == 0
                ? "No hay turnos para este día."
                : fecha == hoy
                    ? "No quedan pacientes por atender hoy."
                    : "No hay pacientes pendientes de atención este día.";
            _lblProximoDetalle.ForeColor = Color.FromArgb(100, 110, 120);
            return;
        }

        var p = _proximoPaciente;
        string edad = p.Edad is int anios ? $"{anios} años" : "Sin datos de edad";
        _lblProximoDetalle.Text = $"{p.PacienteNombre}   ·   {p.Turno.HoraInicio:HH:mm} hs   ·   {edad}   ·   {p.ObraSocial}   ·   {p.Perfil}";
        _lblProximoDetalle.ForeColor = Color.FromArgb(27, 42, 74);
    }

    private void PintarGrilla()
    {
        var filtro = TxtBuscarPaciente.Text?.Trim() ?? "";
        IEnumerable<TurnoAgendaItem> visible = _turnosDelDia;
        if (!string.IsNullOrWhiteSpace(filtro))
        {
            var f = filtro.ToLower();
            visible = _turnosDelDia.Where(t =>
                t.PacienteNombre.ToLower().Contains(f) ||
                t.PacienteDni.Contains(f));
        }

        var listaVisible = visible.ToList();
        var fechaSeleccionada = DateOnly.FromDateTime(DtpFecha.Value);

        DgvTurnos.SelectionChanged -= DgvTurnos_SelectionChanged;
        DgvTurnos.DataSource = listaVisible;

        lblTurnosContador.Text = $"{listaVisible.Count} turno(s) visibles para el {fechaSeleccionada:dd/MM/yyyy}";

        // Coloreamos las filas
        for (int i = 0; i < DgvTurnos.Rows.Count; i++)
        {
            if (listaVisible[i].Estado == EstadoTurno.Cancelado)
            {
                DgvTurnos.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(250, 235, 235);
                DgvTurnos.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(180, 50, 50);
            }
            else if (listaVisible[i].Atendido)
            {
                DgvTurnos.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(235, 250, 240);
                DgvTurnos.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(30, 100, 50);
            }

            // El proximo paciente se destaca en negrita tambien dentro de la lista.
            if (ReferenceEquals(listaVisible[i], _proximoPaciente))
                DgvTurnos.Rows[i].DefaultCellStyle.Font = new Font(DgvTurnos.Font, FontStyle.Bold);
        }

        DgvTurnos.ClearSelection();
        _turnoSeleccionado = null;
        BtnAtender.Enabled = false;
        BtnHistorialRapido.Enabled = false;
        LblMensaje.Text = "Sala de espera: atienda para escribir la consulta. El historial solo se consulta.";
        LblMensaje.ForeColor = Color.FromArgb(100, 110, 120);

        DgvTurnos.SelectionChanged += DgvTurnos_SelectionChanged;

        if (listaVisible.Count > 0)
        {
            DgvTurnos.Rows[0].Selected = true;
        }
    }

    private void DgvTurnos_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0 || DgvTurnos.Rows[e.RowIndex].DataBoundItem is not TurnoAgendaItem item)
            return;

        string columna = DgvTurnos.Columns[e.ColumnIndex].Name;
        if (columna == "colEstado" && e.Value is EstadoTurno estado)
        {
            // Mismo texto (con tildes) que el resto de las pantallas.
            e.Value = PaletaEstados.Texto(estado);
            e.FormattingApplied = true;
        }
        else if (columna == "colPerfil" && !item.EsRecurrente && item.Estado != EstadoTurno.Cancelado)
        {
            // Los pacientes nuevos se ven de un vistazo.
            e.CellStyle!.ForeColor = Color.FromArgb(41, 128, 185);
            e.CellStyle.Font = new Font(DgvTurnos.Font, FontStyle.Bold);
        }
    }

    private void DgvTurnos_SelectionChanged(object? sender, EventArgs e)
    {
        if (DgvTurnos.CurrentRow?.DataBoundItem is not TurnoAgendaItem item)
        {
            _turnoSeleccionado = null;
            BtnAtender.Enabled = false;
            BtnHistorialRapido.Enabled = false;
            return;
        }

        _turnoSeleccionado = item.Turno;
        bool cancelado = _turnoSeleccionado.Estado == EstadoTurno.Cancelado;
        bool yaAtendido = _turnoSeleccionado.ActividadMedica?.Activo == true;
        bool fechaFutura = _turnoSeleccionado.Fecha > DateOnly.FromDateTime(DateTime.Today);

        BtnAtender.Enabled = !cancelado && !fechaFutura;
        BtnAtender.Text = yaAtendido ? "Ver / Modificar Atención" : "Atender Paciente";
        BtnHistorialRapido.Enabled = _turnoSeleccionado.Paciente != null;

        if (cancelado)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = $"Turno cancelado para {_turnoSeleccionado.PacienteNombre}.";
        }
        else if (fechaFutura)
        {
            LblMensaje.ForeColor = Color.FromArgb(180, 120, 20);
            LblMensaje.Text = "Ese turno todavía no llegó. Puede consultar el historial, pero no cargar la atención.";
        }
        else if (yaAtendido)
        {
            LblMensaje.ForeColor = Color.FromArgb(39, 174, 96);
            LblMensaje.Text = $"Paciente atendido: {_turnoSeleccionado.PacienteNombre} ({_turnoSeleccionado.HorarioRango}).";
        }
        else
        {
            LblMensaje.ForeColor = Color.FromArgb(41, 128, 185);
            LblMensaje.Text = $"Paciente listo para atender: {_turnoSeleccionado.PacienteNombre} ({_turnoSeleccionado.HorarioRango}).";
        }
    }

    private void BtnAtender_Click(object? sender, EventArgs e)
    {
        if (_turnoSeleccionado == null)
        {
            MessageBox.Show("Seleccione un turno de la lista para registrar la atención.",
                "Seleccionar turno", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_turnoSeleccionado.Estado == EstadoTurno.Cancelado)
        {
            MessageBox.Show("No se puede registrar atención médica para un turno cancelado.",
                "Turno cancelado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_turnoSeleccionado.Fecha > DateOnly.FromDateTime(DateTime.Today))
        {
            MessageBox.Show("No se puede cargar la atención de un turno futuro.",
                "Fecha no alcanzada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Si tenemos el navegador principal, abrimos la pantalla de Registro de Actividad integrada
        if (_navegador != null)
        {
            _navegador(new FormRegistrarActividad(_turnoSeleccionado, _usuarioActivo, _navegador));
        }
        else
        {
            var formActividad = new FormRegistrarActividad(_turnoSeleccionado, _usuarioActivo);
            if (formActividad.ShowDialog() == DialogResult.OK)
            {
                CargarTurnos();
            }
        }
    }

    private void BtnHistorialRapido_Click(object? sender, EventArgs e)
    {
        if (_turnoSeleccionado?.Paciente == null)
        {
            MessageBox.Show("Seleccione un turno con un paciente válido.",
                "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_navegador != null)
        {
            _navegador(new FormHistorialClinico(_usuarioActivo, _turnoSeleccionado.Paciente, _navegador));
        }
        else
        {
            var formHistorial = new FormHistorialPaciente(_turnoSeleccionado.Paciente, _usuarioActivo);
            formHistorial.ShowDialog();
        }
    }

    private void BtnFiltroTotal_Click(object sender, EventArgs e)
    {

    }
}
