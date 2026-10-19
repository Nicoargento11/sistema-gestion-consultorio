using System.Globalization;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using SGC.Entidades;
using SGC.Entidades.Reportes;
using SGC.Logica;

namespace SGC.UI;

// Pantalla de reportes y estadisticas. La agregacion vive en stored
// procedures de SQL Server (consumidos via ReporteService); aca solo se
// arman filtros, grilla, resumen, grafico y exportacion a CSV.
//
// Pestanias por rol (se agregan al TabControl solo las permitidas):
//   Administrador : Turnos, Recaudacion, Asistencia, Ocupacion de agenda.
//   Medico        : Turnos, Asistencia, Ocupacion (siempre filtrado a su
//                   propio MedicoId). Recaudacion NO se ofrece porque
//                   sp_ReporteRecaudacion no admite filtrar por un medico.
//   Recepcionista : solo Turnos, solo de los medicos que tiene asignados.
// RNF#07: ningun reporte incluye datos clinicos.
public partial class FormReportes : Form
{
    private static readonly CultureInfo CulturaAr = new("es-AR");
    private static readonly Color ColorAzul = Color.FromArgb(46, 134, 222);
    private static readonly Color ColorVerde = Color.FromArgb(39, 174, 96);
    private static readonly Color ColorFondo = Color.FromArgb(245, 246, 250);

    // Limite del procedimiento de ocupacion (un registro por dia).
    private const int MaxDiasOcupacion = 366;

    private readonly ReporteService _reporteService = new();
    private readonly MedicoService _medicoService = new();
    private readonly Usuario _usuarioActivo;

    private VistaReporte _vistaTurnos = null!;
    private VistaReporte? _vistaRecaudacion;
    private VistaReporte? _vistaAsistencia;
    private VistaReporte? _vistaOcupacion;

    // true si el usuario no puede generar nada (Medico sin MedicoId o
    // Recepcionista sin medicos asignados): los botones Generar quedan
    // deshabilitados.
    private bool _sinAcceso;

    private sealed record OpcionMedico(int? Id, string Nombre);
    private sealed record OpcionEstado(EstadoTurno? Valor, string Texto);
    private sealed record OpcionAgrupacion(AgrupacionRecaudacion Valor, string Texto);

    // Todo lo que compone una pestania, para tratarlas de forma uniforme.
    private sealed class VistaReporte
    {
        public string Clave = "";
        public TabPage Pagina = null!;
        public DateTimePicker Desde = null!;
        public DateTimePicker Hasta = null!;
        public ComboBox? CboMedico;
        public ComboBox? CboOpcion;
        public Button BtnGenerar = null!;
        public Button BtnExportar = null!;
        public DataGridView Grilla = null!;
        public Label LblResumen = null!;
        public Chart? Grafico;
        public bool LimitaUnAnio;
        public string[] Encabezados = Array.Empty<string>();
        public List<string[]> FilasExportacion = new();
        public DateOnly PeriodoDesde;
        public DateOnly PeriodoHasta;
    }

    public FormReportes(Usuario usuarioActivo)
    {
        _usuarioActivo = usuarioActivo ?? throw new ArgumentNullException(nameof(usuarioActivo));
        InitializeComponent();

        // Todo se arma en codigo (no en el Designer) para que el disenador
        // visual de Visual Studio no pueda borrarlo.
        ConstruirPestaniasPorRol();
        CargarCombos();

        // Los rangos por defecto (mes actual) son validos; si el usuario no
        // tiene acceso, Generar nace deshabilitado.
        foreach (var vista in TodasLasVistas())
            vista.BtnGenerar.Enabled = !_sinAcceso;

        TabReportes.SelectedIndexChanged += (s, e) => RevalidarPestaniaActual();
    }

    // ---------------------------------------------------------------
    // Construccion de pestanias
    // ---------------------------------------------------------------

