namespace SGC.UI;

partial class FormConfirmarAsistencia
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblInfo = new Label();
        RbAsistio = new RadioButton();
        RbAusente = new RadioButton();
        lblMedioPago = new Label();
        CboMedioPago = new ComboBox();
        lblMonto = new Label();
        NudMonto = new NumericUpDown();
        BtnConfirmar = new Button();
        BtnCancelar = new Button();
        LblMensaje = new Label();
        SuspendLayout();

        // lblInfo
        lblInfo.AutoSize = true;
        lblInfo.Font = new Font("Segoe UI", 10F);
        lblInfo.ForeColor = Color.FromArgb(27, 42, 74);
        lblInfo.Location = new Point(20, 20);
        lblInfo.MaximumSize = new Size(360, 0);
        lblInfo.Name = "lblInfo";

        // RbAsistio
        RbAsistio.AutoSize = true;
        RbAsistio.Font = new Font("Segoe UI", 10F);
        RbAsistio.Location = new Point(20, 115);
        RbAsistio.Name = "RbAsistio";
        RbAsistio.Size = new Size(90, 29);
        RbAsistio.TabIndex = 0;
        RbAsistio.TabStop = true;
        RbAsistio.Text = "Asistio";
        RbAsistio.UseVisualStyleBackColor = true;

        // RbAusente
        RbAusente.AutoSize = true;
        RbAusente.Font = new Font("Segoe UI", 10F);
        RbAusente.Location = new Point(150, 115);
        RbAusente.Name = "RbAusente";
        RbAusente.Size = new Size(95, 29);
        RbAusente.TabIndex = 1;
        RbAusente.Text = "Ausente";
        RbAusente.UseVisualStyleBackColor = true;

        // lblMedioPago
        lblMedioPago.AutoSize = true;
        lblMedioPago.Font = new Font("Segoe UI", 9F);
        lblMedioPago.Location = new Point(20, 160);
        lblMedioPago.Name = "lblMedioPago";
        lblMedioPago.Text = "Medio de pago";

        // CboMedioPago
        CboMedioPago.DropDownStyle = ComboBoxStyle.DropDownList;
        CboMedioPago.Enabled = false;
        CboMedioPago.Font = new Font("Segoe UI", 10F);
        CboMedioPago.Location = new Point(20, 183);
        CboMedioPago.Name = "CboMedioPago";
        CboMedioPago.Size = new Size(280, 34);
        CboMedioPago.TabIndex = 2;

        // lblMonto
        lblMonto.AutoSize = true;
        lblMonto.Font = new Font("Segoe UI", 9F);
        lblMonto.Location = new Point(20, 225);
        lblMonto.Name = "lblMonto";
        lblMonto.Text = "Monto";

        // NudMonto
        NudMonto.DecimalPlaces = 2;
        NudMonto.Enabled = false;
        NudMonto.Font = new Font("Segoe UI", 10F);
        NudMonto.Location = new Point(20, 248);
        NudMonto.Maximum = 99999999;
        NudMonto.Name = "NudMonto";
        NudMonto.Size = new Size(180, 34);
        NudMonto.TabIndex = 5;
        NudMonto.ThousandsSeparator = true;

        // LblMensaje
        LblMensaje.AutoSize = true;
        LblMensaje.Font = new Font("Segoe UI", 9F);
        LblMensaje.ForeColor = Color.FromArgb(200, 40, 40);
        LblMensaje.Location = new Point(20, 293);
        LblMensaje.MaximumSize = new Size(360, 0);
        LblMensaje.Name = "LblMensaje";

        // BtnConfirmar
        BtnConfirmar.BackColor = Color.FromArgb(46, 134, 222);
        BtnConfirmar.FlatAppearance.BorderSize = 0;
        BtnConfirmar.FlatStyle = FlatStyle.Flat;
        BtnConfirmar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnConfirmar.ForeColor = Color.White;
        BtnConfirmar.Location = new Point(20, 335);
        BtnConfirmar.Name = "BtnConfirmar";
        BtnConfirmar.Size = new Size(130, 34);
        BtnConfirmar.TabIndex = 3;
        BtnConfirmar.Text = "Confirmar";
        BtnConfirmar.UseVisualStyleBackColor = false;

        // BtnCancelar
        BtnCancelar.BackColor = Color.FromArgb(120, 130, 145);
        BtnCancelar.FlatAppearance.BorderSize = 0;
        BtnCancelar.FlatStyle = FlatStyle.Flat;
        BtnCancelar.Font = new Font("Segoe UI", 9.5F);
        BtnCancelar.ForeColor = Color.White;
        BtnCancelar.Location = new Point(160, 335);
        BtnCancelar.Name = "BtnCancelar";
        BtnCancelar.Size = new Size(130, 34);
        BtnCancelar.TabIndex = 4;
        BtnCancelar.Text = "Cancelar";
        BtnCancelar.UseVisualStyleBackColor = false;
        BtnCancelar.DialogResult = DialogResult.Cancel;

        // FormConfirmarAsistencia
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(400, 400);
        Controls.Add(LblMensaje);
        Controls.Add(BtnCancelar);
        Controls.Add(BtnConfirmar);
        Controls.Add(NudMonto);
        Controls.Add(lblMonto);
        Controls.Add(CboMedioPago);
        Controls.Add(lblMedioPago);
        Controls.Add(RbAusente);
        Controls.Add(RbAsistio);
        Controls.Add(lblInfo);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormConfirmarAsistencia";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Confirmar asistencia";
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblInfo;
    private RadioButton RbAsistio;
    private RadioButton RbAusente;
    private Label lblMedioPago;
    private ComboBox CboMedioPago;
    private Label lblMonto;
    private NumericUpDown NudMonto;
    private Button BtnConfirmar;
    private Button BtnCancelar;
    private Label LblMensaje;
}
