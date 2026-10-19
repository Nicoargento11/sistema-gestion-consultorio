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

    // Estructura estatica del login: una tarjeta blanca centrada sobre el fondo de
    // la app. Los tamanos de la ventana se calculan desde el contenido en
    // FormLogin.cs (ver AjustarVentana).
    private void InitializeComponent()
    {
        tlpRaiz = new TableLayoutPanel();
        pnlMarco = new Panel();
        pnlTarjeta = new Panel();
        tlpTarjeta = new TableLayoutPanel();
        lblIconoMarca = new Label();
        lblMarca = new Label();
        lblSubtitulo = new Label();
        label1 = new Label();
        TxtUsuario = new TextBox();
        label2 = new Label();
        TxtContrasenia = new TextBox();
        BtnIngresar = new Button();
        pnlEstado = new Panel();
        LblError = new Label();
        tlpRaiz.SuspendLayout();
        pnlMarco.SuspendLayout();
        pnlTarjeta.SuspendLayout();
        tlpTarjeta.SuspendLayout();
        pnlEstado.SuspendLayout();
        SuspendLayout();
        //
        // tlpRaiz
        //
        tlpRaiz.ColumnCount = 3;
        tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpRaiz.ColumnStyles.Add(new ColumnStyle());
        tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpRaiz.Controls.Add(pnlMarco, 1, 1);
        tlpRaiz.Dock = DockStyle.Fill;
        tlpRaiz.Name = "tlpRaiz";
        tlpRaiz.RowCount = 3;
        tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tlpRaiz.RowStyles.Add(new RowStyle());
        tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tlpRaiz.TabIndex = 0;
        //
        // pnlMarco
        //
        pnlMarco.Anchor = AnchorStyles.None;
        pnlMarco.AutoSize = true;
        pnlMarco.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlMarco.BackColor = Color.FromArgb(214, 220, 230);
        pnlMarco.Controls.Add(pnlTarjeta);
        pnlMarco.Margin = new Padding(0);
        pnlMarco.Name = "pnlMarco";
        pnlMarco.Padding = new Padding(1);
        pnlMarco.TabIndex = 0;
        //
        // pnlTarjeta
        //
        pnlTarjeta.AutoSize = true;
        pnlTarjeta.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlTarjeta.BackColor = Color.White;
        pnlTarjeta.Controls.Add(tlpTarjeta);
        pnlTarjeta.Dock = DockStyle.Fill;
        pnlTarjeta.Name = "pnlTarjeta";
        pnlTarjeta.Padding = new Padding(28, 16, 28, 18);
        pnlTarjeta.TabIndex = 0;
        //
        // tlpTarjeta
        //
        tlpTarjeta.AutoSize = true;
        tlpTarjeta.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpTarjeta.ColumnCount = 1;
        tlpTarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpTarjeta.Controls.Add(lblIconoMarca, 0, 0);
        tlpTarjeta.Controls.Add(lblMarca, 0, 1);
        tlpTarjeta.Controls.Add(lblSubtitulo, 0, 2);
        tlpTarjeta.Controls.Add(label1, 0, 3);
        tlpTarjeta.Controls.Add(TxtUsuario, 0, 4);
        tlpTarjeta.Controls.Add(label2, 0, 5);
        tlpTarjeta.Controls.Add(TxtContrasenia, 0, 6);
        tlpTarjeta.Controls.Add(BtnIngresar, 0, 7);
        tlpTarjeta.Controls.Add(pnlEstado, 0, 8);
        tlpTarjeta.Dock = DockStyle.Fill;
        tlpTarjeta.MinimumSize = new Size(300, 0);
        tlpTarjeta.Name = "tlpTarjeta";
        tlpTarjeta.RowCount = 9;
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.RowStyles.Add(new RowStyle());
        tlpTarjeta.TabIndex = 0;
        //
        // lblIconoMarca
        //
        lblIconoMarca.Anchor = AnchorStyles.None;
        lblIconoMarca.AutoSize = true;
        lblIconoMarca.Font = new Font("Segoe MDL2 Assets", 22F);
        lblIconoMarca.ForeColor = Color.FromArgb(46, 134, 222);
        lblIconoMarca.Margin = new Padding(0, 0, 0, 4);
        lblIconoMarca.Name = "lblIconoMarca";
        lblIconoMarca.TabIndex = 9;
        //
        // lblMarca
        //
        lblMarca.Anchor = AnchorStyles.None;
        lblMarca.AutoSize = true;
        lblMarca.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblMarca.ForeColor = Color.FromArgb(27, 42, 74);
        lblMarca.Margin = new Padding(0);
        lblMarca.Name = "lblMarca";
        lblMarca.TabIndex = 8;
        lblMarca.Text = "Gestión de Consultorio";
        //
        // lblSubtitulo
        //
        lblSubtitulo.Anchor = AnchorStyles.None;
        lblSubtitulo.AutoSize = true;
        lblSubtitulo.Font = new Font("Segoe UI", 10F);
        lblSubtitulo.ForeColor = Color.FromArgb(110, 120, 135);
        lblSubtitulo.Margin = new Padding(0, 0, 0, 4);
        lblSubtitulo.Name = "lblSubtitulo";
        lblSubtitulo.TabIndex = 7;
        lblSubtitulo.Text = "Iniciar sesión";
        //
        // label1
        //
        label1.AutoSize = true;
        label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        label1.ForeColor = Color.FromArgb(27, 42, 74);
        label1.Margin = new Padding(0, 10, 0, 3);
        label1.Name = "label1";
        label1.TabIndex = 5;
        label1.Text = "1. Usuario";
        //
        // TxtUsuario
        //
        TxtUsuario.Dock = DockStyle.Fill;
        TxtUsuario.Font = new Font("Segoe UI", 11F);
        TxtUsuario.Margin = new Padding(0, 0, 0, 4);
        TxtUsuario.Name = "TxtUsuario";
        TxtUsuario.TabIndex = 0;
        //
        // label2
        //
        label2.AutoSize = true;
        label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        label2.ForeColor = Color.FromArgb(27, 42, 74);
        label2.Margin = new Padding(0, 10, 0, 3);
        label2.Name = "label2";
        label2.TabIndex = 6;
        label2.Text = "2. Contraseña";
        //
        // TxtContrasenia
        //
        TxtContrasenia.Dock = DockStyle.Fill;
        TxtContrasenia.Font = new Font("Segoe UI", 11F);
        TxtContrasenia.Margin = new Padding(0, 0, 0, 4);
        TxtContrasenia.Name = "TxtContrasenia";
        TxtContrasenia.PasswordChar = '*';
        TxtContrasenia.TabIndex = 1;
        //
        // BtnIngresar
        //
        BtnIngresar.BackColor = Color.FromArgb(46, 134, 222);
        BtnIngresar.Dock = DockStyle.Fill;
        BtnIngresar.FlatAppearance.BorderSize = 0;
        BtnIngresar.FlatStyle = FlatStyle.Flat;
        BtnIngresar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        BtnIngresar.ForeColor = Color.White;
        BtnIngresar.Margin = new Padding(0, 12, 0, 0);
        BtnIngresar.Name = "BtnIngresar";
        BtnIngresar.Size = new Size(0, 40);
        BtnIngresar.TabIndex = 2;
        BtnIngresar.Text = "Ingresar";
        BtnIngresar.UseVisualStyleBackColor = false;
        BtnIngresar.Click += BtnIngresar_Click;
        //
        // pnlEstado
        //
        pnlEstado.Controls.Add(LblError);
        pnlEstado.Dock = DockStyle.Fill;
        pnlEstado.Margin = new Padding(0);
        pnlEstado.Name = "pnlEstado";
        pnlEstado.Padding = new Padding(2, 8, 2, 0);
        pnlEstado.Size = new Size(0, 36);
        pnlEstado.TabIndex = 3;
        pnlEstado.Visible = false;
        //
        // LblError
        //
        LblError.Dock = DockStyle.Fill;
        LblError.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        LblError.Name = "LblError";
        LblError.TabIndex = 0;
        //
        // FormLogin
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(560, 520);
        Controls.Add(tlpRaiz);
        Name = "FormLogin";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Sistema de Gestión de Consultorio - Login";
        tlpRaiz.ResumeLayout(false);
        tlpRaiz.PerformLayout();
        pnlMarco.ResumeLayout(false);
        pnlMarco.PerformLayout();
        pnlTarjeta.ResumeLayout(false);
        pnlTarjeta.PerformLayout();
        tlpTarjeta.ResumeLayout(false);
        tlpTarjeta.PerformLayout();
        pnlEstado.ResumeLayout(false);
        ResumeLayout(false);
    }

    private TableLayoutPanel tlpRaiz;
    private Panel pnlMarco;
    private Panel pnlTarjeta;
    private TableLayoutPanel tlpTarjeta;
    private Label lblIconoMarca;
    private Label lblMarca;
    private Label lblSubtitulo;
    private Label label1;
    private TextBox TxtUsuario;
    private Label label2;
    private TextBox TxtContrasenia;
    private Button BtnIngresar;
    private Panel pnlEstado;
    private Label LblError;
}
