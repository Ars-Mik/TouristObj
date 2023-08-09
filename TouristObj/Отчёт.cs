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
using System.Globalization;


namespace TouristObj
{
    public partial class Отчёт : Form
    {
        public Отчёт()
        {
            InitializeComponent();
        }
        NpgsqlConnection connection, connection2;
        NpgsqlCommand command;
        NpgsqlDataReader reader;
        bool ifcon = false; // Флаг соединения с бд

        private void Отчёт_Load_1(object sender, EventArgs e)
        {
            try
            {
                connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
                connection.Open();
                connection2 = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
                connection2.Open();
                ifcon = true;
            }
            catch (System.Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка доступа к базе данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Отчёт_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ifcon)
            {
                connection.Close();
                connection2.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=tourist_objects;User Id=postgres;Password=1234");
            connection.Open();

            DataTable DT = new DataTable();
            // В этой таблице заказываем две колонки "Фильм" и "Кассовый сбор"
            DT.Columns.Add("КиносеансId");
            DT.Columns.Add("Кассовый сбор");
            DT.Columns.Add("Дата");
            DateTime dat1 = dateTimePicker1.Value;
            DateTime dat2 = dateTimePicker2.Value;
            // Приведение даты к формату dd.mm.yyyy
            // Дата 1
            int d1 = dat1.Day;
            int m1 = dat1.Month;
            int y1 = dat1.Year;
            string dats1 = "";
            if (m1 < 10)
                dats1 = Convert.ToString(d1) + ".0" + Convert.ToString(m1) + "." + Convert.ToString(y1);
            else
                dats1 = Convert.ToString(d1) + "." + Convert.ToString(m1) + "." + Convert.ToString(y1);

            // Дата 2
            int d2 = dat2.Day;
            int m2 = dat2.Month;
            int y2 = dat2.Year;
            string dats2 = "";
            if (m2 < 10)
                dats2 = Convert.ToString(d2) + ".0" + Convert.ToString(m2) + "." + Convert.ToString(y2);
            else
                dats2 = Convert.ToString(d2) + "." + Convert.ToString(m2) + "." + Convert.ToString(y2);

            //string temp = "";

            // SUM(Цена) AS [Сумма]
            NpgsqlCommand cmd = new NpgsqlCommand(@"SELECT КиносеансId, SUM(Цена), [Дата] AS [Кассовый сбор]  FROM [КупленныеБилеты2]" +
                       "WHERE [Дата] >= @dats1 AND [Дата] <= @dats2 " +
                       "GROUP BY КиносеансId, Цена, [Дата] ", connection);

            //command = new SqlCommand(cmd, connection);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("dats1", dats1);
            cmd.Parameters.AddWithValue("dats2", dats2);
            reader = cmd.ExecuteReader();
            bool flag = false;
            long ido;
            long old_ido =-1;
            Decimal st;
            decimal sto = 0;
            int kol = 0;
            string name = "";
            NpgsqlCommand command2 = new NpgsqlCommand();
            while (reader.Read())
            {
                flag = true; // Есть данные за указанный период

                ido = Convert.ToInt64(reader.GetValue(0));
                st = Convert.ToDecimal(reader.GetValue(1));
                sto += st;

                 // Поиск наименования отделения по ID_отделения
                    command2 = new NpgsqlCommand("SELECT КиносеансId FROM [КупленныеБилеты2] WHERE КиносеансId = " + ido, connection2);
                    // Получение наименования отделения
                    name = Convert.ToString(command2.ExecuteScalar());
                    DT.Rows.Add(name, Math.Ceiling(sto));
                    sto = st;
               
            }
            reader.Close();
            // Поиск наименования отделения по ID_отделения
            command2 = new NpgsqlCommand("SELECT КиносеансId FROM [КупленныеБилеты2] WHERE КиносеансId = " + old_ido, connection2);
            // Получение наименования отделения
            name = Convert.ToString(command2.ExecuteScalar());
            DT.Rows.Add(name, Math.Ceiling(sto));

            dataGridView1.DataSource = DT;
            if (flag != true) // Есть ли данные
            {
                MessageBox.Show("Отсутствуют данные за указанный период", "Отсутствие данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Составленную таблицу указываем в качестве источника данных:
            chart1.DataSource = DT;
            // По горизонтальной оси откладываем названия отделений:
            chart1.Series["Series1"].XValueMember = "КиносеансId";
            // По вертикальной оси откладываем объёмы затрат:
            chart1.Series["Series1"].YValueMembers = "Кассовый сбор";
            // Название графика (диаграммы):
            chart1.Titles.Clear();
            chart1.Titles.Add("Кассовый сбор определённого фильма ");
            // Задаём тип диаграммы по умолчанию - столбиковая диаграмма:
            chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chart1.Series["Series1"].Color = Color.Orange;

        }
    }
}
