using System.Globalization;
using SGC.Logica;

namespace SGC.UI;

// Copia de seguridad de la base (solo Administrador; el menu oculta el boton
// al resto de los roles). Tres tarjetas: crear copia, historial y restaurar.
// Toda la logica de SQL vive en BackupService; aca solo se arma la pantalla.
//
// Las operaciones corren en un hilo aparte (Task.Run) para que la ventana no
// se congele mientras SQL Server escribe o lee el .bak: mientras dura, los
// botones quedan deshabilitados.
//
// El archivo .bak lo lee y lo escribe la cuenta de servicio de SQL Server, no
// la aplicacion: la carpeta debe existir en el servidor y esa cuenta debe tener
// permiso. Por eso se sugiere la carpeta de copias predeterminada de la instancia.
public partial class FormBackup : Form
{
    private static readonly CultureInfo CulturaAr = new("es-AR");

    private readonly BackupService _backupService = new();
    private readonly FranjaMensaje _franja;

    public FormBackup()
    {
        InitializeComponent();
        _franja = new FranjaMensaje(pnlMensaje, LblMensaje) { ReacomodarContenedor = true };

        // Estilos y columnas en codigo (no en el Designer) para que el
        // disenador visual de Visual Studio no pueda borrarlos.
        EstiloUi.EstilizarBoton(btnCrear, TipoBoton.Primario);
        EstiloUi.EstilizarBoton(btnExaminar, TipoBoton.Secundario);
        EstiloUi.EstilizarBoton(btnElegirArchivo, TipoBoton.Secundario);
        EstiloUi.EstilizarBoton(btnUsarSeleccionada, TipoBoton.Secundario);
        EstiloUi.EstilizarBoton(btnRestaurar, TipoBoton.Peligro);

        foreach (var tarjeta in new[] { cardCrear, cardHistorial, cardRestaurar })
        {
            var actual = tarjeta;
            actual.Paint += (s, e) =>
            {
                using var lapiz = new Pen(EstiloUi.Borde);
                e.Graphics.DrawRectangle(lapiz, 0, 0, actual.Width - 1, actual.Height - 1);
            };
        }

        ConfigurarGrilla();

        // Los textos largos ocupan varias lineas: se acotan al ancho de su tarjeta.
        tlpCrear.SizeChanged += (s, e) => EstiloUi.Acotar(lblCrearAyuda, tlpCrear);
        tlpRestaurar.SizeChanged += (s, e) => EstiloUi.Acotar(lblRestaurarAdvertencia, tlpRestaurar);
        tlpHistorial.SizeChanged += (s, e) => EstiloUi.Acotar(lblHistorialEstado, tlpHistorial);

        btnExaminar.Click += (s, e) => ElegirCarpeta();
        btnElegirArchivo.Click += (s, e) => ElegirArchivo();
        btnUsarSeleccionada.Click += (s, e) => UsarSeleccionada();
        btnCrear.Click += async (s, e) => await CrearCopiaAsync();
        btnRestaurar.Click += async (s, e) => await RestaurarAsync();

        Load += async (s, e) => await CargarInicialAsync();
    }

    // ------------------------------------------------------------- grilla

    private void ConfigurarGrilla()
    {
        GrillaUi.Estilizar(dgvHistorial);
        GrillaUi.AgregarColumna(dgvHistorial, "colFecha", "Fecha", nameof(FilaHistorialBackup.Fecha), 30,
            GrillaUi.MinimoPara(dgvHistorial, "00/00/0000 00:00"), "dd/MM/yyyy HH:mm");
        GrillaUi.AgregarColumna(dgvHistorial, "colArchivo", "Archivo", nameof(FilaHistorialBackup.Archivo), 50,
            GrillaUi.MinimoPara(dgvHistorial, "SGC_00000000_000000.bak"));
        var tamano = GrillaUi.AgregarColumna(dgvHistorial, "colTamano", "Tamaño (MB)", nameof(FilaHistorialBackup.TamanoMb), 20,
            GrillaUi.MinimoPara(dgvHistorial, "0000,00"), "N2");
        tamano.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgvHistorial.CellFormatting += (s, e) =>
        {
            // El tooltip de la celda muestra la ruta completa del archivo.
            if (e.RowIndex >= 0 && dgvHistorial.Rows[e.RowIndex].DataBoundItem is FilaHistorialBackup fila)
                dgvHistorial.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = fila.Ruta;
        };
    }

    // -------------------------------------------------------------- carga

    private async Task CargarInicialAsync()
    {
        SetOcupado(true);
        try
        {
            string carpeta = await Task.Run(() => _backupService.ObtenerCarpetaPorDefecto());
            if (IsDisposed) return;

            txtCarpeta.Text = carpeta;
            if (string.IsNullOrEmpty(carpeta))
                _franja.Mostrar("No se pudo detectar la carpeta de copias predeterminada del servidor. Indicá una carpeta que exista en el servidor.", TipoMensaje.Advertencia);

            await CargarHistorialAsync();
        }
        catch (Exception ex)
        {
            if (!IsDisposed) _franja.Mostrar("No se pudo preparar la pantalla: " + ex.Message, TipoMensaje.Error);
        }
        finally
        {
            if (!IsDisposed) SetOcupado(false);
        }
    }

