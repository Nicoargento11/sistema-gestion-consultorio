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

    // Estructura estatica del menu: la barra lateral (cabecera con la marca y el
    // usuario, botones de seccion y "Cerrar sesión" fijo abajo) usa solo controles
    // estandar. Los iconos, los tamanos medidos y los estados activo/hover se
    // resuelven en FormMenuPrincipal.cs.
    private void InitializeComponent()
    {
        pnlContenido = new Panel();
        pnlSidebar = new Panel();
        flowMenu = new FlowLayoutPanel();
        btnInicio = new Button();
        btnPacientes = new Button();
        btnTurnos = new Button();
        btnAgenda = new Button();
        btnActividad = new Button();
        btnHistorial = new Button();
        btnMedicos = new Button();
        btnHorarios = new Button();
        btnUsuarios = new Button();
        btnObrasSociales = new Button();
        btnExcepcionesAgenda = new Button();
        btnReportes = new Button();
        pnlPie = new Panel();
        btnCerrarSesion = new Button();
        pnlSeparador = new Panel();
        tlpCabecera = new TableLayoutPanel();
        lblIconoApp = new Label();
        lblAppName = new Label();
        lblUsuarioActivo = new Label();
        lblRolUsuario = new Label();
        pnlSidebar.SuspendLayout();
        flowMenu.SuspendLayout();
        pnlPie.SuspendLayout();
        tlpCabecera.SuspendLayout();
        SuspendLayout();
        //
        // pnlSidebar
        //
        pnlSidebar.BackColor = Color.FromArgb(27, 42, 74);
        pnlSidebar.Controls.Add(flowMenu);
        pnlSidebar.Controls.Add(pnlPie);
        pnlSidebar.Controls.Add(tlpCabecera);
        pnlSidebar.Dock = DockStyle.Left;
        pnlSidebar.Name = "pnlSidebar";
        pnlSidebar.Size = new Size(240, 520);
        pnlSidebar.TabIndex = 1;
        //
        // tlpCabecera
        //
        tlpCabecera.AutoSize = true;
        tlpCabecera.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpCabecera.ColumnCount = 2;
        tlpCabecera.ColumnStyles.Add(new ColumnStyle());
        tlpCabecera.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpCabecera.Controls.Add(lblIconoApp, 0, 0);
        tlpCabecera.Controls.Add(lblAppName, 1, 0);
        tlpCabecera.Controls.Add(lblUsuarioActivo, 0, 1);
        tlpCabecera.Controls.Add(lblRolUsuario, 0, 2);
        tlpCabecera.Dock = DockStyle.Top;
        tlpCabecera.Name = "tlpCabecera";
        tlpCabecera.Padding = new Padding(8, 10, 4, 8);
        tlpCabecera.RowCount = 3;
        tlpCabecera.RowStyles.Add(new RowStyle());
        tlpCabecera.RowStyles.Add(new RowStyle());
        tlpCabecera.RowStyles.Add(new RowStyle());
        tlpCabecera.Size = new Size(240, 120);
        tlpCabecera.TabIndex = 0;
        //
        // lblIconoApp
        //
        lblIconoApp.Anchor = AnchorStyles.Left;
        lblIconoApp.AutoSize = true;
        lblIconoApp.Font = new Font("Segoe MDL2 Assets", 11F);
        lblIconoApp.ForeColor = Color.FromArgb(46, 134, 222);
        lblIconoApp.Margin = new Padding(0, 0, 4, 0);
        lblIconoApp.Name = "lblIconoApp";
        lblIconoApp.TabIndex = 0;
        //
        // lblAppName
        //
        lblAppName.Anchor = AnchorStyles.Left;
        lblAppName.AutoSize = true;
        lblAppName.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblAppName.ForeColor = Color.White;
        lblAppName.Margin = new Padding(0);
        lblAppName.Name = "lblAppName";
        lblAppName.TabIndex = 1;
        lblAppName.Text = "Gestión de\nConsultorio";
        //
        // lblUsuarioActivo
        //
        lblUsuarioActivo.AutoEllipsis = true;
        tlpCabecera.SetColumnSpan(lblUsuarioActivo, 2);
        lblUsuarioActivo.Dock = DockStyle.Fill;
        lblUsuarioActivo.Font = new Font("Segoe UI", 9F);
        lblUsuarioActivo.ForeColor = Color.White;
        lblUsuarioActivo.Margin = new Padding(0, 8, 0, 2);
        lblUsuarioActivo.Name = "lblUsuarioActivo";
        lblUsuarioActivo.Size = new Size(0, 24);
        lblUsuarioActivo.TabIndex = 2;
        lblUsuarioActivo.TextAlign = ContentAlignment.MiddleLeft;
        //
        // lblRolUsuario
        //
        lblRolUsuario.Anchor = AnchorStyles.Left;
        lblRolUsuario.AutoSize = true;
        lblRolUsuario.BackColor = Color.FromArgb(40, 70, 118);
        tlpCabecera.SetColumnSpan(lblRolUsuario, 2);
        lblRolUsuario.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblRolUsuario.ForeColor = Color.FromArgb(170, 205, 245);
        lblRolUsuario.Margin = new Padding(0);
        lblRolUsuario.Name = "lblRolUsuario";
        lblRolUsuario.Padding = new Padding(6, 1, 6, 1);
        lblRolUsuario.TabIndex = 3;
        //
        // pnlPie
        //
        pnlPie.Controls.Add(btnCerrarSesion);
        pnlPie.Controls.Add(pnlSeparador);
        pnlPie.Dock = DockStyle.Bottom;
        pnlPie.Name = "pnlPie";
        pnlPie.Padding = new Padding(6, 0, 6, 6);
        pnlPie.Size = new Size(240, 64);
        pnlPie.TabIndex = 2;
        //
        // pnlSeparador
        //
        pnlSeparador.BackColor = Color.FromArgb(52, 72, 110);
        pnlSeparador.Dock = DockStyle.Top;
        pnlSeparador.Name = "pnlSeparador";
        pnlSeparador.Size = new Size(0, 1);
        pnlSeparador.TabIndex = 1;
        //
        // btnCerrarSesion
        //
        btnCerrarSesion.BackColor = Color.FromArgb(27, 42, 74);
        btnCerrarSesion.Dock = DockStyle.Bottom;
        btnCerrarSesion.FlatAppearance.BorderSize = 0;
        btnCerrarSesion.FlatAppearance.MouseOverBackColor = Color.FromArgb(105, 45, 58);
        btnCerrarSesion.FlatStyle = FlatStyle.Flat;
        btnCerrarSesion.Font = new Font("Segoe UI", 10F);
        btnCerrarSesion.ForeColor = Color.FromArgb(255, 205, 198);
        btnCerrarSesion.Name = "btnCerrarSesion";
        btnCerrarSesion.Size = new Size(224, 40);
        btnCerrarSesion.TabIndex = 0;
        btnCerrarSesion.Text = "Cerrar sesión";
        btnCerrarSesion.TextAlign = ContentAlignment.MiddleLeft;
        btnCerrarSesion.UseVisualStyleBackColor = false;
        //
        // flowMenu
        //
        flowMenu.AutoScroll = true;
        flowMenu.Controls.Add(btnInicio);
        flowMenu.Controls.Add(btnPacientes);
        flowMenu.Controls.Add(btnTurnos);
        flowMenu.Controls.Add(btnAgenda);
        flowMenu.Controls.Add(btnActividad);
        flowMenu.Controls.Add(btnHistorial);
        flowMenu.Controls.Add(btnMedicos);
        flowMenu.Controls.Add(btnHorarios);
        flowMenu.Controls.Add(btnUsuarios);
        flowMenu.Controls.Add(btnObrasSociales);
        flowMenu.Controls.Add(btnExcepcionesAgenda);
        flowMenu.Controls.Add(btnReportes);
        flowMenu.Dock = DockStyle.Fill;
        flowMenu.FlowDirection = FlowDirection.TopDown;
        flowMenu.Name = "flowMenu";
        flowMenu.Padding = new Padding(6, 4, 6, 4);
        flowMenu.Size = new Size(240, 330);
        flowMenu.TabIndex = 1;
        flowMenu.WrapContents = false;
        //
        // btnInicio
        //
        btnInicio.BackColor = Color.FromArgb(27, 42, 74);
        btnInicio.FlatAppearance.BorderSize = 0;
        btnInicio.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 62, 105);
        btnInicio.FlatStyle = FlatStyle.Flat;
        btnInicio.Font = new Font("Segoe UI", 10F);
        btnInicio.ForeColor = Color.White;
        btnInicio.Margin = new Padding(0, 0, 0, 2);
        btnInicio.Name = "btnInicio";
        btnInicio.Size = new Size(224, 40);
        btnInicio.TabIndex = 0;
        btnInicio.Text = "Inicio";
        btnInicio.TextAlign = ContentAlignment.MiddleLeft;
        btnInicio.UseVisualStyleBackColor = false;
        //
        // btnPacientes
        //
        btnPacientes.BackColor = Color.FromArgb(27, 42, 74);
        btnPacientes.FlatAppearance.BorderSize = 0;
        btnPacientes.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 62, 105);
        btnPacientes.FlatStyle = FlatStyle.Flat;
        btnPacientes.Font = new Font("Segoe UI", 10F);
        btnPacientes.ForeColor = Color.White;
        btnPacientes.Margin = new Padding(0, 0, 0, 2);
        btnPacientes.Name = "btnPacientes";
        btnPacientes.Size = new Size(224, 40);
        btnPacientes.TabIndex = 1;
        btnPacientes.Text = "Pacientes";
        btnPacientes.TextAlign = ContentAlignment.MiddleLeft;
        btnPacientes.UseVisualStyleBackColor = false;
        //
        // btnTurnos
        //
        btnTurnos.BackColor = Color.FromArgb(27, 42, 74);
        btnTurnos.FlatAppearance.BorderSize = 0;
        btnTurnos.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 62, 105);
        btnTurnos.FlatStyle = FlatStyle.Flat;
        btnTurnos.Font = new Font("Segoe UI", 10F);
        btnTurnos.ForeColor = Color.White;
        btnTurnos.Margin = new Padding(0, 0, 0, 2);
        btnTurnos.Name = "btnTurnos";
        btnTurnos.Size = new Size(224, 40);
        btnTurnos.TabIndex = 2;
        btnTurnos.Text = "Turnos";
        btnTurnos.TextAlign = ContentAlignment.MiddleLeft;
        btnTurnos.UseVisualStyleBackColor = false;
        //
        // btnAgenda
        //
        btnAgenda.BackColor = Color.FromArgb(27, 42, 74);
        btnAgenda.FlatAppearance.BorderSize = 0;
        btnAgenda.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 62, 105);
        btnAgenda.FlatStyle = FlatStyle.Flat;
        btnAgenda.Font = new Font("Segoe UI", 10F);
        btnAgenda.ForeColor = Color.White;
        btnAgenda.Margin = new Padding(0, 0, 0, 2);
        btnAgenda.Name = "btnAgenda";
        btnAgenda.Size = new Size(224, 40);
        btnAgenda.TabIndex = 3;
        btnAgenda.Text = "Mi Agenda";
        btnAgenda.TextAlign = ContentAlignment.MiddleLeft;
        btnAgenda.UseVisualStyleBackColor = false;
        //
        // btnActividad
        //
        btnActividad.BackColor = Color.FromArgb(27, 42, 74);
        btnActividad.FlatAppearance.BorderSize = 0;
        btnActividad.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 62, 105);
        btnActividad.FlatStyle = FlatStyle.Flat;
        btnActividad.Font = new Font("Segoe UI", 10F);
        btnActividad.ForeColor = Color.White;
        btnActividad.Margin = new Padding(0, 0, 0, 2);
        btnActividad.Name = "btnActividad";
        btnActividad.Size = new Size(224, 40);
        btnActividad.TabIndex = 4;
        btnActividad.Text = "Cargar atención";
        btnActividad.TextAlign = ContentAlignment.MiddleLeft;
        btnActividad.UseVisualStyleBackColor = false;
        //
        // btnHistorial
        //
        btnHistorial.BackColor = Color.FromArgb(27, 42, 74);
        btnHistorial.FlatAppearance.BorderSize = 0;
        btnHistorial.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 62, 105);
        btnHistorial.FlatStyle = FlatStyle.Flat;
        btnHistorial.Font = new Font("Segoe UI", 10F);
        btnHistorial.ForeColor = Color.White;
        btnHistorial.Margin = new Padding(0, 0, 0, 2);
        btnHistorial.Name = "btnHistorial";
        btnHistorial.Size = new Size(224, 40);
        btnHistorial.TabIndex = 5;
        btnHistorial.Text = "Consultar historial";
        btnHistorial.TextAlign = ContentAlignment.MiddleLeft;
        btnHistorial.UseVisualStyleBackColor = false;
        //
        // btnMedicos
        //
        btnMedicos.BackColor = Color.FromArgb(27, 42, 74);
        btnMedicos.FlatAppearance.BorderSize = 0;
        btnMedicos.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 62, 105);
        btnMedicos.FlatStyle = FlatStyle.Flat;
        btnMedicos.Font = new Font("Segoe UI", 10F);
        btnMedicos.ForeColor = Color.White;
        btnMedicos.Margin = new Padding(0, 0, 0, 2);
        btnMedicos.Name = "btnMedicos";
        btnMedicos.Size = new Size(224, 40);
        btnMedicos.TabIndex = 6;
        btnMedicos.Text = "Médicos";
        btnMedicos.TextAlign = ContentAlignment.MiddleLeft;
        btnMedicos.UseVisualStyleBackColor = false;
        //
        // btnHorarios
        //
        btnHorarios.BackColor = Color.FromArgb(27, 42, 74);
        btnHorarios.FlatAppearance.BorderSize = 0;
        btnHorarios.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 62, 105);
        btnHorarios.FlatStyle = FlatStyle.Flat;
        btnHorarios.Font = new Font("Segoe UI", 10F);
        btnHorarios.ForeColor = Color.White;
        btnHorarios.Margin = new Padding(0, 0, 0, 2);
        btnHorarios.Name = "btnHorarios";
        btnHorarios.Size = new Size(224, 40);
        btnHorarios.TabIndex = 7;
        btnHorarios.Text = "Horarios";
        btnHorarios.TextAlign = ContentAlignment.MiddleLeft;
        btnHorarios.UseVisualStyleBackColor = false;
        //
        // btnUsuarios
        //
        btnUsuarios.BackColor = Color.FromArgb(27, 42, 74);
        btnUsuarios.FlatAppearance.BorderSize = 0;
        btnUsuarios.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 62, 105);
        btnUsuarios.FlatStyle = FlatStyle.Flat;
        btnUsuarios.Font = new Font("Segoe UI", 10F);
        btnUsuarios.ForeColor = Color.White;
        btnUsuarios.Margin = new Padding(0, 0, 0, 2);
        btnUsuarios.Name = "btnUsuarios";
        btnUsuarios.Size = new Size(224, 40);
        btnUsuarios.TabIndex = 8;
        btnUsuarios.Text = "Usuarios";
        btnUsuarios.TextAlign = ContentAlignment.MiddleLeft;
        btnUsuarios.UseVisualStyleBackColor = false;
        //
        // btnObrasSociales
        //
        btnObrasSociales.BackColor = Color.FromArgb(27, 42, 74);
        btnObrasSociales.FlatAppearance.BorderSize = 0;
        btnObrasSociales.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 62, 105);
        btnObrasSociales.FlatStyle = FlatStyle.Flat;
        btnObrasSociales.Font = new Font("Segoe UI", 10F);
        btnObrasSociales.ForeColor = Color.White;
        btnObrasSociales.Margin = new Padding(0, 0, 0, 2);
        btnObrasSociales.Name = "btnObrasSociales";
        btnObrasSociales.Size = new Size(224, 40);
        btnObrasSociales.TabIndex = 9;
        btnObrasSociales.Text = "Obras Sociales";
        btnObrasSociales.TextAlign = ContentAlignment.MiddleLeft;
        btnObrasSociales.UseVisualStyleBackColor = false;
        //
        // btnExcepcionesAgenda
        //
        btnExcepcionesAgenda.BackColor = Color.FromArgb(27, 42, 74);
        btnExcepcionesAgenda.FlatAppearance.BorderSize = 0;
        btnExcepcionesAgenda.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 62, 105);
        btnExcepcionesAgenda.FlatStyle = FlatStyle.Flat;
        btnExcepcionesAgenda.Font = new Font("Segoe UI", 10F);
        btnExcepcionesAgenda.ForeColor = Color.White;
        btnExcepcionesAgenda.Margin = new Padding(0, 0, 0, 2);
        btnExcepcionesAgenda.Name = "btnExcepcionesAgenda";
        btnExcepcionesAgenda.Size = new Size(224, 40);
        btnExcepcionesAgenda.TabIndex = 10;
        btnExcepcionesAgenda.Text = "Vacaciones/Ausencias";
        btnExcepcionesAgenda.TextAlign = ContentAlignment.MiddleLeft;
        btnExcepcionesAgenda.UseVisualStyleBackColor = false;
        //
        // btnReportes
        //
        btnReportes.BackColor = Color.FromArgb(27, 42, 74);
        btnReportes.FlatAppearance.BorderSize = 0;
        btnReportes.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 62, 105);
        btnReportes.FlatStyle = FlatStyle.Flat;
        btnReportes.Font = new Font("Segoe UI", 10F);
        btnReportes.ForeColor = Color.White;
        btnReportes.Margin = new Padding(0, 0, 0, 2);
        btnReportes.Name = "btnReportes";
        btnReportes.Size = new Size(224, 40);
        btnReportes.TabIndex = 11;
        btnReportes.Text = "Reportes";
        btnReportes.TextAlign = ContentAlignment.MiddleLeft;
        btnReportes.UseVisualStyleBackColor = false;
        //
        // pnlContenido
        //
        pnlContenido.BackColor = Color.FromArgb(245, 246, 250);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Margin = new Padding(2);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(640, 520);
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
        flowMenu.ResumeLayout(false);
        pnlPie.ResumeLayout(false);
        tlpCabecera.ResumeLayout(false);
        tlpCabecera.PerformLayout();
        ResumeLayout(false);
    }

    private Panel pnlContenido; // panel grande a la derecha donde se abre cada pantalla
    private Panel pnlSidebar;
    private FlowLayoutPanel flowMenu;
    private Panel pnlPie;
    private Panel pnlSeparador;
    private TableLayoutPanel tlpCabecera;
    private Label lblIconoApp;
    private Label lblAppName;
    private Label lblUsuarioActivo;
    private Label lblRolUsuario;

    private Button btnInicio;
    private Button btnPacientes;
    private Button btnTurnos;
    private Button btnAgenda;
    private Button btnActividad;
    private Button btnHistorial;
    private Button btnMedicos;
    private Button btnHorarios;
    private Button btnUsuarios;
    private Button btnObrasSociales;
    private Button btnExcepcionesAgenda;
    private Button btnReportes;
    private Button btnCerrarSesion;
}
