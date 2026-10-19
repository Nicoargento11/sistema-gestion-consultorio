using System.Globalization;
using SGC.Entidades;
using SGC.Entidades.Reportes;
using SGC.Logica;

namespace SGC.UI;

// Pantalla de inicio: resumen del dia segun el rol del usuario. Se hospeda en
// FormMenuPrincipal con AbrirFormularioHijo, igual que el resto de las pantallas.
// Toda la interfaz se arma por codigo (no hay InitializeComponent) para que el
// disenador de Visual Studio no pueda borrarla.
//
//   Administrador : turnos de hoy por estado, semana, ausentes y recaudacion
//                   del mes, medicos con agenda y avisos.
//   Medico        : sus turnos de hoy, proximo paciente y sus avisos.
//   Recepcionista : turnos de hoy de sus medicos asignados, turnos sin
//                   confirmar y avisos.
public class FormInicio : Form
{
    private static readonly CultureInfo CulturaAr = new("es-AR");

    private static readonly Color ColorTexto = Color.FromArgb(27, 42, 74);
    private static readonly Color ColorSecundario = Color.FromArgb(110, 120, 135);
    private static readonly Color ColorFondo = Color.FromArgb(245, 246, 250);
    private static readonly Color ColorAzul = Color.FromArgb(46, 134, 222);

    private static readonly Font FuenteTitulo = new("Segoe UI", 18F, FontStyle.Bold);
    private static readonly Font FuenteSeccion = new("Segoe UI", 11F, FontStyle.Bold);
    private static readonly Font FuenteNormal = new("Segoe UI", 9.5F);
    private static readonly Font FuenteChica = new("Segoe UI", 8.5F);
    private static readonly Font FuenteNumero = new("Segoe UI", 22F, FontStyle.Bold);
    private static readonly Font FuenteNumeroMedio = new("Segoe UI", 16F, FontStyle.Bold);

    private const int AltoTarjeta = 92;

    private readonly Usuario _usuario;
    private readonly DateOnly _fecha;
    private readonly ResumenService _resumenService = new();
    private readonly NotificacionService _notificacionService = new();
    private readonly MedicoService _medicoService = new();

    private readonly Panel _raiz = new();
    private readonly FlowLayoutPanel _contenido = new();

    // fechaReferencia solo se usa para pruebas; en la aplicacion es siempre hoy.
    public FormInicio(Usuario usuario, DateOnly? fechaReferencia = null)
    {
        _usuario = usuario;
        _fecha = fechaReferencia ?? DateOnly.FromDateTime(DateTime.Today);

        Text = "Inicio";
        BackColor = ColorFondo;
        Font = FuenteNormal;
        Name = "FormInicio";

        _raiz.Dock = DockStyle.Fill;
        _raiz.AutoScroll = true;
        _raiz.Padding = new Padding(24, 20, 24, 20);
        _raiz.BackColor = ColorFondo;

        _contenido.FlowDirection = FlowDirection.TopDown;
        _contenido.WrapContents = false;
        _contenido.AutoSize = true;
        _contenido.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _contenido.Location = new Point(24, 20);
        _contenido.BackColor = ColorFondo;

        _raiz.Controls.Add(_contenido);
        Controls.Add(_raiz);

        _raiz.Resize += (s, e) => AjustarAnchos();
        Load += (s, e) => AjustarAnchos();

        Cargar();
    }

    // ---------------------------------------------------------------- datos

    private void Cargar()
    {
        _contenido.SuspendLayout();
        foreach (Control c in _contenido.Controls.Cast<Control>().ToList())
        {
            _contenido.Controls.Remove(c);
            c.Dispose();
        }

        _contenido.Controls.Add(CrearEncabezado());

        try
        {
            switch (_usuario.Rol)
            {
                case RolUsuario.Administrador:
                    CargarAdministrador();
                    break;
                case RolUsuario.Medico:
                    CargarMedico();
                    break;
                case RolUsuario.Recepcionista:
                    CargarRecepcionista();
                    break;
            }
        }
        catch (Exception ex)
        {
            _contenido.Controls.Add(CrearAviso(
                "No se pudo cargar el resumen del día: " + ex.Message, Color.Firebrick));
        }

        _contenido.ResumeLayout(true);
        AjustarAnchos();
    }

