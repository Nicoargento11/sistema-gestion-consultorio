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
        pnlDerecho = new Panel();
        tlpDerecho = new TableLayoutPanel();
        pnlMarcoToolbar = new Panel();
        pnlToolbar = new Panel();
        tlpToolbar = new TableLayoutPanel();
        LblVista = new Label();
        flpFiltros = new FlowLayoutPanel();
        lblBuscar = new Label();
        TxtBuscar = new TextBox();
        ChkMostrarInactivos = new CheckBox();
        FlpContadores = new FlowLayoutPanel();
        pnlMarcoGrilla = new Panel();
        DgvUsuarios = new DataGridView();
        pnlMarcoAcciones = new Panel();
        pnlAcciones = new Panel();
        tlpAcciones = new TableLayoutPanel();
        LblSeleccion = new Label();
        flpBotonesAccion = new FlowLayoutPanel();
        BtnEliminar = new Button();
        BtnReactivar = new Button();
        LblAyudaAcciones = new Label();
        LblDetalle = new Label();
        pnlIzquierdo = new Panel();
        pnlMarcoTarjeta = new Panel();
        pnlTarjeta = new Panel();
        tlpTarjeta = new TableLayoutPanel();
        lblTituloTarjeta = new Label();
        LblModo = new Label();
        lblNombreUsuario = new Label();
        TxtNombreUsuario = new TextBox();
        lblContrasena = new Label();
        TxtContrasena = new TextBox();
        LblAyudaContrasena = new Label();
        lblRol = new Label();
        CboRol = new ComboBox();
        tlpSlotRol = new TableLayoutPanel();
        lblSlotRol = new Label();
        CboMedico = new ComboBox();
        ClbMedicosAsignados = new CheckedListBox();
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
        ((System.ComponentModel.ISupportInitialize)DgvUsuarios).BeginInit();
        pnlMarcoAcciones.SuspendLayout();
        pnlAcciones.SuspendLayout();
        tlpAcciones.SuspendLayout();
        flpBotonesAccion.SuspendLayout();
        pnlIzquierdo.SuspendLayout();
        pnlMarcoTarjeta.SuspendLayout();
        pnlTarjeta.SuspendLayout();
        tlpTarjeta.SuspendLayout();
        tlpSlotRol.SuspendLayout();
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
        LblVista.Margin = new Padding(0, 0, 0, 4);
        LblVista.TabIndex = 0;
        LblVista.Name = "LblVista";
        //
        // flpFiltros
        //
        flpFiltros.Controls.Add(lblBuscar);
        flpFiltros.Controls.Add(TxtBuscar);
        flpFiltros.Controls.Add(ChkMostrarInactivos);
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
        TxtBuscar.PlaceholderText = "Usuario, rol o médico";
        TxtBuscar.Size = new Size(260, 29);
        TxtBuscar.TabIndex = 1;
        TxtBuscar.Name = "TxtBuscar";
        //
        // ChkMostrarInactivos
        //
        ChkMostrarInactivos.AutoSize = true;
        ChkMostrarInactivos.Font = new Font("Segoe UI", 9.5F);
        ChkMostrarInactivos.Margin = new Padding(18, 5, 0, 0);
        ChkMostrarInactivos.Text = "Mostrar inactivos";
        ChkMostrarInactivos.UseVisualStyleBackColor = true;
        ChkMostrarInactivos.TabIndex = 2;
        ChkMostrarInactivos.Name = "ChkMostrarInactivos";
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
        pnlMarcoGrilla.Controls.Add(DgvUsuarios);
        pnlMarcoGrilla.BackColor = Color.FromArgb(214, 220, 230);
        pnlMarcoGrilla.Dock = DockStyle.Fill;
        pnlMarcoGrilla.Margin = new Padding(0, 0, 0, 8);
        pnlMarcoGrilla.Padding = new Padding(1);
        pnlMarcoGrilla.TabIndex = 2;
        pnlMarcoGrilla.Name = "pnlMarcoGrilla";
        //
        // DgvUsuarios
        //
        DgvUsuarios.AllowUserToAddRows = false;
        DgvUsuarios.AllowUserToDeleteRows = false;
        DgvUsuarios.AllowUserToResizeRows = false;
        DgvUsuarios.BackgroundColor = Color.White;
        DgvUsuarios.BorderStyle = BorderStyle.None;
        DgvUsuarios.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        DgvUsuarios.ColumnHeadersHeight = 40;
        DgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        DgvUsuarios.Dock = DockStyle.Fill;
        DgvUsuarios.Font = new Font("Segoe UI", 9F);
        DgvUsuarios.MultiSelect = false;
        DgvUsuarios.ReadOnly = true;
        DgvUsuarios.RowHeadersVisible = false;
        DgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvUsuarios.TabIndex = 0;
        DgvUsuarios.Name = "DgvUsuarios";
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
        tlpAcciones.Controls.Add(LblDetalle, 0, 2);
        tlpAcciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpAcciones.RowCount = 3;
        tlpAcciones.RowStyles.Add(new RowStyle());
        tlpAcciones.RowStyles.Add(new RowStyle());
        tlpAcciones.RowStyles.Add(new RowStyle());
        tlpAcciones.ColumnCount = 1;
        tlpAcciones.AutoSize = true;
        tlpAcciones.AutoSizeMode = AutoSizeMode.GrowAndShrink;
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
        LblSeleccion.Name = "LblSeleccion";
        //
        // flpBotonesAccion
        //
        flpBotonesAccion.Controls.Add(BtnEliminar);
        flpBotonesAccion.Controls.Add(BtnReactivar);
        flpBotonesAccion.Controls.Add(LblAyudaAcciones);
        flpBotonesAccion.Size = new Size(0, 30);
        flpBotonesAccion.Dock = DockStyle.Fill;
        flpBotonesAccion.Margin = new Padding(0);
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
        BtnEliminar.Size = new Size(116, 32);
        BtnEliminar.Text = "Eliminar";
        BtnEliminar.UseVisualStyleBackColor = false;
        BtnEliminar.TabIndex = 0;
        BtnEliminar.Name = "BtnEliminar";
        //
        // BtnReactivar
        //
        BtnReactivar.BackColor = Color.FromArgb(39, 174, 96);
        BtnReactivar.FlatAppearance.BorderSize = 0;
        BtnReactivar.FlatStyle = FlatStyle.Flat;
        BtnReactivar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnReactivar.ForeColor = Color.White;
        BtnReactivar.Margin = new Padding(0, 0, 14, 0);
        BtnReactivar.Size = new Size(116, 32);
        BtnReactivar.Text = "Reactivar";
        BtnReactivar.UseVisualStyleBackColor = false;
        BtnReactivar.TabIndex = 1;
        BtnReactivar.Name = "BtnReactivar";
        //
        // LblAyudaAcciones
        //
        LblAyudaAcciones.AutoSize = true;
        LblAyudaAcciones.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        LblAyudaAcciones.ForeColor = Color.FromArgb(110, 120, 135);
        LblAyudaAcciones.Margin = new Padding(0, 7, 0, 0);
        LblAyudaAcciones.Visible = false;
        LblAyudaAcciones.TabIndex = 2;
        LblAyudaAcciones.Name = "LblAyudaAcciones";
        //
        // LblDetalle
        //
        LblDetalle.AutoSize = true;
        LblDetalle.Font = new Font("Segoe UI", 9.5F);
        LblDetalle.ForeColor = Color.FromArgb(27, 42, 74);
        LblDetalle.Margin = new Padding(0, 8, 0, 0);
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
        pnlTarjeta.Controls.Add(tlpTarjeta);
        pnlTarjeta.AutoScroll = true;
        pnlTarjeta.BackColor = Color.White;
        pnlTarjeta.Dock = DockStyle.Fill;
        pnlTarjeta.Padding = new Padding(16, 14, 16, 14);
        pnlTarjeta.TabIndex = 0;
        pnlTarjeta.Name = "pnlTarjeta";
        //
        // tlpTarjeta
        //
        tlpTarjeta.Controls.Add(lblTituloTarjeta, 0, 0);
        tlpTarjeta.Controls.Add(LblModo, 0, 1);
        tlpTarjeta.Controls.Add(lblNombreUsuario, 0, 2);
        tlpTarjeta.Controls.Add(TxtNombreUsuario, 0, 3);
        tlpTarjeta.Controls.Add(lblContrasena, 0, 4);
        tlpTarjeta.Controls.Add(TxtContrasena, 0, 5);
        tlpTarjeta.Controls.Add(LblAyudaContrasena, 0, 6);
        tlpTarjeta.Controls.Add(lblRol, 0, 7);
        tlpTarjeta.Controls.Add(CboRol, 0, 8);
        tlpTarjeta.Controls.Add(tlpSlotRol, 0, 9);
        tlpTarjeta.Controls.Add(tlpBotonesTarjeta, 0, 10);
        tlpTarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpTarjeta.RowCount = 11;
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
        tlpTarjeta.ColumnCount = 1;
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
        lblTituloTarjeta.Margin = new Padding(0, 0, 0, 4);
        lblTituloTarjeta.Text = "NUEVO USUARIO";
        lblTituloTarjeta.TabIndex = 0;
        lblTituloTarjeta.Name = "lblTituloTarjeta";
        //
        // LblModo
        //
        LblModo.AutoSize = true;
        LblModo.Font = new Font("Segoe UI", 9F);
        LblModo.ForeColor = Color.FromArgb(176, 96, 0);
        LblModo.Margin = new Padding(0, 0, 0, 4);
        LblModo.Visible = false;
        LblModo.TabIndex = 1;
        LblModo.Name = "LblModo";
        //
        // lblNombreUsuario
        //
        lblNombreUsuario.AutoSize = true;
        lblNombreUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblNombreUsuario.ForeColor = Color.FromArgb(27, 42, 74);
        lblNombreUsuario.Margin = new Padding(0, 10, 0, 3);
        lblNombreUsuario.Text = "1. Usuario";
        lblNombreUsuario.TabIndex = 2;
        lblNombreUsuario.Name = "lblNombreUsuario";
        //
        // TxtNombreUsuario
        //
        TxtNombreUsuario.Dock = DockStyle.Fill;
        TxtNombreUsuario.Font = new Font("Segoe UI", 10F);
        TxtNombreUsuario.Margin = new Padding(0, 0, 0, 4);
        TxtNombreUsuario.MaxLength = 50;
        TxtNombreUsuario.TabIndex = 3;
        TxtNombreUsuario.Name = "TxtNombreUsuario";
        //
        // lblContrasena
        //
        lblContrasena.AutoSize = true;
        lblContrasena.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblContrasena.ForeColor = Color.FromArgb(27, 42, 74);
        lblContrasena.Margin = new Padding(0, 10, 0, 3);
        lblContrasena.Text = "2. Contraseña";
        lblContrasena.TabIndex = 4;
        lblContrasena.Name = "lblContrasena";
        //
        // TxtContrasena
        //
        TxtContrasena.Dock = DockStyle.Fill;
        TxtContrasena.Font = new Font("Segoe UI", 10F);
        TxtContrasena.Margin = new Padding(0, 0, 0, 4);
        TxtContrasena.MaxLength = 200;
        TxtContrasena.PasswordChar = '*';
        TxtContrasena.TabIndex = 5;
        TxtContrasena.Name = "TxtContrasena";
        //
        // LblAyudaContrasena
        //
        LblAyudaContrasena.AutoSize = true;
        LblAyudaContrasena.Font = new Font("Segoe UI", 8.5F);
        LblAyudaContrasena.ForeColor = Color.FromArgb(110, 120, 135);
        LblAyudaContrasena.Margin = new Padding(0, 0, 0, 2);
        LblAyudaContrasena.Text = "Mínimo 6 caracteres.";
        LblAyudaContrasena.TabIndex = 6;
        LblAyudaContrasena.Name = "LblAyudaContrasena";
        //
        // lblRol
        //
        lblRol.AutoSize = true;
        lblRol.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblRol.ForeColor = Color.FromArgb(27, 42, 74);
        lblRol.Margin = new Padding(0, 10, 0, 3);
        lblRol.Text = "3. Rol";
        lblRol.TabIndex = 7;
        lblRol.Name = "lblRol";
        //
        // CboRol
        //
        CboRol.Dock = DockStyle.Fill;
        CboRol.DropDownStyle = ComboBoxStyle.DropDownList;
        CboRol.Font = new Font("Segoe UI", 10F);
        CboRol.FormattingEnabled = true;
        CboRol.Margin = new Padding(0, 0, 0, 4);
        CboRol.TabIndex = 8;
        CboRol.Name = "CboRol";
        //
        // tlpSlotRol
        //
        tlpSlotRol.Controls.Add(lblSlotRol, 0, 0);
        tlpSlotRol.Controls.Add(CboMedico, 0, 1);
        tlpSlotRol.Controls.Add(ClbMedicosAsignados, 0, 2);
        tlpSlotRol.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpSlotRol.RowCount = 3;
        tlpSlotRol.RowStyles.Add(new RowStyle());
        tlpSlotRol.RowStyles.Add(new RowStyle());
        tlpSlotRol.RowStyles.Add(new RowStyle());
        tlpSlotRol.ColumnCount = 1;
        tlpSlotRol.AutoSize = true;
        tlpSlotRol.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpSlotRol.Dock = DockStyle.Fill;
        tlpSlotRol.Margin = new Padding(0);
        tlpSlotRol.TabIndex = 9;
        tlpSlotRol.Name = "tlpSlotRol";
        //
        // lblSlotRol
        //
        lblSlotRol.AutoSize = true;
        lblSlotRol.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblSlotRol.ForeColor = Color.FromArgb(27, 42, 74);
        lblSlotRol.Margin = new Padding(0, 10, 0, 3);
        lblSlotRol.Text = "4. Médico";
        lblSlotRol.TabIndex = 0;
        lblSlotRol.Name = "lblSlotRol";
        //
        // CboMedico
        //
        CboMedico.Dock = DockStyle.Fill;
        CboMedico.DropDownStyle = ComboBoxStyle.DropDownList;
        CboMedico.Font = new Font("Segoe UI", 10F);
        CboMedico.FormattingEnabled = true;
        CboMedico.Margin = new Padding(0, 0, 0, 4);
        CboMedico.TabIndex = 1;
        CboMedico.Name = "CboMedico";
        //
        // ClbMedicosAsignados
        //
        ClbMedicosAsignados.BorderStyle = BorderStyle.FixedSingle;
        ClbMedicosAsignados.CheckOnClick = true;
        ClbMedicosAsignados.Dock = DockStyle.Fill;
        ClbMedicosAsignados.Font = new Font("Segoe UI", 9.5F);
        ClbMedicosAsignados.FormattingEnabled = true;
        ClbMedicosAsignados.IntegralHeight = false;
        ClbMedicosAsignados.Margin = new Padding(0, 0, 0, 4);
        ClbMedicosAsignados.Size = new Size(0, 120);
        ClbMedicosAsignados.TabIndex = 2;
        ClbMedicosAsignados.Name = "ClbMedicosAsignados";
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
        tlpBotonesTarjeta.Margin = new Padding(0, 16, 0, 0);
        tlpBotonesTarjeta.TabIndex = 10;
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
        BtnGuardar.Size = new Size(0, 40);
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
        BtnNuevo.Size = new Size(0, 40);
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
        // FormUsuarios
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(1130, 650);
        Controls.Add(pnlDerecho);
        Controls.Add(pnlIzquierdo);
        Controls.Add(pnlEstado);
        Name = "FormUsuarios";
        Padding = new Padding(16, 14, 16, 12);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Usuarios";
        pnlEstado.ResumeLayout(false);
        tlpBotonesTarjeta.ResumeLayout(false);
        tlpBotonesTarjeta.PerformLayout();
        tlpSlotRol.ResumeLayout(false);
        tlpSlotRol.PerformLayout();
        tlpTarjeta.ResumeLayout(false);
        tlpTarjeta.PerformLayout();
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
        ((System.ComponentModel.ISupportInitialize)DgvUsuarios).EndInit();
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
    private CheckBox ChkMostrarInactivos;
    private FlowLayoutPanel FlpContadores;
    private Panel pnlMarcoGrilla;
    private DataGridView DgvUsuarios;
    private Panel pnlMarcoAcciones;
    private Panel pnlAcciones;
    private TableLayoutPanel tlpAcciones;
    private Label LblSeleccion;
    private FlowLayoutPanel flpBotonesAccion;
    private Button BtnEliminar;
    private Button BtnReactivar;
    private Label LblAyudaAcciones;
    private Label LblDetalle;
    private Panel pnlIzquierdo;
    private Panel pnlMarcoTarjeta;
    private Panel pnlTarjeta;
    private TableLayoutPanel tlpTarjeta;
    private Label lblTituloTarjeta;
    private Label LblModo;
    private Label lblNombreUsuario;
    private TextBox TxtNombreUsuario;
    private Label lblContrasena;
    private TextBox TxtContrasena;
    private Label LblAyudaContrasena;
    private Label lblRol;
    private ComboBox CboRol;
    private TableLayoutPanel tlpSlotRol;
    private Label lblSlotRol;
    private ComboBox CboMedico;
    private CheckedListBox ClbMedicosAsignados;
    private TableLayoutPanel tlpBotonesTarjeta;
    private Button BtnGuardar;
    private Button BtnNuevo;
    private Panel pnlEstado;
    private Label LblMensaje;
}
