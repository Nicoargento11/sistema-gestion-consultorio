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
        pnlFormulario.Name = "pnlFormulario";
        pnlFormulario.Size = new Size(1000, 150);
        pnlFormulario.TabIndex = 0;
        //
        // LblMensaje
        //
        LblMensaje.AutoSize = true;
        LblMensaje.Font = new Font("Segoe UI", 9F);
        LblMensaje.Location = new Point(500, 100);
        LblMensaje.MaximumSize = new Size(400, 0);
        LblMensaje.Name = "LblMensaje";
        LblMensaje.Size = new Size(0, 25);
        LblMensaje.TabIndex = 10;
        //
        // BtnEliminar
        //
        BtnEliminar.BackColor = Color.FromArgb(200, 60, 60);
        BtnEliminar.FlatAppearance.BorderSize = 0;
        BtnEliminar.FlatStyle = FlatStyle.Flat;
        BtnEliminar.Font = new Font("Segoe UI", 9.5F);
        BtnEliminar.ForeColor = Color.White;
        BtnEliminar.Location = new Point(240, 100);
        BtnEliminar.Name = "BtnEliminar";
        BtnEliminar.Size = new Size(100, 34);
        BtnEliminar.TabIndex = 6;
        BtnEliminar.Text = "Eliminar";
        BtnEliminar.UseVisualStyleBackColor = false;
        BtnEliminar.Click += BtnEliminar_Click;
        //
        // BtnGuardar
        //
        BtnGuardar.BackColor = Color.FromArgb(46, 134, 222);
        BtnGuardar.FlatAppearance.BorderSize = 0;
        BtnGuardar.FlatStyle = FlatStyle.Flat;
        BtnGuardar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnGuardar.ForeColor = Color.White;
        BtnGuardar.Location = new Point(130, 100);
        BtnGuardar.Name = "BtnGuardar";
        BtnGuardar.Size = new Size(100, 34);
        BtnGuardar.TabIndex = 5;
        BtnGuardar.Text = "Guardar";
        BtnGuardar.UseVisualStyleBackColor = false;
        BtnGuardar.Click += BtnGuardar_Click;
        //
        // BtnNuevo
        //
        BtnNuevo.BackColor = Color.FromArgb(120, 130, 145);
        BtnNuevo.FlatAppearance.BorderSize = 0;
        BtnNuevo.FlatStyle = FlatStyle.Flat;
        BtnNuevo.Font = new Font("Segoe UI", 9.5F);
        BtnNuevo.ForeColor = Color.White;
        BtnNuevo.Location = new Point(20, 100);
        BtnNuevo.Name = "BtnNuevo";
        BtnNuevo.Size = new Size(100, 34);
        BtnNuevo.TabIndex = 4;
        BtnNuevo.Text = "Nuevo";
        BtnNuevo.UseVisualStyleBackColor = false;
        BtnNuevo.Click += BtnNuevo_Click;
        //
        // DtpHoraInicio
        //
        DtpHoraInicio.Font = new Font("Segoe UI", 10F);
        DtpHoraInicio.Format = DateTimePickerFormat.Custom;
        DtpHoraInicio.CustomFormat = "HH:mm";
        DtpHoraInicio.ShowUpDown = true;
        DtpHoraInicio.Location = new Point(460, 38);
        DtpHoraInicio.Name = "DtpHoraInicio";
        DtpHoraInicio.Size = new Size(100, 33);
        DtpHoraInicio.TabIndex = 2;
        //
        // lblHoraInicio
        //
        lblHoraInicio.AutoSize = true;
        lblHoraInicio.Font = new Font("Segoe UI", 9F);
        lblHoraInicio.Location = new Point(460, 15);
        lblHoraInicio.Name = "lblHoraInicio";
        lblHoraInicio.Size = new Size(66, 25);
        lblHoraInicio.TabIndex = 9;
        lblHoraInicio.Text = "Desde";
        //
        // DtpHoraFin
        //
        DtpHoraFin.Font = new Font("Segoe UI", 10F);
        DtpHoraFin.Format = DateTimePickerFormat.Custom;
        DtpHoraFin.CustomFormat = "HH:mm";
        DtpHoraFin.ShowUpDown = true;
        DtpHoraFin.Location = new Point(575, 38);
        DtpHoraFin.Name = "DtpHoraFin";
        DtpHoraFin.Size = new Size(100, 33);
        DtpHoraFin.TabIndex = 3;
        //
        // lblHoraFin
        //
        lblHoraFin.AutoSize = true;
        lblHoraFin.Font = new Font("Segoe UI", 9F);
        lblHoraFin.Location = new Point(575, 15);
        lblHoraFin.Name = "lblHoraFin";
        lblHoraFin.Size = new Size(66, 25);
        lblHoraFin.TabIndex = 11;
        lblHoraFin.Text = "Hasta";
        //
        // CboDiaSemana
        //
        CboDiaSemana.DropDownStyle = ComboBoxStyle.DropDownList;
        CboDiaSemana.Font = new Font("Segoe UI", 10F);
        CboDiaSemana.Location = new Point(260, 38);
        CboDiaSemana.Name = "CboDiaSemana";
        CboDiaSemana.Size = new Size(180, 35);
        CboDiaSemana.TabIndex = 1;
        //
        // lblDiaSemana
        //
        lblDiaSemana.AutoSize = true;
        lblDiaSemana.Font = new Font("Segoe UI", 9F);
        lblDiaSemana.Location = new Point(260, 15);
        lblDiaSemana.Name = "lblDiaSemana";
        lblDiaSemana.Size = new Size(120, 25);
        lblDiaSemana.TabIndex = 8;
        lblDiaSemana.Text = "Dia de la semana";
        //
        // CboMedico
        //
        CboMedico.DropDownStyle = ComboBoxStyle.DropDownList;
        CboMedico.Font = new Font("Segoe UI", 10F);
        CboMedico.Location = new Point(20, 38);
        CboMedico.Name = "CboMedico";
        CboMedico.Size = new Size(220, 35);
        CboMedico.TabIndex = 0;
        //
        // lblMedico
        //
        lblMedico.AutoSize = true;
        lblMedico.Font = new Font("Segoe UI", 9F);
        lblMedico.Location = new Point(20, 15);
        lblMedico.Name = "lblMedico";
        lblMedico.Size = new Size(66, 25);
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
        DgvAgenda.Location = new Point(0, 150);
        DgvAgenda.MultiSelect = false;
        DgvAgenda.Name = "DgvAgenda";
        DgvAgenda.ReadOnly = true;
        DgvAgenda.RowHeadersWidth = 62;
        DgvAgenda.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvAgenda.Size = new Size(1000, 450);
        DgvAgenda.TabIndex = 1;
        DgvAgenda.SelectionChanged += DgvAgenda_SelectionChanged;
        //
        // FormConfigurarAgenda
        //
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 600);
        Controls.Add(DgvAgenda);
        Controls.Add(pnlFormulario);
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
