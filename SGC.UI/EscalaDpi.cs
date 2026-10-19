namespace SGC.UI;

// Los controles que se crean en codigo despues de InitializeComponent no pasan
// por el autoescalado del Designer: sus tamanos y posiciones son pixeles reales.
// Px convierte una medida de "diseno" (la misma unidad que usan las coordenadas
// del Designer) a pixeles con el mismo factor de autoescala que recibieron los
// controles del Designer, para que los controles creados en codigo queden
// alineados con ellos.
internal static class EscalaDpi
{
    // Todos los formularios se disenaron con AutoScaleDimensions = 8 x 20
    // (AutoScaleMode.Font). Tras el escalado el formulario reemplaza esa
    // propiedad por la medida actual, asi que el valor de diseno se fija aca.
    private const double AnchoDeFuenteDeDiseno = 8.0;

    public static double Factor(Control control)
    {
        var contenedor = control as ContainerControl ?? control.FindForm();
        if (contenedor != null && contenedor.AutoScaleMode == AutoScaleMode.Font)
            return contenedor.CurrentAutoScaleDimensions.Width / AnchoDeFuenteDeDiseno;

        return control.DeviceDpi / 96.0;
    }

    public static int Px(Control control, int valor) => (int)Math.Round(valor * Factor(control));
}