    private void ConstruirPestaniasPorRol()
    {
        _vistaTurnos = CrearVista("Turnos", "Turnos", conMedico: true, etiquetaOpcion: "Estado", conGrafico: false, limitaUnAnio: false);
        AgregarColumnasTurnos(_vistaTurnos.Grilla);
        TabReportes.TabPages.Add(_vistaTurnos.Pagina);

        if (_usuarioActivo.Rol == RolUsuario.Administrador)
        {
            _vistaRecaudacion = CrearVista("Recaudacion", "Recaudación", conMedico: false, etiquetaOpcion: "Agrupar por", conGrafico: true, limitaUnAnio: false);
            AgregarColumnasRecaudacion(_vistaRecaudacion.Grilla);
            TabReportes.TabPages.Add(_vistaRecaudacion.Pagina);
        }

        if (_usuarioActivo.Rol != RolUsuario.Recepcionista)
        {
            _vistaAsistencia = CrearVista("Asistencia", "Asistencia", conMedico: true, etiquetaOpcion: null, conGrafico: false, limitaUnAnio: false);
            AgregarColumnasAsistencia(_vistaAsistencia.Grilla);
            TabReportes.TabPages.Add(_vistaAsistencia.Pagina);

            _vistaOcupacion = CrearVista("Ocupacion", "Ocupación de agenda", conMedico: true, etiquetaOpcion: null, conGrafico: true, limitaUnAnio: true);
            AgregarColumnasOcupacion(_vistaOcupacion.Grilla);
            TabReportes.TabPages.Add(_vistaOcupacion.Pagina);
        }
    }

    private VistaReporte CrearVista(string clave, string titulo, bool conMedico, string? etiquetaOpcion, bool conGrafico, bool limitaUnAnio)
    {
        var vista = new VistaReporte
        {
            Clave = clave,
            LimitaUnAnio = limitaUnAnio,
            Pagina = new TabPage(titulo) { Name = "Tab" + clave, BackColor = ColorFondo }
        };

        var hoy = DateTime.Today;
        var primerDia = new DateTime(hoy.Year, hoy.Month, 1);
        var ultimoDia = primerDia.AddMonths(1).AddDays(-1);

        // --- Panel de filtros ---
        var pnlFiltros = new Panel { Dock = DockStyle.Top, Height = 96, BackColor = ColorFondo };

        int x = 14;
        vista.Desde = CrearSelectorFecha(pnlFiltros, "Desde", x, "Dtp" + clave + "Desde", primerDia);
        x += 134;
        vista.Hasta = CrearSelectorFecha(pnlFiltros, "Hasta", x, "Dtp" + clave + "Hasta", ultimoDia);
        x += 134;

        if (conMedico)
        {
            vista.CboMedico = CrearCombo(pnlFiltros, "Médico", x, 220, "Cbo" + clave + "Medico");
            x += 232;
        }

        if (etiquetaOpcion != null)
        {
            vista.CboOpcion = CrearCombo(pnlFiltros, etiquetaOpcion, x, 170, "Cbo" + clave + "Opcion");
        }

        vista.BtnGenerar = CrearBoton("Generar", ColorVerde, 14, 56, 110, "Btn" + clave + "Generar", negrita: true);
        vista.BtnExportar = CrearBoton("Exportar a CSV", ColorAzul, 132, 56, 150, "Btn" + clave + "Exportar", negrita: false);
        vista.BtnExportar.Enabled = false;
        pnlFiltros.Controls.Add(vista.BtnGenerar);
        pnlFiltros.Controls.Add(vista.BtnExportar);

        // --- Resumen (abajo) ---
        var pnlResumen = new Panel { Dock = DockStyle.Bottom, Height = 34, BackColor = Color.White, Padding = new Padding(14, 0, 14, 0) };
        vista.LblResumen = new Label
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(27, 42, 74),
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true,
            Name = "LblResumen" + clave
        };
        pnlResumen.Controls.Add(vista.LblResumen);

        // --- Grilla ---
        vista.Grilla = new DataGridView
        {
            Name = "Dgv" + clave,
            Dock = DockStyle.Fill,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AutoGenerateColumns = false,
            BackgroundColor = Color.White,
            ColumnHeadersHeight = 34,
            Font = new Font("Segoe UI", 9.5F),
            MultiSelect = false,
            ReadOnly = true,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        // El orden de agregado importa para el docking: primero el control
        // Fill, despues los que se pegan a los bordes.
        vista.Pagina.Controls.Add(vista.Grilla);

        if (conGrafico)
        {
            vista.Grafico = CrearGrafico("Grafico" + clave);
            vista.Pagina.Controls.Add(vista.Grafico);
        }

        vista.Pagina.Controls.Add(pnlResumen);
        vista.Pagina.Controls.Add(pnlFiltros);

        // Eventos de la pestania.
        vista.Desde.ValueChanged += (s, e) => RevalidarVista(vista);
        vista.Hasta.ValueChanged += (s, e) => RevalidarVista(vista);
        vista.BtnGenerar.Click += (s, e) => Generar(vista);
        vista.BtnExportar.Click += (s, e) => ExportarCsv(vista);

        return vista;
    }

