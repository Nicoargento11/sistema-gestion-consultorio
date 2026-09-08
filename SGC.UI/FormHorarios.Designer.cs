namespace SGC.UI;

partial class FormHorarios
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
        lblMedico = new Label();
        CboMedico = new ComboBox();
        lblDia = new Label();
        CboDia = new ComboBox();
        lblEntrada = new Label();
        DtpEntrada = new DateTimePicker();
        lblSalida = new Label();
        DtpSalida = new DateTimePicker();
        BtnNuevo = new Button();
        BtnGuardar = new Button();
        BtnEliminar = new Button();
        BtnConsultar = new Button();
        LblMensaje = new Label();
        ChkFiltrarFecha = new CheckBox();
        DtpFechaConsulta = new DateTimePicker();
        lblPacienteFiltro = new Label();
        CboPacienteFiltro = new ComboBox();
        DgvHorarios = new DataGridView();
        pnlHeader.SuspendLayout();
        pnlFormulario.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvHorarios).BeginInit();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(27, 42, 74);
        pnlHeader.Controls.Add(lblSubtitulo);
        pnlHeader.Controls.Add(lblTitulo);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Size = new Size(1000, 70);
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(18, 8);
        lblTitulo.Text = "Control de Horarios";
        // 
        // lblSubtitulo
        // 
        lblSubtitulo.AutoSize = true;
        lblSubtitulo.Font = new Font("Segoe UI", 9.5F);
        lblSubtitulo.ForeColor = Color.FromArgb(180, 205, 235);
        lblSubtitulo.Location = new Point(20, 40);
        lblSubtitulo.Text = "Alta, baja, modificacion y consulta de bloques de atencion (RF 01 a 04)";
        // 
        // pnlFormulario
        // 
        pnlFormulario.BackColor = Color.FromArgb(245, 246, 250);
        pnlFormulario.Controls.Add(CboPacienteFiltro);
        pnlFormulario.Controls.Add(lblPacienteFiltro);
        pnlFormulario.Controls.Add(DtpFechaConsulta);
        pnlFormulario.Controls.Add(ChkFiltrarFecha);
        pnlFormulario.Controls.Add(LblMensaje);
        pnlFormulario.Controls.Add(BtnConsultar);
        pnlFormulario.Controls.Add(BtnEliminar);
        pnlFormulario.Controls.Add(BtnGuardar);
        pnlFormulario.Controls.Add(BtnNuevo);
        pnlFormulario.Controls.Add(DtpSalida);
        pnlFormulario.Controls.Add(lblSalida);
        pnlFormulario.Controls.Add(DtpEntrada);
        pnlFormulario.Controls.Add(lblEntrada);
        pnlFormulario.Controls.Add(CboDia);
        pnlFormulario.Controls.Add(lblDia);
        pnlFormulario.Controls.Add(CboMedico);
        pnlFormulario.Controls.Add(lblMedico);
        pnlFormulario.Dock = DockStyle.Top;
        pnlFormulario.Size = new Size(1000, 175);
        // 
        // lblMedico
        // 
        lblMedico.AutoSize = true;
        lblMedico.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMedico.Location = new Point(20, 12);
        lblMedico.Text = "Medico";
        // 
        // CboMedico
        // 
        CboMedico.DropDownStyle = ComboBoxStyle.DropDownList;
        CboMedico.Font = new Font("Segoe UI", 9.5F);
        CboMedico.Location = new Point(20, 34);
        CboMedico.Size = new Size(280, 29);
        CboMedico.TabIndex = 0;
        // 
        // lblDia
        // 
        lblDia.AutoSize = true;
        lblDia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDia.Location = new Point(320, 12);
        lblDia.Text = "Dia";
        // 
        // CboDia
        // 
        CboDia.DropDownStyle = ComboBoxStyle.DropDownList;
        CboDia.Font = new Font("Segoe UI", 9.5F);
        CboDia.Location = new Point(320, 34);
        CboDia.Size = new Size(150, 29);
        CboDia.TabIndex = 1;
        // 
        // lblEntrada
        // 
        lblEntrada.AutoSize = true;
        lblEntrada.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblEntrada.Location = new Point(490, 12);
        lblEntrada.Text = "Hora entrada";
        // 
        // DtpEntrada
        // 
        DtpEntrada.CustomFormat = "HH:mm";
        DtpEntrada.Format = DateTimePickerFormat.Custom;
        DtpEntrada.Location = new Point(490, 34);
        DtpEntrada.ShowUpDown = true;
        DtpEntrada.Size = new Size(100, 27);
        DtpEntrada.TabIndex = 2;
        DtpEntrada.Value = new DateTime(2026, 1, 1, 8, 0, 0);
        // 
        // lblSalida
        // 
        lblSalida.AutoSize = true;
        lblSalida.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblSalida.Location = new Point(610, 12);
        lblSalida.Text = "Hora salida";
        // 
        // DtpSalida
        // 
        DtpSalida.CustomFormat = "HH:mm";
        DtpSalida.Format = DateTimePickerFormat.Custom;
        DtpSalida.Location = new Point(610, 34);
        DtpSalida.ShowUpDown = true;
        DtpSalida.Size = new Size(100, 27);
        DtpSalida.TabIndex = 3;
        DtpSalida.Value = new DateTime(2026, 1, 1, 8, 30, 0);
        // 
        // BtnNuevo
        // 
        BtnNuevo.BackColor = Color.FromArgb(46, 134, 222);
        BtnNuevo.FlatAppearance.BorderSize = 0;
        BtnNuevo.FlatStyle = FlatStyle.Flat;
        BtnNuevo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        BtnNuevo.ForeColor = Color.White;
        BtnNuevo.Location = new Point(20, 78);
        BtnNuevo.Size = new Size(100, 32);
        BtnNuevo.TabIndex = 4;
        BtnNuevo.Text = "Nuevo";
        BtnNuevo.UseVisualStyleBackColor = false;
        // 
        // BtnGuardar
        // 
        BtnGuardar.BackColor = Color.FromArgb(39, 174, 96);
        BtnGuardar.FlatAppearance.BorderSize = 0;
        BtnGuardar.FlatStyle = FlatStyle.Flat;
        BtnGuardar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        BtnGuardar.ForeColor = Color.White;
        BtnGuardar.Location = new Point(130, 78);
        BtnGuardar.Size = new Size(100, 32);
        BtnGuardar.TabIndex = 5;
        BtnGuardar.Text = "Guardar";
        BtnGuardar.UseVisualStyleBackColor = false;
        // 
        // BtnEliminar
        // 
        BtnEliminar.BackColor = Color.FromArgb(200, 60, 60);
        BtnEliminar.FlatAppearance.BorderSize = 0;
        BtnEliminar.FlatStyle = FlatStyle.Flat;
        BtnEliminar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        BtnEliminar.ForeColor = Color.White;
        BtnEliminar.Location = new Point(240, 78);
        BtnEliminar.Size = new Size(100, 32);
        BtnEliminar.TabIndex = 6;
        BtnEliminar.Text = "Eliminar";
        BtnEliminar.UseVisualStyleBackColor = false;
        // 
        // BtnConsultar
        // 
        BtnConsultar.BackColor = Color.FromArgb(27, 42, 74);
        BtnConsultar.FlatAppearance.BorderSize = 0;
        BtnConsultar.FlatStyle = FlatStyle.Flat;
        BtnConsultar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        BtnConsultar.ForeColor = Color.White;
        BtnConsultar.Location = new Point(350, 78);
        BtnConsultar.Size = new Size(110, 32);
        BtnConsultar.TabIndex = 7;
        BtnConsultar.Text = "Consultar";
        BtnConsultar.UseVisualStyleBackColor = false;
        // 
        // ChkFiltrarFecha
        // 
        ChkFiltrarFecha.AutoSize = true;
        ChkFiltrarFecha.Location = new Point(20, 122);
        ChkFiltrarFecha.Text = "Filtrar por fecha";
        // 
        // DtpFechaConsulta
        // 
        DtpFechaConsulta.Format = DateTimePickerFormat.Short;
        DtpFechaConsulta.Location = new Point(160, 118);
        DtpFechaConsulta.Size = new Size(120, 27);
        // 
        // lblPacienteFiltro
        // 
        lblPacienteFiltro.AutoSize = true;
        lblPacienteFiltro.Location = new Point(300, 122);
        lblPacienteFiltro.Text = "Paciente:";
        // 
        // CboPacienteFiltro
        // 
        CboPacienteFiltro.DropDownStyle = ComboBoxStyle.DropDownList;
        CboPacienteFiltro.Location = new Point(370, 118);
        CboPacienteFiltro.Size = new Size(250, 29);
        // 
        // LblMensaje
        // 
        LblMensaje.AutoSize = true;
        LblMensaje.Location = new Point(640, 84);
        LblMensaje.MaximumSize = new Size(340, 0);
        // 
        // DgvHorarios
        // 
        DgvHorarios.AllowUserToAddRows = false;
        DgvHorarios.AllowUserToDeleteRows = false;
        DgvHorarios.BackgroundColor = Color.White;
        DgvHorarios.Dock = DockStyle.Fill;
        DgvHorarios.MultiSelect = false;
        DgvHorarios.ReadOnly = true;
        DgvHorarios.RowHeadersVisible = false;
        DgvHorarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        // 
        // FormHorarios
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 620);
        Controls.Add(DgvHorarios);
        Controls.Add(pnlFormulario);
        Controls.Add(pnlHeader);
        Name = "FormHorarios";
        Text = "Control de Horarios";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlFormulario.ResumeLayout(false);
        pnlFormulario.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)DgvHorarios).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlHeader;
    private Label lblTitulo;
    private Label lblSubtitulo;
    private Panel pnlFormulario;
    private Label lblMedico;
    private ComboBox CboMedico;
    private Label lblDia;
    private ComboBox CboDia;
    private Label lblEntrada;
    private DateTimePicker DtpEntrada;
    private Label lblSalida;
    private DateTimePicker DtpSalida;
    private Button BtnNuevo;
    private Button BtnGuardar;
    private Button BtnEliminar;
    private Button BtnConsultar;
    private Label LblMensaje;
    private CheckBox ChkFiltrarFecha;
    private DateTimePicker DtpFechaConsulta;
    private Label lblPacienteFiltro;
    private ComboBox CboPacienteFiltro;
    private DataGridView DgvHorarios;
}
