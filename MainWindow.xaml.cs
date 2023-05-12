using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Lab8
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var connectionString = ConfigurationManager.ConnectionStrings["SQLServerConnection"].ConnectionString;
            var cnn = new SqlConnection(connectionString);
            cnn.Open();

            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandText = @"Select ФИО from Преподаватели";
            SqlDataReader r = cmd.ExecuteReader();
            listTeachers.Items.Clear();
            while (r.Read())
               listTeachers.Items.Add(String.Format("{0}", r.GetValue(0).ToString()));
            r.Close();

            cnn.Close();
            cnn.Open();
            SqlCommand cmd2 = cnn.CreateCommand();
            cmd2.CommandText = @"SELECT id_преподавателя, SUM(Количество_часов) AS 'Суммарное количество часов' FROM Нагрузка GROUP BY id_преподавателя ORDER BY id_преподавателя ASC;";
            SqlDataReader r2 = cmd2.ExecuteReader();
            List2.Items.Clear();
            while (r2.Read())
            {
                int teacherId = r2.GetInt32(0);
                int hoursw = 0;
                if (!r2.IsDBNull(1))
                {
                    if (r2.GetFieldType(1) == typeof(int))
                    {
                        hoursw = r2.GetInt32(1);
                    }
                    else if (r2.GetFieldType(1) == typeof(decimal))
                    {
                        hoursw = (int)r2.GetDecimal(1);
                    }
                }
                string item = $"Преподаватель {teacherId}: {hoursw} часов";
                List2.Items.Add(item);
            }
                
            r2.Close();
        }

        private void optionChange_Click(object sender, RoutedEventArgs e)
        {
            // Получаем id выбранного преподавателя
            int teacherId = listTeachers.SelectedIndex + 1;

            // Получаем подключение к БД
            var connectionString = ConfigurationManager.ConnectionStrings["SQLServerConnection"].ConnectionString;
            var cnn = new SqlConnection(connectionString);
            cnn.Open();

            // Получаем нагрузки всех штатных преподавателей
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM Нагрузка WHERE id_преподавателя <> {teacherId}";
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet dataset = new DataSet();
            adapter.Fill(dataset);

            // Создаем словарь для хранения нагрузок штатных преподавателей
            Dictionary<int, int> teacherLoads = new Dictionary<int, int>();

            // Считаем общую нагрузку каждого преподавателя
            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                int id = (int)row["id_преподавателя"];
                int hours = (int)row["Количество_часов"];
                if (teacherLoads.ContainsKey(id))
                    teacherLoads[id] += hours;
                else
                    teacherLoads[id] = hours;
            }

            // Получаем данные о свободном преподавателе
            cmd = cnn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM Преподаватели WHERE id = {teacherId}";
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int freeTeacherId = (int)reader["id"];
            int freeTeacherHours = (int)reader["Количество_часов"];
            reader.Close();

            // Проверяем, есть ли у свободного преподавателя свободное время для выполнения задач уволенного преподавателя
            foreach (KeyValuePair<int, int> entry in teacherLoads)
            {
                int id = entry.Key;
                int hours = entry.Value;
                if (freeTeacherHours >= hours)
                {
                    // Можем назначить эту нагрузку свободному преподавателю
                    freeTeacherHours -= hours;
                    cmd = cnn.CreateCommand();
                    cmd.CommandText = $"UPDATE Нагрузка SET id_преподавателя = {freeTeacherId} WHERE id_преподавателя = {id} AND Количество_часов = {hours}";
                    cmd.ExecuteNonQuery();
                }
            }
            cnn.Close();
            MessageBox.Show("Нагрузка была успешно распределена!", "Выполнено");
            cnn.Open();
            SqlCommand cmd2 = cnn.CreateCommand();
            cmd2.CommandText = @"SELECT id_преподавателя, SUM(Количество_часов) AS 'Суммарное количество часов' FROM Нагрузка GROUP BY id_преподавателя ORDER BY id_преподавателя ASC;";
            SqlDataReader r3 = cmd2.ExecuteReader();
            List2.Items.Clear();
            while (r3.Read())
            {
                int teacherId1 = r3.GetInt32(0);
                int hoursw = 0;
                if (!r3.IsDBNull(1))
                {
                    if (r3.GetFieldType(1) == typeof(int))
                    {
                        hoursw = r3.GetInt32(1);
                    }
                    else if (r3.GetFieldType(1) == typeof(decimal))
                    {
                        hoursw = (int)r3.GetDecimal(1);
                    }
                }
                string item = $"Преподаватель {teacherId}: {hoursw} часов";
                List2.Items.Add(item);
            }
            cnn.Close();

            cnn.Open();
            SqlCommand cmd3 = cnn.CreateCommand();
            cmd3.CommandText = $"DELETE FROM Нагрузка WHERE id_преподавателя = {teacherId}; DELETE FROM Преподаватели WHERE id = {teacherId};";
            cmd3.ExecuteNonQuery();
            listTeachers.Items.Remove(listTeachers.SelectedItem);

            r3.Close();
            List2.Items.Refresh();
        }
    }
}
