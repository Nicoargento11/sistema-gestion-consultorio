using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormExcepcionesAgenda : Form
{
    private readonly ExcepcionAgendaService _service = new();
    private readonly MedicoService _medicoService = new();
    private int? _idSeleccionado = null;

    // Resumen/alerta sobre la grilla: se crea en codigo (no en el Designer)
    // para que el disenador visual no lo borre.
    private readonly Label LblResumenExcepciones = new();
    private Font? _fuenteNegritaCache;
    private Font _fuenteNegrita => _fuenteNegritaCache ??= new Font(DgvExcepciones.Font, FontStyle.Bold);

    public FormExcepcionesAgenda()
    {
        InitializeComponent();
        CrearResumen();
        ConfigurarColumnas();
        CargarCombos();
        CargarGrilla();
        ActualizarVisibilidadPorTipo();
        // WinForms selecciona sola la primera fila recien cuando el control
        // se muestra de verdad en pantalla (no en el constructor) - limpiar
        // ahi mismo no sirve, hay que hacerlo en Load, que corre despues.
        Load += (s, e) => BtnNuevo_Click(this, EventArgs.Empty);
    }

    private void ConfigurarColumnas()
    {
        DgvExcepciones.AutoGenerateColumns = false;
        DgvExcepciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvExcepciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedico", HeaderText = "Medico", DataPropertyName = "MedicoNombre", FillWeight = 180 });
        DgvExcepciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFecha", HeaderText = "Fecha", DataPropertyName = "Fecha", FillWeight = 100 });
        DgvExcepciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDescripcion", HeaderText = "Detalle", DataPropertyName = "DescripcionTexto", FillWeight = 140 });
        DgvExcepciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMotivo", HeaderText = "Motivo", DataPropertyName = "Motivo", FillWeight = 200 });
        DgvExcepciones.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colEstado",
            HeaderText = "Estado",
            DataPropertyName = "EstadoTexto",
            FillWeight = 80,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        DgvExcepciones.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colAfectados",
            HeaderText = "Turnos afectados",
            DataPropertyName = "TurnosAfectados",
            FillWeight = 90,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });

        // El encabezado "Turnos afectados" puede envolver en dos lineas en pantallas angostas.
        DgvExcepciones.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
        DgvExcepciones.ColumnHeadersHeight = EscalaDpi.Px(this, 48);
        DgvExcepciones.CellFormatting += DgvExcepciones_CellFormatting;
        DgvExcepciones.CellToolTipTextNeeded += DgvExcepciones_CellToolTipTextNeeded;
    }

    private void CrearResumen()
    {
        LblResumenExcepciones.Dock = DockStyle.Top;
        LblResumenExcepciones.Height = EscalaDpi.Px(this, 46);
        LblResumenExcepciones.Padding = new Padding(EscalaDpi.Px(this, 20), 0, EscalaDpi.Px(this, 20), 0);
        LblResumenExcepciones.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        LblResumenExcepciones.TextAlign = ContentAlignment.MiddleLeft;
        Controls.Add(LblResumenExcepciones);
        // Debajo del formulario y encima de la grilla (la grilla es la de Fill).
        Controls.SetChildIndex(LblResumenExcepciones, 1);
    }

    private void DgvExcepciones_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.CellStyle == null || e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (DgvExcepciones.Rows[e.RowIndex].DataBoundItem is not ExcepcionFila fila) return;

        var columna = DgvExcepciones.Columns[e.ColumnIndex].Name;
        if (columna == "colEstado")
        {
            e.CellStyle.BackColor = PaletaEstados.Fondo(fila.Estado);
            e.CellStyle.ForeColor = PaletaEstados.Acento(fila.Estado);
            e.CellStyle.Font = _fuenteNegrita;
        }
        else if (columna == "colAfectados" && fila.Estado != EstadoExcepcion.Pasada && fila.TurnosAfectados > 0)
        {
            // Alerta: una ausencia vigente o proxima que todavia tiene turnos activos encima.
            e.CellStyle.BackColor = PaletaEstados.Fondo(EstadoTurno.Ausente);
            e.CellStyle.ForeColor = PaletaEstados.Acento(EstadoTurno.Ausente);
            e.CellStyle.Font = _fuenteNegrita;
        }
    }

    private void DgvExcepciones_CellToolTipTextNeeded(object? sender, DataGridViewCellToolTipTextNeededEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (DgvExcepciones.Columns[e.ColumnIndex].Name != "colAfectados") return;
        if (DgvExcepciones.Rows[e.RowIndex].DataBoundItem is not ExcepcionFila fila) return;

        e.ToolTipText = fila.TurnosAfectados == 0
            ? "No hay turnos activos del médico que se superpongan con esta excepción."
            : $"{fila.TurnosAfectados} turno(s) activo(s) no cancelado(s) del médico se superponen con esta excepción.";
    }

    private void ActualizarResumen(List<ExcepcionFila> filas)
    {
        if (filas.Count == 0)
        {
            LblResumenExcepciones.ForeColor = Color.FromArgb(70, 90, 120);
            LblResumenExcepciones.Text = "No hay excepciones de agenda cargadas.";
            return;
        }

        var vigentes = filas.Count(f => f.Estado == EstadoExcepcion.Vigente);
        var proximas = filas.Count(f => f.Estado == EstadoExcepcion.Proxima);
        var pasadas = filas.Count - vigentes - proximas;
        var conAfectados = filas.Where(f => f.Estado != EstadoExcepcion.Pasada && f.TurnosAfectados > 0).ToList();

        var texto = $"{filas.Count} excepción(es): {vigentes} vigente(s), {proximas} próxima(s), {pasadas} pasada(s).";
        if (conAfectados.Count > 0)
        {
            texto += $"  Atención: {conAfectados.Sum(f => f.TurnosAfectados)} turno(s) activo(s) se superponen con {conAfectados.Count} excepción(es) vigente(s) o próxima(s).";
            LblResumenExcepciones.ForeColor = PaletaEstados.Acento(EstadoTurno.Ausente);
        }
        else
        {
            LblResumenExcepciones.ForeColor = Color.FromArgb(70, 90, 120);
        }
        LblResumenExcepciones.Text = texto;
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

        DtpHoraInicio.Value = DateTime.Today.AddHours(8);
        DtpHoraFin.Value = DateTime.Today.AddHours(9);
    }

    private void CargarGrilla()
    {
        List<ExcepcionFila> filas;
        try
        {
            filas = _service.ObtenerListado();
        }
        catch (Exception ex)
        {
            filas = new List<ExcepcionFila>();
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "No se pudo cargar el listado de excepciones: " + ex.Message;
        }

        // Se desengancha SelectionChanged mientras se reasigna el DataSource
        // para que no corra con filas a medio cargar.
        DgvExcepciones.SelectionChanged -= DgvExcepciones_SelectionChanged;
        try
        {
            DgvExcepciones.DataSource = filas;

            // Sin esto, WinForms selecciona sola la primera fila al asignar el
            // DataSource y precarga el formulario con ese registro.
            DgvExcepciones.ClearSelection();
            DgvExcepciones.CurrentCell = null;
        }
        finally
        {
            DgvExcepciones.SelectionChanged += DgvExcepciones_SelectionChanged;
        }

        ActualizarResumen(filas);
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

        var excepcion = ((ExcepcionFila)DgvExcepciones.CurrentRow.DataBoundItem).Excepcion;

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

    private void DgvExcepciones_SelectionChanged(object? sender, EventArgs e)
    {
        if (DgvExcepciones.CurrentRow == null) return;

        var excepcion = ((ExcepcionFila)DgvExcepciones.CurrentRow.DataBoundItem).Excepcion;
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
