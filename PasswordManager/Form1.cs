namespace PasswordManager
{
    public partial class Form1 : Form
    {
        byte[] key;
        byte[] iv;
        public Form1()
        {
            key = AesEncryptor.generateKey();
            iv = AesEncryptor.generateIV();
            InitializeComponent();
        }

        private void bBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "ֻ‏בûו פאיכû (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tbFilePath.Text = ofd.FileName;
            }
        }

        private void bEcnrypt_Click(object sender, EventArgs e)
        {
            AesEncryptor.encryptFile(tbFilePath.Text, key);
        }

        private void bDecrypt_Click(object sender, EventArgs e)
        {
            AesEncryptor.decryptFile(tbFilePath.Text, key);
        }
    }
}
