using System.Globalization;
using SGC.Entidades;

namespace SGC.UI;

// Opciones de las pestanias "por anio" del modulo de reportes de Luciano Villordo.
internal sealed record OpcionesVistaAnio(bool ConMes, bool ConGrafico);

// Panel donde se dibujan a mano los graficos nativos (GDI+). Doble buffer para
// que no parpadee al redimensionar; el dibujo lo aporta el formulario.
internal sealed class PanelGrafico : Panel
{
    public Action<Graphics, Rectangle>? Dibujar { get; set; }

    public PanelGrafico()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        BackColor = Color.White;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Dibujar?.Invoke(e.Graphics, ClientRectangle);

        using var lapiz = new Pen(EstiloUi.Borde);
        e.Graphics.DrawRectangle(lapiz, 0, 0, Width - 1, Height - 1);
    }
}

// Modulo de reportes de Luciano Villordo integrado a la pantalla unificada:
//   Pacientes por mes     -> sp_ReportePacientesPorMes      (grafico de barras por mes)
//   Rendimiento de medicos -> sp_ReporteRendimientoMedicos  (solo Administrador)
//   Turnos por obra social -> sp_ReporteTurnosPorObraSocial (grafico de barras horizontales)
// Se conservan sus procedimientos, sus metodos de ReporteService, sus clases
// (ReporteMensualItem, ...) y su adaptacion por rol; solo se adapta el aspecto.
public partial class FormReportes
{
    private List<ReporteMensualItem> _datosMensuales = new();
    private List<ReporteObraSocialItem> _datosObrasSociales = new();
    private List<ReporteMedicoRendimientoItem> _datosMedicos = new();

    // Cantidad maxima de obras sociales que dibuja el grafico (las de mas turnos).
    private const int MaxObrasSocialesEnGrafico = 7;

    private static readonly string[] NombresMeses =
    {
        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
        "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
    };

    // ---------------------------------------------------------------
    // Construccion
    // ---------------------------------------------------------------

