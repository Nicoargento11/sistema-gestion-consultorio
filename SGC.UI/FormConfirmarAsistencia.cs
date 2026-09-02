using SGC.Entidades;

namespace SGC.UI;

public partial class FormConfirmarAsistencia : Form
{
    private readonly Turno _turno;

    public bool Asistio { get; private set; }
    public string? MedioPagoSeleccionado { get; private set; }
    public decimal? MontoSeleccionado { get; private set; }

    public FormConfirmarAsistencia(Turno turno)
    {
        InitializeComponent();
        _turno = turno;

        lblInfo.Text = $"Paciente: {turno.PacienteNombre}\n" +
                       $"Medico: {turno.MedicoNombre}\n" +
                       $"Fecha: {turno.Fecha:dd/MM/yyyy}\n" +
                       $"Horario: {turno.HorarioRango}";

        CboMedioPago.Items.Add("Particular");
        if (!string.IsNullOrWhiteSpace(turno.Paciente?.ObraSocial))
            CboMedioPago.Items.Add(turno.Paciente.ObraSocial);

        RbAsistio.CheckedChanged += RadioButtons_CheckedChanged;
        RbAusente.CheckedChanged += RadioButtons_CheckedChanged;
        BtnConfirmar.Click += BtnConfirmar_Click;
    }

    private void RadioButtons_CheckedChanged(object? sender, EventArgs e)
    {
        // El medio de pago y el monto solo importan si el paciente asistio.
        CboMedioPago.Enabled = RbAsistio.Checked;
        NudMonto.Enabled = RbAsistio.Checked;
    }

    private void BtnConfirmar_Click(object? sender, EventArgs e)
    {
        if (!RbAsistio.Checked && !RbAusente.Checked)
        {
            LblMensaje.Text = "Indique si el paciente asistio o no.";
            return;
        }

        if (RbAsistio.Checked)
        {
            if (CboMedioPago.SelectedItem == null)
            {
                LblMensaje.Text = "Seleccione el medio de pago.";
                return;
            }

            if (NudMonto.Value < 0)
            {
                LblMensaje.Text = "El monto no puede ser negativo.";
                return;
            }

            Asistio = true;
            MedioPagoSeleccionado = CboMedioPago.SelectedItem.ToString();
            MontoSeleccionado = NudMonto.Value;
        }
        else
        {
            Asistio = false;
            MedioPagoSeleccionado = null;
            MontoSeleccionado = null;
        }

        DialogResult = DialogResult.OK;
        Close();
    }
}
