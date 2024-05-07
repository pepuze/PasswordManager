namespace PasswordKeeper
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            checkBox1 = new CheckBox();
            label4 = new Label();
            textBox4 = new TextBox();
            label5 = new Label();
            label6 = new Label();
            button2 = new Button();
            button3 = new Button();
            maskedTextBox1 = new MaskedTextBox();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(128, 34);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(236, 23);
            textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(128, 83);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(236, 23);
            textBox2.TabIndex = 1;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(128, 135);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(236, 23);
            textBox3.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(86, 37);
            label1.Name = "label1";
            label1.Size = new Size(36, 15);
            label1.TabIndex = 3;
            label1.Text = "Сайт:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(78, 86);
            label2.Name = "label2";
            label2.Size = new Size(44, 15);
            label2.TabIndex = 4;
            label2.Text = "Логин:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(70, 138);
            label3.Name = "label3";
            label3.Size = new Size(52, 15);
            label3.TabIndex = 5;
            label3.Text = "Пароль:";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(70, 174);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(119, 19);
            checkBox1.TabIndex = 6;
            checkBox1.Text = "Показать пароль";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 218);
            label4.Name = "label4";
            label4.Size = new Size(200, 15);
            label4.TabIndex = 7;
            label4.Text = "Доп. информация (необязательно)";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(128, 258);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(236, 23);
            textBox4.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(85, 261);
            label5.Name = "label5";
            label5.Size = new Size(37, 15);
            label5.TabIndex = 10;
            label5.Text = "ФИО:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(64, 312);
            label6.Name = "label6";
            label6.Size = new Size(58, 15);
            label6.TabIndex = 11;
            label6.Text = "Телефон:";
            // 
            // button2
            // 
            button2.Location = new Point(35, 359);
            button2.Name = "button2";
            button2.Size = new Size(153, 40);
            button2.TabIndex = 13;
            button2.Text = "Сохранить изменения";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(313, 359);
            button3.Name = "button3";
            button3.Size = new Size(110, 40);
            button3.TabIndex = 14;
            button3.Text = "Отмена";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // maskedTextBox1
            // 
            maskedTextBox1.Location = new Point(128, 309);
            maskedTextBox1.Mask = "+7 (000) 000-0000";
            maskedTextBox1.Name = "maskedTextBox1";
            maskedTextBox1.Size = new Size(236, 23);
            maskedTextBox1.TabIndex = 15;
            maskedTextBox1.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(461, 421);
            Controls.Add(maskedTextBox1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(textBox4);
            Controls.Add(label4);
            Controls.Add(checkBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form2";
            Text = "Ввод/изменение данных";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private Label label1;
        private Label label2;
        private Label label3;
        private CheckBox checkBox1;
        private Label label4;
        private TextBox textBox4;
        private Label label5;
        private Label label6;
        private Button button2;
        private Button button3;
        private MaskedTextBox maskedTextBox1;
    }
}