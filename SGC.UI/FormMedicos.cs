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

        BtnNuevo.Click += BtnNuevo_Click;
        BtnGuardar.Click += BtnGuardar_Click;
        BtnEliminar.Click += BtnEliminar_Click;
        TxtBuscar.TextChanged += (s, e) => CargarGrilla(TxtBuscar.Text);
        DgvMedicos.SelectionChanged += DgvMedicos_SelectionChanged;

        CargarGrilla();
        AcceptButton = BtnGuardar;
    }

    private void ConfigurarColumnas()
    {
        DgvMedicos.AutoGenerateColumns = false;
        DgvMedicos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "Id", DataPropertyName = "Id", FillWeight = 8 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colApellido", HeaderText = "Apellido", DataPropertyName = "Apellido", FillWeight = 22 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNombre", HeaderText = "Nombre", DataPropertyName = "Nombre", FillWeight = 22 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMatricula", HeaderText = "Matricula", DataPropertyName = "Matricula", FillWeight = 16 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEspecialidad", HeaderText = "Especialidad", DataPropertyName = "Especialidad", FillWeight = 20 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDni", HeaderText = "DNI", DataPropertyName = "Dni", FillWeight = 14 });
    }

    private void CargarGrilla(string? filtro = null)
    {
        var medicos = _service.ObtenerTodos();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            var f = filtro.Trim().ToLower();
            medicos = medicos.Where(m =>
                m.NombreCompleto.ToLower().Contains(f) ||
                m.Dni.Contains(f) ||
                m.Matricula.ToLower().Contains(f) ||
                m.Especialidad.ToLower().Contains(f)).ToList();
        }

        DgvMedicos.SelectionChanged -= DgvMedicos_SelectionChanged;
        DgvMedicos.DataSource = medicos;
        DgvMedicos.ClearSelection();
        DgvMedicos.SelectionChanged += DgvMedicos_SelectionChanged;
    }

    private void BtnNuevo_Click(object? sender, EventArgs e)
    {
        _idSeleccionado = null;
        TxtNombre.Text = "";
        TxtApellido.Text = "";
        TxtDni.Text = "";
        TxtMatricula.Text = "";
        if (CboEspecialidad.Items.Count > 0)
            CboEspecialidad.SelectedIndex = 0;
        BtnGuardar.Text = "Guardar";
        LblMensaje.Text = "";
        DgvMedicos.ClearSelection();
        TxtNombre.Focus();
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        try
        {
            var medico = new Medico
            {
                Id = _idSeleccionado ?? 0,
                Nombre = TxtNombre.Text.Trim(),
                Apellido = TxtApellido.Text.Trim(),
                Dni = TxtDni.Text.Trim(),
                Matricula = TxtMatricula.Text.Trim(),
                Especialidad = CboEspecialidad.Text.Trim(),
                Activo = true
            };

            if (_idSeleccionado == null)
                _service.Agregar(medico);
            else
                _service.Modificar(medico);

            CargarGrilla(TxtBuscar.Text);
            BtnNuevo_Click(this, EventArgs.Empty);

            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Profesional medico guardado correctamente.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void BtnEliminar_Click(object? sender, EventArgs e)
    {
        if (DgvMedicos.CurrentRow == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un medico de la lista primero.";
            return;
        }

        var medicoSeleccionado = (Medico)DgvMedicos.CurrentRow.DataBoundItem;

        var respuesta = MessageBox.Show(
            $"Esta seguro que desea dar de baja al Dr./Dra. {medicoSeleccionado.NombreCompleto}?",
            "Confirmar eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (respuesta != DialogResult.Yes)
            return;

        try
        {
            _service.EliminarLogico(medicoSeleccionado.Id);
            CargarGrilla(TxtBuscar.Text);
            BtnNuevo_Click(this, EventArgs.Empty);

            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Profesional medico dado de baja correctamente.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void DgvMedicos_SelectionChanged(object? sender, EventArgs e)
    {
        if (DgvMedicos.CurrentRow == null) return;

        var medico = (Medico)DgvMedicos.CurrentRow.DataBoundItem;

        _idSeleccionado = medico.Id;
        TxtNombre.Text = medico.Nombre;
        TxtApellido.Text = medico.Apellido;
        TxtDni.Text = medico.Dni;
        TxtMatricula.Text = medico.Matricula;
        CboEspecialidad.Text = medico.Especialidad;
        BtnGuardar.Text = "Actualizar";
        LblMensaje.Text = "";
    }
}