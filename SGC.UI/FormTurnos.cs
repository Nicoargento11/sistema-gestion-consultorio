using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormTurnos : Form
{
    private readonly PacienteService _pacienteService = new();
    private readonly MedicoService _medicoService = new();
    private readonly HorarioService _horarioService = new();
    private readonly TurnoService _turnoService = new();

    public FormTurnos()
    {
        InitializeComponent();
        ConfigurarColumnas();
        CargarCombos();
        CargarGrilla();
    }

    private void ConfigurarColumnas()
    {
        DgvTurnos.AutoGenerateColumns = false;
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFecha", HeaderText = "Fecha", DataPropertyName = "Fecha", Width = 100 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedico", HeaderText = "Medico", DataPropertyName = "MedicoNombre", Width = 220 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPaciente", HeaderText = "Paciente", DataPropertyName = "PacienteNombre", Width = 200 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHorario", HeaderText = "Horario", DataPropertyName = "HorarioRango", Width = 130 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstado", HeaderText = "Estado", DataPropertyName = "Estado", Width = 100 });
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
        DgvTurnos.DataSource = _turnoService.ObtenerTodos();
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
            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Turno cancelado correctamente.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }
}
