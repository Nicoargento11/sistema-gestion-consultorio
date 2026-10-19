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
        pnlDerecho = new Panel();
        tlpDerecho = new TableLayoutPanel();
        pnlMarcoToolbar = new Panel();
        pnlToolbar = new Panel();
        tlpToolbar = new TableLayoutPanel();
        flpFiltros = new FlowLayoutPanel();
        lblFiltroMedico = new Label();
        CboFiltroMedico = new ComboBox();
        FlpContadores = new FlowLayoutPanel();
        pnlMarcoGrilla = new Panel();
        TabHorarios = new TabControl();
        TabSemanal = new TabPage();
        DgvSemanal = new DataGridView();
        TabListado = new TabPage();
        DgvAgenda = new DataGridView();
        pnlMarcoAcciones = new Panel();
        pnlAcciones = new Panel();
        tlpAcciones = new TableLayoutPanel();
        LblSeleccion = new Label();
        flpBotonesAccion = new FlowLayoutPanel();
        BtnEliminar = new Button();
        LblAyudaAcciones = new Label();
        LblDetalle = new Label();
        pnlIzquierdo = new Panel();
        pnlMarcoTarjeta = new Panel();
        pnlTarjeta = new Panel();
        pnlCampos = new Panel();
        tlpTarjeta = new TableLayoutPanel();
        lblTituloTarjeta = new Label();
        LblModo = new Label();
        lblMedico = new Label();
        CboMedico = new ComboBox();
        lblDiaSemana = new Label();
        CboDiaSemana = new ComboBox();
        lblPasoHoras = new Label();
        lblHoraInicio = new Label();
        lblHoraFin = new Label();
        DtpHoraInicio = new DateTimePicker();
        DtpHoraFin = new DateTimePicker();
        pnlBotones = new Panel();
        tlpBotonesTarjeta = new TableLayoutPanel();
        BtnGuardar = new Button();
        BtnNuevo = new Button();
        pnlEstado = new Panel();
        LblMensaje = new Label();
        pnlDerecho.SuspendLayout();
        tlpDerecho.SuspendLayout();
        pnlMarcoToolbar.SuspendLayout();
        pnlToolbar.SuspendLayout();
        tlpToolbar.SuspendLayout();
        flpFiltros.SuspendLayout();
        FlpContadores.SuspendLayout();
        pnlMarcoGrilla.SuspendLayout();
        TabHorarios.SuspendLayout();
        TabSemanal.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvSemanal).BeginInit();
        TabListado.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvAgenda).BeginInit();
        pnlMarcoAcciones.SuspendLayout();
        pnlAcciones.SuspendLayout();
        tlpAcciones.SuspendLayout();
        flpBotonesAccion.SuspendLayout();
        pnlIzquierdo.SuspendLayout();
        pnlMarcoTarjeta.SuspendLayout();
        pnlTarjeta.SuspendLayout();
        pnlCampos.SuspendLayout();
        tlpTarjeta.SuspendLayout();
        pnlBotones.SuspendLayout();
        tlpBotonesTarjeta.SuspendLayout();
        pnlEstado.SuspendLayout();
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
        pnlMarcoToolbar.Margin = new Padding(0, 0, 0, 6);
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
        pnlToolbar.Padding = new Padding(12, 5, 12, 3);
        pnlToolbar.TabIndex = 0;
        pnlToolbar.Name = "pnlToolbar";
        //
        // tlpToolbar
        //
        tlpToolbar.Controls.Add(flpFiltros, 0, 0);
        tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpToolbar.RowCount = 1;
        tlpToolbar.RowStyles.Add(new RowStyle());
        tlpToolbar.ColumnCount = 1;
        tlpToolbar.AutoSize = true;
        tlpToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpToolbar.Dock = DockStyle.Fill;
        tlpToolbar.TabIndex = 0;
        tlpToolbar.Name = "tlpToolbar";
        //
        // flpFiltros
        //
        flpFiltros.Controls.Add(lblFiltroMedico);
        flpFiltros.Controls.Add(CboFiltroMedico);
        flpFiltros.Size = new Size(0, 30);
        flpFiltros.Dock = DockStyle.Fill;
        flpFiltros.Margin = new Padding(0);
        flpFiltros.TabIndex = 0;
        flpFiltros.Name = "flpFiltros";
        //
        // lblFiltroMedico
        //
        lblFiltroMedico.AutoSize = true;
        lblFiltroMedico.Font = new Font("Segoe UI", 9.5F);
        lblFiltroMedico.Margin = new Padding(0, 7, 6, 0);
        lblFiltroMedico.Text = "Médico:";
        lblFiltroMedico.TabIndex = 0;
        lblFiltroMedico.Name = "lblFiltroMedico";
        //
        // CboFiltroMedico
        //
        CboFiltroMedico.DropDownStyle = ComboBoxStyle.DropDownList;
        CboFiltroMedico.Font = new Font("Segoe UI", 9.5F);
        CboFiltroMedico.FormattingEnabled = true;
        CboFiltroMedico.Margin = new Padding(0, 3, 0, 3);
        CboFiltroMedico.Size = new Size(300, 29);
        CboFiltroMedico.TabIndex = 1;
        CboFiltroMedico.Name = "CboFiltroMedico";
        //
        // FlpContadores
        //
        FlpContadores.Size = new Size(0, 30);
        FlpContadores.Dock = DockStyle.Fill;
        FlpContadores.Margin = new Padding(0, 0, 0, 2);
        FlpContadores.TabIndex = 1;
        FlpContadores.Name = "FlpContadores";
        //
        // pnlMarcoGrilla
        //
        pnlMarcoGrilla.Controls.Add(TabHorarios);
        pnlMarcoGrilla.BackColor = Color.FromArgb(245, 246, 250);
        pnlMarcoGrilla.Dock = DockStyle.Fill;
        pnlMarcoGrilla.Margin = new Padding(0, 0, 0, 6);
        pnlMarcoGrilla.Padding = new Padding(0);
        pnlMarcoGrilla.TabIndex = 2;
        pnlMarcoGrilla.Name = "pnlMarcoGrilla";
        //
        // TabHorarios
        //
        TabHorarios.Controls.Add(TabSemanal);
        TabHorarios.Controls.Add(TabListado);
        TabHorarios.Dock = DockStyle.Fill;
        TabHorarios.Font = new Font("Segoe UI", 9.5F);
        TabHorarios.Padding = new Point(18, 6);
        TabHorarios.TabIndex = 0;
        TabHorarios.Name = "TabHorarios";
        //
        // TabSemanal
        //
        TabSemanal.Controls.Add(DgvSemanal);
        TabSemanal.BackColor = Color.White;
        TabSemanal.Padding = new Padding(0);
        TabSemanal.Text = "Vista semanal";
        TabSemanal.TabIndex = 0;
        TabSemanal.Name = "TabSemanal";
        //
        // DgvSemanal
        //
        DgvSemanal.AllowUserToAddRows = false;
        DgvSemanal.AllowUserToDeleteRows = false;
        DgvSemanal.AllowUserToResizeRows = false;
        DgvSemanal.BackgroundColor = Color.White;
        DgvSemanal.BorderStyle = BorderStyle.None;
        DgvSemanal.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        DgvSemanal.ColumnHeadersHeight = 40;
        DgvSemanal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        DgvSemanal.Dock = DockStyle.Fill;
        DgvSemanal.Font = new Font("Segoe UI", 9F);
        DgvSemanal.MultiSelect = false;
        DgvSemanal.ReadOnly = true;
        DgvSemanal.RowHeadersVisible = false;
        DgvSemanal.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvSemanal.TabIndex = 0;
        DgvSemanal.Name = "DgvSemanal";
        //
        // TabListado
        //
        TabListado.Controls.Add(DgvAgenda);
        TabListado.BackColor = Color.White;
        TabListado.Padding = new Padding(0);
        TabListado.Text = "Listado";
        TabListado.TabIndex = 1;
        TabListado.Name = "TabListado";
        //
        // DgvAgenda
        //
        DgvAgenda.AllowUserToAddRows = false;
        DgvAgenda.AllowUserToDeleteRows = false;
        DgvAgenda.AllowUserToResizeRows = false;
        DgvAgenda.BackgroundColor = Color.White;
        DgvAgenda.BorderStyle = BorderStyle.None;
        DgvAgenda.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        DgvAgenda.ColumnHeadersHeight = 40;
        DgvAgenda.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        DgvAgenda.Dock = DockStyle.Fill;
        DgvAgenda.Font = new Font("Segoe UI", 9F);
        DgvAgenda.MultiSelect = false;
        DgvAgenda.ReadOnly = true;
        DgvAgenda.RowHeadersVisible = false;
        DgvAgenda.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvAgenda.TabIndex = 0;
        DgvAgenda.Name = "DgvAgenda";
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
        pnlAcciones.Padding = new Padding(12, 5, 12, 5);
        pnlAcciones.TabIndex = 0;
        pnlAcciones.Name = "pnlAcciones";
        //
        // tlpAcciones
        //
        tlpAcciones.Controls.Add(LblSeleccion, 0, 0);
        tlpAcciones.Controls.Add(flpBotonesAccion, 1, 0);
        tlpAcciones.Controls.Add(LblDetalle, 0, 1);
        tlpAcciones.SetColumnSpan(LblDetalle, 2);
        tlpAcciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpAcciones.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlpAcciones.RowCount = 2;
        tlpAcciones.RowStyles.Add(new RowStyle());
        tlpAcciones.RowStyles.Add(new RowStyle());
        tlpAcciones.ColumnCount = 2;
        tlpAcciones.AutoSize = true;
        tlpAcciones.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpAcciones.Dock = DockStyle.Fill;
        tlpAcciones.TabIndex = 0;
        tlpAcciones.Name = "tlpAcciones";
        //
        // LblSeleccion
        //
        LblSeleccion.AutoEllipsis = true;
        LblSeleccion.AutoSize = false;
        LblSeleccion.Dock = DockStyle.Fill;
        LblSeleccion.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        LblSeleccion.ForeColor = Color.FromArgb(27, 42, 74);
        LblSeleccion.Margin = new Padding(0, 0, 10, 0);
        LblSeleccion.TextAlign = ContentAlignment.MiddleLeft;
        LblSeleccion.TabIndex = 0;
        LblSeleccion.Name = "LblSeleccion";
        //
        // flpBotonesAccion
        //
        flpBotonesAccion.Controls.Add(BtnEliminar);
        flpBotonesAccion.Controls.Add(LblAyudaAcciones);
        flpBotonesAccion.AutoSize = true;
        flpBotonesAccion.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flpBotonesAccion.Dock = DockStyle.Fill;
        flpBotonesAccion.Margin = new Padding(0);
        flpBotonesAccion.WrapContents = false;
        flpBotonesAccion.TabIndex = 1;
        flpBotonesAccion.Name = "flpBotonesAccion";
        //
        // BtnEliminar
        //
        BtnEliminar.BackColor = Color.FromArgb(231, 76, 60);
        BtnEliminar.FlatAppearance.BorderSize = 0;
        BtnEliminar.FlatStyle = FlatStyle.Flat;
        BtnEliminar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnEliminar.ForeColor = Color.White;
        BtnEliminar.Margin = new Padding(0, 0, 14, 0);
        BtnEliminar.Size = new Size(116, 30);
        BtnEliminar.Text = "Eliminar";
        BtnEliminar.UseVisualStyleBackColor = false;
        BtnEliminar.TabIndex = 0;
        BtnEliminar.Name = "BtnEliminar";
        //
        // LblAyudaAcciones
        //
        LblAyudaAcciones.AutoSize = true;
        LblAyudaAcciones.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        LblAyudaAcciones.ForeColor = Color.FromArgb(110, 120, 135);
        LblAyudaAcciones.Margin = new Padding(0, 7, 0, 0);
        LblAyudaAcciones.Visible = false;
        LblAyudaAcciones.TabIndex = 1;
        LblAyudaAcciones.Name = "LblAyudaAcciones";
        //
        // LblDetalle
        //
        LblDetalle.AutoSize = true;
        LblDetalle.Font = new Font("Segoe UI", 9.5F);
        LblDetalle.ForeColor = Color.FromArgb(27, 42, 74);
        LblDetalle.Margin = new Padding(0, 4, 0, 0);
        LblDetalle.TabIndex = 2;
        LblDetalle.Name = "LblDetalle";
        //
        // pnlIzquierdo
        //
        pnlIzquierdo.Controls.Add(pnlMarcoTarjeta);
        pnlIzquierdo.Dock = DockStyle.Left;
        pnlIzquierdo.Padding = new Padding(0, 0, 12, 0);
        pnlIzquierdo.Size = new Size(250, 0);
        pnlIzquierdo.TabIndex = 0;
        pnlIzquierdo.Name = "pnlIzquierdo";
        //
        // pnlMarcoTarjeta
        //
        pnlMarcoTarjeta.Controls.Add(pnlTarjeta);
        pnlMarcoTarjeta.BackColor = Color.FromArgb(214, 220, 230);
        pnlMarcoTarjeta.Dock = DockStyle.Fill;
        pnlMarcoTarjeta.Padding = new Padding(1);
        pnlMarcoTarjeta.TabIndex = 0;
        pnlMarcoTarjeta.Name = "pnlMarcoTarjeta";
        //
        // pnlTarjeta
        //
        pnlTarjeta.Controls.Add(pnlCampos);
        pnlTarjeta.Controls.Add(pnlBotones);
        pnlTarjeta.BackColor = Color.White;
        pnlTarjeta.Dock = DockStyle.Fill;
        pnlTarjeta.Padding = new Padding(16, 12, 16, 12);
        pnlTarjeta.TabIndex = 0;
        pnlTarjeta.Name = "pnlTarjeta";
        //
        // pnlCampos
        //
        pnlCampos.Controls.Add(tlpTarjeta);
        pnlCampos.AutoScroll = true;
        pnlCampos.Dock = DockStyle.Fill;
        pnlCampos.Padding = new Padding(0);
        pnlCampos.TabIndex = 0;
        pnlCampos.Name = "pnlCampos";
        //
        // tlpTarjeta
        //
        tlpTarjeta.Controls.Add(lblTituloTarjeta, 0, 0);
        tlpTarjeta.Controls.Add(LblModo, 0, 1);
        tlpTarjeta.Controls.Add(lblMedico, 0, 2);
        tlpTarjeta.Controls.Add(CboMedico, 0, 3);
        tlpTarjeta.Controls.Add(lblDiaSemana, 0, 4);
        tlpTarjeta.Controls.Add(CboDiaSemana, 0, 5);
        tlpTarjeta.Controls.Add(lblPasoHoras, 0, 6);
        tlpTarjeta.Controls.Add(lblHoraInicio, 0, 7);
        tlpTarjeta.Controls.Add(lblHoraFin, 1, 7);
        tlpTarjeta.Controls.Add(DtpHoraInicio, 0, 8);
        tlpTarjeta.Controls.Add(DtpHoraFin, 1, 8);
        tlpTarjeta.SetColumnSpan(lblTituloTarjeta, 2);
        tlpTarjeta.SetColumnSpan(LblModo, 2);
        tlpTarjeta.SetColumnSpan(lblMedico, 2);
        tlpTarjeta.SetColumnSpan(CboMedico, 2);
        tlpTarjeta.SetColumnSpan(lblDiaSemana, 2);
        tlpTarjeta.SetColumnSpan(CboDiaSemana, 2);
        tlpTarjeta.SetColumnSpan(lblPasoHoras, 2);
        tlpTarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpTarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpTarjeta.RowCount = 9;
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.ColumnCount = 2;
        tlpTarjeta.AutoSize = true;
        tlpTarjeta.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpTarjeta.Dock = DockStyle.Top;
        tlpTarjeta.TabIndex = 0;
        tlpTarjeta.Name = "tlpTarjeta";
        //
        // lblTituloTarjeta
        //
        lblTituloTarjeta.AutoSize = true;
        lblTituloTarjeta.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblTituloTarjeta.ForeColor = Color.FromArgb(27, 42, 74);
        lblTituloTarjeta.Margin = new Padding(0, 0, 0, 2);
        lblTituloTarjeta.Text = "NUEVO HORARIO";
        lblTituloTarjeta.TabIndex = 0;
        lblTituloTarjeta.Name = "lblTituloTarjeta";
        //
        // LblModo
        //
        LblModo.AutoEllipsis = true;
        LblModo.AutoSize = false;
        LblModo.Dock = DockStyle.Fill;
        LblModo.Font = new Font("Segoe UI", 9F);
        LblModo.ForeColor = Color.FromArgb(176, 96, 0);
        LblModo.Margin = new Padding(0, 0, 0, 2);
        LblModo.Size = new Size(0, 20);
        LblModo.TextAlign = ContentAlignment.MiddleLeft;
        LblModo.Visible = false;
        LblModo.TabIndex = 1;
        LblModo.Name = "LblModo";
        //
        // lblMedico
        //
        lblMedico.AutoSize = true;
        lblMedico.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMedico.ForeColor = Color.FromArgb(27, 42, 74);
        lblMedico.Margin = new Padding(0, 8, 0, 3);
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
        // lblDiaSemana
        //
        lblDiaSemana.AutoSize = true;
        lblDiaSemana.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDiaSemana.ForeColor = Color.FromArgb(27, 42, 74);
        lblDiaSemana.Margin = new Padding(0, 8, 0, 3);
        lblDiaSemana.Text = "2. Día de la semana";
        lblDiaSemana.TabIndex = 4;
        lblDiaSemana.Name = "lblDiaSemana";
        //
        // CboDiaSemana
        //
        CboDiaSemana.Dock = DockStyle.Fill;
        CboDiaSemana.DropDownStyle = ComboBoxStyle.DropDownList;
        CboDiaSemana.Font = new Font("Segoe UI", 10F);
        CboDiaSemana.FormattingEnabled = true;
        CboDiaSemana.Margin = new Padding(0, 0, 0, 4);
        CboDiaSemana.TabIndex = 5;
        CboDiaSemana.Name = "CboDiaSemana";
        //
        // lblPasoHoras
        //
        lblPasoHoras.AutoSize = true;
        lblPasoHoras.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPasoHoras.ForeColor = Color.FromArgb(27, 42, 74);
        lblPasoHoras.Margin = new Padding(0, 8, 0, 3);
        lblPasoHoras.Text = "3. Horario";
        lblPasoHoras.TabIndex = 6;
        lblPasoHoras.Name = "lblPasoHoras";
        //
        // lblHoraInicio
        //
        lblHoraInicio.AutoSize = true;
        lblHoraInicio.Font = new Font("Segoe UI", 9F);
        lblHoraInicio.ForeColor = Color.FromArgb(110, 120, 135);
        lblHoraInicio.Margin = new Padding(0, 2, 8, 2);
        lblHoraInicio.Text = "Desde";
        lblHoraInicio.TabIndex = 7;
        lblHoraInicio.Name = "lblHoraInicio";
        //
        // lblHoraFin
        //
        lblHoraFin.AutoSize = true;
        lblHoraFin.Font = new Font("Segoe UI", 9F);
        lblHoraFin.ForeColor = Color.FromArgb(110, 120, 135);
        lblHoraFin.Margin = new Padding(0, 2, 0, 2);
        lblHoraFin.Text = "Hasta";
        lblHoraFin.TabIndex = 8;
        lblHoraFin.Name = "lblHoraFin";
        //
        // DtpHoraInicio
        //
        DtpHoraInicio.Dock = DockStyle.Fill;
        DtpHoraInicio.Format = DateTimePickerFormat.Custom;
        DtpHoraInicio.CustomFormat = "HH:mm";
        DtpHoraInicio.ShowUpDown = true;
        DtpHoraInicio.Font = new Font("Segoe UI", 10F);
        DtpHoraInicio.Margin = new Padding(0, 0, 8, 4);
        DtpHoraInicio.TabIndex = 9;
        DtpHoraInicio.Name = "DtpHoraInicio";
        //
        // DtpHoraFin
        //
        DtpHoraFin.Dock = DockStyle.Fill;
        DtpHoraFin.Format = DateTimePickerFormat.Custom;
        DtpHoraFin.CustomFormat = "HH:mm";
        DtpHoraFin.ShowUpDown = true;
        DtpHoraFin.Font = new Font("Segoe UI", 10F);
        DtpHoraFin.Margin = new Padding(0, 0, 0, 4);
        DtpHoraFin.TabIndex = 10;
        DtpHoraFin.Name = "DtpHoraFin";
        //
        // pnlBotones
        //
        pnlBotones.Controls.Add(tlpBotonesTarjeta);
        pnlBotones.AutoSize = true;
        pnlBotones.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlBotones.Dock = DockStyle.Bottom;
        pnlBotones.Padding = new Padding(0, 10, 0, 0);
        pnlBotones.TabIndex = 1;
        pnlBotones.Name = "pnlBotones";
        //
        // tlpBotonesTarjeta
        //
        tlpBotonesTarjeta.Controls.Add(BtnGuardar, 0, 0);
        tlpBotonesTarjeta.Controls.Add(BtnNuevo, 1, 0);
        tlpBotonesTarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
        tlpBotonesTarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
        tlpBotonesTarjeta.RowCount = 1;
        tlpBotonesTarjeta.RowStyles.Add(new RowStyle());
        tlpBotonesTarjeta.ColumnCount = 2;
        tlpBotonesTarjeta.AutoSize = true;
        tlpBotonesTarjeta.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpBotonesTarjeta.Dock = DockStyle.Fill;
        tlpBotonesTarjeta.Margin = new Padding(0);
        tlpBotonesTarjeta.TabIndex = 0;
        tlpBotonesTarjeta.Name = "tlpBotonesTarjeta";
        //
        // BtnGuardar
        //
        BtnGuardar.Dock = DockStyle.Fill;
        BtnGuardar.BackColor = Color.FromArgb(46, 134, 222);
        BtnGuardar.FlatAppearance.BorderSize = 0;
        BtnGuardar.FlatStyle = FlatStyle.Flat;
        BtnGuardar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        BtnGuardar.ForeColor = Color.White;
        BtnGuardar.Margin = new Padding(0, 0, 8, 0);
        BtnGuardar.Size = new Size(0, 38);
        BtnGuardar.Text = "Guardar";
        BtnGuardar.UseVisualStyleBackColor = false;
        BtnGuardar.TabIndex = 0;
        BtnGuardar.Name = "BtnGuardar";
        //
        // BtnNuevo
        //
        BtnNuevo.Dock = DockStyle.Fill;
        BtnNuevo.BackColor = Color.White;
        BtnNuevo.FlatAppearance.BorderColor = Color.FromArgb(214, 220, 230);
        BtnNuevo.FlatAppearance.BorderSize = 1;
        BtnNuevo.FlatStyle = FlatStyle.Flat;
        BtnNuevo.Font = new Font("Segoe UI", 9.5F);
        BtnNuevo.ForeColor = Color.FromArgb(27, 42, 74);
        BtnNuevo.Margin = new Padding(0);
        BtnNuevo.Size = new Size(0, 38);
        BtnNuevo.Text = "Nuevo";
        BtnNuevo.UseVisualStyleBackColor = false;
        BtnNuevo.TabIndex = 1;
        BtnNuevo.Name = "BtnNuevo";
        //
        // pnlEstado
        //
        pnlEstado.Controls.Add(LblMensaje);
        pnlEstado.Dock = DockStyle.Bottom;
        pnlEstado.Padding = new Padding(2, 8, 2, 0);
        pnlEstado.Size = new Size(0, 36);
        pnlEstado.Visible = false;
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
        // FormConfigurarAgenda
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(1130, 650);
        Controls.Add(pnlDerecho);
        Controls.Add(pnlIzquierdo);
        Controls.Add(pnlEstado);
        Name = "FormConfigurarAgenda";
        Padding = new Padding(16, 14, 16, 12);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Horarios de los médicos";
        pnlEstado.ResumeLayout(false);
        tlpBotonesTarjeta.ResumeLayout(false);
        tlpBotonesTarjeta.PerformLayout();
        pnlBotones.ResumeLayout(false);
        pnlBotones.PerformLayout();
        tlpTarjeta.ResumeLayout(false);
        tlpTarjeta.PerformLayout();
        pnlCampos.ResumeLayout(false);
        pnlTarjeta.ResumeLayout(false);
        pnlMarcoTarjeta.ResumeLayout(false);
        pnlIzquierdo.ResumeLayout(false);
        flpBotonesAccion.ResumeLayout(false);
        flpBotonesAccion.PerformLayout();
        tlpAcciones.ResumeLayout(false);
        tlpAcciones.PerformLayout();
        pnlAcciones.ResumeLayout(false);
        pnlAcciones.PerformLayout();
        pnlMarcoAcciones.ResumeLayout(false);
        pnlMarcoAcciones.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)DgvAgenda).EndInit();
        TabListado.ResumeLayout(false);
        TabListado.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)DgvSemanal).EndInit();
        TabSemanal.ResumeLayout(false);
        TabSemanal.PerformLayout();
        TabHorarios.ResumeLayout(false);
        TabHorarios.PerformLayout();
        pnlMarcoGrilla.ResumeLayout(false);
        FlpContadores.ResumeLayout(false);
        flpFiltros.ResumeLayout(false);
        flpFiltros.PerformLayout();
        tlpToolbar.ResumeLayout(false);
        tlpToolbar.PerformLayout();
        pnlToolbar.ResumeLayout(false);
        pnlToolbar.PerformLayout();
        pnlMarcoToolbar.ResumeLayout(false);
        pnlMarcoToolbar.PerformLayout();
        tlpDerecho.ResumeLayout(false);
        tlpDerecho.PerformLayout();
        pnlDerecho.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlDerecho;
    private TableLayoutPanel tlpDerecho;
    private Panel pnlMarcoToolbar;
    private Panel pnlToolbar;
    private TableLayoutPanel tlpToolbar;
    private FlowLayoutPanel flpFiltros;
    private Label lblFiltroMedico;
    private ComboBox CboFiltroMedico;
    private FlowLayoutPanel FlpContadores;
    private Panel pnlMarcoGrilla;
    private TabControl TabHorarios;
    private TabPage TabSemanal;
    private DataGridView DgvSemanal;
    private TabPage TabListado;
    private DataGridView DgvAgenda;
    private Panel pnlMarcoAcciones;
    private Panel pnlAcciones;
    private TableLayoutPanel tlpAcciones;
    private Label LblSeleccion;
    private FlowLayoutPanel flpBotonesAccion;
    private Button BtnEliminar;
    private Label LblAyudaAcciones;
    private Label LblDetalle;
    private Panel pnlIzquierdo;
    private Panel pnlMarcoTarjeta;
    private Panel pnlTarjeta;
    private Panel pnlCampos;
    private TableLayoutPanel tlpTarjeta;
    private Label lblTituloTarjeta;
    private Label LblModo;
    private Label lblMedico;
    private ComboBox CboMedico;
    private Label lblDiaSemana;
    private ComboBox CboDiaSemana;
    private Label lblPasoHoras;
    private Label lblHoraInicio;
    private Label lblHoraFin;
    private DateTimePicker DtpHoraInicio;
    private DateTimePicker DtpHoraFin;
    private Panel pnlBotones;
    private TableLayoutPanel tlpBotonesTarjeta;
    private Button BtnGuardar;
    private Button BtnNuevo;
    private Panel pnlEstado;
    private Label LblMensaje;
}
