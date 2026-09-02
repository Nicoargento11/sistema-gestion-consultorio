using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormTurnos : Form
{
    private readonly PacienteService _pacienteService = new();
    private readonly MedicoService _medicoService = new();
    private readonly HorarioService _horarioService = new();
    private readonly TurnoService _turnoService = new();
    private int? _idTurnoSeleccionado = null;

    public FormTurnos()
    {
        InitializeComponent();
        ConfigurarColumnas();
        CargarCombos();
        DgvTurnos.SelectionChanged += DgvTurnos_SelectionChanged;
        BtnModificar.Click += BtnModificar_Click;
        BtnNuevoTurno.Click += (s, e) =>
        {
            DgvTurnos.SelectionChanged -= DgvTurnos_SelectionChanged;
            DgvTurnos.ClearSelection();
            DgvTurnos.SelectionChanged += DgvTurnos_SelectionChanged;
            LimpiarSeleccion();
        };
        ChkMostrarCancelados.CheckedChanged += (s, e) => CargarGrilla();
        ChkTodosMedicos.CheckedChanged += (s, e) => CargarGrilla();
        ChkFiltrarFecha.CheckedChanged += (s, e) => CargarGrilla();
        CboMedico.SelectedIndexChanged += (s, e) => { ActualizarAgenda(); CargarGrilla(); };
        DtpFecha.ValueChanged += (s, e) => { ActualizarAgenda(); CargarGrilla(); };
        DgvAgenda.CellClick += DgvAgenda_CellClick;
        DgvAgenda.CellDoubleClick += DgvAgenda_CellDoubleClick;

        CargarGrilla();
        ActualizarAgenda();
    }

    private void DgvAgenda_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var horario = (Horario)DgvAgenda.Rows[e.RowIndex].Tag!;
        CboHorario.SelectedValue = horario.Id;
    }

    private void DgvAgenda_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var horario = (Horario)DgvAgenda.Rows[e.RowIndex].Tag!;

        if (CboMedico.SelectedItem != null &&
            _turnoService.HorarioOcupado(((Medico)CboMedico.SelectedItem).Id, horario.Id, DateOnly.FromDateTime(DtpFecha.Value)))
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Ese horario ya esta ocupado.";
            return;
        }

        CboHorario.SelectedValue = horario.Id;
        BtnAsignar_Click(this, e);
    }

    private void ActualizarAgenda()
    {
        DgvAgenda.Rows.Clear();

        if (CboMedico.SelectedItem == null) return;

        var medico = (Medico)CboMedico.SelectedItem;
        var fecha = DateOnly.FromDateTime(DtpFecha.Value);

        foreach (var horario in _horarioService.ObtenerTodos())
        {
            bool ocupado = _turnoService.HorarioOcupado(medico.Id, horario.Id, fecha);
            int fila = DgvAgenda.Rows.Add(horario.Rango, ocupado ? "Ocupado" : "Disponible");

            DgvAgenda.Rows[fila].Tag = horario;

            // Mismo patron de fila coloreada que ya usaste en el TP4 (saldo < 50 = fila roja).
            DgvAgenda.Rows[fila].DefaultCellStyle.BackColor = ocupado
                ? Color.FromArgb(250, 220, 220)
                : Color.FromArgb(220, 245, 225);
        }
    }

    private void DgvTurnos_SelectionChanged(object? sender, EventArgs e)
    {
        if (DgvTurnos.CurrentRow == null) return;

        var turno = (Turno)DgvTurnos.CurrentRow.DataBoundItem;

        _idTurnoSeleccionado = turno.Id;
        CboPaciente.SelectedValue = turno.PacienteId;
        CboMedico.SelectedValue = turno.MedicoId;
        CboHorario.SelectedValue = turno.HorarioId;
        DtpFecha.Value = turno.Fecha.ToDateTime(TimeOnly.MinValue);

        // Paciente y medico de un turno ya asignado no se cambian aca (RF#05
        // solo permite modificar fecha/horario). Se deshabilitan para que
        // quede claro que no son editables mientras hay una fila seleccionada.
        CboPaciente.Enabled = false;
        CboMedico.Enabled = false;
    }

    private void LimpiarSeleccion()
    {
        _idTurnoSeleccionado = null;
        CboPaciente.Enabled = true;
        CboMedico.Enabled = true;
    }

    private void BtnModificar_Click(object? sender, EventArgs e)
    {
        if (_idTurnoSeleccionado == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un turno de la lista para modificar.";
            return;
        }

        if (CboHorario.SelectedItem == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un horario.";
            return;
        }

        try
        {
            var horario = (Horario)CboHorario.SelectedItem;
            var fecha = DateOnly.FromDateTime(DtpFecha.Value);

            _turnoService.ModificarTurno(_idTurnoSeleccionado.Value, horario, fecha);

            CargarGrilla();
            LimpiarSeleccion();
            ActualizarAgenda();
            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Turno modificado correctamente.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void ConfigurarColumnas()
    {
        DgvTurnos.AutoGenerateColumns = false;
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFecha", HeaderText = "Fecha", DataPropertyName = "Fecha", Width = 100 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedico", HeaderText = "Medico", DataPropertyName = "MedicoNombre", Width = 220 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPaciente", HeaderText = "Paciente", DataPropertyName = "PacienteNombre", Width = 200 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHorario", HeaderText = "Horario", DataPropertyName = "HorarioRango", Width = 130 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstado", HeaderText = "Estado", DataPropertyName = "Estado", Width = 100 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedioPago", HeaderText = "Medio de pago", DataPropertyName = "MedioPago", Width = 120 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMonto", HeaderText = "Monto", DataPropertyName = "Monto", Width = 90 });

        DgvAgenda.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAgendaHorario", HeaderText = "Horario", Width = 130 });
        DgvAgenda.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAgendaEstado", HeaderText = "Estado", Width = 100 });
    }

    private void CargarCombos()
    {
        CboPaciente.DataSource = _pacienteService.ObtenerTodos();
        CboPaciente.DisplayMember = "NombreCompleto";
        CboPaciente.ValueMember = "Id";

        CboMedico.DataSource = _medicoService.ObtenerTodos();
        CboMedico.DisplayMember = "NombreCompleto";
        CboMedico.ValueMember = "Id";

        CboHorario.DataSource = _horarioService.ObtenerTodos();
        CboHorario.DisplayMember = "Rango";
        CboHorario.ValueMember = "Id";

        DtpFecha.MinDate = DateTime.Today;
    }

    private void CargarGrilla()
    {
        int? medicoFiltro = null;
        if (!ChkTodosMedicos.Checked && CboMedico.SelectedItem != null)
            medicoFiltro = ((Medico)CboMedico.SelectedItem).Id;

        DateOnly? fechaFiltro = ChkFiltrarFecha.Checked
            ? DateOnly.FromDateTime(DtpFecha.Value)
            : null;

        // Desconectamos el evento antes de reasignar el DataSource: WinForms
        // selecciona sola la primera fila al hacerlo, y eso disparaba
        // SelectionChanged sin que el usuario clickeara nada (el bug de
        // "se vuelve a Gomez, Laura" y de no poder deseleccionar). Reconectamos
        // apenas termina, asi el clic manual del usuario sigue funcionando normal.
        DgvTurnos.SelectionChanged -= DgvTurnos_SelectionChanged;
        DgvTurnos.DataSource = _turnoService.ObtenerTodos(ChkMostrarCancelados.Checked, medicoFiltro, fechaFiltro);
        DgvTurnos.ClearSelection();
        DgvTurnos.SelectionChanged += DgvTurnos_SelectionChanged;
    }

    private void BtnAsignar_Click(object sender, EventArgs e)
    {
        if (CboPaciente.SelectedItem == null || CboMedico.SelectedItem == null || CboHorario.SelectedItem == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Debe seleccionar paciente, medico y horario.";
            return;
        }

        try
        {
            var paciente = (Paciente)CboPaciente.SelectedItem;
            var medico = (Medico)CboMedico.SelectedItem;
            var horario = (Horario)CboHorario.SelectedItem;
            var fecha = DateOnly.FromDateTime(DtpFecha.Value);

            _turnoService.AsignarTurno(paciente, medico, horario, fecha);

            CargarGrilla();
            LimpiarSeleccion();
            ActualizarAgenda();
            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Turno asignado correctamente.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void BtnCancelar_Click(object sender, EventArgs e)
    {
        if (DgvTurnos.CurrentRow == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un turno de la lista primero.";
            return;
        }

        var turnoSeleccionado = (Turno)DgvTurnos.CurrentRow.DataBoundItem;

        var respuesta = MessageBox.Show(
            $"Esta seguro que desea cancelar el turno de {turnoSeleccionado.PacienteNombre} el {turnoSeleccionado.Fecha:dd/MM/yyyy}?",
            "Confirmar cancelacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (respuesta != DialogResult.Yes)
            return;

        try
        {
            _turnoService.CancelarTurno(turnoSeleccionado.Id);
            CargarGrilla();
            LimpiarSeleccion();
            ActualizarAgenda();
            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Turno cancelado correctamente.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void BtnConfirmarAsistencia_Click(object sender, EventArgs e)
    {
        if (DgvTurnos.CurrentRow == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un turno de la lista primero";
            return;
        }

        var turno = (Turno)DgvTurnos.CurrentRow.DataBoundItem;

        var dialogo = new FormConfirmarAsistencia(turno);

        if (dialogo.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        try
        {
            _turnoService.ConfirmarAsistencia(turno.Id, dialogo.Asistio, dialogo.MedioPagoSeleccionado, dialogo.MontoSeleccionado);
            CargarGrilla();
            ActualizarAgenda();
            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Asistencia confirmada.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }
}
