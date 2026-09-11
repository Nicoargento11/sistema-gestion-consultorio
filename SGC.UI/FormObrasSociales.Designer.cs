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
        pnlHeader = new Panel();
        lblSubtitulo = new Label();
        lblTitulo = new Label();
        pnlFormulario = new Panel();
        LblMensaje = new Label();
        BtnEliminar = new Button();
        BtnGuardar = new Button();
        BtnNuevo = new Button();
        NudPorcentaje = new NumericUpDown();
        lblPorcentaje = new Label();
        TxtNombre = new TextBox();
        lblNombre = new Label();
        DgvObrasSociales = new DataGridView();
        pnlHeader.SuspendLayout();
        pnlFormulario.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)NudPorcentaje).BeginInit();
        ((System.ComponentModel.ISupportInitialize)DgvObrasSociales).BeginInit();
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
        pnlHeader.Padding = new Padding(20, 10, 20, 10);
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
        lblSubtitulo.Text = "Alta, modificacion y baja del catalogo de obras sociales";
        //
        // lblTitulo
        //
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(18, 10);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(230, 32);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Obras Sociales";
        //
        // pnlFormulario
        //
        pnlFormulario.BackColor = Color.FromArgb(245, 246, 250);
        pnlFormulario.Controls.Add(LblMensaje);
        pnlFormulario.Controls.Add(BtnEliminar);
        pnlFormulario.Controls.Add(BtnGuardar);
        pnlFormulario.Controls.Add(BtnNuevo);
        pnlFormulario.Controls.Add(NudPorcentaje);
        pnlFormulario.Controls.Add(lblPorcentaje);
        pnlFormulario.Controls.Add(TxtNombre);
        pnlFormulario.Controls.Add(lblNombre);
        pnlFormulario.Dock = DockStyle.Top;
        pnlFormulario.Location = new Point(0, 75);
        pnlFormulario.Name = "pnlFormulario";
        pnlFormulario.Padding = new Padding(20, 10, 20, 10);
        pnlFormulario.Size = new Size(1000, 150);
        pnlFormulario.TabIndex = 1;
        //
        // LblMensaje
        //
        LblMensaje.AutoSize = true;
        LblMensaje.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        LblMensaje.Location = new Point(430, 80);
        LblMensaje.MaximumSize = new Size(400, 0);
        LblMensaje.Name = "LblMensaje";
        LblMensaje.Size = new Size(0, 21);
        LblMensaje.TabIndex = 8;
        //
        // BtnEliminar
        //
        BtnEliminar.BackColor = Color.FromArgb(231, 76, 60);
        BtnEliminar.FlatAppearance.BorderSize = 0;
        BtnEliminar.FlatStyle = FlatStyle.Flat;
        BtnEliminar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnEliminar.ForeColor = Color.White;
        BtnEliminar.Location = new Point(240, 100);
        BtnEliminar.Name = "BtnEliminar";
        BtnEliminar.Size = new Size(100, 32);
        BtnEliminar.TabIndex = 4;
        BtnEliminar.Text = "Eliminar";
        BtnEliminar.UseVisualStyleBackColor = false;
        BtnEliminar.Click += BtnEliminar_Click;
        //
        // BtnGuardar
        //
        BtnGuardar.BackColor = Color.FromArgb(39, 174, 96);
        BtnGuardar.FlatAppearance.BorderSize = 0;
        BtnGuardar.FlatStyle = FlatStyle.Flat;
        BtnGuardar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnGuardar.ForeColor = Color.White;
        BtnGuardar.Location = new Point(130, 100);
        BtnGuardar.Name = "BtnGuardar";
        BtnGuardar.Size = new Size(100, 32);
        BtnGuardar.TabIndex = 3;
        BtnGuardar.Text = "Guardar";
        BtnGuardar.UseVisualStyleBackColor = false;
        BtnGuardar.Click += BtnGuardar_Click;
        //
        // BtnNuevo
        //
        BtnNuevo.BackColor = Color.FromArgb(46, 134, 222);
        BtnNuevo.FlatAppearance.BorderSize = 0;
        BtnNuevo.FlatStyle = FlatStyle.Flat;
        BtnNuevo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnNuevo.ForeColor = Color.White;
        BtnNuevo.Location = new Point(20, 100);
        BtnNuevo.Name = "BtnNuevo";
        BtnNuevo.Size = new Size(100, 32);
        BtnNuevo.TabIndex = 2;
        BtnNuevo.Text = "+ Nuevo";
        BtnNuevo.UseVisualStyleBackColor = false;
        BtnNuevo.Click += BtnNuevo_Click;
        //
        // NudPorcentaje
        //
        NudPorcentaje.DecimalPlaces = 0;
        NudPorcentaje.Font = new Font("Segoe UI", 9.5F);
        NudPorcentaje.Location = new Point(215, 28);
        NudPorcentaje.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
        NudPorcentaje.Name = "NudPorcentaje";
        NudPorcentaje.Size = new Size(120, 29);
        NudPorcentaje.TabIndex = 1;
        //
        // lblPorcentaje
        //
        lblPorcentaje.AutoSize = true;
        lblPorcentaje.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPorcentaje.ForeColor = Color.FromArgb(50, 60, 75);
        lblPorcentaje.Location = new Point(215, 8);
        lblPorcentaje.Name = "lblPorcentaje";
        lblPorcentaje.Size = new Size(160, 20);
        lblPorcentaje.TabIndex = 7;
        lblPorcentaje.Text = "% Cobertura";
        //
        // TxtNombre
        //
        TxtNombre.Font = new Font("Segoe UI", 9.5F);
        TxtNombre.Location = new Point(20, 28);
        TxtNombre.Name = "TxtNombre";
        TxtNombre.Size = new Size(180, 29);
        TxtNombre.TabIndex = 0;
        //
        // lblNombre
        //
        lblNombre.AutoSize = true;
        lblNombre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblNombre.ForeColor = Color.FromArgb(50, 60, 75);
        lblNombre.Location = new Point(20, 8);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(67, 20);
        lblNombre.TabIndex = 6;
        lblNombre.Text = "Nombre";
        //
        // DgvObrasSociales
        //
        DgvObrasSociales.AllowUserToAddRows = false;
        DgvObrasSociales.AllowUserToDeleteRows = false;
        DgvObrasSociales.BackgroundColor = Color.White;
        DgvObrasSociales.ColumnHeadersHeight = 34;
        DgvObrasSociales.Dock = DockStyle.Fill;
        DgvObrasSociales.Font = new Font("Segoe UI", 9.5F);
        DgvObrasSociales.Location = new Point(0, 225);
        DgvObrasSociales.MultiSelect = false;
        DgvObrasSociales.Name = "DgvObrasSociales";
        DgvObrasSociales.ReadOnly = true;
        DgvObrasSociales.RowHeadersVisible = false;
        DgvObrasSociales.RowHeadersWidth = 51;
        DgvObrasSociales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvObrasSociales.Size = new Size(1000, 425);
        DgvObrasSociales.TabIndex = 2;
        DgvObrasSociales.SelectionChanged += DgvObrasSociales_SelectionChanged;
        //
        // FormObrasSociales
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(1000, 650);
        Controls.Add(DgvObrasSociales);
        Controls.Add(pnlFormulario);
        Controls.Add(pnlHeader);
        Name = "FormObrasSociales";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Obras Sociales";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlFormulario.ResumeLayout(false);
        pnlFormulario.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)NudPorcentaje).EndInit();
        ((System.ComponentModel.ISupportInitialize)DgvObrasSociales).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlHeader;
    private Label lblTitulo;
    private Label lblSubtitulo;
    private Panel pnlFormulario;
    private Label lblNombre;
    private TextBox TxtNombre;
    private Label lblPorcentaje;
    private NumericUpDown NudPorcentaje;
    private Button BtnNuevo;
    private Button BtnGuardar;
    private Button BtnEliminar;
    private Label LblMensaje;
    private DataGridView DgvObrasSociales;
}
