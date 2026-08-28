using SGC.Entidades;

namespace SGC.UI;

public partial class FormMenuPrincipal : Form
{
    private readonly Usuario _usuarioActivo;

    public FormMenuPrincipal(Usuario usuarioActivo)
    {
        InitializeComponent();
        _usuarioActivo = usuarioActivo;

        lblUsuarioActivo.Text = $"{_usuarioActivo.NombreUsuario}\n({_usuarioActivo.Rol})";

        ConfigurarBotonesPorRol();

        btnPacientes.Click += (s, e) => new FormPacientes().ShowDialog();
        btnMedicos.Click += (s, e) => AbrirPantallaPendiente("ABM de Médicos");
        btnTurnos.Click += (s, e) => new FormTurnos().ShowDialog();
        btnAgenda.Click += (s, e) => AbrirPantallaPendiente("Mi Agenda");
        btnActividad.Click += (s, e) => AbrirPantallaPendiente("Registrar Actividad Médica");
        btnHistorial.Click += (s, e) => AbrirPantallaPendiente("Historial Clínico");
        btnCerrarSesion.Click += BtnCerrarSesion_Click;
    }

    private void ConfigurarBotonesPorRol()
    {
        // Cada rol solo ve las opciones que le corresponden según el ERS.
        btnPacientes.Visible = _usuarioActivo.Rol == RolUsuario.Recepcionista;
        btnTurnos.Visible = _usuarioActivo.Rol == RolUsuario.Recepcionista;

        btnMedicos.Visible = _usuarioActivo.Rol == RolUsuario.Administrador;

        btnAgenda.Visible = _usuarioActivo.Rol == RolUsuario.Medico;
        btnActividad.Visible = _usuarioActivo.Rol == RolUsuario.Medico;
        btnHistorial.Visible = _usuarioActivo.Rol == RolUsuario.Medico;
    }

    private void AbrirPantallaPendiente(string nombrePantalla)
    {
        MessageBox.Show($"La pantalla \"{nombrePantalla}\" todavía no está implementada.",
            "Próximamente", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnCerrarSesion_Click(object? sender, EventArgs e)
    {
        var login = new FormLogin();
        login.Show();
        Close();
    }

    private void FormMenuPrincipal_Load(object sender, EventArgs e)
    {

    }
}