    private void CargarAdministrador()
    {
        var resumen = _resumenService.ObtenerResumen(_fecha);

        AgregarTurnosDelDia(resumen);

        AgregarSeccion("Resumen general");
        var tarjetas = CrearFilaTarjetas();
        tarjetas.Controls.Add(CrearTarjeta(resumen.TurnosSemana.ToString(CulturaAr), "Turnos de la semana", ColorAzul));
        tarjetas.Controls.Add(CrearTarjeta(resumen.AusentesMes.ToString(CulturaAr), "Ausentes del mes", PaletaEstados.Acento(EstadoTurno.Ausente)));
        tarjetas.Controls.Add(CrearTarjeta(resumen.RecaudacionMes.ToString("C2", CulturaAr), "Recaudación del mes", PaletaEstados.Acento(EstadoTurno.Asistio), 210));
        tarjetas.Controls.Add(CrearTarjeta(resumen.MedicosConAgendaHoy.ToString(CulturaAr), EsHoy ? "Médicos con agenda hoy" : "Médicos con agenda ese día", ColorTexto, 200));
        _contenido.Controls.Add(tarjetas);

        AgregarAvisos(_notificacionService.ObtenerAvisos(), "Avisos");
    }

    private void CargarMedico()
    {
        if (_usuario.MedicoId == null)
        {
            _contenido.Controls.Add(CrearAviso("Este usuario no tiene un médico asociado, por eso no hay resumen para mostrar.", Color.DarkOrange));
            return;
        }

        int medicoId = _usuario.MedicoId.Value;
        var resumen = _resumenService.ObtenerResumen(_fecha, medicoId);

        AgregarTurnosDelDia(resumen);

        AgregarSeccion("Mi agenda");
        var tarjetas = CrearFilaTarjetas();

        // Si la fecha es hoy, solo cuentan los pacientes que todavia no pasaron de hora.
        var desde = EsHoy ? TimeOnly.FromDateTime(DateTime.Now) : TimeOnly.MinValue;
        var proximo = _resumenService.ObtenerProximoPaciente(medicoId, _fecha, desde);
        if (proximo != null)
            tarjetas.Controls.Add(CrearTarjeta(proximo.HoraInicio.ToString("HH:mm"), "Próximo paciente", PaletaEstados.Acento(EstadoTurno.Confirmado), 300, proximo.PacienteNombre));
        else
            tarjetas.Controls.Add(CrearTarjeta("-", "Próximo paciente", PaletaEstados.Acento(EstadoTurno.Cancelado), 300, EsHoy ? "No quedan pacientes por atender hoy" : "No hay pacientes pendientes ese día"));

        tarjetas.Controls.Add(CrearTarjeta(resumen.TurnosSemana.ToString(CulturaAr), "Turnos de la semana", ColorAzul));
        tarjetas.Controls.Add(CrearTarjeta(resumen.AusentesMes.ToString(CulturaAr), "Ausentes del mes", PaletaEstados.Acento(EstadoTurno.Ausente)));
        _contenido.Controls.Add(tarjetas);

        var medico = _usuario.Medico ?? _medicoService.ObtenerPorId(medicoId);
        AgregarAvisos(medico == null ? new List<string>() : _notificacionService.ObtenerAvisosDeMedico(medico), "Mis avisos");
    }

    private void CargarRecepcionista()
    {
        var ids = _usuario.MedicosAsignados.Select(m => m.Id).ToList();
        if (ids.Count == 0)
        {
            _contenido.Controls.Add(CrearAviso("Este usuario no tiene médicos asignados, por eso no hay resumen para mostrar.", Color.DarkOrange));
            AgregarAvisos(_notificacionService.ObtenerAvisos(), "Avisos");
            return;
        }

        var resumen = _resumenService.ObtenerResumenDeMedicos(_fecha, ids);

        AgregarTurnosDelDia(resumen);

        AgregarSeccion("Esta semana y este mes");
        var tarjetas = CrearFilaTarjetas();
        tarjetas.Controls.Add(CrearTarjeta(resumen.TurnosSemana.ToString(CulturaAr), "Turnos de la semana", ColorAzul));
        tarjetas.Controls.Add(CrearTarjeta(resumen.AusentesMes.ToString(CulturaAr), "Ausentes del mes", PaletaEstados.Acento(EstadoTurno.Ausente)));
        _contenido.Controls.Add(tarjetas);

        AgregarSeccion("Próximos turnos sin confirmar");
        _contenido.Controls.Add(CrearTablaProximos(_resumenService.ObtenerProximosSinConfirmar(_fecha, ids)));

        AgregarAvisos(_notificacionService.ObtenerAvisos(), "Avisos");
    }

    private bool EsHoy => _fecha == DateOnly.FromDateTime(DateTime.Today);

    // ------------------------------------------------------------- secciones

