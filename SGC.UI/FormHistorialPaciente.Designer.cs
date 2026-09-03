namespace SGC.UI;

partial class FormHistorialPaciente
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

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        pnlHeader = new Panel();
        lblPacienteInfo = new Label();
        lblTitulo = new Label();
        DgvHistorial = new DataGridView();
        pnlDetalle = new Panel();
        BtnCerrar = new Button();
        TxtReceta = new TextBox();
        lblReceta = new Label();
        TxtDiagnostico = new TextBox();
        lblDiagnostico = new Label();
        TxtMotivo = new TextBox();
        lblMotivo = new Label();
        lblDetalleTitulo = new Label();
        pnlHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvHistorial).BeginInit();
        pnlDetalle.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(27, 42, 74);
        pnlHeader.Controls.Add(lblPacienteInfo);
        pnlHeader.Controls.Add(lblTitulo);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Padding = new Padding(15);
        pnlHeader.Size = new Size(850, 75);
        pnlHeader.TabIndex = 0;
        // 
        // lblPacienteInfo
        // 
        lblPacienteInfo.AutoSize = true;
        lblPacienteInfo.Font = new Font("Segoe UI", 10F);
        lblPacienteInfo.ForeColor = Color.FromArgb(180, 205, 235);
        lblPacienteInfo.Location = new Point(15, 42);
        lblPacienteInfo.Name = "lblPacienteInfo";
        lblPacienteInfo.Size = new Size(183, 23);
        lblPacienteInfo.TabIndex = 1;
        lblPacienteInfo.Text = "Paciente: ... | DNI: ...";
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(14, 12);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(385, 30);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Historial Clinico de Atenciones (RF#09)";
        // 
        // DgvHistorial
        // 
        DgvHistorial.AllowUserToAddRows = false;
        DgvHistorial.AllowUserToDeleteRows = false;
        DgvHistorial.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        DgvHistorial.BackgroundColor = Color.White;
        DgvHistorial.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        DgvHistorial.Location = new Point(15, 90);
        DgvHistorial.MultiSelect = false;
        DgvHistorial.Name = "DgvHistorial";
        DgvHistorial.ReadOnly = true;
        DgvHistorial.RowHeadersVisible = false;
        DgvHistorial.RowHeadersWidth = 51;
        DgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvHistorial.Size = new Size(820, 230);
        DgvHistorial.TabIndex = 1;
        // 
        // pnlDetalle
        // 
        pnlDetalle.BackColor = Color.White;
        pnlDetalle.Controls.Add(BtnCerrar);
        pnlDetalle.Controls.Add(TxtReceta);
        pnlDetalle.Controls.Add(lblReceta);
        pnlDetalle.Controls.Add(TxtDiagnostico);
        pnlDetalle.Controls.Add(lblDiagnostico);
        pnlDetalle.Controls.Add(TxtMotivo);
        pnlDetalle.Controls.Add(lblMotivo);
        pnlDetalle.Controls.Add(lblDetalleTitulo);
        pnlDetalle.Dock = DockStyle.Bottom;
        pnlDetalle.Location = new Point(0, 335);
        pnlDetalle.Name = "pnlDetalle";
        pnlDetalle.Padding = new Padding(15);
        pnlDetalle.Size = new Size(850, 265);
        pnlDetalle.TabIndex = 2;
        // 
        // BtnCerrar
        // 
        BtnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnCerrar.BackColor = Color.FromArgb(27, 42, 74);
        BtnCerrar.FlatAppearance.BorderSize = 0;
        BtnCerrar.FlatStyle = FlatStyle.Flat;
        BtnCerrar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnCerrar.ForeColor = Color.White;
        BtnCerrar.Location = new Point(720, 220);
        BtnCerrar.Name = "BtnCerrar";
        BtnCerrar.Size = new Size(115, 35);
        BtnCerrar.TabIndex = 7;
        BtnCerrar.Text = "Cerrar";
        BtnCerrar.UseVisualStyleBackColor = false;
        // 
        // TxtReceta
        // 
        TxtReceta.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        TxtReceta.Font = new Font("Segoe UI", 9F);
        TxtReceta.Location = new Point(15, 175);
        TxtReceta.Multiline = true;
        TxtReceta.Name = "TxtReceta";
        TxtReceta.ReadOnly = true;
        TxtReceta.ScrollBars = ScrollBars.Vertical;
        TxtReceta.Size = new Size(820, 38);
        TxtReceta.TabIndex = 6;
        // 
        // lblReceta
        // 
        lblReceta.AutoSize = true;
        lblReceta.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblReceta.ForeColor = Color.FromArgb(50, 60, 75);
        lblReceta.Location = new Point(15, 155);
        lblReceta.Name = "lblReceta";
        lblReceta.Size = new Size(157, 20);
        lblReceta.TabIndex = 5;
        lblReceta.Text = "Prescripcion / Receta:";
        // 
        // TxtDiagnostico
        // 
        TxtDiagnostico.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        TxtDiagnostico.Font = new Font("Segoe UI", 9F);
        TxtDiagnostico.Location = new Point(15, 110);
        TxtDiagnostico.Multiline = true;
        TxtDiagnostico.Name = "TxtDiagnostico";
        TxtDiagnostico.ReadOnly = true;
        TxtDiagnostico.ScrollBars = ScrollBars.Vertical;
        TxtDiagnostico.Size = new Size(820, 38);
        TxtDiagnostico.TabIndex = 4;
        // 
        // lblDiagnostico
        // 
        lblDiagnostico.AutoSize = true;
        lblDiagnostico.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblDiagnostico.ForeColor = Color.FromArgb(50, 60, 75);
        lblDiagnostico.Location = new Point(15, 90);
        lblDiagnostico.Name = "lblDiagnostico";
        lblDiagnostico.Size = new Size(207, 20);
        lblDiagnostico.TabIndex = 3;
        lblDiagnostico.Text = "Diagnostico / Procedimiento:";
        // 
        // TxtMotivo
        // 
        TxtMotivo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        TxtMotivo.Font = new Font("Segoe UI", 9F);
        TxtMotivo.Location = new Point(15, 45);
        TxtMotivo.Multiline = true;
        TxtMotivo.Name = "TxtMotivo";
        TxtMotivo.ReadOnly = true;
        TxtMotivo.ScrollBars = ScrollBars.Vertical;
        TxtMotivo.Size = new Size(820, 38);
        TxtMotivo.TabIndex = 2;
        // 
        // lblMotivo
        // 
        lblMotivo.AutoSize = true;
        lblMotivo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblMotivo.ForeColor = Color.FromArgb(50, 60, 75);
        lblMotivo.Location = new Point(15, 25);
        lblMotivo.Name = "lblMotivo";
        lblMotivo.Size = new Size(146, 20);
        lblMotivo.TabIndex = 1;
        lblMotivo.Text = "Motivo de Consulta:";
        // 
        // lblDetalleTitulo
        // 
        lblDetalleTitulo.AutoSize = true;
        lblDetalleTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblDetalleTitulo.ForeColor = Color.FromArgb(27, 42, 74);
        lblDetalleTitulo.Location = new Point(15, 2);
        lblDetalleTitulo.Name = "lblDetalleTitulo";
        lblDetalleTitulo.Size = new Size(276, 23);
        lblDetalleTitulo.TabIndex = 0;
        lblDetalleTitulo.Text = "Detalle de la Atencion Clinica:";
        // 
        // FormHistorialPaciente
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(850, 600);
        Controls.Add(pnlDetalle);
        Controls.Add(DgvHistorial);
        Controls.Add(pnlHeader);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormHistorialPaciente";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Historial Clinico del Paciente";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)DgvHistorial).EndInit();
        pnlDetalle.ResumeLayout(false);
        pnlDetalle.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Panel pnlHeader;
    private Label lblTitulo;
    private Label lblPacienteInfo;
    private DataGridView DgvHistorial;
    private Panel pnlDetalle;
    private Label lblDetalleTitulo;
    private Label lblMotivo;
    private TextBox TxtMotivo;
    private Label lblDiagnostico;
    private TextBox TxtDiagnostico;
    private Label lblReceta;
    private TextBox TxtReceta;
    private Button BtnCerrar;
}
