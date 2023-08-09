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
            Form authorization = new Authorization();
            Hide();
            authorization.ShowDialog();
        }

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

            // фильмы
            textBox13.Clear();
            textBox15.Clear();
            textBox12.Clear();
            textBox14.Clear();
             textBox9.Clear();

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
				table1.Columns.Add("ID_ВидОбъекта");
				table1.Columns.Add("Вид Объекта");
				table1.Columns.Add("ID_Принадлежность");
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
				table.Columns.Add("ID_ВидОбъекта"); //Убрать в релизе
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
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void tabPage5_Click(object sender, EventArgs e)
        {

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
                connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=Cinema;User Id=postgres;Password=Admin1234");
                connection.Open();

                using (NpgsqlCommand cmd1 = new NpgsqlCommand(@"INSERT INTO Пользователи (Логин, Пароль, Имя, Фамилия) VALUES (@Логин, @Пароль, @Имя, @Фамилия)", connection))
                {
                    cmd1.Parameters.AddWithValue("@Логин", textBox4.Text);
                    cmd1.Parameters.AddWithValue("@id", Convert.ToInt32(textBox8.Text));

                    cmd1.ExecuteNonQuery();
                }
                MessageBox.Show("Пользователь добавлен!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=Cinema;User Id=postgres;Password=Admin1234");
                connection.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE Пользователи SET Логин = @Логин, Пароль = @Пароль, Имя = @Имя," +
                    "Фамилия = @Фамилия " + "WHERE id_пользователя = @id;", connection))
                {
                    cmd.Parameters.AddWithValue("@Логин", textBox4.Text);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(textBox8.Text));
                    //cmd.ExecuteNonQuery();
                    cmd.ExecuteScalar();
                }
                MessageBox.Show("Изменения сохранены", "Изменение записи", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        } // изменить данные
        private void button6_Click(object sender, EventArgs e)
        {
            connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=Cinema;User Id=postgres;Password=Admin1234");
            connection.Open();

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Пользователи WHERE id_пользователя = @id", connection))
                {
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(textBox8.Text));
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Пользователь удален!", "Пользователь удален", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Ошибка удаления пользователя!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // удаление 


        // -------------- Кнопки выполнения действий в базе данных Туристические объекты ------------------ //
        private void button9_Click(object sender, EventArgs e)
        {
            if (!ifNull()) // Проверка полей на пустые значения
            {

                connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=Cinema;User Id=postgres;Password=Admin1234");
                connection.Open();

                string[] str = textBox1.Text.Split(';', ',');

                using (NpgsqlCommand cmd1 = new NpgsqlCommand(@"CALL addfilms(@ФИО, @Жанр, @Название, @ГодПроизводства, @Возраст, @Длительность, @Описание)", connection))
                {
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.Clear();
                    cmd1.Parameters.Add("@ФИО",            NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox1.Text;
                    cmd1.Parameters.AddWithValue("@Жанр", str);
                    cmd1.Parameters.Add("@Название",       NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox13.Text;
                    cmd1.Parameters.Add("@ГодПроизводства",NpgsqlTypes.NpgsqlDbType.Text).Value = textBox12.Text;
                    cmd1.Parameters.Add("@Возраст",        NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox15.Text;
                    //cmd1.Parameters.Add("@Длительность",   NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox34.Text;
                    cmd1.Parameters.Add("@Описание",       NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox14.Text;

                    cmd1.ExecuteScalar();
                }
                MessageBox.Show("Фильм добавлен!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Пустые поля не допустимы!", "Контроль данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
        } // добавление 
        private void button7_Click(object sender, EventArgs e)
        {
            connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
            connection.Open();

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Фильмы WHERE id_фильма = @id; ", connection))
                {
                    cmd.Parameters.AddWithValue("@Название", textBox13.Text);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(textBox9.Text));
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Успешное удаление фильма!", "Фильм удален", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Ошибка удаления фильма!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // удаление 
        private void label13_Click(object sender, EventArgs e)
        {

        }
        private void button8_Click(object sender, EventArgs e) 
        {
            if (!ifNull()) // Проверка полей на пустые значения
            {
                connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
                connection.Open();

                string[] str = textBox1.Text.Split(';', ',');

                using (NpgsqlCommand cmd1 = new NpgsqlCommand(@"CALL updfilms(@ФИО, @Жанр, @Название, @ГодПроизводства, @Возраст, @Длительность, @Описание, @idJ, @idR, @idF)", connection))
                {
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.Clear();
                    cmd1.Parameters.Add("@ФИО", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox1.Text;
                    cmd1.Parameters.AddWithValue("@Жанр", str);
                    cmd1.Parameters.Add("@Название", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox13.Text;
                    cmd1.Parameters.Add("@ГодПроизводства", NpgsqlTypes.NpgsqlDbType.Text).Value = textBox12.Text;
                    cmd1.Parameters.Add("@Возраст", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox15.Text;
                    //cmd1.Parameters.Add("@Длительность", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox34.Text;
                    cmd1.Parameters.Add("@Описание", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox14.Text;
                    cmd1.Parameters.Add("@idJ", NpgsqlTypes.NpgsqlDbType.Integer).Value = Convert.ToInt32(textBox1.Text);
                    cmd1.Parameters.Add("@idR", NpgsqlTypes.NpgsqlDbType.Integer).Value = Convert.ToInt32(textBox2.Text);
                    cmd1.Parameters.Add("@idF", NpgsqlTypes.NpgsqlDbType.Integer).Value = Convert.ToInt32(textBox9.Text);

                    cmd1.ExecuteScalar();
                }

                MessageBox.Show("Изменения сохранены", "Изменение записи", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if ((textBox18.Text.Trim() == "" || textBox22.Text.Trim() == ""))
            {
                MessageBox.Show("Пустые поля не допустимы!", "Контроль данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
                connection.Open();

                using (NpgsqlCommand cmd1 = new NpgsqlCommand(@"INSERT INTO Киносеансы (Дата, Время, id_фильма, Зал, Цена) VALUES (@Дата, @Время, @Filmid, @Зал, @Цена)", connection))
                {
                    //cmd1.Parameters.AddWithValue("@Id", textBox18.Text);
                    cmd1.Parameters.AddWithValue("@Дата", textBox22.Text);

                    //cmd1.Parameters.AddWithValue("@FilmId", Convert.ToInt32(textBox20.Text));


                    cmd1.ExecuteNonQuery();
                }
                MessageBox.Show("Киносеанс добавлен!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE Киносеансы SET Дата = @Дата, Время = @Время, id_фильма = @Filmid," +
                    "Зал = @Зал, Цена = @Цена " + "WHERE id_киносеанса = @id;", connection))
                {
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(textBox18.Text));
                    cmd.Parameters.AddWithValue("@Дата", textBox22.Text);
                    //cmd.ExecuteNonQuery();
                    cmd.ExecuteScalar();
                }
                MessageBox.Show("Изменения сохранены!", "Изменение записи", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        } // изменение 
        private void button10_Click(object sender, EventArgs e)
        {
            connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
            connection.Open();

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Киносеансы WHERE id_киносеанса = @id; ", connection))
                {
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(textBox18.Text));
                    cmd.Parameters.AddWithValue("@Дата", textBox22.Text);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Успешное удаление киносеанса!", "Фильм удален", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Ошибка удаления киносеанса!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // удаление 



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
				textBox1.Text = dataGridView2[3, d].Value.ToString(); // вид объекта (id)
				comboBox1.Text = dataGridView2[4, d].Value.ToString(); // вид объекта (название)
				textBox2.Text = dataGridView2[5, d].Value.ToString(); // принадлежность (id)
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
				table.Columns.Add("ID_ВидОбъекта"); //Убрать в релизе
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
							st += str[i] + ";";
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
				table1.Columns.Add("ID_ВидОбъекта");
				table1.Columns.Add("Вид Объекта");
				table1.Columns.Add("ID_Принадлежность");
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

			using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Принадлежность; ", connection))
            {
                adapter2 = new NpgsqlDataAdapter(cmd);
                table2 = new DataTable();
                adapter2.Fill(table2);
                dataGridView3.DataSource = table2;
                //dataGridView3.Columns[0].Visible = true;
            }

            connection.Close();
        }
    }
}