    private Control CrearEncabezado()
    {
        var panel = new Panel { Height = 64, Margin = new Padding(0, 0, 0, 14), Name = "pnlEncabezado" };

        var saludo = new Label
        {
            AutoSize = true,
            Font = FuenteTitulo,
            ForeColor = ColorTexto,
            Location = new Point(0, 0),
            Text = $"{SaludoSegunHora()}, {_usuario.NombreUsuario}",
            Name = "lblSaludo"
        };

        string fechaTexto = _fecha.ToDateTime(TimeOnly.MinValue).ToString("dddd d 'de' MMMM 'de' yyyy", CulturaAr);
        fechaTexto = char.ToUpper(fechaTexto[0], CulturaAr) + fechaTexto.Substring(1);

        var subtitulo = new Label
        {
            AutoSize = true,
            Font = FuenteNormal,
            ForeColor = ColorSecundario,
            Location = new Point(2, 38),
            Text = $"{fechaTexto}  |  {DescripcionRol()}",
            Name = "lblFechaRol"
        };

        var actualizar = new Button
        {
            Name = "BtnActualizarInicio",
            Text = "Actualizar",
            FlatStyle = FlatStyle.Flat,
            BackColor = ColorAzul,
            ForeColor = Color.White,
            Font = FuenteNormal,
            Size = new Size(100, 28),
            Location = new Point(0, 6)
        };
        actualizar.FlatAppearance.BorderSize = 0;
        actualizar.Click += (s, e) => Cargar();
        panel.Tag = actualizar;

        panel.Controls.Add(saludo);
        panel.Controls.Add(subtitulo);
        panel.Controls.Add(actualizar);
        return panel;
    }

    private string DescripcionRol() => _usuario.Rol switch
    {
        RolUsuario.Administrador => "Resumen general del consultorio",
        RolUsuario.Medico => "Resumen de su agenda",
        RolUsuario.Recepcionista => "Resumen de los médicos asignados",
        _ => ""
    };

    private static string SaludoSegunHora()
    {
        int hora = DateTime.Now.Hour;
        if (hora < 12) return "Buenos días";
        if (hora < 20) return "Buenas tardes";
        return "Buenas noches";
    }

    private void AgregarTurnosDelDia(ResumenDelDiaFila resumen)
    {
        AgregarSeccion(EsHoy ? "Turnos de hoy" : $"Turnos del {_fecha:dd/MM/yyyy}");

        var tarjetas = CrearFilaTarjetas();
        tarjetas.Controls.Add(CrearTarjeta(resumen.Pendientes.ToString(CulturaAr), "Pendientes", PaletaEstados.Acento(EstadoTurno.Pendiente)));
        tarjetas.Controls.Add(CrearTarjeta(resumen.Confirmados.ToString(CulturaAr), "Confirmados", PaletaEstados.Acento(EstadoTurno.Confirmado)));
        tarjetas.Controls.Add(CrearTarjeta(resumen.Asistieron.ToString(CulturaAr), "Asistieron", PaletaEstados.Acento(EstadoTurno.Asistio)));
        tarjetas.Controls.Add(CrearTarjeta(resumen.Ausentes.ToString(CulturaAr), "Ausentes", PaletaEstados.Acento(EstadoTurno.Ausente)));
        tarjetas.Controls.Add(CrearTarjeta(resumen.Cancelados.ToString(CulturaAr), "Cancelados", PaletaEstados.Acento(EstadoTurno.Cancelado)));
        _contenido.Controls.Add(tarjetas);

        if (resumen.TotalDia == 0)
        {
            _contenido.Controls.Add(CrearAviso(
                EsHoy ? "No hay turnos cargados para hoy." : "No hay turnos cargados para esa fecha.", ColorSecundario));
        }
    }

    private void AgregarSeccion(string titulo)
    {
        _contenido.Controls.Add(new Label
        {
            AutoSize = true,
            Font = FuenteSeccion,
            ForeColor = ColorTexto,
            Margin = new Padding(0, 10, 0, 6),
            Text = titulo
        });
    }

    private void AgregarAvisos(List<string> avisos, string titulo)
    {
        AgregarSeccion(titulo);

        if (avisos.Count == 0)
        {
            _contenido.Controls.Add(CrearAviso(
                "No hay avisos registrados en esta sesión. Aparecen al asignar, reprogramar o cancelar turnos.", ColorSecundario));
            return;
        }

        var lista = new ListBox
        {
            Name = "LstAvisos",
            Font = FuenteNormal,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.White,
            ForeColor = ColorTexto,
            IntegralHeight = false,
            Height = Math.Min(avisos.Count, 6) * 20 + 8,
            Margin = new Padding(0, 0, 0, 8)
        };
        // Los mas recientes primero.
        foreach (var aviso in Enumerable.Reverse(avisos))
            lista.Items.Add(aviso);
        _contenido.Controls.Add(lista);
    }

