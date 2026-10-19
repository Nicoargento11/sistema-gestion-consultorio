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

    // Vista semanal: se arma en codigo (no en el Designer) para que el
    // disenador visual no la borre. El listado existente pasa a la solapa
    // "Listado" y la grilla semanal queda en "Vista semanal".
    private readonly TabControl TabHorarios = new();
    private readonly TabPage TabListado = new("Listado");
    private readonly TabPage TabSemanal = new("Vista semanal");
    private readonly DataGridView DgvSemanal = new();
    private readonly ComboBox CboMedicoSemanal = new();
    private readonly Label LblResumenSemanal = new();
    private List<AgendaSemanalFila> _resumenSemanal = new();
    private Dictionary<int, int> _minutosPorMedico = new();
    private Font? _fuenteNegritaCache;
    private Font _fuenteNegrita => _fuenteNegritaCache ??= new Font(DgvSemanal.Font, FontStyle.Bold);

    private const string TextoSinHorario = "—";

    public FormConfigurarAgenda()
    {
        InitializeComponent();
        ConfigurarColumnas();
        ConfigurarVistaSemanal();
        CargarCombos();
        CargarGrilla();
        // WinForms selecciona sola la primera fila recien cuando el control
        // se muestra de verdad en pantalla (no en el constructor) - limpiar
        // ahi mismo no sirve, hay que hacerlo en Load, que corre despues.
        Load += (s, e) => BtnNuevo_Click(this, EventArgs.Empty);
    }

    private void ConfigurarColumnas()
    {
        DgvAgenda.AutoGenerateColumns = false;
        DgvAgenda.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvAgenda.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedico", HeaderText = "Medico", DataPropertyName = "MedicoNombre", FillWeight = 160 });

        // Usamos DiaNombre y HorarioRango, ya calculados en la entidad AgendaMedico
        DgvAgenda.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDia", HeaderText = "Dia", DataPropertyName = "DiaNombre", FillWeight = 90 });
        DgvAgenda.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRango", HeaderText = "Horario", DataPropertyName = "HorarioRango", FillWeight = 90 });

        // Columna sin enlace a datos: el valor lo completa CellFormatting con
        // las horas semanales del medico de la fila (suma de todas sus franjas).
        DgvAgenda.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colHorasMedico",
            HeaderText = "Horas semanales por médico",
            FillWeight = 110,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        DgvAgenda.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
        DgvAgenda.CellFormatting += DgvAgenda_CellFormatting;
    }

    private void DgvAgenda_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (DgvAgenda.Columns[e.ColumnIndex].Name != "colHorasMedico") return;
        if (DgvAgenda.Rows[e.RowIndex].DataBoundItem is not AgendaMedico agenda) return;

        e.Value = _minutosPorMedico.TryGetValue(agenda.MedicoId, out var minutos)
            ? AgendaSemanalFila.FormatearMinutos(minutos)
            : "-";
        e.FormattingApplied = true;
    }

    private void ConfigurarVistaSemanal()
    {
        // Solapa "Listado": se pasa a ella la grilla existente (sigue en Fill).
        Controls.Remove(DgvAgenda);
        TabListado.Controls.Add(DgvAgenda);

        // Solapa "Vista semanal": selector de medico arriba, grilla al centro, resumen abajo.
        var pnlSuperior = new Panel { Dock = DockStyle.Top, Height = Px(42), Padding = new Padding(Px(8), Px(6), Px(8), Px(6)) };
        var lblMedicoSemanal = new Label
        {
            Text = "Médico:",
            AutoSize = true,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            Location = new Point(Px(10), Px(12))
        };
        CboMedicoSemanal.DropDownStyle = ComboBoxStyle.DropDownList;
        CboMedicoSemanal.Font = new Font("Segoe UI", 9.5F);
        CboMedicoSemanal.Location = new Point(Px(70), Px(8));
        CboMedicoSemanal.Size = new Size(Px(320), Px(29));
        pnlSuperior.Controls.Add(lblMedicoSemanal);
        pnlSuperior.Controls.Add(CboMedicoSemanal);

        LblResumenSemanal.Dock = DockStyle.Bottom;
        LblResumenSemanal.Height = Px(30);
        LblResumenSemanal.Padding = new Padding(Px(10), 0, Px(10), 0);
        LblResumenSemanal.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        LblResumenSemanal.ForeColor = Color.FromArgb(27, 42, 74);
        LblResumenSemanal.TextAlign = ContentAlignment.MiddleLeft;
        LblResumenSemanal.AutoEllipsis = true;

        ConfigurarGrillaSemanal();

        // Orden de acoplado: la grilla (Fill) primero en la lista, el resto despues.
        TabSemanal.Controls.Add(DgvSemanal);
        TabSemanal.Controls.Add(LblResumenSemanal);
        TabSemanal.Controls.Add(pnlSuperior);

        TabHorarios.Dock = DockStyle.Fill;
        TabHorarios.Font = new Font("Segoe UI", 9.5F);
        TabHorarios.TabPages.Add(TabListado);
        TabHorarios.TabPages.Add(TabSemanal);
        Controls.Add(TabHorarios);
        Controls.SetChildIndex(TabHorarios, 0);

    }

    private int Px(int valor) => EscalaDpi.Px(this, valor);

    private void ConfigurarGrillaSemanal()
    {
        DgvSemanal.Dock = DockStyle.Fill;
        DgvSemanal.AutoGenerateColumns = false;
        DgvSemanal.AllowUserToAddRows = false;
        DgvSemanal.AllowUserToDeleteRows = false;
        DgvSemanal.AllowUserToResizeRows = false;
        DgvSemanal.ReadOnly = true;
        DgvSemanal.MultiSelect = false;
        DgvSemanal.RowHeadersVisible = false;
        DgvSemanal.BackgroundColor = Color.White;
        DgvSemanal.Font = new Font("Segoe UI", 9.5F);
        DgvSemanal.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvSemanal.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvSemanal.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
        DgvSemanal.ColumnHeadersHeight = Px(54);
        DgvSemanal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        DgvSemanal.RowTemplate.Height = Px(38);
        DgvSemanal.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        DgvSemanal.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        DgvSemanal.DefaultCellStyle.Padding = new Padding(Px(2), Px(3), Px(2), Px(3));

        DgvSemanal.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSemMedico", HeaderText = "Médico", FillWeight = 130, MinimumWidth = Px(130) });
        foreach (var dia in AgendaSemanalFila.DiasEnOrden)
        {
            DgvSemanal.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSem" + dia,
                HeaderText = AgendaSemanalFila.NombreDia(dia),
                FillWeight = 75,
                MinimumWidth = Px(80),
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
        }
        DgvSemanal.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colSemHoras",
            HeaderText = "Horas semanales",
            FillWeight = 100,
            MinimumWidth = Px(90),
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });

        DgvSemanal.CellFormatting += DgvSemanal_CellFormatting;
    }

    private void DgvSemanal_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.CellStyle == null || e.RowIndex < 0) return;

        var columna = DgvSemanal.Columns[e.ColumnIndex].Name;
        if (columna == "colSemHoras")
        {
            e.CellStyle.Font = _fuenteNegrita;
        }
        else if (e.ColumnIndex >= 1)
        {
            // Dia con franjas: bloque azul suave (mismo tono que "Confirmado");
            // dia sin atencion: guion gris.
            if (e.Value is string texto && texto != TextoSinHorario && texto.Length > 0)
            {
                e.CellStyle.BackColor = PaletaEstados.Fondo(EstadoTurno.Confirmado);
                e.CellStyle.ForeColor = Color.FromArgb(27, 42, 74);
            }
            else
            {
                e.CellStyle.ForeColor = Color.FromArgb(150, 158, 170);
            }
        }

        // La grilla es solo de lectura: la seleccion no se destaca para que la
        // fila que WinForms marca sola al mostrarse no parezca una eleccion del usuario.
        e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
        e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
    }

    // Recarga el resumen semanal desde la base y repinta combo, grilla y
    // columna "Horas semanales por medico" del listado. Se llama desde
    // CargarGrilla, que corre tras Guardar y Eliminar.
    private void ActualizarVistaSemanal()
    {
        try
        {
            _resumenSemanal = _service.ObtenerResumenSemanal();
        }
        catch (Exception ex)
        {
            _resumenSemanal = new List<AgendaSemanalFila>();
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "No se pudo cargar la vista semanal de horarios: " + ex.Message;
        }
        _minutosPorMedico = _resumenSemanal.ToDictionary(f => f.MedicoId, f => f.MinutosSemanales);

        // Se conserva el medico elegido en la vista semanal entre recargas.
        var idElegido = (CboMedicoSemanal.SelectedItem as Medico)?.Id ?? 0;
        var items = new List<Medico> { new() { Id = 0, Apellido = "Todos", Nombre = "los médicos", Especialidad = "-" } };
        items.AddRange(_resumenSemanal.Select(f => f.Medico));

        CboMedicoSemanal.SelectedIndexChanged -= CboMedicoSemanal_SelectedIndexChangedHandler;
        CboMedicoSemanal.DataSource = items;
        CboMedicoSemanal.DisplayMember = "NombreCompleto";
        CboMedicoSemanal.ValueMember = "Id";
        CboMedicoSemanal.SelectedIndex = Math.Max(0, items.FindIndex(m => m.Id == idElegido));
        CboMedicoSemanal.SelectedIndexChanged += CboMedicoSemanal_SelectedIndexChangedHandler;

        PintarVistaSemanal();
        DgvAgenda.Invalidate();
    }

    private void CboMedicoSemanal_SelectedIndexChangedHandler(object? sender, EventArgs e)
    {
        PintarVistaSemanal();
    }

    private void PintarVistaSemanal()
    {
        var elegido = CboMedicoSemanal.SelectedItem as Medico;
        IEnumerable<AgendaSemanalFila> filas = _resumenSemanal;
        if (elegido != null && elegido.Id > 0)
            filas = filas.Where(f => f.MedicoId == elegido.Id);
        var lista = filas.ToList();

        DgvSemanal.Rows.Clear();
        foreach (var fila in lista)
        {
            var valores = new object[AgendaSemanalFila.DiasEnOrden.Length + 2];
            valores[0] = fila.MedicoNombre;
            for (var i = 0; i < AgendaSemanalFila.DiasEnOrden.Length; i++)
            {
                var texto = fila.TextoDia(AgendaSemanalFila.DiasEnOrden[i]);
                valores[i + 1] = texto.Length == 0 ? TextoSinHorario : texto;
            }
            valores[^1] = fila.HorasSemanalesTexto;
            var indice = DgvSemanal.Rows.Add(valores);
            DgvSemanal.Rows[indice].Tag = fila;
        }
        DgvSemanal.ClearSelection();
        DgvSemanal.CurrentCell = null;

        if (lista.Count == 0)
        {
            LblResumenSemanal.Text = "No hay médicos activos para mostrar.";
        }
        else if (lista.Count == 1)
        {
            LblResumenSemanal.Text = $"{lista[0].MedicoNombre}: {lista[0].HorasSemanalesTexto} semanales.";
        }
        else
        {
            var total = lista.Sum(f => f.MinutosSemanales);
            var sinHorario = lista.Count(f => f.MinutosSemanales == 0);
            LblResumenSemanal.Text = $"{lista.Count} médicos  |  Total entre todos: {AgendaSemanalFila.FormatearMinutos(total)} semanales"
                + (sinHorario > 0 ? $"  |  {sinHorario} sin horarios cargados" : "");
        }
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
        List<AgendaMedico> agendas;
        try
        {
            agendas = _service.ObtenerTodos();
        }
        catch (Exception ex)
        {
            agendas = new List<AgendaMedico>();
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "No se pudo cargar el listado de horarios: " + ex.Message;
        }

        // Se desengancha SelectionChanged mientras se reasigna el DataSource
        // para que no corra con filas a medio cargar.
        DgvAgenda.SelectionChanged -= DgvAgenda_SelectionChanged;
        try
        {
            DgvAgenda.DataSource = agendas;

            // Sin esto, WinForms selecciona sola la primera fila al asignar el
            // DataSource y precarga el formulario con ese registro.
            DgvAgenda.ClearSelection();
            DgvAgenda.CurrentCell = null;
        }
        finally
        {
            DgvAgenda.SelectionChanged += DgvAgenda_SelectionChanged;
        }

        ActualizarVistaSemanal();
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

        // Mensaje usando los nombres actuales de la entidad
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

    private void DgvAgenda_SelectionChanged(object? sender, EventArgs e)
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
