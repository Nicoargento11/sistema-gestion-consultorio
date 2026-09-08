using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormHorarios : Form
{
    private readonly MedicoService _medicoService = new();
    private readonly PacienteService _pacienteService = new();
    private readonly HorarioService _horarioService = new();
    private readonly AgendaService _agendaService = new();
    private readonly TurnoService _turnoService = new();
    private readonly NotificacionService _notificacionService = new();

    private int? _idSeleccionado;

    public FormHorarios()
    {
        InitializeComponent();
        ConfigurarColumnas();
        CargarCombos();
        CboDia.Format += (s, e) =>
        {
            if (e.ListItem is DayOfWeek dia)
                e.Value = AgendaMedico.NombreDia(dia);
        };

        BtnNuevo.Click += (s, e) => LimpiarFormulario();
        BtnGuardar.Click += BtnGuardar_Click;
        BtnEliminar.Click += BtnEliminar_Click;
        BtnConsultar.Click += (s, e) => Consultar();
        DgvHorarios.SelectionChanged += DgvHorarios_SelectionChanged;
        AcceptButton = BtnGuardar;

        CargarGrilla(_agendaService.Consultar());
    }

    private void ConfigurarColumnas()
    {
        DgvHorarios.AutoGenerateColumns = false;
        DgvHorarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvHorarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedico", HeaderText = "Medico", DataPropertyName = "MedicoNombre", FillWeight = 40 });
        DgvHorarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDia", HeaderText = "Dia", DataPropertyName = "DiaNombre", FillWeight = 20 });
        DgvHorarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHorario", HeaderText = "Bloque", DataPropertyName = "HorarioRango", FillWeight = 25 });
    }

    private void CargarCombos()
    {
        CboMedico.DataSource = _medicoService.ObtenerTodos();
        CboMedico.DisplayMember = "NombreCompleto";
        CboMedico.ValueMember = "Id";

        CboDia.DataSource = new[]
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
            DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
        };

        var pacientes = _pacienteService.ObtenerTodos().ToList();
        pacientes.Insert(0, new Paciente { Id = 0, Apellido = "(Todos)", Nombre = "los pacientes" });
        CboPacienteFiltro.DataSource = pacientes;
        CboPacienteFiltro.DisplayMember = "NombreCompleto";
        CboPacienteFiltro.ValueMember = "Id";
    }

    private void CargarGrilla(List<AgendaMedico> filas)
    {
        DgvHorarios.SelectionChanged -= DgvHorarios_SelectionChanged;
        DgvHorarios.DataSource = filas;
        DgvHorarios.ClearSelection();
        DgvHorarios.SelectionChanged += DgvHorarios_SelectionChanged;
        _idSeleccionado = null;
    }

    private void Consultar()
    {
        bool filtroMedico = CboMedico.SelectedItem != null;
        bool filtroFecha = ChkFiltrarFecha.Checked;
        int pacienteId = CboPacienteFiltro.SelectedValue is int id ? id : 0;
        bool filtroPaciente = pacienteId > 0;

        if (!filtroMedico && !filtroFecha && !filtroPaciente)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Debe completar al menos un criterio de busqueda.";
            return;
        }

        int? medicoId = filtroMedico ? ((Medico)CboMedico.SelectedItem!).Id : null;
        DayOfWeek? dia = filtroFecha ? DateOnly.FromDateTime(DtpFechaConsulta.Value).DayOfWeek : null;

        var resultado = _agendaService.Consultar(medicoId, dia);

        if (filtroPaciente)
        {
            var horariosDelPaciente = _turnoService.ObtenerTodos(true, null, null, pacienteId)
                .Select(t => t.HorarioId)
                .Distinct()
                .ToHashSet();

            resultado = resultado.Where(a => horariosDelPaciente.Contains(a.HorarioId)).ToList();
        }

        if (resultado.Count == 0)
        {
            CargarGrilla(resultado);
            LblMensaje.ForeColor = Color.DarkOrange;
            LblMensaje.Text = filtroFecha || filtroPaciente
                ? "No existen resultados asociados a la busqueda realizada."
                : "Ingrese datos validos.";
            return;
        }

        CargarGrilla(resultado);
        LblMensaje.ForeColor = Color.Green;
        LblMensaje.Text = $"{resultado.Count} horario(s) encontrados.";
    }

    private void LimpiarFormulario()
    {
        _idSeleccionado = null;
        DgvHorarios.ClearSelection();
        DtpEntrada.Value = DateTime.Today.AddHours(8);
        DtpSalida.Value = DateTime.Today.AddHours(8).AddMinutes(30);
        LblMensaje.Text = "";
    }

    private void DgvHorarios_SelectionChanged(object? sender, EventArgs e)
    {
        if (DgvHorarios.CurrentRow?.DataBoundItem is not AgendaMedico agenda) return;

        _idSeleccionado = agenda.Id;
        CboMedico.SelectedValue = agenda.MedicoId;
        CboDia.SelectedItem = agenda.DiaSemana;
        if (agenda.Horario != null)
        {
            DtpEntrada.Value = DateTime.Today.Add(agenda.Horario.HoraInicio.ToTimeSpan());
            DtpSalida.Value = DateTime.Today.Add(agenda.Horario.HoraFin.ToTimeSpan());
        }
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (CboMedico.SelectedItem == null || CboDia.SelectedItem == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Debe completar todos los campos obligatorios.";
            return;
        }

        try
        {
            var medico = (Medico)CboMedico.SelectedItem;
            var dia = (DayOfWeek)CboDia.SelectedItem;
            var horario = _horarioService.ObtenerOCrear(
                TimeOnly.FromDateTime(DtpEntrada.Value),
                TimeOnly.FromDateTime(DtpSalida.Value));

            if (_idSeleccionado == null)
            {
                _agendaService.Agregar(medico, dia, horario);
                LblMensaje.ForeColor = Color.Green;
                LblMensaje.Text = "Horario guardado. " + _notificacionService.AvisarMedico(medico, "Alta de horario", $"{AgendaMedico.NombreDia(dia)} {horario.Rango}");
            }
            else
            {
                _agendaService.Modificar(_idSeleccionado.Value, medico, dia, horario);
                LblMensaje.ForeColor = Color.Green;
                LblMensaje.Text = "Horario actualizado.";
            }

            CargarGrilla(_agendaService.Consultar(medico.Id));
            _idSeleccionado = null;
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void BtnEliminar_Click(object? sender, EventArgs e)
    {
        if (_idSeleccionado == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un horario.";
            return;
        }

        var agenda = _agendaService.ObtenerPorId(_idSeleccionado.Value);
        if (agenda == null) return;

        var confirmar = MessageBox.Show(
            $"Eliminar el bloque {agenda.HorarioRango} del {agenda.DiaNombre} para {agenda.MedicoNombre}?",
            "Confirmar",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmar != DialogResult.Yes) return;

        try
        {
            var turnosAfectados = _turnoService.ObtenerTodos(false, agenda.MedicoId)
                .Where(t => t.HorarioId == agenda.HorarioId && t.Fecha.DayOfWeek == agenda.DiaSemana)
                .ToList();

            foreach (var turno in turnosAfectados)
            {
                _turnoService.CancelarTurno(turno.Id);
                if (turno.Paciente != null)
                {
                    _notificacionService.AvisarTurno(turno.Paciente, "Cancelacion", $"Se libero el horario {agenda.HorarioRango}.");
                }
            }

            _agendaService.EliminarLogico(agenda.Id);
            if (agenda.Medico != null)
            {
                _notificacionService.AvisarMedico(agenda.Medico, "Baja de horario", agenda.HorarioRango);
            }

            CargarGrilla(_agendaService.Consultar(agenda.MedicoId));
            LimpiarFormulario();
            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = turnosAfectados.Count > 0
                ? $"Horario eliminado. Se cancelaron {turnosAfectados.Count} turno(s) asociados."
                : "Horario eliminado correctamente.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }
}
