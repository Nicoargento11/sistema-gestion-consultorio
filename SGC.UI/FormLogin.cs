using SGC.Entidades;
using SGC.Logica;
namespace SGC.UI;

public partial class FormLogin : Form
{
    private readonly FranjaMensaje _franja;

    public FormLogin()
    {
        InitializeComponent();
        AcceptButton = BtnIngresar;

        lblIconoMarca.Font = IconosUi.Fuente(22F);
        lblIconoMarca.Text = IconosUi.Salud;
        EstiloUi.EstilizarBoton(BtnIngresar, TipoBoton.Primario);

        _franja = new FranjaMensaje(pnlEstado, LblError) { ReacomodarContenedor = true };
        AjustarVentana();
    }

    // La ventana se dimensiona desde el contenido real de la tarjeta (las fuentes
    // escalan con el DPI de Windows, los pixeles fijos no) y deja lugar para un
    // mensaje de hasta dos lineas sin que la tarjeta toque los bordes.
    private void AjustarVentana()
    {
        var tarjeta = pnlMarco.GetPreferredSize(Size.Empty);
        int altoMensaje = 2 * LblError.Font.Height + pnlEstado.Padding.Vertical + EscalaDpi.Px(this, 4);

        ClientSize = new Size(
            tarjeta.Width + 2 * EscalaDpi.Px(this, 48),
            tarjeta.Height + altoMensaje + 2 * EscalaDpi.Px(this, 36));
        MinimumSize = Size;
    }

    private void BtnIngresar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtUsuario.Text) || string.IsNullOrWhiteSpace(TxtContrasenia.Text))
        {
            _franja.Mostrar("Debe completar usuario y clave", TipoMensaje.Error);
            return;
        }

        Usuario? usuario;
        try
        {
            var servicio = new AutenticacionService();
            usuario = servicio.Autenticar(TxtUsuario.Text, TxtContrasenia.Text);
        }
        catch (Exception ex)
        {
            _franja.Mostrar("No se pudo verificar el acceso. Verifique la conexión con la base de datos e intente nuevamente. Detalle: " + ex.Message, TipoMensaje.Error);
            return;
        }

        if (usuario == null)
        {
            _franja.Mostrar("Usuario o contraseña incorrectos", TipoMensaje.Error);
            TxtContrasenia.SelectAll();
            TxtContrasenia.Focus();
            return;
        }

        _franja.Limpiar();
        var menu = new FormMenuPrincipal(usuario);
        menu.Show();
        Hide();
    }
}
