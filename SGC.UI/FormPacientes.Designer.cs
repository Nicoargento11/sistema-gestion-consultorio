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
        pnlDerecho = new Panel();
        tlpDerecho = new TableLayoutPanel();
        pnlMarcoToolbar = new Panel();
        pnlToolbar = new Panel();
        tlpToolbar = new TableLayoutPanel();
        LblVista = new Label();
        flpFiltros = new FlowLayoutPanel();
        lblBuscar = new Label();
        TxtBuscar = new TextBox();
        ChkSoloAusencias = new CheckBox();
        FlpContadores = new FlowLayoutPanel();
        pnlMarcoGrilla = new Panel();
        DgvPacientes = new DataGridView();
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
        lblGrupoPersonales = new Label();
        lblNombre = new Label();
        lblApellido = new Label();
        TxtNombre = new TextBox();
        TxtApellido = new TextBox();
        lblDni = new Label();
        lblFechaNacimiento = new Label();
        TxtDni = new TextBox();
        DtpFechaNacimiento = new DateTimePicker();
        lblGrupoContacto = new Label();
        lblEmail = new Label();
        TxtEmail = new TextBox();
        lblTelefono = new Label();
        TxtTelefono = new TextBox();
        lblGrupoCobertura = new Label();
        lblObraSocial = new Label();
        CboObraSocial = new ComboBox();
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
        ((System.ComponentModel.ISupportInitialize)DgvPacientes).BeginInit();
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
        flpFiltros.Controls.Add(ChkSoloAusencias);
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
        TxtBuscar.PlaceholderText = "Nombre, apellido o DNI";
        TxtBuscar.Size = new Size(260, 29);
        TxtBuscar.TabIndex = 1;
        TxtBuscar.Name = "TxtBuscar";
        //
        // ChkSoloAusencias
        //
        ChkSoloAusencias.AutoSize = true;
        ChkSoloAusencias.Font = new Font("Segoe UI", 9.5F);
        ChkSoloAusencias.Margin = new Padding(18, 5, 0, 0);
        ChkSoloAusencias.Text = "Solo con ausencias";
        ChkSoloAusencias.UseVisualStyleBackColor = true;
        ChkSoloAusencias.TabIndex = 2;
        ChkSoloAusencias.Name = "ChkSoloAusencias";
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
        pnlMarcoGrilla.Controls.Add(DgvPacientes);
        pnlMarcoGrilla.BackColor = Color.FromArgb(214, 220, 230);
        pnlMarcoGrilla.Dock = DockStyle.Fill;
        pnlMarcoGrilla.Margin = new Padding(0, 0, 0, 6);
        pnlMarcoGrilla.Padding = new Padding(1);
        pnlMarcoGrilla.TabIndex = 2;
        pnlMarcoGrilla.Name = "pnlMarcoGrilla";
        //
        // DgvPacientes
        //
        DgvPacientes.AllowUserToAddRows = false;
        DgvPacientes.AllowUserToDeleteRows = false;
        DgvPacientes.AllowUserToResizeRows = false;
        DgvPacientes.BackgroundColor = Color.White;
        DgvPacientes.BorderStyle = BorderStyle.None;
        DgvPacientes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        DgvPacientes.ColumnHeadersHeight = 40;
        DgvPacientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        DgvPacientes.Dock = DockStyle.Fill;
        DgvPacientes.Font = new Font("Segoe UI", 9F);
        DgvPacientes.MultiSelect = false;
        DgvPacientes.ReadOnly = true;
        DgvPacientes.RowHeadersVisible = false;
        DgvPacientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvPacientes.TabIndex = 0;
        DgvPacientes.Name = "DgvPacientes";
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
        tlpTarjeta.Controls.Add(lblGrupoPersonales, 0, 2);
        tlpTarjeta.Controls.Add(lblNombre, 0, 3);
        tlpTarjeta.Controls.Add(lblApellido, 1, 3);
        tlpTarjeta.Controls.Add(TxtNombre, 0, 4);
        tlpTarjeta.Controls.Add(TxtApellido, 1, 4);
        tlpTarjeta.Controls.Add(lblDni, 0, 5);
        tlpTarjeta.Controls.Add(lblFechaNacimiento, 1, 5);
        tlpTarjeta.Controls.Add(TxtDni, 0, 6);
        tlpTarjeta.Controls.Add(DtpFechaNacimiento, 1, 6);
        tlpTarjeta.Controls.Add(lblGrupoContacto, 0, 7);
        tlpTarjeta.Controls.Add(lblEmail, 0, 8);
        tlpTarjeta.Controls.Add(TxtEmail, 0, 9);
        tlpTarjeta.Controls.Add(lblTelefono, 0, 10);
        tlpTarjeta.Controls.Add(TxtTelefono, 0, 11);
        tlpTarjeta.Controls.Add(lblGrupoCobertura, 0, 12);
        tlpTarjeta.Controls.Add(lblObraSocial, 0, 13);
        tlpTarjeta.Controls.Add(CboObraSocial, 0, 14);
        tlpTarjeta.SetColumnSpan(lblTituloTarjeta, 2);
        tlpTarjeta.SetColumnSpan(LblModo, 2);
        tlpTarjeta.SetColumnSpan(lblGrupoPersonales, 2);
        tlpTarjeta.SetColumnSpan(lblGrupoContacto, 2);
        tlpTarjeta.SetColumnSpan(lblEmail, 2);
        tlpTarjeta.SetColumnSpan(TxtEmail, 2);
        tlpTarjeta.SetColumnSpan(lblTelefono, 2);
        tlpTarjeta.SetColumnSpan(TxtTelefono, 2);
        tlpTarjeta.SetColumnSpan(lblGrupoCobertura, 2);
        tlpTarjeta.SetColumnSpan(lblObraSocial, 2);
        tlpTarjeta.SetColumnSpan(CboObraSocial, 2);
        tlpTarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpTarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpTarjeta.RowCount = 15;
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
        lblTituloTarjeta.Text = "NUEVO PACIENTE";
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
        // lblGrupoPersonales
        //
        lblGrupoPersonales.AutoSize = true;
        lblGrupoPersonales.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblGrupoPersonales.ForeColor = Color.FromArgb(27, 42, 74);
        lblGrupoPersonales.Margin = new Padding(0, 8, 0, 3);
        lblGrupoPersonales.Text = "1. Datos personales";
        lblGrupoPersonales.TabIndex = 2;
        lblGrupoPersonales.Name = "lblGrupoPersonales";
        //
        // lblNombre
        //
        lblNombre.AutoSize = true;
        lblNombre.Font = new Font("Segoe UI", 9F);
        lblNombre.ForeColor = Color.FromArgb(110, 120, 135);
        lblNombre.Margin = new Padding(0, 2, 8, 2);
        lblNombre.Text = "Nombre";
        lblNombre.TabIndex = 3;
        lblNombre.Name = "lblNombre";
        //
        // lblApellido
        //
        lblApellido.AutoSize = true;
        lblApellido.Font = new Font("Segoe UI", 9F);
        lblApellido.ForeColor = Color.FromArgb(110, 120, 135);
        lblApellido.Margin = new Padding(0, 2, 0, 2);
        lblApellido.Text = "Apellido";
        lblApellido.TabIndex = 4;
        lblApellido.Name = "lblApellido";
        //
        // TxtNombre
        //
        TxtNombre.Dock = DockStyle.Fill;
        TxtNombre.Font = new Font("Segoe UI", 10F);
        TxtNombre.Margin = new Padding(0, 0, 8, 4);
        TxtNombre.MaxLength = 80;
        TxtNombre.TabIndex = 5;
        TxtNombre.Name = "TxtNombre";
        //
        // TxtApellido
        //
        TxtApellido.Dock = DockStyle.Fill;
        TxtApellido.Font = new Font("Segoe UI", 10F);
        TxtApellido.Margin = new Padding(0, 0, 0, 4);
        TxtApellido.MaxLength = 80;
        TxtApellido.TabIndex = 6;
        TxtApellido.Name = "TxtApellido";
        //
        // lblDni
        //
        lblDni.AutoSize = true;
        lblDni.Font = new Font("Segoe UI", 9F);
        lblDni.ForeColor = Color.FromArgb(110, 120, 135);
        lblDni.Margin = new Padding(0, 4, 8, 2);
        lblDni.Text = "DNI";
        lblDni.TabIndex = 7;
        lblDni.Name = "lblDni";
        //
        // lblFechaNacimiento
        //
        lblFechaNacimiento.AutoSize = true;
        lblFechaNacimiento.Font = new Font("Segoe UI", 9F);
        lblFechaNacimiento.ForeColor = Color.FromArgb(110, 120, 135);
        lblFechaNacimiento.Margin = new Padding(0, 4, 0, 2);
        lblFechaNacimiento.Text = "Nacimiento";
        lblFechaNacimiento.TabIndex = 8;
        lblFechaNacimiento.Name = "lblFechaNacimiento";
        //
        // TxtDni
        //
        TxtDni.Dock = DockStyle.Fill;
        TxtDni.Font = new Font("Segoe UI", 10F);
        TxtDni.Margin = new Padding(0, 0, 8, 4);
        TxtDni.MaxLength = 8;
        TxtDni.TabIndex = 9;
        TxtDni.Name = "TxtDni";
        //
        // DtpFechaNacimiento
        //
        DtpFechaNacimiento.Dock = DockStyle.Fill;
        DtpFechaNacimiento.Format = DateTimePickerFormat.Custom;
        DtpFechaNacimiento.CustomFormat = "dd/MM/yyyy";
        DtpFechaNacimiento.Font = new Font("Segoe UI", 10F);
        DtpFechaNacimiento.Margin = new Padding(0, 0, 0, 4);
        DtpFechaNacimiento.TabIndex = 10;
        DtpFechaNacimiento.Name = "DtpFechaNacimiento";
        //
        // lblGrupoContacto
        //
        lblGrupoContacto.AutoSize = true;
        lblGrupoContacto.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblGrupoContacto.ForeColor = Color.FromArgb(27, 42, 74);
        lblGrupoContacto.Margin = new Padding(0, 8, 0, 3);
        lblGrupoContacto.Text = "2. Contacto";
        lblGrupoContacto.TabIndex = 11;
        lblGrupoContacto.Name = "lblGrupoContacto";
        //
        // lblEmail
        //
        lblEmail.AutoSize = true;
        lblEmail.Font = new Font("Segoe UI", 9F);
        lblEmail.ForeColor = Color.FromArgb(110, 120, 135);
        lblEmail.Margin = new Padding(0, 2, 0, 2);
        lblEmail.Text = "Email";
        lblEmail.TabIndex = 12;
        lblEmail.Name = "lblEmail";
        //
        // TxtEmail
        //
        TxtEmail.Dock = DockStyle.Fill;
        TxtEmail.Font = new Font("Segoe UI", 10F);
        TxtEmail.Margin = new Padding(0, 0, 0, 4);
        TxtEmail.TabIndex = 13;
        TxtEmail.Name = "TxtEmail";
        //
        // lblTelefono
        //
        lblTelefono.AutoSize = true;
        lblTelefono.Font = new Font("Segoe UI", 9F);
        lblTelefono.ForeColor = Color.FromArgb(110, 120, 135);
        lblTelefono.Margin = new Padding(0, 4, 0, 2);
        lblTelefono.Text = "Teléfono";
        lblTelefono.TabIndex = 14;
        lblTelefono.Name = "lblTelefono";
        //
        // TxtTelefono
        //
        TxtTelefono.Dock = DockStyle.Fill;
        TxtTelefono.Font = new Font("Segoe UI", 10F);
        TxtTelefono.Margin = new Padding(0, 0, 0, 4);
        TxtTelefono.MaxLength = 15;
        TxtTelefono.TabIndex = 15;
        TxtTelefono.Name = "TxtTelefono";
        //
        // lblGrupoCobertura
        //
        lblGrupoCobertura.AutoSize = true;
        lblGrupoCobertura.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblGrupoCobertura.ForeColor = Color.FromArgb(27, 42, 74);
        lblGrupoCobertura.Margin = new Padding(0, 8, 0, 3);
        lblGrupoCobertura.Text = "3. Cobertura";
        lblGrupoCobertura.TabIndex = 16;
        lblGrupoCobertura.Name = "lblGrupoCobertura";
        //
        // lblObraSocial
        //
        lblObraSocial.AutoSize = true;
        lblObraSocial.Font = new Font("Segoe UI", 9F);
        lblObraSocial.ForeColor = Color.FromArgb(110, 120, 135);
        lblObraSocial.Margin = new Padding(0, 2, 0, 2);
        lblObraSocial.Text = "Obra social (vacío = Particular)";
        lblObraSocial.TabIndex = 17;
        lblObraSocial.Name = "lblObraSocial";
        //
        // CboObraSocial
        //
        CboObraSocial.Dock = DockStyle.Fill;
        CboObraSocial.DropDownStyle = ComboBoxStyle.DropDownList;
        CboObraSocial.Font = new Font("Segoe UI", 10F);
        CboObraSocial.FormattingEnabled = true;
        CboObraSocial.Margin = new Padding(0, 0, 0, 4);
        CboObraSocial.TabIndex = 18;
        CboObraSocial.Name = "CboObraSocial";
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
        // FormPacientes
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(1130, 650);
        Controls.Add(pnlDerecho);
        Controls.Add(pnlIzquierdo);
        Controls.Add(pnlEstado);
        Name = "FormPacientes";
        Padding = new Padding(16, 14, 16, 12);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Pacientes";
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
        ((System.ComponentModel.ISupportInitialize)DgvPacientes).EndInit();
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
    private CheckBox ChkSoloAusencias;
    private FlowLayoutPanel FlpContadores;
    private Panel pnlMarcoGrilla;
    private DataGridView DgvPacientes;
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
    private Label lblGrupoPersonales;
    private Label lblNombre;
    private Label lblApellido;
    private TextBox TxtNombre;
    private TextBox TxtApellido;
    private Label lblDni;
    private Label lblFechaNacimiento;
    private TextBox TxtDni;
    private DateTimePicker DtpFechaNacimiento;
    private Label lblGrupoContacto;
    private Label lblEmail;
    private TextBox TxtEmail;
    private Label lblTelefono;
    private TextBox TxtTelefono;
    private Label lblGrupoCobertura;
    private Label lblObraSocial;
    private ComboBox CboObraSocial;
    private Panel pnlBotones;
    private TableLayoutPanel tlpBotonesTarjeta;
    private Button BtnGuardar;
    private Button BtnNuevo;
    private Panel pnlEstado;
    private Label LblMensaje;
}
