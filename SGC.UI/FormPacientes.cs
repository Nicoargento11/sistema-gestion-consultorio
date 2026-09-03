using SGC.Entidades;
using SGC.Logica;
namespace SGC.UI;

public partial class FormPacientes : Form
{
    private readonly PacienteService _service = new();
    private int? _idSeleccionado = null;
    public FormPacientes()
    {
        InitializeComponent();
        ConfigurarColumnas();
        CargarGrilla();
        AcceptButton = BtnGuardar;
    }

    private void ConfigurarColumnas()
    {
        // Se configura acá, en código, y no en el Designer, porque el diseñador
        // visual de Visual Studio borra las columnas de un DataGridView cada vez
        // que se abre el formulario. Acá es inmune a eso.
        DgvPacientes.AutoGenerateColumns = false;
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "Id", DataPropertyName = "Id", Width = 50 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNombre", HeaderText = "Nombre", DataPropertyName = "Nombre", Width = 150 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colApellido", HeaderText = "Apellido", DataPropertyName = "Apellido", Width = 150 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFechaNacimiento", HeaderText = "Fecha Nacimiento", DataPropertyName = "FechaNacimiento", Width = 120 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDni", HeaderText = "DNI", DataPropertyName = "Dni", Width = 120 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEmail", HeaderText = "Email", DataPropertyName = "Email", Width = 200 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTelefono", HeaderText = "Telefono", DataPropertyName = "Telefono", Width = 130 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colObraSocial", HeaderText = "Obra Social", DataPropertyName = "ObraSocial", Width = 150 });

    }

    private void FormPacientes_Load(object sender, EventArgs e)
    {

    }

    private void CargarGrilla()
    {
        DgvPacientes.DataSource = _service.ObtenerTodos();
    }

    private void BtnNuevo_Click(object sender, EventArgs e)
    {
        _idSeleccionado = null;
        TxtNombre.Text = "";
        TxtApellido.Text = "";
        TxtDni.Text = "";
        TxtEmail.Text = "";
        TxtTelefono.Text = "";
        CboObraSocial.Text = "";
        DtpFechaNacimiento.Value = DateTime.Today;
    }

    private void BtnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            var paciente = new Paciente
            {
                Id = _idSeleccionado ?? 0,
                Nombre = TxtNombre.Text,
                Apellido = TxtApellido.Text,
                Dni = TxtDni.Text,
                Email = TxtEmail.Text,
                Telefono = TxtTelefono.Text,
                ObraSocial = CboObraSocial.Text,
                FechaNacimiento = DateOnly.FromDateTime(DtpFechaNacimiento.Value)

            };

            if (_idSeleccionado == null)
                _service.Agregar(paciente);
            else
                _service.Modificar(paciente);

            CargarGrilla();

            // CargarGrilla selecciona sola la primera fila, lo que dispara
            // SelectionChanged y pisa _idSeleccionado. Lo reseteamos a propósito
            // después, para que el próximo alta no quede pensando que edita al primero.
            _idSeleccionado = null;
            TxtNombre.Text = "";
            TxtApellido.Text = "";
            TxtDni.Text = "";
            TxtEmail.Text = "";
            TxtTelefono.Text = "";
            CboObraSocial.Text = "";
            DtpFechaNacimiento.Value = DateTime.Today;

            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Paciente guardado correctamente";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void BtnEliminar_Click(object sender, EventArgs e)
    {
        if (DgvPacientes.CurrentRow == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un paciente de la lista primero.";
            return;
        }

        var pacienteSeleccionado = (Paciente)DgvPacientes.CurrentRow.DataBoundItem;

        var respuesta = MessageBox.Show(
            $"Esta seguro que desea eliminar a {pacienteSeleccionado.Nombre} {pacienteSeleccionado.Apellido}?",
            "Confirmar eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (respuesta != DialogResult.Yes)
            return;

        try
        {
            _service.EliminarLogico(pacienteSeleccionado.Id);
            CargarGrilla();

            _idSeleccionado = null;
            TxtNombre.Text = "";
            TxtApellido.Text = "";
            TxtDni.Text = "";
            TxtEmail.Text = "";
            TxtTelefono.Text = "";
            CboObraSocial.Text = "";
            DtpFechaNacimiento.Value = DateTime.Today;

            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Paciente eliminado correctamente.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void DgvPacientes_SelectionChanged(object sender, EventArgs e)
    {
        if (DgvPacientes.CurrentRow == null) return;

        var paciente = (Paciente)DgvPacientes.CurrentRow.DataBoundItem;

        _idSeleccionado = paciente.Id;
        TxtNombre.Text = paciente.Nombre;
        TxtApellido.Text = paciente.Apellido;
        TxtDni.Text = paciente.Dni;
        TxtEmail.Text = paciente.Email;
        TxtTelefono.Text = paciente.Telefono;
        CboObraSocial.Text = paciente.ObraSocial;
        DtpFechaNacimiento.Value = paciente.FechaNacimiento.ToDateTime(TimeOnly.MinValue);
    }

    private void DgvPacientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }
}
