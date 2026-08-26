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
        pnlContenido = new Panel();
        lblAppName = new Label();
        lblUsuarioActivo = new Label();
        btnPacientes = new Button();
        btnMedicos = new Button();
        btnTurnos = new Button();
        btnAgenda = new Button();
        btnActividad = new Button();
        btnHistorial = new Button();
        btnCerrarSesion = new Button();
        pnlSidebar.SuspendLayout();
        SuspendLayout();

        // pnlSidebar
        pnlSidebar.BackColor = Color.FromArgb(27, 42, 74);
        pnlSidebar.Dock = DockStyle.Left;
        pnlSidebar.Width = 220;
        pnlSidebar.Controls.Add(btnCerrarSesion);
        pnlSidebar.Controls.Add(btnHistorial);
        pnlSidebar.Controls.Add(btnActividad);
        pnlSidebar.Controls.Add(btnAgenda);
        pnlSidebar.Controls.Add(btnTurnos);
        pnlSidebar.Controls.Add(btnMedicos);
        pnlSidebar.Controls.Add(btnPacientes);
        pnlSidebar.Controls.Add(lblUsuarioActivo);
        pnlSidebar.Controls.Add(lblAppName);

        // lblAppName
        lblAppName.AutoSize = true;
        lblAppName.MaximumSize = new Size(190, 0);
        lblAppName.Text = "Gestión de Consultorio";
        lblAppName.ForeColor = Color.White;
        lblAppName.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblAppName.Location = new Point(15, 25);

        // lblUsuarioActivo
        lblUsuarioActivo.AutoSize = true;
        lblUsuarioActivo.MaximumSize = new Size(190, 0);
        lblUsuarioActivo.Text = "";
        lblUsuarioActivo.ForeColor = Color.FromArgb(180, 195, 220);
        lblUsuarioActivo.Font = new Font("Segoe UI", 9F);
        lblUsuarioActivo.Location = new Point(15, 95);

        ConfigurarBotonSidebar(btnPacientes, "Pacientes", 150);
        ConfigurarBotonSidebar(btnMedicos, "Médicos", 200);
        ConfigurarBotonSidebar(btnTurnos, "Turnos", 250);
        ConfigurarBotonSidebar(btnAgenda, "Mi Agenda", 150);
        ConfigurarBotonSidebar(btnActividad, "Registrar Actividad", 200);
        ConfigurarBotonSidebar(btnHistorial, "Historial Clínico", 250);
        ConfigurarBotonSidebar(btnCerrarSesion, "Cerrar sesión", 500);

        // pnlContenido
        pnlContenido.BackColor = Color.FromArgb(245, 246, 250);
        pnlContenido.Dock = DockStyle.Fill;

        // FormMenuPrincipal
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 650);
        Controls.Add(pnlContenido);
        Controls.Add(pnlSidebar);
        Text = "Sistema de Gestión de Consultorio";
        WindowState = FormWindowState.Maximized;
        pnlSidebar.ResumeLayout(false);
        ResumeLayout(false);
    }

    private void ConfigurarBotonSidebar(Button boton, string texto, int posicionY)
    {
        boton.Text = texto;
        boton.ForeColor = Color.White;
        boton.BackColor = Color.FromArgb(27, 42, 74);
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderSize = 0;
        boton.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 222);
        boton.Font = new Font("Segoe UI", 10F);
        boton.TextAlign = ContentAlignment.MiddleLeft;
        boton.Padding = new Padding(20, 0, 0, 0);
        boton.Location = new Point(0, posicionY);
        boton.Size = new Size(220, 45);
    }
}
