namespace SGC.UI
{
    partial class FormAtencion
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
            lblPacienteNombre = new Label();
            DtpFecha = new DateTimePicker();
            lblNroAfiliado = new Label();
            lblReceta = new Label();
            Diagnostico = new Label();
            lblSello = new Label();
            lblDireccion = new Label();
            gbDatosPaciente = new GroupBox();
            txtNroAfiliado = new TextBox();
            txtPacienteNombre = new TextBox();
            lblFecha = new Label();
            gbPrescripcion = new GroupBox();
            rtbReceta = new RichTextBox();
            txtDiagnostico = new TextBox();
            btnImprimir = new Button();
            gbDatosPaciente.SuspendLayout();
            gbPrescripcion.SuspendLayout();
            SuspendLayout();
            // 
            // lblPacienteNombre
            // 
            lblPacienteNombre.AutoSize = true;
            lblPacienteNombre.Font = new Font("Segoe UI", 10F);
            lblPacienteNombre.Location = new Point(20, 35);
            lblPacienteNombre.Name = "lblPacienteNombre";
            lblPacienteNombre.Size = new Size(78, 23);
            lblPacienteNombre.TabIndex = 0;
            lblPacienteNombre.Text = "Paciente:";
            // 
            // DtpFecha
            // 
            DtpFecha.Font = new Font("Segoe UI", 10F);
            DtpFecha.Format = DateTimePickerFormat.Custom;
            DtpFecha.Location = new Point(625, 32);
            DtpFecha.Name = "DtpFecha";
            DtpFecha.Size = new Size(110, 30);
            DtpFecha.TabIndex = 1;
            // 
            // lblNroAfiliado
            // 
            lblNroAfiliado.AutoSize = true;
            lblNroAfiliado.Font = new Font("Segoe UI", 10F);
            lblNroAfiliado.Location = new Point(20, 70);
            lblNroAfiliado.Name = "lblNroAfiliado";
            lblNroAfiliado.Size = new Size(120, 23);
            lblNroAfiliado.TabIndex = 2;
            lblNroAfiliado.Text = "Nº de Afiliado:";
            // 
            // lblReceta
            // 
            lblReceta.AutoSize = true;
            lblReceta.Font = new Font("Segoe UI", 14F, FontStyle.Bold | FontStyle.Italic);
            lblReceta.Location = new Point(20, 75);
            lblReceta.Name = "lblReceta";
            lblReceta.Size = new Size(54, 32);
            lblReceta.TabIndex = 3;
            lblReceta.Text = "Rp/";
            // 
            // Diagnostico
            // 
            Diagnostico.AutoSize = true;
            Diagnostico.Font = new Font("Segoe UI", 10F);
            Diagnostico.Location = new Point(20, 35);
            Diagnostico.Name = "Diagnostico";
            Diagnostico.Size = new Size(104, 23);
            Diagnostico.TabIndex = 4;
            Diagnostico.Text = "Diagnóstico:";
            // 
            // lblSello
            // 
            lblSello.AutoSize = true;
            lblSello.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            lblSello.Location = new Point(589, 518);
            lblSello.Name = "lblSello";
            lblSello.Size = new Size(186, 23);
            lblSello.TabIndex = 5;
            lblSello.Text = "Firma y Sello del médico";
            lblSello.Click += lblSello_Click;
            // 
            // lblDireccion
            // 
            lblDireccion.AutoSize = true;
            lblDireccion.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDireccion.ForeColor = Color.FromArgb(64, 64, 64);
            lblDireccion.Location = new Point(481, 23);
            lblDireccion.Name = "lblDireccion";
            lblDireccion.Size = new Size(255, 23);
            lblDireccion.TabIndex = 6;
            lblDireccion.Text = "DIRECCIÓN DE AYUDA SOCIAL";
            // 
            // gbDatosPaciente
            // 
            gbDatosPaciente.Controls.Add(txtNroAfiliado);
            gbDatosPaciente.Controls.Add(lblNroAfiliado);
            gbDatosPaciente.Controls.Add(txtPacienteNombre);
            gbDatosPaciente.Controls.Add(lblPacienteNombre);
            gbDatosPaciente.Controls.Add(DtpFecha);
            gbDatosPaciente.Controls.Add(lblFecha);
            gbDatosPaciente.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbDatosPaciente.Location = new Point(25, 70);
            gbDatosPaciente.Name = "gbDatosPaciente";
            gbDatosPaciente.Size = new Size(750, 100);
            gbDatosPaciente.TabIndex = 7;
            gbDatosPaciente.TabStop = false;
            gbDatosPaciente.Text = "Datos del Afiliado";
            // 
            // txtNroAfiliado
            // 
            txtNroAfiliado.Font = new Font("Segoe UI", 10F);
            txtNroAfiliado.Location = new Point(145, 67);
            txtNroAfiliado.Name = "txtNroAfiliado";
            txtNroAfiliado.Size = new Size(255, 30);
            txtNroAfiliado.TabIndex = 3;
            // 
            // txtPacienteNombre
            // 
            txtPacienteNombre.Font = new Font("Segoe UI", 10F);
            txtPacienteNombre.Location = new Point(100, 32);
            txtPacienteNombre.Name = "txtPacienteNombre";
            txtPacienteNombre.Size = new Size(300, 30);
            txtPacienteNombre.TabIndex = 1;
            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Font = new Font("Segoe UI", 10F);
            lblFecha.Location = new Point(480, 35);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(146, 23);
            lblFecha.TabIndex = 8;
            lblFecha.Text = "Fecha de Emisión:";
            // 
            // gbPrescripcion
            // 
            gbPrescripcion.Controls.Add(rtbReceta);
            gbPrescripcion.Controls.Add(txtDiagnostico);
            gbPrescripcion.Controls.Add(Diagnostico);
            gbPrescripcion.Controls.Add(lblReceta);
            gbPrescripcion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbPrescripcion.Location = new Point(25, 190);
            gbPrescripcion.Name = "gbPrescripcion";
            gbPrescripcion.Size = new Size(750, 260);
            gbPrescripcion.TabIndex = 8;
            gbPrescripcion.TabStop = false;
            gbPrescripcion.Text = "Prescripción Médica";
            // 
            // rtbReceta
            // 
            rtbReceta.Font = new Font("Segoe UI", 11F);
            rtbReceta.Location = new Point(85, 75);
            rtbReceta.Name = "rtbReceta";
            rtbReceta.Size = new Size(645, 160);
            rtbReceta.TabIndex = 6;
            rtbReceta.Text = "";
            // 
            // txtDiagnostico
            // 
            txtDiagnostico.Font = new Font("Segoe UI", 10F);
            txtDiagnostico.Location = new Point(130, 32);
            txtDiagnostico.Name = "txtDiagnostico";
            txtDiagnostico.Size = new Size(600, 30);
            txtDiagnostico.TabIndex = 5;
            // 
            // btnImprimir
            // 
            btnImprimir.BackColor = Color.SeaGreen;
            btnImprimir.FlatAppearance.BorderSize = 0;
            btnImprimir.FlatStyle = FlatStyle.Flat;
            btnImprimir.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnImprimir.ForeColor = Color.White;
            btnImprimir.Location = new Point(25, 480);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(220, 45);
            btnImprimir.TabIndex = 9;
            btnImprimir.Text = "🖨️ Imprimir Receta";
            btnImprimir.UseVisualStyleBackColor = false;
            // 
            // FormAtencion
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 550);
            Controls.Add(btnImprimir);
            Controls.Add(gbPrescripcion);
            Controls.Add(gbDatosPaciente);
            Controls.Add(lblDireccion);
            Controls.Add(lblSello);
            Name = "FormAtencion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Atención al Paciente - Emisión de Recetas";
            Load += FormAtencion_Load_1;
            gbDatosPaciente.ResumeLayout(false);
            gbDatosPaciente.PerformLayout();
            gbPrescripcion.ResumeLayout(false);
            gbPrescripcion.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPacienteNombre;
        private DateTimePicker DtpFecha;
        private Label lblNroAfiliado;
        private Label lblReceta;
        private Label Diagnostico;
        private Label lblSello;
        private Label lblDireccion;

        // Declaración de los controles nuevos
        private GroupBox gbDatosPaciente;
        private TextBox txtPacienteNombre;
        private TextBox txtNroAfiliado;
        private Label lblFecha;

        private GroupBox gbPrescripcion;
        private TextBox txtDiagnostico;
        private RichTextBox rtbReceta;

        private Button btnImprimir;
    }
}