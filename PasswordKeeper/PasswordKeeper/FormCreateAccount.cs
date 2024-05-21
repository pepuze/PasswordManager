using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordKeeper
{
    public partial class FormCreateAccount : Form
    {
        public UserData user;
        Dictionary<string, UserData> _usersExisting;

        public FormCreateAccount(in Dictionary<string, UserData> usersExisting)
        {
            _usersExisting = usersExisting;
            InitializeComponent();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {

        }

        private void bCreate_Click(object sender, EventArgs e)
        {
            string login = tb_Login.Text,
                   password = tb_Password.Text,
                   passwordRepeat = tb_PasswordRepeat.Text;

            if (password != passwordRepeat)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_usersExisting.ContainsKey(login))
            {
                MessageBox.Show("Учетная запись с таким логином уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!checkIfValid(password))
            {
                MessageBox.Show("Введенный пароль не соответствует требованиям.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            byte[] hash = FormSignIn.getStringHash(password);
            user = new UserData(login, hash, "placeholder_id");

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool checkIfValid(string password)
        {
            if (password.Length < 8) return false;
            bool lowerRegister = false;
            bool upperRegister = false;
            bool specialSymbols = false;
            bool hasDigits = false;

            foreach (char c in password)
            {
                if (!Char.IsLetterOrDigit(c)) specialSymbols = true;
                else if (Char.IsLetter(c))
                {
                    if (!Char.IsUpper(c)) lowerRegister = true;
                    else upperRegister = true;
                }
                else if (Char.IsDigit(c)) hasDigits = true;
            }

            return lowerRegister && upperRegister && specialSymbols && hasDigits;
        }

        private void tb_PasswordRepeat_Enter(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int visibleTime = 5000;
            ToolTip tt = new ToolTip();
            tt.Show("Пароль должен содержать цифры,\r\n буквы разных регистров,\r\n специальные символы", tb, 120, 20, visibleTime);
        }
    }
}
