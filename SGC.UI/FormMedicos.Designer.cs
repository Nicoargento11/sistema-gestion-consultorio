namespace SGC.UI;

partial class FormMedicos
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
        TxtEspecialidad = new TextBox();
        lblEspecialidad = new Label();
        TxtMatricula = new TextBox();
        lblMatricula = new Label();
        TxtDni = new TextBox();
        lblDni = new Label();
        TxtApellido = new TextBox();
        lblApellido = new Label();
        TxtNombre = new TextBox();
        lblNombre = new Label();
        DgvMedicos = new DataGridView();
        pnlFormulario.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvMedicos).BeginInit();
        SuspendLayout();
        // 
        // pnlFormulario
        // 
        pnlFormulario.BackColor = Color.FromArgb(245, 246, 250);
        pnlFormulario.Controls.Add(LblMensaje);
        pnlFormulario.Controls.Add(BtnEliminar);
        pnlFormulario.Controls.Add(BtnGuardar);
        pnlFormulario.Controls.Add(BtnNuevo);
        pnlFormulario.Controls.Add(TxtEspecialidad);
        pnlFormulario.Controls.Add(lblEspecialidad);
        pnlFormulario.Controls.Add(TxtMatricula);
        pnlFormulario.Controls.Add(lblMatricula);
        pnlFormulario.Controls.Add(TxtDni);
        pnlFormulario.Controls.Add(lblDni);
        pnlFormulario.Controls.Add(TxtApellido);
        pnlFormulario.Controls.Add(lblApellido);
        pnlFormulario.Controls.Add(TxtNombre);
        pnlFormulario.Controls.Add(lblNombre);
        pnlFormulario.Dock = DockStyle.Top;
        pnlFormulario.Location = new Point(0, 0);
        pnlFormulario.Name = "pnlFormulario";
        pnlFormulario.Size = new Size(1000, 190);
        pnlFormulario.TabIndex = 1;
        // 
        // LblMensaje
        // 
        LblMensaje.AutoSize = true;
        LblMensaje.Font = new Font("Segoe UI", 9F);
        LblMensaje.Location = new Point(500, 100);
        LblMensaje.MaximumSize = new Size(400, 0);
        LblMensaje.Name = "LblMensaje";
        LblMensaje.Size = new Size(0, 25);
        LblMensaje.TabIndex = 0;
        // 
        // BtnEliminar
        // 
        BtnEliminar.BackColor = Color.FromArgb(200, 60, 60);
        BtnEliminar.FlatAppearance.BorderSize = 0;
        BtnEliminar.FlatStyle = FlatStyle.Flat;
        BtnEliminar.Font = new Font("Segoe UI", 9.5F);
        BtnEliminar.ForeColor = Color.White;
        BtnEliminar.Location = new Point(240, 145);
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
        BtnGuardar.Location = new Point(130, 145);
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
        BtnNuevo.Location = new Point(20, 145);
        BtnNuevo.Name = "BtnNuevo";
        BtnNuevo.Size = new Size(100, 34);
        BtnNuevo.TabIndex = 5;
        BtnNuevo.Text = "Nuevo";
        BtnNuevo.UseVisualStyleBackColor = false;
        BtnNuevo.Click += BtnNuevo_Click;
        // 
        // TxtEspecialidad
        // 
        TxtEspecialidad.Font = new Font("Segoe UI", 10F);
        TxtEspecialidad.Location = new Point(220, 103);
        TxtEspecialidad.Name = "TxtEspecialidad";
        TxtEspecialidad.Size = new Size(260, 34);
        TxtEspecialidad.TabIndex = 4;
        // 
        // lblEspecialidad
        // 
        lblEspecialidad.AutoSize = true;
        lblEspecialidad.Font = new Font("Segoe UI", 9F);
        lblEspecialidad.Location = new Point(220, 80);
        lblEspecialidad.Name = "lblEspecialidad";
        lblEspecialidad.Size = new Size(109, 25);
        lblEspecialidad.TabIndex = 8;
        lblEspecialidad.Text = "Especialidad";
        // 
        // TxtMatricula
        // 
        TxtMatricula.Font = new Font("Segoe UI", 10F);
        TxtMatricula.Location = new Point(20, 103);
        TxtMatricula.Name = "TxtMatricula";
        TxtMatricula.Size = new Size(180, 34);
        TxtMatricula.TabIndex = 3;
        // 
        // lblMatricula
        // 
        lblMatricula.AutoSize = true;
        lblMatricula.Font = new Font("Segoe UI", 9F);
        lblMatricula.Location = new Point(20, 80);
        lblMatricula.Name = "lblMatricula";
        lblMatricula.Size = new Size(84, 25);
        lblMatricula.TabIndex = 9;
        lblMatricula.Text = "Matricula";
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
        lblDni.TabIndex = 10;
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
        lblNombre.TabIndex = 0;
        lblNombre.Text = "Nombre";
        // 
        // DgvMedicos
        // 
        DgvMedicos.AllowUserToAddRows = false;
        DgvMedicos.BackgroundColor = Color.White;
        DgvMedicos.ColumnHeadersHeight = 34;
        DgvMedicos.Dock = DockStyle.Fill;
        DgvMedicos.Font = new Font("Segoe UI", 9.5F);
        DgvMedicos.Location = new Point(0, 190);
        DgvMedicos.MultiSelect = false;
        DgvMedicos.Name = "DgvMedicos";
        DgvMedicos.ReadOnly = true;
        DgvMedicos.RowHeadersWidth = 62;
        DgvMedicos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvMedicos.Size = new Size(1000, 410);
        DgvMedicos.TabIndex = 0;
        DgvMedicos.SelectionChanged += DgvMedicos_SelectionChanged_1;
        // 
        // FormMedicos
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 600);
        Controls.Add(DgvMedicos);
        Controls.Add(pnlFormulario);
        Name = "FormMedicos";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ABM de Medicos";
        pnlFormulario.ResumeLayout(false);
        pnlFormulario.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)DgvMedicos).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlFormulario;
    private Label lblNombre;
    private TextBox TxtNombre;
    private Label lblApellido;
    private TextBox TxtApellido;
    private Label lblDni;
    private TextBox TxtDni;
    private Label lblMatricula;
    private TextBox TxtMatricula;
    private Label lblEspecialidad;
    private TextBox TxtEspecialidad;
    private Button BtnNuevo;
    private Button BtnGuardar;
    private Button BtnEliminar;
    private Label LblMensaje;
    private DataGridView DgvMedicos;
}
