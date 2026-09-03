using SGC.Entidades;
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
    private readonly TurnoService _turnoService = new();
    private readonly ActividadMedicaService _actividadService = new();

    private Medico? _medicoActual;
    private Turno? _turnoSeleccionado;
    private FiltroTurno _filtroActual = FiltroTurno.Todos;

    public FormAgendaMedico(Usuario? usuarioActivo = null, Action<Form>? navegador = null)
    {
        InitializeComponent();
        _usuarioActivo = usuarioActivo;
        _navegador = navegador;

        DeterminarMedicoActivo();
        ConfigurarColumnas();

        DtpFecha.Value = DateTime.Today;
        DtpFecha.ValueChanged += (s, e) => CargarTurnos();
        BtnHoy.Click += (s, e) => DtpFecha.Value = DateTime.Today;

        BtnFiltroTotal.Click += (s, e) => { _filtroActual = FiltroTurno.Todos; CargarTurnos(); };
        BtnFiltroPendientes.Click += (s, e) => { _filtroActual = FiltroTurno.Pendientes; CargarTurnos(); };
        BtnFiltroAtendidos.Click += (s, e) => { _filtroActual = FiltroTurno.Atendidos; CargarTurnos(); };
        BtnFiltroCancelados.Click += (s, e) => { _filtroActual = FiltroTurno.Cancelados; CargarTurnos(); };

        DgvTurnos.SelectionChanged += DgvTurnos_SelectionChanged;
        DgvTurnos.CellDoubleClick += (s, e) => BtnAtender_Click(s, e);
        BtnAtender.Click += BtnAtender_Click;
        BtnHistorialRapido.Click += BtnHistorialRapido_Click;

        CargarTurnos();
    }

    private void DeterminarMedicoActivo()
    {
        if (_usuarioActivo?.MedicoId != null)
        {
            _medicoActual = _medicoService.ObtenerPorId(_usuarioActivo.MedicoId.Value);
        }

        if (_medicoActual == null)
        {
            _medicoActual = _medicoService.ObtenerTodos().FirstOrDefault();
        }

        if (_medicoActual != null)
        {
            lblMedicoInfo.Text = $"Profesional: {_medicoActual.NombreCompleto} | Matricula: {_medicoActual.Matricula}";
        }
        else
        {
            lblMedicoInfo.Text = "Profesional: No se encontro perfil medico asociado.";
        }
    }

    private void ConfigurarColumnas()
    {
        DgvTurnos.AutoGenerateColumns = false;
        DgvTurnos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHorario", HeaderText = "Horario", DataPropertyName = "HorarioRango", FillWeight = 16 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPaciente", HeaderText = "Paciente", DataPropertyName = "PacienteNombre", FillWeight = 32 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDni", HeaderText = "DNI Paciente", DataPropertyName = "PacienteDni", FillWeight = 16 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstadoAtencion", HeaderText = "Atencion Clinica", DataPropertyName = "EstadoAtencion", FillWeight = 18 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstado", HeaderText = "Estado Turno", DataPropertyName = "Estado", FillWeight = 18 });
    }

    private void ActualizarEstiloBotonesFiltro()
    {
        // Resaltamos con borde grueso y color al boton activo
        BtnFiltroTotal.FlatAppearance.BorderSize = _filtroActual == FiltroTurno.Todos ? 3 : 1;
        BtnFiltroPendientes.FlatAppearance.BorderSize = _filtroActual == FiltroTurno.Pendientes ? 3 : 1;
        BtnFiltroAtendidos.FlatAppearance.BorderSize = _filtroActual == FiltroTurno.Atendidos ? 3 : 1;
        BtnFiltroCancelados.FlatAppearance.BorderSize = _filtroActual == FiltroTurno.Cancelados ? 3 : 1;
    }

    private void CargarTurnos()
    {
        if (_medicoActual == null) return;

        ActualizarEstiloBotonesFiltro();

        var fechaSeleccionada = DateOnly.FromDateTime(DtpFecha.Value);
        // Siempre traemos todos los turnos del dia para calcular los totales reales
        var todosDelDia = _turnoService.ObtenerPorMedicoYFecha(_medicoActual.Id, fechaSeleccionada, true);

        int total = todosDelDia.Count;
        int cancelados = todosDelDia.Count(t => t.Estado == EstadoTurno.Cancelado);
        int atendidos = todosDelDia.Count(t => t.ActividadMedica?.Activo == true);
        int pendientes = todosDelDia.Count(t => t.Estado != EstadoTurno.Cancelado && (t.ActividadMedica == null || !t.ActividadMedica.Activo));

        BtnFiltroTotal.Text = $"TOTAL TURNOS ({total})";
        BtnFiltroPendientes.Text = $"PENDIENTES ({pendientes})";
        BtnFiltroAtendidos.Text = $"ATENDIDOS ({atendidos})";
        BtnFiltroCancelados.Text = $"CANCELADOS ({cancelados})";

        // Filtramos segun el boton presionado
        IEnumerable<Turno> filtrados = todosDelDia;
        switch (_filtroActual)
        {
            case FiltroTurno.Pendientes:
                filtrados = todosDelDia.Where(t => t.Estado != EstadoTurno.Cancelado && (t.ActividadMedica == null || !t.ActividadMedica.Activo));
                lblGrillaTitulo.Text = $"Turnos Pendientes de Atencion ({pendientes})";
                break;
            case FiltroTurno.Atendidos:
                filtrados = todosDelDia.Where(t => t.ActividadMedica?.Activo == true);
                lblGrillaTitulo.Text = $"Pacientes Ya Atendidos ({atendidos})";
                break;
            case FiltroTurno.Cancelados:
                filtrados = todosDelDia.Where(t => t.Estado == EstadoTurno.Cancelado);
                lblGrillaTitulo.Text = $"Turnos Cancelados ({cancelados})";
                break;
            default:
                filtrados = todosDelDia;
                lblGrillaTitulo.Text = $"Lista Completa de Turnos del Dia ({total})";
                break;
        }

        var listaVisible = filtrados.ToList();

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
            else if (listaVisible[i].ActividadMedica?.Activo == true)
            {
                DgvTurnos.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(235, 250, 240);
                DgvTurnos.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(30, 100, 50);
            }
        }

        DgvTurnos.ClearSelection();
        _turnoSeleccionado = null;
        BtnAtender.Enabled = false;
        BtnHistorialRapido.Enabled = false;
        LblMensaje.Text = "Haga clic en una tarjeta arriba para filtrar, o seleccione un paciente de la lista.";
        LblMensaje.ForeColor = Color.FromArgb(100, 110, 120);

        DgvTurnos.SelectionChanged += DgvTurnos_SelectionChanged;

        if (listaVisible.Count > 0)
        {
            DgvTurnos.Rows[0].Selected = true;
        }
    }

    private void DgvTurnos_SelectionChanged(object? sender, EventArgs e)
    {
        if (DgvTurnos.CurrentRow == null)
        {
            _turnoSeleccionado = null;
            BtnAtender.Enabled = false;
            BtnHistorialRapido.Enabled = false;
            return;
        }

        _turnoSeleccionado = (Turno)DgvTurnos.CurrentRow.DataBoundItem;
        bool cancelado = _turnoSeleccionado.Estado == EstadoTurno.Cancelado;
        bool yaAtendido = _turnoSeleccionado.ActividadMedica?.Activo == true;

        BtnAtender.Enabled = !cancelado;
        BtnAtender.Text = yaAtendido ? "Ver / Modificar Atencion" : "Atender Paciente";
        BtnHistorialRapido.Enabled = _turnoSeleccionado.Paciente != null;

        if (cancelado)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = $"Turno cancelado para {_turnoSeleccionado.PacienteNombre}.";
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
            MessageBox.Show("Seleccione un turno de la lista para registrar la atencion.",
                "Seleccionar turno", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_turnoSeleccionado.Estado == EstadoTurno.Cancelado)
        {
            MessageBox.Show("No se puede registrar atencion medica para un turno cancelado.",
                "Turno cancelado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            MessageBox.Show("Seleccione un turno con paciente valido.",
                "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_navegador != null)
        {
            _navegador(new FormHistorialClinico(_usuarioActivo, _turnoSeleccionado.Paciente, _navegador));
        }
        else
        {
            var formHistorial = new FormHistorialPaciente(_turnoSeleccionado.Paciente);
            formHistorial.ShowDialog();
        }
    }
}