    private Control CrearTablaProximos(List<Turno> turnos)
    {
        if (turnos.Count == 0)
            return CrearAviso("No hay turnos pendientes de confirmación.", ColorSecundario);

        var grilla = new DataGridView
        {
            Name = "DgvProximosSinConfirmar",
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            ReadOnly = true,
            RowHeadersVisible = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Font = FuenteNormal,
            ColumnHeadersHeight = 30,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ScrollBars = ScrollBars.None,
            Margin = new Padding(0, 0, 0, 8)
        };
        grilla.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFecha", HeaderText = "Fecha", FillWeight = 70 });
        grilla.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHora", HeaderText = "Hora", FillWeight = 50 });
        grilla.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPaciente", HeaderText = "Paciente", FillWeight = 160 });
        grilla.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedico", HeaderText = "Médico", FillWeight = 190 });

        foreach (var t in turnos)
            grilla.Rows.Add(t.Fecha.ToString("dd/MM/yyyy"), t.HoraInicio.ToString("HH:mm"), t.PacienteNombre, t.MedicoNombre);

        // Lista de solo lectura: sin seleccion visible y con encabezado plano
        // (el estilo visual por defecto pinta de azul el encabezado activo).
        grilla.EnableHeadersVisualStyles = false;
        grilla.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(236, 238, 242);
        grilla.ColumnHeadersDefaultCellStyle.ForeColor = ColorTexto;
        grilla.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 238, 242);
        grilla.ColumnHeadersDefaultCellStyle.SelectionForeColor = ColorTexto;
        grilla.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grilla.GridColor = Color.FromArgb(226, 229, 236);
        grilla.DefaultCellStyle.SelectionBackColor = Color.White;
        grilla.DefaultCellStyle.SelectionForeColor = ColorTexto;
        grilla.ClearSelection();
        grilla.Height = grilla.ColumnHeadersHeight + turnos.Count * grilla.RowTemplate.Height + 2;
        return grilla;
    }

    // --------------------------------------------------------------- piezas

    private static FlowLayoutPanel CrearFilaTarjetas()
    {
        return new FlowLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            WrapContents = true,
            FlowDirection = FlowDirection.LeftToRight,
            Margin = new Padding(0),
            BackColor = ColorFondo
        };
    }

    // Tarjeta de indicador: numero grande + descripcion. Es un unico Panel
    // plano con una franja de color a la izquierda dibujada en Paint (sin
    // paneles anidados).
    private static Panel CrearTarjeta(string valor, string titulo, Color acento, int ancho = 168, string? detalle = null)
    {
        var tarjeta = new Panel
        {
            Size = new Size(ancho, AltoTarjeta),
            BackColor = Color.White,
            Margin = new Padding(0, 0, 12, 12)
        };
        tarjeta.Paint += (s, e) =>
        {
            using var pincel = new SolidBrush(acento);
            e.Graphics.FillRectangle(pincel, 0, 0, 5, tarjeta.Height);
            using var lapiz = new Pen(Color.FromArgb(226, 229, 236));
            e.Graphics.DrawRectangle(lapiz, 0, 0, tarjeta.Width - 1, tarjeta.Height - 1);
        };

        tarjeta.Controls.Add(new Label
        {
            AutoSize = false,
            AutoEllipsis = true,
            Font = valor.Length > 9 ? FuenteNumeroMedio : FuenteNumero,
            ForeColor = acento,
            Location = new Point(16, 8),
            Size = new Size(ancho - 24, 38),
            TextAlign = ContentAlignment.MiddleLeft,
            Text = valor
        });
        tarjeta.Controls.Add(new Label
        {
            AutoSize = false,
            AutoEllipsis = true,
            Font = FuenteNormal,
            ForeColor = ColorTexto,
            Location = new Point(16, 48),
            Size = new Size(ancho - 24, 20),
            Text = titulo
        });
        if (detalle != null)
        {
            tarjeta.Controls.Add(new Label
            {
                AutoSize = false,
                AutoEllipsis = true,
                Font = FuenteChica,
                ForeColor = ColorSecundario,
                Location = new Point(16, 68),
                Size = new Size(ancho - 24, 18),
                Text = detalle
            });
        }
        return tarjeta;
    }

    private static Label CrearAviso(string texto, Color color)
    {
        return new Label
        {
            AutoSize = true,
            MaximumSize = new Size(900, 0),
            Font = FuenteNormal,
            ForeColor = color,
            Margin = new Padding(0, 0, 0, 8),
            Text = texto
        };
    }

    // Los controles de ancho completo (tablas, avisos, encabezado) siguen el
    // ancho disponible; las tarjetas hacen wrap dentro de su fila.
    private void AjustarAnchos()
    {
        int ancho = Math.Max(480, _raiz.ClientSize.Width - _raiz.Padding.Horizontal);

        foreach (Control c in _contenido.Controls)
        {
            if (c is FlowLayoutPanel fila)
            {
                fila.MaximumSize = new Size(ancho, 0);
            }
            else if (c is DataGridView || c is ListBox || c is Panel)
            {
                c.Width = ancho;
            }

            // El boton Actualizar queda pegado al borde derecho del encabezado.
            if (c is Panel encabezado && encabezado.Tag is Button boton)
                boton.Left = ancho - boton.Width;
        }
    }
}
