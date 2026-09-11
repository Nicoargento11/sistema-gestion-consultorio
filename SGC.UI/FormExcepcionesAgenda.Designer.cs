namespace SGC.UI;

partial class FormExcepcionesAgenda
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
        pnlHeader = new Panel();
        lblSubtitulo = new Label();
        lblTitulo = new Label();
        pnlFormulario = new Panel();
        LblMensaje = new Label();
        BtnEliminar = new Button();
        BtnGuardar = new Button();
        BtnNuevo = new Button();
        DtpHoraFin = new DateTimePicker();
        lblHoraFin = new Label();
        DtpHoraInicio = new DateTimePicker();
        lblHoraInicio = new Label();
        TxtMotivo = new TextBox();
        lblMotivo = new Label();
        CboTipo = new ComboBox();
        lblTipo = new Label();
        DtpFecha = new DateTimePicker();
        lblFecha = new Label();
        CboMedico = new ComboBox();
        lblMedico = new Label();
        DgvExcepciones = new DataGridView();
        pnlHeader.SuspendLayout();
        pnlFormulario.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvExcepciones).BeginInit();
        SuspendLayout();
        //
        // pnlHeader
        //
        pnlHeader.BackColor = Color.FromArgb(27, 42, 74);
        pnlHeader.Controls.Add(lblSubtitulo);
        pnlHeader.Controls.Add(lblTitulo);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Padding = new Padding(20, 10, 20, 10);
        pnlHeader.Size = new Size(1000, 75);
        pnlHeader.TabIndex = 0;
        //
        // lblSubtitulo
        //
        lblSubtitulo.AutoSize = true;
        lblSubtitulo.Font = new Font("Segoe UI", 10F);
        lblSubtitulo.ForeColor = Color.FromArgb(180, 205, 235);
        lblSubtitulo.Location = new Point(20, 42);
        lblSubtitulo.Name = "lblSubtitulo";
        lblSubtitulo.Size = new Size(395, 23);
        lblSubtitulo.TabIndex = 1;
        lblSubtitulo.Text = "Vacaciones y ausencias puntuales por medico";
        //
        // lblTitulo
        //
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(18, 10);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(230, 32);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Excepciones de Agenda";
        //
        // pnlFormulario
        //
        pnlFormulario.BackColor = Color.FromArgb(245, 246, 250);
        pnlFormulario.Controls.Add(LblMensaje);
        pnlFormulario.Controls.Add(BtnEliminar);
        pnlFormulario.Controls.Add(BtnGuardar);
        pnlFormulario.Controls.Add(BtnNuevo);
        pnlFormulario.Controls.Add(DtpHoraFin);
        pnlFormulario.Controls.Add(lblHoraFin);
        pnlFormulario.Controls.Add(DtpHoraInicio);
        pnlFormulario.Controls.Add(lblHoraInicio);
        pnlFormulario.Controls.Add(TxtMotivo);
        pnlFormulario.Controls.Add(lblMotivo);
        pnlFormulario.Controls.Add(CboTipo);
        pnlFormulario.Controls.Add(lblTipo);
        pnlFormulario.Controls.Add(DtpFecha);
        pnlFormulario.Controls.Add(lblFecha);
        pnlFormulario.Controls.Add(CboMedico);
        pnlFormulario.Controls.Add(lblMedico);
        pnlFormulario.Dock = DockStyle.Top;
        pnlFormulario.Location = new Point(0, 75);
        pnlFormulario.Name = "pnlFormulario";
        pnlFormulario.Padding = new Padding(20, 10, 20, 10);
        pnlFormulario.Size = new Size(1000, 170);
        pnlFormulario.TabIndex = 1;
        //
        // LblMensaje
        //
        LblMensaje.AutoSize = true;
        LblMensaje.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        LblMensaje.Location = new Point(480, 132);
        LblMensaje.MaximumSize = new Size(400, 0);
        LblMensaje.Name = "LblMensaje";
        LblMensaje.Size = new Size(0, 21);
        LblMensaje.TabIndex = 16;
        //
        // BtnEliminar
        //
        BtnEliminar.BackColor = Color.FromArgb(231, 76, 60);
        BtnEliminar.FlatAppearance.BorderSize = 0;
        BtnEliminar.FlatStyle = FlatStyle.Flat;
        BtnEliminar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnEliminar.ForeColor = Color.White;
        BtnEliminar.Location = new Point(240, 130);
        BtnEliminar.Name = "BtnEliminar";
        BtnEliminar.Size = new Size(100, 32);
        BtnEliminar.TabIndex = 8;
        BtnEliminar.Text = "Eliminar";
        BtnEliminar.UseVisualStyleBackColor = false;
        BtnEliminar.Click += BtnEliminar_Click;
        //
        // BtnGuardar
        //
        BtnGuardar.BackColor = Color.FromArgb(39, 174, 96);
        BtnGuardar.FlatAppearance.BorderSize = 0;
        BtnGuardar.FlatStyle = FlatStyle.Flat;
        BtnGuardar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnGuardar.ForeColor = Color.White;
        BtnGuardar.Location = new Point(130, 130);
        BtnGuardar.Name = "BtnGuardar";
        BtnGuardar.Size = new Size(100, 32);
        BtnGuardar.TabIndex = 7;
        BtnGuardar.Text = "Guardar";
        BtnGuardar.UseVisualStyleBackColor = false;
        BtnGuardar.Click += BtnGuardar_Click;
        //
        // BtnNuevo
        //
        BtnNuevo.BackColor = Color.FromArgb(46, 134, 222);
        BtnNuevo.FlatAppearance.BorderSize = 0;
        BtnNuevo.FlatStyle = FlatStyle.Flat;
        BtnNuevo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnNuevo.ForeColor = Color.White;
        BtnNuevo.Location = new Point(20, 130);
        BtnNuevo.Name = "BtnNuevo";
        BtnNuevo.Size = new Size(100, 32);
        BtnNuevo.TabIndex = 6;
        BtnNuevo.Text = "+ Nuevo";
        BtnNuevo.UseVisualStyleBackColor = false;
        BtnNuevo.Click += BtnNuevo_Click;
        //
        // DtpHoraFin
        //
        DtpHoraFin.Font = new Font("Segoe UI", 9.5F);
        DtpHoraFin.Format = DateTimePickerFormat.Custom;
        DtpHoraFin.CustomFormat = "HH:mm";
        DtpHoraFin.ShowUpDown = true;
        DtpHoraFin.Location = new Point(565, 80);
        DtpHoraFin.Name = "DtpHoraFin";
        DtpHoraFin.Size = new Size(100, 29);
        DtpHoraFin.TabIndex = 5;
        //
        // lblHoraFin
        //
        lblHoraFin.AutoSize = true;
        lblHoraFin.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblHoraFin.ForeColor = Color.FromArgb(50, 60, 75);
        lblHoraFin.Location = new Point(565, 60);
        lblHoraFin.Name = "lblHoraFin";
        lblHoraFin.Size = new Size(66, 20);
        lblHoraFin.TabIndex = 15;
        lblHoraFin.Text = "Hasta";
        //
        // DtpHoraInicio
        //
        DtpHoraInicio.Font = new Font("Segoe UI", 9.5F);
        DtpHoraInicio.Format = DateTimePickerFormat.Custom;
        DtpHoraInicio.CustomFormat = "HH:mm";
        DtpHoraInicio.ShowUpDown = true;
        DtpHoraInicio.Location = new Point(450, 80);
        DtpHoraInicio.Name = "DtpHoraInicio";
        DtpHoraInicio.Size = new Size(100, 29);
        DtpHoraInicio.TabIndex = 4;
        //
        // lblHoraInicio
        //
        lblHoraInicio.AutoSize = true;
        lblHoraInicio.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblHoraInicio.ForeColor = Color.FromArgb(50, 60, 75);
        lblHoraInicio.Location = new Point(450, 60);
        lblHoraInicio.Name = "lblHoraInicio";
        lblHoraInicio.Size = new Size(66, 20);
        lblHoraInicio.TabIndex = 14;
        lblHoraInicio.Text = "Desde";
        //
        // TxtMotivo
        //
        TxtMotivo.Font = new Font("Segoe UI", 9.5F);
        TxtMotivo.Location = new Point(20, 80);
        TxtMotivo.Name = "TxtMotivo";
        TxtMotivo.PlaceholderText = "Ej: Vacaciones, congreso...";
        TxtMotivo.Size = new Size(400, 29);
        TxtMotivo.TabIndex = 3;
        //
        // lblMotivo
        //
        lblMotivo.AutoSize = true;
        lblMotivo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMotivo.ForeColor = Color.FromArgb(50, 60, 75);
        lblMotivo.Location = new Point(20, 60);
        lblMotivo.Name = "lblMotivo";
        lblMotivo.Size = new Size(90, 20);
        lblMotivo.TabIndex = 13;
        lblMotivo.Text = "Motivo";
        //
        // CboTipo
        //
        CboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
        CboTipo.Font = new Font("Segoe UI", 9.5F);
        CboTipo.Location = new Point(575, 28);
        CboTipo.Name = "CboTipo";
        CboTipo.Size = new Size(160, 29);
        CboTipo.TabIndex = 2;
        CboTipo.SelectedIndexChanged += CboTipo_SelectedIndexChanged;
        //
        // lblTipo
        //
        lblTipo.AutoSize = true;
        lblTipo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTipo.ForeColor = Color.FromArgb(50, 60, 75);
        lblTipo.Location = new Point(575, 8);
        lblTipo.Name = "lblTipo";
        lblTipo.Size = new Size(90, 20);
        lblTipo.TabIndex = 12;
        lblTipo.Text = "Tipo";
        //
        // DtpFecha
        //
        DtpFecha.Font = new Font("Segoe UI", 9.5F);
        DtpFecha.Format = DateTimePickerFormat.Custom;
        DtpFecha.CustomFormat = "dd/MM/yyyy";
        DtpFecha.Location = new Point(410, 28);
        DtpFecha.Name = "DtpFecha";
        DtpFecha.Size = new Size(150, 29);
        DtpFecha.TabIndex = 1;
        //
        // lblFecha
        //
        lblFecha.AutoSize = true;
        lblFecha.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblFecha.ForeColor = Color.FromArgb(50, 60, 75);
        lblFecha.Location = new Point(410, 8);
        lblFecha.Name = "lblFecha";
        lblFecha.Size = new Size(90, 20);
        lblFecha.TabIndex = 11;
        lblFecha.Text = "Fecha";
        //
        // CboMedico
        //
        CboMedico.DropDownStyle = ComboBoxStyle.DropDownList;
        CboMedico.Font = new Font("Segoe UI", 9.5F);
        CboMedico.Location = new Point(20, 28);
        CboMedico.Name = "CboMedico";
        CboMedico.Size = new Size(380, 29);
        CboMedico.TabIndex = 0;
        //
        // lblMedico
        //
        lblMedico.AutoSize = true;
        lblMedico.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMedico.ForeColor = Color.FromArgb(50, 60, 75);
        lblMedico.Location = new Point(20, 8);
        lblMedico.Name = "lblMedico";
        lblMedico.Size = new Size(90, 20);
        lblMedico.TabIndex = 10;
        lblMedico.Text = "Medico";
        //
        // DgvExcepciones
        //
        DgvExcepciones.AllowUserToAddRows = false;
        DgvExcepciones.AllowUserToDeleteRows = false;
        DgvExcepciones.BackgroundColor = Color.White;
        DgvExcepciones.ColumnHeadersHeight = 34;
        DgvExcepciones.Dock = DockStyle.Fill;
        DgvExcepciones.Font = new Font("Segoe UI", 9.5F);
        DgvExcepciones.Location = new Point(0, 245);
        DgvExcepciones.MultiSelect = false;
        DgvExcepciones.Name = "DgvExcepciones";
        DgvExcepciones.ReadOnly = true;
        DgvExcepciones.RowHeadersVisible = false;
        DgvExcepciones.RowHeadersWidth = 51;
        DgvExcepciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvExcepciones.Size = new Size(1000, 405);
        DgvExcepciones.TabIndex = 2;
        DgvExcepciones.SelectionChanged += DgvExcepciones_SelectionChanged;
        //
        // FormExcepcionesAgenda
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(1000, 650);
        Controls.Add(DgvExcepciones);
        Controls.Add(pnlFormulario);
        Controls.Add(pnlHeader);
        Name = "FormExcepcionesAgenda";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Excepciones de Agenda";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlFormulario.ResumeLayout(false);
        pnlFormulario.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)DgvExcepciones).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlHeader;
    private Label lblTitulo;
    private Label lblSubtitulo;
    private Panel pnlFormulario;
    private Label lblMedico;
    private ComboBox CboMedico;
    private Label lblFecha;
    private DateTimePicker DtpFecha;
    private Label lblTipo;
    private ComboBox CboTipo;
    private Label lblMotivo;
    private TextBox TxtMotivo;
    private Label lblHoraInicio;
    private DateTimePicker DtpHoraInicio;
    private Label lblHoraFin;
    private DateTimePicker DtpHoraFin;
    private Button BtnNuevo;
    private Button BtnGuardar;
    private Button BtnEliminar;
    private Label LblMensaje;
    private DataGridView DgvExcepciones;
}
