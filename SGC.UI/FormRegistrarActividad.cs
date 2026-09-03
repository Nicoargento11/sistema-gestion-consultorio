using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormRegistrarActividad : Form
{
    private readonly Usuario? _usuarioActivo;
    private readonly Turno? _turnoInicial;
    private readonly Action<Form>? _navegador;
    private readonly MedicoService _medicoService = new();
    private readonly TurnoService _turnoService = new();
    private readonly ActividadMedicaService _actividadService = new();

    private Medico? _medicoActual;
    private Turno? _turnoSeleccionado;

    public FormRegistrarActividad(Turno? turnoInicial = null, Usuario? usuarioActivo = null, Action<Form>? navegador = null)
    {
        InitializeComponent();
        _turnoInicial = turnoInicial;
        _usuarioActivo = usuarioActivo;
        _navegador = navegador;

        DeterminarMedicoActivo();
        CargarTiposActividad();

        DtpFecha.Value = _turnoInicial != null
            ? _turnoInicial.Fecha.ToDateTime(TimeOnly.MinValue)
            : DateTime.Today;

        DtpFecha.ValueChanged += (s, e) => CargarComboTurnos();
        BtnRefrescar.Click += (s, e) => CargarComboTurnos();
        CboTurnos.SelectedIndexChanged += CboTurnos_SelectedIndexChanged;

        BtnGuardar.Click += BtnGuardar_Click;
        BtnBorrarRegistro.Click += BtnBorrarRegistro_Click;
        BtnVerHistorial.Click += BtnVerHistorial_Click;
        BtnLimpiar.Click += (s, e) => LimpiarFormulario();
        BtnVolverAgenda.Click += BtnVolverAgenda_Click;

        CargarComboTurnos();

        if (_turnoInicial != null)
        {
            CboTurnos.SelectedValue = _turnoInicial.Id;
        }
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
            lblMedicoInfo.Text = $"Profesional: {_medicoActual.NombreCompleto} | Mat: {_medicoActual.Matricula}";
        }
    }

    private void CargarTiposActividad()
    {
        CboTipoActividad.DataSource = _actividadService.ObtenerTiposActividad();
        CboTipoActividad.DisplayMember = "NombreTipo";
        CboTipoActividad.ValueMember = "Id";
    }

    private void CargarComboTurnos()
    {
        if (_medicoActual == null) return;

        var fechaSeleccionada = DateOnly.FromDateTime(DtpFecha.Value);
        var turnos = _turnoService.ObtenerPorMedicoYFecha(_medicoActual.Id, fechaSeleccionada, false);

        var listaTurnosCombo = turnos.Select(t => new
        {
            t.Id,
            Descripcion = $"{t.HorarioRango} - {t.PacienteNombre} (DNI: {t.PacienteDni}) [{(t.ActividadMedica?.Activo == true ? "Atendido" : "Pendiente")}]",
            TurnoObj = t
        }).ToList();

        CboTurnos.SelectedIndexChanged -= CboTurnos_SelectedIndexChanged;
        CboTurnos.DataSource = listaTurnosCombo;
        CboTurnos.DisplayMember = "Descripcion";
        CboTurnos.ValueMember = "Id";
        CboTurnos.SelectedIndexChanged += CboTurnos_SelectedIndexChanged;

        if (listaTurnosCombo.Count > 0)
        {
            CboTurnos.SelectedIndex = 0;
            CboTurnos_SelectedIndexChanged(this, EventArgs.Empty);
        }
        else
        {
            _turnoSeleccionado = null;
            lblInfoPaciente.Text = "No hay turnos activos registrados para la fecha seleccionada.";
            LimpiarFormulario();
            BtnGuardar.Enabled = false;
        }
    }

    private void CboTurnos_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (CboTurnos.SelectedItem == null || CboTurnos.SelectedValue is not int turnoId)
        {
            _turnoSeleccionado = null;
            LimpiarFormulario();
            return;
        }

        _turnoSeleccionado = _turnoService.ObtenerPorId(turnoId);

        if (_turnoSeleccionado != null)
        {
            lblInfoPaciente.Text = $"Paciente: {_turnoSeleccionado.PacienteNombre} | DNI: {_turnoSeleccionado.PacienteDni} | Horario: {_turnoSeleccionado.HorarioRango} | Fecha: {_turnoSeleccionado.Fecha:dd/MM/yyyy}";

            var actividad = _actividadService.ObtenerPorTurnoId(_turnoSeleccionado.Id) ?? _turnoSeleccionado.ActividadMedica;

            if (actividad != null && actividad.Activo)
            {
                CboTipoActividad.SelectedValue = actividad.TipoActividadId;
                TxtMotivo.Text = actividad.MotivoConsulta;
                TxtDiagnostico.Text = actividad.Procedimiento;
                TxtReceta.Text = actividad.RecetaMedicamentos;

                BtnGuardar.Text = "Actualizar Atencion";
                BtnBorrarRegistro.Visible = true;
                BtnVerHistorial.Enabled = _turnoSeleccionado.Paciente != null;
                LblMensaje.ForeColor = Color.FromArgb(46, 134, 222);
                LblMensaje.Text = "Atencion cargada previamente. Puede modificarla y guardar los cambios.";
            }
            else
            {
                TxtMotivo.Clear();
                TxtDiagnostico.Clear();
                TxtReceta.Clear();
                if (CboTipoActividad.Items.Count > 0)
                    CboTipoActividad.SelectedIndex = 0;

                BtnGuardar.Text = "Guardar Atencion";
                BtnBorrarRegistro.Visible = false;
                BtnVerHistorial.Enabled = _turnoSeleccionado.Paciente != null;
                LblMensaje.Text = "";
            }

            BtnGuardar.Enabled = true;
        }
    }

    private void LimpiarFormulario()
    {
        TxtMotivo.Clear();
        TxtDiagnostico.Clear();
        TxtReceta.Clear();
        if (CboTipoActividad.Items.Count > 0)
            CboTipoActividad.SelectedIndex = 0;

        BtnGuardar.Text = "Guardar Atencion";
        BtnBorrarRegistro.Visible = false;
        BtnVerHistorial.Enabled = _turnoSeleccionado != null;
        LblMensaje.Text = "";
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_turnoSeleccionado == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Debe seleccionar un turno primero.";
            return;
        }

        if (CboTipoActividad.SelectedValue == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un tipo de actividad.";
            return;
        }

        try
        {
            int tipoId = (int)CboTipoActividad.SelectedValue;
            _actividadService.RegistrarOModificarActividad(
                _turnoSeleccionado,
                tipoId,
                TxtMotivo.Text,
                TxtDiagnostico.Text,
                TxtReceta.Text);

            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Atencion medica guardada exitosamente.";

            DialogResult = DialogResult.OK;
            int idActual = _turnoSeleccionado.Id;
            CargarComboTurnos();
            CboTurnos.SelectedValue = idActual;
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void BtnBorrarRegistro_Click(object? sender, EventArgs e)
    {
        if (_turnoSeleccionado == null) return;

        var actividad = _actividadService.ObtenerPorTurnoId(_turnoSeleccionado.Id);
        if (actividad == null) return;

        var confirmacion = MessageBox.Show(
            "Esta seguro que desea eliminar la atencion registrada para este paciente?",
            "Confirmar eliminacion",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmacion != DialogResult.Yes) return;

        try
        {
            _actividadService.EliminarLogico(actividad.Id);
            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Registro clinico eliminado correctamente (Baja logica).";

            DialogResult = DialogResult.OK;
            int idActual = _turnoSeleccionado.Id;
            CargarComboTurnos();
            CboTurnos.SelectedValue = idActual;
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void BtnVerHistorial_Click(object? sender, EventArgs e)
    {
        if (_turnoSeleccionado?.Paciente == null)
        {
            MessageBox.Show("Seleccione un turno con paciente valido.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

    private void BtnVolverAgenda_Click(object? sender, EventArgs e)
    {
        if (_navegador != null)
        {
            _navegador(new FormAgendaMedico(_usuarioActivo, _navegador));
        }
        else
        {
            Close();
        }
    }
}