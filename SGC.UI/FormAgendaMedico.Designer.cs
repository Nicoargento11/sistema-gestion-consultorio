namespace SGC.UI;

partial class FormAgendaMedico
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

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        pnlSuperior = new Panel();
        BtnHoy = new Button();
        DtpFecha = new DateTimePicker();
        lblFecha = new Label();
        lblMedicoInfo = new Label();
        lblTitulo = new Label();
        pnlResumen = new Panel();
        BtnFiltroCancelados = new Button();
        BtnFiltroAtendidos = new Button();
        BtnFiltroPendientes = new Button();
        BtnFiltroTotal = new Button();
        pnlGrilla = new Panel();
        pnlAcciones = new Panel();
        LblMensaje = new Label();
        BtnHistorialRapido = new Button();
        BtnAtender = new Button();
        lblTurnosContador = new Label();
        lblGrillaTitulo = new Label();
        DgvTurnos = new DataGridView();
        pnlSuperior.SuspendLayout();
        pnlResumen.SuspendLayout();
        pnlGrilla.SuspendLayout();
        pnlAcciones.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvTurnos).BeginInit();
        SuspendLayout();
        // 
        // pnlSuperior
        // 
        pnlSuperior.BackColor = Color.FromArgb(27, 42, 74);
        pnlSuperior.Controls.Add(BtnHoy);
        pnlSuperior.Controls.Add(DtpFecha);
        pnlSuperior.Controls.Add(lblFecha);
        pnlSuperior.Controls.Add(lblMedicoInfo);
        pnlSuperior.Controls.Add(lblTitulo);
        pnlSuperior.Dock = DockStyle.Top;
        pnlSuperior.Location = new Point(0, 0);
        pnlSuperior.Name = "pnlSuperior";
        pnlSuperior.Size = new Size(1000, 75);
        pnlSuperior.TabIndex = 0;
        // 
        // BtnHoy
        // 
        BtnHoy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        BtnHoy.BackColor = Color.FromArgb(46, 134, 222);
        BtnHoy.FlatAppearance.BorderSize = 0;
        BtnHoy.FlatStyle = FlatStyle.Flat;
        BtnHoy.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        BtnHoy.ForeColor = Color.White;
        BtnHoy.Location = new Point(895, 23);
        BtnHoy.Name = "BtnHoy";
        BtnHoy.Size = new Size(80, 30);
        BtnHoy.TabIndex = 4;
        BtnHoy.Text = "Hoy";
        BtnHoy.UseVisualStyleBackColor = false;
        // 
        // DtpFecha
        // 
        DtpFecha.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        DtpFecha.Font = new Font("Segoe UI", 9.5F);
        DtpFecha.Format = DateTimePickerFormat.Short;
        DtpFecha.Location = new Point(755, 24);
        DtpFecha.Name = "DtpFecha";
        DtpFecha.Size = new Size(130, 29);
        DtpFecha.TabIndex = 3;
        // 
        // lblFecha
        // 
        lblFecha.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblFecha.AutoSize = true;
        lblFecha.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblFecha.ForeColor = Color.White;
        lblFecha.Location = new Point(695, 27);
        lblFecha.Name = "lblFecha";
        lblFecha.Size = new Size(59, 23);
        lblFecha.TabIndex = 2;
        lblFecha.Text = "Fecha:";
        // 
        // lblMedicoInfo
        // 
        lblMedicoInfo.AutoSize = true;
        lblMedicoInfo.Font = new Font("Segoe UI", 10F);
        lblMedicoInfo.ForeColor = Color.FromArgb(180, 205, 235);
        lblMedicoInfo.Location = new Point(20, 42);
        lblMedicoInfo.Name = "lblMedicoInfo";
        lblMedicoInfo.Size = new Size(198, 23);
        lblMedicoInfo.TabIndex = 1;
        lblMedicoInfo.Text = "Profesional: Dr. / Dra. ...";
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(18, 10);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(335, 32);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Mi Agenda de Turnos del Dia";
        // 
        // pnlResumen
        // 
        pnlResumen.BackColor = Color.FromArgb(245, 246, 250);
        pnlResumen.Controls.Add(BtnFiltroCancelados);
        pnlResumen.Controls.Add(BtnFiltroAtendidos);
        pnlResumen.Controls.Add(BtnFiltroPendientes);
        pnlResumen.Controls.Add(BtnFiltroTotal);
        pnlResumen.Dock = DockStyle.Top;
        pnlResumen.Location = new Point(0, 75);
        pnlResumen.Name = "pnlResumen";
        pnlResumen.Padding = new Padding(20, 15, 20, 10);
        pnlResumen.Size = new Size(1000, 95);
        pnlResumen.TabIndex = 1;
        // 
        // BtnFiltroCancelados
        // 
        BtnFiltroCancelados.BackColor = Color.FromArgb(250, 235, 235);
        BtnFiltroCancelados.Cursor = Cursors.Hand;
        BtnFiltroCancelados.FlatAppearance.BorderColor = Color.FromArgb(192, 57, 43);
        BtnFiltroCancelados.FlatAppearance.BorderSize = 1;
        BtnFiltroCancelados.FlatStyle = FlatStyle.Flat;
        BtnFiltroCancelados.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnFiltroCancelados.ForeColor = Color.FromArgb(192, 57, 43);
        BtnFiltroCancelados.Location = new Point(665, 15);
        BtnFiltroCancelados.Name = "BtnFiltroCancelados";
        BtnFiltroCancelados.Size = new Size(195, 65);
        BtnFiltroCancelados.TabIndex = 3;
        BtnFiltroCancelados.Text = "CANCELADOS (0)";
        BtnFiltroCancelados.UseVisualStyleBackColor = false;
        // 
        // BtnFiltroAtendidos
        // 
        BtnFiltroAtendidos.BackColor = Color.FromArgb(235, 250, 240);
        BtnFiltroAtendidos.Cursor = Cursors.Hand;
        BtnFiltroAtendidos.FlatAppearance.BorderColor = Color.FromArgb(39, 174, 96);
        BtnFiltroAtendidos.FlatAppearance.BorderSize = 1;
        BtnFiltroAtendidos.FlatStyle = FlatStyle.Flat;
        BtnFiltroAtendidos.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnFiltroAtendidos.ForeColor = Color.FromArgb(39, 174, 96);
        BtnFiltroAtendidos.Location = new Point(450, 15);
        BtnFiltroAtendidos.Name = "BtnFiltroAtendidos";
        BtnFiltroAtendidos.Size = new Size(195, 65);
        BtnFiltroAtendidos.TabIndex = 2;
        BtnFiltroAtendidos.Text = "ATENDIDOS (0)";
        BtnFiltroAtendidos.UseVisualStyleBackColor = false;
        // 
        // BtnFiltroPendientes
        // 
        BtnFiltroPendientes.BackColor = Color.FromArgb(235, 245, 255);
        BtnFiltroPendientes.Cursor = Cursors.Hand;
        BtnFiltroPendientes.FlatAppearance.BorderColor = Color.FromArgb(41, 128, 185);
        BtnFiltroPendientes.FlatAppearance.BorderSize = 1;
        BtnFiltroPendientes.FlatStyle = FlatStyle.Flat;
        BtnFiltroPendientes.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnFiltroPendientes.ForeColor = Color.FromArgb(41, 128, 185);
        BtnFiltroPendientes.Location = new Point(235, 15);
        BtnFiltroPendientes.Name = "BtnFiltroPendientes";
        BtnFiltroPendientes.Size = new Size(195, 65);
        BtnFiltroPendientes.TabIndex = 1;
        BtnFiltroPendientes.Text = "PENDIENTES (0)";
        BtnFiltroPendientes.UseVisualStyleBackColor = false;
        // 
        // BtnFiltroTotal
        // 
        BtnFiltroTotal.BackColor = Color.White;
        BtnFiltroTotal.Cursor = Cursors.Hand;
        BtnFiltroTotal.FlatAppearance.BorderColor = Color.FromArgb(27, 42, 74);
        BtnFiltroTotal.FlatAppearance.BorderSize = 2;
        BtnFiltroTotal.FlatStyle = FlatStyle.Flat;
        BtnFiltroTotal.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnFiltroTotal.ForeColor = Color.FromArgb(27, 42, 74);
        BtnFiltroTotal.Location = new Point(20, 15);
        BtnFiltroTotal.Name = "BtnFiltroTotal";
        BtnFiltroTotal.Size = new Size(195, 65);
        BtnFiltroTotal.TabIndex = 0;
        BtnFiltroTotal.Text = "TOTAL TURNOS (0)";
        BtnFiltroTotal.UseVisualStyleBackColor = false;
        // 
        // pnlGrilla
        // 
        pnlGrilla.BackColor = Color.FromArgb(245, 246, 250);
        pnlGrilla.Controls.Add(DgvTurnos);
        pnlGrilla.Controls.Add(pnlAcciones);
        pnlGrilla.Controls.Add(lblTurnosContador);
        pnlGrilla.Controls.Add(lblGrillaTitulo);
        pnlGrilla.Dock = DockStyle.Fill;
        pnlGrilla.Location = new Point(0, 170);
        pnlGrilla.Name = "pnlGrilla";
        pnlGrilla.Padding = new Padding(20, 10, 20, 20);
        pnlGrilla.Size = new Size(1000, 480);
        pnlGrilla.TabIndex = 2;
        // 
        // pnlAcciones
        // 
        pnlAcciones.BackColor = Color.Transparent;
        pnlAcciones.Controls.Add(LblMensaje);
        pnlAcciones.Controls.Add(BtnHistorialRapido);
        pnlAcciones.Controls.Add(BtnAtender);
        pnlAcciones.Dock = DockStyle.Bottom;
        pnlAcciones.Location = new Point(20, 410);
        pnlAcciones.Name = "pnlAcciones";
        pnlAcciones.Size = new Size(960, 50);
        pnlAcciones.TabIndex = 3;
        // 
        // LblMensaje
        // 
        LblMensaje.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        LblMensaje.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        LblMensaje.Location = new Point(0, 12);
        LblMensaje.Name = "LblMensaje";
        LblMensaje.Size = new Size(540, 28);
        LblMensaje.TabIndex = 2;
        // 
        // BtnHistorialRapido
        // 
        BtnHistorialRapido.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnHistorialRapido.BackColor = Color.FromArgb(27, 42, 74);
        BtnHistorialRapido.FlatAppearance.BorderSize = 0;
        BtnHistorialRapido.FlatStyle = FlatStyle.Flat;
        BtnHistorialRapido.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnHistorialRapido.ForeColor = Color.White;
        BtnHistorialRapido.Location = new Point(560, 8);
        BtnHistorialRapido.Name = "BtnHistorialRapido";
        BtnHistorialRapido.Size = new Size(185, 36);
        BtnHistorialRapido.TabIndex = 1;
        BtnHistorialRapido.Text = "Ver Historial Clinico";
        BtnHistorialRapido.UseVisualStyleBackColor = false;
        // 
        // BtnAtender
        // 
        BtnAtender.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnAtender.BackColor = Color.FromArgb(39, 174, 96);
        BtnAtender.FlatAppearance.BorderSize = 0;
        BtnAtender.FlatStyle = FlatStyle.Flat;
        BtnAtender.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnAtender.ForeColor = Color.White;
        BtnAtender.Location = new Point(760, 8);
        BtnAtender.Name = "BtnAtender";
        BtnAtender.Size = new Size(200, 36);
        BtnAtender.TabIndex = 0;
        BtnAtender.Text = "Atender Paciente";
        BtnAtender.UseVisualStyleBackColor = false;
        // 
        // lblTurnosContador
        // 
        lblTurnosContador.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblTurnosContador.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        lblTurnosContador.ForeColor = Color.FromArgb(100, 110, 120);
        lblTurnosContador.Location = new Point(680, 12);
        lblTurnosContador.Name = "lblTurnosContador";
        lblTurnosContador.Size = new Size(300, 20);
        lblTurnosContador.TabIndex = 1;
        lblTurnosContador.Text = "0 turno(s) para hoy";
        lblTurnosContador.TextAlign = ContentAlignment.TopRight;
        // 
        // lblGrillaTitulo
        // 
        lblGrillaTitulo.AutoSize = true;
        lblGrillaTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblGrillaTitulo.ForeColor = Color.FromArgb(27, 42, 74);
        lblGrillaTitulo.Location = new Point(20, 8);
        lblGrillaTitulo.Name = "lblGrillaTitulo";
        lblGrillaTitulo.Size = new Size(245, 28);
        lblGrillaTitulo.TabIndex = 0;
        lblGrillaTitulo.Text = "Lista de Turnos y Pacientes";
        // 
        // DgvTurnos
        // 
        DgvTurnos.AllowUserToAddRows = false;
        DgvTurnos.AllowUserToDeleteRows = false;
        DgvTurnos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        DgvTurnos.BackgroundColor = Color.White;
        DgvTurnos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        DgvTurnos.Location = new Point(20, 40);
        DgvTurnos.MultiSelect = false;
        DgvTurnos.Name = "DgvTurnos";
        DgvTurnos.ReadOnly = true;
        DgvTurnos.RowHeadersVisible = false;
        DgvTurnos.RowHeadersWidth = 51;
        DgvTurnos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvTurnos.Size = new Size(960, 360);
        DgvTurnos.TabIndex = 2;
        // 
        // FormAgendaMedico
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(1000, 650);
        Controls.Add(pnlGrilla);
        Controls.Add(pnlResumen);
        Controls.Add(pnlSuperior);
        Name = "FormAgendaMedico";
        Text = "Mi Agenda de Atencion Medica";
        pnlSuperior.ResumeLayout(false);
        pnlSuperior.PerformLayout();
        pnlResumen.ResumeLayout(false);
        pnlGrilla.ResumeLayout(false);
        pnlGrilla.PerformLayout();
        pnlAcciones.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)DgvTurnos).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Panel pnlSuperior;
    private Label lblTitulo;
    private Label lblMedicoInfo;
    private Label lblFecha;
    private DateTimePicker DtpFecha;
    private Button BtnHoy;
    private Panel pnlResumen;
    private Button BtnFiltroTotal;
    private Button BtnFiltroPendientes;
    private Button BtnFiltroAtendidos;
    private Button BtnFiltroCancelados;
    private Panel pnlGrilla;
    private Label lblGrillaTitulo;
    private Label lblTurnosContador;
    private DataGridView DgvTurnos;
    private Panel pnlAcciones;
    private Button BtnAtender;
    private Button BtnHistorialRapido;
    private Label LblMensaje;
}