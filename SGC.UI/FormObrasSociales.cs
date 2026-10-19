using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormObrasSociales : Form
{
    private readonly ObraSocialService _service = new();
    private int? _idSeleccionado = null;

    // Detalle de la obra social seleccionada (medicos que la aceptan). Se crea
    // en codigo y no en el Designer para que el disenador no lo borre.
    private readonly Label _lblDetalle = new();
    private readonly ToolTip _toolTip = new();

    public FormObrasSociales()
    {
        InitializeComponent();
        ConfigurarDetalle();
        ConfigurarColumnas();
        CargarGrilla();
        // WinForms selecciona sola la primera fila recien cuando el control
        // se muestra de verdad en pantalla (no en el constructor) - limpiar
        // ahi mismo no sirve, hay que hacerlo en Load, que corre despues.
        Load += (s, e) => BtnNuevo_Click(this, EventArgs.Empty);
    }

    private void ConfigurarColumnas()
    {
        DgvObrasSociales.AutoGenerateColumns = false;
        DgvObrasSociales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvObrasSociales.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNombre", HeaderText = "Nombre", DataPropertyName = "Nombre", FillWeight = 200 });
        DgvObrasSociales.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPorcentaje", HeaderText = "% Cobertura", DataPropertyName = "PorcentajeCobertura", FillWeight = 100 });
        DgvObrasSociales.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colMedicos",
            HeaderText = "Médicos que la aceptan",
            DataPropertyName = "MedicosQueAceptan",
            FillWeight = 130,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        DgvObrasSociales.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colPacientes",
            HeaderText = "Pacientes afiliados",
            DataPropertyName = "PacientesAfiliados",
            FillWeight = 110,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        AjustarEncabezados();
    }

    // Los encabezados largos envuelven en dos lineas en vez de cortarse.
    private void AjustarEncabezados()
    {
        DgvObrasSociales.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
        DgvObrasSociales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        DgvObrasSociales.ColumnHeadersHeight = 52;
        foreach (DataGridViewColumn columna in DgvObrasSociales.Columns)
            columna.MinimumWidth = 90;
    }

    private void ConfigurarDetalle()
    {
        _lblDetalle.Dock = DockStyle.Bottom;
        _lblDetalle.Height = 54;
        _lblDetalle.Padding = new Padding(20, 6, 20, 6);
        _lblDetalle.Font = new Font("Segoe UI", 9.5F);
        _lblDetalle.ForeColor = Color.FromArgb(27, 42, 74);
        _lblDetalle.BackColor = Color.FromArgb(244, 246, 250);
        _lblDetalle.AutoEllipsis = true;
        _lblDetalle.TextAlign = ContentAlignment.MiddleLeft;
        Controls.Add(_lblDetalle);
        MostrarDetalle(null);
    }

    // Muestra los medicos que aceptan la obra social elegida; el texto largo se
    // corta con puntos suspensivos y el tooltip muestra la lista completa.
    private void MostrarDetalle(ObraSocialFila? fila)
    {
        string texto;
        if (fila == null)
            texto = "Seleccione una obra social para ver los médicos que la aceptan.";
        else if (fila.Medicos.Count == 0)
            texto = $"{fila.Nombre}: ningún médico activo la acepta por el momento.";
        else
            texto = $"Médicos que aceptan {fila.Nombre} ({fila.Medicos.Count}): {string.Join("; ", fila.Medicos)}";

        _lblDetalle.Text = texto;
        _toolTip.SetToolTip(_lblDetalle, texto);
    }

    private void CargarGrilla()
    {
        // Se desengancha SelectionChanged mientras se reasigna el DataSource
        // para que no corra con filas a medio cargar.
        DgvObrasSociales.SelectionChanged -= DgvObrasSociales_SelectionChanged;
        try
        {
            DgvObrasSociales.DataSource = _service.ObtenerListado();
        }
        catch (Exception ex)
        {
            DgvObrasSociales.DataSource = new List<ObraSocialFila>();
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "No se pudo cargar el listado de obras sociales: " + ex.Message;
        }
        finally
        {
            // Sin esto, WinForms selecciona sola la primera fila al asignar el
            // DataSource y precarga el formulario con ese registro.
            DgvObrasSociales.ClearSelection();
            DgvObrasSociales.CurrentCell = null;
            DgvObrasSociales.SelectionChanged += DgvObrasSociales_SelectionChanged;
        }
        MostrarDetalle(null);
    }

    private void BtnNuevo_Click(object sender, EventArgs e)
    {
        _idSeleccionado = null;
        MostrarDetalle(null);
        TxtNombre.Text = "";
        NudPorcentaje.Value = 0;
    }

    private void BtnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            var obraSocial = new ObraSocial
            {
                Id = _idSeleccionado ?? 0,
                Nombre = TxtNombre.Text,
                PorcentajeCobertura = NudPorcentaje.Value
            };
            if (_idSeleccionado == null)
            {
                _service.Agregar(obraSocial);
            }
            else
            {
                _service.Modificar(obraSocial);
            }
            CargarGrilla();
            BtnNuevo_Click(sender, e);

            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Obra social guardada correctamente.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = $"Error: {ex.Message}";
        }
    }

    private void BtnEliminar_Click(object sender, EventArgs e)
    {
        if (DgvObrasSociales.CurrentRow == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Debe seleccionar una obra social para eliminar.";
            return;
        }
        var obraSocialSeleccionada = ((ObraSocialFila)DgvObrasSociales.CurrentRow.DataBoundItem).ObraSocial;

        var respuesta = MessageBox.Show(
            $"Esta seguro que desea eliminar la obra social {obraSocialSeleccionada.Nombre}?",
            "Confirmar eliminacion",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (respuesta != DialogResult.Yes) return;

        try
        {
            _service.EliminarLogico(obraSocialSeleccionada.Id);
            CargarGrilla();
            BtnNuevo_Click(sender, e);
            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Obra social eliminada correctamente.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = $"Error: {ex.Message}";
        }
    }

    private void DgvObrasSociales_SelectionChanged(object? sender, EventArgs e)
    {
        if (DgvObrasSociales.CurrentRow == null) return;
        var fila = (ObraSocialFila)DgvObrasSociales.CurrentRow.DataBoundItem;
        var obraSocialSeleccionada = fila.ObraSocial;
        MostrarDetalle(fila);
        _idSeleccionado = obraSocialSeleccionada.Id;
        TxtNombre.Text = obraSocialSeleccionada.Nombre;
        NudPorcentaje.Value = obraSocialSeleccionada.PorcentajeCobertura;
    }
}