    // Adaptacion por rol de Luciano: el Medico solo ve "Pacientes por mes" (de
    // si mismo); la Recepcionista ve ademas "Obras sociales"; el rendimiento de
    // los medicos es solo del Administrador.
    private void ConstruirPestaniasEstadisticas()
    {
        _vistaMensual = CrearVista("PacientesPorMes", "Pacientes por mes", conMedico: true, etiquetaOpcion: null,
            conGrafico: false, limitaUnAnio: false, porAnio: new OpcionesVistaAnio(ConMes: false, ConGrafico: true));
        AgregarColumnasMensual(_vistaMensual.Grilla);
        _vistaMensual.PanelGrafico!.Dibujar = DibujarGraficoBarrasMensual;
        var m = _vistaMensual.Contadores!;
        m.Agregar("Total", EstiloUi.Texto, Color.White);
        m.Agregar("Atendidos", PaletaEstados.Fondo(EstadoTurno.Asistio), Color.FromArgb(28, 120, 66));
        m.Agregar("Ausentes", PaletaEstados.Fondo(EstadoTurno.Ausente), Color.FromArgb(180, 50, 38));
        m.Agregar("Cancelados", PaletaEstados.Fondo(EstadoTurno.Cancelado), Color.FromArgb(90, 98, 112));
        m.Agregar("Pendientes", PaletaEstados.Fondo(EstadoTurno.Pendiente), PaletaEstados.Acento(EstadoTurno.Pendiente));
        if (_usuarioActivo.Rol == RolUsuario.Administrador)
            m.Agregar("Facturacion", Color.FromArgb(240, 230, 247), Color.FromArgb(110, 50, 140));
        PonerContadoresVacios(_vistaMensual, "Total", "Atendidos", "Ausentes", "Cancelados", "Pendientes", "Facturacion");
        TabReportes.TabPages.Add(_vistaMensual.Pagina);

        if (_usuarioActivo.Rol == RolUsuario.Administrador)
        {
            _vistaMedicos = CrearVista("RendimientoMedicos", "Rendimiento de médicos", conMedico: false, etiquetaOpcion: null,
                conGrafico: false, limitaUnAnio: false, porAnio: new OpcionesVistaAnio(ConMes: true, ConGrafico: false));
            AgregarColumnasMedicos(_vistaMedicos.Grilla);
            CargarMeses(_vistaMedicos.CboMes!);
            var r = _vistaMedicos.Contadores!;
            r.Agregar("Medicos", EstiloUi.Texto, Color.White);
            r.Agregar("Turnos", PaletaEstados.Fondo(EstadoTurno.Confirmado), PaletaEstados.Acento(EstadoTurno.Confirmado));
            r.Agregar("Atendidos", PaletaEstados.Fondo(EstadoTurno.Asistio), Color.FromArgb(28, 120, 66));
            r.Agregar("Ausentes", PaletaEstados.Fondo(EstadoTurno.Ausente), Color.FromArgb(180, 50, 38));
            r.Agregar("Recaudado", Color.FromArgb(240, 230, 247), Color.FromArgb(110, 50, 140));
            PonerContadoresVacios(_vistaMedicos, "Medicos", "Turnos", "Atendidos", "Ausentes", "Recaudado");
            TabReportes.TabPages.Add(_vistaMedicos.Pagina);
        }

        if (_usuarioActivo.Rol != RolUsuario.Medico)
        {
            _vistaObrasSociales = CrearVista("ObrasSociales", "Obras sociales", conMedico: false, etiquetaOpcion: null,
                conGrafico: false, limitaUnAnio: false, porAnio: new OpcionesVistaAnio(ConMes: false, ConGrafico: true));
            AgregarColumnasObrasSociales(_vistaObrasSociales.Grilla);
            _vistaObrasSociales.PanelGrafico!.Dibujar = DibujarGraficoBarrasObrasSociales;
            var o = _vistaObrasSociales.Contadores!;
            o.Agregar("ObrasSociales", EstiloUi.Texto, Color.White);
            o.Agregar("Turnos", PaletaEstados.Fondo(EstadoTurno.Confirmado), PaletaEstados.Acento(EstadoTurno.Confirmado));
            if (_usuarioActivo.Rol == RolUsuario.Administrador)
                o.Agregar("Facturacion", Color.FromArgb(240, 230, 247), Color.FromArgb(110, 50, 140));
            PonerContadoresVacios(_vistaObrasSociales, "ObrasSociales", "Turnos", "Facturacion");
            TabReportes.TabPages.Add(_vistaObrasSociales.Pagina);
        }

        // Como en el modulo original, el reporte se recalcula al cambiar el anio
        // o el mes (una vez generado por primera vez).
        foreach (var vista in new[] { _vistaMensual, _vistaMedicos, _vistaObrasSociales })
        {
            if (vista == null) continue;
            vista.Anio!.ValueChanged += (s, e) => RegenerarSiCorresponde(vista);
            if (vista.CboMes != null) vista.CboMes.SelectedIndexChanged += (s, e) => RegenerarSiCorresponde(vista);
        }
    }

    private void RegenerarSiCorresponde(VistaReporte vista)
    {
        if (vista.Generada && vista.BtnGenerar.Enabled) Generar(vista);
    }

    // Las pestanias de estadisticas se calculan al abrirlas por primera vez
    // (el modulo original cargaba sus datos al abrir la pantalla).
    private void GenerarEstadisticasAlAbrir()
    {
        var vista = TodasLasVistas().FirstOrDefault(v => v.Pagina == TabReportes.SelectedTab);
        if (vista?.Anio != null && !vista.Generada && vista.BtnGenerar.Enabled) Generar(vista);
    }

