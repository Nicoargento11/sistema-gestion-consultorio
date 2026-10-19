namespace SGC.UI;

partial class FormTurnos
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
        pnlDerecho = new Panel();
        pnlIzquierdo = new Panel();
        pnlEstado = new Panel();
        LblMensaje = new Label();
        pnlMarcoNuevo = new Panel();
        pnlNuevoTurno = new Panel();
        tlpNuevoTurno = new TableLayoutPanel();
        lblTituloNuevo = new Label();
        LblModo = new Label();
        lblMedico = new Label();
        CboMedico = new ComboBox();
        lblFecha = new Label();
        DtpFecha = new DateTimePicker();
        lblHorarios = new Label();
        tlpHorarios = new TableLayoutPanel();
        LblAviso = new Label();
        FlpHorarios = new FlowLayoutPanel();
        FlpLeyenda = new FlowLayoutPanel();
        tlpPacienteDuracion = new TableLayoutPanel();
        lblPaciente = new Label();
        lblDuracion = new Label();
        CboPaciente = new ComboBox();
        CboDuracion = new ComboBox();
        tlpBotonesNuevo = new TableLayoutPanel();
        BtnAsignar = new Button();
        BtnLimpiar = new Button();
        tlpDerecho = new TableLayoutPanel();
        pnlMarcoToolbar = new Panel();
        pnlToolbar = new Panel();
        tlpToolbar = new TableLayoutPanel();
        LblVista = new Label();
        flpFiltros = new FlowLayoutPanel();
        flpPaciente = new FlowLayoutPanel();
        ChkTodosMedicos = new CheckBox();
        ChkFiltrarFecha = new CheckBox();
        ChkMostrarCancelados = new CheckBox();
        lblFiltroPaciente = new Label();
        CboFiltroPaciente = new ComboBox();
        FlpContadores = new FlowLayoutPanel();
        pnlMarcoGrilla = new Panel();
        DgvTurnos = new DataGridView();
        pnlMarcoAcciones = new Panel();
        pnlAcciones = new Panel();
        tlpAcciones = new TableLayoutPanel();
        LblSeleccion = new Label();
        flpBotonesAccion = new FlowLayoutPanel();
        BtnModificar = new Button();
        BtnCancelar = new Button();
        BtnConfirmarAsistencia = new Button();
        LblAyudaAcciones = new Label();
        pnlDerecho.SuspendLayout();
        pnlIzquierdo.SuspendLayout();
        pnlEstado.SuspendLayout();
        pnlMarcoNuevo.SuspendLayout();
        pnlNuevoTurno.SuspendLayout();
        tlpNuevoTurno.SuspendLayout();
        tlpHorarios.SuspendLayout();
        FlpHorarios.SuspendLayout();
        FlpLeyenda.SuspendLayout();
        tlpPacienteDuracion.SuspendLayout();
        tlpBotonesNuevo.SuspendLayout();
        tlpDerecho.SuspendLayout();
        pnlMarcoToolbar.SuspendLayout();
        pnlToolbar.SuspendLayout();
        tlpToolbar.SuspendLayout();
        flpFiltros.SuspendLayout();
        flpPaciente.SuspendLayout();
        FlpContadores.SuspendLayout();
        pnlMarcoGrilla.SuspendLayout();
        pnlMarcoAcciones.SuspendLayout();
        pnlAcciones.SuspendLayout();
        tlpAcciones.SuspendLayout();
        flpBotonesAccion.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvTurnos).BeginInit();
        SuspendLayout();
        // 
        // pnlDerecho
        // 
        pnlDerecho.Controls.Add(tlpDerecho);
        pnlDerecho.Dock = DockStyle.Fill;
        pnlDerecho.Padding = new Padding(0);
        pnlDerecho.TabIndex = 1;
        pnlDerecho.Name = "pnlDerecho";
        // 
        // pnlIzquierdo
        // 
        pnlIzquierdo.Controls.Add(pnlMarcoNuevo);
        pnlIzquierdo.Dock = DockStyle.Left;
        pnlIzquierdo.Padding = new Padding(0, 0, 12, 0);
        pnlIzquierdo.Size = new Size(312, 0);
        pnlIzquierdo.TabIndex = 0;
        pnlIzquierdo.Name = "pnlIzquierdo";
        // 
        // pnlEstado
        // 
        pnlEstado.Controls.Add(LblMensaje);
        pnlEstado.Dock = DockStyle.Bottom;
        pnlEstado.Padding = new Padding(2, 8, 2, 0);
        pnlEstado.Size = new Size(0, 36);
        pnlEstado.TabIndex = 2;
        pnlEstado.Name = "pnlEstado";
        // 
        // LblMensaje
        // 
        LblMensaje.Dock = DockStyle.Fill;
        LblMensaje.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        LblMensaje.TabIndex = 0;
        LblMensaje.Name = "LblMensaje";
        // 
        // pnlMarcoNuevo
        // 
        pnlMarcoNuevo.Controls.Add(pnlNuevoTurno);
        pnlMarcoNuevo.BackColor = Color.FromArgb(214, 220, 230);
        pnlMarcoNuevo.Dock = DockStyle.Fill;
        pnlMarcoNuevo.Padding = new Padding(1);
        pnlMarcoNuevo.TabIndex = 0;
        pnlMarcoNuevo.Name = "pnlMarcoNuevo";
        // 
        // pnlNuevoTurno
        // 
        pnlNuevoTurno.Controls.Add(tlpNuevoTurno);
        pnlNuevoTurno.AutoScroll = true;
        pnlNuevoTurno.BackColor = Color.White;
        pnlNuevoTurno.Dock = DockStyle.Fill;
        pnlNuevoTurno.Padding = new Padding(16, 14, 16, 14);
        pnlNuevoTurno.TabIndex = 0;
        pnlNuevoTurno.Name = "pnlNuevoTurno";
        // 
        // tlpNuevoTurno
        // 
        tlpNuevoTurno.Controls.Add(lblTituloNuevo, 0, 0);
        tlpNuevoTurno.Controls.Add(LblModo, 0, 1);
        tlpNuevoTurno.Controls.Add(lblMedico, 0, 2);
        tlpNuevoTurno.Controls.Add(CboMedico, 0, 3);
        tlpNuevoTurno.Controls.Add(lblFecha, 0, 4);
        tlpNuevoTurno.Controls.Add(DtpFecha, 0, 5);
        tlpNuevoTurno.Controls.Add(lblHorarios, 0, 6);
        tlpNuevoTurno.Controls.Add(tlpHorarios, 0, 7);
        tlpNuevoTurno.Controls.Add(FlpLeyenda, 0, 8);
        tlpNuevoTurno.Controls.Add(tlpPacienteDuracion, 0, 9);
        tlpNuevoTurno.Controls.Add(tlpBotonesNuevo, 0, 10);
        tlpNuevoTurno.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpNuevoTurno.RowCount = 11;
        tlpNuevoTurno.RowStyles.Add(new RowStyle());
        tlpNuevoTurno.RowStyles.Add(new RowStyle());
        tlpNuevoTurno.RowStyles.Add(new RowStyle());
        tlpNuevoTurno.RowStyles.Add(new RowStyle());
        tlpNuevoTurno.RowStyles.Add(new RowStyle());
        tlpNuevoTurno.RowStyles.Add(new RowStyle());
        tlpNuevoTurno.RowStyles.Add(new RowStyle());
        tlpNuevoTurno.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
        tlpNuevoTurno.RowStyles.Add(new RowStyle());
        tlpNuevoTurno.RowStyles.Add(new RowStyle());
        tlpNuevoTurno.RowStyles.Add(new RowStyle());
        tlpNuevoTurno.AutoSize = true;
        tlpNuevoTurno.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpNuevoTurno.ColumnCount = 1;
        tlpNuevoTurno.Dock = DockStyle.Top;
        tlpNuevoTurno.TabIndex = 0;
        tlpNuevoTurno.Name = "tlpNuevoTurno";
        // 
        // lblTituloNuevo
        // 
        lblTituloNuevo.AutoSize = true;
        lblTituloNuevo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblTituloNuevo.ForeColor = Color.FromArgb(27, 42, 74);
        lblTituloNuevo.Margin = new Padding(0, 0, 0, 4);
        lblTituloNuevo.Text = "NUEVO TURNO";
        lblTituloNuevo.TabIndex = 0;
        lblTituloNuevo.Name = "lblTituloNuevo";
        // 
        // LblModo
        // 
        LblModo.AutoSize = true;
        LblModo.Font = new Font("Segoe UI", 9F);
        LblModo.ForeColor = Color.FromArgb(176, 96, 0);
        LblModo.Margin = new Padding(0, 0, 0, 4);
        LblModo.TabIndex = 1;
        LblModo.Visible = false;
        LblModo.Name = "LblModo";
        // 
        // lblMedico
        // 
        lblMedico.AutoSize = true;
        lblMedico.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMedico.ForeColor = Color.FromArgb(27, 42, 74);
        lblMedico.Margin = new Padding(0, 10, 0, 3);
        lblMedico.Text = "1. Médico";
        lblMedico.TabIndex = 2;
        lblMedico.Name = "lblMedico";
        // 
        // CboMedico
        // 
        CboMedico.Dock = DockStyle.Fill;
        CboMedico.DropDownStyle = ComboBoxStyle.DropDownList;
        CboMedico.Font = new Font("Segoe UI", 10F);
        CboMedico.FormattingEnabled = true;
        CboMedico.Margin = new Padding(0, 0, 0, 4);
        CboMedico.TabIndex = 3;
        CboMedico.Name = "CboMedico";
        // 
        // lblFecha
        // 
        lblFecha.AutoSize = true;
        lblFecha.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblFecha.ForeColor = Color.FromArgb(27, 42, 74);
        lblFecha.Margin = new Padding(0, 10, 0, 3);
        lblFecha.Text = "2. Fecha";
        lblFecha.TabIndex = 4;
        lblFecha.Name = "lblFecha";
        // 
        // DtpFecha
        // 
        DtpFecha.Dock = DockStyle.Fill;
        DtpFecha.Font = new Font("Segoe UI", 10F);
        DtpFecha.Format = DateTimePickerFormat.Short;
        DtpFecha.Margin = new Padding(0, 0, 0, 4);
        DtpFecha.TabIndex = 5;
        DtpFecha.Name = "DtpFecha";
        // 
        // lblHorarios
        // 
        lblHorarios.AutoSize = true;
        lblHorarios.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblHorarios.ForeColor = Color.FromArgb(27, 42, 74);
        lblHorarios.Margin = new Padding(0, 10, 0, 3);
        lblHorarios.Text = "3. Horarios libres";
        lblHorarios.TabIndex = 6;
        lblHorarios.Name = "lblHorarios";
        // 
        // tlpHorarios
        // 
        tlpHorarios.Controls.Add(LblAviso, 0, 0);
        tlpHorarios.Controls.Add(FlpHorarios, 0, 1);
        tlpHorarios.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpHorarios.RowCount = 2;
        tlpHorarios.RowStyles.Add(new RowStyle());
        tlpHorarios.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpHorarios.ColumnCount = 1;
        tlpHorarios.Dock = DockStyle.Fill;
        tlpHorarios.Margin = new Padding(0, 0, 0, 4);
        tlpHorarios.TabIndex = 7;
        tlpHorarios.Name = "tlpHorarios";
        // 
        // LblAviso
        // 
        LblAviso.AutoSize = true;
        LblAviso.Font = new Font("Segoe UI", 9F);
        LblAviso.ForeColor = Color.FromArgb(176, 96, 0);
        LblAviso.Margin = new Padding(0, 0, 0, 4);
        LblAviso.TabIndex = 0;
        LblAviso.Visible = false;
        LblAviso.Name = "LblAviso";
        // 
        // FlpHorarios
        // 
        FlpHorarios.AutoScroll = true;
        FlpHorarios.Dock = DockStyle.Fill;
        FlpHorarios.Margin = new Padding(0);
        FlpHorarios.TabIndex = 1;
        FlpHorarios.Name = "FlpHorarios";
        // 
        // FlpLeyenda
        // 
        FlpLeyenda.Size = new Size(0, 30);
        FlpLeyenda.Dock = DockStyle.Fill;
        FlpLeyenda.Margin = new Padding(0, 2, 0, 2);
        FlpLeyenda.TabIndex = 8;
        FlpLeyenda.Name = "FlpLeyenda";
        // 
        // tlpPacienteDuracion
        // 
        tlpPacienteDuracion.Controls.Add(lblPaciente, 0, 0);
        tlpPacienteDuracion.Controls.Add(lblDuracion, 1, 0);
        tlpPacienteDuracion.Controls.Add(CboPaciente, 0, 1);
        tlpPacienteDuracion.Controls.Add(CboDuracion, 1, 1);
        tlpPacienteDuracion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpPacienteDuracion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112F));
        tlpPacienteDuracion.RowCount = 2;
        tlpPacienteDuracion.RowStyles.Add(new RowStyle());
        tlpPacienteDuracion.RowStyles.Add(new RowStyle());
        tlpPacienteDuracion.AutoSize = true;
        tlpPacienteDuracion.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpPacienteDuracion.ColumnCount = 2;
        tlpPacienteDuracion.Dock = DockStyle.Fill;
        tlpPacienteDuracion.Margin = new Padding(0, 0, 0, 12);
        tlpPacienteDuracion.TabIndex = 9;
        tlpPacienteDuracion.Name = "tlpPacienteDuracion";
        // 
        // lblPaciente
        // 
        lblPaciente.AutoSize = true;
        lblPaciente.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPaciente.ForeColor = Color.FromArgb(27, 42, 74);
        lblPaciente.Margin = new Padding(0, 10, 0, 3);
        lblPaciente.Text = "4. Paciente";
        lblPaciente.TabIndex = 0;
        lblPaciente.Name = "lblPaciente";
        // 
        // lblDuracion
        // 
        lblDuracion.AutoSize = true;
        lblDuracion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDuracion.ForeColor = Color.FromArgb(27, 42, 74);
        lblDuracion.Margin = new Padding(0, 10, 0, 3);
        lblDuracion.Text = "Duración (min)";
        lblDuracion.TabIndex = 1;
        lblDuracion.Name = "lblDuracion";
        // 
        // CboPaciente
        // 
        CboPaciente.Dock = DockStyle.Fill;
        CboPaciente.DropDownStyle = ComboBoxStyle.DropDownList;
        CboPaciente.Font = new Font("Segoe UI", 10F);
        CboPaciente.FormattingEnabled = true;
        CboPaciente.Margin = new Padding(0, 0, 8, 4);
        CboPaciente.TabIndex = 2;
        CboPaciente.Name = "CboPaciente";
        // 
        // CboDuracion
        // 
        CboDuracion.Dock = DockStyle.Fill;
        CboDuracion.DropDownStyle = ComboBoxStyle.DropDownList;
        CboDuracion.Font = new Font("Segoe UI", 10F);
        CboDuracion.FormattingEnabled = true;
        CboDuracion.Margin = new Padding(0, 0, 0, 4);
        CboDuracion.TabIndex = 3;
        CboDuracion.Name = "CboDuracion";
        // 
        // tlpBotonesNuevo
        // 
        tlpBotonesNuevo.Controls.Add(BtnAsignar, 0, 0);
        tlpBotonesNuevo.Controls.Add(BtnLimpiar, 1, 0);
        tlpBotonesNuevo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
        tlpBotonesNuevo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
        tlpBotonesNuevo.RowCount = 1;
        tlpBotonesNuevo.RowStyles.Add(new RowStyle());
        tlpBotonesNuevo.AutoSize = true;
        tlpBotonesNuevo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpBotonesNuevo.ColumnCount = 2;
        tlpBotonesNuevo.Dock = DockStyle.Fill;
        tlpBotonesNuevo.Margin = new Padding(0);
        tlpBotonesNuevo.TabIndex = 10;
        tlpBotonesNuevo.Name = "tlpBotonesNuevo";
        // 
        // BtnAsignar
        // 
        BtnAsignar.Dock = DockStyle.Fill;
        BtnAsignar.BackColor = Color.FromArgb(46, 134, 222);
        BtnAsignar.FlatAppearance.BorderSize = 0;
        BtnAsignar.FlatStyle = FlatStyle.Flat;
        BtnAsignar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        BtnAsignar.ForeColor = Color.White;
        BtnAsignar.Margin = new Padding(0, 0, 8, 0);
        BtnAsignar.Size = new Size(0, 40);
        BtnAsignar.Text = "Asignar turno";
        BtnAsignar.UseVisualStyleBackColor = false;
        BtnAsignar.TabIndex = 0;
        BtnAsignar.Name = "BtnAsignar";
        // 
        // BtnLimpiar
        // 
        BtnLimpiar.Dock = DockStyle.Fill;
        BtnLimpiar.BackColor = Color.White;
        BtnLimpiar.FlatAppearance.BorderColor = Color.FromArgb(214, 220, 230);
        BtnLimpiar.FlatAppearance.BorderSize = 1;
        BtnLimpiar.FlatStyle = FlatStyle.Flat;
        BtnLimpiar.Font = new Font("Segoe UI", 9.5F);
        BtnLimpiar.ForeColor = Color.FromArgb(27, 42, 74);
        BtnLimpiar.Margin = new Padding(0);
        BtnLimpiar.Size = new Size(0, 40);
        BtnLimpiar.Text = "Limpiar";
        BtnLimpiar.UseVisualStyleBackColor = false;
        BtnLimpiar.TabIndex = 1;
        BtnLimpiar.Name = "BtnLimpiar";
        // 
        // tlpDerecho
        // 
        tlpDerecho.Controls.Add(pnlMarcoToolbar, 0, 0);
        tlpDerecho.Controls.Add(FlpContadores, 0, 1);
        tlpDerecho.Controls.Add(pnlMarcoGrilla, 0, 2);
        tlpDerecho.Controls.Add(pnlMarcoAcciones, 0, 3);
        tlpDerecho.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpDerecho.RowCount = 4;
        tlpDerecho.RowStyles.Add(new RowStyle());
        tlpDerecho.RowStyles.Add(new RowStyle());
        tlpDerecho.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpDerecho.RowStyles.Add(new RowStyle());
        tlpDerecho.ColumnCount = 1;
        tlpDerecho.Dock = DockStyle.Fill;
        tlpDerecho.TabIndex = 0;
        tlpDerecho.Name = "tlpDerecho";
        // 
        // pnlMarcoToolbar
        // 
        pnlMarcoToolbar.Controls.Add(pnlToolbar);
        pnlMarcoToolbar.AutoSize = true;
        pnlMarcoToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlMarcoToolbar.BackColor = Color.FromArgb(214, 220, 230);
        pnlMarcoToolbar.Dock = DockStyle.Fill;
        pnlMarcoToolbar.Margin = new Padding(0, 0, 0, 8);
        pnlMarcoToolbar.Padding = new Padding(1);
        pnlMarcoToolbar.TabIndex = 0;
        pnlMarcoToolbar.Name = "pnlMarcoToolbar";
        // 
        // pnlToolbar
        // 
        pnlToolbar.Controls.Add(tlpToolbar);
        pnlToolbar.AutoSize = true;
        pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlToolbar.BackColor = Color.White;
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.Padding = new Padding(14, 8, 14, 6);
        pnlToolbar.TabIndex = 0;
        pnlToolbar.Name = "pnlToolbar";
        // 
        // tlpToolbar
        // 
        tlpToolbar.Controls.Add(LblVista, 0, 0);
        tlpToolbar.Controls.Add(flpFiltros, 0, 1);
        tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpToolbar.RowCount = 2;
        tlpToolbar.RowStyles.Add(new RowStyle());
        tlpToolbar.RowStyles.Add(new RowStyle());
        tlpToolbar.AutoSize = true;
        tlpToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpToolbar.ColumnCount = 1;
        tlpToolbar.Dock = DockStyle.Fill;
        tlpToolbar.TabIndex = 0;
        tlpToolbar.Name = "tlpToolbar";
        // 
        // LblVista
        // 
        LblVista.AutoSize = true;
        LblVista.Font = new Font("Segoe UI", 9F);
        LblVista.ForeColor = Color.FromArgb(110, 120, 135);
        LblVista.Margin = new Padding(0, 0, 0, 4);
        LblVista.TabIndex = 0;
        LblVista.Name = "LblVista";
        // 
        // flpFiltros
        // 
        flpFiltros.Controls.Add(ChkTodosMedicos);
        flpFiltros.Controls.Add(ChkFiltrarFecha);
        flpFiltros.Controls.Add(ChkMostrarCancelados);
        flpFiltros.Controls.Add(flpPaciente);
        flpFiltros.Size = new Size(0, 30);
        flpFiltros.Dock = DockStyle.Fill;
        flpFiltros.Margin = new Padding(0);
        flpFiltros.TabIndex = 2;
        flpFiltros.Name = "flpFiltros";
        // 
        // flpPaciente
        // 
        flpPaciente.Controls.Add(lblFiltroPaciente);
        flpPaciente.Controls.Add(CboFiltroPaciente);
        flpPaciente.AutoSize = true;
        flpPaciente.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flpPaciente.Margin = new Padding(0);
        flpPaciente.TabIndex = 3;
        flpPaciente.WrapContents = false;
        flpPaciente.Name = "flpPaciente";
        // 
        // ChkTodosMedicos
        // 
        ChkTodosMedicos.AutoSize = true;
        ChkTodosMedicos.Font = new Font("Segoe UI", 9.5F);
        ChkTodosMedicos.Margin = new Padding(0, 4, 18, 4);
        ChkTodosMedicos.TabIndex = 0;
        ChkTodosMedicos.Text = "Ver todos los médicos";
        ChkTodosMedicos.UseVisualStyleBackColor = true;
        ChkTodosMedicos.Name = "ChkTodosMedicos";
        // 
        // ChkFiltrarFecha
        // 
        ChkFiltrarFecha.AutoSize = true;
        ChkFiltrarFecha.Font = new Font("Segoe UI", 9.5F);
        ChkFiltrarFecha.Margin = new Padding(0, 4, 18, 4);
        ChkFiltrarFecha.TabIndex = 1;
        ChkFiltrarFecha.Text = "Filtrar por fecha";
        ChkFiltrarFecha.UseVisualStyleBackColor = true;
        ChkFiltrarFecha.Name = "ChkFiltrarFecha";
        // 
        // ChkMostrarCancelados
        // 
        ChkMostrarCancelados.AutoSize = true;
        ChkMostrarCancelados.Font = new Font("Segoe UI", 9.5F);
        ChkMostrarCancelados.Margin = new Padding(0, 4, 18, 4);
        ChkMostrarCancelados.TabIndex = 2;
        ChkMostrarCancelados.Text = "Mostrar cancelados";
        ChkMostrarCancelados.UseVisualStyleBackColor = true;
        ChkMostrarCancelados.Name = "ChkMostrarCancelados";
        // 
        // lblFiltroPaciente
        // 
        lblFiltroPaciente.AutoSize = true;
        lblFiltroPaciente.Font = new Font("Segoe UI", 9.5F);
        lblFiltroPaciente.Margin = new Padding(0, 7, 6, 0);
        lblFiltroPaciente.TabIndex = 0;
        lblFiltroPaciente.Text = "Paciente:";
        lblFiltroPaciente.Name = "lblFiltroPaciente";
        // 
        // CboFiltroPaciente
        // 
        CboFiltroPaciente.DropDownStyle = ComboBoxStyle.DropDownList;
        CboFiltroPaciente.Font = new Font("Segoe UI", 9.5F);
        CboFiltroPaciente.FormattingEnabled = true;
        CboFiltroPaciente.Margin = new Padding(0, 3, 0, 3);
        CboFiltroPaciente.Size = new Size(200, 25);
        CboFiltroPaciente.TabIndex = 1;
        CboFiltroPaciente.Name = "CboFiltroPaciente";
        // 
        // FlpContadores
        // 
        FlpContadores.Size = new Size(0, 30);
        FlpContadores.Dock = DockStyle.Fill;
        FlpContadores.Margin = new Padding(0, 0, 0, 4);
        FlpContadores.TabIndex = 1;
        FlpContadores.Name = "FlpContadores";
        // 
        // pnlMarcoGrilla
        // 
        pnlMarcoGrilla.Controls.Add(DgvTurnos);
        pnlMarcoGrilla.BackColor = Color.FromArgb(214, 220, 230);
        pnlMarcoGrilla.Dock = DockStyle.Fill;
        pnlMarcoGrilla.Margin = new Padding(0, 0, 0, 8);
        pnlMarcoGrilla.Padding = new Padding(1);
        pnlMarcoGrilla.TabIndex = 2;
        pnlMarcoGrilla.Name = "pnlMarcoGrilla";
        // 
        // DgvTurnos
        // 
        DgvTurnos.AllowUserToAddRows = false;
        DgvTurnos.AllowUserToDeleteRows = false;
        DgvTurnos.AllowUserToResizeRows = false;
        DgvTurnos.BackgroundColor = Color.White;
        DgvTurnos.BorderStyle = BorderStyle.None;
        DgvTurnos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        DgvTurnos.ColumnHeadersHeight = 40;
        DgvTurnos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        DgvTurnos.Dock = DockStyle.Fill;
        DgvTurnos.Font = new Font("Segoe UI", 9F);
        DgvTurnos.MultiSelect = false;
        DgvTurnos.ReadOnly = true;
        DgvTurnos.RowHeadersVisible = false;
        DgvTurnos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvTurnos.TabIndex = 0;
        DgvTurnos.Name = "DgvTurnos";
        // 
        // pnlMarcoAcciones
        // 
        pnlMarcoAcciones.Controls.Add(pnlAcciones);
        pnlMarcoAcciones.AutoSize = true;
        pnlMarcoAcciones.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlMarcoAcciones.BackColor = Color.FromArgb(214, 220, 230);
        pnlMarcoAcciones.Dock = DockStyle.Fill;
        pnlMarcoAcciones.Margin = new Padding(0);
        pnlMarcoAcciones.Padding = new Padding(1);
        pnlMarcoAcciones.TabIndex = 3;
        pnlMarcoAcciones.Name = "pnlMarcoAcciones";
        // 
        // pnlAcciones
        // 
        pnlAcciones.Controls.Add(tlpAcciones);
        pnlAcciones.AutoSize = true;
        pnlAcciones.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlAcciones.BackColor = Color.White;
        pnlAcciones.Dock = DockStyle.Fill;
        pnlAcciones.Padding = new Padding(14, 8, 14, 8);
        pnlAcciones.TabIndex = 0;
        pnlAcciones.Name = "pnlAcciones";
        // 
        // tlpAcciones
        // 
        tlpAcciones.Controls.Add(LblSeleccion, 0, 0);
        tlpAcciones.Controls.Add(flpBotonesAccion, 0, 1);
        tlpAcciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpAcciones.RowCount = 2;
        tlpAcciones.RowStyles.Add(new RowStyle());
        tlpAcciones.RowStyles.Add(new RowStyle());
        tlpAcciones.AutoSize = true;
        tlpAcciones.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpAcciones.ColumnCount = 1;
        tlpAcciones.Dock = DockStyle.Fill;
        tlpAcciones.TabIndex = 0;
        tlpAcciones.Name = "tlpAcciones";
        // 
        // LblSeleccion
        // 
        LblSeleccion.AutoSize = true;
        LblSeleccion.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        LblSeleccion.ForeColor = Color.FromArgb(27, 42, 74);
        LblSeleccion.Margin = new Padding(0, 0, 0, 6);
        LblSeleccion.TabIndex = 0;
        LblSeleccion.Text = "Turno seleccionado: ninguno";
        LblSeleccion.Name = "LblSeleccion";
        // 
        // flpBotonesAccion
        // 
        flpBotonesAccion.Controls.Add(BtnModificar);
        flpBotonesAccion.Controls.Add(BtnCancelar);
        flpBotonesAccion.Controls.Add(BtnConfirmarAsistencia);
        flpBotonesAccion.Controls.Add(LblAyudaAcciones);
        flpBotonesAccion.Size = new Size(0, 30);
        flpBotonesAccion.Dock = DockStyle.Fill;
        flpBotonesAccion.Margin = new Padding(0);
        flpBotonesAccion.TabIndex = 1;
        flpBotonesAccion.Name = "flpBotonesAccion";
        // 
        // BtnModificar
        // 
        BtnModificar.BackColor = Color.FromArgb(230, 145, 45);
        BtnModificar.FlatAppearance.BorderSize = 0;
        BtnModificar.FlatStyle = FlatStyle.Flat;
        BtnModificar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnModificar.ForeColor = Color.White;
        BtnModificar.Margin = new Padding(0, 0, 8, 0);
        BtnModificar.Size = new Size(116, 32);
        BtnModificar.Text = "Modificar";
        BtnModificar.UseVisualStyleBackColor = false;
        BtnModificar.TabIndex = 0;
        BtnModificar.Name = "BtnModificar";
        // 
        // BtnCancelar
        // 
        BtnCancelar.BackColor = Color.FromArgb(231, 76, 60);
        BtnCancelar.FlatAppearance.BorderSize = 0;
        BtnCancelar.FlatStyle = FlatStyle.Flat;
        BtnCancelar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnCancelar.ForeColor = Color.White;
        BtnCancelar.Margin = new Padding(0, 0, 8, 0);
        BtnCancelar.Size = new Size(136, 32);
        BtnCancelar.Text = "Cancelar turno";
        BtnCancelar.UseVisualStyleBackColor = false;
        BtnCancelar.TabIndex = 1;
        BtnCancelar.Name = "BtnCancelar";
        // 
        // BtnConfirmarAsistencia
        // 
        BtnConfirmarAsistencia.BackColor = Color.FromArgb(39, 174, 96);
        BtnConfirmarAsistencia.FlatAppearance.BorderSize = 0;
        BtnConfirmarAsistencia.FlatStyle = FlatStyle.Flat;
        BtnConfirmarAsistencia.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnConfirmarAsistencia.ForeColor = Color.White;
        BtnConfirmarAsistencia.Margin = new Padding(0, 0, 14, 0);
        BtnConfirmarAsistencia.Size = new Size(176, 32);
        BtnConfirmarAsistencia.Text = "Confirmar asistencia";
        BtnConfirmarAsistencia.UseVisualStyleBackColor = false;
        BtnConfirmarAsistencia.TabIndex = 2;
        BtnConfirmarAsistencia.Name = "BtnConfirmarAsistencia";
        // 
        // LblAyudaAcciones
        // 
        LblAyudaAcciones.AutoSize = true;
        LblAyudaAcciones.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        LblAyudaAcciones.ForeColor = Color.FromArgb(110, 120, 135);
        LblAyudaAcciones.Margin = new Padding(0, 7, 0, 0);
        LblAyudaAcciones.TabIndex = 3;
        LblAyudaAcciones.Visible = false;
        LblAyudaAcciones.Name = "LblAyudaAcciones";
        // 
        // FormTurnos
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(1130, 650);
        Controls.Add(pnlDerecho);
        Controls.Add(pnlIzquierdo);
        Controls.Add(pnlEstado);
        Name = "FormTurnos";
        Padding = new Padding(16, 14, 16, 12);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Turnos";
        flpBotonesAccion.ResumeLayout(false);
        tlpAcciones.ResumeLayout(false);
        tlpAcciones.PerformLayout();
        pnlAcciones.ResumeLayout(false);
        pnlAcciones.PerformLayout();
        pnlMarcoAcciones.ResumeLayout(false);
        pnlMarcoAcciones.PerformLayout();
        pnlMarcoGrilla.ResumeLayout(false);
        FlpContadores.ResumeLayout(false);
        flpPaciente.ResumeLayout(false);
        flpPaciente.PerformLayout();
        flpFiltros.ResumeLayout(false);
        tlpToolbar.ResumeLayout(false);
        tlpToolbar.PerformLayout();
        pnlToolbar.ResumeLayout(false);
        pnlToolbar.PerformLayout();
        pnlMarcoToolbar.ResumeLayout(false);
        pnlMarcoToolbar.PerformLayout();
        tlpDerecho.ResumeLayout(false);
        tlpDerecho.PerformLayout();
        tlpBotonesNuevo.ResumeLayout(false);
        tlpBotonesNuevo.PerformLayout();
        tlpPacienteDuracion.ResumeLayout(false);
        tlpPacienteDuracion.PerformLayout();
        FlpLeyenda.ResumeLayout(false);
        FlpHorarios.ResumeLayout(false);
        tlpHorarios.ResumeLayout(false);
        tlpHorarios.PerformLayout();
        tlpNuevoTurno.ResumeLayout(false);
        tlpNuevoTurno.PerformLayout();
        pnlNuevoTurno.ResumeLayout(false);
        pnlMarcoNuevo.ResumeLayout(false);
        pnlEstado.ResumeLayout(false);
        pnlIzquierdo.ResumeLayout(false);
        pnlDerecho.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)DgvTurnos).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlDerecho;
    private Panel pnlIzquierdo;
    private Panel pnlEstado;
    private Label LblMensaje;
    private Panel pnlMarcoNuevo;
    private Panel pnlNuevoTurno;
    private TableLayoutPanel tlpNuevoTurno;
    private Label lblTituloNuevo;
    private Label LblModo;
    private Label lblMedico;
    private ComboBox CboMedico;
    private Label lblFecha;
    private DateTimePicker DtpFecha;
    private Label lblHorarios;
    private TableLayoutPanel tlpHorarios;
    private Label LblAviso;
    private FlowLayoutPanel FlpHorarios;
    private FlowLayoutPanel FlpLeyenda;
    private TableLayoutPanel tlpPacienteDuracion;
    private Label lblPaciente;
    private Label lblDuracion;
    private ComboBox CboPaciente;
    private ComboBox CboDuracion;
    private TableLayoutPanel tlpBotonesNuevo;
    private Button BtnAsignar;
    private Button BtnLimpiar;
    private TableLayoutPanel tlpDerecho;
    private Panel pnlMarcoToolbar;
    private Panel pnlToolbar;
    private TableLayoutPanel tlpToolbar;
    private Label LblVista;
    private FlowLayoutPanel flpFiltros;
    private FlowLayoutPanel flpPaciente;
    private CheckBox ChkTodosMedicos;
    private CheckBox ChkFiltrarFecha;
    private CheckBox ChkMostrarCancelados;
    private Label lblFiltroPaciente;
    private ComboBox CboFiltroPaciente;
    private FlowLayoutPanel FlpContadores;
    private Panel pnlMarcoGrilla;
    private DataGridView DgvTurnos;
    private Panel pnlMarcoAcciones;
    private Panel pnlAcciones;
    private TableLayoutPanel tlpAcciones;
    private Label LblSeleccion;
    private FlowLayoutPanel flpBotonesAccion;
    private Button BtnModificar;
    private Button BtnCancelar;
    private Button BtnConfirmarAsistencia;
    private Label LblAyudaAcciones;
}
