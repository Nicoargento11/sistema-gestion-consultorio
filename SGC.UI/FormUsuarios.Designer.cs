namespace SGC.UI;

partial class FormUsuarios
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
        ClbMedicosAsignados = new CheckedListBox();
        lblMedicosAsignados = new Label();
        CboMedico = new ComboBox();
        lblMedico = new Label();
        LblMensaje = new Label();
        BtnEliminar = new Button();
        BtnGuardar = new Button();
        BtnNuevo = new Button();
        CboRol = new ComboBox();
        lblRol = new Label();
        TxtContrasena = new TextBox();
        lblContrasena = new Label();
        TxtNombreUsuario = new TextBox();
        lblNombreUsuario = new Label();
        DgvUsuarios = new DataGridView();
        pnlHeader.SuspendLayout();
        pnlFormulario.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvUsuarios).BeginInit();
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
        lblSubtitulo.Text = "Alta de logins y asignacion de medicos por rol";
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
        lblTitulo.Text = "Gestion de Usuarios";
        //
        // pnlFormulario
        //
        pnlFormulario.BackColor = Color.FromArgb(245, 246, 250);
        pnlFormulario.Controls.Add(ClbMedicosAsignados);
        pnlFormulario.Controls.Add(lblMedicosAsignados);
        pnlFormulario.Controls.Add(CboMedico);
        pnlFormulario.Controls.Add(lblMedico);
        pnlFormulario.Controls.Add(LblMensaje);
        pnlFormulario.Controls.Add(BtnEliminar);
        pnlFormulario.Controls.Add(BtnGuardar);
        pnlFormulario.Controls.Add(BtnNuevo);
        pnlFormulario.Controls.Add(CboRol);
        pnlFormulario.Controls.Add(lblRol);
        pnlFormulario.Controls.Add(TxtContrasena);
        pnlFormulario.Controls.Add(lblContrasena);
        pnlFormulario.Controls.Add(TxtNombreUsuario);
        pnlFormulario.Controls.Add(lblNombreUsuario);
        pnlFormulario.Dock = DockStyle.Top;
        pnlFormulario.Location = new Point(0, 75);
        pnlFormulario.Name = "pnlFormulario";
        pnlFormulario.Padding = new Padding(20, 10, 20, 10);
        pnlFormulario.Size = new Size(1000, 240);
        pnlFormulario.TabIndex = 1;
        //
        // ClbMedicosAsignados
        //
        ClbMedicosAsignados.CheckOnClick = true;
        ClbMedicosAsignados.Font = new Font("Segoe UI", 9.5F);
        ClbMedicosAsignados.Location = new Point(20, 80);
        ClbMedicosAsignados.Name = "ClbMedicosAsignados";
        ClbMedicosAsignados.Size = new Size(400, 94);
        ClbMedicosAsignados.TabIndex = 3;
        //
        // lblMedicosAsignados
        //
        lblMedicosAsignados.AutoSize = true;
        lblMedicosAsignados.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMedicosAsignados.ForeColor = Color.FromArgb(50, 60, 75);
        lblMedicosAsignados.Location = new Point(20, 60);
        lblMedicosAsignados.Name = "lblMedicosAsignados";
        lblMedicosAsignados.Size = new Size(220, 20);
        lblMedicosAsignados.TabIndex = 15;
        lblMedicosAsignados.Text = "Medicos asignados (Recepcionista)";
        //
        // CboMedico
        //
        CboMedico.DropDownStyle = ComboBoxStyle.DropDownList;
        CboMedico.Font = new Font("Segoe UI", 9.5F);
        CboMedico.Location = new Point(20, 80);
        CboMedico.Name = "CboMedico";
        CboMedico.Size = new Size(300, 29);
        CboMedico.TabIndex = 2;
        //
        // lblMedico
        //
        lblMedico.AutoSize = true;
        lblMedico.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMedico.ForeColor = Color.FromArgb(50, 60, 75);
        lblMedico.Location = new Point(20, 60);
        lblMedico.Name = "lblMedico";
        lblMedico.Size = new Size(150, 20);
        lblMedico.TabIndex = 14;
        lblMedico.Text = "Medico (rol Medico)";
        //
        // LblMensaje
        //
        LblMensaje.AutoSize = true;
        LblMensaje.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        LblMensaje.Location = new Point(360, 185);
        LblMensaje.MaximumSize = new Size(400, 0);
        LblMensaje.Name = "LblMensaje";
        LblMensaje.Size = new Size(0, 21);
        LblMensaje.TabIndex = 13;
        //
        // BtnEliminar
        //
        BtnEliminar.BackColor = Color.FromArgb(231, 76, 60);
        BtnEliminar.FlatAppearance.BorderSize = 0;
        BtnEliminar.FlatStyle = FlatStyle.Flat;
        BtnEliminar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnEliminar.ForeColor = Color.White;
        BtnEliminar.Location = new Point(240, 180);
        BtnEliminar.Name = "BtnEliminar";
        BtnEliminar.Size = new Size(100, 32);
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
        BtnGuardar.Location = new Point(130, 180);
        BtnGuardar.Name = "BtnGuardar";
        BtnGuardar.Size = new Size(100, 32);
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
        BtnNuevo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnNuevo.ForeColor = Color.White;
        BtnNuevo.Location = new Point(20, 180);
        BtnNuevo.Name = "BtnNuevo";
        BtnNuevo.Size = new Size(100, 32);
        BtnNuevo.TabIndex = 4;
        BtnNuevo.Text = "+ Nuevo";
        BtnNuevo.UseVisualStyleBackColor = false;
        BtnNuevo.Click += BtnNuevo_Click;
        //
        // CboRol
        //
        CboRol.DropDownStyle = ComboBoxStyle.DropDownList;
        CboRol.Font = new Font("Segoe UI", 9.5F);
        CboRol.Location = new Point(390, 28);
        CboRol.Name = "CboRol";
        CboRol.Size = new Size(160, 29);
        CboRol.TabIndex = 2;
        CboRol.SelectedIndexChanged += CboRol_SelectedIndexChanged;
        //
        // lblRol
        //
        lblRol.AutoSize = true;
        lblRol.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblRol.ForeColor = Color.FromArgb(50, 60, 75);
        lblRol.Location = new Point(390, 8);
        lblRol.Name = "lblRol";
        lblRol.Size = new Size(37, 20);
        lblRol.TabIndex = 12;
        lblRol.Text = "Rol";
        //
        // TxtContrasena
        //
        TxtContrasena.Font = new Font("Segoe UI", 9.5F);
        TxtContrasena.Location = new Point(215, 28);
        TxtContrasena.Name = "TxtContrasena";
        TxtContrasena.PasswordChar = '*';
        TxtContrasena.Size = new Size(160, 29);
        TxtContrasena.TabIndex = 1;
        //
        // lblContrasena
        //
        lblContrasena.AutoSize = true;
        lblContrasena.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblContrasena.ForeColor = Color.FromArgb(50, 60, 75);
        lblContrasena.Location = new Point(215, 8);
        lblContrasena.Name = "lblContrasena";
        lblContrasena.Size = new Size(100, 20);
        lblContrasena.TabIndex = 11;
        lblContrasena.Text = "Contrasena";
        //
        // TxtNombreUsuario
        //
        TxtNombreUsuario.Font = new Font("Segoe UI", 9.5F);
        TxtNombreUsuario.Location = new Point(20, 28);
        TxtNombreUsuario.Name = "TxtNombreUsuario";
        TxtNombreUsuario.Size = new Size(180, 29);
        TxtNombreUsuario.TabIndex = 0;
        //
        // lblNombreUsuario
        //
        lblNombreUsuario.AutoSize = true;
        lblNombreUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblNombreUsuario.ForeColor = Color.FromArgb(50, 60, 75);
        lblNombreUsuario.Location = new Point(20, 8);
        lblNombreUsuario.Name = "lblNombreUsuario";
        lblNombreUsuario.Size = new Size(130, 20);
        lblNombreUsuario.TabIndex = 10;
        lblNombreUsuario.Text = "Nombre de usuario";
        //
        // DgvUsuarios
        //
        DgvUsuarios.AllowUserToAddRows = false;
        DgvUsuarios.AllowUserToDeleteRows = false;
        DgvUsuarios.BackgroundColor = Color.White;
        DgvUsuarios.ColumnHeadersHeight = 34;
        DgvUsuarios.Dock = DockStyle.Fill;
        DgvUsuarios.Font = new Font("Segoe UI", 9.5F);
        DgvUsuarios.Location = new Point(0, 315);
        DgvUsuarios.MultiSelect = false;
        DgvUsuarios.Name = "DgvUsuarios";
        DgvUsuarios.ReadOnly = true;
        DgvUsuarios.RowHeadersVisible = false;
        DgvUsuarios.RowHeadersWidth = 51;
        DgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvUsuarios.Size = new Size(1000, 335);
        DgvUsuarios.TabIndex = 2;
        DgvUsuarios.SelectionChanged += DgvUsuarios_SelectionChanged;
        //
        // FormUsuarios
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(1000, 650);
        Controls.Add(DgvUsuarios);
        Controls.Add(pnlFormulario);
        Controls.Add(pnlHeader);
        Name = "FormUsuarios";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Gestion de Usuarios";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlFormulario.ResumeLayout(false);
        pnlFormulario.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)DgvUsuarios).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlHeader;
    private Label lblTitulo;
    private Label lblSubtitulo;
    private Panel pnlFormulario;
    private Label lblNombreUsuario;
    private TextBox TxtNombreUsuario;
    private Label lblContrasena;
    private TextBox TxtContrasena;
    private Label lblRol;
    private ComboBox CboRol;
    private Label lblMedico;
    private ComboBox CboMedico;
    private Label lblMedicosAsignados;
    private CheckedListBox ClbMedicosAsignados;
    private Button BtnNuevo;
    private Button BtnGuardar;
    private Button BtnEliminar;
    private Label LblMensaje;
    private DataGridView DgvUsuarios;
}