    private NumericUpDown CrearSelectorAnio(string nombre)
    {
        var fuente = new Font("Segoe UI", 10F);
        return new NumericUpDown
        {
            Name = nombre,
            Font = fuente,
            Minimum = 2000,
            Maximum = 2100,
            Value = Math.Clamp(DateTime.Today.Year, 2000, 2100),
            Width = TextRenderer.MeasureText("8888", fuente).Width + SystemInformation.VerticalScrollBarWidth + EscalaDpi.Px(this, 24)
        };
    }

    // Items de texto (no un DataSource): el combo todavia no esta en la jerarquia
    // del formulario y un DataSource no se enlaza sin BindingContext. El indice
    // seleccionado es directamente el numero de mes (0 = todos los meses).
    private void CargarMeses(ComboBox combo)
    {
        combo.Items.Add("Todos los meses");
        for (int i = 0; i < NombresMeses.Length; i++)
            combo.Items.Add($"{i + 1} - {NombresMeses[i]}");

        combo.SelectedIndex = 0;
        AjustarAnchoCombo(combo, 130, 240);
    }

    private static void AgregarColumnasMensual(DataGridView grilla)
    {
        grilla.Columns.Add(Col("colMes", "Mes", nameof(ReporteMensualItem.NombreMes), 110));
        grilla.Columns.Add(Col("colTotalTurnos", "Total de turnos", nameof(ReporteMensualItem.TotalTurnos), 80, "0", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colAtendidos", "Atendidos", nameof(ReporteMensualItem.PacientesAtendidos), 80, "0", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colAusentes", "Ausentes", nameof(ReporteMensualItem.Ausentes), 80, "0", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colCancelados", "Cancelados", nameof(ReporteMensualItem.Cancelados), 80, "0", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colPendientes", "Pend. o conf.", nameof(ReporteMensualItem.PendientesOConfirmados), 80, "0", DataGridViewContentAlignment.MiddleRight));
    }

