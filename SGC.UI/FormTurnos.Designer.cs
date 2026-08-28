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
        BtnNuevoTurno = new Button();
        BtnModificar = new Button();
        ChkTodosMedicos = new CheckBox();
        ChkFiltrarFecha = new CheckBox();
        ChkMostrarCancelados = new CheckBox();
        LblMensaje = new Label();
        BtnCancelar = new Button();
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
        lblAgendaTitulo = new Label();
        DgvAgenda = new DataGridView();
        pnlFormulario.SuspendLayout();
        pnlAgenda.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DgvTurnos).BeginInit();
        ((System.ComponentModel.ISupportInitialize)DgvAgenda).BeginInit();
        SuspendLayout();
        // 
        // pnlFormulario
        // 
        pnlFormulario.BackColor = Color.FromArgb(245, 246, 250);
        pnlFormulario.Controls.Add(ChkFiltrarFecha);
        pnlFormulario.Controls.Add(ChkTodosMedicos);
        pnlFormulario.Controls.Add(BtnNuevoTurno);
        pnlFormulario.Controls.Add(BtnModificar);
        pnlFormulario.Controls.Add(ChkMostrarCancelados);
        pnlFormulario.Controls.Add(LblMensaje);
        pnlFormulario.Controls.Add(BtnCancelar);
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
        pnlFormulario.Name = "pnlFormulario";
        pnlFormulario.Size = new Size(1100, 190);
        pnlFormulario.TabIndex = 1;
        //
        // ChkTodosMedicos
        //
        ChkTodosMedicos.AutoSize = true;
        ChkTodosMedicos.Font = new Font("Segoe UI", 9F);
        ChkTodosMedicos.Location = new Point(610, 40);
        ChkTodosMedicos.Name = "ChkTodosMedicos";
        ChkTodosMedicos.Size = new Size(170, 29);
        ChkTodosMedicos.TabIndex = 9;
        ChkTodosMedicos.Text = "Ver todos los medicos";
        ChkTodosMedicos.UseVisualStyleBackColor = true;
        //
        // ChkFiltrarFecha
        //
        ChkFiltrarFecha.AutoSize = true;
        ChkFiltrarFecha.Font = new Font("Segoe UI", 9F);
        ChkFiltrarFecha.Location = new Point(220, 155);
        ChkFiltrarFecha.Name = "ChkFiltrarFecha";
        ChkFiltrarFecha.Size = new Size(160, 29);
        ChkFiltrarFecha.TabIndex = 10;
        ChkFiltrarFecha.Text = "Filtrar por fecha";
        ChkFiltrarFecha.UseVisualStyleBackColor = true;
        //
        // BtnNuevoTurno
        //
        BtnNuevoTurno.BackColor = Color.FromArgb(120, 130, 145);
        BtnNuevoTurno.FlatAppearance.BorderSize = 0;
        BtnNuevoTurno.FlatStyle = FlatStyle.Flat;
        BtnNuevoTurno.Font = new Font("Segoe UI", 9.5F);
        BtnNuevoTurno.ForeColor = Color.White;
        BtnNuevoTurno.Location = new Point(430, 113);
        BtnNuevoTurno.Name = "BtnNuevoTurno";
        BtnNuevoTurno.Size = new Size(130, 34);
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
        BtnModificar.Location = new Point(710, 113);
        BtnModificar.Name = "BtnModificar";
        BtnModificar.Size = new Size(130, 34);
        BtnModificar.TabIndex = 7;
        BtnModificar.Text = "Modificar";
        BtnModificar.UseVisualStyleBackColor = false;
        //
        // ChkMostrarCancelados
        //
        ChkMostrarCancelados.AutoSize = true;
        ChkMostrarCancelados.Font = new Font("Segoe UI", 9F);
        ChkMostrarCancelados.Location = new Point(20, 155);
        ChkMostrarCancelados.Name = "ChkMostrarCancelados";
        ChkMostrarCancelados.Size = new Size(160, 29);
        ChkMostrarCancelados.TabIndex = 6;
        ChkMostrarCancelados.Text = "Mostrar cancelados";
        ChkMostrarCancelados.UseVisualStyleBackColor = true;
        //
        // LblMensaje
        //
        LblMensaje.AutoSize = true;
        LblMensaje.Font = new Font("Segoe UI", 9F);
        LblMensaje.Location = new Point(430, 155);
        LblMensaje.MaximumSize = new Size(500, 0);
        LblMensaje.Name = "LblMensaje";
        LblMensaje.Size = new Size(0, 25);
        LblMensaje.TabIndex = 0;
        // 
        // BtnCancelar
        // 
        BtnCancelar.BackColor = Color.FromArgb(200, 60, 60);
        BtnCancelar.FlatAppearance.BorderSize = 0;
        BtnCancelar.FlatStyle = FlatStyle.Flat;
        BtnCancelar.Font = new Font("Segoe UI", 9.5F);
        BtnCancelar.ForeColor = Color.White;
        BtnCancelar.Location = new Point(850, 113);
        BtnCancelar.Name = "BtnCancelar";
        BtnCancelar.Size = new Size(150, 34);
        BtnCancelar.TabIndex = 5;
        BtnCancelar.Text = "Cancelar turno sel.";
        BtnCancelar.UseVisualStyleBackColor = false;
        BtnCancelar.Click += BtnCancelar_Click;
        // 
        // BtnAsignar
        // 
        BtnAsignar.BackColor = Color.FromArgb(46, 134, 222);
        BtnAsignar.FlatAppearance.BorderSize = 0;
        BtnAsignar.FlatStyle = FlatStyle.Flat;
        BtnAsignar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        BtnAsignar.ForeColor = Color.White;
        BtnAsignar.Location = new Point(570, 113);
        BtnAsignar.Name = "BtnAsignar";
        BtnAsignar.Size = new Size(130, 34);
        BtnAsignar.TabIndex = 4;
        BtnAsignar.Text = "Asignar turno";
        BtnAsignar.UseVisualStyleBackColor = false;
        BtnAsignar.Click += BtnAsignar_Click;
        // 
        // CboHorario
        // 
        CboHorario.DropDownStyle = ComboBoxStyle.DropDownList;
        CboHorario.Font = new Font("Segoe UI", 10F);
        CboHorario.Location = new Point(220, 113);
        CboHorario.Name = "CboHorario";
        CboHorario.Size = new Size(180, 36);
        CboHorario.TabIndex = 3;
        // 
        // lblHorario
        // 
        lblHorario.AutoSize = true;
        lblHorario.Font = new Font("Segoe UI", 9F);
        lblHorario.Location = new Point(220, 90);
        lblHorario.Name = "lblHorario";
        lblHorario.Size = new Size(72, 25);
        lblHorario.TabIndex = 6;
        lblHorario.Text = "Horario";
        // 
        // DtpFecha
        // 
        DtpFecha.Font = new Font("Segoe UI", 10F);
        DtpFecha.Format = DateTimePickerFormat.Short;
        DtpFecha.Location = new Point(20, 113);
        DtpFecha.Name = "DtpFecha";
        DtpFecha.Size = new Size(180, 34);
        DtpFecha.TabIndex = 2;
        // 
        // lblFecha
        // 
        lblFecha.AutoSize = true;
        lblFecha.Font = new Font("Segoe UI", 9F);
        lblFecha.Location = new Point(20, 90);
        lblFecha.Name = "lblFecha";
        lblFecha.Size = new Size(57, 25);
        lblFecha.TabIndex = 7;
        lblFecha.Text = "Fecha";
        // 
        // CboMedico
        // 
        CboMedico.DropDownStyle = ComboBoxStyle.DropDownList;
        CboMedico.Font = new Font("Segoe UI", 10F);
        CboMedico.Location = new Point(300, 38);
        CboMedico.Name = "CboMedico";
        CboMedico.Size = new Size(300, 36);
        CboMedico.TabIndex = 1;
        // 
        // lblMedico
        // 
        lblMedico.AutoSize = true;
        lblMedico.Font = new Font("Segoe UI", 9F);
        lblMedico.Location = new Point(300, 15);
        lblMedico.Name = "lblMedico";
        lblMedico.Size = new Size(71, 25);
        lblMedico.TabIndex = 8;
        lblMedico.Text = "Medico";
        // 
        // CboPaciente
        // 
        CboPaciente.DropDownStyle = ComboBoxStyle.DropDownList;
        CboPaciente.Font = new Font("Segoe UI", 10F);
        CboPaciente.Location = new Point(20, 38);
        CboPaciente.Name = "CboPaciente";
        CboPaciente.Size = new Size(260, 36);
        CboPaciente.TabIndex = 0;
        // 
        // lblPaciente
        // 
        lblPaciente.AutoSize = true;
        lblPaciente.Font = new Font("Segoe UI", 9F);
        lblPaciente.Location = new Point(20, 15);
        lblPaciente.Name = "lblPaciente";
        lblPaciente.Size = new Size(76, 25);
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
        DgvTurnos.Location = new Point(0, 190);
        DgvTurnos.MultiSelect = false;
        DgvTurnos.Name = "DgvTurnos";
        DgvTurnos.ReadOnly = true;
        DgvTurnos.RowHeadersWidth = 62;
        DgvTurnos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvTurnos.Size = new Size(1100, 460);
        DgvTurnos.TabIndex = 0;
        //
        // pnlAgenda
        //
        pnlAgenda.BackColor = Color.FromArgb(245, 246, 250);
        pnlAgenda.Controls.Add(DgvAgenda);
        pnlAgenda.Controls.Add(lblAgendaTitulo);
        pnlAgenda.Dock = DockStyle.Right;
        pnlAgenda.Location = new Point(800, 190);
        pnlAgenda.Name = "pnlAgenda";
        pnlAgenda.Padding = new Padding(10);
        pnlAgenda.Size = new Size(300, 460);
        pnlAgenda.TabIndex = 2;
        //
        // lblAgendaTitulo
        //
        lblAgendaTitulo.AutoSize = true;
        lblAgendaTitulo.Dock = DockStyle.Top;
        lblAgendaTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblAgendaTitulo.ForeColor = Color.FromArgb(27, 42, 74);
        lblAgendaTitulo.Location = new Point(10, 10);
        lblAgendaTitulo.Name = "lblAgendaTitulo";
        lblAgendaTitulo.Padding = new Padding(0, 0, 0, 8);
        lblAgendaTitulo.Size = new Size(148, 41);
        lblAgendaTitulo.TabIndex = 0;
        lblAgendaTitulo.Text = "Disponibilidad";
        //
        // DgvAgenda
        //
        DgvAgenda.AllowUserToAddRows = false;
        DgvAgenda.BackgroundColor = Color.White;
        DgvAgenda.ColumnHeadersHeight = 34;
        DgvAgenda.Dock = DockStyle.Fill;
        DgvAgenda.Font = new Font("Segoe UI", 9.5F);
        DgvAgenda.Location = new Point(10, 51);
        DgvAgenda.Name = "DgvAgenda";
        DgvAgenda.ReadOnly = true;
        DgvAgenda.RowHeadersWidth = 20;
        DgvAgenda.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DgvAgenda.Size = new Size(280, 399);
        DgvAgenda.TabIndex = 1;
        //
        // FormTurnos
        //
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 650);
        Controls.Add(DgvTurnos);
        Controls.Add(pnlAgenda);
        Controls.Add(pnlFormulario);
        Name = "FormTurnos";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Gestion de Turnos";
        pnlFormulario.ResumeLayout(false);
        pnlFormulario.PerformLayout();
        pnlAgenda.ResumeLayout(false);
        pnlAgenda.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)DgvTurnos).EndInit();
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
    private Label LblMensaje;
    private DataGridView DgvTurnos;
}
