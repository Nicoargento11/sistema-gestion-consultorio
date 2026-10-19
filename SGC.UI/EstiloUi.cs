namespace SGC.UI;

// Tipos de mensaje de la franja inferior (FranjaMensaje).
public enum TipoMensaje { Ok, Error, Advertencia }

// Tipos de boton plano de la app (ver EstiloUi.EstilizarBoton).
public enum TipoBoton { Primario, Secundario, Peligro, Exito, Advertencia }

// Colores, fuentes y pequenos utilitarios de estilo compartidos por todas las
// pantallas redisenadas (el patron de FormTurnos): tarjeta blanca con borde de
// 1 px, botones planos, franja de mensajes, chips de contadores y grilla.
//
// Receta de la tarjeta (se arma en el Designer, con controles estandar):
//   marco   = Panel BackColor = EstiloUi.Borde, Padding = 1, Dock = Fill
//   tarjeta = Panel BackColor = White, Padding = (16, 14, 16, 14), AutoScroll
//   titulo  = Label Segoe UI 11 Bold, ForeColor = EstiloUi.Texto ("NUEVO ...")
//   pasos   = Label Segoe UI 9 Bold, Margin (0, 10, 0, 3), texto "1. Nombre"
internal static class EstiloUi
{
    // Colores: misma paleta que el resto de la app (FormInicio / FormMedicos).
    public static readonly Color Texto = Color.FromArgb(27, 42, 74);
    public static readonly Color Secundario = Color.FromArgb(110, 120, 135);
    public static readonly Color Azul = Color.FromArgb(46, 134, 222);
    public static readonly Color Borde = Color.FromArgb(214, 220, 230);
    public static readonly Color Ok = Color.FromArgb(28, 120, 66);
    public static readonly Color Error = Color.FromArgb(192, 57, 43);
    public static readonly Color Advertencia = Color.FromArgb(176, 96, 0);
    public static readonly Color FondoPantalla = Color.FromArgb(245, 246, 250);
    public static readonly Color GrillaLinea = Color.FromArgb(228, 232, 240);

    // Colores de botones planos.
    public static readonly Color BotonPrimario = Azul;
    public static readonly Color BotonExito = Color.FromArgb(39, 174, 96);
    public static readonly Color BotonPeligro = Color.FromArgb(231, 76, 60);
    public static readonly Color BotonAdvertencia = Color.FromArgb(230, 145, 45);
    public static readonly Color BotonDeshabilitadoFondo = Color.FromArgb(226, 230, 237);
    public static readonly Color BotonDeshabilitadoTexto = Color.FromArgb(140, 148, 160);

    public static readonly Font FuenteChip = new("Segoe UI", 9.5F);
    public static readonly Font FuenteChica = new("Segoe UI", 8.5F);
    public static readonly Font FuenteContador = new("Segoe UI", 9F, FontStyle.Bold);
    public static readonly Font FuenteEncabezadoGrilla = new("Segoe UI", 9.5F, FontStyle.Bold);

    public static Color ColorDe(TipoMensaje tipo) => tipo switch
    {
        TipoMensaje.Error => Error,
        TipoMensaje.Advertencia => Advertencia,
        _ => Ok
    };

    // Botones planos de color: cuando se deshabilitan pasan a un gris uniforme.
    // Toma como color activo el BackColor que el boton ya tiene (Designer).
    public static void EstilizarBoton(Button boton)
    {
        Color colorActivo = boton.BackColor;
        void Aplicar()
        {
            boton.BackColor = boton.Enabled ? colorActivo : BotonDeshabilitadoFondo;
            boton.ForeColor = boton.Enabled ? Color.White : BotonDeshabilitadoTexto;
            boton.Cursor = boton.Enabled ? Cursors.Hand : Cursors.Default;
        }
        boton.EnabledChanged += (s, e) => Aplicar();
        Aplicar();
    }

    // Aplica el aspecto completo de un tipo de boton y su estado deshabilitado.
    // Primario/Exito/Peligro/Advertencia: relleno de color con texto blanco.
    // Secundario: blanco con borde (solo el texto se atenua al deshabilitarse).
    public static void EstilizarBoton(Button boton, TipoBoton tipo)
    {
        boton.FlatStyle = FlatStyle.Flat;
        boton.UseVisualStyleBackColor = false;

        if (tipo == TipoBoton.Secundario)
        {
            boton.BackColor = Color.White;
            boton.ForeColor = Texto;
            boton.FlatAppearance.BorderColor = Borde;
            boton.FlatAppearance.BorderSize = 1;
            boton.Cursor = Cursors.Hand;
            boton.EnabledChanged += (s, e) =>
            {
                boton.ForeColor = boton.Enabled ? Texto : BotonDeshabilitadoTexto;
                boton.Cursor = boton.Enabled ? Cursors.Hand : Cursors.Default;
            };
            return;
        }

        boton.FlatAppearance.BorderSize = 0;
        boton.BackColor = tipo switch
        {
            TipoBoton.Exito => BotonExito,
            TipoBoton.Peligro => BotonPeligro,
            TipoBoton.Advertencia => BotonAdvertencia,
            _ => BotonPrimario
        };
        EstilizarBoton(boton);
    }

    // Limita el ancho de un texto que puede ocupar varias lineas al de su contenedor.
    public static void Acotar(Label etiqueta, Control contenedor)
    {
        int ancho = contenedor.ClientSize.Width;
        if (ancho <= 0) return;

        var maximo = new Size(ancho, 0);
        if (etiqueta.MaximumSize != maximo) etiqueta.MaximumSize = maximo;
    }

    // Los paneles de flujo que parten sus controles en varias filas (filtros,
    // contadores, botones, leyenda) no usan AutoSize: dentro de contenedores
    // AutoSize anidados se calcula su alto con un ancho equivocado. En su lugar
    // el alto se fija aca con el ancho real, cada vez que cambia el ancho o el contenido.
    public static void AjustarFlujos(params FlowLayoutPanel[] flujos)
    {
        foreach (var flujo in flujos)
        {
            if (flujo.Width <= 0) continue;
            int alto = flujo.GetPreferredSize(new Size(flujo.Width, 0)).Height;
            if (flujo.Height != alto) flujo.Height = alto;
        }
    }
}
