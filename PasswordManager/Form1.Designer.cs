namespace PasswordManager
{
    partial class Form1
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
            tbFilePath = new TextBox();
            bBrowse = new Button();
            bEcnrypt = new Button();
            bDecrypt = new Button();
            SuspendLayout();
            // 
            // tbFilePath
            // 
            tbFilePath.Location = new Point(12, 12);
            tbFilePath.Name = "tbFilePath";
            tbFilePath.ReadOnly = true;
            tbFilePath.Size = new Size(366, 26);
            tbFilePath.TabIndex = 0;
            // 
            // bBrowse
            // 
            bBrowse.Location = new Point(384, 12);
            bBrowse.Name = "bBrowse";
            bBrowse.Size = new Size(90, 28);
            bBrowse.TabIndex = 1;
            bBrowse.Text = "Обзор";
            bBrowse.UseVisualStyleBackColor = true;
            bBrowse.Click += bBrowse_Click;
            // 
            // bEcnrypt
            // 
            bEcnrypt.Location = new Point(21, 60);
            bEcnrypt.Name = "bEcnrypt";
            bEcnrypt.Size = new Size(118, 28);
            bEcnrypt.TabIndex = 2;
            bEcnrypt.Text = "Зашифровать";
            bEcnrypt.UseVisualStyleBackColor = true;
            bEcnrypt.Click += bEcnrypt_Click;
            // 
            // bDecrypt
            // 
            bDecrypt.Location = new Point(356, 60);
            bDecrypt.Name = "bDecrypt";
            bDecrypt.Size = new Size(118, 28);
            bDecrypt.TabIndex = 3;
            bDecrypt.Text = "Дешифровать";
            bDecrypt.UseVisualStyleBackColor = true;
            bDecrypt.Click += bDecrypt_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(486, 100);
            Controls.Add(bDecrypt);
            Controls.Add(bEcnrypt);
            Controls.Add(bBrowse);
            Controls.Add(tbFilePath);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbFilePath;
        private Button bBrowse;
        private Button bEcnrypt;
        private Button bDecrypt;
    }
}
