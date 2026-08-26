namespace SGC.UI;

partial class FormLogin
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
        pnlMarca = new Panel();
        lblSubtitulo = new Label();
        lblMarca = new Label();
        pnlFormulario = new Panel();
        LblError = new Label();
        BtnIngresar = new Button();
        TxtContrasenia = new TextBox();
        label2 = new Label();
        TxtUsuario = new TextBox();
        label1 = new Label();
        pnlMarca.SuspendLayout();
        pnlFormulario.SuspendLayout();
        SuspendLayout();
        // 
        // pnlMarca
        // 
        pnlMarca.BackColor = Color.FromArgb(27, 42, 74);
        pnlMarca.Controls.Add(lblSubtitulo);
        pnlMarca.Controls.Add(lblMarca);
        pnlMarca.Dock = DockStyle.Left;
        pnlMarca.Location = new Point(0, 0);
        pnlMarca.Name = "pnlMarca";
        pnlMarca.Size = new Size(320, 500);
        pnlMarca.TabIndex = 1;
        // 
        // lblSubtitulo
        // 
        lblSubtitulo.AutoSize = true;
        lblSubtitulo.Font = new Font("Segoe UI", 11F);
        lblSubtitulo.ForeColor = Color.FromArgb(180, 195, 220);
        lblSubtitulo.Location = new Point(30, 300);
        lblSubtitulo.Name = "lblSubtitulo";
        lblSubtitulo.Size = new Size(136, 30);
        lblSubtitulo.TabIndex = 0;
        lblSubtitulo.Text = "Iniciar sesión";
        // 
        // lblMarca
        // 
        lblMarca.AutoSize = true;
        lblMarca.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblMarca.ForeColor = Color.White;
        lblMarca.Location = new Point(30, 180);
        lblMarca.MaximumSize = new Size(260, 0);
        lblMarca.Name = "lblMarca";
        lblMarca.Size = new Size(240, 108);
        lblMarca.TabIndex = 1;
        lblMarca.Text = "Gestión de Consultorio";
        // 
        // pnlFormulario
        // 
        pnlFormulario.BackColor = Color.FromArgb(245, 246, 250);
        pnlFormulario.Controls.Add(LblError);
        pnlFormulario.Controls.Add(BtnIngresar);
        pnlFormulario.Controls.Add(TxtContrasenia);
        pnlFormulario.Controls.Add(label2);
        pnlFormulario.Controls.Add(TxtUsuario);
        pnlFormulario.Controls.Add(label1);
        pnlFormulario.Dock = DockStyle.Fill;
        pnlFormulario.Location = new Point(320, 0);
        pnlFormulario.Name = "pnlFormulario";
        pnlFormulario.Size = new Size(580, 500);
        pnlFormulario.TabIndex = 0;
        // 
        // LblError
        // 
        LblError.AutoSize = true;
        LblError.Font = new Font("Segoe UI", 9F);
        LblError.ForeColor = Color.FromArgb(200, 40, 40);
        LblError.Location = new Point(80, 360);
        LblError.MaximumSize = new Size(260, 0);
        LblError.Name = "LblError";
        LblError.Size = new Size(0, 25);
        LblError.TabIndex = 0;
        // 
        // BtnIngresar
        // 
        BtnIngresar.BackColor = Color.FromArgb(46, 134, 222);
        BtnIngresar.FlatAppearance.BorderSize = 0;
        BtnIngresar.FlatStyle = FlatStyle.Flat;
        BtnIngresar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        BtnIngresar.ForeColor = Color.White;
        BtnIngresar.Location = new Point(80, 305);
        BtnIngresar.Name = "BtnIngresar";
        BtnIngresar.Size = new Size(260, 42);
        BtnIngresar.TabIndex = 1;
        BtnIngresar.Text = "Ingresar";
        BtnIngresar.UseVisualStyleBackColor = false;
        BtnIngresar.Click += BtnIngresar_Click;
        // 
        // TxtContrasenia
        // 
        TxtContrasenia.Font = new Font("Segoe UI", 11F);
        TxtContrasenia.Location = new Point(80, 253);
        TxtContrasenia.Name = "TxtContrasenia";
        TxtContrasenia.PasswordChar = '*';
        TxtContrasenia.Size = new Size(260, 37);
        TxtContrasenia.TabIndex = 2;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Segoe UI", 10F);
        label2.ForeColor = Color.FromArgb(27, 42, 74);
        label2.Location = new Point(80, 225);
        label2.Name = "label2";
        label2.Size = new Size(110, 28);
        label2.TabIndex = 3;
        label2.Text = "Contraseña";
        // 
        // TxtUsuario
        // 
        TxtUsuario.Font = new Font("Segoe UI", 11F);
        TxtUsuario.Location = new Point(80, 178);
        TxtUsuario.Name = "TxtUsuario";
        TxtUsuario.Size = new Size(260, 37);
        TxtUsuario.TabIndex = 4;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Segoe UI", 10F);
        label1.ForeColor = Color.FromArgb(27, 42, 74);
        label1.Location = new Point(80, 150);
        label1.Name = "label1";
        label1.Size = new Size(79, 28);
        label1.TabIndex = 5;
        label1.Text = "Usuario";
        // 
        // FormLogin
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 500);
        Controls.Add(pnlFormulario);
        Controls.Add(pnlMarca);
        Name = "FormLogin";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Sistema de Gestión de Consultorio - Login";
        Load += FormLogin_Load;
        pnlMarca.ResumeLayout(false);
        pnlMarca.PerformLayout();
        pnlFormulario.ResumeLayout(false);
        pnlFormulario.PerformLayout();
        ResumeLayout(false);
    }

    private Panel pnlMarca;
    private Label lblMarca;
    private Label lblSubtitulo;
    private Panel pnlFormulario;
    private Label label1;
    private TextBox TxtUsuario;
    private Label label2;
    private TextBox TxtContrasenia;
    private Button BtnIngresar;
    private Label LblError;
}
