using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TouristObj
{
    public partial class UserMenu : Form
    {
        //string sql = @"Data Source=.\\SQLEXPRESS; " + "Initial Catalog=Cinema;Integrated Security=True;Pooling=False";
        //string sql = @"Data Source=DESKTOP-2FTJPGV\SQLEXPRESS;AttachDbFilename=C:\Games\Cinema\Cinema.mdf;Integrated Security=True";

        string id;
        NpgsqlConnection connection;
        NpgsqlDataAdapter adapter1, adapter2, adapter3, adapter4 = null;
        DataTable table1, table2, table3, table4 = null;

        public UserMenu()
        {
            InitializeComponent();
			this.id = id;
        }

		private void showCB()
		{

            NpgsqlCommand comCB;
            comCB = new NpgsqlCommand();
            comCB.CommandText = "SELECT * FROM Вид_Объекта";
            comCB.Connection = connection;
            NpgsqlDataReader reader = comCB.ExecuteReader();
            comboBox3.Items.Clear();
            while (reader.Read())
            {

				comboBox2.Items.Add(reader.GetValue(0));
                comboBox3.Items.AddRange((string[])reader.GetValue(1));
            }
            reader.Close();


        }           // Вывод видов объекта в ComboBox

		private void showCB2()
		{

			NpgsqlCommand comCB;
			comCB = new NpgsqlCommand();
			comCB.CommandText = "SELECT * FROM Принадлежность";
			comCB.Connection = connection;
			NpgsqlDataReader reader = comCB.ExecuteReader();
			comboBox3.Items.Clear();
			while (reader.Read())
			{

				comboBox2.Items.Add(reader.GetValue(0));
				comboBox3.Items.Add(reader.GetValue(1));
			}
			reader.Close();


		}           // Вывод видов объекта в ComboBox

		private void clearAll1()
        {
            // пользователи
            //textBox4.Clear();
            //textBox8.Clear();
            //textBox1.Clear();
            //textBox7.Clear();
            //textBox5.Clear();
            //textBox6.Clear();
            //textBox27.Clear();

            //dateTimePicker1.Value = DateTime.Today;
        } // очистка полей


        private void сменаПароляToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string password;

            connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
            connection.Open();

            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT Пароль FROM Пользователи WHERE id_пользователя = @id", connection))
            {
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));
                password = cmd.ExecuteScalar().ToString();
            }

            connection.Close();
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form authorization = new Authorization();
            Hide();
            authorization.Show();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
            connection.Open();

			using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM selectto();", connection))

			{
				NpgsqlDataReader reader = cmd.ExecuteReader();
				//adapter1 = new NpgsqlDataAdapter(cmd);
				table1 = new DataTable();
				table1.Columns.Add("id_ТурОбъект"); //Убрать в релизе
				table1.Columns.Add("Название");
				table1.Columns.Add("Адрес");
				table1.Columns.Add("id_ВидОбъекта");
				table1.Columns.Add("Вид Объекта");
				table1.Columns.Add("id_Принадлежность");
				table1.Columns.Add("Принадлежность");
				table1.Columns.Add("Особенности_доступа");
				table1.Columns.Add("Вместимость");
				table1.Columns.Add("Примечание");

				while (reader.Read())
				{
					if (reader.GetValue(4) == DBNull.Value)
					{
						table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), "-",
							reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
					}
					else
					{
						string[] str;
						str = (string[])reader.GetValue(4);
						string st = "";
						for (int i = 0; i < str.Length; i++)
							st += str[i] + ",";
						table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), st,
							reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
					}
				}
				reader.Close();

				dataGridView2.DataSource = table1;
				//dataGridView2.Columns[0].Visible = false;
				//dataGridView2.Columns[3].Visible = false;
				//dataGridView2.Columns[5].Visible = false;

			}

			//using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM КупленныеБилеты WHERE id_пользователь = @ПользовательId; ", connection))
			//{

			//    cmd.Parameters.AddWithValue("@ПользовательId", Convert.ToInt32(id));
			//    adapter3 = new NpgsqlDataAdapter(cmd);
			//    table3 = new DataTable();
			//    adapter3.Fill(table3);
			//    dataGridView4.DataSource = table3;
			//}

			//using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM ВозврашенныеБилеты", connection))
			//{

			//    adapter4 = new NpgsqlDataAdapter(cmd);
			//    table4 = new DataTable();
			//    adapter4.Fill(table4);
			//    dataGridView5.DataSource = table4;
			//}

			connection.Close();
        }


		private void button2_Click(object sender, EventArgs e)
        {
            connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");

                connection.Open();

			switch (comboBox1.SelectedIndex)
			{
                case 0: // название

					if ((textBox2.Text.Trim() == ""))
					{
						MessageBox.Show("Пустые поля не допустимы!", "Контроль данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
						connection.Open();

						using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM selTurNaz(@Название);", connection))
						{
							cmd.Parameters.Clear();
							cmd.Parameters.AddWithValue("@Название", textBox2.Text);
							NpgsqlDataReader reader = cmd.ExecuteReader();
							table1 = new DataTable();
							table1.Columns.Add("id_ТурОбъект"); //Убрать в релизе
							table1.Columns.Add("Название");
							table1.Columns.Add("Адрес");
							table1.Columns.Add("id_ВидОбъекта");
							table1.Columns.Add("Вид Объекта");
							table1.Columns.Add("id_Принадлежность");
							table1.Columns.Add("Принадлежность");
							table1.Columns.Add("Особенности_доступа");
							table1.Columns.Add("Вместимость");
							table1.Columns.Add("Примечание");

							while (reader.Read())
							{
								if (reader.GetValue(4) == DBNull.Value)
								{
									table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), "-",
										reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
								}
								else
								{
									string[] str;
									str = (string[])reader.GetValue(4);
									string st = "";
									for (int i = 0; i < str.Length; i++)
										st += str[i] + ",";
									table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), st,
										reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
								}
							}
							reader.Close();
							dataGridView2.DataSource = table1;
						}

					}

					break; 
                
                case 1: // адрес

					if ((textBox2.Text.Trim() == ""))
					{
						MessageBox.Show("Пустые поля не допустимы!", "Контроль данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
						connection.Open();

						using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM selTurAdr(@Адрес);", connection))
						{
							cmd.Parameters.Clear();
							cmd.Parameters.AddWithValue("@Адрес", textBox2.Text);
							NpgsqlDataReader reader = cmd.ExecuteReader();
							table1 = new DataTable();
							table1.Columns.Add("№"); //Убрать в релизе
							table1.Columns.Add("Название");
							table1.Columns.Add("Адрес");
							table1.Columns.Add("id_ВидОбъекта");
							table1.Columns.Add("Вид Объекта");
							table1.Columns.Add("id_Принадлежность");
							table1.Columns.Add("Принадлежность");
							table1.Columns.Add("Особенности_доступа");
							table1.Columns.Add("Вместимость");
							table1.Columns.Add("Примечание");

							while (reader.Read())
							{
								if (reader.GetValue(4) == DBNull.Value)
								{
									table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), "-",
										reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
								}
								else
								{
									string[] str;
									str = (string[])reader.GetValue(4);
									string st = "";
									for (int i = 0; i < str.Length; i++)
										st += str[i] + ",";
									table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), st,
										reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
								}
							}
							reader.Close();
							dataGridView2.DataSource = table1;
						}
					}

					break;

				case 2: // вид объекта 

					if ((comboBox3.Text.Trim() == ""))
					{
						MessageBox.Show("Пустые поля не допустимы!", "Контроль данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
						connection.Open();

						using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM selTurVid(@Наименование);", connection))
						{
							cmd.Parameters.Clear();
							cmd.Parameters.AddWithValue("@Наименование", comboBox3.Text);
							NpgsqlDataReader reader = cmd.ExecuteReader();
							table1 = new DataTable();
							table1.Columns.Add("№"); //Убрать в релизе
							table1.Columns.Add("Название");
							table1.Columns.Add("Адрес");
							table1.Columns.Add("id_ВидОбъекта");
							table1.Columns.Add("Вид_Объекта");
							table1.Columns.Add("id_Принадлежность");
							table1.Columns.Add("Принадлежность");
							table1.Columns.Add("Особенности_доступа");
							table1.Columns.Add("Вместимость");
							table1.Columns.Add("Примечание");

							while (reader.Read())
							{
								if (reader.GetValue(4) == DBNull.Value)
								{
									table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), "-",
										reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
								}
								else
								{
									string[] str;
									str = (string[])reader.GetValue(4);
									string st = "";
									for (int i = 0; i < str.Length; i++)
										st += str[i] + ",";
									table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), st,
										reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
								}
							}
							reader.Close();
							dataGridView2.DataSource = table1;
						}
					}

					break;

				case 3: // принадлежность 

					if ((comboBox3.Text.Trim() == ""))
					{
						MessageBox.Show("Пустые поля не допустимы!", "Контроль данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
						connection.Open();

						using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM selTurPrin(@Наименование);", connection))
						{
							cmd.Parameters.Clear();
							cmd.Parameters.AddWithValue("@Наименование", comboBox3.Text);
							NpgsqlDataReader reader = cmd.ExecuteReader();
							table1 = new DataTable();
							table1.Columns.Add("№"); //Убрать в релизе
							table1.Columns.Add("Название");
							table1.Columns.Add("Адрес");
							table1.Columns.Add("id_ВидОбъекта");
							table1.Columns.Add("Вид_Объекта");
							table1.Columns.Add("id_Принадлежность");
							table1.Columns.Add("Принадлежность");
							table1.Columns.Add("Особенности_доступа");
							table1.Columns.Add("Вместимость");
							table1.Columns.Add("Примечание");

							while (reader.Read())
							{
								if (reader.GetValue(4) == DBNull.Value)
								{
									table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), "-",
										reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
								}
								else
								{
									string[] str;
									str = (string[])reader.GetValue(4);
									string st = "";
									for (int i = 0; i < str.Length; i++)
										st += str[i] + ",";
									table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), st,
										reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
								}
							}
							reader.Close();
							dataGridView2.DataSource = table1;
						}
					}

					break;

				case 4: // вместимость 

					if ((textBox2.Text.Trim() == ""))
					{
						MessageBox.Show("Пустые поля не допустимы!", "Контроль данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
						connection.Open();

						using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM selTurVmest(@Вместимость);", connection))
						{
							cmd.Parameters.Clear();
							cmd.Parameters.AddWithValue("@Вместимость", textBox2.Text);
							NpgsqlDataReader reader = cmd.ExecuteReader();
							table1 = new DataTable();
							table1.Columns.Add("№"); //Убрать в релизе
							table1.Columns.Add("Название");
							table1.Columns.Add("Адрес");
							table1.Columns.Add("id_ВидОбъекта");
							table1.Columns.Add("Вид Объекта");
							table1.Columns.Add("id_Принадлежность");
							table1.Columns.Add("Принадлежность");
							table1.Columns.Add("Особенности_доступа");
							table1.Columns.Add("Вместимость");
							table1.Columns.Add("Примечание");

							while (reader.Read())
							{
								if (reader.GetValue(4) == DBNull.Value)
								{
									table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), "-",
										reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
								}
								else
								{
									string[] str;
									str = (string[])reader.GetValue(4);
									string st = "";
									for (int i = 0; i < str.Length; i++)
										st += str[i] + ",";
									table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), st,
										reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
								}
							}
							reader.Close();
							dataGridView2.DataSource = table1;
						}
						//MessageBox.Show("Успешно!", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}


					break;

			}

        }

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");

			connection.Open();

            switch (comboBox1.SelectedIndex) 
            {
                case 2:
					
					comboBox3.Visible = true;
					textBox2.Visible = false;
					showCB();
                    break;
                case 3:
					comboBox3.Visible = true;
					textBox2.Visible = false;
					showCB2();
                    break;
                default:
					comboBox3.Visible = false;
					textBox2.Visible = true;
                    break;

			}
			textBox2.Clear();
			comboBox3.Text = "  -- Выберите --";

		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}

		private void label3_Click(object sender, EventArgs e)
		{

		}

		private void button3_Click_1(object sender, EventArgs e)
		{

			connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
			connection.Open();

			using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM selectto();", connection))

			{
				NpgsqlDataReader reader = cmd.ExecuteReader();
				//adapter1 = new NpgsqlDataAdapter(cmd);
				table1 = new DataTable();
				table1.Columns.Add("№т"); //Убрать в релизе
				table1.Columns.Add("Название");
				table1.Columns.Add("Адрес");
				table1.Columns.Add("id_ВидОбъекта");
				table1.Columns.Add("Вид Объекта");
				table1.Columns.Add("id_Принадлежность");
				table1.Columns.Add("Принадлежность");
				table1.Columns.Add("Особенности_доступа");
				table1.Columns.Add("Вместимость");
				table1.Columns.Add("Примечание");

				while (reader.Read())
				{
					if (reader.GetValue(4) == DBNull.Value)
					{
						table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), "-",
							reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
					}
					else
					{
						string[] str;
						str = (string[])reader.GetValue(4);
						string st = "";
						for (int i = 0; i < str.Length; i++)
							st += str[i] + ",";
						table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), st,
							reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
					}
				}
				reader.Close();
				dataGridView2.DataSource = table1;

				//dataGridView2.Columns[0].Visible = false;
				//dataGridView2.Columns[3].Visible = false;
				//dataGridView2.Columns[5].Visible = false;

			}

		}

		private void dataGridView2_SelectionChanged(object sender, EventArgs e)
		{
			int i = dataGridView2.CurrentRow == null ? -1 : dataGridView2.CurrentRow.Index;
			if (i >= 0)
			{

				textBox1.Text = dataGridView2[0, i].Value.ToString(); // ID - Номер объекта
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			int number;
			number = Convert.ToInt32(textBox1.Text);
			Form info = new InformationAbout(number);
			info.Show();

		}

		private void личныйКабинетToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Form authorization = new Authorization();
			authorization.ShowDialog();
		}

		private void button20_Click(object sender, EventArgs e)
        {
            clearAll1();
        } // очистка полей
        
        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Название КП: Учёт туристических объектов Астраханской области\nВыполнил студент: Уланов Бадма Александрович\nГруппа: ДИНРБ31", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void UserMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
			Form user = new UserMenu();
			user.Close();
		}

        private void UserMenu_Load(object sender, EventArgs e)
        {
            connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
            connection.Open();

            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM selectto();", connection))

			{
                NpgsqlDataReader reader = cmd.ExecuteReader();
                //adapter1 = new NpgsqlDataAdapter(cmd);
                table1 = new DataTable();
                table1.Columns.Add("№"); //Убрать в релизе
                table1.Columns.Add("Название");
                table1.Columns.Add("Адрес");
                table1.Columns.Add("id_ВидОбъекта");
				table1.Columns.Add("Вид Объекта");
				table1.Columns.Add("id_Принадлежность");
				table1.Columns.Add("Принадлежность");
				table1.Columns.Add("Особенности_доступа");
                table1.Columns.Add("Вместимость");
                table1.Columns.Add("Примечание");

                while (reader.Read())
                {
                    if (reader.GetValue(4) == DBNull.Value)
                    {
                        table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), "-",
                            reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
                    }
                    else
                    {
                        string[] str;
                        str = (string[])reader.GetValue(4);
                        string st = "";
                        for (int i = 0; i < str.Length; i++)
                            st += str[i] + ",";
                        table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), st,
                            reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
                    }
                }
                reader.Close();
                dataGridView2.DataSource = table1;
				dataGridView2.Columns[3].Visible = false;
				dataGridView2.Columns[5].Visible = false;

			}
            connection.Close();
        }
    }
}
