namespace PasswordKeeper
{
    partial class FormCreateAccount
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
            tb_PasswordRepeat = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            bCreate = new Button();
            bCancel = new Button();
            SuspendLayout();
            // 
            // tb_Login
            // 
            tb_Login.Location = new Point(175, 23);
            tb_Login.Margin = new Padding(3, 4, 3, 4);
            tb_Login.MaxLength = 64;
            tb_Login.Name = "tb_Login";
            tb_Login.Size = new Size(172, 26);
            tb_Login.TabIndex = 0;
            // 
            // tb_Password
            // 
            tb_Password.Location = new Point(175, 60);
            tb_Password.Margin = new Padding(3, 4, 3, 4);
            tb_Password.MaxLength = 64;
            tb_Password.Name = "tb_Password";
            tb_Password.PasswordChar = '*';
            tb_Password.Size = new Size(172, 26);
            tb_Password.TabIndex = 1;
            tb_Password.Enter += tb_PasswordRepeat_Enter;
            // 
            // tb_PasswordRepeat
            // 
            tb_PasswordRepeat.Location = new Point(175, 97);
            tb_PasswordRepeat.Margin = new Padding(3, 4, 3, 4);
            tb_PasswordRepeat.MaxLength = 64;
            tb_PasswordRepeat.Name = "tb_PasswordRepeat";
            tb_PasswordRepeat.PasswordChar = '*';
            tb_PasswordRepeat.Size = new Size(172, 26);
            tb_PasswordRepeat.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 26);
            label1.Name = "label1";
            label1.Size = new Size(143, 20);
            label1.TabIndex = 3;
            label1.Text = "Придумайте логин:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 97);
            label2.Name = "label2";
            label2.Size = new Size(142, 20);
            label2.TabIndex = 4;
            label2.Text = "Повторите пароль:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 63);
            label3.Name = "label3";
            label3.Size = new Size(153, 20);
            label3.TabIndex = 5;
            label3.Text = "Придумайте пароль:";
            // 
            // bCreate
            // 
            bCreate.Location = new Point(12, 134);
            bCreate.Name = "bCreate";
            bCreate.Size = new Size(122, 28);
            bCreate.TabIndex = 6;
            bCreate.Text = "Создать";
            bCreate.UseVisualStyleBackColor = true;
            bCreate.Click += bCreate_Click;
            // 
            // bCancel
            // 
            bCancel.DialogResult = DialogResult.Cancel;
            bCancel.Location = new Point(231, 134);
            bCancel.Name = "bCancel";
            bCancel.Size = new Size(116, 28);
            bCancel.TabIndex = 7;
            bCancel.Text = "Отменить";
            bCancel.UseVisualStyleBackColor = true;
            bCancel.Click += bCancel_Click;
            // 
            // FormCreateAccount
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(373, 173);
            Controls.Add(bCancel);
            Controls.Add(bCreate);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(tb_PasswordRepeat);
            Controls.Add(tb_Password);
            Controls.Add(tb_Login);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormCreateAccount";
            Text = "Создание учетной записи";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tb_Login;
        private TextBox tb_Password;
        private TextBox tb_PasswordRepeat;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button bCreate;
        private Button bCancel;
    }
}