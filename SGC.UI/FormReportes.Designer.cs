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
        pnlHeader = new TableLayoutPanel();
        lblTitulo = new Label();
        lblSubtitulo = new Label();
        pnlMensaje = new Panel();
        LblMensaje = new Label();
        TabReportes = new TabControl();
        pnlHeader.SuspendLayout();
        pnlMensaje.SuspendLayout();
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
        lblTitulo.Text = "Reportes y estadísticas";

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
        lblSubtitulo.Text = "Estadísticas del consultorio generadas desde la base de datos";
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
        // TabReportes
        //
        TabReportes.Dock = DockStyle.Fill;
        TabReportes.Font = new Font("Segoe UI", 10F);
        TabReportes.Name = "TabReportes";
        TabReportes.Padding = new Point(14, 4);
        TabReportes.SelectedIndex = 0;
        TabReportes.Size = new Size(1000, 545);
        TabReportes.TabIndex = 1;

        //
        // FormReportes
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(1000, 650);
        Controls.Add(TabReportes);
        Controls.Add(pnlMensaje);
        Controls.Add(pnlHeader);
        Name = "FormReportes";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Reportes y estadísticas";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlMensaje.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    private TableLayoutPanel pnlHeader;
    private Label lblSubtitulo;
    private Label lblTitulo;
    private Panel pnlMensaje;
    private Label LblMensaje;
    private TabControl TabReportes;
}
