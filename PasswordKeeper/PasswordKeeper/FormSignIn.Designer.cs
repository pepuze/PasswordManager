namespace PasswordKeeper
{
    partial class FormSignIn
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tb_Login = new TextBox();
            tb_Password = new TextBox();
            label2 = new Label();
            bLogIn = new Button();
            label1 = new Label();
            bCreateAccount = new Button();
            SuspendLayout();
            // 
            // tb_Login
            // 
            tb_Login.Location = new Point(154, 130);
            tb_Login.Margin = new Padding(3, 4, 3, 4);
            tb_Login.MaxLength = 64;
            tb_Login.Name = "tb_Login";
            tb_Login.Size = new Size(178, 26);
            tb_Login.TabIndex = 0;
            // 
            // tb_Password
            // 
            tb_Password.HideSelection = false;
            tb_Password.Location = new Point(154, 167);
            tb_Password.Margin = new Padding(3, 4, 3, 4);
            tb_Password.MaxLength = 64;
            tb_Password.Name = "tb_Password";
            tb_Password.PasswordChar = '*';
            tb_Password.Size = new Size(178, 26);
            tb_Password.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(97, 134);
            label2.Name = "label2";
            label2.Size = new Size(55, 20);
            label2.TabIndex = 3;
            label2.Text = "Логин:";
            // 
            // bLogIn
            // 
            bLogIn.Location = new Point(203, 204);
            bLogIn.Margin = new Padding(3, 4, 3, 4);
            bLogIn.Name = "bLogIn";
            bLogIn.Size = new Size(86, 29);
            bLogIn.TabIndex = 4;
            bLogIn.Text = "Войти";
            bLogIn.UseVisualStyleBackColor = true;
            bLogIn.Click += bLogIn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(88, 171);
            label1.Name = "label1";
            label1.Size = new Size(65, 20);
            label1.TabIndex = 5;
            label1.Text = "Пароль:";
            // 
            // bCreateAccount
            // 
            bCreateAccount.Location = new Point(145, 241);
            bCreateAccount.Margin = new Padding(3, 4, 3, 4);
            bCreateAccount.Name = "bCreateAccount";
            bCreateAccount.Size = new Size(187, 29);
            bCreateAccount.TabIndex = 6;
            bCreateAccount.Text = "Создать учетную запись";
            bCreateAccount.UseVisualStyleBackColor = true;
            bCreateAccount.Click += bCreateAccount_Click;
            // 
            // FormSignIn
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(475, 315);
            Controls.Add(bCreateAccount);
            Controls.Add(label1);
            Controls.Add(bLogIn);
            Controls.Add(label2);
            Controls.Add(tb_Password);
            Controls.Add(tb_Login);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormSignIn";
            Text = "FormSignIn";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tb_Login;
        private TextBox tb_Password;
        private Label label2;
        private Button bLogIn;
        private Label label1;
        private Button bCreateAccount;
    }
}