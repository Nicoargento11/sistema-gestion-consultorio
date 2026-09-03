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

        BtnNuevo.Click += BtnNuevo_Click;
        BtnGuardar.Click += BtnGuardar_Click;
        BtnEliminar.Click += BtnEliminar_Click;
        TxtBuscar.TextChanged += (s, e) => CargarGrilla(TxtBuscar.Text);
        DgvPacientes.SelectionChanged += DgvPacientes_SelectionChanged;

        CargarGrilla();
        AcceptButton = BtnGuardar;
    }

    private void ConfigurarColumnas()
    {
        DgvPacientes.AutoGenerateColumns = false;
        DgvPacientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "Id", DataPropertyName = "Id", FillWeight = 8 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colApellido", HeaderText = "Apellido", DataPropertyName = "Apellido", FillWeight = 22 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNombre", HeaderText = "Nombre", DataPropertyName = "Nombre", FillWeight = 22 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDni", HeaderText = "DNI", DataPropertyName = "Dni", FillWeight = 16 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTelefono", HeaderText = "Telefono", DataPropertyName = "Telefono", FillWeight = 16 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEmail", HeaderText = "Email", DataPropertyName = "Email", FillWeight = 24 });
    }

    private void CargarGrilla(string? filtro = null)
    {
        var pacientes = _service.ObtenerTodos();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            var f = filtro.Trim().ToLower();
            pacientes = pacientes.Where(p =>
                p.NombreCompleto.ToLower().Contains(f) ||
                p.Dni.Contains(f) ||
                p.Telefono.Contains(f) ||
                p.Email.ToLower().Contains(f)).ToList();
        }

        DgvPacientes.SelectionChanged -= DgvPacientes_SelectionChanged;
        DgvPacientes.DataSource = pacientes;
        DgvPacientes.ClearSelection();
        DgvPacientes.SelectionChanged += DgvPacientes_SelectionChanged;
    }

    private void BtnNuevo_Click(object? sender, EventArgs e)
    {
        _idSeleccionado = null;
        TxtNombre.Text = "";
        TxtApellido.Text = "";
        TxtDni.Text = "";
        TxtEmail.Text = "";
        TxtTelefono.Text = "";
        BtnGuardar.Text = "Guardar";
        LblMensaje.Text = "";
        DgvPacientes.ClearSelection();
        TxtNombre.Focus();
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        try
        {
            var paciente = new Paciente
            {
                Id = _idSeleccionado ?? 0,
                Nombre = TxtNombre.Text.Trim(),
                Apellido = TxtApellido.Text.Trim(),
                Dni = TxtDni.Text.Trim(),
                Email = TxtEmail.Text.Trim(),
                Telefono = TxtTelefono.Text.Trim()
            };

            if (_idSeleccionado == null)
                _service.Agregar(paciente);
            else
                _service.Modificar(paciente);

            CargarGrilla(TxtBuscar.Text);
            BtnNuevo_Click(this, EventArgs.Empty);

            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Paciente guardado correctamente.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void BtnEliminar_Click(object? sender, EventArgs e)
    {
        if (DgvPacientes.CurrentRow == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un paciente de la lista primero.";
            return;
        }

        var pacienteSeleccionado = (Paciente)DgvPacientes.CurrentRow.DataBoundItem;

        var respuesta = MessageBox.Show(
            $"Esta seguro que desea dar de baja al paciente {pacienteSeleccionado.NombreCompleto}?",
            "Confirmar eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (respuesta != DialogResult.Yes)
            return;

        try
        {
            _service.EliminarLogico(pacienteSeleccionado.Id);
            CargarGrilla(TxtBuscar.Text);
            BtnNuevo_Click(this, EventArgs.Empty);

            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Paciente eliminado correctamente (Baja logica).";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void DgvPacientes_SelectionChanged(object? sender, EventArgs e)
    {
        if (DgvPacientes.CurrentRow == null) return;

        var paciente = (Paciente)DgvPacientes.CurrentRow.DataBoundItem;

        _idSeleccionado = paciente.Id;
        TxtNombre.Text = paciente.Nombre;
        TxtApellido.Text = paciente.Apellido;
        TxtDni.Text = paciente.Dni;
        TxtEmail.Text = paciente.Email;
        TxtTelefono.Text = paciente.Telefono;
        BtnGuardar.Text = "Actualizar";
        LblMensaje.Text = "";
    }
}