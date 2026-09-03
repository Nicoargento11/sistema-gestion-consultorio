namespace SGC.UI;

partial class FormHistorialClinico
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
        pnlFiltro = new Panel();
        BtnNuevaConsulta = new Button();
        BtnBuscar = new Button();
        CboPacientes = new ComboBox();
        lblSeleccionar = new Label();
        TxtBuscar = new TextBox();
        lblBuscar = new Label();
        pnlCardPaciente = new Panel();
        lblPacienteDetalle = new Label();
        splitContainerHistorial = new SplitContainer();
        pnlGrilla = new Panel();
        lblHistorialTitulo = new Label();
        DgvHistorial = new DataGridView();
        pnlDetalle = new Panel();
        TxtReceta = new TextBox();
        lblReceta = new Label();
        TxtDiagnostico = new TextBox();
        lblDiagnostico = new Label();
        TxtMotivo = new TextBox();
        lblMotivo = new Label();
        lblDetalleTitulo = new Label();
        pnlHeader.SuspendLayout();
        pnlFiltro.SuspendLayout();
        pnlCardPaciente.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainerHistorial).BeginInit();
        splitContainerHistorial.Panel1.SuspendLayout();
        splitContainerHistorial.Panel2.SuspendLayout();
        splitContainerHistorial.SuspendLayout();
        pnlGrilla.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvHistorial).BeginInit();
        pnlDetalle.SuspendLayout();
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
        lblSubtitulo.Text = "Consulta de atenciones clinicas y evolucion medica";
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
        lblTitulo.Text = "Historial Clinico de Pacientes";
        // 
        // pnlFiltro
        // 
        pnlFiltro.BackColor = Color.FromArgb(245, 246, 250);
        pnlFiltro.Controls.Add(BtnNuevaConsulta);
        pnlFiltro.Controls.Add(BtnBuscar);
        pnlFiltro.Controls.Add(CboPacientes);
        pnlFiltro.Controls.Add(lblSeleccionar);
        pnlFiltro.Controls.Add(TxtBuscar);
        pnlFiltro.Controls.Add(lblBuscar);
        pnlFiltro.Dock = DockStyle.Top;
        pnlFiltro.Location = new Point(0, 75);
        pnlFiltro.Name = "pnlFiltro";
        pnlFiltro.Padding = new Padding(20, 15, 20, 10);
        pnlFiltro.Size = new Size(1000, 65);
        pnlFiltro.TabIndex = 1;
        // 
        // BtnNuevaConsulta
        // 
        BtnNuevaConsulta.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        BtnNuevaConsulta.BackColor = Color.FromArgb(39, 174, 96);
        BtnNuevaConsulta.FlatAppearance.BorderSize = 0;
        BtnNuevaConsulta.FlatStyle = FlatStyle.Flat;
        BtnNuevaConsulta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        BtnNuevaConsulta.ForeColor = Color.White;
        BtnNuevaConsulta.Location = new Point(830, 17);
        BtnNuevaConsulta.Name = "BtnNuevaConsulta";
        BtnNuevaConsulta.Size = new Size(150, 30);
        BtnNuevaConsulta.TabIndex = 5;
        BtnNuevaConsulta.Text = "+ Nueva Atencion";
        BtnNuevaConsulta.UseVisualStyleBackColor = false;
        // 
        // BtnBuscar
        // 
        BtnBuscar.BackColor = Color.FromArgb(46, 134, 222);
        BtnBuscar.FlatAppearance.BorderSize = 0;
        BtnBuscar.FlatStyle = FlatStyle.Flat;
        BtnBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        BtnBuscar.ForeColor = Color.White;
        BtnBuscar.Location = new Point(275, 17);
        BtnBuscar.Name = "BtnBuscar";
        BtnBuscar.Size = new Size(75, 30);
        BtnBuscar.TabIndex = 2;
        BtnBuscar.Text = "Buscar";
        BtnBuscar.UseVisualStyleBackColor = false;
        // 
        // CboPacientes
        // 
        CboPacientes.DropDownStyle = ComboBoxStyle.DropDownList;
        CboPacientes.Font = new Font("Segoe UI", 9.5F);
        CboPacientes.FormattingEnabled = true;
        CboPacientes.Location = new Point(450, 18);
        CboPacientes.Name = "CboPacientes";
        CboPacientes.Size = new Size(360, 29);
        CboPacientes.TabIndex = 4;
        // 
        // lblSeleccionar
        // 
        lblSeleccionar.AutoSize = true;
        lblSeleccionar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblSeleccionar.ForeColor = Color.FromArgb(27, 42, 74);
        lblSeleccionar.Location = new Point(365, 22);
        lblSeleccionar.Name = "lblSeleccionar";
        lblSeleccionar.Size = new Size(79, 21);
        lblSeleccionar.TabIndex = 3;
        lblSeleccionar.Text = "Paciente:";
        // 
        // TxtBuscar
        // 
        TxtBuscar.Font = new Font("Segoe UI", 9.5F);
        TxtBuscar.Location = new Point(135, 18);
        TxtBuscar.Name = "TxtBuscar";
        TxtBuscar.PlaceholderText = "Nombre o DNI...";
        TxtBuscar.Size = new Size(130, 29);
        TxtBuscar.TabIndex = 1;
        // 
        // lblBuscar
        // 
        lblBuscar.AutoSize = true;
        lblBuscar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblBuscar.ForeColor = Color.FromArgb(27, 42, 74);
        lblBuscar.Location = new Point(20, 22);
        lblBuscar.Name = "lblBuscar";
        lblBuscar.Size = new Size(111, 21);
        lblBuscar.TabIndex = 0;
        lblBuscar.Text = "Filtrar Pac.:";
        // 
        // pnlCardPaciente
        // 
        pnlCardPaciente.BackColor = Color.FromArgb(235, 243, 253);
        pnlCardPaciente.BorderStyle = BorderStyle.FixedSingle;
        pnlCardPaciente.Controls.Add(lblPacienteDetalle);
        pnlCardPaciente.Dock = DockStyle.Top;
        pnlCardPaciente.Location = new Point(0, 140);
        pnlCardPaciente.Name = "pnlCardPaciente";
        pnlCardPaciente.Padding = new Padding(15, 10, 15, 10);
        pnlCardPaciente.Size = new Size(1000, 48);
        pnlCardPaciente.TabIndex = 2;
        // 
        // lblPacienteDetalle
        // 
        lblPacienteDetalle.Dock = DockStyle.Fill;
        lblPacienteDetalle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblPacienteDetalle.ForeColor = Color.FromArgb(27, 42, 74);
        lblPacienteDetalle.Location = new Point(15, 10);
        lblPacienteDetalle.Name = "lblPacienteDetalle";
        lblPacienteDetalle.Size = new Size(968, 26);
        lblPacienteDetalle.TabIndex = 0;
        lblPacienteDetalle.Text = "Paciente: ... | DNI: ... | Email: ... | Telefono: ...";
        lblPacienteDetalle.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // splitContainerHistorial
        // 
        splitContainerHistorial.Dock = DockStyle.Fill;
        splitContainerHistorial.Location = new Point(0, 188);
        splitContainerHistorial.Name = "splitContainerHistorial";
        splitContainerHistorial.Orientation = Orientation.Horizontal;
        // 
        // splitContainerHistorial.Panel1
        // 
        splitContainerHistorial.Panel1.Controls.Add(pnlGrilla);
        splitContainerHistorial.Panel1MinSize = 150;
        // 
        // splitContainerHistorial.Panel2
        // 
        splitContainerHistorial.Panel2.Controls.Add(pnlDetalle);
        splitContainerHistorial.Panel2MinSize = 180;
        splitContainerHistorial.Size = new Size(1000, 462);
        splitContainerHistorial.SplitterDistance = 210;
        splitContainerHistorial.TabIndex = 3;
        // 
        // pnlGrilla
        // 
        pnlGrilla.BackColor = Color.FromArgb(245, 246, 250);
        pnlGrilla.Controls.Add(DgvHistorial);
        pnlGrilla.Controls.Add(lblHistorialTitulo);
        pnlGrilla.Dock = DockStyle.Fill;
        pnlGrilla.Location = new Point(0, 0);
        pnlGrilla.Name = "pnlGrilla";
        pnlGrilla.Padding = new Padding(20, 10, 20, 10);
        pnlGrilla.Size = new Size(1000, 210);
        pnlGrilla.TabIndex = 0;
        // 
        // lblHistorialTitulo
        // 
        lblHistorialTitulo.AutoSize = true;
        lblHistorialTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblHistorialTitulo.ForeColor = Color.FromArgb(27, 42, 74);
        lblHistorialTitulo.Location = new Point(20, 6);
        lblHistorialTitulo.Name = "lblHistorialTitulo";
        lblHistorialTitulo.Size = new Size(307, 25);
        lblHistorialTitulo.TabIndex = 0;
        lblHistorialTitulo.Text = "Registro Cronologico de Atenciones";
        // 
        // DgvHistorial
        // 
        DgvHistorial.AllowUserToAddRows = false;
        DgvHistorial.AllowUserToDeleteRows = false;
        DgvHistorial.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        DgvHistorial.BackgroundColor = Color.White;
        DgvHistorial.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        DgvHistorial.Location = new Point(20, 35);
        DgvHistorial.MultiSelect = false;
        DgvHistorial.Name = "DgvHistorial";
        DgvHistorial.ReadOnly = true;
        DgvHistorial.RowHeadersVisible = false;
        DgvHistorial.RowHeadersWidth = 51;
        DgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvHistorial.Size = new Size(960, 165);
        DgvHistorial.TabIndex = 1;
        // 
        // pnlDetalle
        // 
        pnlDetalle.AutoScroll = true;
        pnlDetalle.BackColor = Color.White;
        pnlDetalle.Controls.Add(TxtReceta);
        pnlDetalle.Controls.Add(lblReceta);
        pnlDetalle.Controls.Add(TxtDiagnostico);
        pnlDetalle.Controls.Add(lblDiagnostico);
        pnlDetalle.Controls.Add(TxtMotivo);
        pnlDetalle.Controls.Add(lblMotivo);
        pnlDetalle.Controls.Add(lblDetalleTitulo);
        pnlDetalle.Dock = DockStyle.Fill;
        pnlDetalle.Location = new Point(0, 0);
        pnlDetalle.Name = "pnlDetalle";
        pnlDetalle.Padding = new Padding(25, 10, 25, 15);
        pnlDetalle.Size = new Size(1000, 248);
        pnlDetalle.TabIndex = 0;
        // 
        // TxtReceta
        // 
        TxtReceta.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        TxtReceta.Font = new Font("Segoe UI", 9.5F);
        TxtReceta.Location = new Point(25, 175);
        TxtReceta.Multiline = true;
        TxtReceta.Name = "TxtReceta";
        TxtReceta.ReadOnly = true;
        TxtReceta.ScrollBars = ScrollBars.Vertical;
        TxtReceta.Size = new Size(950, 48);
        TxtReceta.TabIndex = 6;
        // 
        // lblReceta
        // 
        lblReceta.AutoSize = true;
        lblReceta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblReceta.ForeColor = Color.FromArgb(50, 60, 75);
        lblReceta.Location = new Point(25, 155);
        lblReceta.Name = "lblReceta";
        lblReceta.Size = new Size(157, 20);
        lblReceta.TabIndex = 5;
        lblReceta.Text = "Prescripcion / Receta:";
        // 
        // TxtDiagnostico
        // 
        TxtDiagnostico.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        TxtDiagnostico.Font = new Font("Segoe UI", 9.5F);
        TxtDiagnostico.Location = new Point(25, 105);
        TxtDiagnostico.Multiline = true;
        TxtDiagnostico.Name = "TxtDiagnostico";
        TxtDiagnostico.ReadOnly = true;
        TxtDiagnostico.ScrollBars = ScrollBars.Vertical;
        TxtDiagnostico.Size = new Size(950, 45);
        TxtDiagnostico.TabIndex = 4;
        // 
        // lblDiagnostico
        // 
        lblDiagnostico.AutoSize = true;
        lblDiagnostico.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDiagnostico.ForeColor = Color.FromArgb(50, 60, 75);
        lblDiagnostico.Location = new Point(25, 85);
        lblDiagnostico.Name = "lblDiagnostico";
        lblDiagnostico.Size = new Size(207, 20);
        lblDiagnostico.TabIndex = 3;
        lblDiagnostico.Text = "Diagnostico / Procedimiento:";
        // 
        // TxtMotivo
        // 
        TxtMotivo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        TxtMotivo.Font = new Font("Segoe UI", 9.5F);
        TxtMotivo.Location = new Point(25, 38);
        TxtMotivo.Multiline = true;
        TxtMotivo.Name = "TxtMotivo";
        TxtMotivo.ReadOnly = true;
        TxtMotivo.ScrollBars = ScrollBars.Vertical;
        TxtMotivo.Size = new Size(950, 42);
        TxtMotivo.TabIndex = 2;
        // 
        // lblMotivo
        // 
        lblMotivo.AutoSize = true;
        lblMotivo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMotivo.ForeColor = Color.FromArgb(50, 60, 75);
        lblMotivo.Location = new Point(25, 18);
        lblMotivo.Name = "lblMotivo";
        lblMotivo.Size = new Size(146, 20);
        lblMotivo.TabIndex = 1;
        lblMotivo.Text = "Motivo de Consulta:";
        // 
        // lblDetalleTitulo
        // 
        lblDetalleTitulo.AutoSize = true;
        lblDetalleTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblDetalleTitulo.ForeColor = Color.FromArgb(27, 42, 74);
        lblDetalleTitulo.Location = new Point(25, 0);
        lblDetalleTitulo.Name = "lblDetalleTitulo";
        lblDetalleTitulo.Size = new Size(276, 23);
        lblDetalleTitulo.TabIndex = 0;
        lblDetalleTitulo.Text = "Detalle de la Atencion Clinica:";
        // 
        // FormHistorialClinico
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(1000, 650);
        Controls.Add(splitContainerHistorial);
        Controls.Add(pnlCardPaciente);
        Controls.Add(pnlFiltro);
        Controls.Add(pnlHeader);
        Name = "FormHistorialClinico";
        Text = "Historial Clinico de Pacientes";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlFiltro.ResumeLayout(false);
        pnlFiltro.PerformLayout();
        pnlCardPaciente.ResumeLayout(false);
        splitContainerHistorial.Panel1.ResumeLayout(false);
        splitContainerHistorial.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainerHistorial).EndInit();
        splitContainerHistorial.ResumeLayout(false);
        pnlGrilla.ResumeLayout(false);
        pnlGrilla.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)DgvHistorial).EndInit();
        pnlDetalle.ResumeLayout(false);
        pnlDetalle.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Panel pnlHeader;
    private Label lblTitulo;
    private Label lblSubtitulo;
    private Panel pnlFiltro;
    private Label lblBuscar;
    private TextBox TxtBuscar;
    private Button BtnBuscar;
    private Label lblSeleccionar;
    private ComboBox CboPacientes;
    private Button BtnNuevaConsulta;
    private Panel pnlCardPaciente;
    private Label lblPacienteDetalle;
    private SplitContainer splitContainerHistorial;
    private Panel pnlGrilla;
    private Label lblHistorialTitulo;
    private DataGridView DgvHistorial;
    private Panel pnlDetalle;
    private Label lblDetalleTitulo;
    private Label lblMotivo;
    private TextBox TxtMotivo;
    private Label lblDiagnostico;
    private TextBox TxtDiagnostico;
    private Label lblReceta;
    private TextBox TxtReceta;
}