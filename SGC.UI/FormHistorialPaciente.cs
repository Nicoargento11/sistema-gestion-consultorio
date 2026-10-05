using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormHistorialPaciente : Form
{
    private readonly Paciente _paciente;
    private readonly ActividadMedicaService _actividadService = new();

    public FormHistorialPaciente(Paciente paciente)
    {
        InitializeComponent();
        _paciente = paciente;

        lblPacienteInfo.Text = $"Paciente: {_paciente.NombreCompleto} | DNI: {_paciente.Dni} | Tel: {_paciente.Telefono}";

        ConfigurarColumnas();
        DgvHistorial.SelectionChanged += DgvHistorial_SelectionChanged;
        BtnCerrar.Click += (s, e) => Close();
        AcceptButton = BtnCerrar;

        CargarHistorial();

        // WinForms selecciona sola la primera fila recien cuando el control
        // se muestra de verdad en pantalla (no en el constructor) - limpiar
        // ahi mismo no sirve, hay que hacerlo en Load, que corre despues.
        Load += (s, e) => CargarHistorial();
    }

    private void ConfigurarColumnas()
    {
        DgvHistorial.AutoGenerateColumns = false;
        DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFecha", HeaderText = "Fecha", DataPropertyName = "FechaStr", Width = 95 });
        DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHorario", HeaderText = "Horario", DataPropertyName = "HorarioStr", Width = 110 });
        DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedico", HeaderText = "Medico Tratante", DataPropertyName = "MedicoNombre", Width = 180 });
        DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTipo", HeaderText = "Tipo de Atencion", DataPropertyName = "TipoActividadNombre", Width = 150 });
        DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMotivo", HeaderText = "Motivo de Consulta", DataPropertyName = "MotivoConsulta", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
    }

    private void CargarHistorial()
    {
        var historial = _actividadService.ObtenerHistorialPorPaciente(_paciente.Id);
        DgvHistorial.DataSource = historial;

        // Sin esto, WinForms selecciona sola la primera fila al asignar el
        // DataSource y muestra el detalle de esa atencion sin que el
        // usuario haya clickeado nada.
        DgvHistorial.ClearSelection();
        DgvHistorial.CurrentCell = null;

        if (historial.Count == 0)
        {
            lblDetalleTitulo.Text = "No se registraron atenciones previas para este paciente.";
        }
        else
        {
            lblDetalleTitulo.Text = "Seleccione una atencion de la lista para ver el detalle.";
        }
        TxtMotivo.Clear();
        TxtDiagnostico.Clear();
        TxtReceta.Clear();
    }

    private void DgvHistorial_SelectionChanged(object? sender, EventArgs e)
    {
        if (DgvHistorial.CurrentRow == null) return;

        var actividad = (ActividadMedica)DgvHistorial.CurrentRow.DataBoundItem;
        lblDetalleTitulo.Text = $"Atencion del {actividad.FechaStr} con {actividad.MedicoNombre} ({actividad.TipoActividadNombre})";
        TxtMotivo.Text = actividad.MotivoConsulta;
        TxtDiagnostico.Text = string.IsNullOrWhiteSpace(actividad.Procedimiento) ? "Sin observaciones adicionales." : actividad.Procedimiento;
        TxtReceta.Text = string.IsNullOrWhiteSpace(actividad.RecetaMedicamentos) ? "Sin prescripcion farmacologica." : actividad.RecetaMedicamentos;
    }
}
