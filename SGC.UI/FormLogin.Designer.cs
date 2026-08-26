namespace SGC.UI;

partial class FormLogin
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        label1 = new Label();
        TxtUsuario = new TextBox();
        label2 = new Label();
        TxtContrasenia = new TextBox();
        BtnIngresar = new Button();
        LblError = new Label();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(212, 118);
        label1.Name = "label1";
        label1.Size = new Size(72, 25);
        label1.TabIndex = 0;
        label1.Text = "Usuario";
        // 
        // TxtUsuario
        // 
        TxtUsuario.Location = new Point(307, 118);
        TxtUsuario.Name = "TxtUsuario";
        TxtUsuario.Size = new Size(150, 31);
        TxtUsuario.TabIndex = 1;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(199, 173);
        label2.Name = "label2";
        label2.Size = new Size(101, 25);
        label2.TabIndex = 2;
        label2.Text = "Contraseña";
        label2.Click += label2_Click;
        // 
        // TxtContrasenia
        // 
        TxtContrasenia.Location = new Point(307, 173);
        TxtContrasenia.Name = "TxtContrasenia";
        TxtContrasenia.PasswordChar = '*';
        TxtContrasenia.Size = new Size(150, 31);
        TxtContrasenia.TabIndex = 3;

        // 
        // BtnIngresar
        // 
        BtnIngresar.Location = new Point(340, 302);
        BtnIngresar.Name = "BtnIngresar";
        BtnIngresar.RightToLeft = RightToLeft.No;
        BtnIngresar.Size = new Size(112, 34);
        BtnIngresar.TabIndex = 4;
        BtnIngresar.Text = "Ingresar";
        BtnIngresar.UseVisualStyleBackColor = true;
        BtnIngresar.Click += BtnIngresar_Click;
        // 
        // LblError
        // 
        LblError.AutoSize = true;
        LblError.Location = new Point(340, 274);
        LblError.Name = "LblError";
        LblError.Size = new Size(0, 25);
        LblError.TabIndex = 5;
        // 
        // FormLogin
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(LblError);
        Controls.Add(BtnIngresar);
        Controls.Add(TxtContrasenia);
        Controls.Add(label2);
        Controls.Add(TxtUsuario);
        Controls.Add(label1);
        Name = "FormLogin";
        Text = "Sistema de Gestión de Consultorio - Login";
        Load += FormLogin_Load;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label label1;
    private TextBox TxtUsuario;
    private Label label2;
    private TextBox TxtContrasenia;
    private Button BtnIngresar;
    private Label LblError;
}
