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

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        pnlHeader = new Panel();
        lblSubtitulo = new Label();
        lblTitulo = new Label();
        pnlFormulario = new Panel();
        CboEspecialidad = new ComboBox();
        lblEspecialidad = new Label();
        TxtMatricula = new TextBox();
        lblMatricula = new Label();
        TxtBuscar = new TextBox();
        lblBuscar = new Label();
        LblMensaje = new Label();
        BtnEliminar = new Button();
        BtnGuardar = new Button();
        BtnNuevo = new Button();
        TxtDni = new TextBox();
        lblDni = new Label();
        TxtApellido = new TextBox();
        lblApellido = new Label();
        TxtNombre = new TextBox();
        lblNombre = new Label();
        DgvMedicos = new DataGridView();

        pnlHeader.SuspendLayout();
        pnlFormulario.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvMedicos).BeginInit();
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
        lblSubtitulo.Text = "Alta, modificacion y padron del cuerpo medico";

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
        lblTitulo.Text = "ABM de Medicos";

        // 
        // pnlFormulario
        // 
        pnlFormulario.BackColor = Color.FromArgb(245, 246, 250);
        pnlFormulario.Controls.Add(CboEspecialidad);
        pnlFormulario.Controls.Add(lblEspecialidad);
        pnlFormulario.Controls.Add(TxtMatricula);
        pnlFormulario.Controls.Add(lblMatricula);
        pnlFormulario.Controls.Add(TxtBuscar);
        pnlFormulario.Controls.Add(lblBuscar);
        pnlFormulario.Controls.Add(LblMensaje);
        pnlFormulario.Controls.Add(BtnEliminar);
        pnlFormulario.Controls.Add(BtnGuardar);
        pnlFormulario.Controls.Add(BtnNuevo);
        pnlFormulario.Controls.Add(TxtDni);
        pnlFormulario.Controls.Add(lblDni);
        pnlFormulario.Controls.Add(TxtApellido);
        pnlFormulario.Controls.Add(lblApellido);
        pnlFormulario.Controls.Add(TxtNombre);
        pnlFormulario.Controls.Add(lblNombre);
        pnlFormulario.Dock = DockStyle.Top;
        pnlFormulario.Location = new Point(0, 75);
        pnlFormulario.Name = "pnlFormulario";
        pnlFormulario.Padding = new Padding(20, 10, 20, 10);
        pnlFormulario.Size = new Size(1000, 175);
        pnlFormulario.TabIndex = 1;

        // 
        // CboEspecialidad
        // 
        CboEspecialidad.Font = new Font("Segoe UI", 9.5F);
        CboEspecialidad.FormattingEnabled = true;
        CboEspecialidad.Items.AddRange(new object[] {
            "Clinica General",
            "Cardiologia",
            "Pediatria",
            "Dermatologia",
            "Traumatologia",
            "Ginecologia",
            "Neurologia",
            "Oftalmologia"
        });
        CboEspecialidad.Location = new Point(215, 80);
        CboEspecialidad.Name = "CboEspecialidad";
        CboEspecialidad.Size = new Size(240, 29);
        CboEspecialidad.TabIndex = 4;

        // 
        // lblEspecialidad
        // 
        lblEspecialidad.AutoSize = true;
        lblEspecialidad.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblEspecialidad.ForeColor = Color.FromArgb(50, 60, 75);
        lblEspecialidad.Location = new Point(215, 60);
        lblEspecialidad.Name = "lblEspecialidad";
        lblEspecialidad.Size = new Size(95, 20);
        lblEspecialidad.TabIndex = 16;
        lblEspecialidad.Text = "Especialidad";

        // 
        // TxtMatricula
        // 
        TxtMatricula.Font = new Font("Segoe UI", 9.5F);
        TxtMatricula.Location = new Point(20, 80);
        TxtMatricula.Name = "TxtMatricula";
        TxtMatricula.PlaceholderText = "Ej: MP1234";
        TxtMatricula.Size = new Size(180, 29);
        TxtMatricula.TabIndex = 3;

        // 
        // lblMatricula
        // 
        lblMatricula.AutoSize = true;
        lblMatricula.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMatricula.ForeColor = Color.FromArgb(50, 60, 75);
        lblMatricula.Location = new Point(20, 60);
        lblMatricula.Name = "lblMatricula";
        lblMatricula.Size = new Size(75, 20);
        lblMatricula.TabIndex = 15;
        lblMatricula.Text = "Matricula";

        // 
        // TxtBuscar
        // 
        TxtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        TxtBuscar.Font = new Font("Segoe UI", 9.5F);
        TxtBuscar.Location = new Point(710, 125);
        TxtBuscar.Name = "TxtBuscar";
        TxtBuscar.PlaceholderText = "Buscar por Especialidad o Apellido...";
        TxtBuscar.Size = new Size(270, 29);
        TxtBuscar.TabIndex = 8;

        // 
        // lblBuscar
        // 
        lblBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblBuscar.AutoSize = true;
        lblBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblBuscar.ForeColor = Color.FromArgb(27, 42, 74);
        lblBuscar.Location = new Point(595, 129);
        lblBuscar.Name = "lblBuscar";
        lblBuscar.Size = new Size(111, 20);
        lblBuscar.TabIndex = 14;
        lblBuscar.Text = "Filtrar Cuerpo:";

        // 
        // LblMensaje
        // 
        LblMensaje.AutoSize = true;
        LblMensaje.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        LblMensaje.Location = new Point(480, 80);
        LblMensaje.MaximumSize = new Size(400, 0);
        LblMensaje.Name = "LblMensaje";
        LblMensaje.Size = new Size(0, 21);
        LblMensaje.TabIndex = 0;

        // 
        // BtnEliminar
        // 
        BtnEliminar.BackColor = Color.FromArgb(231, 76, 60);
        BtnEliminar.FlatAppearance.BorderSize = 0;
        BtnEliminar.FlatStyle = FlatStyle.Flat;
        BtnEliminar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnEliminar.ForeColor = Color.White;
        BtnEliminar.Location = new Point(240, 125);
        BtnEliminar.Name = "BtnEliminar";
        BtnEliminar.Size = new Size(100, 32);
        BtnEliminar.TabIndex = 7;
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
        BtnGuardar.Location = new Point(130, 125);
        BtnGuardar.Name = "BtnGuardar";
        BtnGuardar.Size = new Size(100, 32);
        BtnGuardar.TabIndex = 6;
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
        BtnNuevo.Location = new Point(20, 125);
        BtnNuevo.Name = "BtnNuevo";
        BtnNuevo.Size = new Size(100, 32);
        BtnNuevo.TabIndex = 5;
        BtnNuevo.Text = "+ Nuevo";
        BtnNuevo.UseVisualStyleBackColor = false;
        BtnNuevo.Click += BtnNuevo_Click;

        // 
        // TxtDni
        // 
        TxtDni.Font = new Font("Segoe UI", 9.5F);
        TxtDni.Location = new Point(410, 28);
        TxtDni.Name = "TxtDni";
        TxtDni.Size = new Size(140, 29);
        TxtDni.TabIndex = 2;

        // 
        // lblDni
        // 
        lblDni.AutoSize = true;
        lblDni.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDni.ForeColor = Color.FromArgb(50, 60, 75);
        lblDni.Location = new Point(410, 8);
        lblDni.Name = "lblDni";
        lblDni.Size = new Size(37, 20);
        lblDni.TabIndex = 9;
        lblDni.Text = "DNI";

        // 
        // TxtApellido
        // 
        TxtApellido.Font = new Font("Segoe UI", 9.5F);
        TxtApellido.Location = new Point(215, 28);
        TxtApellido.Name = "TxtApellido";
        TxtApellido.Size = new Size(180, 29);
        TxtApellido.TabIndex = 1;

        // 
        // lblApellido
        // 
        lblApellido.AutoSize = true;
        lblApellido.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblApellido.ForeColor = Color.FromArgb(50, 60, 75);
        lblApellido.Location = new Point(215, 8);
        lblApellido.Name = "lblApellido";
        lblApellido.Size = new Size(67, 20);
        lblApellido.TabIndex = 11;
        lblApellido.Text = "Apellido";

        // 
        // TxtNombre
        // 
        TxtNombre.Font = new Font("Segoe UI", 9.5F);
        TxtNombre.Location = new Point(20, 28);
        TxtNombre.Name = "TxtNombre";
        TxtNombre.Size = new Size(180, 29);
        TxtNombre.TabIndex = 0;

        // 
        // lblNombre
        // 
        lblNombre.AutoSize = true;
        lblNombre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblNombre.ForeColor = Color.FromArgb(50, 60, 75);
        lblNombre.Location = new Point(20, 8);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(67, 20);
        lblNombre.TabIndex = 13;
        lblNombre.Text = "Nombre";

        // 
        // DgvMedicos
        // 
        DgvMedicos.AllowUserToAddRows = false;
        DgvMedicos.AllowUserToDeleteRows = false;
        DgvMedicos.BackgroundColor = Color.White;
        DgvMedicos.ColumnHeadersHeight = 34;
        DgvMedicos.Dock = DockStyle.Fill;
        DgvMedicos.Font = new Font("Segoe UI", 9.5F);
        DgvMedicos.Location = new Point(0, 250);
        DgvMedicos.MultiSelect = false;
        DgvMedicos.Name = "DgvMedicos";
        DgvMedicos.ReadOnly = true;
        DgvMedicos.RowHeadersVisible = false;
        DgvMedicos.RowHeadersWidth = 51;
        DgvMedicos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvMedicos.Size = new Size(1000, 400);
        DgvMedicos.TabIndex = 2;
        

        // 
        // FormMedicos
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(1000, 650);
        Controls.Add(DgvMedicos);
        Controls.Add(pnlFormulario);
        Controls.Add(pnlHeader);
        Name = "FormMedicos";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ABM de Medicos";

        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlFormulario.ResumeLayout(false);
        pnlFormulario.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)DgvMedicos).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Panel pnlHeader;
    private Label lblTitulo;
    private Label lblSubtitulo;
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
    private ComboBox CboEspecialidad;
    private Button BtnNuevo;
    private Button BtnGuardar;
    private Button BtnEliminar;
    private Label LblMensaje;
    private Label lblBuscar;
    private TextBox TxtBuscar;
    private DataGridView DgvMedicos;
}