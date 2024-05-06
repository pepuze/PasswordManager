using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PasswordKeeper
{
    public partial class Form2 : Form
    {
        public string f2_site, f2_userLogin, f2_userPass, f2_userName, f2_userPhone;

        public Form2()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        public void SetTextBox()
        {
            textBox1.Text = f2_site;
            textBox2.Text = f2_userLogin;
            textBox3.Text = f2_userPass;
            textBox4.Text = f2_userName;
            textBox5.Text = f2_userPhone;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f2_site = textBox1.Text;
            f2_userLogin = textBox2.Text;
            f2_userPass = textBox3.Text;
            f2_userName = textBox4.Text;
            f2_userPhone = textBox5.Text;
            if (f2_site == "" || f2_userLogin == "" || f2_userPass == "")
            {
                MessageBox.Show("Вы не ввели сайт, логин и пароль для добавления пароля!", "Внимание");
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
