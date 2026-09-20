using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormHistorialClinico : Form
{
    private readonly Usuario? _usuarioActivo;
    private readonly Paciente? _pacienteInicial;
    private readonly Action<Form>? _navegador;
    private readonly PacienteService _pacienteService = new();
    private readonly MedicoService _medicoService = new();
    private readonly ActividadMedicaService _actividadService = new();

    private Paciente? _pacienteSeleccionado;

    public FormHistorialClinico(Usuario? usuarioActivo = null, Paciente? pacienteInicial = null, Action<Form>? navegador = null)
    {
        InitializeComponent();
        _usuarioActivo = usuarioActivo;
        _pacienteInicial = pacienteInicial;
        _navegador = navegador;

        ConfigurarGrilla();
        CargarTiposFiltro();
        CargarMedicosFiltro();
        CargarPacientes();

        BtnBuscar.Click += (s, e) => FiltrarPacientes();
        BtnLimpiar.Click += (s, e) => LimpiarFiltros();
        TxtBuscar.KeyDown += TxtBuscar_KeyDown;
        CboPacientes.Format += CboPacientes_Format;
        CboPacientes.SelectedIndexChanged += CboPacientes_SelectedIndexChanged;
        CboTipoFiltro.SelectedIndexChanged += (s, e) => CargarHistorialPaciente();
        CboMedicoFiltro.SelectedIndexChanged += (s, e) => CargarHistorialPaciente();
        ChkFiltrarFecha.CheckedChanged += (s, e) => CargarHistorialPaciente();
        DtpFechaFiltro.ValueChanged += (s, e) => { if (ChkFiltrarFecha.Checked) CargarHistorialPaciente(); };
        DgvHistorial.SelectionChanged += DgvHistorial_SelectionChanged;
        BtnNuevaConsulta.Click += BtnNuevaConsulta_Click;

        AcceptButton = BtnBuscar;
    }

    private void ConfigurarGrilla()
    {
        DgvHistorial.AutoGenerateColumns = false;
        DgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvHistorial.EnableHeadersVisualStyles = false;
        DgvHistorial.ColumnHeadersHeight = 34;
        DgvHistorial.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(27, 42, 74);
        DgvHistorial.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        DgvHistorial.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        DgvHistorial.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        DgvHistorial.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 0, 0);
        DgvHistorial.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
        DgvHistorial.DefaultCellStyle.Padding = new Padding(6, 2, 4, 2);
        DgvHistorial.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 134, 222);
        DgvHistorial.DefaultCellStyle.SelectionForeColor = Color.White;
        DgvHistorial.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 252);
        DgvHistorial.RowTemplate.Height = 30;
        DgvHistorial.GridColor = Color.FromArgb(225, 230, 238);

        DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFecha", HeaderText = "Fecha", DataPropertyName = "FechaStr", FillWeight = 12 });
        DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHorario", HeaderText = "Horario", DataPropertyName = "HorarioStr", FillWeight = 14 });
        DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedico", HeaderText = "Medico tratante", DataPropertyName = "MedicoNombre", FillWeight = 24 });
        DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTipo", HeaderText = "Tipo de atencion", DataPropertyName = "TipoActividadNombre", FillWeight = 20 });
        DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMotivo", HeaderText = "Motivo de consulta", DataPropertyName = "MotivoConsulta", FillWeight = 30 });
    }

    private void CargarTiposFiltro()
    {
        var tipos = _actividadService.ObtenerTiposActividad();
        tipos.Insert(0, new TipoActividad { Id = 0, NombreTipo = "Todos los tipos" });

        CboTipoFiltro.DataSource = tipos;
        CboTipoFiltro.DisplayMember = "NombreTipo";
        CboTipoFiltro.ValueMember = "Id";
        CboTipoFiltro.SelectedIndex = 0;
    }

    private void CargarMedicosFiltro()
    {
        var medicos = _medicoService.ObtenerTodos();
        medicos.Insert(0, new Medico { Id = 0, Apellido = "Todos", Nombre = "los medicos", Especialidad = "-" });
        CboMedicoFiltro.DataSource = medicos;
        CboMedicoFiltro.DisplayMember = "NombreCompleto";
        CboMedicoFiltro.ValueMember = "Id";
        CboMedicoFiltro.SelectedIndex = 0;
    }

    private void CargarPacientes(string? filtro = null)
    {
        var idAConservar = _pacienteSeleccionado?.Id ?? _pacienteInicial?.Id;
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

        if (pacientes.Count == 0)
        {
            _pacienteSeleccionado = null;
            lblPacienteDetalle.Text = "No se encontraron pacientes con ese criterio de busqueda.";
            lblResumenHistorial.Text = "Pruebe con otro nombre, apellido o DNI, o pulse Limpiar.";
            DgvHistorial.DataSource = null;
            lblHistorialTitulo.Text = "Registro cronologico de atenciones (0)";
            LimpiarDetalle("Seleccione un paciente para ver el detalle.");
            ActualizarBotonNuevaConsulta();
            return;
        }

        if (idAConservar.HasValue && pacientes.Any(p => p.Id == idAConservar.Value))
        {
            CboPacientes.SelectedValue = idAConservar.Value;
        }
        else
        {
            CboPacientes.SelectedIndex = 0;
        }

        CboPacientes_SelectedIndexChanged(this, EventArgs.Empty);
    }

    private void FiltrarPacientes()
    {
        CargarPacientes(TxtBuscar.Text);
    }

    private void LimpiarFiltros()
    {
        TxtBuscar.Clear();
        if (CboTipoFiltro.Items.Count > 0)
        {
            CboTipoFiltro.SelectedIndex = 0;
        }
        if (CboMedicoFiltro.Items.Count > 0)
        {
            CboMedicoFiltro.SelectedIndex = 0;
        }
        ChkFiltrarFecha.Checked = false;

        CargarPacientes();
        TxtBuscar.Focus();
    }

    private void TxtBuscar_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        FiltrarPacientes();
        e.SuppressKeyPress = true;
    }

    private void CboPacientes_Format(object? sender, ListControlConvertEventArgs e)
    {
        if (e.ListItem is Paciente paciente)
        {
            e.Value = $"{paciente.NombreCompleto}  |  DNI {paciente.Dni}";
        }
    }

    private void CboPacientes_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (CboPacientes.SelectedItem == null)
        {
            _pacienteSeleccionado = null;
            lblPacienteDetalle.Text = "Seleccione un paciente para consultar su historial.";
            lblResumenHistorial.Text = "";
            DgvHistorial.DataSource = null;
            lblHistorialTitulo.Text = "Registro cronologico de atenciones";
            LimpiarDetalle("Seleccione un paciente para ver el detalle.");
            ActualizarBotonNuevaConsulta();
            return;
        }

        _pacienteSeleccionado = (Paciente)CboPacientes.SelectedItem;
        CargarHistorialPaciente();
        ActualizarBotonNuevaConsulta();
    }

    private void CargarHistorialPaciente()
    {
        if (_pacienteSeleccionado == null) return;

        var tipoId = ObtenerTipoFiltroId();
        int? medicoId = null;
        if (CboMedicoFiltro.SelectedItem is Medico medico && medico.Id > 0)
            medicoId = medico.Id;

        DateOnly? fecha = ChkFiltrarFecha.Checked
            ? DateOnly.FromDateTime(DtpFechaFiltro.Value)
            : null;

        var historial = _actividadService.Consultar(
            _pacienteSeleccionado.Id,
            medicoId,
            fecha,
            tipoId > 0 ? tipoId : null);

        DgvHistorial.SelectionChanged -= DgvHistorial_SelectionChanged;
        DgvHistorial.DataSource = historial;
        DgvHistorial.ClearSelection();
        DgvHistorial.SelectionChanged += DgvHistorial_SelectionChanged;

        MostrarFichaPaciente(historial);

        if (historial.Count == 0)
        {
            lblHistorialTitulo.Text = tipoId > 0
                ? "No hay atenciones de ese tipo para este paciente."
                : "El paciente no posee atenciones clinicas registradas.";
            LimpiarDetalle("Sin registros para mostrar.");
            return;
        }

        lblHistorialTitulo.Text = $"Registro cronologico de atenciones ({historial.Count})";
        DgvHistorial.Rows[0].Selected = true;
        if (DgvHistorial.Rows.Count > 0)
        {
            DgvHistorial.CurrentCell = DgvHistorial.Rows[0].Cells[0];
        }
    }

    private void MostrarFichaPaciente(List<ActividadMedica> historial)
    {
        if (_pacienteSeleccionado == null) return;

        var edad = CalcularEdad(_pacienteSeleccionado.FechaNacimiento);
        var obraSocial = string.IsNullOrWhiteSpace(_pacienteSeleccionado.ObraSocial?.Nombre)

            ? "Particular"
            : _pacienteSeleccionado.ObraSocial?.Nombre;

        lblPacienteDetalle.Text =
            $"{_pacienteSeleccionado.NombreCompleto}  |  DNI {_pacienteSeleccionado.Dni}  |  {edad} anios  |  {obraSocial}  |  {_pacienteSeleccionado.Telefono}";

        if (historial.Count == 0)
        {
            lblResumenHistorial.Text = "Sin atenciones previas en el historial.";
            return;
        }

        var ultima = historial[0];
        lblResumenHistorial.Text = $"{historial.Count} atencion(es)  |  Ultima: {ultima.FechaStr} - {ultima.TipoActividadNombre}";
    }

    private void DgvHistorial_SelectionChanged(object? sender, EventArgs e)
    {
        if (DgvHistorial.CurrentRow?.DataBoundItem is not ActividadMedica actividad)
        {
            return;
        }

        lblDetalleTitulo.Text = $"{actividad.FechaStr}  {actividad.HorarioStr}  |  {actividad.MedicoNombre}  |  {actividad.TipoActividadNombre}";
        TxtMotivo.Text = TextoOVacio(actividad.MotivoConsulta, "Sin motivo de consulta registrado.");
        TxtDiagnostico.Text = TextoOVacio(actividad.Procedimiento, "Sin observaciones de procedimiento o diagnostico.");
        TxtReceta.Text = TextoOVacio(actividad.RecetaMedicamentos, "Sin recetas o prescripciones.");
    }

    private void LimpiarDetalle(string titulo)
    {
        lblDetalleTitulo.Text = titulo;
        TxtMotivo.Clear();
        TxtDiagnostico.Clear();
        TxtReceta.Clear();
    }

    private void ActualizarBotonNuevaConsulta()
    {
        BtnNuevaConsulta.Enabled = _pacienteSeleccionado != null;
    }

    private void BtnNuevaConsulta_Click(object? sender, EventArgs e)
    {
        if (_pacienteSeleccionado == null)
        {
            MessageBox.Show(
                "Seleccione un paciente antes de registrar una nueva atencion.",
                "Historial clinico",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        if (_navegador != null)
        {
            _navegador(new FormRegistrarActividad(null, _usuarioActivo, _navegador));
            return;
        }

        var formActividad = new FormRegistrarActividad(null, _usuarioActivo);
        if (formActividad.ShowDialog() == DialogResult.OK)
        {
            CargarHistorialPaciente();
        }
    }

    private int ObtenerTipoFiltroId()
    {
        if (CboTipoFiltro.SelectedValue is int id)
        {
            return id;
        }

        if (CboTipoFiltro.SelectedItem is TipoActividad tipo)
        {
            return tipo.Id;
        }

        return 0;
    }

    private static int CalcularEdad(DateOnly fechaNacimiento)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var edad = hoy.Year - fechaNacimiento.Year;
        if (fechaNacimiento > hoy.AddYears(-edad))
        {
            edad--;
        }

        return edad < 0 ? 0 : edad;
    }

    private static string TextoOVacio(string? valor, string placeholder)
    {
        return string.IsNullOrWhiteSpace(valor) ? placeholder : valor;
    }

    private void DtpFechaFiltro_ValueChanged(object sender, EventArgs e)
    {

    }
}
