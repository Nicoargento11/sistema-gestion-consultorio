using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormHistorialPaciente : Form
{
    private static readonly Color ColorResumen = Color.FromArgb(70, 90, 120);
    private static readonly Color ColorError = Color.FromArgb(192, 57, 43);

    private readonly Paciente _paciente;
    private readonly Usuario? _usuarioActivo;
    private readonly ActividadMedicaService _actividadService = new();
    private readonly MedicoService _medicoService = new();

    // Barra de filtros y contador: se crean en codigo (no en el Designer)
    // para que el disenador visual no los borre.
    private readonly Panel pnlFiltros = new();
    private readonly CheckBox ChkFiltrarFecha = new();
    private readonly DateTimePicker DtpDesde = new();
    private readonly DateTimePicker DtpHasta = new();
    private readonly Label lblHasta = new();
    private readonly ComboBox CboTipoFiltro = new();
    private readonly ComboBox CboMedicoFiltro = new();
    private readonly Label lblContador = new();
    private bool _cargandoFiltros;

    // RNF#07: los datos clinicos (motivo, procedimiento, receta) solo los ve el rol Medico.
    // Sin usuario o con otro rol se oculta todo lo clinico (falla cerrado).
    private bool PuedeVerClinico => _usuarioActivo?.Rol == RolUsuario.Medico;

    public FormHistorialPaciente(Paciente paciente, Usuario? usuarioActivo = null)
    {
        InitializeComponent();
        _paciente = paciente;
        _usuarioActivo = usuarioActivo;

        lblPacienteInfo.Text = $"Paciente: {_paciente.NombreCompleto} | DNI: {_paciente.Dni} | Tel: {_paciente.Telefono}";

        ConfigurarColumnas();
        ConfigurarFiltros();
        DgvHistorial.SelectionChanged += DgvHistorial_SelectionChanged;
        BtnCerrar.Click += (s, e) => Close();
        AcceptButton = BtnCerrar;

        CargarHistorial();

        // WinForms selecciona sola la primera fila recien cuando el control
        // se muestra de verdad en pantalla (no en el constructor) - limpiar
        // ahi mismo no sirve, hay que hacerlo en Load, que corre despues.
        Load += (s, e) => CargarHistorial();
    }

    private void ConfigurarColumnas()
    {
        // Las columnas clinicas solo existen para el rol Medico (RNF#07).
        HistorialGrilla.AgregarColumnas(DgvHistorial, PuedeVerClinico);

        if (!PuedeVerClinico)
        {
            lblDetalleTitulo.Text = "Información clínica reservada al rol Médico.";
            lblMotivo.Visible = lblDiagnostico.Visible = lblReceta.Visible = false;
            TxtMotivo.Visible = TxtDiagnostico.Visible = TxtReceta.Visible = false;
        }
    }

    // Filtros (rango de fechas, tipo de atencion y medico) y contador entre el
    // encabezado y la grilla; la grilla se corre hacia abajo para dejarles lugar.
    private void ConfigurarFiltros()
    {
        pnlFiltros.Location = new Point(Px(15), Px(82));
        pnlFiltros.Size = new Size(Px(820), Px(34));
        pnlFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        var fuenteEtiqueta = new Font("Segoe UI", 9F, FontStyle.Bold);
        var colorEtiqueta = Color.FromArgb(27, 42, 74);

        ChkFiltrarFecha.AutoSize = true;
        ChkFiltrarFecha.Text = "Filtrar desde";
        ChkFiltrarFecha.Location = new Point(Px(0), Px(7));

        DtpDesde.Format = DateTimePickerFormat.Short;
        DtpDesde.Location = new Point(Px(112), Px(4));
        DtpDesde.Size = new Size(Px(104), Px(27));
        DtpDesde.Value = DateTime.Today.AddMonths(-1);

        lblHasta.AutoSize = true;
        lblHasta.Text = "hasta";
        lblHasta.Font = fuenteEtiqueta;
        lblHasta.ForeColor = colorEtiqueta;
        lblHasta.Location = new Point(Px(222), Px(8));

        DtpHasta.Format = DateTimePickerFormat.Short;
        DtpHasta.Location = new Point(Px(266), Px(4));
        DtpHasta.Size = new Size(Px(104), Px(27));
        DtpHasta.Value = DateTime.Today;

        var lblTipo = new Label { AutoSize = true, Text = "Tipo:", Font = fuenteEtiqueta, ForeColor = colorEtiqueta, Location = new Point(Px(380), Px(8)) };
        CboTipoFiltro.DropDownStyle = ComboBoxStyle.DropDownList;
        CboTipoFiltro.Location = new Point(Px(422), Px(4));
        CboTipoFiltro.Size = new Size(Px(150), Px(27));

        var lblMedico = new Label { AutoSize = true, Text = "Médico:", Font = fuenteEtiqueta, ForeColor = colorEtiqueta, Location = new Point(Px(582), Px(8)) };
        CboMedicoFiltro.DropDownStyle = ComboBoxStyle.DropDownList;
        CboMedicoFiltro.Location = new Point(Px(646), Px(4));
        CboMedicoFiltro.Size = new Size(Px(174), Px(27));

        pnlFiltros.Controls.AddRange(new Control[]
        {
            ChkFiltrarFecha, DtpDesde, lblHasta, DtpHasta, lblTipo, CboTipoFiltro, lblMedico, CboMedicoFiltro
        });

        lblContador.Location = new Point(Px(15), Px(118));
        lblContador.Size = new Size(Px(820), Px(24));
        lblContador.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblContador.AutoEllipsis = true;
        lblContador.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblContador.ForeColor = colorEtiqueta;
        lblContador.TextAlign = ContentAlignment.MiddleLeft;

        Controls.Add(pnlFiltros);
        Controls.Add(lblContador);

        var nuevoTop = Px(146);
        DgvHistorial.Height -= nuevoTop - DgvHistorial.Top;
        DgvHistorial.Location = new Point(Px(15), nuevoTop);

        _cargandoFiltros = true;
        CargarCombosFiltro();
        _cargandoFiltros = false;
        ActualizarEstadoFechas();

        ChkFiltrarFecha.CheckedChanged += (s, e) => { ActualizarEstadoFechas(); CargarHistorial(); };
        DtpDesde.ValueChanged += (s, e) => { if (ChkFiltrarFecha.Checked) CargarHistorial(); };
        DtpHasta.ValueChanged += (s, e) => { if (ChkFiltrarFecha.Checked) CargarHistorial(); };
        CboTipoFiltro.SelectedIndexChanged += (s, e) => { if (!_cargandoFiltros) CargarHistorial(); };
        CboMedicoFiltro.SelectedIndexChanged += (s, e) => { if (!_cargandoFiltros) CargarHistorial(); };
    }

    private int Px(int valor) => EscalaDpi.Px(this, valor);

    private void CargarCombosFiltro()
    {
        List<TipoActividad> tipos;
        try
        {
            tipos = _actividadService.ObtenerTiposActividad();
        }
        catch (Exception ex)
        {
            tipos = new List<TipoActividad>();
            lblContador.ForeColor = ColorError;
            lblContador.Text = "No se pudo cargar la lista de tipos de atención: " + ex.Message;
        }
        tipos.Insert(0, new TipoActividad { Id = 0, NombreTipo = "Todos los tipos" });
        CboTipoFiltro.DataSource = tipos;
        CboTipoFiltro.DisplayMember = "NombreTipo";
        CboTipoFiltro.ValueMember = "Id";

        List<Medico> medicos;
        try
        {
            medicos = _medicoService.ObtenerTodos();
        }
        catch (Exception ex)
        {
            medicos = new List<Medico>();
            lblContador.ForeColor = ColorError;
            lblContador.Text = "No se pudo cargar la lista de médicos: " + ex.Message;
        }
        medicos.Insert(0, new Medico { Id = 0, Apellido = "Todos", Nombre = "los médicos", Especialidad = "-" });
        CboMedicoFiltro.DataSource = medicos;
        CboMedicoFiltro.DisplayMember = "NombreCompleto";
        CboMedicoFiltro.ValueMember = "Id";

        CboTipoFiltro.SelectedIndex = 0;
        CboMedicoFiltro.SelectedIndex = 0;
    }

    private void ActualizarEstadoFechas()
    {
        DtpDesde.Enabled = ChkFiltrarFecha.Checked;
        DtpHasta.Enabled = ChkFiltrarFecha.Checked;
        lblHasta.Enabled = ChkFiltrarFecha.Checked;
    }

    private void LimpiarDetalle()
    {
        TxtMotivo.Clear();
        TxtDiagnostico.Clear();
        TxtReceta.Clear();
    }

    private void DesenlazarGrilla()
    {
        DgvHistorial.SelectionChanged -= DgvHistorial_SelectionChanged;
        DgvHistorial.DataSource = null;
        DgvHistorial.SelectionChanged += DgvHistorial_SelectionChanged;
    }

    private void CargarHistorial()
    {
        var tipoId = CboTipoFiltro.SelectedItem is TipoActividad tipo ? tipo.Id : 0;
        int? medicoId = CboMedicoFiltro.SelectedItem is Medico medico && medico.Id > 0 ? medico.Id : null;
        var hayFiltros = tipoId > 0 || medicoId.HasValue || ChkFiltrarFecha.Checked;

        DateOnly? desde = null;
        DateOnly? hasta = null;
        if (ChkFiltrarFecha.Checked)
        {
            var error = HistorialGrilla.ValidarRango(DtpDesde.Value, DtpHasta.Value);
            if (error != null)
            {
                // Rango invalido: no se consulta; se avisa y se vacia la grilla.
                DesenlazarGrilla();
                LimpiarDetalle();
                lblContador.ForeColor = ColorError;
                lblContador.Text = HistorialGrilla.Contador(0) + ". " + error;
                lblDetalleTitulo.Text = PuedeVerClinico ? "Corrija el rango de fechas para ver el detalle." : "Información clínica reservada al rol Médico.";
                return;
            }

            desde = DateOnly.FromDateTime(DtpDesde.Value);
            hasta = DateOnly.FromDateTime(DtpHasta.Value);
        }

        List<AtencionFila> historial;
        try
        {
            historial = _actividadService.ConsultarFilas(_usuarioActivo?.Rol, _paciente.Id, medicoId, desde, hasta, tipoId > 0 ? tipoId : null);
        }
        catch (Exception ex)
        {
            DesenlazarGrilla();
            LimpiarDetalle();
            lblContador.ForeColor = ColorError;
            lblContador.Text = "No se pudo cargar el historial del paciente: " + ex.Message;
            return;
        }

        DgvHistorial.SelectionChanged -= DgvHistorial_SelectionChanged;
        DgvHistorial.DataSource = historial;
        DgvHistorial.SelectionChanged += DgvHistorial_SelectionChanged;

        // Sin esto, WinForms selecciona sola la primera fila al asignar el
        // DataSource y muestra el detalle de esa atencion sin que el
        // usuario haya clickeado nada.
        DgvHistorial.ClearSelection();
        DgvHistorial.CurrentCell = null;

        lblContador.ForeColor = Color.FromArgb(27, 42, 74);
        if (historial.Count == 0)
        {
            lblContador.Text = HistorialGrilla.Contador(0) + ". " + HistorialGrilla.MensajeVacio(hayFiltros);
            lblDetalleTitulo.Text = PuedeVerClinico ? "No hay atenciones para mostrar." : "Información clínica reservada al rol Médico.";
        }
        else
        {
            lblContador.Text = HistorialGrilla.Contador(historial.Count);
            lblDetalleTitulo.Text = PuedeVerClinico
                ? "Seleccione una atención de la lista para ver el detalle."
                : "Información clínica reservada al rol Médico.";
        }
        LimpiarDetalle();
    }

    private void DgvHistorial_SelectionChanged(object? sender, EventArgs e)
    {
        if (DgvHistorial.CurrentRow?.DataBoundItem is not AtencionFila actividad) return;

        // RNF#07: solo el rol Medico ve el detalle clinico.
        if (!PuedeVerClinico || !actividad.IncluyeDatosClinicos)
        {
            LimpiarDetalle();
            return;
        }

        lblDetalleTitulo.Text = $"Atención del {actividad.FechaStr} con {actividad.MedicoNombre} ({actividad.TipoActividadNombre})";
        TxtMotivo.Text = string.IsNullOrWhiteSpace(actividad.MotivoConsulta) ? "Sin motivo de consulta registrado." : actividad.MotivoConsulta;
        TxtDiagnostico.Text = string.IsNullOrWhiteSpace(actividad.Procedimiento) ? "Sin observaciones adicionales." : actividad.Procedimiento;
        TxtReceta.Text = string.IsNullOrWhiteSpace(actividad.RecetaMedicamentos) ? "Sin prescripción farmacológica." : actividad.RecetaMedicamentos;
    }
}
