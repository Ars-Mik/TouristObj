using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace TouristObj
{
    public partial class Authorization : Form
    {
        private const string adminLogin = "admin";
        private const string adminPassword = "123";
        public string id;

        //string sql = @"Data Source=.\\SQLEXPRESS; " + "Initial Catalog=Cinema;Integrated Security=True;Pooling=False";
        //string sql = @"Data Source=DESKTOP-2FTJPGV\SQLEXPRESS;AttachDbFilename=C:\Games\Cinema\Cinema.mdf;Integrated Security=True";

        public Authorization()
        {
            InitializeComponent();
            checkBox1.Checked = true;
        }
        NpgsqlConnection connection;

        

        private void button1_Click(object sender, EventArgs e)
        {
            connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
            string checkRow = "";

            try
            {
                connection.Open();

                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("Не все поля заполнены!", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (textBox1.Text == adminLogin && textBox2.Text == adminPassword)
                {
                    MessageBox.Show("Успешная авторизация!", "Аккаунт администратора", MessageBoxButtons.OK, MessageBoxIcon.Information);
					Form adminMenu = new AdminMenu();
					Hide();
					adminMenu.ShowDialog();
					

				}
            }
            catch
            {
                MessageBox.Show("Ошибка авторизации!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = true;
                checkBox1.Text = "Скрыть пароль";
            }
            else
            {
                textBox2.UseSystemPasswordChar = false;
                checkBox1.Text = "Скрыть пароль";
            }
        }

        private void Authorization_FormClosing(object sender, FormClosingEventArgs e)
        {
			Form auth = new Authorization();
			auth.Close();
		}

    }
}
