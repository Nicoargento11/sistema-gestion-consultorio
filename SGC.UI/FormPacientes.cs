using System.Globalization;
using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

// Pantalla de Pacientes (Recepcionista). Izquierda: tarjeta "Nuevo paciente"
// (pasa a "Editar paciente" al elegir una fila) con los datos personales, el
// contacto y la cobertura. Derecha: buscador, contadores, grilla y barra del
// paciente seleccionado (Eliminar + detalle de turnos).
// La estructura estatica vive en el Designer; las columnas, los chips y el
// estilo de botones se arman aca para que el disenador no los borre.
public partial class FormPacientes : Form
{
    private readonly PacienteService _service = new();
    private readonly ObraSocialService _obraSocialService = new();
    private readonly Usuario? _usuarioActivo;

    // Id del paciente cargado en la tarjeta (null = modo "nuevo").
    private int? _idSeleccionado = null;

    // Listado completo traido del servicio; el buscador filtra sobre esta lista.
    private List<PacienteFila> _todos = new();

    // true mientras se cargan o sincronizan controles por codigo: los eventos
    // de seleccion y busqueda se ignoran para no recargar a mitad de camino.
    private bool _cargando = true;

    private FranjaMensaje _mensajes = null!;
    private BarraContadores _contadores = null!;
    private Font? _fuenteNegritaCache;
    private Font FuenteNegrita => _fuenteNegritaCache ??= new Font(DgvPacientes.Font, FontStyle.Bold);

    private const string ContTotal = "Total";
    private const string ContConObraSocial = "ConObraSocial";
    private const string ContParticulares = "Particulares";
    private const string ContConAusencias = "ConAusencias";

    private PacienteFila? FilaSeleccionada =>
        DgvPacientes.SelectedRows.Count > 0 ? DgvPacientes.SelectedRows[0].DataBoundItem as PacienteFila : null;

    // usuarioActivo define que medicos puede ver una recepcionista en la
    // columna "Proximo turno"; null = sin restriccion.
    public FormPacientes(Usuario? usuarioActivo = null)
    {
        InitializeComponent();
        _usuarioActivo = usuarioActivo;

        _mensajes = new FranjaMensaje(pnlEstado, LblMensaje) { ReacomodarContenedor = true };
        _contadores = new BarraContadores(FlpContadores);

        AcceptButton = BtnGuardar;
        // Enter dentro del buscador no debe guardar la tarjeta.
        TxtBuscar.Enter += (s, e) => AcceptButton = null;
        TxtBuscar.Leave += (s, e) => AcceptButton = BtnGuardar;

        BtnNuevo.Click += BtnNuevo_Click;
        BtnGuardar.Click += BtnGuardar_Click;
        BtnEliminar.Click += BtnEliminar_Click;
        DgvPacientes.SelectionChanged += DgvPacientes_SelectionChanged;
        DgvPacientes.CellFormatting += DgvPacientes_CellFormatting;
        TxtBuscar.TextChanged += TxtBuscar_TextChanged;
        ChkSoloAusencias.CheckedChanged += ChkSoloAusencias_CheckedChanged;

        TxtNombre.KeyPress += FiltrosTexto.SoloLetras;
        TxtApellido.KeyPress += FiltrosTexto.SoloLetras;
        TxtDni.KeyPress += FiltrosTexto.SoloNumeros;
        TxtTelefono.KeyPress += FiltrosTexto.SoloNumeros;

        // Los textos y flujos con ancho limitado se recalculan cuando cambia el ancho.
        tlpTarjeta.SizeChanged += (s, e) => AcotarEtiquetas();
        tlpToolbar.SizeChanged += (s, e) => AcotarEtiquetas();
        tlpAcciones.SizeChanged += (s, e) => AcotarEtiquetas();
        foreach (var flujo in new[] { flpFiltros, FlpContadores, flpBotonesAccion })
            flujo.SizeChanged += (s, e) => EstiloUi.AjustarFlujos(flpFiltros, FlpContadores, flpBotonesAccion);

        Load += FormPacientes_Load;
    }