    private static void AgregarColumnasObrasSociales(DataGridView grilla)
    {
        grilla.Columns.Add(Col("colObraSocial", "Obra social", nameof(ReporteObraSocialItem.ObraSocial), 160));
        grilla.Columns.Add(Col("colCantidadTurnos", "Cantidad de turnos", nameof(ReporteObraSocialItem.CantidadTurnos), 90, "0", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colTotalRecaudado", "Total recaudado", nameof(ReporteObraSocialItem.TotalRecaudado), 110, "C2", DataGridViewContentAlignment.MiddleRight));
    }

    private static void AgregarColumnasMedicos(DataGridView grilla)
    {
        grilla.Columns.Add(Col("colProfesional", "Profesional", nameof(ReporteMedicoRendimientoItem.MedicoNombre), 150));
        grilla.Columns.Add(Col("colEspecialidad", "Especialidad", nameof(ReporteMedicoRendimientoItem.Especialidad), 120));
        grilla.Columns.Add(Col("colTurnos", "Turnos", nameof(ReporteMedicoRendimientoItem.TotalTurnos), 60, "0", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colAtendidos", "Atendidos", nameof(ReporteMedicoRendimientoItem.TurnosAtendidos), 70, "0", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colAusentes", "Ausentes", nameof(ReporteMedicoRendimientoItem.TurnosAusentes), 70, "0", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colCancelados", "Cancelados", nameof(ReporteMedicoRendimientoItem.TurnosCancelados), 70, "0", DataGridViewContentAlignment.MiddleRight));
        grilla.Columns.Add(Col("colRecaudado", "Recaudado", nameof(ReporteMedicoRendimientoItem.TotalRecaudado), 100, "C2", DataGridViewContentAlignment.MiddleRight));
    }

    // ---------------------------------------------------------------
    // Periodo y generacion
    // ---------------------------------------------------------------

    private static (DateOnly Desde, DateOnly Hasta) PeriodoDe(VistaReporte vista)
    {
        if (vista.Anio == null)
            return (DateOnly.FromDateTime(vista.Desde!.Value), DateOnly.FromDateTime(vista.Hasta!.Value));

        int anio = (int)vista.Anio.Value;
        int mes = vista.CboMes?.SelectedIndex ?? 0;
        if (mes > 0)
        {
            var primero = new DateOnly(anio, mes, 1);
            return (primero, primero.AddMonths(1).AddDays(-1));
        }
        return (new DateOnly(anio, 1, 1), new DateOnly(anio, 12, 31));
    }

    private int GenerarEstadisticas(VistaReporte vista)
    {
        int anio = (int)vista.Anio!.Value;
        if (vista == _vistaMensual) return GenerarMensual(vista, anio);
        if (vista == _vistaObrasSociales) return GenerarObrasSociales(vista, anio);
        return GenerarRendimientoMedicos(vista, anio);
    }

    private int GenerarMensual(VistaReporte vista, int anio)
    {
        int? medicoId = MedicoSeleccionado(vista);

        if (_usuarioActivo.Rol == RolUsuario.Recepcionista && medicoId == null)
        {
            // "Todos mis medicos": el procedimiento recibe un solo medico, asi que se
            // consulta cada uno asignado y se suman los meses (la Recepcionista solo
            // ve a sus medicos asignados, igual que en el reporte de turnos).
            _datosMensuales = _usuarioActivo.MedicosAsignados
                .SelectMany(a => _reporteService.ObtenerPacientesPorMes(anio, a.Id))
                .GroupBy(i => i.MesNumero)
                .OrderBy(g => g.Key)
                .Select(g => new ReporteMensualItem
                {
                    MesNumero = g.Key,
                    NombreMes = g.First().NombreMes,
                    TotalTurnos = g.Sum(i => i.TotalTurnos),
                    PacientesAtendidos = g.Sum(i => i.PacientesAtendidos),
                    Ausentes = g.Sum(i => i.Ausentes),
                    Cancelados = g.Sum(i => i.Cancelados),
                    PendientesOConfirmados = g.Sum(i => i.PendientesOConfirmados)
                })
                .ToList();
        }
        else
        {
            _datosMensuales = _reporteService.ObtenerPacientesPorMes(anio, medicoId);
        }

        MostrarDatos(vista, _datosMensuales);

        var c = vista.Contadores!;
        c.Poner("Total", $"Total de turnos: {_datosMensuales.Sum(d => d.TotalTurnos)}");
        c.Poner("Atendidos", $"Pacientes atendidos: {_datosMensuales.Sum(d => d.PacientesAtendidos)}");
        c.Poner("Ausentes", $"Ausentes: {_datosMensuales.Sum(d => d.Ausentes)}");
        c.Poner("Cancelados", $"Cancelados: {_datosMensuales.Sum(d => d.Cancelados)}");
        c.Poner("Pendientes", $"Pendientes o confirmados: {_datosMensuales.Sum(d => d.PendientesOConfirmados)}");
        if (_usuarioActivo.Rol == RolUsuario.Administrador)
        {
            // KPI "Facturacion anual" del modulo original (solo Administrador).
            decimal facturado = _reporteService.ObtenerTurnosPorObraSocial(anio).Sum(d => d.TotalRecaudado);
            c.Poner("Facturacion", $"Facturación anual: {facturado.ToString("C0", CulturaAr)}");
        }
        EstiloUi.AjustarFlujos(vista.FlujoContadores!);

        vista.LblResumen.Text = _datosMensuales.Count == 0 ? "" : $"Año {anio}: {_datosMensuales.Count} {(_datosMensuales.Count == 1 ? "mes" : "meses")} con turnos";
        vista.PanelGrafico!.Invalidate();

        vista.Encabezados = new[] { "Mes", "Total de turnos", "Atendidos", "Ausentes", "Cancelados", "Pendientes o confirmados" };
        vista.FilasExportacion = _datosMensuales.Select(d => new[]
        {
            d.NombreMes,
            d.TotalTurnos.ToString(CulturaAr),
            d.PacientesAtendidos.ToString(CulturaAr),
            d.Ausentes.ToString(CulturaAr),
            d.Cancelados.ToString(CulturaAr),
            d.PendientesOConfirmados.ToString(CulturaAr)
        }).ToList();

        return _datosMensuales.Count;
    }

    private int GenerarObrasSociales(VistaReporte vista, int anio)
    {
        _datosObrasSociales = _reporteService.ObtenerTurnosPorObraSocial(anio);
        MostrarDatos(vista, _datosObrasSociales);

        var c = vista.Contadores!;
        c.Poner("ObrasSociales", $"Obras sociales: {_datosObrasSociales.Count}");
        c.Poner("Turnos", $"Turnos: {_datosObrasSociales.Sum(d => d.CantidadTurnos)}");
        if (_usuarioActivo.Rol == RolUsuario.Administrador)
            c.Poner("Facturacion", $"Facturación anual: {_datosObrasSociales.Sum(d => d.TotalRecaudado).ToString("C0", CulturaAr)}");
        EstiloUi.AjustarFlujos(vista.FlujoContadores!);

        vista.LblResumen.Text = _datosObrasSociales.Count == 0 ? "" : $"Año {anio}: turnos activos agrupados por la obra social del paciente";
        vista.PanelGrafico!.Invalidate();

        vista.Encabezados = new[] { "Obra social", "Cantidad de turnos", "Total recaudado" };
        vista.FilasExportacion = _datosObrasSociales.Select(d => new[]
        {
            d.ObraSocial,
            d.CantidadTurnos.ToString(CulturaAr),
            d.TotalRecaudado.ToString("0.00", CulturaAr)
        }).ToList();

        return _datosObrasSociales.Count;
    }

    private int GenerarRendimientoMedicos(VistaReporte vista, int anio)
    {
        int? mes = vista.CboMes!.SelectedIndex > 0 ? vista.CboMes.SelectedIndex : null;
        _datosMedicos = _reporteService.ObtenerRendimientoMedicos(anio, mes);
        MostrarDatos(vista, _datosMedicos);

        var c = vista.Contadores!;
        c.Poner("Medicos", $"Médicos: {_datosMedicos.Count}");
        c.Poner("Turnos", $"Turnos: {_datosMedicos.Sum(d => d.TotalTurnos)}");
        c.Poner("Atendidos", $"Atendidos: {_datosMedicos.Sum(d => d.TurnosAtendidos)}");
        c.Poner("Ausentes", $"Ausentes: {_datosMedicos.Sum(d => d.TurnosAusentes)}");
        c.Poner("Recaudado", $"Recaudado: {_datosMedicos.Sum(d => d.TotalRecaudado).ToString("C0", CulturaAr)}");
        EstiloUi.AjustarFlujos(vista.FlujoContadores!);

        string periodo = mes.HasValue ? $"{NombresMeses[mes.Value - 1]} de {anio}" : $"año {anio}";
        vista.LblResumen.Text = _datosMedicos.Count == 0 ? "" : $"Rendimiento de los médicos activos en {periodo}";

        vista.Encabezados = new[] { "Profesional", "Especialidad", "Turnos", "Atendidos", "Ausentes", "Cancelados", "Recaudado" };
        vista.FilasExportacion = _datosMedicos.Select(d => new[]
        {
            d.MedicoNombre,
            d.Especialidad,
            d.TotalTurnos.ToString(CulturaAr),
            d.TurnosAtendidos.ToString(CulturaAr),
            d.TurnosAusentes.ToString(CulturaAr),
            d.TurnosCancelados.ToString(CulturaAr),
            d.TotalRecaudado.ToString("0.00", CulturaAr)
        }).ToList();

        return _datosMedicos.Count;
    }

    private static void PonerContadoresVacios(VistaReporte vista, params string[] claves)
    {
        foreach (var clave in claves)
        {
            try { vista.Contadores!.Poner(clave, "-"); }
            catch (KeyNotFoundException) { /* el contador no aplica a este rol */ }
        }
    }

    private void LimpiarEstadisticas(VistaReporte vista)
    {
        if (vista.Anio == null) return;

        if (vista == _vistaMensual) _datosMensuales = new();
        else if (vista == _vistaObrasSociales) _datosObrasSociales = new();
        else if (vista == _vistaMedicos) _datosMedicos = new();

        vista.PanelGrafico?.Invalidate();
    }

    // ---------------------------------------------------------------
    // Graficos nativos (GDI+), a escala de pantalla
    // ---------------------------------------------------------------

    private static readonly Color ColorTotal = Color.FromArgb(176, 184, 196);
    private static readonly Color ColorAtendidos = PaletaEstados.Acento(EstadoTurno.Asistio);

    private const TextFormatFlags FlagsTexto = TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter;

    // Paso "redondo" (1, 2, 5 x 10^k) para que el eje Y tenga como mucho 5 divisiones.
    private static int PasoEje(int maximo)
    {
        int[] bases = { 1, 2, 5 };
        for (int potencia = 1; ; potencia *= 10)
        {
            foreach (int b in bases)
            {
                if (maximo / (double)(b * potencia) <= 5) return b * potencia;
            }
        }
    }

    private static void DibujarMensajeVacio(Graphics g, Rectangle area, string texto)
    {
        using var fuente = new Font("Segoe UI", 10F, FontStyle.Italic);
        TextRenderer.DrawText(g, texto, fuente, area, EstiloUi.Secundario,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
    }

    // Titulo del grafico: puede ocupar varias lineas; devuelve el alto usado.
    private static int DibujarTitulo(Graphics g, Control referencia, Rectangle area, string titulo)
    {
        int margen = EscalaDpi.Px(referencia, 12);
        using var fuente = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        var ancho = Math.Max(1, area.Width - 2 * margen);
        var tam = TextRenderer.MeasureText(g, titulo, fuente, new Size(ancho, 0), TextFormatFlags.WordBreak | TextFormatFlags.NoPadding);
        TextRenderer.DrawText(g, titulo, fuente, new Rectangle(area.Left + margen, area.Top + margen, ancho, tam.Height),
            EstiloUi.Texto, TextFormatFlags.WordBreak | TextFormatFlags.NoPadding);
        return margen + tam.Height;
    }

    private void DibujarGraficoBarrasMensual(Graphics g, Rectangle area)
    {
        var panel = _vistaMensual!.PanelGrafico!;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        if (_datosMensuales.Count == 0)
        {
            DibujarMensajeVacio(g, area, _vistaMensual.Generada ? "No hay registros en el año seleccionado." : "Genere el reporte para ver el gráfico.");
            return;
        }

        int px(int valor) => EscalaDpi.Px(panel, valor);
        int margen = px(12);

        // En ventanas bajas se omite el titulo (la pestania ya dice de que es el grafico)
        // para que el plano conserve alto suficiente.
        bool compacto = area.Height < px(260);
        int yTitulo = compacto ? 0 : DibujarTitulo(g, panel, area, "Evolución de pacientes atendidos vs. total de turnos por mes");

        using var fuenteEtiqueta = new Font("Segoe UI", 8.5F);

        // Leyenda
        int yLeyenda = area.Top + (compacto ? margen : yTitulo + px(6));
        int lado = fuenteEtiqueta.Height - px(4);
        int x = area.Left + margen;
        foreach (var (color, texto) in new[] { (ColorTotal, "Total de turnos"), (ColorAtendidos, "Atendidos") })
        {
            using var pincelLeyenda = new SolidBrush(color);
            g.FillRectangle(pincelLeyenda, x, yLeyenda + (fuenteEtiqueta.Height - lado) / 2, lado, lado);
            x += lado + px(4);
            int ancho = TextRenderer.MeasureText(g, texto, fuenteEtiqueta, Size.Empty, FlagsTexto).Width;
            TextRenderer.DrawText(g, texto, fuenteEtiqueta, new Rectangle(x, yLeyenda, ancho, fuenteEtiqueta.Height), EstiloUi.Texto, FlagsTexto);
            x += ancho + px(14);
        }

        int maxTurnos = _datosMensuales.Max(d => Math.Max(d.TotalTurnos, 1));
        int paso = PasoEje(maxTurnos);
        int maxEje = (int)Math.Ceiling(maxTurnos / (double)paso) * paso;

        int anchoEtiquetaY = TextRenderer.MeasureText(g, maxEje.ToString(CulturaAr), fuenteEtiqueta, Size.Empty, FlagsTexto).Width;
        int izquierda = area.Left + margen + anchoEtiquetaY + px(6);
        int derecha = area.Right - margen;
        int arriba = yLeyenda + fuenteEtiqueta.Height + px(10) + fuenteEtiqueta.Height; // lugar para el numero sobre la barra mas alta
        int abajo = area.Bottom - margen - fuenteEtiqueta.Height - px(6);
        int altoPlano = abajo - arriba;
        int anchoPlano = derecha - izquierda;
        if (altoPlano <= px(20) || anchoPlano <= px(40)) return;

        // Grilla horizontal y valores del eje Y
        using var lapizGrilla = new Pen(EstiloUi.GrillaLinea);
        using var lapizEje = new Pen(EstiloUi.Borde);
        for (int valor = 0; valor <= maxEje; valor += paso)
        {
            int y = abajo - (int)Math.Round((double)valor / maxEje * altoPlano);
            g.DrawLine(valor == 0 ? lapizEje : lapizGrilla, izquierda, y, derecha, y);
            TextRenderer.DrawText(g, valor.ToString(CulturaAr), fuenteEtiqueta,
                new Rectangle(area.Left + margen, y - fuenteEtiqueta.Height / 2, anchoEtiquetaY, fuenteEtiqueta.Height),
                EstiloUi.Secundario, FlagsTexto | TextFormatFlags.Right);
        }

        int cantidad = _datosMensuales.Count;
        float anchoGrupo = (float)anchoPlano / cantidad;
        float anchoBarra = Math.Min(anchoGrupo * 0.36f, px(26));

        using var pincelTotal = new SolidBrush(ColorTotal);
        using var pincelAtendidos = new SolidBrush(ColorAtendidos);

        for (int i = 0; i < cantidad; i++)
        {
            var item = _datosMensuales[i];
            float centro = izquierda + i * anchoGrupo + anchoGrupo / 2;
            float altoTotal = (float)item.TotalTurnos / maxEje * altoPlano;
            float altoAtendidos = (float)item.PacientesAtendidos / maxEje * altoPlano;
            float xTotal = centro - anchoBarra - px(1);
            float xAtendidos = centro + px(1);

            g.FillRectangle(pincelTotal, xTotal, abajo - altoTotal, anchoBarra, altoTotal);
            g.FillRectangle(pincelAtendidos, xAtendidos, abajo - altoAtendidos, anchoBarra, altoAtendidos);

            // Mes abreviado debajo del eje
            string mes = item.NombreMes.Length > 3 ? item.NombreMes[..3] : item.NombreMes;
            int anchoMes = TextRenderer.MeasureText(g, mes, fuenteEtiqueta, Size.Empty, FlagsTexto).Width;
            TextRenderer.DrawText(g, mes, fuenteEtiqueta,
                new Rectangle((int)(centro - anchoMes / 2f), abajo + px(4), anchoMes, fuenteEtiqueta.Height), EstiloUi.Texto, FlagsTexto);

            // Valores sobre las barras: ambos si caben, si no solo los atendidos
            string textoTotal = item.TotalTurnos.ToString(CulturaAr);
            string textoAtendidos = item.PacientesAtendidos.ToString(CulturaAr);
            int anchoT = TextRenderer.MeasureText(g, textoTotal, fuenteEtiqueta, Size.Empty, FlagsTexto).Width;
            int anchoA = TextRenderer.MeasureText(g, textoAtendidos, fuenteEtiqueta, Size.Empty, FlagsTexto).Width;
            bool ambos = anchoT + anchoA + px(4) <= anchoGrupo;

            if (ambos && item.TotalTurnos > 0)
                TextRenderer.DrawText(g, textoTotal, fuenteEtiqueta,
                    new Rectangle((int)(xTotal + anchoBarra / 2 - anchoT / 2f), (int)(abajo - altoTotal) - fuenteEtiqueta.Height - px(1), anchoT, fuenteEtiqueta.Height),
                    EstiloUi.Texto, FlagsTexto);
            if (item.PacientesAtendidos > 0)
                TextRenderer.DrawText(g, textoAtendidos, fuenteEtiqueta,
                    new Rectangle((int)(xAtendidos + anchoBarra / 2 - anchoA / 2f), (int)(abajo - altoAtendidos) - fuenteEtiqueta.Height - px(1), anchoA, fuenteEtiqueta.Height),
                    Color.FromArgb(28, 120, 66), FlagsTexto);
        }
    }

    private void DibujarGraficoBarrasObrasSociales(Graphics g, Rectangle area)
    {
        var panel = _vistaObrasSociales!.PanelGrafico!;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        if (_datosObrasSociales.Count == 0)
        {
            DibujarMensajeVacio(g, area, _vistaObrasSociales.Generada ? "No hay registros de obras sociales en el año seleccionado." : "Genere el reporte para ver el gráfico.");
            return;
        }

        int px(int valor) => EscalaDpi.Px(panel, valor);
        int margen = px(12);

        var items = _datosObrasSociales.Take(MaxObrasSocialesEnGrafico).ToList();
        string titulo = _datosObrasSociales.Count > items.Count
            ? $"Turnos por obra social (las {items.Count} con más turnos)"
            : "Turnos por obra social";
        int yTitulo = area.Height < px(200) ? 0 : DibujarTitulo(g, panel, area, titulo);

        using var fuente = new Font("Segoe UI", 9F);

        // Nombres a la izquierda (hasta el 38% del ancho) y cantidades a la derecha de cada barra.
        int anchoNombres = items.Max(d => TextRenderer.MeasureText(g, d.ObraSocial, fuente, Size.Empty, FlagsTexto).Width);
        anchoNombres = Math.Min(anchoNombres, (int)(area.Width * 0.38));
        int anchoValores = items.Max(d => TextRenderer.MeasureText(g, $"{d.CantidadTurnos} turnos", fuente, Size.Empty, FlagsTexto).Width);

        int izquierda = area.Left + margen + anchoNombres + px(8);
        int anchoMaxBarra = area.Right - margen - anchoValores - px(8) - izquierda;
        if (anchoMaxBarra <= px(20)) return;

        int arriba = area.Top + (yTitulo == 0 ? margen : yTitulo + px(10));
        int disponible = area.Bottom - margen - arriba;
        int altoFila = Math.Min(disponible / items.Count, (int)(fuente.Height * 2.2));
        if (altoFila < fuente.Height) altoFila = fuente.Height;
        int altoBarra = Math.Max(px(6), (int)(altoFila * 0.62));

        int maximo = items.Max(d => Math.Max(d.CantidadTurnos, 1));
        using var pincelBarra = new SolidBrush(EstiloUi.Azul);

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            int y = arriba + i * altoFila;

            TextRenderer.DrawText(g, item.ObraSocial, fuente, new Rectangle(area.Left + margen, y, anchoNombres, altoFila),
                EstiloUi.Texto, FlagsTexto | TextFormatFlags.EndEllipsis);

            float ancho = Math.Max(px(3), (float)item.CantidadTurnos / maximo * anchoMaxBarra);
            g.FillRectangle(pincelBarra, izquierda, y + (altoFila - altoBarra) / 2f, ancho, altoBarra);

            TextRenderer.DrawText(g, $"{item.CantidadTurnos} turnos", fuente,
                new Rectangle((int)(izquierda + ancho) + px(6), y, anchoValores + px(2), altoFila), EstiloUi.Texto, FlagsTexto);
        }
    }
}
