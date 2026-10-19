namespace SGC.UI;

partial class FormReportes
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
        lblTitulo = new Label();
        lblSubtitulo = new Label();
        pnlFiltros = new Panel();
        lblFiltroAnio = new Label();
        numAnio = new NumericUpDown();
        lblFiltroMes = new Label();
        cmbMes = new ComboBox();
        btnActualizar = new Button();
        pnlTarjetas = new FlowLayoutPanel();
        tabReportes = new TabControl();
        tabMensual = new TabPage();
        splitContainerMensual = new SplitContainer();
        pnlGraficoMensual = new Panel();
        dgvMensual = new DataGridView();
        tabObrasSociales = new TabPage();
        splitContainerOS = new SplitContainer();
        pnlGraficoOS = new Panel();
        dgvObrasSociales = new DataGridView();
        tabMedicos = new TabPage();
        dgvMedicos = new DataGridView();

        pnlHeader.SuspendLayout();
        pnlFiltros.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numAnio).BeginInit();
        tabReportes.SuspendLayout();
        tabMensual.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainerMensual).BeginInit();
        splitContainerMensual.Panel1.SuspendLayout();
        splitContainerMensual.Panel2.SuspendLayout();
        splitContainerMensual.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMensual).BeginInit();
        tabObrasSociales.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainerOS).BeginInit();
        splitContainerOS.Panel1.SuspendLayout();
        splitContainerOS.Panel2.SuspendLayout();
        splitContainerOS.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvObrasSociales).BeginInit();
        tabMedicos.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMedicos).BeginInit();
        SuspendLayout();

        // pnlHeader
        pnlHeader.BackColor = Color.White;
        pnlHeader.Controls.Add(lblSubtitulo);
        pnlHeader.Controls.Add(lblTitulo);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Padding = new Padding(20, 15, 20, 15);
        pnlHeader.Size = new Size(950, 75);
        pnlHeader.TabIndex = 0;

        // lblTitulo
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.FromArgb(27, 42, 74);
        lblTitulo.Location = new Point(18, 12);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(250, 30);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Reportes y Estadisticas";

        // lblSubtitulo
        lblSubtitulo.AutoSize = true;
        lblSubtitulo.Font = new Font("Segoe UI", 9.5F);
        lblSubtitulo.ForeColor = Color.FromArgb(100, 116, 139);
        lblSubtitulo.Location = new Point(20, 44);
        lblSubtitulo.Name = "lblSubtitulo";
        lblSubtitulo.Size = new Size(390, 17);
        lblSubtitulo.TabIndex = 1;
        lblSubtitulo.Text = "Datos analiticos procesados mediante Procedimientos Almacenados";

        // pnlFiltros
        pnlFiltros.BackColor = Color.White;
        pnlFiltros.Controls.Add(btnActualizar);
        pnlFiltros.Controls.Add(cmbMes);
        pnlFiltros.Controls.Add(lblFiltroMes);
        pnlFiltros.Controls.Add(numAnio);
        pnlFiltros.Controls.Add(lblFiltroAnio);
        pnlFiltros.Dock = DockStyle.Top;
        pnlFiltros.Location = new Point(0, 75);
        pnlFiltros.Name = "pnlFiltros";
        pnlFiltros.Padding = new Padding(20, 5, 20, 10);
        pnlFiltros.Size = new Size(950, 50);
        pnlFiltros.TabIndex = 1;

        // lblFiltroAnio
        lblFiltroAnio.AutoSize = true;
        lblFiltroAnio.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
        lblFiltroAnio.ForeColor = Color.FromArgb(30, 41, 59);
        lblFiltroAnio.Location = new Point(20, 13);
        lblFiltroAnio.Name = "lblFiltroAnio";
        lblFiltroAnio.Size = new Size(36, 17);
        lblFiltroAnio.TabIndex = 0;
        lblFiltroAnio.Text = "Ano:";

        // numAnio
        numAnio.Font = new Font("Segoe UI", 9.5F);
        numAnio.Location = new Point(62, 10);
        numAnio.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
        numAnio.Minimum = new decimal(new int[] { 2020, 0, 0, 0 });
        numAnio.Name = "numAnio";
        numAnio.Size = new Size(80, 24);
        numAnio.TabIndex = 1;
        numAnio.Value = new decimal(new int[] { 2026, 0, 0, 0 });

        // lblFiltroMes
        lblFiltroMes.AutoSize = true;
        lblFiltroMes.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
        lblFiltroMes.ForeColor = Color.FromArgb(30, 41, 59);
        lblFiltroMes.Location = new Point(165, 13);
        lblFiltroMes.Name = "lblFiltroMes";
        lblFiltroMes.Size = new Size(36, 17);
        lblFiltroMes.TabIndex = 2;
        lblFiltroMes.Text = "Mes:";

        // cmbMes
        cmbMes.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMes.Font = new Font("Segoe UI", 9.5F);
        cmbMes.FormattingEnabled = true;
        cmbMes.Location = new Point(207, 10);
        cmbMes.Name = "cmbMes";
        cmbMes.Size = new Size(130, 24);
        cmbMes.TabIndex = 3;

        // btnActualizar
        btnActualizar.BackColor = Color.FromArgb(46, 134, 222);
        btnActualizar.FlatAppearance.BorderSize = 0;
        btnActualizar.FlatStyle = FlatStyle.Flat;
        btnActualizar.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        btnActualizar.ForeColor = Color.White;
        btnActualizar.Location = new Point(355, 9);
        btnActualizar.Name = "btnActualizar";
        btnActualizar.Size = new Size(100, 26);
        btnActualizar.TabIndex = 4;
        btnActualizar.Text = "Actualizar";
        btnActualizar.UseVisualStyleBackColor = false;

        // pnlTarjetas
        pnlTarjetas.BackColor = Color.FromArgb(245, 246, 250);
        pnlTarjetas.Dock = DockStyle.Top;
        pnlTarjetas.Location = new Point(0, 125);
        pnlTarjetas.Name = "pnlTarjetas";
        pnlTarjetas.Padding = new Padding(15, 10, 15, 5);
        pnlTarjetas.Size = new Size(950, 90);
        pnlTarjetas.TabIndex = 2;

        // tabReportes
        tabReportes.Controls.Add(tabMensual);
        tabReportes.Controls.Add(tabObrasSociales);
        tabReportes.Controls.Add(tabMedicos);
        tabReportes.Dock = DockStyle.Fill;
        tabReportes.Font = new Font("Segoe UI", 9.5F);
        tabReportes.Location = new Point(0, 215);
        tabReportes.Name = "tabReportes";
        tabReportes.SelectedIndex = 0;
        tabReportes.Size = new Size(950, 435);
        tabReportes.TabIndex = 3;

        // tabMensual
        tabMensual.BackColor = Color.White;
        tabMensual.Controls.Add(splitContainerMensual);
        tabMensual.Location = new Point(4, 25);
        tabMensual.Name = "tabMensual";
        tabMensual.Padding = new Padding(10);
        tabMensual.Size = new Size(942, 406);
        tabMensual.TabIndex = 0;
        tabMensual.Text = "Pacientes por Mes";

        // splitContainerMensual
        splitContainerMensual.Dock = DockStyle.Fill;
        splitContainerMensual.Location = new Point(10, 10);
        splitContainerMensual.Name = "splitContainerMensual";
        splitContainerMensual.Orientation = Orientation.Horizontal;
        splitContainerMensual.Panel1.Controls.Add(pnlGraficoMensual);
        splitContainerMensual.Panel2.Controls.Add(dgvMensual);
        splitContainerMensual.Size = new Size(922, 386);
        splitContainerMensual.SplitterDistance = 210;
        splitContainerMensual.TabIndex = 0;

        // pnlGraficoMensual
        pnlGraficoMensual.BackColor = Color.White;
        pnlGraficoMensual.Dock = DockStyle.Fill;
        pnlGraficoMensual.Location = new Point(0, 0);
        pnlGraficoMensual.Name = "pnlGraficoMensual";
        pnlGraficoMensual.Size = new Size(922, 210);
        pnlGraficoMensual.TabIndex = 0;

        // dgvMensual
        dgvMensual.BackgroundColor = Color.White;
        dgvMensual.BorderStyle = BorderStyle.None;
        dgvMensual.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvMensual.Dock = DockStyle.Fill;
        dgvMensual.Location = new Point(0, 0);
        dgvMensual.Name = "dgvMensual";
        dgvMensual.ReadOnly = true;
        dgvMensual.RowHeadersVisible = false;
        dgvMensual.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMensual.Size = new Size(922, 172);
        dgvMensual.TabIndex = 0;

        // tabObrasSociales
        tabObrasSociales.BackColor = Color.White;
        tabObrasSociales.Controls.Add(splitContainerOS);
        tabObrasSociales.Location = new Point(4, 25);
        tabObrasSociales.Name = "tabObrasSociales";
        tabObrasSociales.Padding = new Padding(10);
        tabObrasSociales.Size = new Size(942, 406);
        tabObrasSociales.TabIndex = 1;
        tabObrasSociales.Text = "Distribucion Obras Sociales";

        // splitContainerOS
        splitContainerOS.Dock = DockStyle.Fill;
        splitContainerOS.Location = new Point(10, 10);
        splitContainerOS.Name = "splitContainerOS";
        splitContainerOS.Orientation = Orientation.Horizontal;
        splitContainerOS.Panel1.Controls.Add(pnlGraficoOS);
        splitContainerOS.Panel2.Controls.Add(dgvObrasSociales);
        splitContainerOS.Size = new Size(922, 386);
        splitContainerOS.SplitterDistance = 200;
        splitContainerOS.TabIndex = 0;

        // pnlGraficoOS
        pnlGraficoOS.BackColor = Color.White;
        pnlGraficoOS.Dock = DockStyle.Fill;
        pnlGraficoOS.Location = new Point(0, 0);
        pnlGraficoOS.Name = "pnlGraficoOS";
        pnlGraficoOS.Size = new Size(922, 200);
        pnlGraficoOS.TabIndex = 0;

        // dgvObrasSociales
        dgvObrasSociales.BackgroundColor = Color.White;
        dgvObrasSociales.BorderStyle = BorderStyle.None;
        dgvObrasSociales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvObrasSociales.Dock = DockStyle.Fill;
        dgvObrasSociales.Location = new Point(0, 0);
        dgvObrasSociales.Name = "dgvObrasSociales";
        dgvObrasSociales.ReadOnly = true;
        dgvObrasSociales.RowHeadersVisible = false;
        dgvObrasSociales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvObrasSociales.Size = new Size(922, 182);
        dgvObrasSociales.TabIndex = 0;

        // tabMedicos
        tabMedicos.BackColor = Color.White;
        tabMedicos.Controls.Add(dgvMedicos);
        tabMedicos.Location = new Point(4, 25);
        tabMedicos.Name = "tabMedicos";
        tabMedicos.Padding = new Padding(10);
        tabMedicos.Size = new Size(942, 406);
        tabMedicos.TabIndex = 2;
        tabMedicos.Text = "Rendimiento por Medico";

        // dgvMedicos
        dgvMedicos.BackgroundColor = Color.White;
        dgvMedicos.BorderStyle = BorderStyle.None;
        dgvMedicos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvMedicos.Dock = DockStyle.Fill;
        dgvMedicos.Location = new Point(10, 10);
        dgvMedicos.Name = "dgvMedicos";
        dgvMedicos.ReadOnly = true;
        dgvMedicos.RowHeadersVisible = false;
        dgvMedicos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMedicos.Size = new Size(922, 386);
        dgvMedicos.TabIndex = 0;

        // FormReportes
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(950, 650);
        Controls.Add(tabReportes);
        Controls.Add(pnlTarjetas);
        Controls.Add(pnlFiltros);
        Controls.Add(pnlHeader);
        Font = new Font("Segoe UI", 9.5F);
        Name = "FormReportes";
        Text = "Reportes y Estadisticas";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlFiltros.ResumeLayout(false);
        pnlFiltros.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numAnio).EndInit();
        tabReportes.ResumeLayout(false);
        tabMensual.ResumeLayout(false);
        splitContainerMensual.Panel1.ResumeLayout(false);
        splitContainerMensual.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainerMensual).EndInit();
        splitContainerMensual.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvMensual).EndInit();
        tabObrasSociales.ResumeLayout(false);
        splitContainerOS.Panel1.ResumeLayout(false);
        splitContainerOS.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainerOS).EndInit();
        splitContainerOS.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvObrasSociales).EndInit();
        tabMedicos.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvMedicos).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlHeader;
    private Label lblTitulo;
    private Label lblSubtitulo;
    private Panel pnlFiltros;
    private Label lblFiltroAnio;
    private NumericUpDown numAnio;
    private Label lblFiltroMes;
    private ComboBox cmbMes;
    private Button btnActualizar;
    private FlowLayoutPanel pnlTarjetas;
    private TabControl tabReportes;
    private TabPage tabMensual;
    private TabPage tabObrasSociales;
    private TabPage tabMedicos;
    private SplitContainer splitContainerMensual;
    private Panel pnlGraficoMensual;
    private DataGridView dgvMensual;
    private SplitContainer splitContainerOS;
    private Panel pnlGraficoOS;
    private DataGridView dgvObrasSociales;
    private DataGridView dgvMedicos;
}
