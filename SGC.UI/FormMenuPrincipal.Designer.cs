namespace SGC.UI;

partial class FormMenuPrincipal
{
    // Variable interna que usa Windows Forms para manejar los recursos visuales
    private System.ComponentModel.IContainer components = null;

    // Método que limpia la memoria cuando cerrás la ventana
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    
    // DECLARACIÓN DE LOS CONTROLES (Lo que se va a ver en pantalla)

    // Usamos FlowLayoutPanel para el menú lateral. Esto es la clave para que 
    // los botones se apilen solos y no queden espacios en blanco.
    private FlowLayoutPanel pnlSidebar;

    private Panel pnlContenido; // El panel grande a la derecha donde inyectaremos la agenda
    private Label lblAppName;
    private Label lblUsuarioActivo;

    // Todos los botones de nuestro menú
    private Button btnPacientes;
    private Button btnMedicos;
    private Button btnTurnos;
    private Button btnAgenda;
    private Button btnActividad;
    private Button btnHistorial;
    private Button btnCerrarSesion;


    // 2. EL MÉTODO QUE "DIBUJA" LA PANTALLA (InitializeComponent)
    private void InitializeComponent()
    {
        pnlSidebar = new FlowLayoutPanel();
        lblAppName = new Label();
        lblUsuarioActivo = new Label();
        btnPacientes = new Button();
        btnAgenda = new Button();
        btnActividad = new Button();
        btnMedicos = new Button();
        btnTurnos = new Button();
        btnHistorial = new Button();
        btnCerrarSesion = new Button();
        pnlContenido = new Panel();
        pnlSidebar.SuspendLayout();
        SuspendLayout();
        // 
        // pnlSidebar
        // 
        pnlSidebar.BackColor = Color.FromArgb(27, 42, 74);
        pnlSidebar.Controls.Add(lblAppName);
        pnlSidebar.Controls.Add(lblUsuarioActivo);
        pnlSidebar.Controls.Add(btnPacientes);
        pnlSidebar.Controls.Add(btnAgenda);
        pnlSidebar.Controls.Add(btnActividad);
        pnlSidebar.Controls.Add(btnMedicos);
        pnlSidebar.Controls.Add(btnTurnos);
        pnlSidebar.Controls.Add(btnHistorial);
        pnlSidebar.Controls.Add(btnCerrarSesion);
        pnlSidebar.Dock = DockStyle.Left;
        pnlSidebar.FlowDirection = FlowDirection.TopDown;
        pnlSidebar.Location = new Point(0, 0);
        pnlSidebar.Margin = new Padding(2);
        pnlSidebar.Name = "pnlSidebar";
        pnlSidebar.Size = new Size(176, 520);
        pnlSidebar.TabIndex = 1;
        pnlSidebar.WrapContents = false;
        // 
        // lblAppName
        // 
        lblAppName.AutoSize = true;
        lblAppName.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblAppName.ForeColor = Color.White;
        lblAppName.Location = new Point(2, 0);
        lblAppName.Margin = new Padding(2, 0, 2, 0);
        lblAppName.MaximumSize = new Size(152, 0);
        lblAppName.Name = "lblAppName";
        lblAppName.Size = new Size(148, 64);
        lblAppName.TabIndex = 8;
        lblAppName.Text = "Gestión de Consultorio";
        // 
        // lblUsuarioActivo
        // 
        lblUsuarioActivo.AutoSize = true;
        lblUsuarioActivo.Font = new Font("Segoe UI", 9F);
        lblUsuarioActivo.ForeColor = Color.FromArgb(180, 195, 220);
        lblUsuarioActivo.Location = new Point(2, 64);
        lblUsuarioActivo.Margin = new Padding(2, 0, 2, 0);
        lblUsuarioActivo.MaximumSize = new Size(152, 0);
        lblUsuarioActivo.Name = "lblUsuarioActivo";
        lblUsuarioActivo.Size = new Size(0, 20);
        lblUsuarioActivo.TabIndex = 7;
        // 
        // btnPacientes
        // 
        btnPacientes.BackColor = Color.FromArgb(27, 42, 74);
        btnPacientes.FlatAppearance.BorderSize = 0;
        btnPacientes.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 222);
        btnPacientes.FlatStyle = FlatStyle.Flat;
        btnPacientes.Font = new Font("Segoe UI", 10F);
        btnPacientes.ForeColor = Color.White;
        btnPacientes.Location = new Point(2, 86);
        btnPacientes.Margin = new Padding(2);
        btnPacientes.Name = "btnPacientes";
        btnPacientes.Padding = new Padding(16, 0, 0, 0);
        btnPacientes.Size = new Size(176, 36);
        btnPacientes.TabIndex = 6;
        btnPacientes.Text = "Pacientes";
        btnPacientes.TextAlign = ContentAlignment.MiddleLeft;
        btnPacientes.UseVisualStyleBackColor = false;
        // 
        // btnAgenda
        // 
        btnAgenda.BackColor = Color.FromArgb(27, 42, 74);
        btnAgenda.FlatAppearance.BorderSize = 0;
        btnAgenda.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 222);
        btnAgenda.FlatStyle = FlatStyle.Flat;
        btnAgenda.Font = new Font("Segoe UI", 10F);
        btnAgenda.ForeColor = Color.White;
        btnAgenda.Location = new Point(2, 126);
        btnAgenda.Margin = new Padding(2);
        btnAgenda.Name = "btnAgenda";
        btnAgenda.Padding = new Padding(16, 0, 0, 0);
        btnAgenda.Size = new Size(176, 36);
        btnAgenda.TabIndex = 3;
        btnAgenda.Text = "Mi Agenda";
        btnAgenda.TextAlign = ContentAlignment.MiddleLeft;
        btnAgenda.UseVisualStyleBackColor = false;
        // 
        // btnActividad
        // 
        btnActividad.BackColor = Color.FromArgb(27, 42, 74);
        btnActividad.FlatAppearance.BorderSize = 0;
        btnActividad.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 222);
        btnActividad.FlatStyle = FlatStyle.Flat;
        btnActividad.Font = new Font("Segoe UI", 10F);
        btnActividad.ForeColor = Color.White;
        btnActividad.Location = new Point(2, 166);
        btnActividad.Margin = new Padding(2);
        btnActividad.Name = "btnActividad";
        btnActividad.Padding = new Padding(16, 0, 0, 0);
        btnActividad.Size = new Size(176, 36);
        btnActividad.TabIndex = 2;
        btnActividad.Text = "Registrar Actividad";
        btnActividad.TextAlign = ContentAlignment.MiddleLeft;
        btnActividad.UseVisualStyleBackColor = false;
        // 
        // btnMedicos
        // 
        btnMedicos.BackColor = Color.FromArgb(27, 42, 74);
        btnMedicos.FlatAppearance.BorderSize = 0;
        btnMedicos.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 222);
        btnMedicos.FlatStyle = FlatStyle.Flat;
        btnMedicos.Font = new Font("Segoe UI", 10F);
        btnMedicos.ForeColor = Color.White;
        btnMedicos.Location = new Point(2, 206);
        btnMedicos.Margin = new Padding(2);
        btnMedicos.Name = "btnMedicos";
        btnMedicos.Padding = new Padding(16, 0, 0, 0);
        btnMedicos.Size = new Size(176, 36);
        btnMedicos.TabIndex = 5;
        btnMedicos.Text = "Médicos";
        btnMedicos.TextAlign = ContentAlignment.MiddleLeft;
        btnMedicos.UseVisualStyleBackColor = false;
        // 
        // btnTurnos
        // 
        btnTurnos.BackColor = Color.FromArgb(27, 42, 74);
        btnTurnos.FlatAppearance.BorderSize = 0;
        btnTurnos.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 222);
        btnTurnos.FlatStyle = FlatStyle.Flat;
        btnTurnos.Font = new Font("Segoe UI", 10F);
        btnTurnos.ForeColor = Color.White;
        btnTurnos.Location = new Point(2, 246);
        btnTurnos.Margin = new Padding(2);
        btnTurnos.Name = "btnTurnos";
        btnTurnos.Padding = new Padding(16, 0, 0, 0);
        btnTurnos.Size = new Size(176, 36);
        btnTurnos.TabIndex = 4;
        btnTurnos.Text = "Turnos";
        btnTurnos.TextAlign = ContentAlignment.MiddleLeft;
        btnTurnos.UseVisualStyleBackColor = false;
        // 
        // btnHistorial
        // 
        btnHistorial.BackColor = Color.FromArgb(27, 42, 74);
        btnHistorial.FlatAppearance.BorderSize = 0;
        btnHistorial.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 222);
        btnHistorial.FlatStyle = FlatStyle.Flat;
        btnHistorial.Font = new Font("Segoe UI", 10F);
        btnHistorial.ForeColor = Color.White;
        btnHistorial.Location = new Point(2, 286);
        btnHistorial.Margin = new Padding(2);
        btnHistorial.Name = "btnHistorial";
        btnHistorial.Padding = new Padding(16, 0, 0, 0);
        btnHistorial.Size = new Size(176, 36);
        btnHistorial.TabIndex = 1;
        btnHistorial.Text = "Historial Clínico";
        btnHistorial.TextAlign = ContentAlignment.MiddleLeft;
        btnHistorial.UseVisualStyleBackColor = false;
        // 
        // btnCerrarSesion
        // 
        btnCerrarSesion.BackColor = Color.FromArgb(27, 42, 74);
        btnCerrarSesion.FlatAppearance.BorderSize = 0;
        btnCerrarSesion.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 222);
        btnCerrarSesion.FlatStyle = FlatStyle.Flat;
        btnCerrarSesion.Font = new Font("Segoe UI", 10F);
        btnCerrarSesion.ForeColor = Color.White;
        btnCerrarSesion.Location = new Point(2, 326);
        btnCerrarSesion.Margin = new Padding(2);
        btnCerrarSesion.Name = "btnCerrarSesion";
        btnCerrarSesion.Padding = new Padding(16, 0, 0, 0);
        btnCerrarSesion.Size = new Size(176, 36);
        btnCerrarSesion.TabIndex = 0;
        btnCerrarSesion.Text = "Cerrar sesión";
        btnCerrarSesion.TextAlign = ContentAlignment.MiddleLeft;
        btnCerrarSesion.UseVisualStyleBackColor = false;
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = Color.FromArgb(245, 246, 250);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(176, 0);
        pnlContenido.Margin = new Padding(2);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(704, 520);
        pnlContenido.TabIndex = 0;
        pnlContenido.Paint += pnlContenido_Paint;
        // 
        // FormMenuPrincipal
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(880, 520);
        Controls.Add(pnlContenido);
        Controls.Add(pnlSidebar);
        Margin = new Padding(2);
        Name = "FormMenuPrincipal";
        Text = "Sistema de Gestión de Consultorio";
        WindowState = FormWindowState.Maximized;
        pnlSidebar.ResumeLayout(false);
        pnlSidebar.PerformLayout();
        ResumeLayout(false);
    }
}