using SGC.Entidades;

namespace SGC.UI;

public partial class FormMenuPrincipal : Form
{
    private readonly Usuario _usuarioActivo;
    private Form? formularioActivo = null;
    private Button? _botonActivo = null;

    private readonly Color ColorSidebarNormal = Color.FromArgb(27, 42, 74);
    private readonly Color ColorSidebarHover = Color.FromArgb(40, 62, 105);
    private readonly Color ColorSidebarActivo = Color.FromArgb(46, 134, 222);
    private readonly Color ColorIconoNormal = Color.FromArgb(150, 175, 220);
    private readonly Color ColorIconoSalir = Color.FromArgb(255, 150, 140);

    // Glifo de cada boton del menu (se dibuja en el area izquierda del boton).
    private readonly Dictionary<Button, string> _iconos = new();
    private readonly HashSet<Button> _botonesBajoMouse = new();
    private readonly Font _fuenteIconos = IconosUi.Fuente(11F);
    private int _anchoZonaIcono;
    private int _izquierda;
    private int _derecha;
    private int _inicioTexto;
    private readonly Dictionary<Button, List<string>> _lineas = new();

    public FormMenuPrincipal(Usuario usuarioActivo)
    {
        InitializeComponent();
        _usuarioActivo = usuarioActivo;

        ConfigurarCabecera();
        ConfigurarBotonesDelMenu();
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

    // ---------------------------------------------------------------
    // Barra lateral: cabecera, iconos, medidas y estados
    // ---------------------------------------------------------------

    private static string TextoRol(RolUsuario rol) => rol switch
    {
        RolUsuario.Administrador => "Administrador",
        RolUsuario.Medico => "Médico",
        RolUsuario.Recepcionista => "Recepcionista",
        _ => rol.ToString()
    };

    private void ConfigurarCabecera()
    {
        lblIconoApp.Font = IconosUi.Fuente(11F);
        lblIconoApp.Text = IconosUi.Salud;
        lblUsuarioActivo.Text = _usuarioActivo.NombreUsuario;
        lblRolUsuario.Text = TextoRol(_usuarioActivo.Rol);
    }

    private IEnumerable<Button> BotonesDelMenu()
    {
        yield return btnInicio;
        yield return btnPacientes;
        yield return btnTurnos;
        yield return btnAgenda;
        yield return btnActividad;
        yield return btnHistorial;
        yield return btnMedicos;
        yield return btnHorarios;
        yield return btnUsuarios;
        yield return btnObrasSociales;
        yield return btnExcepcionesAgenda;
        yield return btnReportes;
        yield return btnCerrarSesion;
    }

    private void ConfigurarBotonesDelMenu()
    {
        _iconos[btnInicio] = IconosUi.Inicio;
        _iconos[btnPacientes] = IconosUi.Pacientes;
        _iconos[btnTurnos] = IconosUi.Turnos;
        _iconos[btnAgenda] = IconosUi.Turnos;
        _iconos[btnActividad] = IconosUi.Atencion;
        _iconos[btnHistorial] = IconosUi.Historial;
        _iconos[btnMedicos] = IconosUi.Medicos;
        _iconos[btnHorarios] = IconosUi.Horarios;
        _iconos[btnUsuarios] = IconosUi.Usuarios;
        _iconos[btnObrasSociales] = IconosUi.ObrasSociales;
        _iconos[btnExcepcionesAgenda] = IconosUi.Ausencias;
        _iconos[btnReportes] = IconosUi.Reportes;
        _iconos[btnCerrarSesion] = IconosUi.CerrarSesion;

        // La barra es angosta (para no quitarle ancho a las pantallas): el ancho
        // sale de un valor de diseno, y las etiquetas largas se parten en dos
        // lineas (despues de una barra o de un espacio) en vez de ensancharla.
        // El texto se dibuja en BotonMenu_Paint; Button.Text no cambia.
        var sinLimite = new Size(int.MaxValue, int.MaxValue);

        _anchoZonaIcono = _iconos.Values
            .Select(g => TextRenderer.MeasureText(g, _fuenteIconos, sinLimite, FlagsTexto).Width)
            .Max();

        _izquierda = EscalaDpi.Px(this, 6);
        _derecha = EscalaDpi.Px(this, 4);
        _inicioTexto = _izquierda + _anchoZonaIcono + EscalaDpi.Px(this, 5);
        int relleno = flowMenu.Padding.Horizontal;

        // Minimo imprescindible: que entre la palabra (o tramo) mas larga.
        int tramoMasAncho = BotonesDelMenu()
            .SelectMany(b => Tramos(b.Text).Select(t => MedirTexto(t.texto, b.Font)))
            .Max();
        int anchoBotonMinimo = _inicioTexto + tramoMasAncho + _derecha;

        // La cabecera (marca + usuario) tambien tiene que entrar.
        int anchoCabecera = tlpCabecera.Padding.Horizontal + lblIconoApp.PreferredSize.Width + lblIconoApp.Margin.Horizontal
            + lblAppName.PreferredSize.Width;
        int anchoChip = tlpCabecera.Padding.Horizontal + lblRolUsuario.PreferredSize.Width;

        pnlSidebar.Width = Math.Max(
            Math.Max(EscalaDpi.Px(this, 125), anchoBotonMinimo + relleno),
            Math.Max(anchoCabecera, anchoChip));

        int anchoTexto = pnlSidebar.Width - relleno - _inicioTexto - _derecha;
        foreach (var boton in BotonesDelMenu())
        {
            var lineas = Lineas(boton.Text, boton.Font, anchoTexto);
            _lineas[boton] = lineas;
            boton.Height = lineas.Count * boton.Font.Height + EscalaDpi.Px(this, 5);
            boton.Padding = new Padding(_inicioTexto, 0, 0, 0);
            boton.Paint += BotonMenu_Paint;
            boton.MouseEnter += (s, e) => { _botonesBajoMouse.Add((Button)s!); ((Button)s!).Invalidate(); };
            boton.MouseLeave += (s, e) => { _botonesBajoMouse.Remove((Button)s!); ((Button)s!).Invalidate(); };
            boton.Cursor = Cursors.Hand;
        }

        // Pie fijo: separador + "Cerrar sesion" siempre visible, sin importar el alto.
        btnCerrarSesion.Dock = DockStyle.Bottom;
        pnlPie.Height = pnlSeparador.Height + EscalaDpi.Px(this, 4) + btnCerrarSesion.Height + pnlPie.Padding.Bottom;

        flowMenu.SizeChanged += (s, e) => AjustarAnchoDeBotones();
        AjustarAnchoDeBotones();
    }

    private const TextFormatFlags FlagsTexto = TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.Left;

    private static int MedirTexto(string texto, Font fuente) =>
        TextRenderer.MeasureText(texto, fuente, new Size(int.MaxValue, int.MaxValue), FlagsTexto).Width;

    // Tramos del texto: se puede cortar en un espacio o justo despues de una barra.
    private static List<(string texto, bool espacioAntes)> Tramos(string texto)
    {
        var tramos = new List<(string, bool)>();
        var actual = new System.Text.StringBuilder();
        bool espacio = false;
        foreach (char c in texto)
        {
            if (c == ' ')
            {
                if (actual.Length > 0) { tramos.Add((actual.ToString(), espacio)); actual.Clear(); }
                espacio = true;
            }
            else
            {
                actual.Append(c);
                if (c == '/') { tramos.Add((actual.ToString(), espacio)); actual.Clear(); espacio = false; }
            }
        }
        if (actual.Length > 0) tramos.Add((actual.ToString(), espacio));
        return tramos;
    }

    // Reparte el texto en las lineas necesarias para el ancho dado.
    private static List<string> Lineas(string texto, Font fuente, int ancho)
    {
        var lineas = new List<string>();
        string actual = "";
        foreach (var (tramo, espacioAntes) in Tramos(texto))
        {
            string candidata = actual.Length == 0 ? tramo : actual + (espacioAntes ? " " : "") + tramo;
            if (actual.Length > 0 && MedirTexto(candidata, fuente) > ancho)
            {
                lineas.Add(actual);
                actual = tramo;
            }
            else
            {
                actual = candidata;
            }
        }
        lineas.Add(actual);
        return lineas;
    }

    // Cada boton ocupa todo el ancho de la lista (menos el relleno de la lista).
    private void AjustarAnchoDeBotones()
    {
        int ancho = flowMenu.ClientSize.Width - flowMenu.Padding.Horizontal;
        if (ancho <= 0) return;

        foreach (var boton in BotonesDelMenu())
            if (boton != btnCerrarSesion && boton.Width != ancho)
                boton.Width = ancho;
    }

    // Dibuja fondo, icono, texto (en una o dos lineas) y, en el boton activo, una
    // barra de acento. El texto propio tapa al que dibuja el Button.
    private void BotonMenu_Paint(object? sender, PaintEventArgs e)
    {
        var boton = (Button)sender!;
        if (!_iconos.TryGetValue(boton, out var glifo)) return;

        bool activo = ReferenceEquals(boton, _botonActivo);
        bool bajoMouse = _botonesBajoMouse.Contains(boton);
        bool resaltado = activo || bajoMouse;

        using (var fondo = new SolidBrush(bajoMouse ? boton.FlatAppearance.MouseOverBackColor : boton.BackColor))
            e.Graphics.FillRectangle(fondo, boton.ClientRectangle);

        Color colorIcono = boton == btnCerrarSesion
            ? ColorIconoSalir
            : resaltado ? Color.White : ColorIconoNormal;

        var zona = new Rectangle(_izquierda, 0, _anchoZonaIcono, boton.Height);
        TextRenderer.DrawText(e.Graphics, glifo, _fuenteIconos, zona, colorIcono,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);

        var lineas = _lineas[boton];
        int altoLinea = boton.Font.Height;
        int arriba = (boton.Height - lineas.Count * altoLinea) / 2;
        for (int i = 0; i < lineas.Count; i++)
        {
            var area = new Rectangle(_inicioTexto, arriba + i * altoLinea, boton.Width - _inicioTexto, altoLinea);
            TextRenderer.DrawText(e.Graphics, lineas[i], boton.Font, area, boton.ForeColor, FlagsTexto | TextFormatFlags.VerticalCenter);
        }

        if (activo)
        {
            int ancho = Math.Max(3, EscalaDpi.Px(this, 3));
            int margenVertical = EscalaDpi.Px(this, 7);
            using var pincel = new SolidBrush(Color.White);
            e.Graphics.FillRectangle(pincel, 0, margenVertical, ancho, boton.Height - 2 * margenVertical);
        }
    }

    private void ResaltarBoton(Button boton)
    {
        if (_botonActivo != null)
        {
            _botonActivo.BackColor = ColorSidebarNormal;
            _botonActivo.FlatAppearance.MouseOverBackColor = ColorSidebarHover;
            _botonActivo.Invalidate();
        }

        _botonActivo = boton;
        _botonActivo.BackColor = ColorSidebarActivo;
        _botonActivo.FlatAppearance.MouseOverBackColor = ColorSidebarActivo;
        _botonActivo.Invalidate();
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

    // Al entrar con Enter desde el login Windows deja visibles los recuadros de
    // foco; se ocultan hasta que el usuario use el teclado (Tab) en esta ventana.
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        const int WM_UPDATEUISTATE = 0x0128;
        const int UIS_SET = 1;
        const int UISF_HIDEFOCUS = 1;
        SendMessage(Handle, WM_UPDATEUISTATE, (IntPtr)((UISF_HIDEFOCUS << 16) | UIS_SET), IntPtr.Zero);
        foreach (var boton in BotonesDelMenu()) boton.Invalidate();
    }

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    private void BtnCerrarSesion_Click(object? sender, EventArgs e)
    {
        var login = new FormLogin();
        login.Show();
        Close();
    }

    private void pnlContenido_Paint(object? sender, PaintEventArgs e) { }
}
