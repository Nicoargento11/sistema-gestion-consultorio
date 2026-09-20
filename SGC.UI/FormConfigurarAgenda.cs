using System.Globalization;
using SGC.Entidades;
using SGC.Logica;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SGC.UI;

public partial class FormConfigurarAgenda : Form
{
    private readonly AgendaMedicoService _service = new();
    private readonly MedicoService _medicoService = new();
    private int? _idSeleccionado = null;

    public FormConfigurarAgenda()
    {
        InitializeComponent();
        ConfigurarColumnas();
        CargarCombos();
        CargarGrilla();
    }

    private void ConfigurarColumnas()
    {
        DgvAgenda.AutoGenerateColumns = false;
        DgvAgenda.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvAgenda.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedico", HeaderText = "Medico", DataPropertyName = "MedicoNombre", FillWeight = 160 });

        // CORRECCI�N 1: Actualizamos los nombres a DiaNombre y HorarioRango
        DgvAgenda.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDia", HeaderText = "Dia", DataPropertyName = "DiaNombre", FillWeight = 90 });
        DgvAgenda.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRango", HeaderText = "Horario", DataPropertyName = "HorarioRango", FillWeight = 90 });
    }

    private void CargarCombos()
    {
        CboMedico.DataSource = _medicoService.ObtenerTodos();
        CboMedico.DisplayMember = "NombreCompleto";
        CboMedico.ValueMember = "Id";

        CboDiaSemana.DataSource = Enum.GetValues(typeof(DayOfWeek))
            .Cast<DayOfWeek>()
            .Select(dia => new { Valor = dia, Texto = CultureInfo.GetCultureInfo("es-AR").DateTimeFormat.GetDayName(dia) })
            .ToList();
        CboDiaSemana.DisplayMember = "Texto";
        CboDiaSemana.ValueMember = "Valor";

        DtpHoraInicio.Value = DateTime.Today.AddHours(8);
        DtpHoraFin.Value = DateTime.Today.AddHours(9);
    }

    private void CargarGrilla()
    {
        DgvAgenda.DataSource = _service.ObtenerTodos();
    }

    private void BtnNuevo_Click(object sender, EventArgs e)
    {
        _idSeleccionado = null;
        CboMedico.SelectedIndex = -1;
        CboDiaSemana.SelectedIndex = -1;
        DtpHoraInicio.Value = DateTime.Today.AddHours(8);
        DtpHoraFin.Value = DateTime.Today.AddHours(9);
    }

    private void BtnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (CboMedico.SelectedValue == null) throw new ArgumentException("Debe seleccionar un medico.");
            if (CboDiaSemana.SelectedValue == null) throw new ArgumentException("Debe seleccionar un dia de la semana.");

            var agenda = new AgendaMedico
            {
                Id = _idSeleccionado ?? 0,
                MedicoId = (int)CboMedico.SelectedValue,
                DiaSemana = (DayOfWeek)CboDiaSemana.SelectedValue,

                // CORRECCION (RF#02): Ahora AgendaMedico usa HoraInicio y HoraFin PROPIOS
                // por dia y medico, ya NO depende del catalogo compartido Horario.
                HoraInicio = TimeOnly.FromDateTime(DtpHoraInicio.Value),
                HoraFin = TimeOnly.FromDateTime(DtpHoraFin.Value)
            };

            if (_idSeleccionado == null)
            {
                _service.Agregar(agenda);
            }
            else
            {
                _service.Modificar(agenda);
            }
            CargarGrilla();

            BtnNuevo_Click(sender, e);

            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Registro de agenda guardado correctamente";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void BtnEliminar_Click(object sender, EventArgs e)
    {
        if (DgvAgenda.CurrentRow == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un registro de agenda de la lista primero.";
            return;
        }
        var agenda = DgvAgenda.CurrentRow.DataBoundItem as AgendaMedico;

        // CORRECCI�N 3: Ajuste de nombres en el mensaje
        var respuesta = MessageBox.Show(
            $"Esta seguro que desea eliminar el registro de agenda del medico {agenda?.MedicoNombre} para el dia {agenda?.DiaNombre} en el horario {agenda?.HorarioRango}?",
            "Confirmar eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (respuesta != DialogResult.Yes) return;

        try
        {
            if (agenda != null)
            {
                _service.EliminarLogico(agenda.Id);
                CargarGrilla();
                BtnNuevo_Click(sender, e);
                LblMensaje.ForeColor = Color.Green;
                LblMensaje.Text = "Registro de agenda eliminado correctamente";
            }
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void DgvAgenda_SelectionChanged(object sender, EventArgs e)
    {
        if (DgvAgenda.CurrentRow == null) return;
        var agenda = DgvAgenda.CurrentRow.DataBoundItem as AgendaMedico;
        _idSeleccionado = agenda?.Id;

        // CORRECCION (RF#02): Ahora leemos HoraInicio/HoraFin DIRECTAMENTE de
        // AgendaMedico, ya NO tenemos propiedad "Horario" (no mas catalogo compartido).
        if (agenda != null)
        {
            CboMedico.SelectedValue = agenda.MedicoId;
            CboDiaSemana.SelectedValue = agenda.DiaSemana;
            DtpHoraInicio.Value = DateTime.Today.Add(agenda.HoraInicio.ToTimeSpan());
            DtpHoraFin.Value = DateTime.Today.Add(agenda.HoraFin.ToTimeSpan());
        }
    }
}