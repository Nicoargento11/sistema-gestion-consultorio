using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormMedicos : Form
{
    private readonly MedicoService _service = new();
    private int? _idSeleccionado = null;
    public FormMedicos()
    {
        InitializeComponent();
        ConfigurarColumnas();
        CargarGrilla();
    }

    private void ConfigurarColumnas()
    {
        // Se configura acá, en código, y no en el Designer, porque el diseñador
        // visual de Visual Studio borra las columnas de un DataGridView cada vez
        // que se abre el formulario. Acá es inmune a eso.
        DgvMedicos.AutoGenerateColumns = false;
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "Id", DataPropertyName = "Id", Width = 50 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNombre", HeaderText = "Nombre", DataPropertyName = "Nombre", Width = 150 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colApellido", HeaderText = "Apellido", DataPropertyName = "Apellido", Width = 150 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDni", HeaderText = "Dni", DataPropertyName = "Dni", Width = 130 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMatricula", HeaderText = "Matricula", DataPropertyName = "Matricula", Width = 120 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEspecialidad", HeaderText = "Especialidad", DataPropertyName = "Especialidad", Width = 200 });
    }

    private void CargarGrilla()
    {
        DgvMedicos.DataSource = _service.ObtenerTodos();
    }

    private void BtnNuevo_Click(object sender, EventArgs e)
    {
        _idSeleccionado = null;
        TxtNombre.Text = "";
        TxtApellido.Text = "";
        TxtDni.Text = "";
        TxtEspecialidad.Text = "";
        TxtMatricula.Text = "";
    }

    private void BtnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            var medico = new Medico
            {
                Id = _idSeleccionado ?? 0,
                Nombre = TxtNombre.Text,
                Apellido = TxtApellido.Text,
                Dni = TxtDni.Text,
                Especialidad = TxtEspecialidad.Text,
                Matricula = TxtMatricula.Text
            };
            if (_idSeleccionado == null)
            {
                _service.Agregar(medico);
            }
            else
            {
                _service.Modificar(medico);
            }
            CargarGrilla();

            _idSeleccionado = null;
            TxtNombre.Text = "";
            TxtApellido.Text = "";
            TxtDni.Text = "";
            TxtEspecialidad.Text = "";
            TxtMatricula.Text = "";

            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Medico guardado correctamente";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void BtnEliminar_Click(object sender, EventArgs e)
    {
        if (DgvMedicos.CurrentRow == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un medico de la lista primero.";
            return;
        }

        var medicoSeleccionado = (Medico)DgvMedicos.CurrentRow.DataBoundItem;

        var respuesta = MessageBox.Show(
                   $"Esta seguro que desea eliminar a {medicoSeleccionado.Nombre} {medicoSeleccionado.Apellido}?",
                   "Confirmar eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (respuesta != DialogResult.Yes)
            return;

        try
        {
            _service.EliminarLogico(medicoSeleccionado.Id);
            CargarGrilla();

            _idSeleccionado = null;
            TxtNombre.Text = "";
            TxtApellido.Text = "";
            TxtDni.Text = "";
            TxtMatricula.Text = "";
            TxtEspecialidad.Text = "";

            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Medico eliminado correctamente.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }



    private void DgvMedicos_SelectionChanged_1(object sender, EventArgs e)
    {
        if (DgvMedicos.CurrentRow == null) return;

        var medico = (Medico)DgvMedicos.CurrentRow.DataBoundItem;

        _idSeleccionado = medico.Id;
        TxtNombre.Text = medico.Nombre;
        TxtApellido.Text = medico.Apellido;
        TxtDni.Text = medico.Dni;
        TxtEspecialidad.Text = medico.Especialidad;
        TxtMatricula.Text = medico.Matricula;
    }
}
