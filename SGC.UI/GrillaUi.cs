namespace SGC.UI;

// Estilo y columnas comunes de las grillas de las pantallas redisenadas.
internal static class GrillaUi
{
    // Encabezado claro, seleccion azul con texto blanco, alturas escaladas.
    public static void Estilizar(DataGridView grilla, int altoEncabezado = 46, int altoFila = 28)
    {
        grilla.AutoGenerateColumns = false;
        grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grilla.ColumnHeadersHeight = EscalaDpi.Px(grilla, altoEncabezado);
        grilla.RowTemplate.Height = EscalaDpi.Px(grilla, altoFila);

        grilla.EnableHeadersVisualStyles = false;
        grilla.ColumnHeadersDefaultCellStyle.BackColor = EstiloUi.FondoPantalla;
        grilla.ColumnHeadersDefaultCellStyle.ForeColor = EstiloUi.Texto;
        grilla.ColumnHeadersDefaultCellStyle.SelectionBackColor = EstiloUi.FondoPantalla;
        grilla.ColumnHeadersDefaultCellStyle.SelectionForeColor = EstiloUi.Texto;
        grilla.ColumnHeadersDefaultCellStyle.Font = EstiloUi.FuenteEncabezadoGrilla;
        grilla.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
        grilla.ColumnHeadersDefaultCellStyle.Padding = new Padding(EscalaDpi.Px(grilla, 4), 0, 0, 0);
        grilla.GridColor = EstiloUi.GrillaLinea;

        // Fila seleccionada: mismo azul que el resto de la app, texto blanco.
        grilla.DefaultCellStyle.SelectionBackColor = EstiloUi.Azul;
        grilla.DefaultCellStyle.SelectionForeColor = Color.White;
    }

    // Ancho en pixeles reales del texto de muestra con la fuente de la grilla, mas holgura.
    public static int MinimoPara(DataGridView grilla, string muestra) =>
        TextRenderer.MeasureText(muestra, grilla.Font).Width + EscalaDpi.Px(grilla, 6);

    // El encabezado puede partirse en varias lineas, pero no una palabra: el
    // minimo de la columna alcanza para la palabra mas larga del titulo.
    public static int MinimoEncabezado(DataGridView grilla, string titulo)
    {
        var fuente = grilla.ColumnHeadersDefaultCellStyle.Font ?? grilla.Font;
        int palabra = titulo.Split(' ').Max(w => TextRenderer.MeasureText(w, fuente).Width);
        return palabra + EscalaDpi.Px(grilla, 14);
    }

    // MinimumWidth hace que, en una ventana angosta, la tabla scrollee
    // horizontalmente dentro de si misma en vez de apretar las columnas.
    public static DataGridViewTextBoxColumn AgregarColumna(DataGridView grilla, string nombre, string titulo,
        string? propiedad, float peso, int anchoMinimo, string? formato = null)
    {
        var columna = new DataGridViewTextBoxColumn
        {
            Name = nombre,
            HeaderText = titulo,
            FillWeight = peso,
            MinimumWidth = Math.Max(anchoMinimo, MinimoEncabezado(grilla, titulo)),
            SortMode = DataGridViewColumnSortMode.NotSortable
        };
        if (propiedad != null) columna.DataPropertyName = propiedad;
        if (formato != null) columna.DefaultCellStyle.Format = formato;
        columna.DefaultCellStyle.NullValue = "-";
        grilla.Columns.Add(columna);
        return columna;
    }
}
