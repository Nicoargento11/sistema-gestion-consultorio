namespace SGC.UI;

partial class FormRegistrarActividad
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
        pnlHeader = new Panel();
        BtnVolverAgenda = new Button();
        lblMedicoInfo = new Label();
        lblTitulo = new Label();
        pnlSeleccionTurno = new Panel();
        BtnRefrescar = new Button();
        CboTurnos = new ComboBox();
        lblSeleccionarTurno = new Label();
        DtpFecha = new DateTimePicker();
        lblFecha = new Label();
        pnlDetalleTurno = new Panel();
        lblInfoPaciente = new Label();
        pnlFormulario = new Panel();
        LblMensaje = new Label();
        BtnVerHistorial = new Button();
        BtnBorrarRegistro = new Button();
        BtnLimpiar = new Button();
        BtnGuardar = new Button();
        TxtReceta = new TextBox();
        lblReceta = new Label();
        TxtDiagnostico = new TextBox();
        lblDiagnostico = new Label();
        TxtMotivo = new TextBox();
        lblMotivo = new Label();
        CboTipoActividad = new ComboBox();
        lblTipoActividad = new Label();
        pnlHeader.SuspendLayout();
        pnlSeleccionTurno.SuspendLayout();
        pnlDetalleTurno.SuspendLayout();
        pnlFormulario.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(27, 42, 74);
        pnlHeader.Controls.Add(BtnVolverAgenda);
        pnlHeader.Controls.Add(lblMedicoInfo);
        pnlHeader.Controls.Add(lblTitulo);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1000, 75);
        pnlHeader.TabIndex = 0;
        // 
        // BtnVolverAgenda
        // 
        BtnVolverAgenda.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        BtnVolverAgenda.BackColor = Color.FromArgb(46, 134, 222);
        BtnVolverAgenda.FlatAppearance.BorderSize = 0;
        BtnVolverAgenda.FlatStyle = FlatStyle.Flat;
        BtnVolverAgenda.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        BtnVolverAgenda.ForeColor = Color.White;
        BtnVolverAgenda.Location = new Point(835, 23);
        BtnVolverAgenda.Name = "BtnVolverAgenda";
        BtnVolverAgenda.Size = new Size(140, 30);
        BtnVolverAgenda.TabIndex = 2;
        BtnVolverAgenda.Text = "← Volver a Agenda";
        BtnVolverAgenda.UseVisualStyleBackColor = false;
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
        lblTitulo.Size = new Size(350, 32);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Registro de Atencion Clinica";
        // 
        // pnlSeleccionTurno
        // 
        pnlSeleccionTurno.BackColor = Color.FromArgb(245, 246, 250);
        pnlSeleccionTurno.Controls.Add(BtnRefrescar);
        pnlSeleccionTurno.Controls.Add(CboTurnos);
        pnlSeleccionTurno.Controls.Add(lblSeleccionarTurno);
        pnlSeleccionTurno.Controls.Add(DtpFecha);
        pnlSeleccionTurno.Controls.Add(lblFecha);
        pnlSeleccionTurno.Dock = DockStyle.Top;
        pnlSeleccionTurno.Location = new Point(0, 75);
        pnlSeleccionTurno.Name = "pnlSeleccionTurno";
        pnlSeleccionTurno.Padding = new Padding(20, 15, 20, 10);
        pnlSeleccionTurno.Size = new Size(1000, 65);
        pnlSeleccionTurno.TabIndex = 1;
        // 
        // BtnRefrescar
        // 
        BtnRefrescar.BackColor = Color.FromArgb(46, 134, 222);
        BtnRefrescar.FlatAppearance.BorderSize = 0;
        BtnRefrescar.FlatStyle = FlatStyle.Flat;
        BtnRefrescar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        BtnRefrescar.ForeColor = Color.White;
        BtnRefrescar.Location = new Point(870, 17);
        BtnRefrescar.Name = "BtnRefrescar";
        BtnRefrescar.Size = new Size(95, 30);
        BtnRefrescar.TabIndex = 4;
        BtnRefrescar.Text = "Actualizar";
        BtnRefrescar.UseVisualStyleBackColor = false;
        // 
        // CboTurnos
        // 
        CboTurnos.DropDownStyle = ComboBoxStyle.DropDownList;
        CboTurnos.Font = new Font("Segoe UI", 9.5F);
        CboTurnos.FormattingEnabled = true;
        CboTurnos.Location = new Point(365, 18);
        CboTurnos.Name = "CboTurnos";
        CboTurnos.Size = new Size(490, 29);
        CboTurnos.TabIndex = 3;
        // 
        // lblSeleccionarTurno
        // 
        lblSeleccionarTurno.AutoSize = true;
        lblSeleccionarTurno.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblSeleccionarTurno.ForeColor = Color.FromArgb(27, 42, 74);
        lblSeleccionarTurno.Location = new Point(225, 22);
        lblSeleccionarTurno.Name = "lblSeleccionarTurno";
        lblSeleccionarTurno.Size = new Size(134, 21);
        lblSeleccionarTurno.TabIndex = 2;
        lblSeleccionarTurno.Text = "Turno / Paciente:";
        // 
        // DtpFecha
        // 
        DtpFecha.Font = new Font("Segoe UI", 9.5F);
        DtpFecha.Format = DateTimePickerFormat.Short;
        DtpFecha.Location = new Point(80, 18);
        DtpFecha.Name = "DtpFecha";
        DtpFecha.Size = new Size(130, 29);
        DtpFecha.TabIndex = 1;
        // 
        // lblFecha
        // 
        lblFecha.AutoSize = true;
        lblFecha.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblFecha.ForeColor = Color.FromArgb(27, 42, 74);
        lblFecha.Location = new Point(20, 22);
        lblFecha.Name = "lblFecha";
        lblFecha.Size = new Size(58, 21);
        lblFecha.TabIndex = 0;
        lblFecha.Text = "Fecha:";
        // 
        // pnlDetalleTurno
        // 
        pnlDetalleTurno.BackColor = Color.FromArgb(235, 243, 253);
        pnlDetalleTurno.BorderStyle = BorderStyle.FixedSingle;
        pnlDetalleTurno.Controls.Add(lblInfoPaciente);
        pnlDetalleTurno.Dock = DockStyle.Top;
        pnlDetalleTurno.Location = new Point(0, 140);
        pnlDetalleTurno.Name = "pnlDetalleTurno";
        pnlDetalleTurno.Padding = new Padding(15, 10, 15, 10);
        pnlDetalleTurno.Size = new Size(1000, 48);
        pnlDetalleTurno.TabIndex = 2;
        // 
        // lblInfoPaciente
        // 
        lblInfoPaciente.Dock = DockStyle.Fill;
        lblInfoPaciente.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblInfoPaciente.ForeColor = Color.FromArgb(27, 42, 74);
        lblInfoPaciente.Location = new Point(15, 10);
        lblInfoPaciente.Name = "lblInfoPaciente";
        lblInfoPaciente.Size = new Size(968, 26);
        lblInfoPaciente.TabIndex = 0;
        lblInfoPaciente.Text = "Seleccione un turno para comenzar la atencion clinica.";
        lblInfoPaciente.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlFormulario
        // 
        pnlFormulario.AutoScroll = true;
        pnlFormulario.BackColor = Color.White;
        pnlFormulario.Controls.Add(LblMensaje);
        pnlFormulario.Controls.Add(BtnVerHistorial);
        pnlFormulario.Controls.Add(BtnBorrarRegistro);
        pnlFormulario.Controls.Add(BtnLimpiar);
        pnlFormulario.Controls.Add(BtnGuardar);
        pnlFormulario.Controls.Add(TxtReceta);
        pnlFormulario.Controls.Add(lblReceta);
        pnlFormulario.Controls.Add(TxtDiagnostico);
        pnlFormulario.Controls.Add(lblDiagnostico);
        pnlFormulario.Controls.Add(TxtMotivo);
        pnlFormulario.Controls.Add(lblMotivo);
        pnlFormulario.Controls.Add(CboTipoActividad);
        pnlFormulario.Controls.Add(lblTipoActividad);
        pnlFormulario.Dock = DockStyle.Fill;
        pnlFormulario.Location = new Point(0, 188);
        pnlFormulario.Name = "pnlFormulario";
        pnlFormulario.Padding = new Padding(30, 20, 30, 20);
        pnlFormulario.Size = new Size(1000, 462);
        pnlFormulario.TabIndex = 3;
        // 
        // LblMensaje
        // 
        LblMensaje.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        LblMensaje.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        LblMensaje.Location = new Point(30, 420);
        LblMensaje.Name = "LblMensaje";
        LblMensaje.Size = new Size(940, 28);
        LblMensaje.TabIndex = 12;
        // 
        // BtnVerHistorial
        // 
        BtnVerHistorial.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnVerHistorial.BackColor = Color.FromArgb(27, 42, 74);
        BtnVerHistorial.FlatAppearance.BorderSize = 0;
        BtnVerHistorial.FlatStyle = FlatStyle.Flat;
        BtnVerHistorial.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnVerHistorial.ForeColor = Color.White;
        BtnVerHistorial.Location = new Point(715, 375);
        BtnVerHistorial.Name = "BtnVerHistorial";
        BtnVerHistorial.Size = new Size(150, 38);
        BtnVerHistorial.TabIndex = 10;
        BtnVerHistorial.Text = "Ver Historial";
        BtnVerHistorial.UseVisualStyleBackColor = false;
        // 
        // BtnBorrarRegistro
        // 
        BtnBorrarRegistro.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        BtnBorrarRegistro.BackColor = Color.FromArgb(231, 76, 60);
        BtnBorrarRegistro.FlatAppearance.BorderSize = 0;
        BtnBorrarRegistro.FlatStyle = FlatStyle.Flat;
        BtnBorrarRegistro.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnBorrarRegistro.ForeColor = Color.White;
        BtnBorrarRegistro.Location = new Point(230, 375);
        BtnBorrarRegistro.Name = "BtnBorrarRegistro";
        BtnBorrarRegistro.Size = new Size(150, 38);
        BtnBorrarRegistro.TabIndex = 9;
        BtnBorrarRegistro.Text = "Borrar Registro";
        BtnBorrarRegistro.UseVisualStyleBackColor = false;
        // 
        // BtnLimpiar
        // 
        BtnLimpiar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnLimpiar.BackColor = Color.FromArgb(127, 140, 141);
        BtnLimpiar.FlatAppearance.BorderSize = 0;
        BtnLimpiar.FlatStyle = FlatStyle.Flat;
        BtnLimpiar.Font = new Font("Segoe UI", 9.5F);
        BtnLimpiar.ForeColor = Color.White;
        BtnLimpiar.Location = new Point(875, 375);
        BtnLimpiar.Name = "BtnLimpiar";
        BtnLimpiar.Size = new Size(95, 38);
        BtnLimpiar.TabIndex = 11;
        BtnLimpiar.Text = "Limpiar";
        BtnLimpiar.UseVisualStyleBackColor = false;
        // 
        // BtnGuardar
        // 
        BtnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        BtnGuardar.BackColor = Color.FromArgb(39, 174, 96);
        BtnGuardar.FlatAppearance.BorderSize = 0;
        BtnGuardar.FlatStyle = FlatStyle.Flat;
        BtnGuardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        BtnGuardar.ForeColor = Color.White;
        BtnGuardar.Location = new Point(30, 375);
        BtnGuardar.Name = "BtnGuardar";
        BtnGuardar.Size = new Size(185, 38);
        BtnGuardar.TabIndex = 8;
        BtnGuardar.Text = "Guardar Atencion";
        BtnGuardar.UseVisualStyleBackColor = false;
        // 
        // TxtReceta
        // 
        TxtReceta.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        TxtReceta.Font = new Font("Segoe UI", 9.5F);
        TxtReceta.Location = new Point(30, 275);
        TxtReceta.Multiline = true;
        TxtReceta.Name = "TxtReceta";
        TxtReceta.ScrollBars = ScrollBars.Vertical;
        TxtReceta.Size = new Size(940, 75);
        TxtReceta.TabIndex = 7;
        // 
        // lblReceta
        // 
        lblReceta.AutoSize = true;
        lblReceta.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblReceta.ForeColor = Color.FromArgb(50, 60, 75);
        lblReceta.Location = new Point(30, 252);
        lblReceta.Name = "lblReceta";
        lblReceta.Size = new Size(309, 21);
        lblReceta.TabIndex = 6;
        lblReceta.Text = "Prescripcion / Receta de Medicamentos:";
        // 
        // TxtDiagnostico
        // 
        TxtDiagnostico.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        TxtDiagnostico.Font = new Font("Segoe UI", 9.5F);
        TxtDiagnostico.Location = new Point(30, 165);
        TxtDiagnostico.Multiline = true;
        TxtDiagnostico.Name = "TxtDiagnostico";
        TxtDiagnostico.ScrollBars = ScrollBars.Vertical;
        TxtDiagnostico.Size = new Size(940, 75);
        TxtDiagnostico.TabIndex = 5;
        // 
        // lblDiagnostico
        // 
        lblDiagnostico.AutoSize = true;
        lblDiagnostico.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblDiagnostico.ForeColor = Color.FromArgb(50, 60, 75);
        lblDiagnostico.Location = new Point(30, 142);
        lblDiagnostico.Name = "lblDiagnostico";
        lblDiagnostico.Size = new Size(300, 21);
        lblDiagnostico.TabIndex = 4;
        lblDiagnostico.Text = "Diagnostico / Procedimiento Clinico:";
        // 
        // TxtMotivo
        // 
        TxtMotivo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        TxtMotivo.Font = new Font("Segoe UI", 9.5F);
        TxtMotivo.Location = new Point(30, 65);
        TxtMotivo.Multiline = true;
        TxtMotivo.Name = "TxtMotivo";
        TxtMotivo.ScrollBars = ScrollBars.Vertical;
        TxtMotivo.Size = new Size(940, 65);
        TxtMotivo.TabIndex = 3;
        // 
        // lblMotivo
        // 
        lblMotivo.AutoSize = true;
        lblMotivo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblMotivo.ForeColor = Color.FromArgb(50, 60, 75);
        lblMotivo.Location = new Point(30, 42);
        lblMotivo.Name = "lblMotivo";
        lblMotivo.Size = new Size(198, 21);
        lblMotivo.TabIndex = 2;
        lblMotivo.Text = "Motivo de Consulta (*):";
        // 
        // CboTipoActividad
        // 
        CboTipoActividad.DropDownStyle = ComboBoxStyle.DropDownList;
        CboTipoActividad.Font = new Font("Segoe UI", 9.5F);
        CboTipoActividad.FormattingEnabled = true;
        CboTipoActividad.Location = new Point(190, 8);
        CboTipoActividad.Name = "CboTipoActividad";
        CboTipoActividad.Size = new Size(400, 29);
        CboTipoActividad.TabIndex = 1;
        // 
        // lblTipoActividad
        // 
        lblTipoActividad.AutoSize = true;
        lblTipoActividad.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblTipoActividad.ForeColor = Color.FromArgb(50, 60, 75);
        lblTipoActividad.Location = new Point(30, 12);
        lblTipoActividad.Name = "lblTipoActividad";
        lblTipoActividad.Size = new Size(153, 21);
        lblTipoActividad.TabIndex = 0;
        lblTipoActividad.Text = "Tipo de Actividad:";
        // 
        // FormRegistrarActividad
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(1000, 650);
        Controls.Add(pnlFormulario);
        Controls.Add(pnlDetalleTurno);
        Controls.Add(pnlSeleccionTurno);
        Controls.Add(pnlHeader);
        Name = "FormRegistrarActividad";
        Text = "Registro de Atencion Clinica";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlSeleccionTurno.ResumeLayout(false);
        pnlSeleccionTurno.PerformLayout();
        pnlDetalleTurno.ResumeLayout(false);
        pnlFormulario.ResumeLayout(false);
        pnlFormulario.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Panel pnlHeader;
    private Label lblTitulo;
    private Label lblMedicoInfo;
    private Button BtnVolverAgenda;
    private Panel pnlSeleccionTurno;
    private Label lblFecha;
    private DateTimePicker DtpFecha;
    private Label lblSeleccionarTurno;
    private ComboBox CboTurnos;
    private Button BtnRefrescar;
    private Panel pnlDetalleTurno;
    private Label lblInfoPaciente;
    private Panel pnlFormulario;
    private Label lblTipoActividad;
    private ComboBox CboTipoActividad;
    private Label lblMotivo;
    private TextBox TxtMotivo;
    private Label lblDiagnostico;
    private TextBox TxtDiagnostico;
    private Label lblReceta;
    private TextBox TxtReceta;
    private Button BtnGuardar;
    private Button BtnBorrarRegistro;
    private Button BtnVerHistorial;
    private Button BtnLimpiar;
    private Label LblMensaje;
}