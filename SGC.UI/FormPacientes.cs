using SGC.Entidades;
using SGC.Logica;
namespace SGC.UI;

public partial class FormPacientes : Form
{
    private readonly PacienteService _service = new();
    private readonly ObraSocialService _obraSocialService = new();
    private readonly Usuario? _usuarioActivo;
    private int? _idSeleccionado = null;

    // usuarioActivo define que medicos puede ver una recepcionista en la
    // columna "Proximo turno"; null = sin restriccion.
    public FormPacientes(Usuario? usuarioActivo = null)
    {
        InitializeComponent();
        _usuarioActivo = usuarioActivo;
        ConfigurarColumnas();
        CargarCombos();
        CargarGrilla();
        AcceptButton = BtnGuardar;
        // WinForms selecciona sola la primera fila recien cuando el control
        // se muestra de verdad en pantalla (no en el constructor) - limpiar
        // ahi mismo no sirve, hay que hacerlo en Load, que corre despues.
        Load += (s, e) => BtnNuevo_Click(this, EventArgs.Empty);

        TxtNombre.KeyPress += FiltrosTexto.SoloLetras;
        TxtApellido.KeyPress += FiltrosTexto.SoloLetras;
        TxtDni.KeyPress += FiltrosTexto.SoloNumeros;
        TxtTelefono.KeyPress += FiltrosTexto.SoloNumeros;

        DgvPacientes.CellFormatting += DgvPacientes_CellFormatting;
    }

    // Que medicos puede ver el usuario en "Proximo turno": una recepcionista
    // solo los que tiene asignados (sin asignaciones = ninguno), un medico
    // solo el propio; administrador (o sin usuario) todos (null).
    private IReadOnlyCollection<int>? MedicosVisibles()
    {
        if (_usuarioActivo == null)
            return null;

        return _usuarioActivo.Rol switch
        {
            RolUsuario.Recepcionista => _usuarioActivo.MedicosAsignados.Select(m => m.Id).ToList(),
            RolUsuario.Medico => _usuarioActivo.MedicoId is int id ? new List<int> { id } : new List<int>(),
            _ => null
        };
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
        // Los encabezados largos ("Fecha Nacimiento", "Ultima visita") se parten en dos lineas sin cortarse.
        DgvPacientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "Id", DataPropertyName = "Id", FillWeight = 28 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNombre", HeaderText = "Nombre", DataPropertyName = "Nombre", FillWeight = 85 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colApellido", HeaderText = "Apellido", DataPropertyName = "Apellido", FillWeight = 85 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFechaNacimiento", HeaderText = "Fecha Nacimiento", DataPropertyName = "FechaNacimiento", FillWeight = 90 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDni", HeaderText = "DNI", DataPropertyName = "Dni", FillWeight = 75 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEmail", HeaderText = "Email", DataPropertyName = "Email", FillWeight = 115 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTelefono", HeaderText = "Teléfono", DataPropertyName = "Telefono", FillWeight = 90 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colObraSocial", HeaderText = "Obra Social", DataPropertyName = "ObraSocialNombre", FillWeight = 100 });

        // Columnas calculadas (edad, ultima visita, proximo turno, ausencias).
        // "-" cuando no hay dato. Con tantas columnas se fija un minimo para
        // que no queden ilegibles: si no entran, aparece la barra horizontal.
        var estiloCentrado = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, NullValue = "-" };
        var estiloFecha = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, NullValue = "-", Format = "dd/MM/yyyy" };
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEdad", HeaderText = "Edad", DataPropertyName = "Edad", FillWeight = 42, DefaultCellStyle = estiloCentrado });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colUltimaVisita", HeaderText = "Última visita", DataPropertyName = "UltimaVisita", FillWeight = 90, DefaultCellStyle = estiloFecha });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colProximoTurno", HeaderText = "Próximo turno", DataPropertyName = "ProximoTurno", FillWeight = 200 });
        DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAusencias", HeaderText = "Ausencias", DataPropertyName = "Ausencias", FillWeight = 75, DefaultCellStyle = estiloCentrado });

        foreach (DataGridViewColumn columna in DgvPacientes.Columns)
            columna.MinimumWidth = columna.Name == "colId" ? 30 : 55;
    }

    private void FormPacientes_Load(object sender, EventArgs e)
    {

    }

    private void CargarGrilla()
    {
        // Se desengancha SelectionChanged mientras se reasigna el DataSource
        // para que no corra con filas a medio cargar.
        DgvPacientes.SelectionChanged -= DgvPacientes_SelectionChanged;
        try
        {
            DgvPacientes.DataSource = _service.ObtenerListado(MedicosVisibles());
        }
        catch (Exception ex)
        {
            DgvPacientes.DataSource = new List<PacienteFila>();
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "No se pudo cargar el listado de pacientes: " + ex.Message;
        }
        finally
        {
            // Sin esto, WinForms selecciona sola la primera fila al asignar el
            // DataSource y precarga el formulario con ese registro.
            DgvPacientes.ClearSelection();
            DgvPacientes.CurrentCell = null;
            DgvPacientes.SelectionChanged += DgvPacientes_SelectionChanged;
        }
    }

    private void DgvPacientes_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || DgvPacientes.Rows[e.RowIndex].DataBoundItem is not PacienteFila fila)
            return;

        string columna = DgvPacientes.Columns[e.ColumnIndex].Name;
        if (columna == "colAusencias" && fila.Ausencias > 0)
        {
            // Resalta a quienes faltaron a algun turno.
            e.CellStyle!.ForeColor = PaletaEstados.Acento(EstadoTurno.Ausente);
            e.CellStyle.Font = new Font(DgvPacientes.Font, FontStyle.Bold);
        }
        else if (columna == "colProximoTurno")
        {
            // El texto puede quedar cortado; el tooltip muestra el completo.
            DgvPacientes.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = fila.ProximoTurno;
        }
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

        var pacienteSeleccionado = ((PacienteFila)DgvPacientes.CurrentRow.DataBoundItem).Paciente;

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

    private void DgvPacientes_SelectionChanged(object? sender, EventArgs e)
    {
        if (DgvPacientes.CurrentRow == null) return;

        var paciente = ((PacienteFila)DgvPacientes.CurrentRow.DataBoundItem).Paciente;

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
