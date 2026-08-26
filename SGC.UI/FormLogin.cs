using SGC.Logica;
namespace SGC.UI;

public partial class FormLogin : Form
{
    public FormLogin()
    {
        InitializeComponent();
    }

    private void FormLogin_Load(object sender, EventArgs e)
    {

    }

    private void BtnIngresar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtUsuario.Text) || string.IsNullOrWhiteSpace(TxtContrasenia.Text))
        {
            LblError.Text = "Debe completar usuario y clave";
            return;
        }
        var servicio = new AutenticacionService();
        var usuario = servicio.Autenticar(TxtUsuario.Text, TxtContrasenia.Text);

        if (usuario == null)
        {
            LblError.Text = "Usuario o contrasenia incorrectos";
            return;
        }

        MessageBox.Show($"Bienvenido, rol: {usuario.Rol}");
    }

    private void label2_Click(object sender, EventArgs e)
    {

    }


}
