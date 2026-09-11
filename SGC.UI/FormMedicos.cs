using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormMedicos : Form
{
    private readonly MedicoService _service = new();
    private readonly ObraSocialService _obraSocialService = new();
    private int? _idSeleccionado = null;
    public FormMedicos()
    {
        InitializeComponent();
        ConfigurarColumnas();
        CargarCombos();
        CargarGrilla();
        DgvMedicos.SelectionChanged += DgvMedicos_SelectionChanged_1;
    }

    private void CargarCombos()
    {
        ClbObrasSociales.DataSource = _obraSocialService.ObtenerTodos();
        ClbObrasSociales.DisplayMember = "Nombre";
        ClbObrasSociales.ValueMember = "Id";
    }

    private void ConfigurarColumnas()
    {
        // Se configura ac�, en c�digo, y no en el Designer, porque el dise�ador
        // visual de Visual Studio borra las columnas de un DataGridView cada vez
        // que se abre el formulario. Ac� es inmune a eso.
        // AutoSizeColumnsMode = Fill reparte el ancho disponible segun FillWeight
        // (proporcional) en vez de pixeles fijos - necesario porque FormMedicos
        // ahora se embebe en pnlContenido y ya no tiene un ancho de ventana fijo.
        DgvMedicos.AutoGenerateColumns = false;
        DgvMedicos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "Id", DataPropertyName = "Id", FillWeight = 40 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNombre", HeaderText = "Nombre", DataPropertyName = "Nombre", FillWeight = 130 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colApellido", HeaderText = "Apellido", DataPropertyName = "Apellido", FillWeight = 130 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDni", HeaderText = "Dni", DataPropertyName = "Dni", FillWeight = 110 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMatricula", HeaderText = "Matricula", DataPropertyName = "Matricula", FillWeight = 110 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEspecialidad", HeaderText = "Especialidad", DataPropertyName = "Especialidad", FillWeight = 170 });
    }

    private void CargarGrilla()
    {
        DgvMedicos.DataSource = _service.ObtenerTodos();
    }

    private void LimpiarChecksObrasSociales()
    {
        for (int i = 0; i < ClbObrasSociales.Items.Count; i++)
        {
            ClbObrasSociales.SetItemChecked(i, false);
        }
    }

    private void BtnNuevo_Click(object sender, EventArgs e)
    {
        _idSeleccionado = null;
        TxtNombre.Text = "";
        TxtApellido.Text = "";
        TxtDni.Text = "";
        CboEspecialidad.Text = "";
        TxtMatricula.Text = "";
        NudPrecioConsultaParticular.Value = 0;
        LimpiarChecksObrasSociales();
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
                Especialidad = CboEspecialidad.Text,
                Matricula = TxtMatricula.Text,
                PrecioConsultaParticular = NudPrecioConsultaParticular.Value,
                ObrasSocialesAceptadasIds = ClbObrasSociales.CheckedItems.Cast<ObraSocial>().Select(o => o.Id).ToList()
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
            BtnNuevo_Click(sender, e);

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
            BtnNuevo_Click(sender, e);

            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Medico eliminado correctamente.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void DgvMedicos_SelectionChanged_1(object? sender, EventArgs e)
    {
        if (DgvMedicos.CurrentRow == null) return;

        var medico = (Medico)DgvMedicos.CurrentRow.DataBoundItem;

        _idSeleccionado = medico.Id;
        TxtNombre.Text = medico.Nombre;
        TxtApellido.Text = medico.Apellido;
        TxtDni.Text = medico.Dni;
        CboEspecialidad.Text = medico.Especialidad;
        TxtMatricula.Text = medico.Matricula;
        NudPrecioConsultaParticular.Value = medico.PrecioConsultaParticular;

        for (int i = 0; i < ClbObrasSociales.Items.Count; i++)
        {
            var obraSocial = (ObraSocial)ClbObrasSociales.Items[i];
            ClbObrasSociales.SetItemChecked(i, medico.ObrasSocialesAceptadasIds.Contains(obraSocial.Id));
        }
    }
}
