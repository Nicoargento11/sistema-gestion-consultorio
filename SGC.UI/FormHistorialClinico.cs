using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormHistorialClinico : Form
{
    private readonly Usuario? _usuarioActivo;
    private readonly Paciente? _pacienteInicial;
    private readonly Action<Form>? _navegador;
    private readonly PacienteService _pacienteService = new();
    private readonly ActividadMedicaService _actividadService = new();

    private Paciente? _pacienteSeleccionado;

    public FormHistorialClinico(Usuario? usuarioActivo = null, Paciente? pacienteInicial = null, Action<Form>? navegador = null)
    {
        InitializeComponent();
        _usuarioActivo = usuarioActivo;
        _pacienteInicial = pacienteInicial;
        _navegador = navegador;

        ConfigurarColumnas();
        CargarPacientes();

        BtnBuscar.Click += (s, e) => FiltrarPacientes();
        TxtBuscar.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { FiltrarPacientes(); e.SuppressKeyPress = true; } };
        CboPacientes.SelectedIndexChanged += CboPacientes_SelectedIndexChanged;
        DgvHistorial.SelectionChanged += DgvHistorial_SelectionChanged;
        BtnNuevaConsulta.Click += BtnNuevaConsulta_Click;

        if (_pacienteInicial != null)
        {
            CboPacientes.SelectedValue = _pacienteInicial.Id;
        }
    }

    private void ConfigurarColumnas()
    {
        DgvHistorial.AutoGenerateColumns = false;
        DgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFecha", HeaderText = "Fecha", DataPropertyName = "FechaStr", FillWeight = 12 });
        DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHorario", HeaderText = "Horario", DataPropertyName = "HorarioStr", FillWeight = 14 });
        DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedico", HeaderText = "Medico Tratante", DataPropertyName = "MedicoNombre", FillWeight = 24 });
        DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTipo", HeaderText = "Tipo Atencion", DataPropertyName = "TipoActividadNombre", FillWeight = 20 });
        DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMotivo", HeaderText = "Motivo de Consulta", DataPropertyName = "MotivoConsulta", FillWeight = 30 });
    }

    private void CargarPacientes(string? filtro = null)
    {
        var pacientes = _pacienteService.ObtenerTodos();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            var f = filtro.Trim().ToLower();
            pacientes = pacientes.Where(p =>
                p.NombreCompleto.ToLower().Contains(f) ||
                p.Dni.Contains(f) ||
                p.Apellido.ToLower().Contains(f) ||
                p.Nombre.ToLower().Contains(f)).ToList();
        }

        CboPacientes.SelectedIndexChanged -= CboPacientes_SelectedIndexChanged;
        CboPacientes.DataSource = pacientes;
        CboPacientes.DisplayMember = "NombreCompleto";
        CboPacientes.ValueMember = "Id";
        CboPacientes.SelectedIndexChanged += CboPacientes_SelectedIndexChanged;

        if (pacientes.Count > 0)
        {
            CboPacientes.SelectedIndex = 0;
            CboPacientes_SelectedIndexChanged(this, EventArgs.Empty);
        }
        else
        {
            _pacienteSeleccionado = null;
            lblPacienteDetalle.Text = "No se encontraron pacientes con ese criterio de busqueda.";
            DgvHistorial.DataSource = null;
            LimpiarDetalle();
        }
    }

    private void FiltrarPacientes()
    {
        CargarPacientes(TxtBuscar.Text);
    }

    private void CboPacientes_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (CboPacientes.SelectedItem == null)
        {
            _pacienteSeleccionado = null;
            lblPacienteDetalle.Text = "Seleccione un paciente para consultar su historial clinico.";
            DgvHistorial.DataSource = null;
            LimpiarDetalle();
            return;
        }

        _pacienteSeleccionado = (Paciente)CboPacientes.SelectedItem;
        lblPacienteDetalle.Text = $"Paciente: {_pacienteSeleccionado.NombreCompleto} | DNI: {_pacienteSeleccionado.Dni} | Email: {_pacienteSeleccionado.Email} | Telefono: {_pacienteSeleccionado.Telefono}";

        CargarHistorialPaciente();
    }

    private void CargarHistorialPaciente()
    {
        if (_pacienteSeleccionado == null) return;

        var historial = _actividadService.ObtenerHistorialPorPaciente(_pacienteSeleccionado.Id);

        DgvHistorial.SelectionChanged -= DgvHistorial_SelectionChanged;
        DgvHistorial.DataSource = historial;
        DgvHistorial.ClearSelection();
        LimpiarDetalle();
        DgvHistorial.SelectionChanged += DgvHistorial_SelectionChanged;

        lblHistorialTitulo.Text = $"Registro Cronologico de Atenciones ({historial.Count} registro(s) encontrado(s))";

        if (historial.Count > 0)
        {
            DgvHistorial.Rows[0].Selected = true;
        }
        else
        {
            lblDetalleTitulo.Text = "El paciente no posee atenciones clinicas previas registradas.";
        }
    }

    private void DgvHistorial_SelectionChanged(object? sender, EventArgs e)
    {
        if (DgvHistorial.CurrentRow == null)
        {
            LimpiarDetalle();
            return;
        }

        var actividad = (ActividadMedica)DgvHistorial.CurrentRow.DataBoundItem;
        lblDetalleTitulo.Text = $"Consulta del dia {actividad.FechaStr} ({actividad.HorarioStr}) - Medico: {actividad.MedicoNombre}";
        TxtMotivo.Text = actividad.MotivoConsulta;
        TxtDiagnostico.Text = string.IsNullOrWhiteSpace(actividad.Procedimiento) ? "Sin observaciones de procedimiento o diagnostico registradas." : actividad.Procedimiento;
        TxtReceta.Text = string.IsNullOrWhiteSpace(actividad.RecetaMedicamentos) ? "Sin recetas o prescripciones farmacologicas." : actividad.RecetaMedicamentos;
    }

    private void LimpiarDetalle()
    {
        TxtMotivo.Clear();
        TxtDiagnostico.Clear();
        TxtReceta.Clear();
    }

    private void BtnNuevaConsulta_Click(object? sender, EventArgs e)
    {
        if (_navegador != null)
        {
            _navegador(new FormRegistrarActividad(null, _usuarioActivo, _navegador));
        }
        else
        {
            var formActividad = new FormRegistrarActividad(null, _usuarioActivo);
            if (formActividad.ShowDialog() == DialogResult.OK)
            {
                CargarHistorialPaciente();
            }
        }
    }
}