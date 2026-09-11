using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormObrasSociales : Form
{
    private readonly ObraSocialService _service = new();
    private int? _idSeleccionado = null;

    public FormObrasSociales()
    {
        InitializeComponent();
        ConfigurarColumnas();
        CargarGrilla();
    }

    private void ConfigurarColumnas()
    {
        DgvObrasSociales.AutoGenerateColumns = false;
        DgvObrasSociales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvObrasSociales.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNombre", HeaderText = "Nombre", DataPropertyName = "Nombre", FillWeight = 200 });
        DgvObrasSociales.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPorcentaje", HeaderText = "% Cobertura", DataPropertyName = "PorcentajeCobertura", FillWeight = 120 });
    }

    private void CargarGrilla()
    {
        DgvObrasSociales.DataSource = _service.ObtenerTodos();
    }

    private void BtnNuevo_Click(object sender, EventArgs e)
    {
        _idSeleccionado = null;
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
        var obraSocialSeleccionada = (ObraSocial)DgvObrasSociales.CurrentRow.DataBoundItem;

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

    private void DgvObrasSociales_SelectionChanged(object sender, EventArgs e)
    {
        if (DgvObrasSociales.CurrentRow == null) return;
        var obraSocialSeleccionada = (ObraSocial)DgvObrasSociales.CurrentRow.DataBoundItem;
        _idSeleccionado = obraSocialSeleccionada.Id;
        TxtNombre.Text = obraSocialSeleccionada.Nombre;
        NudPorcentaje.Value = obraSocialSeleccionada.PorcentajeCobertura;
    }
}
