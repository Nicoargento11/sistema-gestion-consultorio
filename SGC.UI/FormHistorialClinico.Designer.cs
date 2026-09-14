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
        CboTipoFiltro = new ComboBox();
        lblTipoFiltro = new Label();
        CboMedicoFiltro = new ComboBox();
        lblMedicoFiltro = new Label();
        DtpFechaFiltro = new DateTimePicker();
        ChkFiltrarFecha = new CheckBox();
        CboPacientes = new ComboBox();
        lblSeleccionar = new Label();
        BtnLimpiar = new Button();
        BtnBuscar = new Button();
        TxtBuscar = new TextBox();
        lblBuscar = new Label();
        pnlCardPaciente = new Panel();
        lblResumenHistorial = new Label();
        lblPacienteDetalle = new Label();
        splitContainerHistorial = new SplitContainer();
        pnlGrilla = new Panel();
        DgvHistorial = new DataGridView();
        lblHistorialTitulo = new Label();
        pnlDetalle = new Panel();
        tblDetalle = new TableLayoutPanel();
        lblDetalleTitulo = new Label();
        lblMotivo = new Label();
        TxtMotivo = new TextBox();
        lblDiagnostico = new Label();
        TxtDiagnostico = new TextBox();
        lblReceta = new Label();
        TxtReceta = new TextBox();
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
        tblDetalle.SuspendLayout();
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
        pnlHeader.Size = new Size(1000, 70);
        pnlHeader.TabIndex = 0;
        // 
        // lblSubtitulo
        // 
        lblSubtitulo.AutoSize = true;
        lblSubtitulo.Font = new Font("Segoe UI", 9.5F);
        lblSubtitulo.ForeColor = Color.FromArgb(180, 205, 235);
        lblSubtitulo.Location = new Point(20, 40);
        lblSubtitulo.Name = "lblSubtitulo";
        lblSubtitulo.Size = new Size(596, 21);
        lblSubtitulo.TabIndex = 1;
        lblSubtitulo.Text = "Solo lectura: consulte atenciones ya cargadas. Para escribir una nueva, use la Agenda.";
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(18, 8);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(306, 32);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Consultar historial clinico";
        // 
        // pnlFiltro
        // 
        pnlFiltro.BackColor = Color.White;
        pnlFiltro.Controls.Add(BtnNuevaConsulta);
        pnlFiltro.Controls.Add(CboTipoFiltro);
        pnlFiltro.Controls.Add(lblTipoFiltro);
        pnlFiltro.Controls.Add(CboMedicoFiltro);
        pnlFiltro.Controls.Add(lblMedicoFiltro);
        pnlFiltro.Controls.Add(DtpFechaFiltro);
        pnlFiltro.Controls.Add(ChkFiltrarFecha);
        pnlFiltro.Controls.Add(CboPacientes);
        pnlFiltro.Controls.Add(lblSeleccionar);
        pnlFiltro.Controls.Add(BtnLimpiar);
        pnlFiltro.Controls.Add(BtnBuscar);
        pnlFiltro.Controls.Add(TxtBuscar);
        pnlFiltro.Controls.Add(lblBuscar);
        pnlFiltro.Dock = DockStyle.Top;
        pnlFiltro.Location = new Point(0, 70);
        pnlFiltro.Name = "pnlFiltro";
        pnlFiltro.Size = new Size(1000, 130);
        pnlFiltro.TabIndex = 1;
        // 
        // BtnNuevaConsulta
        // 
        BtnNuevaConsulta.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        BtnNuevaConsulta.BackColor = Color.FromArgb(46, 134, 222);
        BtnNuevaConsulta.Cursor = Cursors.Hand;
        BtnNuevaConsulta.FlatAppearance.BorderSize = 0;
        BtnNuevaConsulta.FlatStyle = FlatStyle.Flat;
        BtnNuevaConsulta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        BtnNuevaConsulta.ForeColor = Color.White;
        BtnNuevaConsulta.Location = new Point(800, 11);
        BtnNuevaConsulta.Name = "BtnNuevaConsulta";
        BtnNuevaConsulta.Size = new Size(180, 32);
        BtnNuevaConsulta.TabIndex = 4;
        BtnNuevaConsulta.Text = "Ir a la Agenda";
        BtnNuevaConsulta.UseVisualStyleBackColor = false;
        // 
        // CboTipoFiltro
        // 
        CboTipoFiltro.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        CboTipoFiltro.DropDownStyle = ComboBoxStyle.DropDownList;
        CboTipoFiltro.Font = new Font("Segoe UI", 9.5F);
        CboTipoFiltro.FormattingEnabled = true;
        CboTipoFiltro.Location = new Point(674, 54);
        CboTipoFiltro.Name = "CboTipoFiltro";
        CboTipoFiltro.Size = new Size(200, 29);
        CboTipoFiltro.TabIndex = 8;
        // 
        // lblTipoFiltro
        // 
        lblTipoFiltro.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblTipoFiltro.AutoSize = true;
        lblTipoFiltro.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTipoFiltro.ForeColor = Color.FromArgb(27, 42, 74);
        lblTipoFiltro.Location = new Point(628, 58);
        lblTipoFiltro.Name = "lblTipoFiltro";
        lblTipoFiltro.Size = new Size(44, 20);
        lblTipoFiltro.TabIndex = 7;
        lblTipoFiltro.Text = "Tipo:";
        // 
        // CboMedicoFiltro
        // 
        CboMedicoFiltro.DropDownStyle = ComboBoxStyle.DropDownList;
        CboMedicoFiltro.Font = new Font("Segoe UI", 9.5F);
        CboMedicoFiltro.Location = new Point(90, 90);
        CboMedicoFiltro.Name = "CboMedicoFiltro";
        CboMedicoFiltro.Size = new Size(280, 29);
        CboMedicoFiltro.TabIndex = 9;
        // 
        // lblMedicoFiltro
        // 
        lblMedicoFiltro.AutoSize = true;
        lblMedicoFiltro.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMedicoFiltro.ForeColor = Color.FromArgb(27, 42, 74);
        lblMedicoFiltro.Location = new Point(20, 94);
        lblMedicoFiltro.Name = "lblMedicoFiltro";
        lblMedicoFiltro.Size = new Size(64, 20);
        lblMedicoFiltro.TabIndex = 10;
        lblMedicoFiltro.Text = "Medico:";
        // 
        // DtpFechaFiltro
        // 
        DtpFechaFiltro.Format = DateTimePickerFormat.Short;
        DtpFechaFiltro.Location = new Point(530, 90);
        DtpFechaFiltro.Name = "DtpFechaFiltro";
        DtpFechaFiltro.Size = new Size(120, 27);
        DtpFechaFiltro.TabIndex = 11;
        DtpFechaFiltro.ValueChanged += DtpFechaFiltro_ValueChanged;
        // 
        // ChkFiltrarFecha
        // 
        ChkFiltrarFecha.AutoSize = true;
        ChkFiltrarFecha.Location = new Point(390, 92);
        ChkFiltrarFecha.Name = "ChkFiltrarFecha";
        ChkFiltrarFecha.Size = new Size(136, 24);
        ChkFiltrarFecha.TabIndex = 10;
        ChkFiltrarFecha.Text = "Filtrar por fecha";
        // 
        // CboPacientes
        // 
        CboPacientes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        CboPacientes.DropDownStyle = ComboBoxStyle.DropDownList;
        CboPacientes.Font = new Font("Segoe UI", 9.5F);
        CboPacientes.FormattingEnabled = true;
        CboPacientes.Location = new Point(90, 54);
        CboPacientes.Name = "CboPacientes";
        CboPacientes.Size = new Size(520, 29);
        CboPacientes.TabIndex = 6;
        // 
        // lblSeleccionar
        // 
        lblSeleccionar.AutoSize = true;
        lblSeleccionar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblSeleccionar.ForeColor = Color.FromArgb(27, 42, 74);
        lblSeleccionar.Location = new Point(20, 58);
        lblSeleccionar.Name = "lblSeleccionar";
        lblSeleccionar.Size = new Size(72, 20);
        lblSeleccionar.TabIndex = 5;
        lblSeleccionar.Text = "Paciente:";
        // 
        // BtnLimpiar
        // 
        BtnLimpiar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        BtnLimpiar.BackColor = Color.FromArgb(120, 130, 145);
        BtnLimpiar.Cursor = Cursors.Hand;
        BtnLimpiar.FlatAppearance.BorderSize = 0;
        BtnLimpiar.FlatStyle = FlatStyle.Flat;
        BtnLimpiar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        BtnLimpiar.ForeColor = Color.White;
        BtnLimpiar.Location = new Point(628, 11);
        BtnLimpiar.Name = "BtnLimpiar";
        BtnLimpiar.Size = new Size(90, 32);
        BtnLimpiar.TabIndex = 3;
        BtnLimpiar.Text = "Limpiar";
        BtnLimpiar.UseVisualStyleBackColor = false;
        // 
        // BtnBuscar
        // 
        BtnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        BtnBuscar.BackColor = Color.FromArgb(46, 134, 222);
        BtnBuscar.Cursor = Cursors.Hand;
        BtnBuscar.FlatAppearance.BorderSize = 0;
        BtnBuscar.FlatStyle = FlatStyle.Flat;
        BtnBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        BtnBuscar.ForeColor = Color.White;
        BtnBuscar.Location = new Point(532, 11);
        BtnBuscar.Name = "BtnBuscar";
        BtnBuscar.Size = new Size(90, 32);
        BtnBuscar.TabIndex = 2;
        BtnBuscar.Text = "Buscar";
        BtnBuscar.UseVisualStyleBackColor = false;
        // 
        // TxtBuscar
        // 
        TxtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        TxtBuscar.Font = new Font("Segoe UI", 9.5F);
        TxtBuscar.Location = new Point(90, 12);
        TxtBuscar.Name = "TxtBuscar";
        TxtBuscar.PlaceholderText = "Nombre, apellido o DNI";
        TxtBuscar.Size = new Size(430, 29);
        TxtBuscar.TabIndex = 1;
        // 
        // lblBuscar
        // 
        lblBuscar.AutoSize = true;
        lblBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblBuscar.ForeColor = Color.FromArgb(27, 42, 74);
        lblBuscar.Location = new Point(20, 16);
        lblBuscar.Name = "lblBuscar";
        lblBuscar.Size = new Size(61, 20);
        lblBuscar.TabIndex = 0;
        lblBuscar.Text = "Buscar:";
        // 
        // pnlCardPaciente
        // 
        pnlCardPaciente.BackColor = Color.FromArgb(235, 243, 253);
        pnlCardPaciente.Controls.Add(lblResumenHistorial);
        pnlCardPaciente.Controls.Add(lblPacienteDetalle);
        pnlCardPaciente.Dock = DockStyle.Top;
        pnlCardPaciente.Location = new Point(0, 200);
        pnlCardPaciente.Name = "pnlCardPaciente";
        pnlCardPaciente.Padding = new Padding(20, 8, 20, 8);
        pnlCardPaciente.Size = new Size(1000, 58);
        pnlCardPaciente.TabIndex = 2;
        // 
        // lblResumenHistorial
        // 
        lblResumenHistorial.AutoEllipsis = true;
        lblResumenHistorial.Dock = DockStyle.Fill;
        lblResumenHistorial.Font = new Font("Segoe UI", 9F);
        lblResumenHistorial.ForeColor = Color.FromArgb(70, 90, 120);
        lblResumenHistorial.Location = new Point(20, 30);
        lblResumenHistorial.Name = "lblResumenHistorial";
        lblResumenHistorial.Size = new Size(960, 20);
        lblResumenHistorial.TabIndex = 1;
        lblResumenHistorial.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblPacienteDetalle
        // 
        lblPacienteDetalle.AutoEllipsis = true;
        lblPacienteDetalle.Dock = DockStyle.Top;
        lblPacienteDetalle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblPacienteDetalle.ForeColor = Color.FromArgb(27, 42, 74);
        lblPacienteDetalle.Location = new Point(20, 8);
        lblPacienteDetalle.Name = "lblPacienteDetalle";
        lblPacienteDetalle.Size = new Size(960, 22);
        lblPacienteDetalle.TabIndex = 0;
        lblPacienteDetalle.Text = "Seleccione un paciente para consultar su historial.";
        lblPacienteDetalle.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // splitContainerHistorial
        // 
        splitContainerHistorial.Dock = DockStyle.Fill;
        splitContainerHistorial.Location = new Point(0, 258);
        splitContainerHistorial.Name = "splitContainerHistorial";
        splitContainerHistorial.Orientation = Orientation.Horizontal;
        // 
        // splitContainerHistorial.Panel1
        // 
        splitContainerHistorial.Panel1.Controls.Add(pnlGrilla);
        splitContainerHistorial.Panel1MinSize = 140;
        // 
        // splitContainerHistorial.Panel2
        // 
        splitContainerHistorial.Panel2.Controls.Add(pnlDetalle);
        splitContainerHistorial.Panel2MinSize = 180;
        splitContainerHistorial.Size = new Size(1000, 392);
        splitContainerHistorial.SplitterDistance = 184;
        splitContainerHistorial.SplitterWidth = 8;
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
        pnlGrilla.Padding = new Padding(16, 8, 16, 8);
        pnlGrilla.Size = new Size(1000, 184);
        pnlGrilla.TabIndex = 0;
        // 
        // DgvHistorial
        // 
        DgvHistorial.AllowUserToAddRows = false;
        DgvHistorial.AllowUserToDeleteRows = false;
        DgvHistorial.AllowUserToResizeRows = false;
        DgvHistorial.BackgroundColor = Color.White;
        DgvHistorial.BorderStyle = BorderStyle.None;
        DgvHistorial.ColumnHeadersHeight = 29;
        DgvHistorial.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        DgvHistorial.Dock = DockStyle.Fill;
        DgvHistorial.Location = new Point(16, 36);
        DgvHistorial.MultiSelect = false;
        DgvHistorial.Name = "DgvHistorial";
        DgvHistorial.ReadOnly = true;
        DgvHistorial.RowHeadersVisible = false;
        DgvHistorial.RowHeadersWidth = 51;
        DgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvHistorial.Size = new Size(968, 140);
        DgvHistorial.TabIndex = 1;
        // 
        // lblHistorialTitulo
        // 
        lblHistorialTitulo.Dock = DockStyle.Top;
        lblHistorialTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblHistorialTitulo.ForeColor = Color.FromArgb(27, 42, 74);
        lblHistorialTitulo.Location = new Point(16, 8);
        lblHistorialTitulo.Name = "lblHistorialTitulo";
        lblHistorialTitulo.Padding = new Padding(0, 0, 0, 6);
        lblHistorialTitulo.Size = new Size(968, 28);
        lblHistorialTitulo.TabIndex = 0;
        lblHistorialTitulo.Text = "Registro cronologico de atenciones";
        // 
        // pnlDetalle
        // 
        pnlDetalle.BackColor = Color.White;
        pnlDetalle.Controls.Add(tblDetalle);
        pnlDetalle.Dock = DockStyle.Fill;
        pnlDetalle.Location = new Point(0, 0);
        pnlDetalle.Name = "pnlDetalle";
        pnlDetalle.Padding = new Padding(16, 8, 16, 12);
        pnlDetalle.Size = new Size(1000, 200);
        pnlDetalle.TabIndex = 0;
        // 
        // tblDetalle
        // 
        tblDetalle.ColumnCount = 1;
        tblDetalle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblDetalle.Controls.Add(lblDetalleTitulo, 0, 0);
        tblDetalle.Controls.Add(lblMotivo, 0, 1);
        tblDetalle.Controls.Add(TxtMotivo, 0, 2);
        tblDetalle.Controls.Add(lblDiagnostico, 0, 3);
        tblDetalle.Controls.Add(TxtDiagnostico, 0, 4);
        tblDetalle.Controls.Add(lblReceta, 0, 5);
        tblDetalle.Controls.Add(TxtReceta, 0, 6);
        tblDetalle.Dock = DockStyle.Fill;
        tblDetalle.Location = new Point(16, 8);
        tblDetalle.Name = "tblDetalle";
        tblDetalle.RowCount = 7;
        tblDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        tblDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
        tblDetalle.RowStyles.Add(new RowStyle(SizeType.Percent, 34F));
        tblDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
        tblDetalle.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
        tblDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
        tblDetalle.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
        tblDetalle.Size = new Size(968, 180);
        tblDetalle.TabIndex = 0;
        // 
        // lblDetalleTitulo
        // 
        lblDetalleTitulo.AutoEllipsis = true;
        lblDetalleTitulo.Dock = DockStyle.Fill;
        lblDetalleTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblDetalleTitulo.ForeColor = Color.FromArgb(27, 42, 74);
        lblDetalleTitulo.Location = new Point(3, 0);
        lblDetalleTitulo.Name = "lblDetalleTitulo";
        lblDetalleTitulo.Size = new Size(962, 28);
        lblDetalleTitulo.TabIndex = 0;
        lblDetalleTitulo.Text = "Detalle de la atencion clinica";
        lblDetalleTitulo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblMotivo
        // 
        lblMotivo.Dock = DockStyle.Fill;
        lblMotivo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblMotivo.ForeColor = Color.FromArgb(50, 60, 75);
        lblMotivo.Location = new Point(3, 28);
        lblMotivo.Name = "lblMotivo";
        lblMotivo.Size = new Size(962, 22);
        lblMotivo.TabIndex = 1;
        lblMotivo.Text = "Motivo de consulta";
        lblMotivo.TextAlign = ContentAlignment.BottomLeft;
        // 
        // TxtMotivo
        // 
        TxtMotivo.BackColor = Color.FromArgb(248, 250, 252);
        TxtMotivo.BorderStyle = BorderStyle.FixedSingle;
        TxtMotivo.Dock = DockStyle.Fill;
        TxtMotivo.Font = new Font("Segoe UI", 9.5F);
        TxtMotivo.Location = new Point(3, 53);
        TxtMotivo.Multiline = true;
        TxtMotivo.Name = "TxtMotivo";
        TxtMotivo.ReadOnly = true;
        TxtMotivo.ScrollBars = ScrollBars.Vertical;
        TxtMotivo.Size = new Size(962, 23);
        TxtMotivo.TabIndex = 0;
        // 
        // lblDiagnostico
        // 
        lblDiagnostico.Dock = DockStyle.Fill;
        lblDiagnostico.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblDiagnostico.ForeColor = Color.FromArgb(50, 60, 75);
        lblDiagnostico.Location = new Point(3, 79);
        lblDiagnostico.Name = "lblDiagnostico";
        lblDiagnostico.Size = new Size(962, 22);
        lblDiagnostico.TabIndex = 2;
        lblDiagnostico.Text = "Diagnostico / Procedimiento";
        lblDiagnostico.TextAlign = ContentAlignment.BottomLeft;
        // 
        // TxtDiagnostico
        // 
        TxtDiagnostico.BackColor = Color.FromArgb(248, 250, 252);
        TxtDiagnostico.BorderStyle = BorderStyle.FixedSingle;
        TxtDiagnostico.Dock = DockStyle.Fill;
        TxtDiagnostico.Font = new Font("Segoe UI", 9.5F);
        TxtDiagnostico.Location = new Point(3, 104);
        TxtDiagnostico.Multiline = true;
        TxtDiagnostico.Name = "TxtDiagnostico";
        TxtDiagnostico.ReadOnly = true;
        TxtDiagnostico.ScrollBars = ScrollBars.Vertical;
        TxtDiagnostico.Size = new Size(962, 22);
        TxtDiagnostico.TabIndex = 1;
        // 
        // lblReceta
        // 
        lblReceta.Dock = DockStyle.Fill;
        lblReceta.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblReceta.ForeColor = Color.FromArgb(50, 60, 75);
        lblReceta.Location = new Point(3, 129);
        lblReceta.Name = "lblReceta";
        lblReceta.Size = new Size(962, 22);
        lblReceta.TabIndex = 3;
        lblReceta.Text = "Prescripcion / Receta";
        lblReceta.TextAlign = ContentAlignment.BottomLeft;
        // 
        // TxtReceta
        // 
        TxtReceta.BackColor = Color.FromArgb(248, 250, 252);
        TxtReceta.BorderStyle = BorderStyle.FixedSingle;
        TxtReceta.Dock = DockStyle.Fill;
        TxtReceta.Font = new Font("Segoe UI", 9.5F);
        TxtReceta.Location = new Point(3, 154);
        TxtReceta.Multiline = true;
        TxtReceta.Name = "TxtReceta";
        TxtReceta.ReadOnly = true;
        TxtReceta.ScrollBars = ScrollBars.Vertical;
        TxtReceta.Size = new Size(962, 23);
        TxtReceta.TabIndex = 2;
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
        MinimumSize = new Size(820, 520);
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
        ((System.ComponentModel.ISupportInitialize)DgvHistorial).EndInit();
        pnlDetalle.ResumeLayout(false);
        tblDetalle.ResumeLayout(false);
        tblDetalle.PerformLayout();
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
    private Button BtnLimpiar;
    private Label lblSeleccionar;
    private ComboBox CboPacientes;
    private Label lblTipoFiltro;
    private ComboBox CboTipoFiltro;
    private ComboBox CboMedicoFiltro;
    private Label lblMedicoFiltro;
    private CheckBox ChkFiltrarFecha;
    private DateTimePicker DtpFechaFiltro;
    private Button BtnNuevaConsulta;
    private Panel pnlCardPaciente;
    private Label lblPacienteDetalle;
    private Label lblResumenHistorial;
    private SplitContainer splitContainerHistorial;
    private Panel pnlGrilla;
    private Label lblHistorialTitulo;
    private DataGridView DgvHistorial;
    private Panel pnlDetalle;
    private TableLayoutPanel tblDetalle;
    private Label lblDetalleTitulo;
    private Label lblMotivo;
    private TextBox TxtMotivo;
    private Label lblDiagnostico;
    private TextBox TxtDiagnostico;
    private Label lblReceta;
    private TextBox TxtReceta;
}
