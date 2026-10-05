namespace SGC.UI;

// Filtros de teclado reutilizables: evitan que se puedan tipear caracteres
// invalidos en el momento, en vez de recien avisar con un error al apretar
// Guardar. Se usan como handler del evento KeyPress de TextBox/ComboBox.
internal static class FiltrosTexto
{
    public static void SoloLetras(object? sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
            e.Handled = true;
    }

    public static void SoloNumeros(object? sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            e.Handled = true;
    }

    // Letras, numeros, '_' y '.' - mismo criterio que UsuarioService.Validar
    // para NombreUsuario.
    public static void SoloUsuario(object? sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '_' && e.KeyChar != '.')
            e.Handled = true;
    }
}
