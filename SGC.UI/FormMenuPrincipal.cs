using SGC.Entidades;

namespace SGC.UI;

public partial class FormMenuPrincipal : Form
{
    private readonly Usuario _usuarioActivo;
    private Form? formularioActivo = null;
    private Button? _botonActivo = null;

    private readonly Color ColorSidebarNormal = Color.FromArgb(27, 42, 74);
    private readonly Color ColorSidebarActivo = Color.FromArgb(46, 134, 222);

    public FormMenuPrincipal(Usuario usuarioActivo)
    {
        InitializeComponent();
        _usuarioActivo = usuarioActivo;

        lblUsuarioActivo.Text = $"{_usuarioActivo.NombreUsuario}\n({_usuarioActivo.Rol})";

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

        // Modulo Medico
        btnAgenda.Visible = _usuarioActivo.Rol == RolUsuario.Medico;
        btnActividad.Visible = _usuarioActivo.Rol == RolUsuario.Medico;
        btnHistorial.Visible = _usuarioActivo.Rol == RolUsuario.Medico;
    }

    private void AsignarEventosNavegacion()
    {
        btnPacientes.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormPacientes()); };
        btnTurnos.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormTurnos()); };
        btnMedicos.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormMedicos()); };

        btnAgenda.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormAgendaMedico(_usuarioActivo, AbrirFormularioHijo)); };
        btnActividad.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormRegistrarActividad(null, _usuarioActivo, AbrirFormularioHijo)); };
        btnHistorial.Click += (s, e) => { ResaltarBoton((Button)s!); AbrirFormularioHijo(new FormHistorialClinico(_usuarioActivo, null, AbrirFormularioHijo)); };

        btnCerrarSesion.Click += BtnCerrarSesion_Click;
    }

    private void CargarPantallaInicialPorDefecto()
    {
        switch (_usuarioActivo.Rol)
        {
            case RolUsuario.Medico:
                ResaltarBoton(btnAgenda);
                AbrirFormularioHijo(new FormAgendaMedico(_usuarioActivo, AbrirFormularioHijo));
                break;
            case RolUsuario.Recepcionista:
                ResaltarBoton(btnTurnos);
                AbrirFormularioHijo(new FormTurnos());
                break;
            case RolUsuario.Administrador:
                ResaltarBoton(btnMedicos);
                AbrirFormularioHijo(new FormMedicos());
                break;
        }
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