    private async Task CargarHistorialAsync()
    {
        var historial = await Task.Run(() => _backupService.ObtenerHistorial());
        if (IsDisposed) return;

        dgvHistorial.DataSource = historial.Filas;
        dgvHistorial.ClearSelection();

        if (!historial.Disponible)
        {
            lblHistorialEstado.Text = historial.Mensaje ?? "No se pudo leer el historial de copias.";
            lblHistorialEstado.Visible = true;
        }
        else if (historial.Filas.Count == 0)
        {
            lblHistorialEstado.Text = "Todavía no hay copias registradas para esta base.";
            lblHistorialEstado.Visible = true;
        }
        else
        {
            lblHistorialEstado.Visible = false;
        }
        EstiloUi.Acotar(lblHistorialEstado, tlpHistorial);
    }

    // -------------------------------------------------------------- crear

    private void ElegirCarpeta()
    {
        using var dialogo = new FolderBrowserDialog
        {
            Description = "Carpeta de destino de la copia (debe ser accesible para el servicio de SQL Server)",
            UseDescriptionForTitle = true
        };
        if (dialogo.ShowDialog(this) == DialogResult.OK)
            txtCarpeta.Text = dialogo.SelectedPath;
    }

    private async Task CrearCopiaAsync()
    {
        string carpeta = txtCarpeta.Text.Trim();
        if (carpeta.Length == 0)
        {
            _franja.Mostrar("Indicá la carpeta de destino de la copia.", TipoMensaje.Advertencia);
            txtCarpeta.Focus();
            return;
        }

        SetOcupado(true);
        _franja.Mostrar("Creando y verificando la copia de seguridad. Puede tardar unos minutos...", TipoMensaje.Advertencia);
        try
        {
            var resultado = await Task.Run(() => _backupService.RealizarBackup(carpeta));
            if (IsDisposed) return;

            string tamano = resultado.TamanoBytes.HasValue
                ? $", {(resultado.TamanoBytes.Value / 1024m / 1024m).ToString("N2", CulturaAr)} MB"
                : "";
            string duracion = $"{resultado.Duracion.TotalSeconds.ToString("N1", CulturaAr)} s";

            if (resultado.Verificado)
                _franja.Mostrar($"Copia creada y verificada: {resultado.Ruta} ({duracion}{tamano}).", TipoMensaje.Ok);
            else
                _franja.Mostrar($"La copia se creó en {resultado.Ruta}, pero la verificación falló: {resultado.DetalleVerificacion} No confíes en este archivo.", TipoMensaje.Error);

            await CargarHistorialAsync();
        }
        catch (Exception ex)
        {
            if (!IsDisposed) _franja.Mostrar(ex.Message, TipoMensaje.Error);
        }
        finally
        {
            if (!IsDisposed) SetOcupado(false);
        }
    }

    // ----------------------------------------------------------- restaurar

    private void ElegirArchivo()
    {
        using var dialogo = new OpenFileDialog
        {
            Title = "Elegir copia de seguridad",
            Filter = "Copias de seguridad (*.bak)|*.bak|Todos los archivos (*.*)|*.*",
            CheckFileExists = true
        };
        if (dialogo.ShowDialog(this) == DialogResult.OK)
            txtArchivo.Text = dialogo.FileName;
    }

    private void UsarSeleccionada()
    {
        if (dgvHistorial.CurrentRow?.DataBoundItem is FilaHistorialBackup fila && dgvHistorial.CurrentRow.Selected)
            txtArchivo.Text = fila.Ruta;
        else
            _franja.Mostrar("Seleccioná primero una copia del historial.", TipoMensaje.Advertencia);
    }

    private async Task RestaurarAsync()
    {
        string archivo = txtArchivo.Text.Trim();
        if (archivo.Length == 0)
        {
            _franja.Mostrar("Elegí el archivo .bak a restaurar.", TipoMensaje.Advertencia);
            txtArchivo.Focus();
            return;
        }

        var respuesta = MessageBox.Show(this,
            "Vas a RESTAURAR la base de datos desde:\n\n" + archivo + "\n\n" +
            "Todos los datos actuales serán REEMPLAZADOS por los de la copia y se cerrarán todas las conexiones a la base. " +
            "Los cambios hechos después de esa copia se perderán y la operación no se puede deshacer.\n\n" +
            "¿Querés continuar?",
            "Restaurar copia de seguridad", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
        if (respuesta != DialogResult.Yes) return;

        SetOcupado(true);
        _franja.Mostrar("Restaurando la base de datos. No cierres la aplicación...", TipoMensaje.Advertencia);
        bool restaurada = false;
        try
        {
            await Task.Run(() => _backupService.Restaurar(archivo));
            restaurada = true;
        }
        catch (Exception ex)
        {
            if (!IsDisposed) _franja.Mostrar(ex.Message, TipoMensaje.Error);
        }
        finally
        {
            if (!IsDisposed) SetOcupado(false);
        }

        if (!restaurada || IsDisposed) return;

        // La base cambio por completo (usuarios, medicos, turnos): se vuelve al
        // login, igual que "Cerrar sesion", para empezar con datos nuevos.
        MessageBox.Show(this,
            "La base de datos se restauró correctamente.\n\nSe cerrará la sesión para volver a ingresar con los datos restaurados. " +
            "Si notás algo raro, reiniciá la aplicación.",
            "Restauración completada", MessageBoxButtons.OK, MessageBoxIcon.Information);

        var menu = Parent?.FindForm();
        var login = new FormLogin();
        login.Show();
        menu?.Close();
    }

    // ------------------------------------------------------------ utilidad

    // Mientras corre una operacion nada mas se puede tocar: evita dobles clics.
    private void SetOcupado(bool ocupado)
    {
        foreach (Control c in new Control[] { btnCrear, btnExaminar, btnElegirArchivo, btnUsarSeleccionada, btnRestaurar, txtCarpeta, txtArchivo, dgvHistorial })
            c.Enabled = !ocupado;
        UseWaitCursor = ocupado;
    }
}
