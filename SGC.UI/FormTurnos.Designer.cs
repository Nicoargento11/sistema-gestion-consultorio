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
        lblDuracion = new Label();
        CboDuracion = new ComboBox();
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
        lblAgendaHint = new Label();
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
        pnlFormulario.Margin = new Padding(1, 2, 1, 2);
        pnlFormulario.Name = "pnlFormulario";
        pnlFormulario.Size = new Size(910, 114);
        pnlFormulario.TabIndex = 1;
        // 
        // lblDuracion
        // 
        lblDuracion.AutoSize = true;
        lblDuracion.Font = new Font("Segoe UI", 9F);
        lblDuracion.Location = new Point(286, 54);
        lblDuracion.Margin = new Padding(1, 0, 1, 0);
        lblDuracion.Name = "lblDuracion";
        lblDuracion.Size = new Size(87, 15);
        lblDuracion.TabIndex = 13;
        lblDuracion.Text = "Duracion (min)";
        // 
        // CboDuracion
        // 
        CboDuracion.DropDownStyle = ComboBoxStyle.DropDownList;
        CboDuracion.Font = new Font("Segoe UI", 10F);
        CboDuracion.Location = new Point(286, 68);
        CboDuracion.Margin = new Padding(1, 2, 1, 2);
        CboDuracion.Name = "CboDuracion";
        CboDuracion.Size = new Size(80, 25);
        CboDuracion.TabIndex = 14;
        // 
        // ChkFiltrarFecha
        // 
        ChkFiltrarFecha.AutoSize = true;
        ChkFiltrarFecha.Font = new Font("Segoe UI", 9F);
        ChkFiltrarFecha.Location = new Point(154, 93);
        ChkFiltrarFecha.Margin = new Padding(1, 2, 1, 2);
        ChkFiltrarFecha.Name = "ChkFiltrarFecha";
        ChkFiltrarFecha.Size = new Size(109, 19);
        ChkFiltrarFecha.TabIndex = 10;
        ChkFiltrarFecha.Text = "Filtrar por fecha";
        ChkFiltrarFecha.UseVisualStyleBackColor = true;
        // 
        // ChkFiltrarPaciente
        // 
        ChkFiltrarPaciente.AutoSize = true;
        ChkFiltrarPaciente.Font = new Font("Segoe UI", 9F);
        ChkFiltrarPaciente.Location = new Point(321, 124);
        ChkFiltrarPaciente.Margin = new Padding(1, 2, 1, 2);
        ChkFiltrarPaciente.Name = "ChkFiltrarPaciente";
        ChkFiltrarPaciente.Size = new Size(125, 19);
        ChkFiltrarPaciente.TabIndex = 12;
        ChkFiltrarPaciente.Text = "Filtrar por paciente";
        ChkFiltrarPaciente.UseVisualStyleBackColor = true;
        // 
        // ChkTodosMedicos
        // 
        ChkTodosMedicos.AutoSize = true;
        ChkTodosMedicos.Font = new Font("Segoe UI", 9F);
        ChkTodosMedicos.Location = new Point(427, 24);
        ChkTodosMedicos.Margin = new Padding(1, 2, 1, 2);
        ChkTodosMedicos.Name = "ChkTodosMedicos";
        ChkTodosMedicos.Size = new Size(141, 19);
        ChkTodosMedicos.TabIndex = 9;
        ChkTodosMedicos.Text = "Ver todos los medicos";
        ChkTodosMedicos.UseVisualStyleBackColor = true;
        // 
        // BtnNuevoTurno
        // 
        BtnNuevoTurno.BackColor = Color.FromArgb(46, 134, 222);
        BtnNuevoTurno.FlatAppearance.BorderSize = 0;
        BtnNuevoTurno.FlatStyle = FlatStyle.Flat;
        BtnNuevoTurno.Font = new Font("Segoe UI", 9.5F);
        BtnNuevoTurno.ForeColor = Color.White;
        BtnNuevoTurno.Location = new Point(372, 68);
        BtnNuevoTurno.Margin = new Padding(1, 2, 1, 2);
        BtnNuevoTurno.Name = "BtnNuevoTurno";
        BtnNuevoTurno.Size = new Size(91, 20);
        BtnNuevoTurno.TabIndex = 20;
        BtnNuevoTurno.Text = "Limpiar";
        BtnNuevoTurno.UseVisualStyleBackColor = false;
        // 
        // BtnModificar
        // 
        BtnModificar.BackColor = Color.FromArgb(230, 145, 45);
        BtnModificar.FlatAppearance.BorderSize = 0;
        BtnModificar.FlatStyle = FlatStyle.Flat;
        BtnModificar.Font = new Font("Segoe UI", 9.5F);
        BtnModificar.ForeColor = Color.White;
        BtnModificar.Location = new Point(601, 68);
        BtnModificar.Margin = new Padding(1, 2, 1, 2);
        BtnModificar.Name = "BtnModificar";
        BtnModificar.Size = new Size(91, 20);
        BtnModificar.TabIndex = 22;
        BtnModificar.Text = "Modificar";
        BtnModificar.UseVisualStyleBackColor = false;
        // 
        // ChkMostrarCancelados
        // 
        ChkMostrarCancelados.AutoSize = true;
        ChkMostrarCancelados.Font = new Font("Segoe UI", 9F);
        ChkMostrarCancelados.Location = new Point(14, 93);
        ChkMostrarCancelados.Margin = new Padding(1, 2, 1, 2);
        ChkMostrarCancelados.Name = "ChkMostrarCancelados";
        ChkMostrarCancelados.Size = new Size(129, 19);
        ChkMostrarCancelados.TabIndex = 6;
        ChkMostrarCancelados.Text = "Mostrar cancelados";
        ChkMostrarCancelados.UseVisualStyleBackColor = true;
        // 
        // LblMensaje
        // 
        LblMensaje.AutoSize = true;
        LblMensaje.Font = new Font("Segoe UI", 9F);
        LblMensaje.Location = new Point(372, 93);
        LblMensaje.Margin = new Padding(1, 0, 1, 0);
        LblMensaje.MaximumSize = new Size(350, 0);
        LblMensaje.Name = "LblMensaje";
        LblMensaje.Size = new Size(0, 15);
        LblMensaje.TabIndex = 0;
        // 
        // BtnCancelar
        // 
        BtnCancelar.BackColor = Color.FromArgb(231, 76, 60);
        BtnCancelar.FlatAppearance.BorderSize = 0;
        BtnCancelar.FlatStyle = FlatStyle.Flat;
        BtnCancelar.Font = new Font("Segoe UI", 9.5F);
        BtnCancelar.ForeColor = Color.White;
        BtnCancelar.Location = new Point(699, 68);
        BtnCancelar.Margin = new Padding(1, 2, 1, 2);
        BtnCancelar.Name = "BtnCancelar";
        BtnCancelar.Size = new Size(132, 20);
        BtnCancelar.TabIndex = 23;
        BtnCancelar.Text = "Cancelar turno";
        BtnCancelar.UseVisualStyleBackColor = false;
        BtnCancelar.Click += BtnCancelar_Click;
        // 
        // BtnConfirmarAsistencia
        // 
        BtnConfirmarAsistencia.BackColor = Color.FromArgb(39, 174, 96);
        BtnConfirmarAsistencia.FlatAppearance.BorderSize = 0;
        BtnConfirmarAsistencia.FlatStyle = FlatStyle.Flat;
        BtnConfirmarAsistencia.Font = new Font("Segoe UI", 9.5F);
        BtnConfirmarAsistencia.ForeColor = Color.White;
        BtnConfirmarAsistencia.Location = new Point(836, 68);
        BtnConfirmarAsistencia.Margin = new Padding(1, 2, 1, 2);
        BtnConfirmarAsistencia.Name = "BtnConfirmarAsistencia";
        BtnConfirmarAsistencia.Size = new Size(158, 20);
        BtnConfirmarAsistencia.TabIndex = 24;
        BtnConfirmarAsistencia.Text = "Confirmar asistencia";
        BtnConfirmarAsistencia.UseVisualStyleBackColor = false;
        BtnConfirmarAsistencia.Click += BtnConfirmarAsistencia_Click;
        // 
        // BtnAsignar
        // 
        BtnAsignar.BackColor = Color.FromArgb(39, 174, 96);
        BtnAsignar.FlatAppearance.BorderSize = 0;
        BtnAsignar.FlatStyle = FlatStyle.Flat;
        BtnAsignar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnAsignar.ForeColor = Color.White;
        BtnAsignar.Location = new Point(470, 68);
        BtnAsignar.Margin = new Padding(1, 2, 1, 2);
        BtnAsignar.Name = "BtnAsignar";
        BtnAsignar.Size = new Size(122, 20);
        BtnAsignar.TabIndex = 21;
        BtnAsignar.Text = "Asignar turno";
        BtnAsignar.UseVisualStyleBackColor = false;
        BtnAsignar.Click += BtnAsignar_Click;
        // 
        // CboHorario
        // 
        CboHorario.DropDownStyle = ComboBoxStyle.DropDownList;
        CboHorario.Font = new Font("Segoe UI", 10F);
        CboHorario.Location = new Point(154, 68);
        CboHorario.Margin = new Padding(1, 2, 1, 2);
        CboHorario.Name = "CboHorario";
        CboHorario.Size = new Size(127, 25);
        CboHorario.TabIndex = 3;
        // 
        // lblHorario
        // 
        lblHorario.AutoSize = true;
        lblHorario.Font = new Font("Segoe UI", 9F);
        lblHorario.Location = new Point(154, 54);
        lblHorario.Margin = new Padding(1, 0, 1, 0);
        lblHorario.Name = "lblHorario";
        lblHorario.Size = new Size(47, 15);
        lblHorario.TabIndex = 6;
        lblHorario.Text = "Horario";
        // 
        // DtpFecha
        // 
        DtpFecha.Font = new Font("Segoe UI", 10F);
        DtpFecha.Format = DateTimePickerFormat.Short;
        DtpFecha.Location = new Point(14, 68);
        DtpFecha.Margin = new Padding(1, 2, 1, 2);
        DtpFecha.Name = "DtpFecha";
        DtpFecha.Size = new Size(127, 25);
        DtpFecha.TabIndex = 2;
        // 
        // lblFecha
        // 
        lblFecha.AutoSize = true;
        lblFecha.Font = new Font("Segoe UI", 9F);
        lblFecha.Location = new Point(14, 54);
        lblFecha.Margin = new Padding(1, 0, 1, 0);
        lblFecha.Name = "lblFecha";
        lblFecha.Size = new Size(38, 15);
        lblFecha.TabIndex = 7;
        lblFecha.Text = "Fecha";
        // 
        // CboMedico
        // 
        CboMedico.DropDownStyle = ComboBoxStyle.DropDownList;
        CboMedico.Font = new Font("Segoe UI", 10F);
        CboMedico.Location = new Point(210, 23);
        CboMedico.Margin = new Padding(1, 2, 1, 2);
        CboMedico.Name = "CboMedico";
        CboMedico.Size = new Size(211, 25);
        CboMedico.TabIndex = 1;
        // 
        // lblMedico
        // 
        lblMedico.AutoSize = true;
        lblMedico.Font = new Font("Segoe UI", 9F);
        lblMedico.Location = new Point(210, 9);
        lblMedico.Margin = new Padding(1, 0, 1, 0);
        lblMedico.Name = "lblMedico";
        lblMedico.Size = new Size(47, 15);
        lblMedico.TabIndex = 8;
        lblMedico.Text = "Medico";
        // 
        // CboPaciente
        // 
        CboPaciente.DropDownStyle = ComboBoxStyle.DropDownList;
        CboPaciente.Font = new Font("Segoe UI", 10F);
        CboPaciente.Location = new Point(14, 23);
        CboPaciente.Margin = new Padding(1, 2, 1, 2);
        CboPaciente.Name = "CboPaciente";
        CboPaciente.Size = new Size(183, 25);
        CboPaciente.TabIndex = 0;
        // 
        // lblPaciente
        // 
        lblPaciente.AutoSize = true;
        lblPaciente.Font = new Font("Segoe UI", 9F);
        lblPaciente.Location = new Point(14, 9);
        lblPaciente.Margin = new Padding(1, 0, 1, 0);
        lblPaciente.Name = "lblPaciente";
        lblPaciente.Size = new Size(52, 15);
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
        DgvTurnos.Location = new Point(0, 114);
        DgvTurnos.Margin = new Padding(1, 2, 1, 2);
        DgvTurnos.MultiSelect = false;
        DgvTurnos.Name = "DgvTurnos";
        DgvTurnos.ReadOnly = true;
        DgvTurnos.RowHeadersWidth = 62;
        DgvTurnos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvTurnos.Size = new Size(700, 276);
        DgvTurnos.TabIndex = 0;
        // 
        // pnlAgenda
        // 
        pnlAgenda.BackColor = Color.FromArgb(245, 246, 250);
        pnlAgenda.Controls.Add(DgvAgenda);
        pnlAgenda.Controls.Add(lblAgendaTitulo);
        pnlAgenda.Controls.Add(lblAgendaHint);
        pnlAgenda.Dock = DockStyle.Right;
        pnlAgenda.Location = new Point(700, 114);
        pnlAgenda.Margin = new Padding(1, 2, 1, 2);
        pnlAgenda.Name = "pnlAgenda";
        pnlAgenda.Padding = new Padding(7, 6, 7, 6);
        pnlAgenda.Size = new Size(210, 276);
        pnlAgenda.TabIndex = 2;
        // 
        // DgvAgenda
        // 
        DgvAgenda.AllowUserToAddRows = false;
        DgvAgenda.BackgroundColor = Color.White;
        DgvAgenda.ColumnHeadersHeight = 34;
        DgvAgenda.Dock = DockStyle.Fill;
        DgvAgenda.Font = new Font("Segoe UI", 9.5F);
        DgvAgenda.Location = new Point(7, 48);
        DgvAgenda.Margin = new Padding(1, 2, 1, 2);
        DgvAgenda.Name = "DgvAgenda";
        DgvAgenda.ReadOnly = true;
        DgvAgenda.RowHeadersWidth = 20;
        DgvAgenda.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvAgenda.Size = new Size(196, 222);
        DgvAgenda.TabIndex = 1;
        // 
        // lblAgendaTitulo
        // 
        lblAgendaTitulo.AutoSize = true;
        lblAgendaTitulo.Dock = DockStyle.Top;
        lblAgendaTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblAgendaTitulo.ForeColor = Color.FromArgb(27, 42, 74);
        lblAgendaTitulo.Location = new Point(7, 24);
        lblAgendaTitulo.Margin = new Padding(1, 0, 1, 0);
        lblAgendaTitulo.Name = "lblAgendaTitulo";
        lblAgendaTitulo.Padding = new Padding(0, 0, 0, 5);
        lblAgendaTitulo.Size = new Size(106, 24);
        lblAgendaTitulo.TabIndex = 0;
        lblAgendaTitulo.Text = "Disponibilidad";
        // 
        // lblAgendaHint
        // 
        lblAgendaHint.AutoSize = true;
        lblAgendaHint.Dock = DockStyle.Top;
        lblAgendaHint.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
        lblAgendaHint.ForeColor = Color.FromArgb(110, 120, 135);
        lblAgendaHint.Location = new Point(7, 6);
        lblAgendaHint.Margin = new Padding(1, 0, 1, 0);
        lblAgendaHint.Name = "lblAgendaHint";
        lblAgendaHint.Padding = new Padding(0, 0, 0, 5);
        lblAgendaHint.Size = new Size(181, 18);
        lblAgendaHint.TabIndex = 2;
        lblAgendaHint.Text = "Clic en un horario para seleccionarlo";
        // 
        // FormTurnos
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(910, 390);
        Controls.Add(DgvTurnos);
        Controls.Add(pnlAgenda);
        Controls.Add(pnlFormulario);
        Margin = new Padding(1, 2, 1, 2);
        Name = "FormTurnos";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "d";
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
    private Label lblAgendaHint;
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