    // Que medicos puede ver el usuario en "Proximo turno": una recepcionista
    // solo los que tiene asignados (sin asignaciones = ninguno), un medico
    // solo el propio; administrador (o sin usuario) todos (null).
    private IReadOnlyCollection<int>? MedicosVisibles()
    {
        if (_usuarioActivo == null)
            return null;

        return _usuarioActivo.Rol switch
        {
            RolUsuario.Recepcionista => _usuarioActivo.MedicosAsignados.Select(m => m.Id).ToList(),
            RolUsuario.Medico => _usuarioActivo.MedicoId is int id ? new List<int> { id } : new List<int>(),
            _ => null
        };
    }

    // ------------------------------------------------------------- arranque

    private void FormPacientes_Load(object? sender, EventArgs e)
    {
        _cargando = true;
        try
        {
            ConfigurarColumnas();
            ConfigurarBarraContadores();
            ConfigurarEstiloBotones();
            CargarCombos();
        }
        finally
        {
            _cargando = false;
        }

        CargarGrilla();
        IrAModoNuevo();

        // WinForms selecciona sola la primera fila recien cuando la grilla se
        // muestra de verdad en pantalla (despues de Load, sobre todo cuando el
        // formulario es hijo del menu). Esa seleccion automatica se ignora y se
        // limpia una vez que el formulario termino de mostrarse.
        _cargando = true;
        BeginInvoke(new Action(() =>
        {
            if (IsDisposed) return;
            _cargando = false;
            IrAModoNuevo();
        }));
    }

    private void ConfigurarEstiloBotones()
    {
        EstiloUi.EstilizarBoton(BtnGuardar, TipoBoton.Primario);
        EstiloUi.EstilizarBoton(BtnNuevo, TipoBoton.Secundario);
        EstiloUi.EstilizarBoton(BtnEliminar, TipoBoton.Peligro);
    }

    private void CargarCombos()
    {
        // Id = 0 es un pseudo-item que representa "Particular" (sin obra
        // social) - ObraSocial real empieza en Id = 1, no hay colision.
        var opciones = new List<ObraSocial> { new ObraSocial { Id = 0, Nombre = "Particular (sin obra social)" } };
        try
        {
            opciones.AddRange(_obraSocialService.ObtenerTodos());
        }
        catch (Exception ex)
        {
            MostrarMensaje("No se pudo cargar la lista de obras sociales: " + ex.Message, TipoMensaje.Error);
        }

        CboObraSocial.DataSource = opciones;
        CboObraSocial.DisplayMember = "Nombre";
        CboObraSocial.ValueMember = "Id";
    }

    private void ConfigurarColumnas()
    {
        // Se configura aca, en codigo, y no en el Designer, porque el disenador
        // visual de Visual Studio borra las columnas de un DataGridView cada vez
        // que se abre el formulario. Aca es inmune a eso.
        GrillaUi.Estilizar(DgvPacientes);

        int texto = EscalaDpi.Px(this, 45);
        int fecha = GrillaUi.MinimoPara(DgvPacientes, "00/00/0000");

        DataGridViewTextBoxColumn Columna(string nombre, string titulo, string propiedad, float peso, int minimo, string? formato = null)
        {
            var columna = GrillaUi.AgregarColumna(DgvPacientes, nombre, titulo, propiedad, peso, minimo, formato);
            // Con 11 columnas se recorta el margen extra del encabezado para que entren mas.
            columna.MinimumWidth = Math.Max(minimo, GrillaUi.MinimoEncabezado(DgvPacientes, titulo) - EscalaDpi.Px(this, 8));
            return columna;
        }

        Columna("colNombre", "Nombre", "Nombre", 80, texto);
        Columna("colApellido", "Apellido", "Apellido", 80, texto);
        Columna("colFechaNacimiento", "Fecha de nacimiento", "FechaNacimiento", 85, fecha, "dd/MM/yyyy")
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        Columna("colDni", "DNI", "Dni", 70, GrillaUi.MinimoPara(DgvPacientes, "00000000"));
        Columna("colEmail", "Email", "Email", 130, texto);
        Columna("colTelefono", "Teléfono", "Telefono", 85, GrillaUi.MinimoPara(DgvPacientes, "0000000000"));
        Columna("colObraSocial", "Obra social", "ObraSocialNombre", 90, texto);

        // Columnas calculadas (edad, ultima visita, proximo turno, ausencias):
        // "-" cuando no hay dato.
        Columna("colEdad", "Edad", "Edad", 40, GrillaUi.MinimoPara(DgvPacientes, "000"))
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        Columna("colUltimaVisita", "Última visita", "UltimaVisita", 85, fecha, "dd/MM/yyyy")
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        Columna("colProximoTurno", "Próximo turno", "ProximoTurno", 260, GrillaUi.MinimoPara(DgvPacientes, "00/00/0000 00:00"));
        Columna("colAusencias", "Ausencias", "Ausencias", 60, GrillaUi.MinimoPara(DgvPacientes, "00"))
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    private void ConfigurarBarraContadores()
    {
        _contadores.Agregar(ContTotal, EstiloUi.Texto, Color.White);
        _contadores.Agregar(ContConObraSocial, PaletaEstados.Fondo(EstadoTurno.Confirmado), PaletaEstados.Acento(EstadoTurno.Confirmado));
        _contadores.Agregar(ContParticulares, PaletaEstados.Fondo(EstadoTurno.Cancelado), PaletaEstados.Acento(EstadoTurno.Cancelado));
        _contadores.Agregar(ContConAusencias, PaletaEstados.Fondo(EstadoTurno.Ausente), PaletaEstados.Acento(EstadoTurno.Ausente));
        ActualizarContadores(new List<PacienteFila>());
    }

    // ------------------------------------------------------- datos y buscador

    // Trae el listado del servicio y lo muestra con el filtro vigente.
    private void CargarGrilla()
    {
        try
        {
            _todos = _service.ObtenerListado(MedicosVisibles());
        }
        catch (Exception ex)
        {
            _todos = new List<PacienteFila>();
            MostrarMensaje("No se pudo cargar el listado de pacientes: " + ex.Message, TipoMensaje.Error);
        }
        AplicarFiltro();
    }

    private void TxtBuscar_TextChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;
        AplicarFiltro();
    }

