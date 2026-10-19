namespace SGC.UI;

// Barra de contadores (chips de color) sobre un FlowLayoutPanel del Designer.
// Los chips se crean en codigo (no en InitializeComponent) para que el
// disenador no los borre y para medir con la escala de pantalla ya aplicada.
// Cada chip es un Label llamado "lblContador" + clave.
internal sealed class BarraContadores
{
    private readonly FlowLayoutPanel _flujo;
    private readonly Dictionary<string, Label> _chips = new();

    public BarraContadores(FlowLayoutPanel flujo) => _flujo = flujo;

    public void Agregar(string clave, Color fondo, Color texto)
    {
        var chip = CrearChip(_flujo, fondo, texto);
        chip.Name = "lblContador" + clave;
        _chips[clave] = chip;
        _flujo.Controls.Add(chip);
    }

    public void Poner(string clave, string texto) => _chips[clave].Text = texto;

    // Chip suelto: etiqueta con fondo de color y texto en negrita.
    public static Label CrearChip(Control referencia, Color fondo, Color texto)
    {
        return new Label
        {
            AutoSize = true,
            Font = EstiloUi.FuenteContador,
            BackColor = fondo,
            ForeColor = texto,
            Margin = new Padding(0, 0, EscalaDpi.Px(referencia, 6), EscalaDpi.Px(referencia, 4)),
            Padding = new Padding(EscalaDpi.Px(referencia, 8), EscalaDpi.Px(referencia, 4), EscalaDpi.Px(referencia, 8), EscalaDpi.Px(referencia, 4))
        };
    }
}
