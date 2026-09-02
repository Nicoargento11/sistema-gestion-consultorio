namespace SGC.UI;

partial class FormPacientes
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
        CboObraSocial = new ComboBox();
        lblObraSocial = new Label();
        DtpFechaNacimiento = new DateTimePicker();
        lblFechaNacimiento = new Label();
        LblMensaje = new Label();
        BtnEliminar = new Button();
        BtnGuardar = new Button();
        BtnNuevo = new Button();
        TxtTelefono = new TextBox();
        lblTelefono = new Label();
        TxtEmail = new TextBox();
        lblEmail = new Label();
        TxtDni = new TextBox();
        lblDni = new Label();
        TxtApellido = new TextBox();
        lblApellido = new Label();
        TxtNombre = new TextBox();
        lblNombre = new Label();
        DgvPacientes = new DataGridView();
        pnlFormulario.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvPacientes).BeginInit();
        SuspendLayout();
        // 
        // pnlFormulario
        // 
        pnlFormulario.BackColor = Color.FromArgb(245, 246, 250);
        pnlFormulario.Controls.Add(CboObraSocial);
        pnlFormulario.Controls.Add(lblObraSocial);
        pnlFormulario.Controls.Add(DtpFechaNacimiento);
        pnlFormulario.Controls.Add(lblFechaNacimiento);
        pnlFormulario.Controls.Add(LblMensaje);
        pnlFormulario.Controls.Add(BtnEliminar);
        pnlFormulario.Controls.Add(BtnGuardar);
        pnlFormulario.Controls.Add(BtnNuevo);
        pnlFormulario.Controls.Add(TxtTelefono);
        pnlFormulario.Controls.Add(lblTelefono);
        pnlFormulario.Controls.Add(TxtEmail);
        pnlFormulario.Controls.Add(lblEmail);
        pnlFormulario.Controls.Add(TxtDni);
        pnlFormulario.Controls.Add(lblDni);
        pnlFormulario.Controls.Add(TxtApellido);
        pnlFormulario.Controls.Add(lblApellido);
        pnlFormulario.Controls.Add(TxtNombre);
        pnlFormulario.Controls.Add(lblNombre);
        pnlFormulario.Dock = DockStyle.Top;
        pnlFormulario.Location = new Point(0, 0);
        pnlFormulario.Name = "pnlFormulario";
        pnlFormulario.Size = new Size(1000, 260);
        pnlFormulario.TabIndex = 1;
        // 
        // LblMensaje
        // 
        LblMensaje.AutoSize = true;
        LblMensaje.Font = new Font("Segoe UI", 9F);
        LblMensaje.Location = new Point(500, 175);
        LblMensaje.MaximumSize = new Size(400, 0);
        LblMensaje.Name = "LblMensaje";
        LblMensaje.Size = new Size(0, 25);
        LblMensaje.TabIndex = 0;
        //
        // lblFechaNacimiento
        //
        lblFechaNacimiento.AutoSize = true;
        lblFechaNacimiento.Font = new Font("Segoe UI", 9F);
        lblFechaNacimiento.Location = new Point(20, 145);
        lblFechaNacimiento.Name = "lblFechaNacimiento";
        lblFechaNacimiento.Text = "Fecha de nacimiento";
        //
        // DtpFechaNacimiento
        //
        DtpFechaNacimiento.Font = new Font("Segoe UI", 10F);
        DtpFechaNacimiento.Format = DateTimePickerFormat.Short;
        DtpFechaNacimiento.Location = new Point(20, 168);
        DtpFechaNacimiento.MaxDate = DateTime.Today;
        DtpFechaNacimiento.Name = "DtpFechaNacimiento";
        DtpFechaNacimiento.Size = new Size(180, 34);
        DtpFechaNacimiento.TabIndex = 8;
        //
        // lblObraSocial
        //
        lblObraSocial.AutoSize = true;
        lblObraSocial.Font = new Font("Segoe UI", 9F);
        lblObraSocial.Location = new Point(220, 145);
        lblObraSocial.Name = "lblObraSocial";
        lblObraSocial.Text = "Obra social (vacio = Particular)";
        //
        // CboObraSocial
        //
        CboObraSocial.DropDownStyle = ComboBoxStyle.DropDown;
        CboObraSocial.Font = new Font("Segoe UI", 10F);
        CboObraSocial.Items.AddRange(new object[] { "Particular", "OSDE", "Swiss Medical", "Galeno", "IOMA", "PAMI", "IOSFA", "Medife", "Sancor Salud" });
        CboObraSocial.Location = new Point(220, 168);
        CboObraSocial.Name = "CboObraSocial";
        CboObraSocial.Size = new Size(260, 34);
        CboObraSocial.TabIndex = 9;
        //
        // BtnEliminar
        //
        BtnEliminar.BackColor = Color.FromArgb(200, 60, 60);
        BtnEliminar.FlatAppearance.BorderSize = 0;
        BtnEliminar.FlatStyle = FlatStyle.Flat;
        BtnEliminar.Font = new Font("Segoe UI", 9.5F);
        BtnEliminar.ForeColor = Color.White;
        BtnEliminar.Location = new Point(240, 220);
        BtnEliminar.Name = "BtnEliminar";
        BtnEliminar.Size = new Size(100, 34);
        BtnEliminar.TabIndex = 7;
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
        BtnGuardar.Location = new Point(130, 220);
        BtnGuardar.Name = "BtnGuardar";
        BtnGuardar.Size = new Size(100, 34);
        BtnGuardar.TabIndex = 6;
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
        BtnNuevo.Location = new Point(20, 220);
        BtnNuevo.Name = "BtnNuevo";
        BtnNuevo.Size = new Size(100, 34);
        BtnNuevo.TabIndex = 5;
        BtnNuevo.Text = "Nuevo";
        BtnNuevo.UseVisualStyleBackColor = false;
        BtnNuevo.Click += BtnNuevo_Click;
        // 
        // TxtTelefono
        // 
        TxtTelefono.Font = new Font("Segoe UI", 10F);
        TxtTelefono.Location = new Point(295, 103);
        TxtTelefono.Name = "TxtTelefono";
        TxtTelefono.Size = new Size(180, 34);
        TxtTelefono.TabIndex = 4;
        // 
        // lblTelefono
        // 
        lblTelefono.AutoSize = true;
        lblTelefono.Font = new Font("Segoe UI", 9F);
        lblTelefono.Location = new Point(295, 80);
        lblTelefono.Name = "lblTelefono";
        lblTelefono.Size = new Size(79, 25);
        lblTelefono.TabIndex = 5;
        lblTelefono.Text = "Teléfono";
        // 
        // TxtEmail
        // 
        TxtEmail.Font = new Font("Segoe UI", 10F);
        TxtEmail.Location = new Point(20, 103);
        TxtEmail.Name = "TxtEmail";
        TxtEmail.Size = new Size(260, 34);
        TxtEmail.TabIndex = 3;
        // 
        // lblEmail
        // 
        lblEmail.AutoSize = true;
        lblEmail.Font = new Font("Segoe UI", 9F);
        lblEmail.Location = new Point(20, 80);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(54, 25);
        lblEmail.TabIndex = 7;
        lblEmail.Text = "Email";
        // 
        // TxtDni
        // 
        TxtDni.Font = new Font("Segoe UI", 10F);
        TxtDni.Location = new Point(410, 38);
        TxtDni.Name = "TxtDni";
        TxtDni.Size = new Size(150, 34);
        TxtDni.TabIndex = 2;
        // 
        // lblDni
        // 
        lblDni.AutoSize = true;
        lblDni.Font = new Font("Segoe UI", 9F);
        lblDni.Location = new Point(410, 15);
        lblDni.Name = "lblDni";
        lblDni.Size = new Size(43, 25);
        lblDni.TabIndex = 9;
        lblDni.Text = "DNI";
        // 
        // TxtApellido
        // 
        TxtApellido.Font = new Font("Segoe UI", 10F);
        TxtApellido.Location = new Point(215, 38);
        TxtApellido.Name = "TxtApellido";
        TxtApellido.Size = new Size(180, 34);
        TxtApellido.TabIndex = 1;
        // 
        // lblApellido
        // 
        lblApellido.AutoSize = true;
        lblApellido.Font = new Font("Segoe UI", 9F);
        lblApellido.Location = new Point(215, 15);
        lblApellido.Name = "lblApellido";
        lblApellido.Size = new Size(78, 25);
        lblApellido.TabIndex = 11;
        lblApellido.Text = "Apellido";
        // 
        // TxtNombre
        // 
        TxtNombre.Font = new Font("Segoe UI", 10F);
        TxtNombre.Location = new Point(20, 38);
        TxtNombre.Name = "TxtNombre";
        TxtNombre.Size = new Size(180, 34);
        TxtNombre.TabIndex = 0;
        // 
        // lblNombre
        // 
        lblNombre.AutoSize = true;
        lblNombre.Font = new Font("Segoe UI", 9F);
        lblNombre.Location = new Point(20, 15);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(78, 25);
        lblNombre.TabIndex = 13;
        lblNombre.Text = "Nombre";
        // 
        // DgvPacientes
        // 
        DgvPacientes.AllowUserToAddRows = false;
        DgvPacientes.BackgroundColor = Color.White;
        DgvPacientes.ColumnHeadersHeight = 34;
        DgvPacientes.Dock = DockStyle.Fill;
        DgvPacientes.Font = new Font("Segoe UI", 9.5F);
        DgvPacientes.Location = new Point(0, 260);
        DgvPacientes.MultiSelect = false;
        DgvPacientes.Name = "DgvPacientes";
        DgvPacientes.ReadOnly = true;
        DgvPacientes.RowHeadersWidth = 62;
        DgvPacientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvPacientes.Size = new Size(1000, 410);
        DgvPacientes.TabIndex = 0;
        DgvPacientes.CellContentClick += DgvPacientes_CellContentClick;
        DgvPacientes.SelectionChanged += DgvPacientes_SelectionChanged;
        // 
        // FormPacientes
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 650);
        Controls.Add(DgvPacientes);
        Controls.Add(pnlFormulario);
        Name = "FormPacientes";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ABM de Pacientes";
        Load += FormPacientes_Load;
        pnlFormulario.ResumeLayout(false);
        pnlFormulario.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)DgvPacientes).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlFormulario;
    private Label lblNombre;
    private TextBox TxtNombre;
    private Label lblApellido;
    private TextBox TxtApellido;
    private Label lblDni;
    private TextBox TxtDni;
    private Label lblEmail;
    private TextBox TxtEmail;
    private Label lblTelefono;
    private TextBox TxtTelefono;
    private Label lblFechaNacimiento;
    private DateTimePicker DtpFechaNacimiento;
    private Label lblObraSocial;
    private ComboBox CboObraSocial;
    private Button BtnNuevo;
    private Button BtnGuardar;
    private Button BtnEliminar;
    private Label LblMensaje;
    private DataGridView DgvPacientes;
}
