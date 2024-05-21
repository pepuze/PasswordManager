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
            if (login.Trim() == "" || password.Trim() == "")
            {
                MessageBox.Show("Пустое поле логина или пароля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(password != passwordRepeat)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_usersExisting.ContainsKey(login))
            {
                MessageBox.Show("Учетная запись с таким логином уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            byte[] hash = FormSignIn.getStringHash(password);
            user = new UserData(login, hash, "placeholder_id");
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
