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
        pnlMarcoTarjeta = new Panel();
        pnlTarjeta = new Panel();
        tlpRaiz = new TableLayoutPanel();
        tlpInfo = new TableLayoutPanel();
        lblPacienteTitulo = new Label();
        lblPaciente = new Label();
        lblMedicoTitulo = new Label();
        lblMedico = new Label();
        lblFechaTitulo = new Label();
        lblFecha = new Label();
        lblHorarioTitulo = new Label();
        lblHorario = new Label();
        pnlSeparador = new Panel();
        lblAsistencia = new Label();
        flpAsistencia = new FlowLayoutPanel();
        RbAsistio = new RadioButton();
        RbAusente = new RadioButton();
        lblMedioPago = new Label();
        CboMedioPago = new ComboBox();
        lblMonto = new Label();
        NudMonto = new NumericUpDown();
        pnlEstado = new Panel();
        LblMensaje = new Label();
        tlpBotones = new TableLayoutPanel();
        BtnConfirmar = new Button();
        BtnCancelar = new Button();
        pnlMarcoTarjeta.SuspendLayout();
        pnlTarjeta.SuspendLayout();
        tlpRaiz.SuspendLayout();
        tlpInfo.SuspendLayout();
        flpAsistencia.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)NudMonto).BeginInit();
        pnlEstado.SuspendLayout();
        tlpBotones.SuspendLayout();
        SuspendLayout();
        //
        // pnlMarcoTarjeta
        //
        pnlMarcoTarjeta.Controls.Add(pnlTarjeta);
        pnlMarcoTarjeta.BackColor = Color.FromArgb(214, 220, 230);
        pnlMarcoTarjeta.Dock = DockStyle.Fill;
        pnlMarcoTarjeta.Padding = new Padding(1);
        pnlMarcoTarjeta.TabIndex = 0;
        pnlMarcoTarjeta.Name = "pnlMarcoTarjeta";
        //
        // pnlTarjeta
        //
        pnlTarjeta.Controls.Add(tlpRaiz);
        pnlTarjeta.BackColor = Color.White;
        pnlTarjeta.Dock = DockStyle.Fill;
        pnlTarjeta.Padding = new Padding(16, 14, 16, 14);
        pnlTarjeta.TabIndex = 0;
        pnlTarjeta.Name = "pnlTarjeta";
        //
        // tlpRaiz
        //
        tlpRaiz.Controls.Add(tlpInfo, 0, 0);
        tlpRaiz.Controls.Add(pnlSeparador, 0, 1);
        tlpRaiz.Controls.Add(lblAsistencia, 0, 2);
        tlpRaiz.Controls.Add(flpAsistencia, 0, 3);
        tlpRaiz.Controls.Add(lblMedioPago, 0, 4);
        tlpRaiz.Controls.Add(CboMedioPago, 0, 5);
        tlpRaiz.Controls.Add(lblMonto, 0, 6);
        tlpRaiz.Controls.Add(NudMonto, 0, 7);
        tlpRaiz.Controls.Add(pnlEstado, 0, 8);
        tlpRaiz.Controls.Add(tlpBotones, 0, 9);
        tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRaiz.RowCount = 10;
        tlpRaiz.RowStyles.Add(new RowStyle());
        tlpRaiz.RowStyles.Add(new RowStyle());
        tlpRaiz.RowStyles.Add(new RowStyle());
        tlpRaiz.RowStyles.Add(new RowStyle());
        tlpRaiz.RowStyles.Add(new RowStyle());
        tlpRaiz.RowStyles.Add(new RowStyle());
        tlpRaiz.RowStyles.Add(new RowStyle());
        tlpRaiz.RowStyles.Add(new RowStyle());
        tlpRaiz.RowStyles.Add(new RowStyle());
        tlpRaiz.RowStyles.Add(new RowStyle());
        tlpRaiz.AutoSize = true;
        tlpRaiz.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpRaiz.ColumnCount = 1;
        tlpRaiz.Dock = DockStyle.Top;
        tlpRaiz.TabIndex = 0;
        tlpRaiz.Name = "tlpRaiz";
        //
        // tlpInfo
        //
        tlpInfo.Controls.Add(lblPacienteTitulo, 0, 0);
        tlpInfo.Controls.Add(lblPaciente, 1, 0);
        tlpInfo.Controls.Add(lblMedicoTitulo, 0, 1);
        tlpInfo.Controls.Add(lblMedico, 1, 1);
        tlpInfo.Controls.Add(lblFechaTitulo, 0, 2);
        tlpInfo.Controls.Add(lblFecha, 1, 2);
        tlpInfo.Controls.Add(lblHorarioTitulo, 0, 3);
        tlpInfo.Controls.Add(lblHorario, 1, 3);
        tlpInfo.ColumnStyles.Add(new ColumnStyle());
        tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpInfo.RowCount = 4;
        tlpInfo.RowStyles.Add(new RowStyle());
        tlpInfo.RowStyles.Add(new RowStyle());
        tlpInfo.RowStyles.Add(new RowStyle());
        tlpInfo.RowStyles.Add(new RowStyle());
        tlpInfo.AutoSize = true;
        tlpInfo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpInfo.ColumnCount = 2;
        tlpInfo.Dock = DockStyle.Fill;
        tlpInfo.Margin = new Padding(0);
        tlpInfo.TabIndex = 0;
        tlpInfo.Name = "tlpInfo";
        //
        // lblPacienteTitulo
        //
        lblPacienteTitulo.AutoSize = true;
        lblPacienteTitulo.Font = new Font("Segoe UI", 9.5F);
        lblPacienteTitulo.ForeColor = Color.FromArgb(110, 120, 135);
        lblPacienteTitulo.Margin = new Padding(0, 0, 12, 6);
        lblPacienteTitulo.Text = "Paciente";
        lblPacienteTitulo.TabIndex = 0;
        lblPacienteTitulo.Name = "lblPacienteTitulo";
        //
        // lblPaciente
        //
        lblPaciente.AutoSize = true;
        lblPaciente.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblPaciente.ForeColor = Color.FromArgb(27, 42, 74);
        lblPaciente.Margin = new Padding(0, 0, 0, 6);
        lblPaciente.TabIndex = 1;
        lblPaciente.Name = "lblPaciente";
        //
        // lblMedicoTitulo
        //
        lblMedicoTitulo.AutoSize = true;
        lblMedicoTitulo.Font = new Font("Segoe UI", 9.5F);
        lblMedicoTitulo.ForeColor = Color.FromArgb(110, 120, 135);
        lblMedicoTitulo.Margin = new Padding(0, 0, 12, 6);
        lblMedicoTitulo.Text = "Médico";
        lblMedicoTitulo.TabIndex = 2;
        lblMedicoTitulo.Name = "lblMedicoTitulo";
        //
        // lblMedico
        //
        lblMedico.AutoSize = true;
        lblMedico.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblMedico.ForeColor = Color.FromArgb(27, 42, 74);
        lblMedico.Margin = new Padding(0, 0, 0, 6);
        lblMedico.TabIndex = 3;
        lblMedico.Name = "lblMedico";
        //
        // lblFechaTitulo
        //
        lblFechaTitulo.AutoSize = true;
        lblFechaTitulo.Font = new Font("Segoe UI", 9.5F);
        lblFechaTitulo.ForeColor = Color.FromArgb(110, 120, 135);
        lblFechaTitulo.Margin = new Padding(0, 0, 12, 6);
        lblFechaTitulo.Text = "Fecha";
        lblFechaTitulo.TabIndex = 4;
        lblFechaTitulo.Name = "lblFechaTitulo";
        //
        // lblFecha
        //
        lblFecha.AutoSize = true;
        lblFecha.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblFecha.ForeColor = Color.FromArgb(27, 42, 74);
        lblFecha.Margin = new Padding(0, 0, 0, 6);
        lblFecha.TabIndex = 5;
        lblFecha.Name = "lblFecha";
        //
        // lblHorarioTitulo
        //
        lblHorarioTitulo.AutoSize = true;
        lblHorarioTitulo.Font = new Font("Segoe UI", 9.5F);
        lblHorarioTitulo.ForeColor = Color.FromArgb(110, 120, 135);
        lblHorarioTitulo.Margin = new Padding(0, 0, 12, 0);
        lblHorarioTitulo.Text = "Horario";
        lblHorarioTitulo.TabIndex = 6;
        lblHorarioTitulo.Name = "lblHorarioTitulo";
        //
        // lblHorario
        //
        lblHorario.AutoSize = true;
        lblHorario.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblHorario.ForeColor = Color.FromArgb(27, 42, 74);
        lblHorario.Margin = new Padding(0);
        lblHorario.TabIndex = 7;
        lblHorario.Name = "lblHorario";
        //
        // pnlSeparador
        //
        pnlSeparador.BackColor = Color.FromArgb(214, 220, 230);
        pnlSeparador.Dock = DockStyle.Fill;
        pnlSeparador.Margin = new Padding(0, 12, 0, 0);
        pnlSeparador.Size = new Size(0, 1);
        pnlSeparador.TabIndex = 1;
        pnlSeparador.Name = "pnlSeparador";
        //
        // lblAsistencia
        //
        lblAsistencia.AutoSize = true;
        lblAsistencia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblAsistencia.ForeColor = Color.FromArgb(27, 42, 74);
        lblAsistencia.Margin = new Padding(0, 12, 0, 3);
        lblAsistencia.Text = "Asistencia del paciente";
        lblAsistencia.TabIndex = 2;
        lblAsistencia.Name = "lblAsistencia";
        //
        // flpAsistencia
        //
        flpAsistencia.Controls.Add(RbAsistio);
        flpAsistencia.Controls.Add(RbAusente);
        flpAsistencia.AutoSize = true;
        flpAsistencia.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flpAsistencia.Dock = DockStyle.Fill;
        flpAsistencia.Margin = new Padding(0, 0, 0, 4);
        flpAsistencia.TabIndex = 3;
        flpAsistencia.WrapContents = false;
        flpAsistencia.Name = "flpAsistencia";
        //
        // RbAsistio
        //
        RbAsistio.AutoSize = true;
        RbAsistio.Font = new Font("Segoe UI", 10F);
        RbAsistio.Margin = new Padding(0, 0, 24, 0);
        RbAsistio.TabIndex = 0;
        RbAsistio.TabStop = true;
        RbAsistio.Text = "Asistió";
        RbAsistio.UseVisualStyleBackColor = true;
        RbAsistio.Name = "RbAsistio";
        //
        // RbAusente
        //
        RbAusente.AutoSize = true;
        RbAusente.Font = new Font("Segoe UI", 10F);
        RbAusente.Margin = new Padding(0);
        RbAusente.TabIndex = 1;
        RbAusente.Text = "Ausente";
        RbAusente.UseVisualStyleBackColor = true;
        RbAusente.Name = "RbAusente";
        //
        // lblMedioPago
        //
        lblMedioPago.AutoSize = true;
        lblMedioPago.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMedioPago.ForeColor = Color.FromArgb(27, 42, 74);
        lblMedioPago.Margin = new Padding(0, 10, 0, 3);
        lblMedioPago.Text = "Medio de pago";
        lblMedioPago.TabIndex = 4;
        lblMedioPago.Name = "lblMedioPago";
        //
        // CboMedioPago
        //
        CboMedioPago.Dock = DockStyle.Fill;
        CboMedioPago.DropDownStyle = ComboBoxStyle.DropDownList;
        CboMedioPago.Enabled = false;
        CboMedioPago.Font = new Font("Segoe UI", 10F);
        CboMedioPago.FormattingEnabled = true;
        CboMedioPago.Margin = new Padding(0, 0, 0, 4);
        CboMedioPago.TabIndex = 2;
        CboMedioPago.Name = "CboMedioPago";
        //
        // lblMonto
        //
        lblMonto.AutoSize = true;
        lblMonto.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMonto.ForeColor = Color.FromArgb(27, 42, 74);
        lblMonto.Margin = new Padding(0, 10, 0, 3);
        lblMonto.Text = "Monto";
        lblMonto.TabIndex = 6;
        lblMonto.Name = "lblMonto";
        //
        // NudMonto
        //
        NudMonto.DecimalPlaces = 2;
        NudMonto.Dock = DockStyle.Fill;
        NudMonto.Enabled = false;
        NudMonto.Font = new Font("Segoe UI", 10F);
        NudMonto.Margin = new Padding(0, 0, 0, 4);
        NudMonto.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
        NudMonto.TabIndex = 3;
        NudMonto.ThousandsSeparator = true;
        NudMonto.Name = "NudMonto";
        //
        // pnlEstado
        //
        pnlEstado.Controls.Add(LblMensaje);
        pnlEstado.Dock = DockStyle.Fill;
        pnlEstado.Margin = new Padding(0);
        pnlEstado.Padding = new Padding(2, 8, 2, 0);
        pnlEstado.Size = new Size(0, 36);
        pnlEstado.TabIndex = 8;
        pnlEstado.Visible = false;
        pnlEstado.Name = "pnlEstado";
        //
        // LblMensaje
        //
        LblMensaje.Dock = DockStyle.Fill;
        LblMensaje.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        LblMensaje.TabIndex = 0;
        LblMensaje.Name = "LblMensaje";
        //
        // tlpBotones
        //
        tlpBotones.Controls.Add(BtnConfirmar, 1, 0);
        tlpBotones.Controls.Add(BtnCancelar, 2, 0);
        tlpBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpBotones.ColumnStyles.Add(new ColumnStyle());
        tlpBotones.ColumnStyles.Add(new ColumnStyle());
        tlpBotones.RowCount = 1;
        tlpBotones.RowStyles.Add(new RowStyle());
        tlpBotones.AutoSize = true;
        tlpBotones.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpBotones.ColumnCount = 3;
        tlpBotones.Dock = DockStyle.Fill;
        tlpBotones.Margin = new Padding(0, 16, 0, 0);
        tlpBotones.TabIndex = 9;
        tlpBotones.Name = "tlpBotones";
        //
        // BtnConfirmar
        //
        BtnConfirmar.BackColor = Color.FromArgb(46, 134, 222);
        BtnConfirmar.FlatAppearance.BorderSize = 0;
        BtnConfirmar.FlatStyle = FlatStyle.Flat;
        BtnConfirmar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        BtnConfirmar.ForeColor = Color.White;
        BtnConfirmar.Margin = new Padding(0, 0, 8, 0);
        BtnConfirmar.Size = new Size(130, 38);
        BtnConfirmar.Text = "Confirmar";
        BtnConfirmar.UseVisualStyleBackColor = false;
        BtnConfirmar.TabIndex = 0;
        BtnConfirmar.Name = "BtnConfirmar";
        //
        // BtnCancelar
        //
        BtnCancelar.BackColor = Color.White;
        BtnCancelar.DialogResult = DialogResult.Cancel;
        BtnCancelar.FlatAppearance.BorderColor = Color.FromArgb(214, 220, 230);
        BtnCancelar.FlatAppearance.BorderSize = 1;
        BtnCancelar.FlatStyle = FlatStyle.Flat;
        BtnCancelar.Font = new Font("Segoe UI", 9.5F);
        BtnCancelar.ForeColor = Color.FromArgb(27, 42, 74);
        BtnCancelar.Margin = new Padding(0);
        BtnCancelar.Size = new Size(110, 38);
        BtnCancelar.Text = "Cancelar";
        BtnCancelar.UseVisualStyleBackColor = false;
        BtnCancelar.TabIndex = 1;
        BtnCancelar.Name = "BtnCancelar";
        //
        // FormConfirmarAsistencia
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        CancelButton = BtnCancelar;
        ClientSize = new Size(440, 470);
        Controls.Add(pnlMarcoTarjeta);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormConfirmarAsistencia";
        Padding = new Padding(16);
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Confirmar asistencia";
        pnlMarcoTarjeta.ResumeLayout(false);
        pnlTarjeta.ResumeLayout(false);
        pnlTarjeta.PerformLayout();
        tlpRaiz.ResumeLayout(false);
        tlpRaiz.PerformLayout();
        tlpInfo.ResumeLayout(false);
        tlpInfo.PerformLayout();
        flpAsistencia.ResumeLayout(false);
        flpAsistencia.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)NudMonto).EndInit();
        pnlEstado.ResumeLayout(false);
        tlpBotones.ResumeLayout(false);
        tlpBotones.PerformLayout();
        ResumeLayout(false);
    }

    private Panel pnlMarcoTarjeta;
    private Panel pnlTarjeta;
    private TableLayoutPanel tlpRaiz;
    private TableLayoutPanel tlpInfo;
    private Label lblPacienteTitulo;
    private Label lblPaciente;
    private Label lblMedicoTitulo;
    private Label lblMedico;
    private Label lblFechaTitulo;
    private Label lblFecha;
    private Label lblHorarioTitulo;
    private Label lblHorario;
    private Panel pnlSeparador;
    private Label lblAsistencia;
    private FlowLayoutPanel flpAsistencia;
    private RadioButton RbAsistio;
    private RadioButton RbAusente;
    private Label lblMedioPago;
    private ComboBox CboMedioPago;
    private Label lblMonto;
    private NumericUpDown NudMonto;
    private Panel pnlEstado;
    private Label LblMensaje;
    private TableLayoutPanel tlpBotones;
    private Button BtnConfirmar;
    private Button BtnCancelar;
}
