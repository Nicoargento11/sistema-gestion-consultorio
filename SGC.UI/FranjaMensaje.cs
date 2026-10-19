namespace SGC.UI;

// Franja inferior de mensajes (ok / error / advertencia) de las pantallas
// redisenadas. Envuelve un Panel (Dock = Bottom, Padding (2, 8, 2, 0)) con un
// Label (Dock = Fill, Segoe UI 9.5 Bold) creados en el Designer. La franja
// queda oculta mientras no hay texto y crece con el texto (varias lineas).
internal sealed class FranjaMensaje
{
    private readonly Panel _franja;
    private readonly Label _etiqueta;

    public FranjaMensaje(Panel franja, Label etiqueta)
    {
        _franja = franja;
        _etiqueta = etiqueta;
        _franja.SizeChanged += (s, e) => Ajustar();
    }

    // Si es true, cada vez que la franja cambia de alto se vuelve a acomodar su
    // contenedor: sin esto Windows Forms puede dejar la franja unos pixeles
    // desfasada (y tapando el borde inferior del contenido) cuando su alto
    // cambia durante el mismo ciclo de layout en que se hace visible.
    public bool ReacomodarContenedor { get; init; }

    // Texto visible ("" si la franja esta oculta).
    public string Texto => _franja.Visible ? _etiqueta.Text : "";

    public void Mostrar(string texto, TipoMensaje tipo)
    {
        _etiqueta.ForeColor = EstiloUi.ColorDe(tipo);
        _etiqueta.Text = texto;
        _franja.Visible = !string.IsNullOrEmpty(texto);
        Ajustar();
    }

    public void Limpiar() => Mostrar("", TipoMensaje.Ok);

    // La franja crece con el texto (varias lineas si hace falta).
    public void Ajustar()
    {
        if (!_franja.Visible || string.IsNullOrEmpty(_etiqueta.Text)) return;

        int ancho = _franja.ClientSize.Width - _franja.Padding.Horizontal;
        if (ancho <= 0) return;

        int alto = TextRenderer.MeasureText(_etiqueta.Text, _etiqueta.Font,
            new Size(ancho, int.MaxValue), TextFormatFlags.WordBreak).Height;
        int deseado = alto + _franja.Padding.Vertical + EscalaDpi.Px(_franja, 4);
        if (_franja.Height != deseado)
        {
            _franja.Height = deseado;
            // Se difiere: el cambio de alto ocurre dentro del propio layout del
            // contenedor y un PerformLayout inmediato quedaria anidado e ignorado.
            if (ReacomodarContenedor && _franja.Parent is { IsHandleCreated: true } contenedor)
                contenedor.BeginInvoke(new Action(() => contenedor.PerformLayout()));
        }
    }
}
