namespace SGC.UI;

partial class FormConfigurarAgenda
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
        pnlFormulario = new Panel();
        LblMensaje = new Label();
        BtnEliminar = new Button();
        BtnGuardar = new Button();
        BtnNuevo = new Button();
        DtpHoraFin = new DateTimePicker();
        lblHoraFin = new Label();
        DtpHoraInicio = new DateTimePicker();
        lblHoraInicio = new Label();
        CboDiaSemana = new ComboBox();
        lblDiaSemana = new Label();
        CboMedico = new ComboBox();
        lblMedico = new Label();
        DgvAgenda = new DataGridView();
        pnlFormulario.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvAgenda).BeginInit();
        SuspendLayout();
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
        pnlFormulario.Controls.Add(CboDiaSemana);
        pnlFormulario.Controls.Add(lblDiaSemana);
        pnlFormulario.Controls.Add(CboMedico);
        pnlFormulario.Controls.Add(lblMedico);
        pnlFormulario.Dock = DockStyle.Top;
        pnlFormulario.Location = new Point(0, 0);
        pnlFormulario.Margin = new Padding(2, 2, 2, 2);
        pnlFormulario.Name = "pnlFormulario";
        pnlFormulario.Size = new Size(800, 120);
        pnlFormulario.TabIndex = 0;
        // 
        // LblMensaje
        // 
        LblMensaje.AutoSize = true;
        LblMensaje.Font = new Font("Segoe UI", 9F);
        LblMensaje.Location = new Point(400, 80);
        LblMensaje.Margin = new Padding(2, 0, 2, 0);
        LblMensaje.MaximumSize = new Size(320, 0);
        LblMensaje.Name = "LblMensaje";
        LblMensaje.Size = new Size(0, 20);
        LblMensaje.TabIndex = 10;
        // 
        // BtnEliminar
        // 
        BtnEliminar.BackColor = Color.FromArgb(231, 76, 60);
        BtnEliminar.FlatAppearance.BorderSize = 0;
        BtnEliminar.FlatStyle = FlatStyle.Flat;
        BtnEliminar.Font = new Font("Segoe UI", 9.5F);
        BtnEliminar.ForeColor = Color.White;
        BtnEliminar.Location = new Point(192, 80);
        BtnEliminar.Margin = new Padding(2, 2, 2, 2);
        BtnEliminar.Name = "BtnEliminar";
        BtnEliminar.Size = new Size(80, 27);
        BtnEliminar.TabIndex = 6;
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
        BtnGuardar.Location = new Point(104, 80);
        BtnGuardar.Margin = new Padding(2, 2, 2, 2);
        BtnGuardar.Name = "BtnGuardar";
        BtnGuardar.Size = new Size(80, 27);
        BtnGuardar.TabIndex = 5;
        BtnGuardar.Text = "Guardar";
        BtnGuardar.UseVisualStyleBackColor = false;
        BtnGuardar.Click += BtnGuardar_Click;
        // 
        // BtnNuevo
        // 
        BtnNuevo.BackColor = Color.FromArgb(46, 134, 222);
        BtnNuevo.FlatAppearance.BorderSize = 0;
        BtnNuevo.FlatStyle = FlatStyle.Flat;
        BtnNuevo.Font = new Font("Segoe UI", 9.5F);
        BtnNuevo.ForeColor = Color.White;
        BtnNuevo.Location = new Point(16, 80);
        BtnNuevo.Margin = new Padding(2, 2, 2, 2);
        BtnNuevo.Name = "BtnNuevo";
        BtnNuevo.Size = new Size(80, 27);
        BtnNuevo.TabIndex = 4;
        BtnNuevo.Text = "Nuevo";
        BtnNuevo.UseVisualStyleBackColor = false;
        BtnNuevo.Click += BtnNuevo_Click;
        // 
        // DtpHoraFin
        // 
        DtpHoraFin.CustomFormat = "HH:mm";
        DtpHoraFin.Font = new Font("Segoe UI", 10F);
        DtpHoraFin.Format = DateTimePickerFormat.Custom;
        DtpHoraFin.Location = new Point(460, 30);
        DtpHoraFin.Margin = new Padding(2, 2, 2, 2);
        DtpHoraFin.Name = "DtpHoraFin";
        DtpHoraFin.ShowUpDown = true;
        DtpHoraFin.Size = new Size(81, 30);
        DtpHoraFin.TabIndex = 3;
        // 
        // lblHoraFin
        // 
        lblHoraFin.AutoSize = true;
        lblHoraFin.Font = new Font("Segoe UI", 9F);
        lblHoraFin.Location = new Point(460, 12);
        lblHoraFin.Margin = new Padding(2, 0, 2, 0);
        lblHoraFin.Name = "lblHoraFin";
        lblHoraFin.Size = new Size(47, 20);
        lblHoraFin.TabIndex = 11;
        lblHoraFin.Text = "Hasta";
        // 
        // DtpHoraInicio
        // 
        DtpHoraInicio.CustomFormat = "HH:mm";
        DtpHoraInicio.Font = new Font("Segoe UI", 10F);
        DtpHoraInicio.Format = DateTimePickerFormat.Custom;
        DtpHoraInicio.Location = new Point(368, 30);
        DtpHoraInicio.Margin = new Padding(2, 2, 2, 2);
        DtpHoraInicio.Name = "DtpHoraInicio";
        DtpHoraInicio.ShowUpDown = true;
        DtpHoraInicio.Size = new Size(81, 30);
        DtpHoraInicio.TabIndex = 2;
        // 
        // lblHoraInicio
        // 
        lblHoraInicio.AutoSize = true;
        lblHoraInicio.Font = new Font("Segoe UI", 9F);
        lblHoraInicio.Location = new Point(368, 12);
        lblHoraInicio.Margin = new Padding(2, 0, 2, 0);
        lblHoraInicio.Name = "lblHoraInicio";
        lblHoraInicio.Size = new Size(51, 20);
        lblHoraInicio.TabIndex = 9;
        lblHoraInicio.Text = "Desde";
        // 
        // CboDiaSemana
        // 
        CboDiaSemana.DropDownStyle = ComboBoxStyle.DropDownList;
        CboDiaSemana.Font = new Font("Segoe UI", 10F);
        CboDiaSemana.Location = new Point(208, 30);
        CboDiaSemana.Margin = new Padding(2, 2, 2, 2);
        CboDiaSemana.Name = "CboDiaSemana";
        CboDiaSemana.Size = new Size(145, 31);
        CboDiaSemana.TabIndex = 1;
        // 
        // lblDiaSemana
        // 
        lblDiaSemana.AutoSize = true;
        lblDiaSemana.Font = new Font("Segoe UI", 9F);
        lblDiaSemana.Location = new Point(208, 12);
        lblDiaSemana.Margin = new Padding(2, 0, 2, 0);
        lblDiaSemana.Name = "lblDiaSemana";
        lblDiaSemana.Size = new Size(124, 20);
        lblDiaSemana.TabIndex = 8;
        lblDiaSemana.Text = "Dia de la semana";
        // 
        // CboMedico
        // 
        CboMedico.DropDownStyle = ComboBoxStyle.DropDownList;
        CboMedico.Font = new Font("Segoe UI", 10F);
        CboMedico.Location = new Point(16, 30);
        CboMedico.Margin = new Padding(2, 2, 2, 2);
        CboMedico.Name = "CboMedico";
        CboMedico.Size = new Size(177, 31);
        CboMedico.TabIndex = 0;
        // 
        // lblMedico
        // 
        lblMedico.AutoSize = true;
        lblMedico.Font = new Font("Segoe UI", 9F);
        lblMedico.Location = new Point(16, 12);
        lblMedico.Margin = new Padding(2, 0, 2, 0);
        lblMedico.Name = "lblMedico";
        lblMedico.Size = new Size(59, 20);
        lblMedico.TabIndex = 7;
        lblMedico.Text = "Medico";
        // 
        // DgvAgenda
        // 
        DgvAgenda.AllowUserToAddRows = false;
        DgvAgenda.BackgroundColor = Color.White;
        DgvAgenda.ColumnHeadersHeight = 34;
        DgvAgenda.Dock = DockStyle.Fill;
        DgvAgenda.Font = new Font("Segoe UI", 9.5F);
        DgvAgenda.Location = new Point(0, 120);
        DgvAgenda.Margin = new Padding(2, 2, 2, 2);
        DgvAgenda.MultiSelect = false;
        DgvAgenda.Name = "DgvAgenda";
        DgvAgenda.ReadOnly = true;
        DgvAgenda.RowHeadersWidth = 62;
        DgvAgenda.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvAgenda.Size = new Size(800, 360);
        DgvAgenda.TabIndex = 1;
        DgvAgenda.SelectionChanged += DgvAgenda_SelectionChanged;
        // 
        // FormConfigurarAgenda
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 480);
        Controls.Add(DgvAgenda);
        Controls.Add(pnlFormulario);
        Margin = new Padding(2, 2, 2, 2);
        Name = "FormConfigurarAgenda";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Configurar Agenda de Medicos";
        pnlFormulario.ResumeLayout(false);
        pnlFormulario.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)DgvAgenda).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlFormulario;
    private Label lblMedico;
    private ComboBox CboMedico;
    private Label lblDiaSemana;
    private ComboBox CboDiaSemana;
    private Label lblHoraInicio;
    private DateTimePicker DtpHoraInicio;
    private Label lblHoraFin;
    private DateTimePicker DtpHoraFin;
    private Button BtnNuevo;
    private Button BtnGuardar;
    private Button BtnEliminar;
    private Label LblMensaje;
    private DataGridView DgvAgenda;
}
