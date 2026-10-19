using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormHistorialClinico : Form
{
    private static readonly Color ColorResumen = Color.FromArgb(70, 90, 120);
    private static readonly Color ColorError = Color.FromArgb(192, 57, 43);

    private readonly Usuario? _usuarioActivo;
    private readonly Paciente? _pacienteInicial;
    private readonly Action<Form>? _navegador;
    private readonly PacienteService _pacienteService = new();
    private readonly MedicoService _medicoService = new();
    private readonly ActividadMedicaService _actividadService = new();

    // Filtro "hasta" del rango de fechas: se crea en codigo (no en el
    // Designer) para que el disenador visual no lo borre.
    private readonly DateTimePicker DtpFechaHasta = new();
    private readonly Label lblFechaHasta = new();

    private Paciente? _pacienteSeleccionado;

    // RNF#07: los datos clinicos (motivo, procedimiento, receta) solo los ve el rol Medico.
    // Sin usuario o con otro rol se oculta todo lo clinico (falla cerrado).
    private bool PuedeVerClinico => _usuarioActivo?.Rol == RolUsuario.Medico;

    public FormHistorialClinico(Usuario? usuarioActivo = null, Paciente? pacienteInicial = null, Action<Form>? navegador = null)
    {
        InitializeComponent();
        _usuarioActivo = usuarioActivo;
        _pacienteInicial = pacienteInicial;
        _navegador = navegador;

        ConfigurarGrilla();
        ConfigurarFiltroFechas();
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
        ChkFiltrarFecha.CheckedChanged += (s, e) => { ActualizarEstadoFechas(); CargarHistorialPaciente(); };
        DtpFechaFiltro.ValueChanged += (s, e) => { if (ChkFiltrarFecha.Checked) CargarHistorialPaciente(); };
        DtpFechaHasta.ValueChanged += (s, e) => { if (ChkFiltrarFecha.Checked) CargarHistorialPaciente(); };
        DgvHistorial.SelectionChanged += DgvHistorial_SelectionChanged;
        BtnNuevaConsulta.Click += BtnNuevaConsulta_Click;

        AcceptButton = BtnBuscar;

        // Antes de que el formulario se muestre, asignar SelectedValue en el
        // combo no tiene efecto y queda el primer paciente; al cargar se
        // reaplica el paciente recibido (si lo hubo) para no mostrar el historial equivocado.
        Load += (s, e) =>
        {
            if (_pacienteInicial != null && CboPacientes.SelectedValue is int actual && actual != _pacienteInicial.Id
                && CboPacientes.Items.Cast<Paciente>().Any(p => p.Id == _pacienteInicial.Id))
            {
                CboPacientes.SelectedValue = _pacienteInicial.Id;
            }
        };
    }

    private void ConfigurarGrilla()
    {
        DgvHistorial.EnableHeadersVisualStyles = false;
        DgvHistorial.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
        DgvHistorial.ColumnHeadersHeight = EscalaDpi.Px(this, 40);
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

        // Las columnas clinicas solo existen para el rol Medico (RNF#07).
        HistorialGrilla.AgregarColumnas(DgvHistorial, PuedeVerClinico);

        if (!PuedeVerClinico)
        {
            lblDetalleTitulo.Text = "Información clínica reservada al rol Médico.";
            lblMotivo.Visible = lblDiagnostico.Visible = lblReceta.Visible = false;
            TxtMotivo.Visible = TxtDiagnostico.Visible = TxtReceta.Visible = false;
        }
    }

    // Rango de fechas: el checkbox y el selector existentes pasan a ser
    // "Filtrar desde"; se agrega el selector "hasta" a su derecha.
    private void ConfigurarFiltroFechas()
    {
        ChkFiltrarFecha.Text = "Filtrar desde";
        ChkFiltrarFecha.Location = new Point(Px(390), Px(92));
        DtpFechaFiltro.Location = new Point(Px(510), Px(90));
        DtpFechaFiltro.Size = new Size(Px(120), DtpFechaFiltro.Height);

        lblFechaHasta.AutoSize = true;
        lblFechaHasta.Text = "hasta";
        lblFechaHasta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblFechaHasta.ForeColor = Color.FromArgb(27, 42, 74);
        lblFechaHasta.Location = new Point(Px(640), Px(94));

        DtpFechaHasta.Format = DateTimePickerFormat.Short;
        DtpFechaHasta.Location = new Point(Px(690), Px(90));
        DtpFechaHasta.Size = new Size(Px(120), DtpFechaFiltro.Height);

        DtpFechaFiltro.Value = DateTime.Today.AddMonths(-1);
        DtpFechaHasta.Value = DateTime.Today;

        pnlFiltro.Controls.Add(lblFechaHasta);
        pnlFiltro.Controls.Add(DtpFechaHasta);
        ActualizarEstadoFechas();
    }

    private int Px(int valor) => EscalaDpi.Px(this, valor);

    private void ActualizarEstadoFechas()
    {
        DtpFechaFiltro.Enabled = ChkFiltrarFecha.Checked;
        DtpFechaHasta.Enabled = ChkFiltrarFecha.Checked;
        lblFechaHasta.Enabled = ChkFiltrarFecha.Checked;
    }

    private void CargarTiposFiltro()
    {
        List<TipoActividad> tipos;
        try
        {
            tipos = _actividadService.ObtenerTiposActividad();
        }
        catch (Exception ex)
        {
            tipos = new List<TipoActividad>();
            MostrarErrorCarga("No se pudo cargar la lista de tipos de atención: " + ex.Message);
        }
        tipos.Insert(0, new TipoActividad { Id = 0, NombreTipo = "Todos los tipos" });

        CboTipoFiltro.DataSource = tipos;
        CboTipoFiltro.DisplayMember = "NombreTipo";
        CboTipoFiltro.ValueMember = "Id";
        CboTipoFiltro.SelectedIndex = 0;
    }

    private void CargarMedicosFiltro()
    {
        List<Medico> medicos;
        try
        {
            medicos = _medicoService.ObtenerTodos();
        }
        catch (Exception ex)
        {
            medicos = new List<Medico>();
            MostrarErrorCarga("No se pudo cargar la lista de médicos: " + ex.Message);
        }
        medicos.Insert(0, new Medico { Id = 0, Apellido = "Todos", Nombre = "los médicos", Especialidad = "-" });
        CboMedicoFiltro.DataSource = medicos;
        CboMedicoFiltro.DisplayMember = "NombreCompleto";
        CboMedicoFiltro.ValueMember = "Id";
        CboMedicoFiltro.SelectedIndex = 0;
    }

    private void MostrarErrorCarga(string mensaje)
    {
        lblResumenHistorial.ForeColor = ColorError;
        lblResumenHistorial.Text = mensaje;
    }

    private void CargarPacientes(string? filtro = null)
    {
        var idAConservar = _pacienteSeleccionado?.Id ?? _pacienteInicial?.Id;
        List<Paciente> pacientes;
        var falloCarga = false;
        try
        {
            pacientes = _pacienteService.ObtenerTodos();
        }
        catch (Exception ex)
        {
            pacientes = new List<Paciente>();
            falloCarga = true;
            MostrarErrorCarga("No se pudo cargar la lista de pacientes: " + ex.Message);
        }
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
            lblPacienteDetalle.Text = "No se encontraron pacientes con ese criterio de búsqueda.";
            if (!falloCarga)
            {
                lblResumenHistorial.ForeColor = ColorResumen;
                lblResumenHistorial.Text = "Pruebe con otro nombre, apellido o DNI, o pulse Limpiar.";
            }
            DesenlazarGrilla();
            lblHistorialTitulo.Text = HistorialGrilla.Contador(0);
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
        DtpFechaFiltro.Value = DateTime.Today.AddMonths(-1);
        DtpFechaHasta.Value = DateTime.Today;

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
            lblResumenHistorial.ForeColor = ColorResumen;
            lblResumenHistorial.Text = "";
            DesenlazarGrilla();
            lblHistorialTitulo.Text = "Registro cronológico de atenciones";
            LimpiarDetalle("Seleccione un paciente para ver el detalle.");
            ActualizarBotonNuevaConsulta();
            return;
        }

        _pacienteSeleccionado = (Paciente)CboPacientes.SelectedItem;
        CargarHistorialPaciente();
        ActualizarBotonNuevaConsulta();
    }

    private void DesenlazarGrilla()
    {
        DgvHistorial.SelectionChanged -= DgvHistorial_SelectionChanged;
        DgvHistorial.DataSource = null;
        DgvHistorial.SelectionChanged += DgvHistorial_SelectionChanged;
    }

    private int? ObtenerMedicoFiltroId()
    {
        return CboMedicoFiltro.SelectedItem is Medico medico && medico.Id > 0 ? medico.Id : null;
    }

    private void CargarHistorialPaciente()
    {
        if (_pacienteSeleccionado == null) return;

        var tipoId = ObtenerTipoFiltroId();
        var medicoId = ObtenerMedicoFiltroId();
        var hayFiltros = tipoId > 0 || medicoId.HasValue || ChkFiltrarFecha.Checked;

        DateOnly? desde = null;
        DateOnly? hasta = null;
        if (ChkFiltrarFecha.Checked)
        {
            var error = HistorialGrilla.ValidarRango(DtpFechaFiltro.Value, DtpFechaHasta.Value);
            if (error != null)
            {
                // Rango invalido: no se consulta; se avisa y se vacia la grilla.
                MostrarFichaPaciente(new List<AtencionFila>());
                DesenlazarGrilla();
                lblResumenHistorial.ForeColor = ColorError;
                lblResumenHistorial.Text = error;
                lblHistorialTitulo.Text = HistorialGrilla.Contador(0);
                LimpiarDetalle("Corrija el rango de fechas para ver el detalle.");
                return;
            }

            desde = DateOnly.FromDateTime(DtpFechaFiltro.Value);
            hasta = DateOnly.FromDateTime(DtpFechaHasta.Value);
        }

        List<AtencionFila> historial;
        try
        {
            historial = _actividadService.ConsultarFilas(
                _usuarioActivo?.Rol,
                _pacienteSeleccionado.Id,
                medicoId,
                desde,
                hasta,
                tipoId > 0 ? tipoId : null);
        }
        catch (Exception ex)
        {
            DesenlazarGrilla();
            lblResumenHistorial.ForeColor = ColorError;
            lblResumenHistorial.Text = "No se pudo cargar el historial del paciente: " + ex.Message;
            lblHistorialTitulo.Text = HistorialGrilla.Contador(0);
            LimpiarDetalle("No se pudo cargar el detalle.");
            return;
        }

        DgvHistorial.SelectionChanged -= DgvHistorial_SelectionChanged;
        DgvHistorial.DataSource = historial;
        DgvHistorial.ClearSelection();
        DgvHistorial.SelectionChanged += DgvHistorial_SelectionChanged;

        MostrarFichaPaciente(historial);

        if (historial.Count == 0)
        {
            lblHistorialTitulo.Text = HistorialGrilla.Contador(0) + ". " + HistorialGrilla.MensajeVacio(hayFiltros);
            LimpiarDetalle("Sin registros para mostrar.");
            return;
        }

        lblHistorialTitulo.Text = $"Registro cronológico de atenciones: {HistorialGrilla.Contador(historial.Count)}";
        DgvHistorial.Rows[0].Selected = true;
        if (DgvHistorial.Rows.Count > 0)
        {
            DgvHistorial.CurrentCell = DgvHistorial.Rows[0].Cells[0];
        }
    }

    private void MostrarFichaPaciente(List<AtencionFila> historial)
    {
        if (_pacienteSeleccionado == null) return;

        var edad = CalcularEdad(_pacienteSeleccionado.FechaNacimiento);
        var obraSocial = string.IsNullOrWhiteSpace(_pacienteSeleccionado.ObraSocial?.Nombre)
            ? "Particular"
            : _pacienteSeleccionado.ObraSocial?.Nombre;

        lblPacienteDetalle.Text =
            $"{_pacienteSeleccionado.NombreCompleto}  |  DNI {_pacienteSeleccionado.Dni}  |  {edad} años  |  {obraSocial}  |  {_pacienteSeleccionado.Telefono}";

        lblResumenHistorial.ForeColor = ColorResumen;
        if (historial.Count == 0)
        {
            lblResumenHistorial.Text = "Sin atenciones en el historial con los filtros actuales.";
            return;
        }

        var ultima = historial[0];
        lblResumenHistorial.Text = $"{HistorialGrilla.Contador(historial.Count)}  |  Última: {ultima.FechaStr} - {ultima.TipoActividadNombre}";
    }

    private void DgvHistorial_SelectionChanged(object? sender, EventArgs e)
    {
        if (DgvHistorial.CurrentRow?.DataBoundItem is not AtencionFila actividad)
        {
            return;
        }

        // RNF#07: solo el rol Medico ve el detalle clinico.
        if (!PuedeVerClinico || !actividad.IncluyeDatosClinicos)
        {
            lblDetalleTitulo.Text = "Información clínica reservada al rol Médico.";
            TxtMotivo.Clear();
            TxtDiagnostico.Clear();
            TxtReceta.Clear();
            return;
        }

        lblDetalleTitulo.Text = $"{actividad.FechaStr}  {actividad.HorarioStr}  |  {actividad.MedicoNombre}  |  {actividad.TipoActividadNombre}";
        TxtMotivo.Text = TextoOVacio(actividad.MotivoConsulta, "Sin motivo de consulta registrado.");
        TxtDiagnostico.Text = TextoOVacio(actividad.Procedimiento, "Sin observaciones de procedimiento o diagnóstico.");
        TxtReceta.Text = TextoOVacio(actividad.RecetaMedicamentos, "Sin recetas o prescripciones.");
    }

    private void LimpiarDetalle(string titulo)
    {
        lblDetalleTitulo.Text = PuedeVerClinico ? titulo : "Información clínica reservada al rol Médico.";
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
                "Seleccione un paciente antes de registrar una nueva atención.",
                "Historial clínico",
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
