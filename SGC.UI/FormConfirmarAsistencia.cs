using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

// Dialogo modal para confirmar la asistencia de un turno (lo abre FormTurnos).
// Tarjeta compacta: datos del turno, Asistio/Ausente, medio de pago y monto.
// Todo se acomoda con TableLayoutPanel y el alto del dialogo se calcula con el
// contenido real (se mide en Load, ya con la escala de pantalla aplicada), asi
// nada se recorta con una escala de 150% o mayor.
public partial class FormConfirmarAsistencia : Form
{
    private readonly Turno _turno;
    private readonly FranjaMensaje _mensajes;
    private const string OpcionParticular = "Particular";

    // true mientras se recalcula el alto: evita reentradas desde SizeChanged.
    private bool _ajustando;

    public bool Asistio { get; private set; }
    public string? MedioPagoSeleccionado { get; private set; }
    public decimal? MontoSeleccionado { get; private set; }

    public FormConfirmarAsistencia(Turno turno)
    {
        InitializeComponent();
        _turno = turno;
        _mensajes = new FranjaMensaje(pnlEstado, LblMensaje);

        lblPaciente.Text = turno.PacienteNombre;
        lblMedico.Text = turno.MedicoNombre;
        lblFecha.Text = $"{turno.Fecha:dd/MM/yyyy}";
        lblHorario.Text = turno.HorarioRango;

        CboMedioPago.Items.Add(OpcionParticular);

        // Solo se ofrece la obra social del paciente como medio de pago si
        // ESTE medico puntual la acepta (Medico.ObrasSocialesAceptadas) - si
        // el paciente tiene una obra social que el medico no cubre, paga
        // particular como cualquier otro paciente sin obra social.
        // La regla vive en TurnoService (la comparte la grilla de FormTurnos).
        if (TurnoService.MedicoAceptaObraSocialDelPaciente(turno))
            CboMedioPago.Items.Add(turno.Paciente!.ObraSocial!.Nombre);

        EstiloUi.EstilizarBoton(BtnConfirmar, TipoBoton.Primario);
        EstiloUi.EstilizarBoton(BtnCancelar, TipoBoton.Secundario);

        RbAsistio.CheckedChanged += RadioButtons_CheckedChanged;
        RbAusente.CheckedChanged += RadioButtons_CheckedChanged;
        CboMedioPago.SelectedIndexChanged += CboMedioPago_SelectedIndexChanged;
        BtnConfirmar.Click += BtnConfirmar_Click;

        // Los textos que pueden partirse en varias lineas y el alto del dialogo
        // se recalculan cuando cambia el ancho o aparece un mensaje.
        tlpInfo.SizeChanged += (s, e) => AcotarEtiquetas();
        pnlEstado.VisibleChanged += (s, e) => AjustarAlto();
        pnlEstado.SizeChanged += (s, e) => AjustarAlto();
        Load += FormConfirmarAsistencia_Load;
    }

    private void FormConfirmarAsistencia_Load(object? sender, EventArgs e)
    {
        AcotarEtiquetas();
        AjustarAlto();

        // Minimo razonable: el tamano calculado con el contenido inicial.
        MinimumSize = Size;
    }

    // Ancho disponible para el contenido de la tarjeta, calculado desde el
    // ClientSize: durante Load los paneles todavia no tienen su ancho final.
    private int AnchoContenido =>
        ClientSize.Width - Padding.Horizontal - pnlMarcoTarjeta.Padding.Horizontal - pnlTarjeta.Padding.Horizontal;

    // Limita el ancho de los valores del turno al lugar que les queda en la tarjeta.
    private void AcotarEtiquetas()
    {
        var columnas = tlpInfo.GetColumnWidths();
        if (columnas.Length < 2) return;

        int ancho = AnchoContenido - columnas[0];
        if (ancho <= 0) return;

        var maximo = new Size(ancho, 0);
        foreach (var valor in new[] { lblPaciente, lblMedico, lblFecha, lblHorario })
            if (valor.MaximumSize != maximo) valor.MaximumSize = maximo;
    }

    // El dialogo mide lo que ocupa su contenido con el ancho real y se ajusta.
    private void AjustarAlto()
    {
        if (_ajustando || !IsHandleCreated) return;
        _ajustando = true;
        try
        {
            AcotarEtiquetas();
            int ancho = AnchoContenido;
            if (ancho <= 0) return;

            // Una fila AutoSize de la tabla toma el tamano preferido del panel del
            // mensaje, que ignora su Height: se lo fija como minimo para que la
            // tabla (y el dialogo) crezcan con el texto del mensaje.
            if (pnlEstado.Visible && pnlEstado.MinimumSize.Height != pnlEstado.Height)
                pnlEstado.MinimumSize = new Size(0, pnlEstado.Height);

            int contenido = tlpRaiz.GetPreferredSize(new Size(ancho, 0)).Height;
            int extra = pnlTarjeta.Padding.Vertical + pnlMarcoTarjeta.Padding.Vertical + Padding.Vertical;
            int deseado = contenido + extra;
            if (ClientSize.Height != deseado)
                ClientSize = new Size(ClientSize.Width, deseado);

            // La tabla raiz (AutoSize) recalcula su alto con el contenido actual
            // (por ejemplo cuando aparece el mensaje de error).
            tlpRaiz.PerformLayout();
            pnlTarjeta.PerformLayout();
        }
        finally
        {
            _ajustando = false;
        }
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

        try
        {
            decimal monto = CboMedioPago.SelectedItem.ToString() == OpcionParticular
                ? TurnoService.CalcularMontoParticular(_turno)
                : TurnoService.CalcularMontoConObraSocial(_turno);
            NudMonto.Value = Math.Min(monto, NudMonto.Maximum);
        }
        catch (Exception ex)
        {
            _mensajes.Mostrar("No se pudo calcular el monto sugerido: " + ex.Message, TipoMensaje.Error);
        }
    }

    private void BtnConfirmar_Click(object? sender, EventArgs e)
    {
        if (!RbAsistio.Checked && !RbAusente.Checked)
        {
            _mensajes.Mostrar("Indique si el paciente asistió o no.", TipoMensaje.Error);
            return;
        }

        if (RbAsistio.Checked)
        {
            if (CboMedioPago.SelectedItem == null)
            {
                _mensajes.Mostrar("Seleccione el medio de pago.", TipoMensaje.Error);
                return;
            }

            if (NudMonto.Value < 0)
            {
                _mensajes.Mostrar("El monto no puede ser negativo.", TipoMensaje.Error);
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