    private static DateTimePicker CrearSelectorFecha(Panel padre, string etiqueta, int x, string nombre, DateTime valor)
    {
        padre.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(50, 60, 75),
            Location = new Point(x, 8),
            Text = etiqueta
        });

        var selector = new DateTimePicker
        {
            Name = nombre,
            Font = new Font("Segoe UI", 10F),
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "dd/MM/yyyy",
            MinDate = new DateTime(2000, 1, 1),
            MaxDate = new DateTime(2100, 12, 31),
            Location = new Point(x, 28),
            Size = new Size(122, 25),
            Value = valor
        };
        padre.Controls.Add(selector);
        return selector;
    }

    private static ComboBox CrearCombo(Panel padre, string etiqueta, int x, int ancho, string nombre)
    {
        padre.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(50, 60, 75),
            Location = new Point(x, 8),
            Text = etiqueta
        });

        var combo = new ComboBox
        {
            Name = nombre,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = new Font("Segoe UI", 10F),
            Location = new Point(x, 28),
            Size = new Size(ancho, 25)
        };
        padre.Controls.Add(combo);
        return combo;
    }

    private static Button CrearBoton(string texto, Color color, int x, int y, int ancho, string nombre, bool negrita)
    {
        var boton = new Button
        {
            Name = nombre,
            BackColor = color,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.5F, negrita ? FontStyle.Bold : FontStyle.Regular),
            ForeColor = Color.White,
            Location = new Point(x, y),
            Size = new Size(ancho, 30),
            Text = texto,
            UseVisualStyleBackColor = false
        };
        boton.FlatAppearance.BorderSize = 0;
        return boton;
    }

    private static Chart CrearGrafico(string nombre)
    {
        var grafico = new Chart
        {
            Name = nombre,
            Dock = DockStyle.Bottom,
            Height = 190,
            BackColor = Color.White
        };

        var area = new ChartArea("Area");
        area.AxisX.MajorGrid.Enabled = false;
        area.AxisY.MajorGrid.LineColor = Color.FromArgb(225, 228, 235);
        area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8.5F);
        area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8.5F);
        area.AxisX.Interval = 1;
        grafico.ChartAreas.Add(area);

        grafico.Series.Add(new Series("Serie")
        {
            ChartType = SeriesChartType.Column,
            IsXValueIndexed = true,
            Color = ColorAzul,
            IsValueShownAsLabel = true,
            Font = new Font("Segoe UI", 8.5F)
        });

        grafico.Titles.Add(new Title("", Docking.Top, new Font("Segoe UI", 9.5F, FontStyle.Bold), Color.FromArgb(27, 42, 74)));
        return grafico;
    }

    // ---------------------------------------------------------------
    // Columnas (en codigo, nunca en el Designer)
    // ---------------------------------------------------------------

    private static DataGridViewTextBoxColumn Col(string nombre, string encabezado, string propiedad, float peso, string? formato = null, DataGridViewContentAlignment? alineacion = null)
    {
        var columna = new DataGridViewTextBoxColumn
        {
            Name = nombre,
            HeaderText = encabezado,
            DataPropertyName = propiedad,
            FillWeight = peso
        };

        if (formato != null)
        {
            columna.DefaultCellStyle.Format = formato;
            columna.DefaultCellStyle.FormatProvider = CulturaAr;
        }
        if (alineacion.HasValue) columna.DefaultCellStyle.Alignment = alineacion.Value;
        return columna;
    }

    private static void AgregarColumnasTurnos(DataGridView grilla)
    {
        grilla.Columns.Add(Col("colFecha", "Fecha", nameof(ReporteTurnoFila.Fecha), 80, "dd/MM/yyyy"));
        grilla.Columns.Add(Col("colHora", "Hora", nameof(ReporteTurnoFila.HoraInicio), 55, "HH:mm"));
        grilla.Columns.Add(Col("colDuracion", "Duración (min)", nameof(ReporteTurnoFila.DuracionMinutos), 70, "0", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colPaciente", "Paciente", nameof(ReporteTurnoFila.Paciente), 150));
        grilla.Columns.Add(Col("colDni", "DNI", nameof(ReporteTurnoFila.Dni), 80));
        grilla.Columns.Add(Col("colMedico", "Médico", nameof(ReporteTurnoFila.Medico), 150));
        grilla.Columns.Add(Col("colEstado", "Estado", nameof(ReporteTurnoFila.EstadoNombre), 80));
        grilla.Columns.Add(Col("colMonto", "Monto", nameof(ReporteTurnoFila.Monto), 90, "C2", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colMedioPago", "Medio de pago", nameof(ReporteTurnoFila.MedioPago), 100));
    }

    private static void AgregarColumnasRecaudacion(DataGridView grilla)
    {
        grilla.Columns.Add(Col("colGrupo", "Médico", nameof(ReporteRecaudacionFila.Grupo), 200));
        grilla.Columns.Add(Col("colCantidad", "Cantidad de turnos", nameof(ReporteRecaudacionFila.CantidadTurnos), 90, "0", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colMontoTotal", "Monto total", nameof(ReporteRecaudacionFila.MontoTotal), 110, "C2", DataGridViewContentAlignment.MiddleRight));
    }

    private static void AgregarColumnasAsistencia(DataGridView grilla)
    {
        grilla.Columns.Add(Col("colMedico", "Médico", nameof(ReporteAsistenciaFila.Medico), 180));
        grilla.Columns.Add(Col("colTotal", "Total de turnos", nameof(ReporteAsistenciaFila.TotalTurnos), 80, "0", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colAtendidos", "Atendidos", nameof(ReporteAsistenciaFila.Atendidos), 80, "0", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colAusentes", "Ausentes", nameof(ReporteAsistenciaFila.Ausentes), 80, "0", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colCancelados", "Cancelados", nameof(ReporteAsistenciaFila.Cancelados), 80, "0", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colPorcentaje", "% de asistencia", nameof(ReporteAsistenciaFila.PorcentajeAsistencia), 90, "0.00'%'", DataGridViewContentAlignment.MiddleRight));
    }

    private static void AgregarColumnasOcupacion(DataGridView grilla)
    {
        grilla.Columns.Add(Col("colMedico", "Médico", nameof(ReporteOcupacionFila.Medico), 180));
        grilla.Columns.Add(Col("colConfiguradas", "Horas configuradas", nameof(ReporteOcupacionFila.HorasConfiguradas), 100, "0.00' h'", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colOcupadas", "Horas ocupadas", nameof(ReporteOcupacionFila.HorasOcupadas), 100, "0.00' h'", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colOcupacion", "% de ocupación", nameof(ReporteOcupacionFila.PorcentajeOcupacion), 90, "0.00'%'", DataGridViewContentAlignment.MiddleRight));
    }

    // ---------------------------------------------------------------
    // Carga de combos
    // ---------------------------------------------------------------

    private void CargarCombos()
    {
        var opcionesMedico = ConstruirOpcionesMedico();

        foreach (var vista in TodasLasVistas())
        {
            if (vista.CboMedico == null) continue;

            vista.CboMedico.DataSource = opcionesMedico.ToList();
            vista.CboMedico.DisplayMember = nameof(OpcionMedico.Nombre);
            vista.CboMedico.ValueMember = nameof(OpcionMedico.Id);
            vista.CboMedico.SelectedIndex = 0;

            // El Medico no puede elegir a otro profesional.
            vista.CboMedico.Enabled = _usuarioActivo.Rol != RolUsuario.Medico;
        }

        var estados = new List<OpcionEstado>
        {
            new(null, "Todos"),
            new(EstadoTurno.Pendiente, "Pendiente"),
            new(EstadoTurno.Confirmado, "Confirmado"),
            new(EstadoTurno.Cancelado, "Cancelado"),
            new(EstadoTurno.Asistio, "Asistió"),
            new(EstadoTurno.Ausente, "Ausente")
        };
        _vistaTurnos.CboOpcion!.DataSource = estados;
        _vistaTurnos.CboOpcion.DisplayMember = nameof(OpcionEstado.Texto);
        _vistaTurnos.CboOpcion.ValueMember = nameof(OpcionEstado.Valor);
        _vistaTurnos.CboOpcion.SelectedIndex = 0;

        if (_vistaRecaudacion?.CboOpcion != null)
        {
            var agrupaciones = new List<OpcionAgrupacion>
            {
                new(AgrupacionRecaudacion.Medico, "Médico"),
                new(AgrupacionRecaudacion.MedioPago, "Medio de pago"),
                new(AgrupacionRecaudacion.ObraSocial, "Obra social")
            };
            _vistaRecaudacion.CboOpcion.DataSource = agrupaciones;
            _vistaRecaudacion.CboOpcion.DisplayMember = nameof(OpcionAgrupacion.Texto);
            _vistaRecaudacion.CboOpcion.ValueMember = nameof(OpcionAgrupacion.Valor);
            _vistaRecaudacion.CboOpcion.SelectedIndex = 0;
        }
    }

    // Arma la lista de medicos segun el rol. Si algo impide dar acceso
    // (medico sin MedicoId, recepcionista sin asignaciones) deja el aviso en
    // el mensaje y marca _sinAcceso.
    private List<OpcionMedico> ConstruirOpcionesMedico()
    {
        switch (_usuarioActivo.Rol)
        {
            case RolUsuario.Medico:
            {
                if (_usuarioActivo.MedicoId == null)
                {
                    _sinAcceso = true;
                    MostrarError("Su usuario no está vinculado a un médico, por lo que no puede generar reportes.");
                    return new List<OpcionMedico> { new(null, "(sin médico asignado)") };
                }

                string nombre = _usuarioActivo.Medico?.NombreCompleto ?? "";
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    try
                    {
                        nombre = _medicoService.ObtenerPorId(_usuarioActivo.MedicoId.Value)?.NombreCompleto ?? "";
                    }
                    catch (Exception)
                    {
                        // Solo se pierde el nombre visible; el filtro usa el Id.
                    }
                }
                if (string.IsNullOrWhiteSpace(nombre)) nombre = "Mis datos";
                return new List<OpcionMedico> { new(_usuarioActivo.MedicoId, nombre) };
            }

            case RolUsuario.Recepcionista:
            {
                var asignados = _usuarioActivo.MedicosAsignados;
                if (asignados == null || asignados.Count == 0)
                {
                    _sinAcceso = true;
                    MostrarError("No tiene médicos asignados, por lo que no puede generar reportes. Consulte al administrador.");
                    return new List<OpcionMedico> { new(null, "(sin médicos asignados)") };
                }

                var lista = new List<OpcionMedico> { new(null, "Todos mis médicos") };
                lista.AddRange(asignados.OrderBy(m => m.Apellido).ThenBy(m => m.Nombre).Select(m => new OpcionMedico(m.Id, m.NombreCompleto)));
                return lista;
            }

            default:
            {
                var lista = new List<OpcionMedico> { new(null, "Todos") };
                try
                {
                    lista.AddRange(_medicoService.ObtenerTodos()
                        .OrderBy(m => m.Apellido).ThenBy(m => m.Nombre)
                        .Select(m => new OpcionMedico(m.Id, m.NombreCompleto)));
                }
                catch (Exception ex)
                {
                    MostrarError("No se pudo cargar la lista de médicos. Verifique la conexión con la base de datos. Detalle: " + ex.Message);
                }
                return lista;
            }
        }
    }

    private IEnumerable<VistaReporte> TodasLasVistas()
    {
        yield return _vistaTurnos;
        if (_vistaRecaudacion != null) yield return _vistaRecaudacion;
        if (_vistaAsistencia != null) yield return _vistaAsistencia;
        if (_vistaOcupacion != null) yield return _vistaOcupacion;
    }

    // ---------------------------------------------------------------
    // Validacion de filtros
    // ---------------------------------------------------------------

    private static string? ValidarFiltros(VistaReporte vista)
    {
        var desde = vista.Desde.Value.Date;
        var hasta = vista.Hasta.Value.Date;

        if (desde > hasta)
            return "La fecha desde no puede ser posterior a la fecha hasta.";

        if (vista.LimitaUnAnio && (hasta - desde).Days >= MaxDiasOcupacion)
            return $"El rango de fechas no puede superar los {MaxDiasOcupacion} días.";

        return null;
    }

    private void RevalidarVista(VistaReporte vista)
    {
        string? error = ValidarFiltros(vista);
        vista.BtnGenerar.Enabled = error == null && !_sinAcceso;

        if (error != null) MostrarError(error);
        else if (!_sinAcceso) LimpiarMensaje();
    }

    private void RevalidarPestaniaActual()
    {
        var vista = TodasLasVistas().FirstOrDefault(v => v.Pagina == TabReportes.SelectedTab);
        if (vista != null) RevalidarVista(vista);
    }

    // ---------------------------------------------------------------
    // Generacion
    // ---------------------------------------------------------------

    private void Generar(VistaReporte vista)
    {
        string? error = ValidarFiltros(vista);
        if (error != null)
        {
            MostrarError(error);
            return;
        }
        if (_sinAcceso)
        {
            MostrarError("No puede generar reportes con este usuario.");
            return;
        }

        var desde = DateOnly.FromDateTime(vista.Desde.Value);
        var hasta = DateOnly.FromDateTime(vista.Hasta.Value);

        var cursorAnterior = Cursor;
        try
        {
            Cursor = Cursors.WaitCursor;
            vista.BtnGenerar.Enabled = false;

            int filas;
            if (vista == _vistaTurnos) filas = GenerarTurnos(vista, desde, hasta);
            else if (vista == _vistaRecaudacion) filas = GenerarRecaudacion(vista, desde, hasta);
            else if (vista == _vistaAsistencia) filas = GenerarAsistencia(vista, desde, hasta);
            else filas = GenerarOcupacion(vista, desde, hasta);

            vista.PeriodoDesde = desde;
            vista.PeriodoHasta = hasta;
            vista.BtnExportar.Enabled = filas > 0;

            if (filas == 0)
                MostrarAviso("No hay datos para el periodo seleccionado.");
            else
                MostrarExito($"Reporte generado: {filas} {(filas == 1 ? "fila" : "filas")}.");
        }
        catch (ArgumentException ex)
        {
            LimpiarResultado(vista);
            MostrarError(ex.Message);
        }
        catch (Exception ex)
        {
            LimpiarResultado(vista);
            MostrarError("No se pudo generar el reporte. Verifique la conexión con la base de datos e intente nuevamente. Detalle: " + ex.Message);
        }
        finally
        {
            Cursor = cursorAnterior;
            vista.BtnGenerar.Enabled = ValidarFiltros(vista) == null && !_sinAcceso;
        }
    }

    // Id de medico a filtrar. Para Medico siempre es el propio, sin importar
    // lo que diga el combo (defensa en profundidad).
    private int? MedicoSeleccionado(VistaReporte vista)
    {
        if (_usuarioActivo.Rol == RolUsuario.Medico) return _usuarioActivo.MedicoId;
        return (vista.CboMedico?.SelectedItem as OpcionMedico)?.Id;
    }

    private int GenerarTurnos(VistaReporte vista, DateOnly desde, DateOnly hasta)
    {
        var estado = (vista.CboOpcion!.SelectedItem as OpcionEstado)?.Valor;
        int? medicoId = MedicoSeleccionado(vista);

        List<ReporteTurnoFila> datos;
        if (_usuarioActivo.Rol == RolUsuario.Recepcionista && medicoId == null)
        {
            // "Todos mis medicos": el procedimiento recibe un solo medico, asi
            // que se consulta cada uno asignado y se unen los resultados.
            datos = new List<ReporteTurnoFila>();
            foreach (var asignado in _usuarioActivo.MedicosAsignados)
                datos.AddRange(_reporteService.ObtenerTurnosPorPeriodo(desde, hasta, asignado.Id, estado));
            datos = datos.OrderBy(t => t.Fecha).ThenBy(t => t.HoraInicio).ToList();
        }
        else
        {
            datos = _reporteService.ObtenerTurnosPorPeriodo(desde, hasta, medicoId, estado);
        }

        MostrarDatos(vista, datos);

        int asistieron = datos.Count(t => t.EstadoNombre == "Asistió");
        int ausentes = datos.Count(t => t.EstadoNombre == "Ausente");
        int cancelados = datos.Count(t => t.EstadoNombre == "Cancelado");
        decimal monto = datos.Sum(t => t.Monto ?? 0m);
        vista.LblResumen.Text = datos.Count == 0
            ? ""
            : $"Total: {datos.Count} turnos   |   Asistieron: {asistieron}   |   Ausentes: {ausentes}   |   Cancelados: {cancelados}   |   Monto total: {monto.ToString("C2", CulturaAr)}";

        vista.Encabezados = new[] { "Fecha", "Hora", "Duración (min)", "Paciente", "DNI", "Médico", "Estado", "Monto", "Medio de pago" };
        vista.FilasExportacion = datos.Select(t => new[]
        {
            t.Fecha.ToString("dd/MM/yyyy", CulturaAr),
            t.HoraInicio.ToString("HH:mm", CulturaAr),
            t.DuracionMinutos.ToString(CulturaAr),
            t.Paciente,
            t.Dni,
            t.Medico,
            t.EstadoNombre,
            t.Monto.HasValue ? t.Monto.Value.ToString("0.00", CulturaAr) : "",
            t.MedioPago ?? ""
        }).ToList();

        return datos.Count;
    }

    private int GenerarRecaudacion(VistaReporte vista, DateOnly desde, DateOnly hasta)
    {
        var agrupacion = (vista.CboOpcion!.SelectedItem as OpcionAgrupacion) ?? throw new ArgumentException("Seleccione un criterio de agrupación.");
        var datos = _reporteService.ObtenerRecaudacion(desde, hasta, agrupacion.Valor);

        vista.Grilla.Columns["colGrupo"]!.HeaderText = agrupacion.Texto;
        MostrarDatos(vista, datos);

        decimal total = datos.Sum(r => r.MontoTotal);
        int turnos = datos.Sum(r => r.CantidadTurnos);
        vista.LblResumen.Text = datos.Count == 0
            ? ""
            : $"Monto total: {total.ToString("C2", CulturaAr)}   |   Turnos cobrados: {turnos}   |   Agrupado por: {agrupacion.Texto}";

        ActualizarGrafico(vista, datos.Select(r => r.Grupo).ToList(), datos.Select(r => r.MontoTotal).ToList(),
            $"Recaudación por {agrupacion.Texto.ToLower(CulturaAr)} ($)", "N0");

        vista.Encabezados = new[] { agrupacion.Texto, "Cantidad de turnos", "Monto total" };
        vista.FilasExportacion = datos.Select(r => new[]
        {
            r.Grupo,
            r.CantidadTurnos.ToString(CulturaAr),
            r.MontoTotal.ToString("0.00", CulturaAr)
        }).ToList();

        return datos.Count;
    }

    private int GenerarAsistencia(VistaReporte vista, DateOnly desde, DateOnly hasta)
    {
        var datos = _reporteService.ObtenerAsistencia(desde, hasta, MedicoSeleccionado(vista));
        MostrarDatos(vista, datos);

        int atendidos = datos.Sum(a => a.Atendidos);
        int ausentes = datos.Sum(a => a.Ausentes);
        int total = datos.Sum(a => a.TotalTurnos);
        // Mismo criterio que el procedimiento: atendidos / (atendidos + ausentes).
        decimal promedio = atendidos + ausentes == 0 ? 0m : 100m * atendidos / (atendidos + ausentes);
        vista.LblResumen.Text = datos.Count == 0
            ? ""
            : $"Total de turnos: {total}   |   Atendidos: {atendidos}   |   Ausentes: {ausentes}   |   Promedio de asistencia: {promedio.ToString("0.00", CulturaAr)}%";

        vista.Encabezados = new[] { "Médico", "Total de turnos", "Atendidos", "Ausentes", "Cancelados", "% de asistencia" };
        vista.FilasExportacion = datos.Select(a => new[]
        {
            a.Medico,
            a.TotalTurnos.ToString(CulturaAr),
            a.Atendidos.ToString(CulturaAr),
            a.Ausentes.ToString(CulturaAr),
            a.Cancelados.ToString(CulturaAr),
            a.PorcentajeAsistencia.ToString("0.00", CulturaAr)
        }).ToList();

        return datos.Count;
    }

    private int GenerarOcupacion(VistaReporte vista, DateOnly desde, DateOnly hasta)
    {
        var datos = _reporteService.ObtenerOcupacionAgenda(desde, hasta, MedicoSeleccionado(vista));
        MostrarDatos(vista, datos);

        decimal configuradas = datos.Sum(o => o.HorasConfiguradas);
        decimal ocupadas = datos.Sum(o => o.HorasOcupadas);
        decimal porcentaje = configuradas == 0 ? 0m : 100m * ocupadas / configuradas;
        vista.LblResumen.Text = datos.Count == 0
            ? ""
            : $"Horas configuradas: {configuradas.ToString("0.00", CulturaAr)} h   |   Horas ocupadas: {ocupadas.ToString("0.00", CulturaAr)} h   |   Ocupación global: {porcentaje.ToString("0.00", CulturaAr)}%";

        ActualizarGrafico(vista, datos.Select(o => o.Medico).ToList(), datos.Select(o => o.PorcentajeOcupacion).ToList(),
            "Ocupación de agenda por médico (%)", "0.##");

        vista.Encabezados = new[] { "Médico", "Horas configuradas", "Horas ocupadas", "% de ocupación" };
        vista.FilasExportacion = datos.Select(o => new[]
        {
            o.Medico,
            o.HorasConfiguradas.ToString("0.00", CulturaAr),
            o.HorasOcupadas.ToString("0.00", CulturaAr),
            o.PorcentajeOcupacion.ToString("0.00", CulturaAr)
        }).ToList();

        return datos.Count;
    }

    private static void MostrarDatos<T>(VistaReporte vista, List<T> datos)
    {
        // Reasignar el DataSource dispara SelectionChanged; estas grillas no
        // tienen handler propio, y se limpia la seleccion para que no quede
        // la primera fila resaltada sin que el usuario la haya elegido.
        vista.Grilla.DataSource = null;
        vista.Grilla.DataSource = datos;
        vista.Grilla.ClearSelection();
    }

    private static void ActualizarGrafico(VistaReporte vista, List<string> etiquetas, List<decimal> valores, string titulo, string formatoValor)
    {
        if (vista.Grafico == null) return;

        var serie = vista.Grafico.Series["Serie"];
        serie.Points.Clear();
        serie.LabelFormat = formatoValor;
        vista.Grafico.Titles[0].Text = valores.Count == 0 ? "" : titulo;

        // AddY + AxisLabel: con AddXY de texto todas las barras caian en la
        // misma posicion X y se superponian.
        for (int i = 0; i < valores.Count; i++)
        {
            int indice = serie.Points.AddY(valores[i]);
            serie.Points[indice].AxisLabel = etiquetas[i];
        }
    }

    private static void LimpiarResultado(VistaReporte vista)
    {
        vista.Grilla.DataSource = null;
        vista.LblResumen.Text = "";
        vista.FilasExportacion = new List<string[]>();
        vista.BtnExportar.Enabled = false;

        if (vista.Grafico != null)
        {
            vista.Grafico.Series["Serie"].Points.Clear();
            vista.Grafico.Titles[0].Text = "";
        }
    }

    // ---------------------------------------------------------------
    // Exportacion CSV
    // ---------------------------------------------------------------

    private void ExportarCsv(VistaReporte vista)
    {
        if (vista.FilasExportacion.Count == 0)
        {
            MostrarAviso("No hay datos para exportar. Genere el reporte primero.");
            return;
        }

        using var dialogo = new SaveFileDialog
        {
            Title = "Exportar reporte a CSV",
            Filter = "Archivos CSV (*.csv)|*.csv",
            DefaultExt = "csv",
            AddExtension = true,
            OverwritePrompt = true,
            FileName = $"reporte_{vista.Clave.ToLowerInvariant()}_{vista.PeriodoDesde:yyyyMMdd}_{vista.PeriodoHasta:yyyyMMdd}.csv"
        };

        if (dialogo.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            EscribirCsv(dialogo.FileName, vista.Encabezados, vista.FilasExportacion);
            MostrarExito($"Reporte exportado correctamente a {dialogo.FileName}");
        }
        catch (UnauthorizedAccessException)
        {
            MostrarError("No se pudo exportar: no tiene permisos para escribir en esa ubicación.");
        }
        catch (IOException ex)
        {
            MostrarError("No se pudo exportar el archivo. Verifique que no esté abierto en otro programa. Detalle: " + ex.Message);
        }
        catch (Exception ex)
        {
            MostrarError("Ocurrió un error inesperado al exportar. Detalle: " + ex.Message);
        }
    }

    // UTF-8 con BOM (para que Excel muestre bien los acentos) y separador
    // punto y coma, que es el que usa Excel con configuracion es-AR.
    internal static void EscribirCsv(string ruta, string[] encabezados, List<string[]> filas)
    {
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(";", encabezados.Select(EscaparCsv)));
        foreach (var fila in filas)
            sb.AppendLine(string.Join(";", fila.Select(EscaparCsv)));

        File.WriteAllText(ruta, sb.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
    }

    internal static string EscaparCsv(string? valor)
    {
        valor ??= "";

        // Evita que Excel interprete un texto como formula (CSV injection).
        if (valor.Length > 0 && (valor[0] == '=' || valor[0] == '+' || valor[0] == '@'))
            valor = "'" + valor;

        bool requiereComillas = valor.Contains(';') || valor.Contains('"') || valor.Contains('\n') || valor.Contains('\r');
        if (valor.Contains('"')) valor = valor.Replace("\"", "\"\"");
        return requiereComillas ? "\"" + valor + "\"" : valor;
    }

    // ---------------------------------------------------------------
    // Mensajes
    // ---------------------------------------------------------------

    private void MostrarError(string texto)
    {
        LblMensaje.ForeColor = Color.Red;
        LblMensaje.Text = texto;
    }

    private void MostrarExito(string texto)
    {
        LblMensaje.ForeColor = Color.Green;
        LblMensaje.Text = texto;
    }

    private void MostrarAviso(string texto)
    {
        LblMensaje.ForeColor = Color.DarkOrange;
        LblMensaje.Text = texto;
    }

    private void LimpiarMensaje()
    {
        LblMensaje.Text = "";
    }
}