    private void ChkSoloAusencias_CheckedChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;
        AplicarFiltro();
    }

    private static bool Contiene(string texto, string buscado) =>
        CultureInfo.CurrentCulture.CompareInfo.IndexOf(texto, buscado, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;

    // Filtra _todos por nombre, apellido o DNI (sin distinguir mayusculas ni
    // acentos) y, si se pide, solo los que tienen ausencias. Si el paciente en
    // edicion queda fuera del filtro, la tarjeta vuelve al modo "nuevo".
    private void AplicarFiltro()
    {
        string texto = TxtBuscar.Text.Trim();
        bool soloAusencias = ChkSoloAusencias.Checked;

        IEnumerable<PacienteFila> consulta = _todos;
        if (!string.IsNullOrEmpty(texto))
        {
            consulta = consulta.Where(f =>
                Contiene($"{f.Nombre} {f.Apellido}", texto) ||
                Contiene($"{f.Apellido} {f.Nombre}", texto) ||
                Contiene(f.Dni, texto));
        }
        if (soloAusencias)
            consulta = consulta.Where(f => f.Ausencias > 0);
        var lista = consulta.ToList();

        bool anterior = _cargando;
        _cargando = true;
        bool reseleccionado = false;
        try
        {
            // Se desengancha SelectionChanged mientras se reasigna el DataSource
            // para que no corra con filas a medio cargar.
            DgvPacientes.SelectionChanged -= DgvPacientes_SelectionChanged;
            DgvPacientes.DataSource = lista;

            // Sin esto, WinForms selecciona sola la primera fila al asignar el
            // DataSource y precarga el formulario con ese registro.
            DgvPacientes.ClearSelection();
            DgvPacientes.CurrentCell = null;

            // Si se estaba editando un paciente y sigue en la lista, queda elegido.
            if (_idSeleccionado != null)
            {
                foreach (DataGridViewRow fila in DgvPacientes.Rows)
                {
                    if (fila.DataBoundItem is PacienteFila f && f.Id == _idSeleccionado)
                    {
                        fila.Selected = true;
                        DgvPacientes.CurrentCell = fila.Cells[0];
                        reseleccionado = true;
                        break;
                    }
                }
            }
            DgvPacientes.SelectionChanged += DgvPacientes_SelectionChanged;
        }
        finally
        {
            _cargando = anterior;
        }

        ActualizarContadores(lista);

        string vista = string.IsNullOrEmpty(texto) && !soloAusencias
            ? $"Mostrando: todos los pacientes ({_todos.Count})"
            : $"Mostrando: {lista.Count} de {_todos.Count}";
        if (!string.IsNullOrEmpty(texto)) vista += $" · búsqueda «{texto}»";
        if (soloAusencias) vista += " · solo con ausencias";
        LblVista.Text = vista;
        AcotarEtiquetas();

        if (_idSeleccionado != null && !reseleccionado)
            IrAModoNuevo();
        else
            ActualizarAcciones();
    }

    // Cuenta sobre lo que muestra la grilla (con la busqueda ya aplicada).
    private void ActualizarContadores(List<PacienteFila> filas)
    {
        int conObraSocial = filas.Count(f => f.Paciente.ObraSocialId != null);
        _contadores.Poner(ContTotal, $"Total: {filas.Count}");
        _contadores.Poner(ContConObraSocial, $"Con obra social: {conObraSocial}");
        _contadores.Poner(ContParticulares, $"Particulares: {filas.Count - conObraSocial}");
        _contadores.Poner(ContConAusencias, $"Con ausencias: {filas.Count(f => f.Ausencias > 0)}");
    }

    private void DgvPacientes_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || DgvPacientes.Rows[e.RowIndex].DataBoundItem is not PacienteFila fila)
            return;

        string columna = DgvPacientes.Columns[e.ColumnIndex].Name;
        if (columna == "colAusencias" && fila.Ausencias > 0)
        {
            // Resalta a quienes faltaron a algun turno (si la fila esta
            // seleccionada se conserva el texto blanco de la seleccion).
            if (!DgvPacientes.Rows[e.RowIndex].Selected)
                e.CellStyle!.ForeColor = PaletaEstados.Acento(EstadoTurno.Ausente);
            e.CellStyle!.Font = FuenteNegrita;
        }
        else if (columna == "colProximoTurno")
        {
            // El texto puede quedar cortado; el tooltip muestra el completo.
            DgvPacientes.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = fila.ProximoTurno;
        }
    }

    // ------------------------------------------------------- modo de la tarjeta

    // Vuelve al modo "nuevo paciente": limpia la tarjeta y deselecciona la fila.
    private void IrAModoNuevo()
    {
        bool anterior = _cargando;
        _cargando = true;
        try
        {
            DgvPacientes.SelectionChanged -= DgvPacientes_SelectionChanged;
            DgvPacientes.ClearSelection();
            DgvPacientes.CurrentCell = null;
            DgvPacientes.SelectionChanged += DgvPacientes_SelectionChanged;

            _idSeleccionado = null;
            TxtNombre.Text = "";
            TxtApellido.Text = "";
            TxtDni.Text = "";
            TxtEmail.Text = "";
            TxtTelefono.Text = "";
            CboObraSocial.SelectedValue = 0;
            DtpFechaNacimiento.Value = DateTime.Today;
        }
        finally
        {
            _cargando = anterior;
        }

        ActualizarModo();
        ActualizarAcciones();
    }

    // Titulo y aviso de la tarjeta segun si hay un paciente cargado para editar.
    private void ActualizarModo()
    {
        bool editando = _idSeleccionado != null;
        lblTituloTarjeta.Text = editando ? "EDITAR PACIENTE" : "NUEVO PACIENTE";

        LblModo.Visible = editando;
        LblModo.Text = editando
            ? $"Editando a «{FilaSeleccionada?.Nombre} {FilaSeleccionada?.Apellido}»: modifique los datos y pulse «Guardar»."
            : "";
        AcotarEtiquetas();
    }

    // Texto de la barra de seleccion, detalle de turnos y estado del boton Eliminar.
    private void ActualizarAcciones()
    {
        var fila = FilaSeleccionada;
        if (fila == null)
        {
            LblSeleccion.Text = "Paciente seleccionado: ninguno";
            BtnEliminar.Enabled = false;
            MostrarAyudaAcciones("Elija una fila de la tabla.");
            LblDetalle.Text = "Seleccione un paciente para ver su próximo turno y su última visita.";
            AcotarEtiquetas();
            return;
        }

        LblSeleccion.Text = $"Paciente seleccionado: {fila.Apellido}, {fila.Nombre} · DNI {fila.Dni}";
        BtnEliminar.Enabled = true;
        MostrarAyudaAcciones("");

        string ultima = fila.UltimaVisita is DateOnly v ? v.ToString("dd/MM/yyyy") : "sin visitas";
        LblDetalle.Text = $"Próximo turno: {fila.ProximoTurno} · Última visita: {ultima} · Ausencias: {fila.Ausencias}";
        AcotarEtiquetas();
    }

    private void MostrarAyudaAcciones(string texto)
    {
        LblAyudaAcciones.Text = texto;
        LblAyudaAcciones.Visible = !string.IsNullOrEmpty(texto);
        AcotarEtiquetas();
    }

    // ------------------------------------------------------------ eventos

    private void DgvPacientes_SelectionChanged(object? sender, EventArgs e)
    {
        if (_cargando) return;

        var fila = FilaSeleccionada;
        if (fila == null)
        {
            ActualizarAcciones();
            return;
        }

        var paciente = fila.Paciente;
        _cargando = true;
        try
        {
            _idSeleccionado = paciente.Id;
            TxtNombre.Text = paciente.Nombre;
            TxtApellido.Text = paciente.Apellido;
            TxtDni.Text = paciente.Dni;
            TxtEmail.Text = paciente.Email;
            TxtTelefono.Text = paciente.Telefono;
            CboObraSocial.SelectedValue = paciente.ObraSocialId ?? 0;

            // Una fecha fuera del rango del selector no debe romper la pantalla.
            var nacimiento = paciente.FechaNacimiento.ToDateTime(TimeOnly.MinValue);
            if (nacimiento < DtpFechaNacimiento.MinDate) nacimiento = DtpFechaNacimiento.MinDate;
            if (nacimiento > DtpFechaNacimiento.MaxDate) nacimiento = DtpFechaNacimiento.MaxDate;
            DtpFechaNacimiento.Value = nacimiento;
        }
        finally
        {
            _cargando = false;
        }

        LimpiarMensaje();
        ActualizarModo();
        ActualizarAcciones();
    }

    private void BtnNuevo_Click(object? sender, EventArgs e)
    {
        LimpiarMensaje();
        IrAModoNuevo();
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        try
        {
            var paciente = new Paciente
            {
                Id = _idSeleccionado ?? 0,
                Nombre = TxtNombre.Text,
                Apellido = TxtApellido.Text,
                Dni = TxtDni.Text,
                Email = TxtEmail.Text,
                Telefono = TxtTelefono.Text,
                ObraSocialId = (int)(CboObraSocial.SelectedValue ?? 0) == 0 ? null : (int)CboObraSocial.SelectedValue!,
                FechaNacimiento = DateOnly.FromDateTime(DtpFechaNacimiento.Value)
            };

            if (_idSeleccionado == null)
                _service.Agregar(paciente);
            else
                _service.Modificar(paciente);
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message, TipoMensaje.Error);
            return;
        }

        CargarGrilla();
        IrAModoNuevo();
        MostrarMensaje("Paciente guardado correctamente", TipoMensaje.Ok);
    }

    private void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var fila = FilaSeleccionada;
        if (fila == null)
        {
            MostrarMensaje("Seleccione un paciente de la lista primero.", TipoMensaje.Error);
            return;
        }
        var pacienteSeleccionado = fila.Paciente;

        var respuesta = MessageBox.Show(
            $"¿Está seguro que desea eliminar a {pacienteSeleccionado.Nombre} {pacienteSeleccionado.Apellido}?",
            "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (respuesta != DialogResult.Yes)
            return;

        try
        {
            _service.EliminarLogico(pacienteSeleccionado.Id);
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message, TipoMensaje.Error);
            return;
        }

        CargarGrilla();
        IrAModoNuevo();
        MostrarMensaje("Paciente eliminado correctamente.", TipoMensaje.Ok);
    }

    // ----------------------------------------------------- mensajes y estilo

    private void MostrarMensaje(string texto, TipoMensaje tipo) => _mensajes.Mostrar(texto, tipo);

    private void LimpiarMensaje() => _mensajes.Limpiar();

    // Limita el ancho de los textos que pueden ocupar varias lineas al de su contenedor.
    private void AcotarEtiquetas()
    {
        EstiloUi.Acotar(LblModo, tlpTarjeta);
        EstiloUi.Acotar(LblVista, tlpToolbar);
        EstiloUi.Acotar(LblSeleccion, tlpAcciones);
        EstiloUi.Acotar(LblAyudaAcciones, tlpAcciones);
        EstiloUi.Acotar(LblDetalle, tlpAcciones);
        EstiloUi.AjustarFlujos(flpFiltros, FlpContadores, flpBotonesAccion);
        _mensajes.Ajustar();
    }
}
