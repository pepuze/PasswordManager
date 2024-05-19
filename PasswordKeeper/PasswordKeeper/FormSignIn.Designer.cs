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
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label2 = new Label();
            bLogIn = new Button();
            label1 = new Label();
            bCreateAccount = new Button();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(135, 103);
            textBox1.MaxLength = 64;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(156, 23);
            textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.HideSelection = false;
            textBox2.Location = new Point(135, 132);
            textBox2.MaxLength = 64;
            textBox2.Name = "textBox2";
            textBox2.PasswordChar = '*';
            textBox2.Size = new Size(156, 23);
            textBox2.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(85, 106);
            label2.Name = "label2";
            label2.Size = new Size(44, 15);
            label2.TabIndex = 3;
            label2.Text = "Логин:";
            // 
            // bLogIn
            // 
            bLogIn.Location = new Point(178, 161);
            bLogIn.Name = "bLogIn";
            bLogIn.Size = new Size(75, 23);
            bLogIn.TabIndex = 4;
            bLogIn.Text = "Войти";
            bLogIn.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(77, 135);
            label1.Name = "label1";
            label1.Size = new Size(52, 15);
            label1.TabIndex = 5;
            label1.Text = "Пароль:";
            // 
            // bCreateAccount
            // 
            bCreateAccount.Location = new Point(135, 190);
            bCreateAccount.Name = "bCreateAccount";
            bCreateAccount.Size = new Size(156, 23);
            bCreateAccount.TabIndex = 6;
            bCreateAccount.Text = "Создать учетную запись";
            bCreateAccount.UseVisualStyleBackColor = true;
            bCreateAccount.Click += bCreateAccount_Click;
            // 
            // FormSignIn
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(416, 249);
            Controls.Add(bCreateAccount);
            Controls.Add(label1);
            Controls.Add(bLogIn);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Name = "FormSignIn";
            Text = "FormSignIn";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private Label label2;
        private Button bLogIn;
        private Label label1;
        private Button bCreateAccount;
    }
}