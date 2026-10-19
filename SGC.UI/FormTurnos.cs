using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormTurnos : Form
{
    private readonly PacienteService _pacienteService = new();
    private readonly MedicoService _medicoService = new();
    private readonly HorarioService _horarioService = new();
    private readonly AgendaMedicoService _agendaMedicoService = new();
    private readonly TurnoService _turnoService = new();
    private readonly NotificacionService _notificacionService = new();
    private readonly Usuario? _usuarioActivo;
    private int? _idTurnoSeleccionado = null;

    private const string ColACobrar = "colACobrar";
    private static readonly System.Globalization.CultureInfo CulturaAr = new("es-AR");

    // Barra de contadores (se arma en ConfigurarBarraContadores, no en el disenador).
    private readonly Dictionary<string, Label> _contadores = new();
    private const string ContTotal = "Total";
    private const string ContPendientes = "Pendientes";
    private const string ContConfirmados = "Confirmados";
    private const string ContAtendidos = "Atendidos";
    private const string ContAusentes = "Ausentes";
    private const string ContCancelados = "Cancelados";

    // Duraciones habituales para elegir en CboDuracion. Si un turno ya
    // guardado tiene una duracion distinta (por ejemplo, cargada por otra
    // pantalla), se agrega dinamicamente en DgvTurnos_SelectionChanged.
    private readonly List<int> _duracionesDisponibles = new() { 15, 20, 30, 45, 60, 90 };

    public FormTurnos(Usuario? usuarioActivo = null)
    {
        _usuarioActivo = usuarioActivo;
        InitializeComponent();
        ConfigurarColumnas();
        ConfigurarBarraContadores();
        CargarCombos();

        // Al entrar (por ejemplo la Recepcionista al iniciar sesion), la
        // grilla arranca mostrando los turnos de HOY, no el historial
        // completo - el filtro de fecha ya viene activado.
        DtpFecha.Value = DateTime.Today;
        ChkFiltrarFecha.Checked = true;

        DgvTurnos.SelectionChanged += DgvTurnos_SelectionChanged;
        DgvTurnos.CellFormatting += DgvTurnos_CellFormatting;
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
        CboDuracion.SelectedIndexChanged += (s, e) => ActualizarAgenda();
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

    // Arma la lista de horarios realmente disponibles para el medico y la
    // fecha elegidos: toma el/los bloque(s) de AgendaMedico configurados
    // por el Administrador para ese dia de la semana, y dentro de cada
    // bloque genera slots consecutivos del largo indicado en CboDuracion.
    // Si el medico no tiene ningun bloque cargado para ese dia, la lista
    // queda vacia (no atiende ese dia).
    private void ActualizarAgenda()
    {
        DgvAgenda.Rows.Clear();
        CboHorario.DataSource = null;

        if (CboMedico.SelectedItem == null || CboDuracion.SelectedItem == null) return;

        var medico = (Medico)CboMedico.SelectedItem;
        var fecha = DateOnly.FromDateTime(DtpFecha.Value);
        int duracion = (int)CboDuracion.SelectedItem;

        var bloquesDelDia = _agendaMedicoService.ObtenerPorMedicoYDia(medico.Id, fecha.DayOfWeek);

        var horariosDisponibles = new List<Horario>();
        foreach (var bloque in bloquesDelDia)
            horariosDisponibles.AddRange(_horarioService.GenerarSlotsEnRango(bloque.HoraInicio, bloque.HoraFin, duracion, duracion));

        horariosDisponibles = horariosDisponibles.OrderBy(h => h.HoraInicio).ToList();

        foreach (var horario in horariosDisponibles)
        {
            bool ocupado = _turnoService.HorarioOcupado(medico.Id, fecha, horario.HoraInicio, duracion);
            int fila = DgvAgenda.Rows.Add(horario.Rango, ocupado ? "Ocupado" : "Disponible");

            DgvAgenda.Rows[fila].Tag = horario;
            DgvAgenda.Rows[fila].DefaultCellStyle.BackColor = ocupado
                ? Color.FromArgb(250, 220, 220)
                : Color.FromArgb(220, 245, 225);
        }

        CboHorario.DataSource = horariosDisponibles;
        CboHorario.DisplayMember = "Rango";
        CboHorario.ValueMember = "Id";

        if (bloquesDelDia.Count == 0)
        {
            LblMensaje.ForeColor = Color.DarkOrange;
            LblMensaje.Text = $"{medico.NombreCompleto} no atiende los {AgendaMedico.NombreDia(fecha.DayOfWeek)}.";
        }
    }

    private void DgvTurnos_SelectionChanged(object? sender, EventArgs e)
    {
        if (DgvTurnos.CurrentRow == null) return;

        var turno = (Turno)DgvTurnos.CurrentRow.DataBoundItem;

        _idTurnoSeleccionado = turno.Id;
        CboPaciente.SelectedValue = turno.PacienteId;
        CboMedico.SelectedValue = turno.MedicoId;
        DtpFecha.Value = turno.Fecha.ToDateTime(TimeOnly.MinValue);

        // Si el turno tiene una duracion que no esta en la lista habitual
        // (por ejemplo, cargada antes de que existiera este combo), la
        // sumamos para poder mostrarla seleccionada.
        if (!_duracionesDisponibles.Contains(turno.DuracionMinutos))
        {
            _duracionesDisponibles.Add(turno.DuracionMinutos);
            _duracionesDisponibles.Sort();
            CboDuracion.DataSource = null;
            CboDuracion.DataSource = _duracionesDisponibles;
        }
        CboDuracion.SelectedItem = turno.DuracionMinutos;

        // ActualizarAgenda() ya se disparo con la duracion correcta (evento
        // de CboDuracion), asi que CboHorario ya tiene los slots armados.
        // Buscamos el que coincide con la hora de inicio del turno.
        var horarioActual = (CboHorario.DataSource as List<Horario>)?
            .FirstOrDefault(h => h.HoraInicio == turno.HoraInicio);

        if (horarioActual != null)
            CboHorario.SelectedValue = horarioActual.Id;
        else
            CboHorario.SelectedIndex = -1;

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

        if (CboHorario.SelectedItem == null || CboDuracion.SelectedItem == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un horario y una duracion.";
            return;
        }

        try
        {
            var horario = (Horario)CboHorario.SelectedItem;
            var fecha = DateOnly.FromDateTime(DtpFecha.Value);
            int duracion = (int)CboDuracion.SelectedItem;

            _turnoService.ModificarTurno(_idTurnoSeleccionado.Value, fecha, horario.HoraInicio, duracion);
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
        DgvTurnos.AutoGenerateColumns = false;
        DgvTurnos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvTurnos.ColumnHeadersHeight = 44;
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFecha", HeaderText = "Fecha", DataPropertyName = "Fecha", FillWeight = 90 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedico", HeaderText = "Medico", DataPropertyName = "MedicoNombre", FillWeight = 170 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPaciente", HeaderText = "Paciente", DataPropertyName = "PacienteNombre", FillWeight = 170 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHorario", HeaderText = "Horario", DataPropertyName = "HorarioRango", FillWeight = 120 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstado", HeaderText = "Estado", DataPropertyName = "Estado", FillWeight = 100 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedioPago", HeaderText = "Medio de pago", DataPropertyName = "MedioPago", FillWeight = 130 });
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMonto", HeaderText = "Monto", DataPropertyName = "Monto", FillWeight = 100 });

        // Columna calculada (sin DataPropertyName): su valor se arma en
        // DgvTurnos_CellFormatting con la regla de cobro de TurnoService.
        DgvTurnos.Columns.Add(new DataGridViewTextBoxColumn { Name = ColACobrar, HeaderText = "A cobrar (estimado)", FillWeight = 150 });

        // Fila seleccionada: mismo azul que el resto de la app, texto blanco.
        DgvTurnos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 134, 222);
        DgvTurnos.DefaultCellStyle.SelectionForeColor = Color.White;

        DgvAgenda.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvAgenda.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAgendaHorario", HeaderText = "Horario", FillWeight = 50 });
        DgvAgenda.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAgendaEstado", HeaderText = "Estado", FillWeight = 50 });
    }

    private void CargarCombos()
    {
        CboPaciente.DataSource = _pacienteService.ObtenerTodos();
        CboPaciente.DisplayMember = "NombreCompleto";
        CboPaciente.ValueMember = "Id";

        // Una recepcionista solo ve los medicos que tiene asignados.
        if (_usuarioActivo != null && _usuarioActivo.Rol == RolUsuario.Recepcionista)
        {
            CboMedico.DataSource = _usuarioActivo.MedicosAsignados;
        }
        else
        {
            CboMedico.DataSource = _medicoService.ObtenerTodos();
        }

        CboMedico.DisplayMember = "NombreCompleto";
        CboMedico.ValueMember = "Id";

        CboDuracion.DataSource = _duracionesDisponibles;
        CboDuracion.SelectedItem = 30;
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

        bool esRecepcionista = _usuarioActivo != null && _usuarioActivo.Rol == RolUsuario.Recepcionista;
        List<Turno> lista;
        bool huboError = false;

        try
        {
            if (esRecepcionista)
            {
                var medicosAsignadosIds = _usuarioActivo!.MedicosAsignados.Select(m => m.Id).ToList();
                lista = _turnoService.ObtenerTodos(ChkMostrarCancelados.Checked, medicoFiltro, fechaFiltro)
                    .Where(t => medicosAsignadosIds.Contains(t.MedicoId))
                    .ToList();
            }
            else
            {
                lista = _turnoService.ObtenerTodos(ChkMostrarCancelados.Checked, medicoFiltro, fechaFiltro, pacienteFiltro);
            }
        }
        catch (Exception ex)
        {
            huboError = true;
            lista = new List<Turno>();
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "No se pudieron cargar los turnos: " + ex.Message;
        }

        DgvTurnos.SelectionChanged -= DgvTurnos_SelectionChanged;
        DgvTurnos.DataSource = lista;
        DgvTurnos.ClearSelection();
        DgvTurnos.SelectionChanged += DgvTurnos_SelectionChanged;

        ActualizarContadores(lista);

        if (!huboError && !esRecepcionista && lista.Count == 0 &&
            (medicoFiltro != null || fechaFiltro != null || pacienteFiltro != null))
        {
            LblMensaje.ForeColor = Color.DarkOrange;
            LblMensaje.Text = "No existen resultados asociados a la busqueda realizada.";
        }
    }

    // Arma la franja de contadores entre el formulario y la grilla. Se crea
    // aca (y no en InitializeComponent) para que el disenador no la borre.
    private void ConfigurarBarraContadores()
    {
        var barra = new FlowLayoutPanel
        {
            Name = "pnlContadores",
            Dock = DockStyle.Top,
            Height = 36,
            BackColor = Color.FromArgb(245, 246, 250),
            Padding = new Padding(14, 4, 0, 0),
            WrapContents = false,
            AutoScroll = false
        };

        AgregarContador(barra, ContTotal, Color.FromArgb(27, 42, 74), Color.White);
        AgregarContador(barra, ContPendientes, PaletaEstados.Fondo(EstadoTurno.Pendiente), PaletaEstados.Acento(EstadoTurno.Pendiente));
        AgregarContador(barra, ContConfirmados, PaletaEstados.Fondo(EstadoTurno.Confirmado), PaletaEstados.Acento(EstadoTurno.Confirmado));
        AgregarContador(barra, ContAtendidos, PaletaEstados.Fondo(EstadoTurno.Asistio), Color.FromArgb(28, 120, 66));
        AgregarContador(barra, ContAusentes, PaletaEstados.Fondo(EstadoTurno.Ausente), Color.FromArgb(180, 50, 38));
        AgregarContador(barra, ContCancelados, PaletaEstados.Fondo(EstadoTurno.Cancelado), Color.FromArgb(90, 98, 112));

        // Debajo del formulario y antes del panel de agenda/grilla en el
        // orden de acople: queda a todo el ancho entre ambos.
        Controls.Add(barra);
        Controls.SetChildIndex(barra, Controls.GetChildIndex(pnlFormulario));
        ActualizarContadores(new List<Turno>());
    }

    private void AgregarContador(FlowLayoutPanel barra, string clave, Color fondo, Color texto)
    {
        var etiqueta = new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            BackColor = fondo,
            ForeColor = texto,
            Margin = new Padding(0, 0, 8, 0),
            Padding = new Padding(10, 5, 10, 5),
            Name = "lblContador" + clave
        };
        _contadores[clave] = etiqueta;
        barra.Controls.Add(etiqueta);
    }

    // Cuenta sobre lo que muestra la grilla (con los filtros ya aplicados).
    private void ActualizarContadores(List<Turno> turnos)
    {
        int Contar(EstadoTurno estado) => turnos.Count(t => t.Estado == estado);

        _contadores[ContTotal].Text = $"Total: {turnos.Count}";
        _contadores[ContPendientes].Text = $"Pendientes: {Contar(EstadoTurno.Pendiente)}";
        _contadores[ContConfirmados].Text = $"Confirmados: {Contar(EstadoTurno.Confirmado)}";
        _contadores[ContAtendidos].Text = $"Atendidos: {Contar(EstadoTurno.Asistio)}";
        _contadores[ContAusentes].Text = $"Ausentes: {Contar(EstadoTurno.Ausente)}";
        _contadores[ContCancelados].Text = $"Cancelados: {Contar(EstadoTurno.Cancelado)}";
    }

    // Color de fila por estado, texto de estado con acento y monto estimado.
    private void DgvTurnos_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= DgvTurnos.Rows.Count) return;
        if (DgvTurnos.Rows[e.RowIndex].DataBoundItem is not Turno turno) return;

        if (e.CellStyle == null) return;

        e.CellStyle.BackColor = PaletaEstados.Fondo(turno.Estado);
        e.CellStyle.ForeColor = turno.Estado == EstadoTurno.Cancelado
            ? Color.FromArgb(110, 120, 135)
            : Color.FromArgb(30, 35, 45);

        string columna = DgvTurnos.Columns[e.ColumnIndex].Name;

        if (columna == "colEstado")
        {
            e.Value = PaletaEstados.Texto(turno.Estado);
            e.FormattingApplied = true;
        }
        else if (columna == ColACobrar)
        {
            // Solo tiene sentido estimar lo que todavia no se cobro: para un
            // turno ya atendido manda el Monto real, y ausente/cancelado no cobra.
            bool pendienteDeCobro = turno.Estado == EstadoTurno.Pendiente || turno.Estado == EstadoTurno.Confirmado;
            e.Value = pendienteDeCobro
                ? TurnoService.CalcularMontoEstimado(turno).ToString("C2", CulturaAr)
                : "-";
            e.FormattingApplied = true;
        }
    }

    private void BtnAsignar_Click(object sender, EventArgs e)
    {
        if (CboPaciente.SelectedItem == null || CboMedico.SelectedItem == null ||
            CboHorario.SelectedItem == null || CboDuracion.SelectedItem == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Debe seleccionar paciente, medico, duracion y horario.";
            return;
        }

        try
        {
            var paciente = (Paciente)CboPaciente.SelectedItem;
            var medico = (Medico)CboMedico.SelectedItem;
            var horario = (Horario)CboHorario.SelectedItem;
            var fecha = DateOnly.FromDateTime(DtpFecha.Value);
            int duracion = (int)CboDuracion.SelectedItem;

            _turnoService.AsignarTurno(paciente, medico, fecha, horario.HoraInicio, duracion);

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
