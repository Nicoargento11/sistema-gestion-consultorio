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
        lblSubtitulo = new Label();
        lblTitulo = new Label();
        pnlMensaje = new Panel();
        LblMensaje = new Label();
        TabReportes = new TabControl();
        pnlHeader.SuspendLayout();
        pnlMensaje.SuspendLayout();
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
        lblSubtitulo.Text = "Estadísticas del consultorio generadas desde la base de datos";

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
        lblTitulo.Text = "Reportes y estadísticas";

        // 
        // pnlMensaje
        // 
        pnlMensaje.BackColor = Color.FromArgb(245, 246, 250);
        pnlMensaje.Controls.Add(LblMensaje);
        pnlMensaje.Dock = DockStyle.Bottom;
        pnlMensaje.Location = new Point(0, 620);
        pnlMensaje.Name = "pnlMensaje";
        pnlMensaje.Padding = new Padding(20, 6, 20, 6);
        pnlMensaje.Size = new Size(1000, 30);
        pnlMensaje.TabIndex = 2;

        // 
        // LblMensaje
        // 
        LblMensaje.AutoEllipsis = true;
        LblMensaje.Dock = DockStyle.Fill;
        LblMensaje.Font = new Font("Segoe UI", 9.5F);
        LblMensaje.Location = new Point(20, 6);
        LblMensaje.Name = "LblMensaje";
        LblMensaje.Size = new Size(960, 18);
        LblMensaje.TabIndex = 0;
        LblMensaje.TextAlign = ContentAlignment.MiddleLeft;

        // 
        // TabReportes
        // 
        TabReportes.Dock = DockStyle.Fill;
        TabReportes.Font = new Font("Segoe UI", 10F);
        TabReportes.Location = new Point(0, 75);
        TabReportes.Name = "TabReportes";
        TabReportes.Padding = new Point(14, 4);
        TabReportes.SelectedIndex = 0;
        TabReportes.Size = new Size(1000, 545);
        TabReportes.TabIndex = 1;

        // 
        // FormReportes
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
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
    }

    private Panel pnlHeader;
    private Label lblSubtitulo;
    private Label lblTitulo;
    private Panel pnlMensaje;
    private Label LblMensaje;
    private TabControl TabReportes;
}
