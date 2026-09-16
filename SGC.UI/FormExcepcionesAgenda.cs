using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormExcepcionesAgenda : Form
{
    private readonly ExcepcionAgendaService _service = new();
    private readonly MedicoService _medicoService = new();
    private int? _idSeleccionado = null;

    public FormExcepcionesAgenda()
    {
        InitializeComponent();
        ConfigurarColumnas();
        CargarCombos();
        CargarGrilla();
        ActualizarVisibilidadPorTipo();
    }

    private void ConfigurarColumnas()
    {
        DgvExcepciones.AutoGenerateColumns = false;
        DgvExcepciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvExcepciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedico", HeaderText = "Medico", DataPropertyName = "MedicoNombre", FillWeight = 180 });
        DgvExcepciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFecha", HeaderText = "Fecha", DataPropertyName = "Fecha", FillWeight = 100 });
        DgvExcepciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDescripcion", HeaderText = "Detalle", DataPropertyName = "DescripcionTexto", FillWeight = 140 });
        DgvExcepciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMotivo", HeaderText = "Motivo", DataPropertyName = "Motivo", FillWeight = 200 });
    }

    private void CargarCombos()
    {
        CboMedico.DataSource = _medicoService.ObtenerTodos();
        CboMedico.DisplayMember = "NombreCompleto";
        CboMedico.ValueMember = "Id";

        // Mismo patron que CboDiaSemana en FormConfigurarAgenda: armamos una
        // lista con Valor/Texto en vez de bindear el enum crudo, para poder
        // mostrar un texto mas legible que TipoExcepcionAgenda.ToString().
        CboTipo.DataSource = new[]
        {
            new { Valor = TipoExcepcionAgenda.DiaCompleto, Texto = "Dia completo" },
            new { Valor = TipoExcepcionAgenda.RangoHorario, Texto = "Rango horario especifico" }
        };
        CboTipo.DisplayMember = "Texto";
        CboTipo.ValueMember = "Valor";

        DtpFecha.MinDate = DateTime.Today;
        DtpHoraInicio.Value = DateTime.Today.AddHours(8);
        DtpHoraFin.Value = DateTime.Today.AddHours(9);
    }

    private void CargarGrilla()
    {
        DgvExcepciones.DataSource = _service.ObtenerTodos();
    }

    private void ActualizarVisibilidadPorTipo()
    {
        if (CboTipo.SelectedValue is TipoExcepcionAgenda tipo)
        {
            bool esRango = tipo == TipoExcepcionAgenda.RangoHorario;
            DtpHoraInicio.Visible = esRango;
            lblHoraInicio.Visible = esRango;
            DtpHoraFin.Visible = esRango;
            lblHoraFin.Visible = esRango;
        }
    }

    private void BtnNuevo_Click(object sender, EventArgs e)
    {
        _idSeleccionado = null;
        CboMedico.SelectedIndex = -1;
        DtpFecha.Value = DateTime.Today;
        CboTipo.SelectedIndex = 0;
        TxtMotivo.Text = "";
        DtpHoraInicio.Value = DateTime.Today.AddHours(8);
        DtpHoraFin.Value = DateTime.Today.AddHours(9);
        ActualizarVisibilidadPorTipo();
    }

    private void BtnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (CboMedico.SelectedValue == null) throw new ArgumentException("Debe seleccionar un medico.");
            if (CboTipo.SelectedValue == null) throw new ArgumentException("Debe seleccionar un tipo de excepcion.");

            var tipo = (TipoExcepcionAgenda)CboTipo.SelectedValue;

            var excepcion = new ExcepcionAgenda
            {
                Id = _idSeleccionado ?? 0,
                MedicoId = (int)CboMedico.SelectedValue,
                Fecha = DateOnly.FromDateTime(DtpFecha.Value),
                Tipo = tipo,
                HoraInicio = tipo == TipoExcepcionAgenda.RangoHorario ? TimeOnly.FromDateTime(DtpHoraInicio.Value) : null,
                HoraFin = tipo == TipoExcepcionAgenda.RangoHorario ? TimeOnly.FromDateTime(DtpHoraFin.Value) : null,
                Motivo = TxtMotivo.Text
            };

            var turnosCancelados = new List<Turno>();
            if (_idSeleccionado == null)
            {
                turnosCancelados = _service.Agregar(excepcion);
            }
            else
            {
                _service.Modificar(excepcion);
            }

            CargarGrilla();
            BtnNuevo_Click(sender, e);

            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Excepcion guardada correctamente." + (turnosCancelados.Count > 0
                ? $" Se cancelaron {turnosCancelados.Count} turno(s) que coincidian con esta ausencia."
                : "");
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void BtnEliminar_Click(object sender, EventArgs e)
    {
        if (DgvExcepciones.CurrentRow == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione una excepcion de la lista primero.";
            return;
        }

        var excepcion = (ExcepcionAgenda)DgvExcepciones.CurrentRow.DataBoundItem;

        var respuesta = MessageBox.Show(
            $"Esta seguro que desea eliminar la excepcion del medico {excepcion.MedicoNombre} para el {excepcion.Fecha:dd/MM/yyyy}?",
            "Confirmar eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (respuesta != DialogResult.Yes) return;

        try
        {
            _service.EliminarLogico(excepcion.Id);
            CargarGrilla();
            BtnNuevo_Click(sender, e);
            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Excepcion eliminada correctamente.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void DgvExcepciones_SelectionChanged(object sender, EventArgs e)
    {
        if (DgvExcepciones.CurrentRow == null) return;

        var excepcion = (ExcepcionAgenda)DgvExcepciones.CurrentRow.DataBoundItem;
        _idSeleccionado = excepcion.Id;

        CboMedico.SelectedValue = excepcion.MedicoId;
        DtpFecha.Value = excepcion.Fecha.ToDateTime(TimeOnly.MinValue);
        CboTipo.SelectedValue = excepcion.Tipo;
        TxtMotivo.Text = excepcion.Motivo;

        if (excepcion.Tipo == TipoExcepcionAgenda.RangoHorario && excepcion.HoraInicio != null && excepcion.HoraFin != null)
        {
            DtpHoraInicio.Value = DateTime.Today.Add(excepcion.HoraInicio.Value.ToTimeSpan());
            DtpHoraFin.Value = DateTime.Today.Add(excepcion.HoraFin.Value.ToTimeSpan());
        }

        ActualizarVisibilidadPorTipo();
    }

    private void CboTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ActualizarVisibilidadPorTipo();
    }
}
