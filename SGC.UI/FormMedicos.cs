using System.Globalization;
using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormMedicos : Form
{
    private readonly MedicoService _service = new();
    private readonly ObraSocialService _obraSocialService = new();
    private static readonly CultureInfo CulturaAr = new("es-AR");
    private int? _idSeleccionado = null;
    public FormMedicos()
    {
        InitializeComponent();
        ConfigurarColumnas();
        CargarCombos();
        CargarGrilla();
        AcceptButton = BtnGuardar;
        // SelectionChanged se engancha dentro de CargarGrilla.
        // WinForms selecciona sola la primera fila recien cuando el control
        // se muestra de verdad en pantalla (no en el constructor) - limpiar
        // ahi mismo no sirve, hay que hacerlo en Load, que corre despues.
        Load += (s, e) => BtnNuevo_Click(this, EventArgs.Empty);
        TxtBuscar.TextChanged += (s, e) => CargarGrilla();

        TxtNombre.KeyPress += FiltrosTexto.SoloLetras;
        TxtApellido.KeyPress += FiltrosTexto.SoloLetras;
        TxtDni.KeyPress += FiltrosTexto.SoloNumeros;
        CboEspecialidad.KeyPress += FiltrosTexto.SoloLetras;
    }

    private void CargarCombos()
    {
        ClbObrasSociales.DataSource = _obraSocialService.ObtenerTodos();
        ClbObrasSociales.DisplayMember = "Nombre";
        ClbObrasSociales.ValueMember = "Id";
    }

    private void ConfigurarColumnas()
    {
        // Se configura aca, en codigo, y no en el Designer, porque el disenador
        // visual de Visual Studio borra las columnas de un DataGridView cada vez
        // que se abre el formulario. Aca es inmune a eso.
        // AutoSizeColumnsMode = Fill reparte el ancho disponible segun FillWeight
        // (proporcional) en vez de pixeles fijos - necesario porque FormMedicos
        // ahora se embebe en pnlContenido y ya no tiene un ancho de ventana fijo.
        DgvMedicos.AutoGenerateColumns = false;
        DgvMedicos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "Id", DataPropertyName = "Id", FillWeight = 30 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNombre", HeaderText = "Nombre", DataPropertyName = "Nombre", FillWeight = 90 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colApellido", HeaderText = "Apellido", DataPropertyName = "Apellido", FillWeight = 90 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDni", HeaderText = "Dni", DataPropertyName = "Dni", FillWeight = 90 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMatricula", HeaderText = "Matrícula", DataPropertyName = "Matricula", FillWeight = 85 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEspecialidad", HeaderText = "Especialidad", DataPropertyName = "Especialidad", FillWeight = 100 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colPrecio",
            HeaderText = "Precio\nparticular",
            DataPropertyName = "PrecioConsultaParticular",
            FillWeight = 95,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", FormatProvider = CulturaAr, Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        // El texto largo se corta con puntos suspensivos; la lista completa
        // sale en el tooltip (CellToolTipTextNeeded).
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colObrasSociales", HeaderText = "Obras sociales\naceptadas", DataPropertyName = "ObrasSocialesTexto", FillWeight = 170 });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colTurnosMes",
            HeaderText = "Turnos del mes",
            DataPropertyName = "TurnosDelMes",
            FillWeight = 80,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        DgvMedicos.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colTurnosHoy",
            HeaderText = "Turnos de hoy",
            DataPropertyName = "TurnosDeHoy",
            FillWeight = 80,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });

        // Con 10 columnas los encabezados largos ("Turnos del mes") se cortan:
        // se permite que envuelvan en dos lineas y se fija un minimo por columna
        // para que DNI y matricula no queden recortados.
        DgvMedicos.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
        DgvMedicos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        DgvMedicos.ColumnHeadersHeight = 68;
        var minimos = new Dictionary<string, int>
        {
            ["colId"] = 30, ["colNombre"] = 60, ["colApellido"] = 60, ["colDni"] = 90, ["colMatricula"] = 90,
            ["colEspecialidad"] = 85, ["colPrecio"] = 100, ["colObrasSociales"] = 120, ["colTurnosMes"] = 80, ["colTurnosHoy"] = 80
        };
        foreach (DataGridViewColumn columna in DgvMedicos.Columns)
            columna.MinimumWidth = minimos.GetValueOrDefault(columna.Name, 55);

        DgvMedicos.CellToolTipTextNeeded += DgvMedicos_CellToolTipTextNeeded;
    }

    private void DgvMedicos_CellToolTipTextNeeded(object? sender, DataGridViewCellToolTipTextNeededEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (DgvMedicos.Columns[e.ColumnIndex].Name != "colObrasSociales") return;
        if (DgvMedicos.Rows[e.RowIndex].DataBoundItem is MedicoFila fila)
            e.ToolTipText = fila.ObrasSocialesTexto;
    }

    private void CargarGrilla()
    {
        List<MedicoFila> medicos;
        try
        {
            medicos = _service.ObtenerListado();
        }
        catch (Exception ex)
        {
            medicos = new List<MedicoFila>();
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "No se pudo cargar el listado de médicos: " + ex.Message;
        }

        var filtro = TxtBuscar.Text?.Trim() ?? "";
        if (!string.IsNullOrWhiteSpace(filtro))
        {
            var f = filtro.ToLower();
            medicos = medicos.Where(m =>
                m.Apellido.ToLower().Contains(f) ||
                m.Nombre.ToLower().Contains(f) ||
                m.Especialidad.ToLower().Contains(f) ||
                m.Matricula.ToLower().Contains(f) ||
                m.Dni.Contains(f)).ToList();
        }

        // Se desengancha SelectionChanged mientras se reasigna el DataSource
        // para que no corra con filas a medio cargar.
        DgvMedicos.SelectionChanged -= DgvMedicos_SelectionChanged_1;
        try
        {
            DgvMedicos.DataSource = medicos;
        }
        finally
        {
            // Sin esto, WinForms selecciona sola la primera fila al asignar el
            // DataSource y precarga el formulario con ese registro - confunde
            // (parece una pantalla vacia que no lo es) y es facil sobrescribir
            // el primer registro de la lista sin querer.
            DgvMedicos.ClearSelection();
            DgvMedicos.CurrentCell = null;
            DgvMedicos.SelectionChanged += DgvMedicos_SelectionChanged_1;
        }
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

        var medicoSeleccionado = ((MedicoFila)DgvMedicos.CurrentRow.DataBoundItem).Medico;

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

        var medico = ((MedicoFila)DgvMedicos.CurrentRow.DataBoundItem).Medico;

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