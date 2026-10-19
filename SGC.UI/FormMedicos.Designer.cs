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
        pnlDerecho = new Panel();
        tlpDerecho = new TableLayoutPanel();
        pnlMarcoToolbar = new Panel();
        pnlToolbar = new Panel();
        tlpToolbar = new TableLayoutPanel();
        flpFiltros = new FlowLayoutPanel();
        lblBuscar = new Label();
        TxtBuscar = new TextBox();
        FlpContadores = new FlowLayoutPanel();
        pnlMarcoGrilla = new Panel();
        DgvMedicos = new DataGridView();
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
        lblMatricula = new Label();
        TxtDni = new TextBox();
        TxtMatricula = new TextBox();
        lblPasoEspecialidad = new Label();
        CboEspecialidad = new ComboBox();
        lblPasoPrecio = new Label();
        NudPrecioConsultaParticular = new NumericUpDown();
        lblPasoObras = new Label();
        ClbObrasSociales = new CheckedListBox();
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
        ((System.ComponentModel.ISupportInitialize)DgvMedicos).BeginInit();
        pnlMarcoAcciones.SuspendLayout();
        pnlAcciones.SuspendLayout();
        tlpAcciones.SuspendLayout();
        flpBotonesAccion.SuspendLayout();
        pnlIzquierdo.SuspendLayout();
        pnlMarcoTarjeta.SuspendLayout();
        pnlTarjeta.SuspendLayout();
        pnlCampos.SuspendLayout();
        tlpTarjeta.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)NudPrecioConsultaParticular).BeginInit();
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
        flpFiltros.Controls.Add(lblBuscar);
        flpFiltros.Controls.Add(TxtBuscar);
        flpFiltros.Size = new Size(0, 30);
        flpFiltros.Dock = DockStyle.Fill;
        flpFiltros.Margin = new Padding(0);
        flpFiltros.TabIndex = 0;
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
        TxtBuscar.PlaceholderText = "Nombre, apellido, DNI, matrícula o especialidad";
        TxtBuscar.Size = new Size(340, 29);
        TxtBuscar.TabIndex = 1;
        TxtBuscar.Name = "TxtBuscar";
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
        pnlMarcoGrilla.Controls.Add(DgvMedicos);
        pnlMarcoGrilla.BackColor = Color.FromArgb(214, 220, 230);
        pnlMarcoGrilla.Dock = DockStyle.Fill;
        pnlMarcoGrilla.Margin = new Padding(0, 0, 0, 6);
        pnlMarcoGrilla.Padding = new Padding(1);
        pnlMarcoGrilla.TabIndex = 2;
        pnlMarcoGrilla.Name = "pnlMarcoGrilla";
        //
        // DgvMedicos
        //
        DgvMedicos.AllowUserToAddRows = false;
        DgvMedicos.AllowUserToDeleteRows = false;
        DgvMedicos.AllowUserToResizeRows = false;
        DgvMedicos.BackgroundColor = Color.White;
        DgvMedicos.BorderStyle = BorderStyle.None;
        DgvMedicos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        DgvMedicos.ColumnHeadersHeight = 40;
        DgvMedicos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        DgvMedicos.Dock = DockStyle.Fill;
        DgvMedicos.Font = new Font("Segoe UI", 9F);
        DgvMedicos.MultiSelect = false;
        DgvMedicos.ReadOnly = true;
        DgvMedicos.RowHeadersVisible = false;
        DgvMedicos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvMedicos.TabIndex = 0;
        DgvMedicos.Name = "DgvMedicos";
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
        pnlIzquierdo.Size = new Size(292, 0);
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
        tlpTarjeta.Controls.Add(lblMatricula, 1, 5);
        tlpTarjeta.Controls.Add(TxtDni, 0, 6);
        tlpTarjeta.Controls.Add(TxtMatricula, 1, 6);
        tlpTarjeta.Controls.Add(lblPasoEspecialidad, 0, 7);
        tlpTarjeta.Controls.Add(CboEspecialidad, 0, 8);
        tlpTarjeta.Controls.Add(lblPasoPrecio, 0, 9);
        tlpTarjeta.Controls.Add(NudPrecioConsultaParticular, 0, 10);
        tlpTarjeta.Controls.Add(lblPasoObras, 0, 11);
        tlpTarjeta.Controls.Add(ClbObrasSociales, 0, 12);
        tlpTarjeta.SetColumnSpan(lblTituloTarjeta, 2);
        tlpTarjeta.SetColumnSpan(LblModo, 2);
        tlpTarjeta.SetColumnSpan(lblGrupoPersonales, 2);
        tlpTarjeta.SetColumnSpan(lblPasoEspecialidad, 2);
        tlpTarjeta.SetColumnSpan(CboEspecialidad, 2);
        tlpTarjeta.SetColumnSpan(lblPasoPrecio, 2);
        tlpTarjeta.SetColumnSpan(NudPrecioConsultaParticular, 2);
        tlpTarjeta.SetColumnSpan(lblPasoObras, 2);
        tlpTarjeta.SetColumnSpan(ClbObrasSociales, 2);
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
        lblTituloTarjeta.Text = "NUEVO MÉDICO";
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
        // lblMatricula
        //
        lblMatricula.AutoSize = true;
        lblMatricula.Font = new Font("Segoe UI", 9F);
        lblMatricula.ForeColor = Color.FromArgb(110, 120, 135);
        lblMatricula.Margin = new Padding(0, 4, 0, 2);
        lblMatricula.Text = "Matrícula";
        lblMatricula.TabIndex = 8;
        lblMatricula.Name = "lblMatricula";
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
        // TxtMatricula
        //
        TxtMatricula.Dock = DockStyle.Fill;
        TxtMatricula.Font = new Font("Segoe UI", 10F);
        TxtMatricula.Margin = new Padding(0, 0, 0, 4);
        TxtMatricula.PlaceholderText = "Ej: MP1234";
        TxtMatricula.TabIndex = 10;
        TxtMatricula.Name = "TxtMatricula";
        //
        // lblPasoEspecialidad
        //
        lblPasoEspecialidad.AutoSize = true;
        lblPasoEspecialidad.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPasoEspecialidad.ForeColor = Color.FromArgb(27, 42, 74);
        lblPasoEspecialidad.Margin = new Padding(0, 8, 0, 3);
        lblPasoEspecialidad.Text = "2. Especialidad";
        lblPasoEspecialidad.TabIndex = 11;
        lblPasoEspecialidad.Name = "lblPasoEspecialidad";
        //
        // CboEspecialidad
        //
        CboEspecialidad.Dock = DockStyle.Fill;
        CboEspecialidad.Font = new Font("Segoe UI", 10F);
        CboEspecialidad.FormattingEnabled = true;
        CboEspecialidad.Items.AddRange(new object[] { "Clinica General", "Cardiologia", "Pediatria", "Dermatologia", "Traumatologia", "Ginecologia", "Neurologia", "Oftalmologia" });
        CboEspecialidad.MaxLength = 80;
        CboEspecialidad.Margin = new Padding(0, 0, 0, 4);
        CboEspecialidad.TabIndex = 12;
        CboEspecialidad.Name = "CboEspecialidad";
        //
        // lblPasoPrecio
        //
        lblPasoPrecio.AutoSize = true;
        lblPasoPrecio.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPasoPrecio.ForeColor = Color.FromArgb(27, 42, 74);
        lblPasoPrecio.Margin = new Padding(0, 8, 0, 3);
        lblPasoPrecio.Text = "3. Precio de consulta particular";
        lblPasoPrecio.TabIndex = 13;
        lblPasoPrecio.Name = "lblPasoPrecio";
        //
        // NudPrecioConsultaParticular
        //
        NudPrecioConsultaParticular.DecimalPlaces = 2;
        NudPrecioConsultaParticular.Dock = DockStyle.Fill;
        NudPrecioConsultaParticular.Font = new Font("Segoe UI", 10F);
        NudPrecioConsultaParticular.Margin = new Padding(0, 0, 0, 4);
        NudPrecioConsultaParticular.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        NudPrecioConsultaParticular.TabIndex = 14;
        NudPrecioConsultaParticular.Name = "NudPrecioConsultaParticular";
        //
        // lblPasoObras
        //
        lblPasoObras.AutoSize = true;
        lblPasoObras.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPasoObras.ForeColor = Color.FromArgb(27, 42, 74);
        lblPasoObras.Margin = new Padding(0, 8, 0, 3);
        lblPasoObras.Text = "4. Obras sociales que acepta";
        lblPasoObras.TabIndex = 15;
        lblPasoObras.Name = "lblPasoObras";
        //
        // ClbObrasSociales
        //
        ClbObrasSociales.BorderStyle = BorderStyle.FixedSingle;
        ClbObrasSociales.CheckOnClick = true;
        ClbObrasSociales.Dock = DockStyle.Fill;
        ClbObrasSociales.Font = new Font("Segoe UI", 9.5F);
        ClbObrasSociales.FormattingEnabled = true;
        ClbObrasSociales.IntegralHeight = false;
        ClbObrasSociales.Margin = new Padding(0, 0, 0, 4);
        ClbObrasSociales.Size = new Size(0, 110);
        ClbObrasSociales.TabIndex = 16;
        ClbObrasSociales.Name = "ClbObrasSociales";
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
        // FormMedicos
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(1130, 650);
        Controls.Add(pnlDerecho);
        Controls.Add(pnlIzquierdo);
        Controls.Add(pnlEstado);
        Name = "FormMedicos";
        Padding = new Padding(16, 14, 16, 12);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ABM de Médicos";
        pnlEstado.ResumeLayout(false);
        tlpBotonesTarjeta.ResumeLayout(false);
        tlpBotonesTarjeta.PerformLayout();
        pnlBotones.ResumeLayout(false);
        pnlBotones.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)NudPrecioConsultaParticular).EndInit();
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
        ((System.ComponentModel.ISupportInitialize)DgvMedicos).EndInit();
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
    private Label lblBuscar;
    private TextBox TxtBuscar;
    private FlowLayoutPanel FlpContadores;
    private Panel pnlMarcoGrilla;
    private DataGridView DgvMedicos;
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
    private Label lblMatricula;
    private TextBox TxtDni;
    private TextBox TxtMatricula;
    private Label lblPasoEspecialidad;
    private ComboBox CboEspecialidad;
    private Label lblPasoPrecio;
    private NumericUpDown NudPrecioConsultaParticular;
    private Label lblPasoObras;
    private CheckedListBox ClbObrasSociales;
    private Panel pnlBotones;
    private TableLayoutPanel tlpBotonesTarjeta;
    private Button BtnGuardar;
    private Button BtnNuevo;
    private Panel pnlEstado;
    private Label LblMensaje;
}
