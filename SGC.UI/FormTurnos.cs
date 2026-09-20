using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormTurnos : Form
{
    private readonly PacienteService _pacienteService = new();
    private readonly MedicoService _medicoService = new();
    private readonly HorarioService _horarioService = new();
    private readonly TurnoService _turnoService = new();
    private readonly NotificacionService _notificacionService = new();
    private readonly Usuario? _usuarioActivo;
    private int? _idTurnoSeleccionado = null;

    public FormTurnos(Usuario? usuarioActivo = null)
    {
        _usuarioActivo = usuarioActivo;
        InitializeComponent();
        ConfigurarColumnas();
        CargarCombos();
        DgvTurnos.SelectionChanged += DgvTurnos_SelectionChanged;
        BtnModificar.Click += BtnModificar_Click;
        BtnNuevoTurno.Click += (s, e) =>
        {
            DgvTurnos.SelectionChanged -= DgvTurnos_SelectionChanged;
            DgvTurnos.ClearSelection();
            DgvTurnos.SelectionChanged += DgvTurnos_SelectionChanged;
            LimpiarSeleccion();
        };
        ChkMostrarCancelados.CheckedChanged += (s, e) => CargarGrilla();
        ChkTodosMedicos.CheckedChanged += (s, e) => CargarGrilla();
        ChkFiltrarFecha.CheckedChanged += (s, e) => CargarGrilla();
        ChkFiltrarPaciente.CheckedChanged += (s, e) => CargarGrilla();
        CboPaciente.SelectedIndexChanged += (s, e) =>
        {
            if (ChkFiltrarPaciente.Checked) CargarGrilla();
        };
        CboMedico.SelectedIndexChanged += (s, e) => { ActualizarAgenda(); CargarGrilla(); };
        DtpFecha.ValueChanged += (s, e) => { ActualizarAgenda(); CargarGrilla(); };
        DgvAgenda.CellClick += DgvAgenda_CellClick;
        DgvAgenda.CellDoubleClick += DgvAgenda_CellDoubleClick;

        CargarGrilla();
        ActualizarAgenda();
    }

    private void DgvAgenda_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var horario = (Horario)DgvAgenda.Rows[e.RowIndex].Tag!;
        CboHorario.SelectedValue = horario.Id;
    }

    private void DgvAgenda_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var horario = (Horario)DgvAgenda.Rows[e.RowIndex].Tag!;

        if (CboMedico.SelectedItem != null &&
            // CORRECCION: TurnoService ya NO usa HorarioId del catalogo.
            // Ahora validamos superposicion pasando directamente fecha, hora
            // inicio y duracion (calculada a partir del Horario elegido en la UI).
            _turnoService.HorarioOcupado(
                ((Medico)CboMedico.SelectedItem).Id,
                DateOnly.FromDateTime(DtpFecha.Value),
                horario.HoraInicio,
                (int)(horario.HoraFin - horario.HoraInicio).TotalMinutes))
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Ese horario ya esta ocupado.";
            return;
        }

        CboHorario.SelectedValue = horario.Id;
        BtnAsignar_Click(this, e);
    }

    private void ActualizarAgenda()
    {
        DgvAgenda.Rows.Clear();

        if (CboMedico.SelectedItem == null) return;

        var medico = (Medico)CboMedico.SelectedItem;
        var fecha = DateOnly.FromDateTime(DtpFecha.Value);

        foreach (var horario in _horarioService.ObtenerTodos())
        {
            // CORRECCION: TurnoService.HorarioOcupado() ya NO usa HorarioId.
            // Le pasamos directamente la fecha, la hora inicial del slot y
            // la duracion calculada (hf - hi) del catalogo Horario de la UI.
            int duracion = (int)(horario.HoraFin - horario.HoraInicio).TotalMinutes;
            bool ocupado = _turnoService.HorarioOcupado(medico.Id, fecha, horario.HoraInicio, duracion);
            int fila = DgvAgenda.Rows.Add(horario.Rango, ocupado ? "Ocupado" : "Disponible");

            DgvAgenda.Rows[fila].Tag = horario;

            // Mismo patron de fila coloreada que ya usaste en el TP4 (saldo < 50 = fila roja).
            DgvAgenda.Rows[fila].DefaultCellStyle.BackColor = ocupado
                ? Color.FromArgb(250, 220, 220)
                : Color.FromArgb(220, 245, 225);
        }
    }

    private void DgvTurnos_SelectionChanged(object? sender, EventArgs e)
    {
        if (DgvTurnos.CurrentRow == null) return;

        var turno = (Turno)DgvTurnos.CurrentRow.DataBoundItem;

        _idTurnoSeleccionado = turno.Id;
        CboPaciente.SelectedValue = turno.PacienteId;
        CboMedico.SelectedValue = turno.MedicoId;

        // CORRECCION (RF#02/RF#04): Turno ya NO tiene "HorarioId" del catalogo.
        // Buscamos en el catalogo Horario de la UI el slot que COINCIDA con
        // la hora de inicio del turno y que alcance para la duracion. Si no
        // hay match (turno de duracion personalizada ej 45min) dejamos sin
        // seleccion para que el usuario elija uno o modifique.
        var horariosCombo = (_horarioService.ObtenerTodos()).ToList();
        var horarioCoincidente = horariosCombo.FirstOrDefault(h =>
            h.HoraInicio == turno.HoraInicio &&
            (int)(h.HoraFin - h.HoraInicio).TotalMinutes >= turno.DuracionMinutos);
        if (horarioCoincidente != null)
            CboHorario.SelectedValue = horarioCoincidente.Id;
        else
            CboHorario.SelectedIndex = -1;

        DtpFecha.Value = turno.Fecha.ToDateTime(TimeOnly.MinValue);

        // Paciente y medico de un turno ya asignado no se cambian aca (RF#05
        // solo permite modificar fecha/horario). Se deshabilitan para que
        // quede claro que no son editables mientras hay una fila seleccionada.
        CboPaciente.Enabled = false;
        CboMedico.Enabled = false;
    }

    private void LimpiarSeleccion()
    {
        _idTurnoSeleccionado = null;
        CboPaciente.Enabled = true;
        CboMedico.Enabled = true;
    }

    private void BtnModificar_Click(object? sender, EventArgs e)
    {
        if (_idTurnoSeleccionado == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un turno de la lista para modificar.";
            return;
        }

        if (CboHorario.SelectedItem == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un horario.";
            return;
        }

        try
        {
            var horario = (Horario)CboHorario.SelectedItem;
            var fecha = DateOnly.FromDateTime(DtpFecha.Value);
            // CORRECCION (RF#02/RF#04): TurnoService.ModificarTurno ya NO
            // recibe "Horario", sino (fecha, HoraInicio, DuracionMinutos).
            // Calculamos la duracion a partir del slot Horario elegido en la UI.
            TimeOnly horaInicio = horario.HoraInicio;
            int duracion = (int)(horario.HoraFin - horario.HoraInicio).TotalMinutes;

            _turnoService.ModificarTurno(_idTurnoSeleccionado.Value, fecha, horaInicio, duracion);
            var turno = _turnoService.ObtenerPorId(_idTurnoSeleccionado.Value);

            CargarGrilla();
            LimpiarSeleccion();
            ActualizarAgenda();
            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Turno modificado. " + (turno?.Paciente != null
                ? _notificacionService.AvisarTurno(turno.Paciente, "Reprogramacion", $"{fecha:dd/MM/yyyy} {horario.Rango}")
                : "");
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void ConfigurarColumnas()
    {
        // AutoSizeColumnsMode = Fill reparte el ancho disponible entre las columnas
        // segun su FillWeight (proporcional, no en pixeles fijos). Es necesario
        // desde que FormTurnos se embebe en pnlContenido y ya no tiene un ancho
        // de ventana fijo: con Width fijo, en una pantalla grande las columnas
        // quedaban chicas y sobraba canvas en blanco sin usar.
        DgvTurnos.AutoGenerateColumns = false;
        DgvTurnos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFecha", HeaderText = "Fecha", DataPropertyName = "Fecha", FillWeight = 90 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedico", HeaderText = "Medico", DataPropertyName = "MedicoNombre", FillWeight = 190 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPaciente", HeaderText = "Paciente", DataPropertyName = "PacienteNombre", FillWeight = 170 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHorario", HeaderText = "Horario", DataPropertyName = "HorarioRango", FillWeight = 130 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstado", HeaderText = "Estado", DataPropertyName = "Estado", FillWeight = 100 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedioPago", HeaderText = "Medio de pago", DataPropertyName = "MedioPago", FillWeight = 130 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMonto", HeaderText = "Monto", DataPropertyName = "Monto", FillWeight = 100 });

        DgvAgenda.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvAgenda.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAgendaHorario", HeaderText = "Horario", FillWeight = 60 });
        DgvAgenda.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAgendaEstado", HeaderText = "Estado", FillWeight = 40 });
    }

    private void CargarCombos()
    {
        CboPaciente.DataSource = _pacienteService.ObtenerTodos();
        CboPaciente.DisplayMember = "NombreCompleto";
        CboPaciente.ValueMember = "Id";

        // TODO: si _usuarioActivo.Rol == RolUsuario.Recepcionista, esta
        // recepcionista no deberia ver medicos que no tiene asignados. En vez
        // de _medicoService.ObtenerTodos(), usa _usuarioActivo.MedicosAsignados
        // (ya viene resuelto con los objetos Medico completos desde
        // UsuarioService, no hace falta volver a buscarlos).
        if (_usuarioActivo != null && _usuarioActivo.Rol == RolUsuario.Recepcionista)
        {
            CboMedico.DataSource = _usuarioActivo.MedicosAsignados;
        } else
        {
            CboMedico.DataSource = _medicoService.ObtenerTodos();
        }

            CboMedico.DisplayMember = "NombreCompleto";
        CboMedico.ValueMember = "Id";

        CboHorario.DataSource = _horarioService.ObtenerTodos();
        CboHorario.DisplayMember = "Rango";
        CboHorario.ValueMember = "Id";

        DtpFecha.MinDate = DateTime.Today;
    }

    private void CargarGrilla()
    {
        int? medicoFiltro = null;
        if (!ChkTodosMedicos.Checked && CboMedico.SelectedItem != null)
            medicoFiltro = ((Medico)CboMedico.SelectedItem).Id;

        DateOnly? fechaFiltro = ChkFiltrarFecha.Checked
            ? DateOnly.FromDateTime(DtpFecha.Value)
            : null;

        int? pacienteFiltro = null;
        if (ChkFiltrarPaciente.Checked && CboPaciente.SelectedItem != null)
            pacienteFiltro = ((Paciente)CboPaciente.SelectedItem).Id;

        if (_usuarioActivo != null && _usuarioActivo.Rol == RolUsuario.Recepcionista)
        {
            var medicosAsignadosIds = _usuarioActivo.MedicosAsignados.Select(m => m.Id).ToList();
            var turnosFiltrados = _turnoService.ObtenerTodos(ChkMostrarCancelados.Checked, medicoFiltro, fechaFiltro)
                .Where(t => medicosAsignadosIds.Contains(t.MedicoId))
                .ToList();
            DgvTurnos.SelectionChanged -= DgvTurnos_SelectionChanged;
            DgvTurnos.DataSource = turnosFiltrados;
            DgvTurnos.ClearSelection();
            DgvTurnos.SelectionChanged += DgvTurnos_SelectionChanged;
            return;
        }
        DgvTurnos.SelectionChanged -= DgvTurnos_SelectionChanged;
        var lista = _turnoService.ObtenerTodos(ChkMostrarCancelados.Checked, medicoFiltro, fechaFiltro, pacienteFiltro);
        DgvTurnos.DataSource = lista;
        DgvTurnos.ClearSelection();
        DgvTurnos.SelectionChanged += DgvTurnos_SelectionChanged;

        if (lista.Count == 0 && (medicoFiltro != null || fechaFiltro != null || pacienteFiltro != null))
        {
            LblMensaje.ForeColor = Color.DarkOrange;
            LblMensaje.Text = "No existen resultados asociados a la busqueda realizada.";
        }
    }

    private void BtnAsignar_Click(object sender, EventArgs e)
    {
        if (CboPaciente.SelectedItem == null || CboMedico.SelectedItem == null || CboHorario.SelectedItem == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Debe seleccionar paciente, medico y horario.";
            return;
        }

        try
        {
            var paciente = (Paciente)CboPaciente.SelectedItem;
            var medico = (Medico)CboMedico.SelectedItem;
            var horario = (Horario)CboHorario.SelectedItem;
            var fecha = DateOnly.FromDateTime(DtpFecha.Value);
            // CORRECCION (RF#02/RF#04): TurnoService.AsignarTurno ya NO usa
            // la entidad "Horario" del catalogo. Le pasamos directamente la
            // fecha, hora inicio y duracion (calculada desde el slot elegido).
            TimeOnly horaInicio = horario.HoraInicio;
            int duracion = (int)(horario.HoraFin - horario.HoraInicio).TotalMinutes;

            _turnoService.AsignarTurno(paciente, medico, fecha, horaInicio, duracion);

            CargarGrilla();
            LimpiarSeleccion();
            ActualizarAgenda();
            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Turno Agendado. " + _notificacionService.AvisarTurno(
                paciente, "Confirmacion", $"{fecha:dd/MM/yyyy} {horario.Rango} con {medico.NombreCompleto}");
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void BtnCancelar_Click(object sender, EventArgs e)
    {
        if (DgvTurnos.CurrentRow == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un turno de la lista primero.";
            return;
        }

        var turnoSeleccionado = (Turno)DgvTurnos.CurrentRow.DataBoundItem;

        var respuesta = MessageBox.Show(
            $"Esta seguro que desea cancelar el turno de {turnoSeleccionado.PacienteNombre} el {turnoSeleccionado.Fecha:dd/MM/yyyy}?",
            "Confirmar cancelacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (respuesta != DialogResult.Yes)
            return;

        try
        {
            _turnoService.CancelarTurno(turnoSeleccionado.Id);
            CargarGrilla();
            LimpiarSeleccion();
            ActualizarAgenda();
            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Turno eliminado. " + (turnoSeleccionado.Paciente != null
                ? _notificacionService.AvisarTurno(turnoSeleccionado.Paciente, "Cancelacion", $"{turnoSeleccionado.Fecha:dd/MM/yyyy} {turnoSeleccionado.HorarioRango}")
                : "");
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }

    private void BtnConfirmarAsistencia_Click(object sender, EventArgs e)
    {
        if (DgvTurnos.CurrentRow == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un turno de la lista primero";
            return;
        }

        var turno = (Turno)DgvTurnos.CurrentRow.DataBoundItem;

        var dialogo = new FormConfirmarAsistencia(turno);

        if (dialogo.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        try
        {
            _turnoService.ConfirmarAsistencia(turno.Id, dialogo.Asistio, dialogo.MedioPagoSeleccionado, dialogo.MontoSeleccionado);
            CargarGrilla();
            ActualizarAgenda();
            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Asistencia confirmada.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = ex.Message;
        }
    }
}
