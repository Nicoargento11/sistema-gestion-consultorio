using System.Drawing.Drawing2D;
using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormReportes : Form
{
    private readonly Usuario _usuarioActivo;
    private readonly ReporteService _reporteService = new();

    private List<ReporteMensualItem> _datosMensuales = new();
    private List<ReporteObraSocialItem> _datosObrasSociales = new();
    private List<ReporteMedicoRendimientoItem> _datosMedicos = new();

    public FormReportes(Usuario usuarioActivo)
    {
        InitializeComponent();
        _usuarioActivo = usuarioActivo;

        ConfigurarVistasPorRol();
        ConfigurarCombos();
        ConfigurarColumnasGrillas();
        AsignarEventosGraficos();

        CargarReportes();
    }

    private void ConfigurarVistasPorRol()
    {
        numAnio.Value = DateTime.Today.Year;

        if (_usuarioActivo.Rol == RolUsuario.Medico)
        {
            lblTitulo.Text = "Mi Desempeno y Estadisticas";
            lblSubtitulo.Text = $"Datos de atencion para el Dr./Dra. {_usuarioActivo.NombreUsuario} (Procedimientos Almacenados)";

            // El medico no necesita ver rendimiento de otros medicos ni obras sociales globales
            tabReportes.TabPages.Remove(tabMedicos);
            tabReportes.TabPages.Remove(tabObrasSociales);
        }
        else if (_usuarioActivo.Rol == RolUsuario.Recepcionista)
        {
            lblTitulo.Text = "Estadisticas de Recepcion";
            lblSubtitulo.Text = "Distribucion de turnos, obras sociales y ausentismo general";

            // La recepcionista no gestiona el rendimiento laboral del medico
            tabReportes.TabPages.Remove(tabMedicos);
        }
        else
        {
            lblTitulo.Text = "Panel de Analitica y Reportes";
            lblSubtitulo.Text = "Informacion global de gestion del consultorio (SQL Server Stored Procedures)";
        }
    }

    private void ConfigurarCombos()
    {
        cmbMes.Items.Clear();
        cmbMes.Items.Add("Todos los meses");
        cmbMes.Items.AddRange(new object[] {
            "1 - Enero", "2 - Febrero", "3 - Marzo", "4 - Abril", "5 - Mayo", "6 - Junio",
            "7 - Julio", "8 - Agosto", "9 - Septiembre", "10 - Octubre", "11 - Noviembre", "12 - Diciembre"
        });
        cmbMes.SelectedIndex = 0;

        btnActualizar.Click += (s, e) => CargarReportes();
        numAnio.ValueChanged += (s, e) => CargarReportes();
        cmbMes.SelectedIndexChanged += (s, e) => CargarReportes();
    }

    private void ConfigurarColumnasGrillas()
    {
        // 1. Grilla Mensual
        dgvMensual.AutoGenerateColumns = false;
        dgvMensual.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvMensual.Columns.Clear();
        dgvMensual.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreMes", HeaderText = "Mes", FillWeight = 25 });
        dgvMensual.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TotalTurnos", HeaderText = "Total Turnos", FillWeight = 15 });
        dgvMensual.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PacientesAtendidos", HeaderText = "Atendidos", FillWeight = 15 });
        dgvMensual.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Ausentes", HeaderText = "Ausentes", FillWeight = 15 });
        dgvMensual.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Cancelados", HeaderText = "Cancelados", FillWeight = 15 });
        dgvMensual.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PendientesOConfirmados", HeaderText = "Pend./Conf.", FillWeight = 15 });

        // 2. Grilla Obras Sociales
        dgvObrasSociales.AutoGenerateColumns = false;
        dgvObrasSociales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvObrasSociales.Columns.Clear();
        dgvObrasSociales.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ObraSocial", HeaderText = "Obra Social", FillWeight = 40 });
        dgvObrasSociales.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CantidadTurnos", HeaderText = "Cantidad Turnos", FillWeight = 30 });
        dgvObrasSociales.Columns.Add(new DataGridViewTextBoxColumn { 
            DataPropertyName = "TotalRecaudado", 
            HeaderText = "Total Recaudado", 
            FillWeight = 30,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
        });

        // 3. Grilla Medicos
        dgvMedicos.AutoGenerateColumns = false;
        dgvMedicos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvMedicos.Columns.Clear();
        dgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MedicoNombre", HeaderText = "Profesional", FillWeight = 30 });
        dgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Especialidad", HeaderText = "Especialidad", FillWeight = 25 });
        dgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TotalTurnos", HeaderText = "Turnos", FillWeight = 11 });
        dgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TurnosAtendidos", HeaderText = "Atendidos", FillWeight = 11 });
        dgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TurnosAusentes", HeaderText = "Ausentes", FillWeight = 11 });
        dgvMedicos.Columns.Add(new DataGridViewTextBoxColumn { 
            DataPropertyName = "TotalRecaudado", 
            HeaderText = "Recaudado", 
            FillWeight = 12,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
        });
    }

    private void CargarReportes()
    {
        try
        {
            int anio = (int)numAnio.Value;
            int? medicoId = _usuarioActivo.Rol == RolUsuario.Medico ? _usuarioActivo.MedicoId : null;
            int? mesSeleccionado = cmbMes.SelectedIndex > 0 ? cmbMes.SelectedIndex : null;

            // 1. Mensual
            _datosMensuales = _reporteService.ObtenerPacientesPorMes(anio, medicoId);
            dgvMensual.DataSource = null;
            dgvMensual.DataSource = _datosMensuales;

            // 2. Obras Sociales (si esta habilitado)
            if (tabReportes.TabPages.Contains(tabObrasSociales))
            {
                _datosObrasSociales = _reporteService.ObtenerTurnosPorObraSocial(anio);
                dgvObrasSociales.DataSource = null;
                dgvObrasSociales.DataSource = _datosObrasSociales;
            }

            // 3. Rendimiento Medicos (si esta habilitado)
            if (tabReportes.TabPages.Contains(tabMedicos))
            {
                _datosMedicos = _reporteService.ObtenerRendimientoMedicos(anio, mesSeleccionado);
                dgvMedicos.DataSource = null;
                dgvMedicos.DataSource = _datosMedicos;
            }

            ActualizarTarjetasResumen();

            pnlGraficoMensual.Invalidate();
            pnlGraficoOS.Invalidate();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ocurrio un error al ejecutar los procedimientos almacenados de reportes:\n\n{ex.Message}", 
                            "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ActualizarTarjetasResumen()
    {
        pnlTarjetas.Controls.Clear();

        int totalTurnos = _datosMensuales.Sum(d => d.TotalTurnos);
        int totalAtendidos = _datosMensuales.Sum(d => d.PacientesAtendidos);
        int totalAusentes = _datosMensuales.Sum(d => d.Ausentes);
        int totalCancelados = _datosMensuales.Sum(d => d.Cancelados);

        pnlTarjetas.Controls.Add(CrearTarjetaKPI("TOTAL TURNOS", totalTurnos.ToString(), Color.FromArgb(46, 134, 222)));
        pnlTarjetas.Controls.Add(CrearTarjetaKPI("PACIENTES ATENDIDOS", totalAtendidos.ToString(), Color.FromArgb(46, 204, 113)));
        pnlTarjetas.Controls.Add(CrearTarjetaKPI("AUSENTES", totalAusentes.ToString(), Color.FromArgb(230, 126, 34)));
        pnlTarjetas.Controls.Add(CrearTarjetaKPI("CANCELADOS", totalCancelados.ToString(), Color.FromArgb(231, 76, 60)));

        if (_usuarioActivo.Rol == RolUsuario.Administrador && _datosObrasSociales.Count > 0)
        {
            decimal totalFacturado = _datosObrasSociales.Sum(d => d.TotalRecaudado);
            pnlTarjetas.Controls.Add(CrearTarjetaKPI("FACTURACION ANUAL", totalFacturado.ToString("C0"), Color.FromArgb(142, 68, 173)));
        }
    }

    private Panel CrearTarjetaKPI(string titulo, string valor, Color colorAcento)
    {
        var panel = new Panel
        {
            Width = 165,
            Height = 65,
            BackColor = Color.White,
            Margin = new Padding(0, 0, 15, 0)
        };

        var pnlBorde = new Panel
        {
            Width = 5,
            Dock = DockStyle.Left,
            BackColor = colorAcento
        };

        var lblTit = new Label
        {
            Text = titulo,
            Font = new Font("Segoe UI Semibold", 7.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(120, 144, 156),
            Location = new Point(12, 10),
            AutoSize = true
        };

        var lblVal = new Label
        {
            Text = valor,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = Color.FromArgb(38, 50, 56),
            Location = new Point(10, 28),
            AutoSize = true
        };

        panel.Controls.Add(pnlBorde);
        panel.Controls.Add(lblTit);
        panel.Controls.Add(lblVal);

        return panel;
    }

    private void AsignarEventosGraficos()
    {
        pnlGraficoMensual.Paint += DibujarGraficoBarrasMensual;
        pnlGraficoOS.Paint += DibujarGraficoBarrasObrasSociales;
    }

    private void DibujarGraficoBarrasMensual(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        int w = pnlGraficoMensual.Width;
        int h = pnlGraficoMensual.Height;

        if (_datosMensuales.Count == 0)
        {
            using var font = new Font("Segoe UI", 10F, FontStyle.Italic);
            using var brush = new SolidBrush(Color.Gray);
            g.DrawString("No hay registros en el ano seleccionado.", font, brush, new PointF(20, 20));
            return;
        }

        int maxTurnos = _datosMensuales.Max(d => Math.Max(d.TotalTurnos, 1));
        int margenIzquierdo = 40;
        int margenAbajo = 30;
        int margenArriba = 25;
        int altoDisponible = h - margenAbajo - margenArriba;
        int anchoDisponible = w - margenIzquierdo - 20;

        int cantidadMeses = _datosMensuales.Count;
        float anchoGrupo = (float)anchoDisponible / Math.Max(cantidadMeses, 1);
        float anchoBarra = Math.Min(anchoGrupo * 0.35f, 22f);

        // Titulo grafico
        using var fontTitulo = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        using var brushTitulo = new SolidBrush(Color.FromArgb(44, 62, 80));
        g.DrawString("Evolucion de Pacientes Atendidos vs Total Turnos por Mes", fontTitulo, brushTitulo, 15, 6);

        // Linea base
        using var penEje = new Pen(Color.FromArgb(200, 210, 220), 1);
        g.DrawLine(penEje, margenIzquierdo, h - margenAbajo, w - 20, h - margenAbajo);

        using var brushTotal = new SolidBrush(Color.FromArgb(189, 195, 199));
        using var brushAtendidos = new SolidBrush(Color.FromArgb(46, 204, 113));
        using var fontEtiqueta = new Font("Segoe UI", 8F);
        using var brushTexto = new SolidBrush(Color.FromArgb(52, 73, 94));

        for (int i = 0; i < cantidadMeses; i++)
        {
            var item = _datosMensuales[i];
            float xCentro = margenIzquierdo + (i * anchoGrupo) + (anchoGrupo / 2);

            float altoBarraTotal = (float)item.TotalTurnos / maxTurnos * altoDisponible;
            float altoBarraAtendidos = (float)item.PacientesAtendidos / maxTurnos * altoDisponible;

            float yTotal = (h - margenAbajo) - altoBarraTotal;
            float yAtendidos = (h - margenAbajo) - altoBarraAtendidos;

            // Barra Total
            g.FillRectangle(brushTotal, xCentro - anchoBarra - 2, yTotal, anchoBarra, altoBarraTotal);
            // Barra Atendidos
            g.FillRectangle(brushAtendidos, xCentro + 2, yAtendidos, anchoBarra, altoBarraAtendidos);

            // Nombre Mes
            string mesCorto = item.NombreMes.Length > 3 ? item.NombreMes.Substring(0, 3) : item.NombreMes;
            var tam = g.MeasureString(mesCorto, fontEtiqueta);
            g.DrawString(mesCorto, fontEtiqueta, brushTexto, xCentro - (tam.Width / 2), h - margenAbajo + 6);

            // Valor sobre la barra
            if (item.PacientesAtendidos > 0)
            {
                g.DrawString(item.PacientesAtendidos.ToString(), fontEtiqueta, brushTexto, xCentro + 2, yAtendidos - 14);
            }
        }
    }

    private void DibujarGraficoBarrasObrasSociales(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        int w = pnlGraficoOS.Width;
        int h = pnlGraficoOS.Height;

        if (_datosObrasSociales.Count == 0)
        {
            using var font = new Font("Segoe UI", 10F, FontStyle.Italic);
            using var brush = new SolidBrush(Color.Gray);
            g.DrawString("No hay registros de obras sociales en el ano seleccionado.", font, brush, new PointF(20, 20));
            return;
        }

        using var fontTitulo = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        using var brushTitulo = new SolidBrush(Color.FromArgb(44, 62, 80));
        g.DrawString("Turnos atendidos por Obra Social", fontTitulo, brushTitulo, 15, 6);

        int maxTurnos = _datosObrasSociales.Max(d => Math.Max(d.CantidadTurnos, 1));
        int cantidadItems = Math.Min(_datosObrasSociales.Count, 7); // Top 7

        int margenIzquierdo = 120;
        int margenArriba = 30;
        int altoFila = Math.Min((h - margenArriba - 20) / cantidadItems, 25);
        int anchoDisponible = w - margenIzquierdo - 80;

        using var fontOS = new Font("Segoe UI", 8.5F);
        using var brushOS = new SolidBrush(Color.FromArgb(44, 62, 80));
        using var brushBarra = new SolidBrush(Color.FromArgb(52, 152, 219));

        for (int i = 0; i < cantidadItems; i++)
        {
            var item = _datosObrasSociales[i];
            int y = margenArriba + (i * altoFila);

            // Nombre OS recortado
            string nombre = item.ObraSocial.Length > 16 ? item.ObraSocial.Substring(0, 14) + ".." : item.ObraSocial;
            g.DrawString(nombre, fontOS, brushOS, 15, y + 2);

            // Barra horizontal
            float anchoBarra = (float)item.CantidadTurnos / maxTurnos * anchoDisponible;
            g.FillRectangle(brushBarra, margenIzquierdo, y + 2, Math.Max(anchoBarra, 3f), altoFila - 6);

            // Etiqueta cantidad
            g.DrawString($"{item.CantidadTurnos} turnos", fontOS, brushOS, margenIzquierdo + anchoBarra + 6, y + 2);
        }
    }
}
