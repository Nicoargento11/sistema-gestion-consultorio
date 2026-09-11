using SGC.Entidades;
using SGC.Logica;
namespace SGC.UI;

public partial class FormPacientes : Form
{
    private readonly PacienteService _service = new();
    private readonly ObraSocialService _obraSocialService = new();
    private int? _idSeleccionado = null;
    public FormPacientes()
    {
        InitializeComponent();
        ConfigurarColumnas();
        CargarCombos();
        CargarGrilla();
        AcceptButton = BtnGuardar;
    }

    private void CargarCombos()
    {
        // Id = 0 es un pseudo-item que representa "Particular" (sin obra
        // social) - ObraSocial real empieza en Id = 1, no hay colision.
        var opciones = new List<ObraSocial> { new ObraSocial { Id = 0, Nombre = "Particular (sin obra social)" } };
        opciones.AddRange(_obraSocialService.ObtenerTodos());

        CboObraSocial.DataSource = opciones;
        CboObraSocial.DisplayMember = "Nombre";
        CboObraSocial.ValueMember = "Id";
    }

    private void ConfigurarColumnas()
    {
        // Se configura acá, en código, y no en el Designer, porque el diseñador
        // visual de Visual Studio borra las columnas de un DataGridView cada vez
        // que se abre el formulario. Acá es inmune a eso.
        // AutoSizeColumnsMode = Fill reparte el ancho disponible segun FillWeight
        // (proporcional) en vez de pixeles fijos - necesario porque FormPacientes
        // ahora se embebe en pnlContenido y ya no tiene un ancho de ventana fijo.
        DgvPacientes.AutoGenerateColumns = false;
        DgvPacientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "Id", DataPropertyName = "Id", FillWeight = 40 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNombre", HeaderText = "Nombre", DataPropertyName = "Nombre", FillWeight = 130 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colApellido", HeaderText = "Apellido", DataPropertyName = "Apellido", FillWeight = 130 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFechaNacimiento", HeaderText = "Fecha Nacimiento", DataPropertyName = "FechaNacimiento", FillWeight = 110 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDni", HeaderText = "DNI", DataPropertyName = "Dni", FillWeight = 100 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEmail", HeaderText = "Email", DataPropertyName = "Email", FillWeight = 170 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTelefono", HeaderText = "Telefono", DataPropertyName = "Telefono", FillWeight = 110 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colObraSocial", HeaderText = "Obra Social", DataPropertyName = "ObraSocialNombre", FillWeight = 130 });
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
        CboObraSocial.SelectedValue = 0;
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
                ObraSocialId = (int)(CboObraSocial.SelectedValue ?? 0) == 0 ? null : (int)CboObraSocial.SelectedValue!,
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
            CboObraSocial.SelectedValue = 0;
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
            CboObraSocial.SelectedValue = 0;
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
        CboObraSocial.SelectedValue = paciente.ObraSocialId ?? 0;
        DtpFechaNacimiento.Value = paciente.FechaNacimiento.ToDateTime(TimeOnly.MinValue);
    }

    private void DgvPacientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }
}
