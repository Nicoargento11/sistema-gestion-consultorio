namespace SGC.UI;

partial class FormBackup
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
        pnlHeader = new TableLayoutPanel();
        lblTitulo = new Label();
        lblSubtitulo = new Label();
        pnlMensaje = new Panel();
        LblMensaje = new Label();
        tlpPrincipal = new TableLayoutPanel();
        cardCrear = new Panel();
        tlpCrear = new TableLayoutPanel();
        lblCrearTitulo = new Label();
        lblCrearAyuda = new Label();
        txtCarpeta = new TextBox();
        btnExaminar = new Button();
        btnCrear = new Button();
        cardHistorial = new Panel();
        tlpHistorial = new TableLayoutPanel();
        lblHistorialTitulo = new Label();
        lblHistorialEstado = new Label();
        dgvHistorial = new DataGridView();
        cardRestaurar = new Panel();
        tlpRestaurar = new TableLayoutPanel();
        lblRestaurarTitulo = new Label();
        lblRestaurarAdvertencia = new Label();
        txtArchivo = new TextBox();
        btnElegirArchivo = new Button();
        btnUsarSeleccionada = new Button();
        btnRestaurar = new Button();
        pnlHeader.SuspendLayout();
        pnlMensaje.SuspendLayout();
        tlpPrincipal.SuspendLayout();
        cardCrear.SuspendLayout();
        tlpCrear.SuspendLayout();
        cardHistorial.SuspendLayout();
        tlpHistorial.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHistorial).BeginInit();
        cardRestaurar.SuspendLayout();
        tlpRestaurar.SuspendLayout();
        SuspendLayout();

        //
        // pnlHeader
        //
        pnlHeader.AutoSize = true;
        pnlHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlHeader.BackColor = Color.FromArgb(27, 42, 74);
        pnlHeader.ColumnCount = 2;
        pnlHeader.ColumnStyles.Add(new ColumnStyle());
        pnlHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        pnlHeader.Controls.Add(lblTitulo, 0, 0);
        pnlHeader.Controls.Add(lblSubtitulo, 1, 0);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Padding = new Padding(20, 8, 20, 8);
        pnlHeader.RowCount = 1;
        pnlHeader.RowStyles.Add(new RowStyle());
        pnlHeader.TabIndex = 0;

        //
        // lblTitulo
        //
        lblTitulo.Anchor = AnchorStyles.Left;
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Margin = new Padding(0, 0, 18, 0);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Copia de seguridad";

        //
        // lblSubtitulo
        //
        lblSubtitulo.AutoEllipsis = true;
        lblSubtitulo.Dock = DockStyle.Fill;
        lblSubtitulo.Font = new Font("Segoe UI", 10F);
        lblSubtitulo.ForeColor = Color.FromArgb(180, 205, 235);
        lblSubtitulo.Margin = new Padding(0);
        lblSubtitulo.Name = "lblSubtitulo";
        lblSubtitulo.Size = new Size(0, 23);
        lblSubtitulo.TabIndex = 1;
        lblSubtitulo.Text = "Crear, verificar y restaurar copias de la base de datos";
        lblSubtitulo.TextAlign = ContentAlignment.MiddleLeft;

        //
        // pnlMensaje
        //
        pnlMensaje.BackColor = Color.FromArgb(245, 246, 250);
        pnlMensaje.Controls.Add(LblMensaje);
        pnlMensaje.Dock = DockStyle.Bottom;
        pnlMensaje.Name = "pnlMensaje";
        pnlMensaje.Padding = new Padding(20, 8, 20, 0);
        pnlMensaje.Size = new Size(1000, 36);
        pnlMensaje.TabIndex = 2;
        pnlMensaje.Visible = false;

        //
        // LblMensaje
        //
        LblMensaje.Dock = DockStyle.Fill;
        LblMensaje.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        LblMensaje.Name = "LblMensaje";
        LblMensaje.TabIndex = 0;
        LblMensaje.TextAlign = ContentAlignment.MiddleLeft;

        //
        // tlpPrincipal: tres tarjetas en columna; el historial se lleva el espacio sobrante.
        //
        tlpPrincipal.ColumnCount = 1;
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpPrincipal.Controls.Add(cardCrear, 0, 0);
        tlpPrincipal.Controls.Add(cardHistorial, 0, 1);
        tlpPrincipal.Controls.Add(cardRestaurar, 0, 2);
        tlpPrincipal.Dock = DockStyle.Fill;
        tlpPrincipal.Name = "tlpPrincipal";
        tlpPrincipal.Padding = new Padding(20, 16, 20, 12);
        tlpPrincipal.RowCount = 3;
        tlpPrincipal.RowStyles.Add(new RowStyle());
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpPrincipal.RowStyles.Add(new RowStyle());
        tlpPrincipal.TabIndex = 1;

        //
        // cardCrear
        //
        cardCrear.AutoSize = true;
        cardCrear.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        cardCrear.BackColor = Color.White;
        cardCrear.Controls.Add(tlpCrear);
        cardCrear.Dock = DockStyle.Fill;
        cardCrear.Margin = new Padding(0, 0, 0, 12);
        cardCrear.Name = "cardCrear";
        cardCrear.Padding = new Padding(16, 12, 16, 14);
        cardCrear.TabIndex = 0;

        //
        // tlpCrear
        //
        tlpCrear.AutoSize = true;
        tlpCrear.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpCrear.ColumnCount = 3;
        tlpCrear.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpCrear.ColumnStyles.Add(new ColumnStyle());
        tlpCrear.ColumnStyles.Add(new ColumnStyle());
        tlpCrear.Controls.Add(lblCrearTitulo, 0, 0);
        tlpCrear.SetColumnSpan(lblCrearTitulo, 3);
        tlpCrear.Controls.Add(lblCrearAyuda, 0, 1);
        tlpCrear.SetColumnSpan(lblCrearAyuda, 3);
        tlpCrear.Controls.Add(txtCarpeta, 0, 2);
        tlpCrear.Controls.Add(btnExaminar, 1, 2);
        tlpCrear.Controls.Add(btnCrear, 2, 2);
        tlpCrear.Dock = DockStyle.Top;
        tlpCrear.Name = "tlpCrear";
        tlpCrear.RowCount = 3;
        tlpCrear.RowStyles.Add(new RowStyle());
        tlpCrear.RowStyles.Add(new RowStyle());
        tlpCrear.RowStyles.Add(new RowStyle());
        tlpCrear.TabIndex = 0;

        //
        // lblCrearTitulo
        //
        lblCrearTitulo.AutoSize = true;
        lblCrearTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblCrearTitulo.ForeColor = Color.FromArgb(27, 42, 74);
        lblCrearTitulo.Margin = new Padding(0, 0, 0, 4);
        lblCrearTitulo.Name = "lblCrearTitulo";
        lblCrearTitulo.TabIndex = 0;
        lblCrearTitulo.Text = "Crear copia";

        //
        // lblCrearAyuda
        //
        lblCrearAyuda.AutoSize = true;
        lblCrearAyuda.Font = new Font("Segoe UI", 9.5F);
        lblCrearAyuda.ForeColor = Color.FromArgb(110, 120, 135);
        lblCrearAyuda.Margin = new Padding(0, 0, 0, 8);
        lblCrearAyuda.Name = "lblCrearAyuda";
        lblCrearAyuda.TabIndex = 1;
        lblCrearAyuda.Text = "La copia la escribe el servicio de SQL Server, no esta aplicación: la carpeta debe existir en el equipo del servidor y esa cuenta de servicio debe poder escribir en ella. Por eso se sugiere la carpeta de copias predeterminada de la instancia. La copia se verifica automáticamente al terminar.";

        //
        // txtCarpeta
        //
        txtCarpeta.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtCarpeta.Font = new Font("Segoe UI", 10F);
        txtCarpeta.Margin = new Padding(0, 0, 8, 0);
        txtCarpeta.Name = "txtCarpeta";
        txtCarpeta.TabIndex = 2;

        //
        // btnExaminar
        //
        btnExaminar.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        btnExaminar.AutoSize = true;
        btnExaminar.Font = new Font("Segoe UI", 10F);
        btnExaminar.Margin = new Padding(0, 0, 8, 0);
        btnExaminar.MinimumSize = new Size(110, 34);
        btnExaminar.Name = "btnExaminar";
        btnExaminar.TabIndex = 3;
        btnExaminar.Text = "Examinar...";

        //
        // btnCrear
        //
        btnCrear.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        btnCrear.AutoSize = true;
        btnCrear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnCrear.Margin = new Padding(0);
        btnCrear.MinimumSize = new Size(170, 34);
        btnCrear.Name = "btnCrear";
        btnCrear.TabIndex = 4;
        btnCrear.Text = "Crear copia ahora";

        //
        // cardHistorial
        //
        cardHistorial.BackColor = Color.White;
        cardHistorial.Controls.Add(tlpHistorial);
        cardHistorial.Dock = DockStyle.Fill;
        cardHistorial.Margin = new Padding(0, 0, 0, 12);
        cardHistorial.MinimumSize = new Size(0, 140);
        cardHistorial.Name = "cardHistorial";
        cardHistorial.Padding = new Padding(16, 12, 16, 12);
        cardHistorial.TabIndex = 1;

        //
        // tlpHistorial
        //
        tlpHistorial.ColumnCount = 1;
        tlpHistorial.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpHistorial.Controls.Add(lblHistorialTitulo, 0, 0);
        tlpHistorial.Controls.Add(lblHistorialEstado, 0, 1);
        tlpHistorial.Controls.Add(dgvHistorial, 0, 2);
        tlpHistorial.Dock = DockStyle.Fill;
        tlpHistorial.Name = "tlpHistorial";
        tlpHistorial.RowCount = 3;
        tlpHistorial.RowStyles.Add(new RowStyle());
        tlpHistorial.RowStyles.Add(new RowStyle());
        tlpHistorial.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpHistorial.TabIndex = 0;

        //
        // lblHistorialTitulo
        //
        lblHistorialTitulo.AutoSize = true;
        lblHistorialTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblHistorialTitulo.ForeColor = Color.FromArgb(27, 42, 74);
        lblHistorialTitulo.Margin = new Padding(0, 0, 0, 6);
        lblHistorialTitulo.Name = "lblHistorialTitulo";
        lblHistorialTitulo.TabIndex = 0;
        lblHistorialTitulo.Text = "Historial";

        //
        // lblHistorialEstado
        //
        lblHistorialEstado.AutoSize = true;
        lblHistorialEstado.Font = new Font("Segoe UI", 9.5F);
        lblHistorialEstado.ForeColor = Color.FromArgb(110, 120, 135);
        lblHistorialEstado.Margin = new Padding(0, 0, 0, 6);
        lblHistorialEstado.Name = "lblHistorialEstado";
        lblHistorialEstado.TabIndex = 1;
        lblHistorialEstado.Visible = false;

        //
        // dgvHistorial
        //
        dgvHistorial.AllowUserToAddRows = false;
        dgvHistorial.AllowUserToDeleteRows = false;
        dgvHistorial.AllowUserToResizeRows = false;
        dgvHistorial.BackgroundColor = Color.White;
        dgvHistorial.BorderStyle = BorderStyle.None;
        dgvHistorial.Dock = DockStyle.Fill;
        dgvHistorial.Font = new Font("Segoe UI", 9.5F);
        dgvHistorial.Margin = new Padding(0);
        dgvHistorial.MultiSelect = false;
        dgvHistorial.Name = "dgvHistorial";
        dgvHistorial.ReadOnly = true;
        dgvHistorial.RowHeadersVisible = false;
        dgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvHistorial.TabIndex = 2;

        //
        // cardRestaurar
        //
        cardRestaurar.AutoSize = true;
        cardRestaurar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        cardRestaurar.BackColor = Color.White;
        cardRestaurar.Controls.Add(tlpRestaurar);
        cardRestaurar.Dock = DockStyle.Fill;
        cardRestaurar.Margin = new Padding(0);
        cardRestaurar.Name = "cardRestaurar";
        cardRestaurar.Padding = new Padding(16, 12, 16, 14);
        cardRestaurar.TabIndex = 2;

        //
        // tlpRestaurar
        //
        tlpRestaurar.AutoSize = true;
        tlpRestaurar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpRestaurar.ColumnCount = 4;
        tlpRestaurar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRestaurar.ColumnStyles.Add(new ColumnStyle());
        tlpRestaurar.ColumnStyles.Add(new ColumnStyle());
        tlpRestaurar.ColumnStyles.Add(new ColumnStyle());
        tlpRestaurar.Controls.Add(lblRestaurarTitulo, 0, 0);
        tlpRestaurar.SetColumnSpan(lblRestaurarTitulo, 4);
        tlpRestaurar.Controls.Add(lblRestaurarAdvertencia, 0, 1);
        tlpRestaurar.SetColumnSpan(lblRestaurarAdvertencia, 4);
        tlpRestaurar.Controls.Add(txtArchivo, 0, 2);
        tlpRestaurar.Controls.Add(btnElegirArchivo, 1, 2);
        tlpRestaurar.Controls.Add(btnUsarSeleccionada, 2, 2);
        tlpRestaurar.Controls.Add(btnRestaurar, 3, 2);
        tlpRestaurar.Dock = DockStyle.Top;
        tlpRestaurar.Name = "tlpRestaurar";
        tlpRestaurar.RowCount = 3;
        tlpRestaurar.RowStyles.Add(new RowStyle());
        tlpRestaurar.RowStyles.Add(new RowStyle());
        tlpRestaurar.RowStyles.Add(new RowStyle());
        tlpRestaurar.TabIndex = 0;

        //
        // lblRestaurarTitulo
        //
        lblRestaurarTitulo.AutoSize = true;
        lblRestaurarTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblRestaurarTitulo.ForeColor = Color.FromArgb(27, 42, 74);
        lblRestaurarTitulo.Margin = new Padding(0, 0, 0, 4);
        lblRestaurarTitulo.Name = "lblRestaurarTitulo";
        lblRestaurarTitulo.TabIndex = 0;
        lblRestaurarTitulo.Text = "Restaurar";

        //
        // lblRestaurarAdvertencia
        //
        lblRestaurarAdvertencia.AutoSize = true;
        lblRestaurarAdvertencia.Font = new Font("Segoe UI", 9.5F);
        lblRestaurarAdvertencia.ForeColor = Color.FromArgb(192, 57, 43);
        lblRestaurarAdvertencia.Margin = new Padding(0, 0, 0, 8);
        lblRestaurarAdvertencia.Name = "lblRestaurarAdvertencia";
        lblRestaurarAdvertencia.TabIndex = 1;
        lblRestaurarAdvertencia.Text = "Restaurar reemplaza todos los datos actuales por los de la copia y cierra todas las conexiones a la base. Elija el archivo .bak (ruta vista desde el servidor) o use la fila seleccionada del historial.";

        //
        // txtArchivo
        //
        txtArchivo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtArchivo.Font = new Font("Segoe UI", 10F);
        txtArchivo.Margin = new Padding(0, 0, 8, 0);
        txtArchivo.Name = "txtArchivo";
        txtArchivo.TabIndex = 2;

        //
        // btnElegirArchivo
        //
        btnElegirArchivo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        btnElegirArchivo.AutoSize = true;
        btnElegirArchivo.Font = new Font("Segoe UI", 10F);
        btnElegirArchivo.Margin = new Padding(0, 0, 8, 0);
        btnElegirArchivo.MinimumSize = new Size(110, 34);
        btnElegirArchivo.Name = "btnElegirArchivo";
        btnElegirArchivo.TabIndex = 3;
        btnElegirArchivo.Text = "Elegir archivo...";

        //
        // btnUsarSeleccionada
        //
        btnUsarSeleccionada.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        btnUsarSeleccionada.AutoSize = true;
        btnUsarSeleccionada.Font = new Font("Segoe UI", 10F);
        btnUsarSeleccionada.Margin = new Padding(0, 0, 8, 0);
        btnUsarSeleccionada.MinimumSize = new Size(110, 34);
        btnUsarSeleccionada.Name = "btnUsarSeleccionada";
        btnUsarSeleccionada.TabIndex = 4;
        btnUsarSeleccionada.Text = "Usar la seleccionada";

        //
        // btnRestaurar
        //
        btnRestaurar.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        btnRestaurar.AutoSize = true;
        btnRestaurar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnRestaurar.Margin = new Padding(0);
        btnRestaurar.MinimumSize = new Size(150, 34);
        btnRestaurar.Name = "btnRestaurar";
        btnRestaurar.TabIndex = 5;
        btnRestaurar.Text = "Restaurar copia";

        //
        // FormBackup
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(1000, 650);
        Controls.Add(tlpPrincipal);
        Controls.Add(pnlMensaje);
        Controls.Add(pnlHeader);
        Name = "FormBackup";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Copia de seguridad";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlMensaje.ResumeLayout(false);
        tlpPrincipal.ResumeLayout(false);
        tlpPrincipal.PerformLayout();
        cardCrear.ResumeLayout(false);
        cardCrear.PerformLayout();
        tlpCrear.ResumeLayout(false);
        tlpCrear.PerformLayout();
        cardHistorial.ResumeLayout(false);
        tlpHistorial.ResumeLayout(false);
        tlpHistorial.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHistorial).EndInit();
        cardRestaurar.ResumeLayout(false);
        cardRestaurar.PerformLayout();
        tlpRestaurar.ResumeLayout(false);
        tlpRestaurar.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private TableLayoutPanel pnlHeader;
    private Label lblSubtitulo;
    private Label lblTitulo;
    private Panel pnlMensaje;
    private Label LblMensaje;
    private TableLayoutPanel tlpPrincipal;
    private Panel cardCrear;
    private TableLayoutPanel tlpCrear;
    private Label lblCrearTitulo;
    private Label lblCrearAyuda;
    private TextBox txtCarpeta;
    private Button btnExaminar;
    private Button btnCrear;
    private Panel cardHistorial;
    private TableLayoutPanel tlpHistorial;
    private Label lblHistorialTitulo;
    private Label lblHistorialEstado;
    private DataGridView dgvHistorial;
    private Panel cardRestaurar;
    private TableLayoutPanel tlpRestaurar;
    private Label lblRestaurarTitulo;
    private Label lblRestaurarAdvertencia;
    private TextBox txtArchivo;
    private Button btnElegirArchivo;
    private Button btnUsarSeleccionada;
    private Button btnRestaurar;
}
