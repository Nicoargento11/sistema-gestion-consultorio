namespace SGC.UI;

partial class FormExcepcionesAgenda
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
        LblVista = new Label();
        flpFiltros = new FlowLayoutPanel();
        lblBuscar = new Label();
        TxtBuscar = new TextBox();
        LblResumenExcepciones = new Label();
        FlpContadores = new FlowLayoutPanel();
        pnlMarcoGrilla = new Panel();
        DgvExcepciones = new DataGridView();
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
        lblFecha = new Label();
        DtpFecha = new DateTimePicker();
        lblTipo = new Label();
        CboTipo = new ComboBox();
        lblPasoHoras = new Label();
        lblHoraInicio = new Label();
        lblHoraFin = new Label();
        DtpHoraInicio = new DateTimePicker();
        DtpHoraFin = new DateTimePicker();
        lblMotivo = new Label();
        TxtMotivo = new TextBox();
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
        ((System.ComponentModel.ISupportInitialize)DgvExcepciones).BeginInit();
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
        tlpDerecho.Controls.Add(LblResumenExcepciones, 0, 1);
        tlpDerecho.Controls.Add(FlpContadores, 0, 2);
        tlpDerecho.Controls.Add(pnlMarcoGrilla, 0, 3);
        tlpDerecho.Controls.Add(pnlMarcoAcciones, 0, 4);
        tlpDerecho.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpDerecho.RowCount = 5;
        tlpDerecho.RowStyles.Add(new RowStyle());
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
        tlpToolbar.Controls.Add(LblVista, 0, 0);
        tlpToolbar.Controls.Add(flpFiltros, 0, 1);
        tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpToolbar.RowCount = 2;
        tlpToolbar.RowStyles.Add(new RowStyle());
        tlpToolbar.RowStyles.Add(new RowStyle());
        tlpToolbar.ColumnCount = 1;
        tlpToolbar.AutoSize = true;
        tlpToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpToolbar.Dock = DockStyle.Fill;
        tlpToolbar.TabIndex = 0;
        tlpToolbar.Name = "tlpToolbar";
        //
        // LblVista
        //
        LblVista.AutoSize = true;
        LblVista.Font = new Font("Segoe UI", 9F);
        LblVista.ForeColor = Color.FromArgb(110, 120, 135);
        LblVista.Margin = new Padding(0, 0, 0, 3);
        LblVista.Visible = false;
        LblVista.TabIndex = 0;
        LblVista.Name = "LblVista";
        //
        // flpFiltros
        //
        flpFiltros.Controls.Add(lblBuscar);
        flpFiltros.Controls.Add(TxtBuscar);
        flpFiltros.Size = new Size(0, 30);
        flpFiltros.Dock = DockStyle.Fill;
        flpFiltros.Margin = new Padding(0);
        flpFiltros.TabIndex = 1;
        flpFiltros.Name = "flpFiltros";
        //
        // lblBuscar
        //
        lblBuscar.AutoSize = true;
        lblBuscar.Font = new Font("Segoe UI", 9.5F);
        lblBuscar.Margin = new Padding(0, 7, 6, 0);
        lblBuscar.Text = "Buscar:";
        lblBuscar.TabIndex = 0;
        lblBuscar.Name = "lblBuscar";
        //
        // TxtBuscar
        //
        TxtBuscar.Font = new Font("Segoe UI", 9.5F);
        TxtBuscar.Margin = new Padding(0, 3, 0, 3);
        TxtBuscar.PlaceholderText = "Médico o motivo";
        TxtBuscar.Size = new Size(260, 29);
        TxtBuscar.TabIndex = 1;
        TxtBuscar.Name = "TxtBuscar";
        //
        // LblResumenExcepciones
        //
        LblResumenExcepciones.AutoEllipsis = true;
        LblResumenExcepciones.AutoSize = false;
        LblResumenExcepciones.Dock = DockStyle.Fill;
        LblResumenExcepciones.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        LblResumenExcepciones.ForeColor = Color.FromArgb(110, 120, 135);
        LblResumenExcepciones.Margin = new Padding(0, 0, 0, 4);
        LblResumenExcepciones.Size = new Size(0, 22);
        LblResumenExcepciones.TextAlign = ContentAlignment.MiddleLeft;
        LblResumenExcepciones.Visible = false;
        LblResumenExcepciones.TabIndex = 1;
        LblResumenExcepciones.Name = "LblResumenExcepciones";
        //
        // FlpContadores
        //
        FlpContadores.Size = new Size(0, 30);
        FlpContadores.Dock = DockStyle.Fill;
        FlpContadores.Margin = new Padding(0, 0, 0, 2);
        FlpContadores.TabIndex = 2;
        FlpContadores.Name = "FlpContadores";
        //
        // pnlMarcoGrilla
        //
        pnlMarcoGrilla.Controls.Add(DgvExcepciones);
        pnlMarcoGrilla.BackColor = Color.FromArgb(214, 220, 230);
        pnlMarcoGrilla.Dock = DockStyle.Fill;
        pnlMarcoGrilla.Margin = new Padding(0, 0, 0, 6);
        pnlMarcoGrilla.Padding = new Padding(1);
        pnlMarcoGrilla.TabIndex = 3;
        pnlMarcoGrilla.Name = "pnlMarcoGrilla";
        //
        // DgvExcepciones
        //
        DgvExcepciones.AllowUserToAddRows = false;
        DgvExcepciones.AllowUserToDeleteRows = false;
        DgvExcepciones.AllowUserToResizeRows = false;
        DgvExcepciones.BackgroundColor = Color.White;
        DgvExcepciones.BorderStyle = BorderStyle.None;
        DgvExcepciones.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        DgvExcepciones.ColumnHeadersHeight = 40;
        DgvExcepciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        DgvExcepciones.Dock = DockStyle.Fill;
        DgvExcepciones.Font = new Font("Segoe UI", 9F);
        DgvExcepciones.MultiSelect = false;
        DgvExcepciones.ReadOnly = true;
        DgvExcepciones.RowHeadersVisible = false;
        DgvExcepciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvExcepciones.TabIndex = 0;
        DgvExcepciones.Name = "DgvExcepciones";
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
        pnlMarcoAcciones.TabIndex = 4;
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
        pnlIzquierdo.Size = new Size(312, 0);
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
        tlpTarjeta.Controls.Add(lblFecha, 0, 4);
        tlpTarjeta.Controls.Add(DtpFecha, 0, 5);
        tlpTarjeta.Controls.Add(lblTipo, 0, 6);
        tlpTarjeta.Controls.Add(CboTipo, 0, 7);
        tlpTarjeta.Controls.Add(lblPasoHoras, 0, 8);
        tlpTarjeta.Controls.Add(lblHoraInicio, 0, 9);
        tlpTarjeta.Controls.Add(lblHoraFin, 1, 9);
        tlpTarjeta.Controls.Add(DtpHoraInicio, 0, 10);
        tlpTarjeta.Controls.Add(DtpHoraFin, 1, 10);
        tlpTarjeta.Controls.Add(lblMotivo, 0, 11);
        tlpTarjeta.Controls.Add(TxtMotivo, 0, 12);
        tlpTarjeta.SetColumnSpan(lblTituloTarjeta, 2);
        tlpTarjeta.SetColumnSpan(LblModo, 2);
        tlpTarjeta.SetColumnSpan(lblMedico, 2);
        tlpTarjeta.SetColumnSpan(CboMedico, 2);
        tlpTarjeta.SetColumnSpan(lblFecha, 2);
        tlpTarjeta.SetColumnSpan(DtpFecha, 2);
        tlpTarjeta.SetColumnSpan(lblTipo, 2);
        tlpTarjeta.SetColumnSpan(CboTipo, 2);
        tlpTarjeta.SetColumnSpan(lblPasoHoras, 2);
        tlpTarjeta.SetColumnSpan(lblMotivo, 2);
        tlpTarjeta.SetColumnSpan(TxtMotivo, 2);
        tlpTarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpTarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpTarjeta.RowCount = 13;
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
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
        lblTituloTarjeta.Text = "NUEVA AUSENCIA";
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
        // lblFecha
        //
        lblFecha.AutoSize = true;
        lblFecha.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblFecha.ForeColor = Color.FromArgb(27, 42, 74);
        lblFecha.Margin = new Padding(0, 8, 0, 3);
        lblFecha.Text = "2. Fecha";
        lblFecha.TabIndex = 4;
        lblFecha.Name = "lblFecha";
        //
        // DtpFecha
        //
        DtpFecha.Dock = DockStyle.Fill;
        DtpFecha.Format = DateTimePickerFormat.Custom;
        DtpFecha.CustomFormat = "dd/MM/yyyy";
        DtpFecha.Font = new Font("Segoe UI", 10F);
        DtpFecha.Margin = new Padding(0, 0, 0, 4);
        DtpFecha.TabIndex = 5;
        DtpFecha.Name = "DtpFecha";
        //
        // lblTipo
        //
        lblTipo.AutoSize = true;
        lblTipo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTipo.ForeColor = Color.FromArgb(27, 42, 74);
        lblTipo.Margin = new Padding(0, 8, 0, 3);
        lblTipo.Text = "3. Tipo de ausencia";
        lblTipo.TabIndex = 6;
        lblTipo.Name = "lblTipo";
        //
        // CboTipo
        //
        CboTipo.Dock = DockStyle.Fill;
        CboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
        CboTipo.Font = new Font("Segoe UI", 10F);
        CboTipo.FormattingEnabled = true;
        CboTipo.Margin = new Padding(0, 0, 0, 4);
        CboTipo.TabIndex = 7;
        CboTipo.Name = "CboTipo";
        //
        // lblPasoHoras
        //
        lblPasoHoras.AutoSize = true;
        lblPasoHoras.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPasoHoras.ForeColor = Color.FromArgb(27, 42, 74);
        lblPasoHoras.Margin = new Padding(0, 8, 0, 3);
        lblPasoHoras.Text = "4. Horario";
        lblPasoHoras.TabIndex = 8;
        lblPasoHoras.Name = "lblPasoHoras";
        //
        // lblHoraInicio
        //
        lblHoraInicio.AutoSize = true;
        lblHoraInicio.Font = new Font("Segoe UI", 9F);
        lblHoraInicio.ForeColor = Color.FromArgb(110, 120, 135);
        lblHoraInicio.Margin = new Padding(0, 2, 8, 2);
        lblHoraInicio.Text = "Desde";
        lblHoraInicio.TabIndex = 9;
        lblHoraInicio.Name = "lblHoraInicio";
        //
        // lblHoraFin
        //
        lblHoraFin.AutoSize = true;
        lblHoraFin.Font = new Font("Segoe UI", 9F);
        lblHoraFin.ForeColor = Color.FromArgb(110, 120, 135);
        lblHoraFin.Margin = new Padding(0, 2, 0, 2);
        lblHoraFin.Text = "Hasta";
        lblHoraFin.TabIndex = 10;
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
        DtpHoraInicio.TabIndex = 11;
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
        DtpHoraFin.TabIndex = 12;
        DtpHoraFin.Name = "DtpHoraFin";
        //
        // lblMotivo
        //
        lblMotivo.AutoSize = true;
        lblMotivo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMotivo.ForeColor = Color.FromArgb(27, 42, 74);
        lblMotivo.Margin = new Padding(0, 8, 0, 3);
        lblMotivo.Text = "5. Motivo";
        lblMotivo.TabIndex = 13;
        lblMotivo.Name = "lblMotivo";
        //
        // TxtMotivo
        //
        TxtMotivo.Dock = DockStyle.Fill;
        TxtMotivo.Font = new Font("Segoe UI", 10F);
        TxtMotivo.Margin = new Padding(0, 0, 0, 4);
        TxtMotivo.PlaceholderText = "Ej: Vacaciones, congreso...";
        TxtMotivo.TabIndex = 14;
        TxtMotivo.Name = "TxtMotivo";
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
        // FormExcepcionesAgenda
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(1130, 650);
        Controls.Add(pnlDerecho);
        Controls.Add(pnlIzquierdo);
        Controls.Add(pnlEstado);
        Name = "FormExcepcionesAgenda";
        Padding = new Padding(16, 14, 16, 12);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Vacaciones y ausencias";
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
        ((System.ComponentModel.ISupportInitialize)DgvExcepciones).EndInit();
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
    private Label LblVista;
    private FlowLayoutPanel flpFiltros;
    private Label lblBuscar;
    private TextBox TxtBuscar;
    private Label LblResumenExcepciones;
    private FlowLayoutPanel FlpContadores;
    private Panel pnlMarcoGrilla;
    private DataGridView DgvExcepciones;
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
    private Label lblFecha;
    private DateTimePicker DtpFecha;
    private Label lblTipo;
    private ComboBox CboTipo;
    private Label lblPasoHoras;
    private Label lblHoraInicio;
    private Label lblHoraFin;
    private DateTimePicker DtpHoraInicio;
    private DateTimePicker DtpHoraFin;
    private Label lblMotivo;
    private TextBox TxtMotivo;
    private Panel pnlBotones;
    private TableLayoutPanel tlpBotonesTarjeta;
    private Button BtnGuardar;
    private Button BtnNuevo;
    private Panel pnlEstado;
    private Label LblMensaje;
}
