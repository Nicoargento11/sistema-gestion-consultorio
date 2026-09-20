namespace SGC.UI;

partial class FormTurnos
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlFormulario = new Panel();
        ChkFiltrarFecha = new CheckBox();
        ChkFiltrarPaciente = new CheckBox();
        ChkTodosMedicos = new CheckBox();
        BtnNuevoTurno = new Button();
        BtnModificar = new Button();
        ChkMostrarCancelados = new CheckBox();
        LblMensaje = new Label();
        BtnCancelar = new Button();
        BtnConfirmarAsistencia = new Button();
        BtnAsignar = new Button();
        // NUEVO: ComboBox Duracion (min) + Label
        CboDuracion = new ComboBox();
        lblDuracion = new Label();
        CboHorario = new ComboBox();
        lblHorario = new Label();
        DtpFecha = new DateTimePicker();
        lblFecha = new Label();
        CboMedico = new ComboBox();
        lblMedico = new Label();
        CboPaciente = new ComboBox();
        lblPaciente = new Label();
        DgvTurnos = new DataGridView();
        pnlAgenda = new Panel();
        DgvAgenda = new DataGridView();
        lblAgendaTitulo = new Label();
        pnlFormulario.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvTurnos).BeginInit();
        pnlAgenda.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvAgenda).BeginInit();
        SuspendLayout();
        // 
        // pnlFormulario
        // 
        pnlFormulario.BackColor = Color.FromArgb(245, 246, 250);
        pnlFormulario.Controls.Add(lblDuracion);
        pnlFormulario.Controls.Add(CboDuracion);
        pnlFormulario.Controls.Add(ChkFiltrarFecha);
        pnlFormulario.Controls.Add(ChkFiltrarPaciente);
        pnlFormulario.Controls.Add(ChkTodosMedicos);
        pnlFormulario.Controls.Add(BtnNuevoTurno);
        pnlFormulario.Controls.Add(BtnModificar);
        pnlFormulario.Controls.Add(ChkMostrarCancelados);
        pnlFormulario.Controls.Add(LblMensaje);
        pnlFormulario.Controls.Add(BtnCancelar);
        pnlFormulario.Controls.Add(BtnConfirmarAsistencia);
        pnlFormulario.Controls.Add(BtnAsignar);
        pnlFormulario.Controls.Add(CboHorario);
        pnlFormulario.Controls.Add(lblHorario);
        pnlFormulario.Controls.Add(DtpFecha);
        pnlFormulario.Controls.Add(lblFecha);
        pnlFormulario.Controls.Add(CboMedico);
        pnlFormulario.Controls.Add(lblMedico);
        pnlFormulario.Controls.Add(CboPaciente);
        pnlFormulario.Controls.Add(lblPaciente);
        pnlFormulario.Dock = DockStyle.Top;
        pnlFormulario.Location = new Point(0, 0);
        pnlFormulario.Margin = new Padding(2, 3, 2, 3);
        pnlFormulario.Name = "pnlFormulario";
        pnlFormulario.Size = new Size(1040, 152);
        pnlFormulario.TabIndex = 1;
        // 
        // ChkFiltrarFecha
        // 
        ChkFiltrarFecha.AutoSize = true;
        ChkFiltrarFecha.Font = new Font("Segoe UI", 9F);
        ChkFiltrarFecha.Location = new Point(176, 124);
        ChkFiltrarFecha.Margin = new Padding(2, 3, 2, 3);
        ChkFiltrarFecha.Name = "ChkFiltrarFecha";
        ChkFiltrarFecha.Size = new Size(136, 24);
        ChkFiltrarFecha.TabIndex = 10;
        ChkFiltrarFecha.Text = "Filtrar por fecha";
        ChkFiltrarFecha.UseVisualStyleBackColor = true;
        // 
        // ChkFiltrarPaciente
        // 
        ChkFiltrarPaciente.AutoSize = true;
        ChkFiltrarPaciente.Font = new Font("Segoe UI", 9F);
        ChkFiltrarPaciente.Location = new Point(366, 165);
        ChkFiltrarPaciente.Margin = new Padding(2, 3, 2, 3);
        ChkFiltrarPaciente.Name = "ChkFiltrarPaciente";
        ChkFiltrarPaciente.Size = new Size(157, 24);
        ChkFiltrarPaciente.TabIndex = 12;
        ChkFiltrarPaciente.Text = "Filtrar por paciente";
        ChkFiltrarPaciente.UseVisualStyleBackColor = true;
        // 
        // ChkTodosMedicos
        // 
        ChkTodosMedicos.AutoSize = true;
        ChkTodosMedicos.Font = new Font("Segoe UI", 9F);
        ChkTodosMedicos.Location = new Point(488, 32);
        ChkTodosMedicos.Margin = new Padding(2, 3, 2, 3);
        ChkTodosMedicos.Name = "ChkTodosMedicos";
        ChkTodosMedicos.Size = new Size(177, 24);
        ChkTodosMedicos.TabIndex = 9;
        ChkTodosMedicos.Text = "Ver todos los medicos";
        ChkTodosMedicos.UseVisualStyleBackColor = true;
        // 
        // BtnNuevoTurno
        // 
        BtnNuevoTurno.BackColor = Color.FromArgb(120, 130, 145);
        BtnNuevoTurno.FlatAppearance.BorderSize = 0;
        BtnNuevoTurno.FlatStyle = FlatStyle.Flat;
        BtnNuevoTurno.Font = new Font("Segoe UI", 9.5F);
        BtnNuevoTurno.ForeColor = Color.White;
        BtnNuevoTurno.Location = new Point(426, 91);
        BtnNuevoTurno.Margin = new Padding(2, 3, 2, 3);
        BtnNuevoTurno.Name = "BtnNuevoTurno";
        BtnNuevoTurno.Size = new Size(104, 27);
        BtnNuevoTurno.TabIndex = 8;
        BtnNuevoTurno.Text = "Nuevo turno";
        BtnNuevoTurno.UseVisualStyleBackColor = false;
        // 
        // BtnModificar
        // 
        BtnModificar.BackColor = Color.FromArgb(120, 130, 145);
        BtnModificar.FlatAppearance.BorderSize = 0;
        BtnModificar.FlatStyle = FlatStyle.Flat;
        BtnModificar.Font = new Font("Segoe UI", 9.5F);
        BtnModificar.ForeColor = Color.White;
        BtnModificar.Location = new Point(650, 91);
        BtnModificar.Margin = new Padding(2, 3, 2, 3);
        BtnModificar.Name = "BtnModificar";
        BtnModificar.Size = new Size(104, 27);
        BtnModificar.TabIndex = 7;
        BtnModificar.Text = "Modificar";
        BtnModificar.UseVisualStyleBackColor = false;
        // 
        // ChkMostrarCancelados
        // 
        ChkMostrarCancelados.AutoSize = true;
        ChkMostrarCancelados.Font = new Font("Segoe UI", 9F);
        ChkMostrarCancelados.Location = new Point(16, 124);
        ChkMostrarCancelados.Margin = new Padding(2, 3, 2, 3);
        ChkMostrarCancelados.Name = "ChkMostrarCancelados";
        ChkMostrarCancelados.Size = new Size(160, 24);
        ChkMostrarCancelados.TabIndex = 6;
        ChkMostrarCancelados.Text = "Mostrar cancelados";
        ChkMostrarCancelados.UseVisualStyleBackColor = true;
        // 
        // LblMensaje
        // 
        LblMensaje.AutoSize = true;
        LblMensaje.Font = new Font("Segoe UI", 9F);
        LblMensaje.Location = new Point(426, 124);
        LblMensaje.Margin = new Padding(2, 0, 2, 0);
        LblMensaje.MaximumSize = new Size(400, 0);
        LblMensaje.Name = "LblMensaje";
        LblMensaje.Size = new Size(0, 20);
        LblMensaje.TabIndex = 0;
        // 
        // BtnCancelar
        // 
        BtnCancelar.BackColor = Color.FromArgb(200, 60, 60);
        BtnCancelar.FlatAppearance.BorderSize = 0;
        BtnCancelar.FlatStyle = FlatStyle.Flat;
        BtnCancelar.Font = new Font("Segoe UI", 9.5F);
        BtnCancelar.ForeColor = Color.White;
        BtnCancelar.Location = new Point(762, 91);
        BtnCancelar.Margin = new Padding(2, 3, 2, 3);
        BtnCancelar.Name = "BtnCancelar";
        BtnCancelar.Size = new Size(120, 27);
        BtnCancelar.TabIndex = 5;
        BtnCancelar.Text = "Cancelar turno sel.";
        BtnCancelar.UseVisualStyleBackColor = false;
        BtnCancelar.Click += BtnCancelar_Click;
        // 
        // BtnConfirmarAsistencia
        // 
        BtnConfirmarAsistencia.BackColor = Color.FromArgb(60, 160, 100);
        BtnConfirmarAsistencia.FlatAppearance.BorderSize = 0;
        BtnConfirmarAsistencia.FlatStyle = FlatStyle.Flat;
        BtnConfirmarAsistencia.Font = new Font("Segoe UI", 9.5F);
        BtnConfirmarAsistencia.ForeColor = Color.White;
        BtnConfirmarAsistencia.Location = new Point(890, 91);
        BtnConfirmarAsistencia.Margin = new Padding(2, 3, 2, 3);
        BtnConfirmarAsistencia.Name = "BtnConfirmarAsistencia";
        BtnConfirmarAsistencia.Size = new Size(144, 27);
        BtnConfirmarAsistencia.TabIndex = 11;
        BtnConfirmarAsistencia.Text = "Confirmar asistencia";
        BtnConfirmarAsistencia.UseVisualStyleBackColor = false;
        BtnConfirmarAsistencia.Click += BtnConfirmarAsistencia_Click;
        // 
        // BtnAsignar
        // 
        BtnAsignar.BackColor = Color.FromArgb(46, 134, 222);
        BtnAsignar.FlatAppearance.BorderSize = 0;
        BtnAsignar.FlatStyle = FlatStyle.Flat;
        BtnAsignar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnAsignar.ForeColor = Color.White;
        BtnAsignar.Location = new Point(538, 91);
        BtnAsignar.Margin = new Padding(2, 3, 2, 3);
        BtnAsignar.Name = "BtnAsignar";
        BtnAsignar.Size = new Size(104, 27);
        BtnAsignar.TabIndex = 4;
        BtnAsignar.Text = "Asignar turno";
        BtnAsignar.UseVisualStyleBackColor = false;
        BtnAsignar.Click += BtnAsignar_Click;
        // 
        // lblDuracion
        // 
        lblDuracion.AutoSize = true;
        lblDuracion.Font = new Font("Segoe UI", 9F);
        lblDuracion.Location = new Point(326, 72);
        lblDuracion.Margin = new Padding(2, 0, 2, 0);
        lblDuracion.Name = "lblDuracion";
        lblDuracion.Size = new Size(99, 20);
        lblDuracion.TabIndex = 13;
        lblDuracion.Text = "Duracion (min)";
        // 
        // CboDuracion
        // 
        CboDuracion.DropDownStyle = ComboBoxStyle.DropDownList;
        CboDuracion.Font = new Font("Segoe UI", 10F);
        CboDuracion.Location = new Point(326, 91);
        CboDuracion.Margin = new Padding(2, 3, 2, 3);
        CboDuracion.Name = "CboDuracion";
        CboDuracion.Size = new Size(90, 31);
        CboDuracion.TabIndex = 14;
        // 
        // CboHorario
        // 
        CboHorario.DropDownStyle = ComboBoxStyle.DropDownList;
        CboHorario.Font = new Font("Segoe UI", 10F);
        CboHorario.Location = new Point(176, 91);
        CboHorario.Margin = new Padding(2, 3, 2, 3);
        CboHorario.Name = "CboHorario";
        CboHorario.Size = new Size(145, 31);
        CboHorario.TabIndex = 3;
        // 
        // lblHorario
        // 
        lblHorario.AutoSize = true;
        lblHorario.Font = new Font("Segoe UI", 9F);
        lblHorario.Location = new Point(176, 72);
        lblHorario.Margin = new Padding(2, 0, 2, 0);
        lblHorario.Name = "lblHorario";
        lblHorario.Size = new Size(60, 20);
        lblHorario.TabIndex = 6;
        lblHorario.Text = "Horario";
        // 
        // DtpFecha
        // 
        DtpFecha.Font = new Font("Segoe UI", 10F);
        DtpFecha.Format = DateTimePickerFormat.Short;
        DtpFecha.Location = new Point(16, 91);
        DtpFecha.Margin = new Padding(2, 3, 2, 3);
        DtpFecha.Name = "DtpFecha";
        DtpFecha.Size = new Size(145, 30);
        DtpFecha.TabIndex = 2;
        // 
        // lblFecha
        // 
        lblFecha.AutoSize = true;
        lblFecha.Font = new Font("Segoe UI", 9F);
        lblFecha.Location = new Point(16, 72);
        lblFecha.Margin = new Padding(2, 0, 2, 0);
        lblFecha.Name = "lblFecha";
        lblFecha.Size = new Size(47, 20);
        lblFecha.TabIndex = 7;
        lblFecha.Text = "Fecha";
        // 
        // CboMedico
        // 
        CboMedico.DropDownStyle = ComboBoxStyle.DropDownList;
        CboMedico.Font = new Font("Segoe UI", 10F);
        CboMedico.Location = new Point(240, 31);
        CboMedico.Margin = new Padding(2, 3, 2, 3);
        CboMedico.Name = "CboMedico";
        CboMedico.Size = new Size(241, 31);
        CboMedico.TabIndex = 1;
        // 
        // lblMedico
        // 
        lblMedico.AutoSize = true;
        lblMedico.Font = new Font("Segoe UI", 9F);
        lblMedico.Location = new Point(240, 12);
        lblMedico.Margin = new Padding(2, 0, 2, 0);
        lblMedico.Name = "lblMedico";
        lblMedico.Size = new Size(59, 20);
        lblMedico.TabIndex = 8;
        lblMedico.Text = "Medico";
        // 
        // CboPaciente
        // 
        CboPaciente.DropDownStyle = ComboBoxStyle.DropDownList;
        CboPaciente.Font = new Font("Segoe UI", 10F);
        CboPaciente.Location = new Point(16, 31);
        CboPaciente.Margin = new Padding(2, 3, 2, 3);
        CboPaciente.Name = "CboPaciente";
        CboPaciente.Size = new Size(209, 31);
        CboPaciente.TabIndex = 0;
        // 
        // lblPaciente
        // 
        lblPaciente.AutoSize = true;
        lblPaciente.Font = new Font("Segoe UI", 9F);
        lblPaciente.Location = new Point(16, 12);
        lblPaciente.Margin = new Padding(2, 0, 2, 0);
        lblPaciente.Name = "lblPaciente";
        lblPaciente.Size = new Size(64, 20);
        lblPaciente.TabIndex = 9;
        lblPaciente.Text = "Paciente";
        // 
        // DgvTurnos
        // 
        DgvTurnos.AllowUserToAddRows = false;
        DgvTurnos.BackgroundColor = Color.White;
        DgvTurnos.ColumnHeadersHeight = 34;
        DgvTurnos.Dock = DockStyle.Fill;
        DgvTurnos.Font = new Font("Segoe UI", 9.5F);
        DgvTurnos.Location = new Point(0, 152);
        DgvTurnos.Margin = new Padding(2, 3, 2, 3);
        DgvTurnos.MultiSelect = false;
        DgvTurnos.Name = "DgvTurnos";
        DgvTurnos.ReadOnly = true;
        DgvTurnos.RowHeadersWidth = 62;
        DgvTurnos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvTurnos.Size = new Size(800, 368);
        DgvTurnos.TabIndex = 0;
        // 
        // pnlAgenda
        // 
        pnlAgenda.BackColor = Color.FromArgb(245, 246, 250);
        pnlAgenda.Controls.Add(DgvAgenda);
        pnlAgenda.Controls.Add(lblAgendaTitulo);
        pnlAgenda.Dock = DockStyle.Right;
        pnlAgenda.Location = new Point(800, 152);
        pnlAgenda.Margin = new Padding(2, 3, 2, 3);
        pnlAgenda.Name = "pnlAgenda";
        pnlAgenda.Padding = new Padding(8);
        pnlAgenda.Size = new Size(240, 368);
        pnlAgenda.TabIndex = 2;
        // 
        // DgvAgenda
        // 
        DgvAgenda.AllowUserToAddRows = false;
        DgvAgenda.BackgroundColor = Color.White;
        DgvAgenda.ColumnHeadersHeight = 34;
        DgvAgenda.Dock = DockStyle.Fill;
        DgvAgenda.Font = new Font("Segoe UI", 9.5F);
        DgvAgenda.Location = new Point(8, 38);
        DgvAgenda.Margin = new Padding(2, 3, 2, 3);
        DgvAgenda.Name = "DgvAgenda";
        DgvAgenda.ReadOnly = true;
        DgvAgenda.RowHeadersWidth = 20;
        DgvAgenda.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvAgenda.Size = new Size(224, 322);
        DgvAgenda.TabIndex = 1;
        // 
        // lblAgendaTitulo
        // 
        lblAgendaTitulo.AutoSize = true;
        lblAgendaTitulo.Dock = DockStyle.Top;
        lblAgendaTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblAgendaTitulo.ForeColor = Color.FromArgb(27, 42, 74);
        lblAgendaTitulo.Location = new Point(8, 8);
        lblAgendaTitulo.Margin = new Padding(2, 0, 2, 0);
        lblAgendaTitulo.Name = "lblAgendaTitulo";
        lblAgendaTitulo.Padding = new Padding(0, 0, 0, 7);
        lblAgendaTitulo.Size = new Size(128, 30);
        lblAgendaTitulo.TabIndex = 0;
        lblAgendaTitulo.Text = "Disponibilidad";
        // 
        // FormTurnos
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1040, 520);
        Controls.Add(DgvTurnos);
        Controls.Add(pnlAgenda);
        Controls.Add(pnlFormulario);
        Margin = new Padding(2, 3, 2, 3);
        Name = "FormTurnos";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Gestion de Turnos";
        pnlFormulario.ResumeLayout(false);
        pnlFormulario.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)DgvTurnos).EndInit();
        pnlAgenda.ResumeLayout(false);
        pnlAgenda.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)DgvAgenda).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlFormulario;
    private Panel pnlAgenda;
    private Label lblAgendaTitulo;
    private DataGridView DgvAgenda;
    private Button BtnNuevoTurno;
    private CheckBox ChkTodosMedicos;
    private CheckBox ChkFiltrarFecha;
    private CheckBox ChkFiltrarPaciente;
    private Button BtnModificar;
    private CheckBox ChkMostrarCancelados;
    private Label lblPaciente;
    private ComboBox CboPaciente;
    private Label lblMedico;
    private ComboBox CboMedico;
    private Label lblFecha;
    private DateTimePicker DtpFecha;
    private Label lblHorario;
    private ComboBox CboHorario;
    private Button BtnAsignar;
    private Button BtnCancelar;
    private Button BtnConfirmarAsistencia;
    private Label LblMensaje;
    private DataGridView DgvTurnos;
    private Label lblDuracion;
    private ComboBox CboDuracion;
}
