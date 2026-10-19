namespace SGC.UI;

partial class FormObrasSociales
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
        pnlMarcoTarjeta = new Panel();
        pnlTarjeta = new Panel();
        tlpTarjeta = new TableLayoutPanel();
        lblTituloTarjeta = new Label();
        LblModo = new Label();
        lblNombre = new Label();
        TxtNombre = new TextBox();
        lblPorcentaje = new Label();
        NudPorcentaje = new NumericUpDown();
        tlpBotonesTarjeta = new TableLayoutPanel();
        BtnGuardar = new Button();
        BtnNuevo = new Button();
        tlpDerecho = new TableLayoutPanel();
        pnlMarcoToolbar = new Panel();
        pnlToolbar = new Panel();
        tlpToolbar = new TableLayoutPanel();
        LblVista = new Label();
        flpFiltros = new FlowLayoutPanel();
        lblBuscar = new Label();
        TxtBuscar = new TextBox();
        FlpContadores = new FlowLayoutPanel();
        pnlMarcoGrilla = new Panel();
        DgvObrasSociales = new DataGridView();
        pnlMarcoAcciones = new Panel();
        pnlAcciones = new Panel();
        tlpAcciones = new TableLayoutPanel();
        LblSeleccion = new Label();
        flpBotonesAccion = new FlowLayoutPanel();
        BtnEliminar = new Button();
        LblAyudaAcciones = new Label();
        LblDetalle = new Label();
        pnlDerecho.SuspendLayout();
        pnlIzquierdo.SuspendLayout();
        pnlEstado.SuspendLayout();
        pnlMarcoTarjeta.SuspendLayout();
        pnlTarjeta.SuspendLayout();
        tlpTarjeta.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)NudPorcentaje).BeginInit();
        tlpBotonesTarjeta.SuspendLayout();
        tlpDerecho.SuspendLayout();
        pnlMarcoToolbar.SuspendLayout();
        pnlToolbar.SuspendLayout();
        tlpToolbar.SuspendLayout();
        flpFiltros.SuspendLayout();
        FlpContadores.SuspendLayout();
        pnlMarcoGrilla.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvObrasSociales).BeginInit();
        pnlMarcoAcciones.SuspendLayout();
        pnlAcciones.SuspendLayout();
        tlpAcciones.SuspendLayout();
        flpBotonesAccion.SuspendLayout();
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
        pnlIzquierdo.Controls.Add(pnlMarcoTarjeta);
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
        pnlEstado.Visible = false;
        pnlEstado.Name = "pnlEstado";
        //
        // LblMensaje
        //
        LblMensaje.Dock = DockStyle.Fill;
        LblMensaje.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        LblMensaje.TabIndex = 0;
        LblMensaje.Name = "LblMensaje";
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
        tlpTarjeta.Controls.Add(lblNombre, 0, 2);
        tlpTarjeta.Controls.Add(TxtNombre, 0, 3);
        tlpTarjeta.Controls.Add(lblPorcentaje, 0, 4);
        tlpTarjeta.Controls.Add(NudPorcentaje, 0, 5);
        tlpTarjeta.Controls.Add(tlpBotonesTarjeta, 0, 6);
        tlpTarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpTarjeta.RowCount = 7;
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.AutoSize = true;
        tlpTarjeta.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpTarjeta.ColumnCount = 1;
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
        lblTituloTarjeta.Text = "NUEVA OBRA SOCIAL";
        lblTituloTarjeta.TabIndex = 0;
        lblTituloTarjeta.Name = "lblTituloTarjeta";
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
        // lblNombre
        //
        lblNombre.AutoSize = true;
        lblNombre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblNombre.ForeColor = Color.FromArgb(27, 42, 74);
        lblNombre.Margin = new Padding(0, 10, 0, 3);
        lblNombre.Text = "1. Nombre";
        lblNombre.TabIndex = 2;
        lblNombre.Name = "lblNombre";
        //
        // TxtNombre
        //
        TxtNombre.Dock = DockStyle.Fill;
        TxtNombre.Font = new Font("Segoe UI", 10F);
        TxtNombre.Margin = new Padding(0, 0, 0, 4);
        TxtNombre.TabIndex = 0;
        TxtNombre.Name = "TxtNombre";
        //
        // lblPorcentaje
        //
        lblPorcentaje.AutoSize = true;
        lblPorcentaje.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPorcentaje.ForeColor = Color.FromArgb(27, 42, 74);
        lblPorcentaje.Margin = new Padding(0, 10, 0, 3);
        lblPorcentaje.Text = "2. Porcentaje de cobertura (%)";
        lblPorcentaje.TabIndex = 4;
        lblPorcentaje.Name = "lblPorcentaje";
        //
        // NudPorcentaje
        //
        NudPorcentaje.DecimalPlaces = 0;
        NudPorcentaje.Dock = DockStyle.Fill;
        NudPorcentaje.Font = new Font("Segoe UI", 10F);
        NudPorcentaje.Margin = new Padding(0, 0, 0, 4);
        NudPorcentaje.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
        NudPorcentaje.TabIndex = 1;
        NudPorcentaje.Name = "NudPorcentaje";
        //
        // tlpBotonesTarjeta
        //
        tlpBotonesTarjeta.Controls.Add(BtnGuardar, 0, 0);
        tlpBotonesTarjeta.Controls.Add(BtnNuevo, 1, 0);
        tlpBotonesTarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
        tlpBotonesTarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
        tlpBotonesTarjeta.RowCount = 1;
        tlpBotonesTarjeta.RowStyles.Add(new RowStyle());
        tlpBotonesTarjeta.AutoSize = true;
        tlpBotonesTarjeta.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpBotonesTarjeta.ColumnCount = 2;
        tlpBotonesTarjeta.Dock = DockStyle.Fill;
        tlpBotonesTarjeta.Margin = new Padding(0, 16, 0, 0);
        tlpBotonesTarjeta.TabIndex = 2;
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
        lblBuscar.TabIndex = 0;
        lblBuscar.Text = "Buscar:";
        lblBuscar.Name = "lblBuscar";
        //
        // TxtBuscar
        //
        TxtBuscar.Font = new Font("Segoe UI", 9.5F);
        TxtBuscar.Margin = new Padding(0, 3, 0, 3);
        TxtBuscar.PlaceholderText = "Nombre de la obra social";
        TxtBuscar.Size = new Size(240, 29);
        TxtBuscar.TabIndex = 1;
        TxtBuscar.Name = "TxtBuscar";
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
        pnlMarcoGrilla.Controls.Add(DgvObrasSociales);
        pnlMarcoGrilla.BackColor = Color.FromArgb(214, 220, 230);
        pnlMarcoGrilla.Dock = DockStyle.Fill;
        pnlMarcoGrilla.Margin = new Padding(0, 0, 0, 8);
        pnlMarcoGrilla.Padding = new Padding(1);
        pnlMarcoGrilla.TabIndex = 2;
        pnlMarcoGrilla.Name = "pnlMarcoGrilla";
        //
        // DgvObrasSociales
        //
        DgvObrasSociales.AllowUserToAddRows = false;
        DgvObrasSociales.AllowUserToDeleteRows = false;
        DgvObrasSociales.AllowUserToResizeRows = false;
        DgvObrasSociales.BackgroundColor = Color.White;
        DgvObrasSociales.BorderStyle = BorderStyle.None;
        DgvObrasSociales.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        DgvObrasSociales.ColumnHeadersHeight = 40;
        DgvObrasSociales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        DgvObrasSociales.Dock = DockStyle.Fill;
        DgvObrasSociales.Font = new Font("Segoe UI", 9F);
        DgvObrasSociales.MultiSelect = false;
        DgvObrasSociales.ReadOnly = true;
        DgvObrasSociales.RowHeadersVisible = false;
        DgvObrasSociales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvObrasSociales.TabIndex = 0;
        DgvObrasSociales.Name = "DgvObrasSociales";
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
        LblSeleccion.Text = "Obra social seleccionada: ninguna";
        LblSeleccion.Name = "LblSeleccion";
        //
        // flpBotonesAccion
        //
        flpBotonesAccion.Controls.Add(BtnEliminar);
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
        // LblAyudaAcciones
        //
        LblAyudaAcciones.AutoSize = true;
        LblAyudaAcciones.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        LblAyudaAcciones.ForeColor = Color.FromArgb(110, 120, 135);
        LblAyudaAcciones.Margin = new Padding(0, 7, 0, 0);
        LblAyudaAcciones.TabIndex = 1;
        LblAyudaAcciones.Visible = false;
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
        // FormObrasSociales
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(1130, 650);
        Controls.Add(pnlDerecho);
        Controls.Add(pnlIzquierdo);
        Controls.Add(pnlEstado);
        Name = "FormObrasSociales";
        Padding = new Padding(16, 14, 16, 12);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Obras Sociales";
        flpBotonesAccion.ResumeLayout(false);
        flpBotonesAccion.PerformLayout();
        tlpAcciones.ResumeLayout(false);
        tlpAcciones.PerformLayout();
        pnlAcciones.ResumeLayout(false);
        pnlAcciones.PerformLayout();
        pnlMarcoAcciones.ResumeLayout(false);
        pnlMarcoAcciones.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)DgvObrasSociales).EndInit();
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
        tlpBotonesTarjeta.ResumeLayout(false);
        tlpBotonesTarjeta.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)NudPorcentaje).EndInit();
        tlpTarjeta.ResumeLayout(false);
        tlpTarjeta.PerformLayout();
        pnlTarjeta.ResumeLayout(false);
        pnlTarjeta.PerformLayout();
        pnlMarcoTarjeta.ResumeLayout(false);
        pnlEstado.ResumeLayout(false);
        pnlIzquierdo.ResumeLayout(false);
        pnlDerecho.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlDerecho;
    private Panel pnlIzquierdo;
    private Panel pnlEstado;
    private Label LblMensaje;
    private Panel pnlMarcoTarjeta;
    private Panel pnlTarjeta;
    private TableLayoutPanel tlpTarjeta;
    private Label lblTituloTarjeta;
    private Label LblModo;
    private Label lblNombre;
    private TextBox TxtNombre;
    private Label lblPorcentaje;
    private NumericUpDown NudPorcentaje;
    private TableLayoutPanel tlpBotonesTarjeta;
    private Button BtnGuardar;
    private Button BtnNuevo;
    private TableLayoutPanel tlpDerecho;
    private Panel pnlMarcoToolbar;
    private Panel pnlToolbar;
    private TableLayoutPanel tlpToolbar;
    private Label LblVista;
    private FlowLayoutPanel flpFiltros;
    private Label lblBuscar;
    private TextBox TxtBuscar;
    private FlowLayoutPanel FlpContadores;
    private Panel pnlMarcoGrilla;
    private DataGridView DgvObrasSociales;
    private Panel pnlMarcoAcciones;
    private Panel pnlAcciones;
    private TableLayoutPanel tlpAcciones;
    private Label LblSeleccion;
    private FlowLayoutPanel flpBotonesAccion;
    private Button BtnEliminar;
    private Label LblAyudaAcciones;
    private Label LblDetalle;
}
