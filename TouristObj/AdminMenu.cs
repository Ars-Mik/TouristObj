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
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace TouristObj
{
    public partial class AdminMenu : Form
    {
        NpgsqlDataAdapter adapter, adapter1, adapter2, adapter3, adapter4;
        DataTable table, table1, table2, table3, table4 = null;
        public AdminMenu()
        {
            InitializeComponent();
        }
        bool ifcon = false;  // Флаг соединения с базой данных

        NpgsqlConnection connection;
        
        private void AdminMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
			Form admin = new AdminMenu();
			admin.Close();
		}
        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form user = new UserMenu();
            Hide();
            user.ShowDialog();
        }

		private void showCB()
		{

			NpgsqlCommand comCB;
			comCB = new NpgsqlCommand();
			comCB.CommandText = "SELECT * FROM Принадлежность";
			comCB.Connection = connection;
			NpgsqlDataReader reader = comCB.ExecuteReader();
			comboBox1.Items.Clear();
			while (reader.Read())
			{

				comboBox3.Items.Add(reader.GetValue(0));
				comboBox2.Items.Add(reader.GetValue(1));
			}
			reader.Close();


		} // Вывод принадлежность объекта в ComboBox

		private bool ifNull()
        {
            if (textBox13.Text.Trim() == "" || textBox9.Text.Trim() == "" ||
                textBox12.Text.Trim() == "" ||
                textBox15.Text.Trim() == "" || textBox14.Text.Trim() == "")
            {
                MessageBox.Show("Пустые поля не допустимы!", "Контроль данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        } // проверка полей на пустые значения
        private void clearAll()
        {
            // пользователи
            textBox8.Clear();
            textBox4.Clear();

            // туробъекты
            textBox13.Clear();
            textBox15.Clear();
            textBox12.Clear();
            textBox14.Clear();
             textBox3.Clear();

            // киносеансы
            textBox18.Clear();
            textBox22.Clear();

        } // очистка полей

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
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

			using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Вид_Объекта; ", connection))
            {

                NpgsqlDataReader reader = cmd.ExecuteReader();
                //adapter1 = new NpgsqlDataAdapter(cmd);
                table = new DataTable();
				table.Columns.Add("id_ВидОбъекта"); //Убрать в релизе       
				table.Columns.Add("Наименование");

				ifcon = true;
                //adapter1.Fill(table1);

                while (reader.Read())
                {
                    if (reader.GetValue(1) == DBNull.Value)
                    {
                        table.Rows.Add(reader.GetValue(0), "-" );
                    }
                    else
                    {
                        string[] str;
                        str = (string[])reader.GetValue(1);
                        string st = "";
                        for (int i = 0; i < str.Length; i++)
                            st += str[i] + ";  ";
                        table.Rows.Add(reader.GetValue(0), st );
                    }
                }
                reader.Close();

                dataGridView1.DataSource = table;
				comboBox1.Items.Clear();
				foreach (DataGridViewRow row in dataGridView1.Rows)
				{
					comboBox1.Items.Add(row.Cells[1].Value.ToString());

				}
				//dataGridView2.Columns[0].Visible = false;
			}

            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Принадлежность;", connection))
            {
                adapter2 = new NpgsqlDataAdapter(cmd);
                table2 = new DataTable();
                adapter2.Fill(table2);
                dataGridView3.DataSource = table2;
            }
           
            connection.Close();
        }
 

        // -------------- Кнопки выполнения действий в базе данных Виды Объектов ------------------ //
        private void button4_Click(object sender, EventArgs e)
        {
            if ((textBox4.Text.Trim() == "" || textBox8.Text.Trim() == ""))
            {
                MessageBox.Show("Пустые поля не допустимы!", "Контроль данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
				connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
				connection.Open();

				string[] str = textBox4.Text.Split(';', ',');

				using (NpgsqlCommand cmd1 = new NpgsqlCommand(@"INSERT INTO Вид_Объекта (Наименование) VALUES (@Наименование)", connection))
                {
					cmd1.CommandType = CommandType.Text;
					cmd1.Parameters.Clear();
					cmd1.Parameters.AddWithValue("@Наименование", str);
					cmd1.ExecuteScalar();
					
				}
                MessageBox.Show("Запись успешно добавлена!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
				AdminMenu_Load(sender, e);

			}
        } // добавление
        private void button5_Click(object sender, EventArgs e)
        {

            if ((textBox4.Text.Trim() == "" || textBox8.Text.Trim() == ""))
            {
                MessageBox.Show("Пустые поля не допустимы!", "Контроль данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
				connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
				connection.Open();
				string[] str = textBox4.Text.Split(';', ',');

				using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE Вид_Объекта SET Наименование = @Наименование WHERE id_ВидОбъекта = @id;", connection))
                {
                    cmd.Parameters.AddWithValue("@Наименование", str);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(textBox8.Text));
                    cmd.ExecuteScalar();
                }
                MessageBox.Show("Изменения сохранены", "Изменение записи", MessageBoxButtons.OK, MessageBoxIcon.Information);
				AdminMenu_Load(sender, e);
			}

        } // изменить данные
        private void button6_Click(object sender, EventArgs e)
        {
			connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
			connection.Open();

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Вид_Объекта WHERE id_ВидОбъекта = @id", connection))
                {
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(textBox8.Text));
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Запись удаленa!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
				AdminMenu_Load(sender, e);
			}
            catch
            {
                MessageBox.Show("Ошибка удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // удаление 


        // -------------- Кнопки выполнения действий в базе данных Туристические объекты ------------------ //
        private void button9_Click(object sender, EventArgs e)
        {
            if (!ifNull()) // Проверка полей на пустые значения
            {

                connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
                connection.Open();
                
                using (NpgsqlCommand cmd1 = new NpgsqlCommand(@"CALL addTurObj(@Название, @Адрес, @id_ВидОбъекта, @id_Принадлежность, @Особенности_доступа, @Вместимость, @Примечание)", connection))
                {
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.Clear();
                    cmd1.Parameters.Add("@Название",        NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox13.Text;
                    cmd1.Parameters.Add("@Адрес",           NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox12.Text;
                    cmd1.Parameters.Add("@id_ВидОбъекта",   NpgsqlTypes.NpgsqlDbType.Integer).Value = Convert.ToInt32(comboBox5.Text);
                    cmd1.Parameters.Add("@id_Принадлежность",   NpgsqlTypes.NpgsqlDbType.Integer).Value = Convert.ToInt32(comboBox3.Text);
                    cmd1.Parameters.Add("@Особенности_доступа", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox15.Text;
                    cmd1.Parameters.Add("@Вместимость",         NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox3.Text;
					cmd1.Parameters.Add("@Примечание",          NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox14.Text;
					cmd1.ExecuteScalar();
                }
                MessageBox.Show("Запись успешно добавлена!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
				AdminMenu_Load(sender, e);
			}
            else
                MessageBox.Show("Пустые поля не допустимы!", "Контроль данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }  // добавление 
        private void button7_Click(object sender, EventArgs e)
        {
            connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
            connection.Open();

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM ТурОбъекты WHERE id_ТурОбъект = @id; ", connection))
                {
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(textBox9.Text));
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Запись была успешно удалена!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
				AdminMenu_Load(sender, e);
			}
            catch
            {
                MessageBox.Show("Ошибка удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }  // удаление    
        private void button8_Click(object sender, EventArgs e) 
        {
			if (!ifNull()) // Проверка полей на пустые значения
			{

				connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
				connection.Open();

				using (NpgsqlCommand cmd1 = new NpgsqlCommand(@"CALL updTurObj(@Название, @Адрес, @id_ВидОбъекта, @id_Принадлежность, @Особенности_доступа, @Вместимость, @Примечание, @id_ТурОбъект)", connection))
				{
					cmd1.CommandType = CommandType.Text;
					cmd1.Parameters.Clear();
					cmd1.Parameters.Add("@Название", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox13.Text;
					cmd1.Parameters.Add("@Адрес", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox12.Text;
					cmd1.Parameters.Add("@id_ВидОбъекта", NpgsqlTypes.NpgsqlDbType.Integer).Value = Convert.ToInt32(comboBox5.Text);
					cmd1.Parameters.Add("@id_Принадлежность", NpgsqlTypes.NpgsqlDbType.Integer).Value = Convert.ToInt32(comboBox3.Text);
					cmd1.Parameters.Add("@Особенности_доступа", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox15.Text;
					cmd1.Parameters.Add("@Вместимость", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox3.Text;
					cmd1.Parameters.Add("@Примечание", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox14.Text;
					cmd1.Parameters.Add("@id_ТурОбъект", NpgsqlTypes.NpgsqlDbType.Integer).Value = Convert.ToInt32(textBox9.Text);
					cmd1.ExecuteScalar();
				}
				MessageBox.Show("Запись успешно изменена!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
				AdminMenu_Load(sender, e);
			}
			else
				MessageBox.Show("Пустые поля не допустимы!", "Контроль данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
		} // Изменение 
        private void button19_Click(object sender, EventArgs e)
        {
            clearAll();
        } // очистить поля фильмов


        // -------------- Кнопки выполнения действий в базе данных Принадлежность ------------------ //
        private void button12_Click(object sender, EventArgs e)
        {
            if ((textBox22.Text.Trim() == ""))
            {
                MessageBox.Show("Пустые поля не допустимы!", "Контроль данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
                connection.Open();

                using (NpgsqlCommand cmd1 = new NpgsqlCommand(@"INSERT INTO Принадлежность (Наименование) VALUES (@Наименование)", connection))
                {
                    cmd1.Parameters.AddWithValue("@Наименование", textBox22.Text);
                    cmd1.ExecuteNonQuery();
				}
                MessageBox.Show("Запись добавлена!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
				AdminMenu_Load(sender, e);
			}
        } // добавление 
        private void button11_Click(object sender, EventArgs e)
        {
            if ((textBox18.Text.Trim() == "" || textBox22.Text.Trim() == ""))
            {
                MessageBox.Show("Пустые поля не допустимы!", "Контроль данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
                connection.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE Принадлежность SET Наименование = @Наименование WHERE id_Принадлежность = @id;", connection))
                {
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(textBox18.Text));
                    cmd.Parameters.AddWithValue("@Наименование", textBox22.Text);

                    cmd.ExecuteScalar();
                }
                MessageBox.Show("Изменения сохранены!", "Изменение записи", MessageBoxButtons.OK, MessageBoxIcon.Information);
				AdminMenu_Load(sender, e);
			}

        } // изменение 
        private void button10_Click(object sender, EventArgs e)
        {
            connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
            connection.Open();

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Принадлежность WHERE id_Принадлежность = @id; ", connection))
                {
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(textBox18.Text));
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Успешное удаление!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
				AdminMenu_Load(sender, e);
			}
            catch
            {
                MessageBox.Show("Ошибка удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // удаление 


		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
            comboBox2.Text = (sender as ComboBox).SelectedItem.ToString();

            if (comboBox2.Text == (sender as ComboBox).SelectedItem.ToString())
            {
                comboBox3.SelectedIndex = comboBox2.SelectedIndex;
            }
        }
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			comboBox1.Text = (sender as ComboBox).SelectedItem.ToString();

			if (comboBox1.Text == (sender as ComboBox).SelectedItem.ToString())
			{
				comboBox5.SelectedIndex = comboBox1.SelectedIndex;
			}
		}

		// ---------------- заполнение полей из базы данных --------------------- //
		private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            int i = dataGridView3.CurrentRow == null ? -1 : dataGridView3.CurrentRow.Index;
            if (i >= 0)
            {

                textBox18.Text = dataGridView3[0, i].Value.ToString(); // скрытый ид
                textBox22.Text = dataGridView3[1, i].Value.ToString(); // Название
            }
        } // тур объекты
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            
			int i = dataGridView1.CurrentRow == null ? -1 : dataGridView1.CurrentRow.Index;
            if (i >= 0)
            {
				textBox8.Text = dataGridView1[0, i].Value.ToString(); // скрытый ид
				textBox4.Text = dataGridView1[1, i].Value.ToString();

            }
        } // виды объектов 
		private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            int d = dataGridView2.CurrentRow == null ? -1 : dataGridView2.CurrentRow.Index;
            if (d >= 0)
            {

                textBox9.Text = dataGridView2[0, d].Value.ToString(); // ид объекта
                textBox13.Text = dataGridView2[1, d].Value.ToString(); // Название
                textBox12.Text = dataGridView2[2, d].Value.ToString(); // адрес
				comboBox5.Text = dataGridView2[3, d].Value.ToString(); // вид объекта (id)
				comboBox1.Text = dataGridView2[4, d].Value.ToString(); // вид объекта (название)
				comboBox3.Text = dataGridView2[5, d].Value.ToString(); // принадлежность (id)
				comboBox2.Text = dataGridView2[6, d].Value.ToString(); // принадлежность (название)
				textBox15.Text = dataGridView2[7, d].Value.ToString(); // особенности доступа
				textBox3.Text = dataGridView2[8, d].Value.ToString(); // вместимость
				textBox14.Text = dataGridView2[9, d].Value.ToString(); // Описание 
            }
        } // принадлежность


        private void button22_Click(object sender, EventArgs e)
        {
            clearAll();
        } // очистка полей
        private void отчётToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Отчёт = new Отчёт();
            Отчёт.ShowDialog();
        }
        private void button23_Click(object sender, EventArgs e)
        {
            clearAll();
        } // очистка полей


        // ---------------------------------------------------------------------- //
        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Название КП: Учёт туристических объектов Астраханской области\nВыполнил студент: Уланов Бадма Александрович\nГруппа: ДИНРБ31", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AdminMenu_Load(object sender, EventArgs e)
        {

            connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
            connection.Open();

			using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Вид_Объекта; ", connection))
            {
				NpgsqlDataReader reader = cmd.ExecuteReader();
				//adapter1 = new NpgsqlDataAdapter(cmd);
				table = new DataTable();
				table.Columns.Add("id_ВидОбъекта"); //Убрать в релизе
				table.Columns.Add("Наименование");

				while (reader.Read())
				{
					if (reader.GetValue(1) == DBNull.Value)
					{
						table.Rows.Add(reader.GetValue(0), "-");
					}
					else
					{
						string[] str;
						str = (string[])reader.GetValue(1);
						string st = "";
						for (int i = 0; i < str.Length; i++)
							st += str[i] + ",";
						table.Rows.Add(reader.GetValue(0), st);
					}
				}
				reader.Close();
				dataGridView1.DataSource = table;
			}

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
						string st1 = "";
						for (int i = 0; i < str.Length; i++)
							st1 += str[i] + ",";
						table1.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), st1,
							reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetValue(8), reader.GetValue(9));
					}
				}
				reader.Close();
				dataGridView2.DataSource = table1;
                dataGridView2.Columns[3].Visible = false;
                dataGridView2.Columns[5].Visible = false;

            }

			using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Принадлежность; ", connection))
            {
                adapter2 = new NpgsqlDataAdapter(cmd);
                table2 = new DataTable();
                adapter2.Fill(table2);
                dataGridView3.DataSource = table2;
                //dataGridView3.Columns[0].Visible = true;
            }
			
            showCB();
			comboBox1.Items.Clear();
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				comboBox1.Items.Add(row.Cells[1].Value.ToString());
				comboBox5.Items.Add(row.Cells[0].Value.ToString());
			}
			connection.Close();
        }
    }
}
