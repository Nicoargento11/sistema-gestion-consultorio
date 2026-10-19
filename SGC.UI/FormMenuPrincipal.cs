using SGC.Entidades;

namespace SGC.UI;

public partial class FormMenuPrincipal : Form
{
    private readonly Usuario _usuarioActivo;
    private Form? formularioActivo = null;
    private Button? _botonActivo = null;
    private readonly Button btnInicio = new();

    private readonly Color ColorSidebarNormal = Color.FromArgb(27, 42, 74);
    private readonly Color ColorSidebarActivo = Color.FromArgb(46, 134, 222);

    public FormMenuPrincipal(Usuario usuarioActivo)
    {
        InitializeComponent();
        _usuarioActivo = usuarioActivo;

        lblUsuarioActivo.Text = $"{_usuarioActivo.NombreUsuario}\n({_usuarioActivo.Rol})";

        CrearBotonInicio();
        ConfigurarBotonesPorRol();
        AsignarEventosNavegacion();

        CargarPantallaInicialPorDefecto();
    }

    private void ConfigurarBotonesPorRol()
    {
        // Modulo Recepcionista
        btnPacientes.Visible = _usuarioActivo.Rol == RolUsuario.Recepcionista;
        btnTurnos.Visible = _usuarioActivo.Rol == RolUsuario.Recepcionista;

        // Modulo Administrador
        btnMedicos.Visible = _usuarioActivo.Rol == RolUsuario.Administrador;
        btnHorarios.Visible = _usuarioActivo.Rol == RolUsuario.Administrador;
        btnUsuarios.Visible = _usuarioActivo.Rol == RolUsuario.Administrador;
        btnObrasSociales.Visible = _usuarioActivo.Rol == RolUsuario.Administrador;
        btnExcepcionesAgenda.Visible = _usuarioActivo.Rol == RolUsuario.Administrador;

        // Modulo Medico
        btnAgenda.Visible = _usuarioActivo.Rol == RolUsuario.Medico;
        btnActividad.Visible = _usuarioActivo.Rol == RolUsuario.Medico;
        btnHistorial.Visible = _usuarioActivo.Rol == RolUsuario.Medico;

        // Reportes: visible para los tres roles; FormReportes limita el alcance segun el rol.
        btnReportes.Visible = true;
    }

    private void AsignarEventosNavegacion()
    {
        btnPacientes.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormPacientes(_usuarioActivo)); };
        btnTurnos.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormTurnos(_usuarioActivo)); };
        btnMedicos.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormMedicos()); };
        btnHorarios.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormConfigurarAgenda()); };
        btnUsuarios.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormUsuarios()); };
        btnObrasSociales.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormObrasSociales()); };
        btnExcepcionesAgenda.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormExcepcionesAgenda()); };

        btnAgenda.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormAgendaMedico(_usuarioActivo, AbrirFormularioHijo)); };
        btnActividad.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormRegistrarActividad(null, _usuarioActivo, AbrirFormularioHijo)); };
        btnHistorial.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormHistorialClinico(_usuarioActivo, null, AbrirFormularioHijo)); };

        btnReportes.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormReportes(_usuarioActivo)); };

        btnInicio.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormInicio(_usuarioActivo)); };

        btnCerrarSesion.Click += BtnCerrarSesion_Click;
    }

    // Todos los roles arrancan en Inicio (resumen del dia); desde ahi se
    // navega al resto de los modulos con el menu lateral.
    private void CargarPantallaInicialPorDefecto()
    {
        ResaltarBoton(btnInicio);
        AbrirFormularioHijo(new FormInicio(_usuarioActivo));
    }

    // El boton se crea aca (no en InitializeComponent) para que el
    // disenador no lo borre; copia el estilo de los demas botones del menu
    // y se ubica primero, debajo del nombre de usuario.
    private void CrearBotonInicio()
    {
        btnInicio.Name = "btnInicio";
        btnInicio.Text = "Inicio";
        btnInicio.BackColor = ColorSidebarNormal;
        btnInicio.FlatStyle = FlatStyle.Flat;
        btnInicio.FlatAppearance.BorderSize = 0;
        btnInicio.FlatAppearance.MouseOverBackColor = ColorSidebarActivo;
        btnInicio.Font = new Font("Segoe UI", 10F);
        btnInicio.ForeColor = Color.White;
        btnInicio.Margin = new Padding(2);
        btnInicio.Padding = new Padding(16, 0, 0, 0);
        btnInicio.Size = new Size(176, 36);
        btnInicio.TextAlign = ContentAlignment.MiddleLeft;
        btnInicio.UseVisualStyleBackColor = false;

        pnlSidebar.Controls.Add(btnInicio);
        pnlSidebar.Controls.SetChildIndex(btnInicio, pnlSidebar.Controls.GetChildIndex(lblUsuarioActivo) + 1);
    }

    private void ResaltarBoton(Button boton)
    {
        if (_botonActivo != null)
        {
            _botonActivo.BackColor = ColorSidebarNormal;
        }

        _botonActivo = boton;
        _botonActivo.BackColor = ColorSidebarActivo;
    }

    public void AbrirFormularioHijo(Form formularioHijo)
    {
        if (formularioActivo != null)
        {
            formularioActivo.Close();
        }

        formularioActivo = formularioHijo;
        formularioHijo.TopLevel = false;
        formularioHijo.FormBorderStyle = FormBorderStyle.None;
        formularioHijo.Dock = DockStyle.Fill;

        pnlContenido.Controls.Add(formularioHijo);
        pnlContenido.Tag = formularioHijo;
        formularioHijo.BringToFront();
        formularioHijo.Show();
    }

    private void BtnCerrarSesion_Click(object? sender, EventArgs e)
    {
        var login = new FormLogin();
        login.Show();
        Close();
    }

    private void pnlContenido_Paint(object? sender, PaintEventArgs e) { }
}