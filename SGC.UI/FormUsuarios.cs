using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormUsuarios : Form
{
    private readonly UsuarioService _service = new();
    private readonly MedicoService _medicoService = new();
    private int? _idSeleccionado = null;

    public FormUsuarios()
    {
        InitializeComponent();
        ConfigurarColumnas();
        CargarCombos();
        CargarGrilla();
        ActualizarVisibilidadPorRol();
    }

    private void ConfigurarColumnas()
    {
        // Mismo patron que FormMedicos/FormConfigurarAgenda: columnas en codigo,
        // DataPropertyName apunta a propiedades aplanadas (MedicoAsignadoNombre,
        // MedicosAsignadosTexto) porque el DataGridView no soporta rutas anidadas.
        DgvUsuarios.AutoGenerateColumns = false;
        DgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNombreUsuario", HeaderText = "Usuario", DataPropertyName = "NombreUsuario", FillWeight = 130 });
        DgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRol", HeaderText = "Rol", DataPropertyName = "Rol", FillWeight = 100 });
        DgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedico", HeaderText = "Medico (si Rol=Medico)", DataPropertyName = "MedicoAsignadoNombre", FillWeight = 170 });
        DgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMedicosAsignados", HeaderText = "Medicos asignados (si Rol=Recepcionista)", DataPropertyName = "MedicosAsignadosTexto", FillWeight = 220 });
    }

    private void CargarCombos()
    {
        CboRol.DataSource = Enum.GetValues(typeof(RolUsuario));

        CboMedico.DataSource = _medicoService.ObtenerTodos();
        CboMedico.DisplayMember = "NombreCompleto";
        CboMedico.ValueMember = "Id";

        ClbMedicosAsignados.DataSource = _medicoService.ObtenerTodos();
        ClbMedicosAsignados.DisplayMember = "NombreCompleto";
        ClbMedicosAsignados.ValueMember = "Id";
    }

    private void CargarGrilla()
    {
        DgvUsuarios.DataSource = _service.ObtenerTodos();
    }

    private void ActualizarVisibilidadPorRol()
    {
        // TODO: mostrar CboMedico solo si CboRol.SelectedItem es RolUsuario.Medico,
        // y ClbMedicosAsignados solo si es RolUsuario.Recepcionista. Si es
        // Administrador, ocultar los dos. Usa esto tanto al cargar el form como
        // desde CboRol_SelectedIndexChanged.
        if (CboRol.SelectedItem is RolUsuario rol)
        {
            CboMedico.Visible = rol == RolUsuario.Medico;
            ClbMedicosAsignados.Visible = rol == RolUsuario.Recepcionista;
        }

    }

    private void BtnNuevo_Click(object sender, EventArgs e)
    {
        // TODO: limpiar _idSeleccionado, TxtNombreUsuario, TxtContrasena,
        // CboRol, CboMedico y los checks de ClbMedicosAsignados (mismo patron
        // que BtnNuevo_Click en FormMedicos/FormConfigurarAgenda).
        _idSeleccionado = null;
        TxtNombreUsuario.Text = "";
        TxtContrasena.Text = "";
        CboRol.SelectedIndex = -1;
        CboMedico.SelectedIndex = -1;
        for (int i = 0; i < ClbMedicosAsignados.Items.Count ; i++)
        {
            ClbMedicosAsignados.SetItemChecked(i, false);
        }
    }

    private void BtnGuardar_Click(object sender, EventArgs e)
    {
        // TODO: armar un Usuario con TxtNombreUsuario.Text, TxtContrasena.Text,
        // (RolUsuario)CboRol.SelectedItem, y segun el rol:
        //  - Medico: MedicoId = (int)CboMedico.SelectedValue
        //  - Recepcionista: MedicosAsignadosIds = los items marcados de
        //    ClbMedicosAsignados (mira ClbMedicosAsignados.CheckedItems, cada
        //    item es un Medico porque le pusimos DataSource+ValueMember).
        // Llamar a _service.Agregar(...) o _service.Modificar(...) segun
        // _idSeleccionado, con try/catch y feedback en LblMensaje.

        try
        {
            if (string.IsNullOrWhiteSpace(TxtNombreUsuario.Text))
            {
                throw new ArgumentException("El nombre de usuario es obligatorio.");
            }
            if (CboRol.SelectedItem == null)
            {
                throw new ArgumentException("Debe seleccionar un rol.");
            }


            var usuario = new Usuario
            {
                Id = _idSeleccionado ?? 0,
                NombreUsuario = TxtNombreUsuario.Text,
                Contrasena = TxtContrasena.Text,
                Rol = (RolUsuario)CboRol.SelectedItem
            };
            if (usuario.Rol == RolUsuario.Medico)
            {
                if (CboMedico.SelectedValue == null) throw new ArgumentException("Debe seleccionar un medico.");
                usuario.MedicoId = (int)CboMedico.SelectedValue;
            }
            else if (usuario.Rol == RolUsuario.Recepcionista)
            {
                usuario.MedicosAsignadosIds = ClbMedicosAsignados.CheckedItems.Cast<Medico>().Select(m => m.Id).ToList();
            }
            if (_idSeleccionado == null)
            {
                _service.Agregar(usuario);
            }
            else
            {
                _service.Modificar(usuario);
            }
            CargarGrilla();
            BtnNuevo_Click(sender, e);
            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Usuario guardado correctamente.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = $"Error: {ex.Message}";
        }
    }

    private void BtnEliminar_Click(object sender, EventArgs e)
    {
        // TODO: validar seleccion en DgvUsuarios, confirmar con MessageBox,
        // y llamar a _service.EliminarLogico(...) (mismo patron que las
        // otras pantallas ABM).
        if (DgvUsuarios.CurrentRow == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un usuario de la lista primero.";
            return;
        }

        var usuarioSeleccionado = (Usuario)DgvUsuarios.CurrentRow.DataBoundItem;
        var respuesta = MessageBox.Show("�Est� seguro que desea eliminar el usuario seleccionado?", "Confirmar eliminaci�n", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (respuesta != DialogResult.Yes) return;
        try
        {
            _service.EliminarLogico(usuarioSeleccionado.Id);
            CargarGrilla();
            BtnNuevo_Click(sender, e);
            LblMensaje.ForeColor = Color.Green;
            LblMensaje.Text = "Usuario eliminado correctamente.";
        }
        catch (Exception ex)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = $"Error: {ex.Message}";


        }    
        }


    private void DgvUsuarios_SelectionChanged(object sender, EventArgs e)
    {
        // TODO: al seleccionar una fila, cargar TxtNombreUsuario, TxtContrasena,
        // CboRol, CboMedico y los checks de ClbMedicosAsignados con los datos
        // del Usuario seleccionado, y guardar el Id en _idSeleccionado.
        if (DgvUsuarios.CurrentRow == null) return;
        var usuarioSeleccionado = (Usuario)DgvUsuarios.CurrentRow.DataBoundItem;
        _idSeleccionado = usuarioSeleccionado.Id;
        TxtNombreUsuario.Text = usuarioSeleccionado.NombreUsuario;
        TxtContrasena.Text = usuarioSeleccionado.Contrasena;
        CboRol.SelectedItem = usuarioSeleccionado.Rol;
        if (usuarioSeleccionado.Rol == RolUsuario.Medico)
        {
            CboMedico.SelectedValue = usuarioSeleccionado.MedicoId;
        }
        else if (usuarioSeleccionado.Rol == RolUsuario.Recepcionista)
        {
            for (int i = 0; i < ClbMedicosAsignados.Items.Count; i++)
            {
                var medico = (Medico)ClbMedicosAsignados.Items[i];
                ClbMedicosAsignados.SetItemChecked(i, usuarioSeleccionado.MedicosAsignadosIds.Contains(medico.Id));
            }
            }
        }

    private void CboRol_SelectedIndexChanged(object sender, EventArgs e)
    {
        ActualizarVisibilidadPorRol();
    }
}
