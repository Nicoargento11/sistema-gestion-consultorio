namespace SGC.UI;

partial class FormMenuPrincipal
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

    private Panel pnlSidebar;
    private Panel pnlContenido;
    private Label lblAppName;
    private Label lblUsuarioActivo;
    private Button btnPacientes;
    private Button btnMedicos;
    private Button btnTurnos;
    private Button btnAgenda;
    private Button btnActividad;
    private Button btnHistorial;
    private Button btnCerrarSesion;

    private void InitializeComponent()
    {
        pnlSidebar = new Panel();
        btnCerrarSesion = new Button();
        btnHistorial = new Button();
        btnActividad = new Button();
        btnAgenda = new Button();
        btnTurnos = new Button();
        btnMedicos = new Button();
        btnPacientes = new Button();
        lblUsuarioActivo = new Label();
        lblAppName = new Label();
        pnlContenido = new Panel();
        pnlSidebar.SuspendLayout();
        SuspendLayout();

        // pnlSidebar
        pnlSidebar.BackColor = Color.FromArgb(27, 42, 74);
        pnlSidebar.Controls.Add(btnCerrarSesion);
        pnlSidebar.Controls.Add(btnHistorial);
        pnlSidebar.Controls.Add(btnActividad);
        pnlSidebar.Controls.Add(btnAgenda);
        pnlSidebar.Controls.Add(btnTurnos);
        pnlSidebar.Controls.Add(btnMedicos);
        pnlSidebar.Controls.Add(btnPacientes);
        pnlSidebar.Controls.Add(lblUsuarioActivo);
        pnlSidebar.Controls.Add(lblAppName);
        pnlSidebar.Dock = DockStyle.Left;
        pnlSidebar.Name = "pnlSidebar";
        pnlSidebar.Size = new Size(220, 650);
        pnlSidebar.TabIndex = 1;

        // btnPacientes
        btnPacientes.BackColor = Color.FromArgb(27, 42, 74);
        btnPacientes.FlatAppearance.BorderSize = 0;
        btnPacientes.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 222);
        btnPacientes.FlatStyle = FlatStyle.Flat;
        btnPacientes.Font = new Font("Segoe UI", 10F);
        btnPacientes.ForeColor = Color.White;
        btnPacientes.Location = new Point(0, 150);
        btnPacientes.Name = "btnPacientes";
        btnPacientes.Padding = new Padding(20, 0, 0, 0);
        btnPacientes.Size = new Size(220, 45);
        btnPacientes.TabIndex = 6;
        btnPacientes.Text = "Pacientes";
        btnPacientes.TextAlign = ContentAlignment.MiddleLeft;
        btnPacientes.UseVisualStyleBackColor = false;

        // btnMedicos
        btnMedicos.BackColor = Color.FromArgb(27, 42, 74);
        btnMedicos.FlatAppearance.BorderSize = 0;
        btnMedicos.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 222);
        btnMedicos.FlatStyle = FlatStyle.Flat;
        btnMedicos.Font = new Font("Segoe UI", 10F);
        btnMedicos.ForeColor = Color.White;
        btnMedicos.Location = new Point(0, 200);
        btnMedicos.Name = "btnMedicos";
        btnMedicos.Padding = new Padding(20, 0, 0, 0);
        btnMedicos.Size = new Size(220, 45);
        btnMedicos.TabIndex = 5;
        btnMedicos.Text = "Médicos";
        btnMedicos.TextAlign = ContentAlignment.MiddleLeft;
        btnMedicos.UseVisualStyleBackColor = false;

        // btnTurnos
        btnTurnos.BackColor = Color.FromArgb(27, 42, 74);
        btnTurnos.FlatAppearance.BorderSize = 0;
        btnTurnos.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 222);
        btnTurnos.FlatStyle = FlatStyle.Flat;
        btnTurnos.Font = new Font("Segoe UI", 10F);
        btnTurnos.ForeColor = Color.White;
        btnTurnos.Location = new Point(0, 250);
        btnTurnos.Name = "btnTurnos";
        btnTurnos.Padding = new Padding(20, 0, 0, 0);
        btnTurnos.Size = new Size(220, 45);
        btnTurnos.TabIndex = 4;
        btnTurnos.Text = "Turnos";
        btnTurnos.TextAlign = ContentAlignment.MiddleLeft;
        btnTurnos.UseVisualStyleBackColor = false;

        // btnAgenda
        btnAgenda.BackColor = Color.FromArgb(27, 42, 74);
        btnAgenda.FlatAppearance.BorderSize = 0;
        btnAgenda.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 222);
        btnAgenda.FlatStyle = FlatStyle.Flat;
        btnAgenda.Font = new Font("Segoe UI", 10F);
        btnAgenda.ForeColor = Color.White;
        btnAgenda.Location = new Point(0, 150);
        btnAgenda.Name = "btnAgenda";
        btnAgenda.Padding = new Padding(20, 0, 0, 0);
        btnAgenda.Size = new Size(220, 45);
        btnAgenda.TabIndex = 3;
        btnAgenda.Text = "Mi Agenda";
        btnAgenda.TextAlign = ContentAlignment.MiddleLeft;
        btnAgenda.UseVisualStyleBackColor = false;

        // btnActividad
        btnActividad.BackColor = Color.FromArgb(27, 42, 74);
        btnActividad.FlatAppearance.BorderSize = 0;
        btnActividad.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 222);
        btnActividad.FlatStyle = FlatStyle.Flat;
        btnActividad.Font = new Font("Segoe UI", 10F);
        btnActividad.ForeColor = Color.White;
        btnActividad.Location = new Point(0, 200);
        btnActividad.Name = "btnActividad";
        btnActividad.Padding = new Padding(20, 0, 0, 0);
        btnActividad.Size = new Size(220, 45);
        btnActividad.TabIndex = 2;
        btnActividad.Text = "Registrar Actividad";
        btnActividad.TextAlign = ContentAlignment.MiddleLeft;
        btnActividad.UseVisualStyleBackColor = false;

        // btnHistorial
        btnHistorial.BackColor = Color.FromArgb(27, 42, 74);
        btnHistorial.FlatAppearance.BorderSize = 0;
        btnHistorial.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 222);
        btnHistorial.FlatStyle = FlatStyle.Flat;
        btnHistorial.Font = new Font("Segoe UI", 10F);
        btnHistorial.ForeColor = Color.White;
        btnHistorial.Location = new Point(0, 250);
        btnHistorial.Name = "btnHistorial";
        btnHistorial.Padding = new Padding(20, 0, 0, 0);
        btnHistorial.Size = new Size(220, 45);
        btnHistorial.TabIndex = 1;
        btnHistorial.Text = "Historial Clínico";
        btnHistorial.TextAlign = ContentAlignment.MiddleLeft;
        btnHistorial.UseVisualStyleBackColor = false;

        // btnCerrarSesion
        btnCerrarSesion.BackColor = Color.FromArgb(27, 42, 74);
        btnCerrarSesion.FlatAppearance.BorderSize = 0;
        btnCerrarSesion.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 222);
        btnCerrarSesion.FlatStyle = FlatStyle.Flat;
        btnCerrarSesion.Font = new Font("Segoe UI", 10F);
        btnCerrarSesion.ForeColor = Color.White;
        btnCerrarSesion.Location = new Point(0, 550);
        btnCerrarSesion.Name = "btnCerrarSesion";
        btnCerrarSesion.Padding = new Padding(20, 0, 0, 0);
        btnCerrarSesion.Size = new Size(220, 45);
        btnCerrarSesion.TabIndex = 0;
        btnCerrarSesion.Text = "Cerrar sesión";
        btnCerrarSesion.TextAlign = ContentAlignment.MiddleLeft;
        btnCerrarSesion.UseVisualStyleBackColor = false;

        // lblUsuarioActivo
        lblUsuarioActivo.AutoSize = true;
        lblUsuarioActivo.Font = new Font("Segoe UI", 9F);
        lblUsuarioActivo.ForeColor = Color.FromArgb(180, 195, 220);
        lblUsuarioActivo.Location = new Point(15, 95);
        lblUsuarioActivo.MaximumSize = new Size(190, 0);
        lblUsuarioActivo.Name = "lblUsuarioActivo";

        // lblAppName
        lblAppName.AutoSize = true;
        lblAppName.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblAppName.ForeColor = Color.White;
        lblAppName.Location = new Point(15, 25);
        lblAppName.MaximumSize = new Size(190, 0);
        lblAppName.Name = "lblAppName";
        lblAppName.Text = "Gestión de Consultorio";

        // pnlContenido
        pnlContenido.BackColor = Color.FromArgb(245, 246, 250);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Name = "pnlContenido";
        pnlContenido.TabIndex = 0;

        // FormMenuPrincipal
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 650);
        Controls.Add(pnlContenido);
        Controls.Add(pnlSidebar);
        Name = "FormMenuPrincipal";
        Text = "Sistema de Gestión de Consultorio";
        WindowState = FormWindowState.Maximized;
        pnlSidebar.ResumeLayout(false);
        pnlSidebar.PerformLayout();
        ResumeLayout(false);
    }
}
