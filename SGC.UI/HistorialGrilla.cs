using SGC.Logica;

namespace SGC.UI;

// Piezas compartidas por FormHistorialClinico y FormHistorialPaciente: columnas
// de la grilla (con o sin datos clinicos), tooltips con el texto completo,
// validacion del rango de fechas y textos del contador / estado vacio.
// RNF#07: las columnas clinicas solo se agregan si el rol las puede ver, y
// los tooltips leen de AtencionFila, que ya viene vacia para otros roles.
internal static class HistorialGrilla
{
    public const string ColMotivo = "colMotivo";
    public const string ColProcedimiento = "colProcedimiento";
    public const string ColReceta = "colReceta";

    public static void AgregarColumnas(DataGridView dgv, bool incluyeClinico)
    {
        dgv.AutoGenerateColumns = false;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        // Anchos minimos en medidas de diseno (se convierten a pixeles reales).
        dgv.Columns.Add(Columna(dgv, "colFecha", "Fecha", "FechaStr", incluyeClinico ? 12 : 16, 95));
        dgv.Columns.Add(Columna(dgv, "colHorario", "Horario", "HorarioStr", incluyeClinico ? 14 : 18, 105));
        dgv.Columns.Add(Columna(dgv, "colMedico", "Médico tratante", "MedicoNombre", incluyeClinico ? 18 : 36, 110));
        dgv.Columns.Add(Columna(dgv, "colTipo", "Tipo de atención", "TipoActividadNombre", incluyeClinico ? 16 : 30, 125));

        if (!incluyeClinico) return;

        dgv.Columns.Add(Columna(dgv, ColMotivo, "Motivo de consulta", "MotivoResumen", 20, 90));
        dgv.Columns.Add(Columna(dgv, ColProcedimiento, "Procedimiento", "ProcedimientoResumen", 22, 90));
        dgv.Columns.Add(Columna(dgv, ColReceta, "Receta", "RecetaResumen", 20, 90));

        // El texto de la celda es un resumen de una linea; el completo sale en el tooltip.
        dgv.CellToolTipTextNeeded += Dgv_CellToolTipTextNeeded;
    }

    private static DataGridViewTextBoxColumn Columna(DataGridView dgv, string nombre, string encabezado, string propiedad, float peso, int minimo)
    {
        return new DataGridViewTextBoxColumn
        {
            Name = nombre,
            HeaderText = encabezado,
            DataPropertyName = propiedad,
            FillWeight = peso,
            MinimumWidth = EscalaDpi.Px(dgv, minimo)
        };
    }

    private static void Dgv_CellToolTipTextNeeded(object? sender, DataGridViewCellToolTipTextNeededEventArgs e)
    {
        if (sender is not DataGridView dgv || e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (dgv.Rows[e.RowIndex].DataBoundItem is not AtencionFila fila) return;

        e.ToolTipText = dgv.Columns[e.ColumnIndex].Name switch
        {
            ColMotivo => fila.MotivoConsulta,
            ColProcedimiento => fila.Procedimiento,
            ColReceta => fila.RecetaMedicamentos,
            _ => e.ToolTipText
        };
    }

    // Devuelve el mensaje de error si el rango es invalido, o null si es correcto.
    public static string? ValidarRango(DateTime desde, DateTime hasta)
    {
        return desde.Date > hasta.Date
            ? "La fecha 'Desde' no puede ser posterior a la fecha 'Hasta'. Corrija el rango para ver las atenciones."
            : null;
    }

    public static string Contador(int cantidad)
    {
        return cantidad == 1 ? "1 atención" : $"{cantidad} atenciones";
    }

    public static string MensajeVacio(bool hayFiltros)
    {
        return hayFiltros
            ? "No hay atenciones que coincidan con los filtros aplicados. Pruebe ampliar el rango o pulse Limpiar."
            : "El paciente no posee atenciones registradas.";
    }
}
