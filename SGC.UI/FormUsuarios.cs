using SGC.Entidades;
using SGC.Logica;

namespace SGC.UI;

public partial class FormUsuarios : Form
{
    private readonly UsuarioService _service = new();
    private readonly MedicoService _medicoService = new();
    private int? _idSeleccionado = null;

    private static readonly Color ColorEliminar = Color.FromArgb(231, 76, 60);
    private static readonly Color ColorReactivar = Color.FromArgb(39, 174, 96);

    public FormUsuarios()
    {
        InitializeComponent();
        ConfigurarColumnas();
        CargarCombos();
        CargarGrilla();
        ActualizarVisibilidadPorRol();
        AcceptButton = BtnGuardar;
        TxtBuscar.TextChanged += (s, e) => CargarGrilla();
        ChkMostrarInactivos.CheckedChanged += (s, e) => CargarGrilla();
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
        DgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstado", HeaderText = "Estado", DataPropertyName = "EstadoTexto", FillWeight = 80 });
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
        var usuarios = _service.ObtenerTodos(ChkMostrarInactivos.Checked);
        var filtro = TxtBuscar.Text?.Trim() ?? "";
        if (!string.IsNullOrWhiteSpace(filtro))
        {
            var f = filtro.ToLower();
            usuarios = usuarios.Where(u =>
                u.NombreUsuario.ToLower().Contains(f) ||
                u.Rol.ToString().ToLower().Contains(f) ||
                u.MedicoAsignadoNombre.ToLower().Contains(f) ||
                u.MedicosAsignadosTexto.ToLower().Contains(f)).ToList();
        }

        DgvUsuarios.DataSource = usuarios;
    }

    private void ActualizarVisibilidadPorRol()
    {
        // Si no hay rol seleccionado (ej: recien apretaste "+ Nuevo"), se
        // ocultan los dos - antes se quedaba pegado el que estuviera visible
        // del usuario anterior que tenias seleccionado.
        if (CboRol.SelectedItem is RolUsuario rol)
        {
            CboMedico.Visible = rol == RolUsuario.Medico;
            ClbMedicosAsignados.Visible = rol == RolUsuario.Recepcionista;
        }
        else
        {
            CboMedico.Visible = false;
            ClbMedicosAsignados.Visible = false;
        }
    }

    private void BtnNuevo_Click(object sender, EventArgs e)
    {
        _idSeleccionado = null;
        TxtNombreUsuario.Text = "";
        TxtContrasena.Text = "";
        CboRol.SelectedIndex = -1;
        CboMedico.SelectedIndex = -1;
        for (int i = 0; i < ClbMedicosAsignados.Items.Count; i++)
        {
            ClbMedicosAsignados.SetItemChecked(i, false);
        }
        ActualizarVisibilidadPorRol();

        BtnEliminar.Text = "Eliminar";
        BtnEliminar.BackColor = ColorEliminar;
    }

    private void BtnGuardar_Click(object sender, EventArgs e)
    {
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

    // Un solo boton que alterna entre "Eliminar" (usuario activo
    // seleccionado) y "Reactivar" (usuario inactivo seleccionado) - ver
    // DgvUsuarios_SelectionChanged, que le cambia texto/color.
    private void BtnEliminar_Click(object sender, EventArgs e)
    {
        if (DgvUsuarios.CurrentRow == null)
        {
            LblMensaje.ForeColor = Color.Red;
            LblMensaje.Text = "Seleccione un usuario de la lista primero.";
            return;
        }

        var usuarioSeleccionado = (Usuario)DgvUsuarios.CurrentRow.DataBoundItem;

        if (usuarioSeleccionado.Activo)
        {
            var respuesta = MessageBox.Show(
                $"¿Está seguro que desea eliminar al usuario \"{usuarioSeleccionado.NombreUsuario}\"?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
        else
        {
            var respuesta = MessageBox.Show(
                $"¿Reactivar al usuario \"{usuarioSeleccionado.NombreUsuario}\"?",
                "Confirmar reactivación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta != DialogResult.Yes) return;

            try
            {
                _service.Reactivar(usuarioSeleccionado.Id);
                CargarGrilla();
                BtnNuevo_Click(sender, e);
                LblMensaje.ForeColor = Color.Green;
                LblMensaje.Text = "Usuario reactivado correctamente.";
            }
            catch (Exception ex)
            {
                LblMensaje.ForeColor = Color.Red;
                LblMensaje.Text = $"Error: {ex.Message}";
            }
        }
    }

    private void DgvUsuarios_SelectionChanged(object sender, EventArgs e)
    {
        if (DgvUsuarios.CurrentRow == null) return;

        var usuarioSeleccionado = (Usuario)DgvUsuarios.CurrentRow.DataBoundItem;
        _idSeleccionado = usuarioSeleccionado.Id;
        TxtNombreUsuario.Text = usuarioSeleccionado.NombreUsuario;

        // Nunca se prellena: el valor guardado es un hash, no la contrasena
        // real. Dejarlo en blanco significa "no cambiarla" al guardar.
        TxtContrasena.Text = "";
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

        ActualizarVisibilidadPorRol();

        BtnEliminar.Text = usuarioSeleccionado.Activo ? "Eliminar" : "Reactivar";
        BtnEliminar.BackColor = usuarioSeleccionado.Activo ? ColorEliminar : ColorReactivar;
    }

    private void CboRol_SelectedIndexChanged(object sender, EventArgs e)
    {
        ActualizarVisibilidadPorRol();
    }
}
