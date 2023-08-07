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
    public partial class InformationAbout : Form
    {
        string name, addres, prin, dostup, vmest, description;
		string vid = "";
		DataTable table1;


		int number;

        public InformationAbout(int number)
        {
            InitializeComponent();
            this.number = number;

        }
        NpgsqlConnection connection;
        private void InformationAboutTheFilm_Load(object sender, EventArgs e)
        {
            try
            {
                connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
                connection.Open();
				


				using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT Название FROM selTurNumber(@ID_ТурОбъект);", connection))
				{
					cmd.Parameters.AddWithValue("@ID_ТурОбъект", Convert.ToInt32(number));
					name = cmd.ExecuteScalar().ToString();
				}
				using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT Адрес FROM selTurNumber(@ID_ТурОбъект);", connection))
				{
					cmd.Parameters.AddWithValue("@ID_ТурОбъект", Convert.ToInt32(number));
					addres = cmd.ExecuteScalar().ToString();
				}
				using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT Наименовани FROM selTurNumber(@ID_ТурОбъект);", connection))
				{
					cmd.Parameters.AddWithValue("@ID_ТурОбъект", Convert.ToInt32(number));
					NpgsqlDataReader reader = cmd.ExecuteReader();
					table1 = new DataTable();
					table1.Columns.Add("Наименовани");
					while (reader.Read())
					{
						if (reader.GetValue(0) == DBNull.Value)
						{
							table1.Rows.Add( "-");
						}
						else
						{
							string[] str;
							str = (string[])reader.GetValue(0);
							for (int i = 0; i < str.Length; i++)
								vid += str[i] + ",";
							table1.Rows.Add(vid);
						}
					}
					reader.Close();
					dataGridView2.DataSource = table1;
					//vid = cmd.ExecuteScalar().ToString();
				}
				string result = string.Join(", ", vid);
				using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT Наименование FROM selTurNumber(@ID_ТурОбъект);", connection))
				{
					cmd.Parameters.AddWithValue("@ID_ТурОбъект", Convert.ToInt32(number));
					prin = cmd.ExecuteScalar().ToString();
				}
				using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT Особенности_доступа FROM selTurNumber(@ID_ТурОбъект);", connection))
				{
					cmd.Parameters.AddWithValue("@ID_ТурОбъект", Convert.ToInt32(number));
					dostup = cmd.ExecuteScalar().ToString();
				}
				using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT Вместимость FROM selTurNumber(@ID_ТурОбъект);", connection))
				{
					cmd.Parameters.AddWithValue("@ID_ТурОбъект", Convert.ToInt32(number));
					vmest = cmd.ExecuteScalar().ToString();
				}
				using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT Примечание FROM selTurNumber(@ID_ТурОбъект);", connection))
				{
					cmd.Parameters.AddWithValue("@ID_ТурОбъект", Convert.ToInt32(number));
					description = cmd.ExecuteScalar().ToString();
				}
				
                connection.Close();

				textBox1.Text = name;
                textBox2.Text = addres;
				textBox3.Text = vid;
				textBox4.Text = prin;
				textBox5.Text = dostup;
                textBox6.Text = vmest;
                richTextBox1.Text = description;
            }
            catch
            {
                MessageBox.Show("Ошибка просмотра информации!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
