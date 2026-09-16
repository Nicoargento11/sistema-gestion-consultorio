using SGC.Entidades;

namespace SGC.UI;

public partial class FormConfirmarAsistencia : Form
{
    private readonly Turno _turno;
    private const string OpcionParticular = "Particular";

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

        CboMedioPago.Items.Add(OpcionParticular);

        // Solo se ofrece la obra social del paciente como medio de pago si
        // ESTE medico puntual la acepta (Medico.ObrasSocialesAceptadas) - si
        // el paciente tiene una obra social que el medico no cubre, paga
        // particular como cualquier otro paciente sin obra social.
        var obraSocial = turno.Paciente?.ObraSocial;
        bool medicoAceptaObraSocial = obraSocial != null &&
            (turno.Medico?.ObrasSocialesAceptadas.Any(o => o.Id == obraSocial.Id) ?? false);

        if (medicoAceptaObraSocial)
            CboMedioPago.Items.Add(obraSocial!.Nombre);

        RbAsistio.CheckedChanged += RadioButtons_CheckedChanged;
        RbAusente.CheckedChanged += RadioButtons_CheckedChanged;
        CboMedioPago.SelectedIndexChanged += CboMedioPago_SelectedIndexChanged;
        BtnConfirmar.Click += BtnConfirmar_Click;
    }

    private void RadioButtons_CheckedChanged(object? sender, EventArgs e)
    {
        // El medio de pago y el monto solo importan si el paciente asistio.
        CboMedioPago.Enabled = RbAsistio.Checked;
        NudMonto.Enabled = RbAsistio.Checked;
    }

    private void CboMedioPago_SelectedIndexChanged(object? sender, EventArgs e)
    {
        // Precalculamos el monto para que el Recepcionista no tenga que
        // buscar a mano el precio ni hacer la cuenta del % de cobertura -
        // sigue quedando editable por si hace falta un ajuste puntual.
        if (CboMedioPago.SelectedItem == null) return;

        decimal precioBase = _turno.Medico?.PrecioConsultaParticular ?? 0;

        if (CboMedioPago.SelectedItem.ToString() == OpcionParticular)
        {
            NudMonto.Value = Math.Min(precioBase, NudMonto.Maximum);
            return;
        }

        decimal cobertura = _turno.Paciente?.ObraSocial?.PorcentajeCobertura ?? 0;
        decimal montoConCobertura = precioBase - (precioBase * cobertura / 100m);
        NudMonto.Value = Math.Min(Math.Max(montoConCobertura, 0), NudMonto.Maximum);
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
