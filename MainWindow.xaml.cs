using System;
using System.Collections.Generic;
using System.Linq;
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
using Npgsql;
using MaterialDesignThemes.Wpf;
using System.Diagnostics;
using System.Data;

namespace Pikachu
{
    using BCrypt.Net;
    using MaterialDesignColors;
    using System.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Brush gr = new SolidColorBrush(Color.FromRgb(174, 234, 0));
        Brush rd = new SolidColorBrush(Color.FromRgb(255, 98, 80));
        Brush bl = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        bool isOpen = false;
        int iLogin = -1;
        List<int> names_key = new();
        List<string> names_title = new();
        List<string> names_rights = new();
        List<string> names_pass = new();
        List<pribors> pr = new();
        NpgsqlCommand? iQuery;
        public NpgsqlConnection iConnect = new($"Server ={Properties.Settings.Default.na};" + //создаем строку подключения к БД из парамеров приложения
            $"Port={Properties.Settings.Default.np};User Id={Properties.Settings.Default.lg};" +
            $"Password={Properties.Settings.Default.ps};Database={Properties.Settings.Default.db};");
        public MainWindow()
        {
            InitializeComponent();
            Conn_init(iConnect); //вызываем метод подключения к БД и чтения таблицы names
            login(); //вызываем окно логина
            pr.Add(new pribors
            {
                pribor_num = "12345",
                pribor_tip = "SGOES",
                pribor_mat = "Алюминий",
                pribor_gaz = "Метан",
                pribor_exp = "USA",
                pribor_range = "0-100",
                last_date = "20-20-20",
                last_status = "Выпущен",
                last_name = "Родионов"
            });
            pr.Add(new pribors
            {
                pribor_num = "55577",
                pribor_tip = "SGOES",
                pribor_mat = "Алюминий",
                pribor_gaz = "Метан",
                pribor_exp = "USA",
                pribor_range = "0-100",
                last_date = "20-20-20",
                last_status = "Выпущен",
                last_name = "Родионов"
            });
            lvDataBinding.ItemsSource = pr;
        }
        public class pribors
        {
            public string? pribor_num { get; set; }
            public string? pribor_tip { get; set; }
            public string? pribor_mat { get; set; }
            public string? pribor_gaz { get; set; }
            public string? pribor_exp { get; set; }
            public string? pribor_range { get; set; }
            public string? last_date { get; set; }
            public string? last_status { get; set; }
            public string? last_name { get; set; }
        }
        public async Task Conn_init(NpgsqlConnection iConnect) //соединение с БД, обработка ошибок, обработка изменения состояния соединения
        {
            iConnect.StateChange += (object sender, StateChangeEventArgs e) =>
            {
                if (!(e.CurrentState == ConnectionState.Open))
                {
                    isOpen = false;
                    isDis.IsChecked = true;
                    isDis_i.Foreground = rd;
                    isDis.IsChecked = true;
                }
                else
                {
                    isOpen = true;
                    isDis.IsChecked = false;
                    isDis_i.Foreground = bl;
                    isCon.IsChecked = true;
                }
            };
            try
            {
                iConnect.Open(); //открываем соеднение с БД
                string sql = "SELECT * FROM names;";
                iQuery = new(sql, iConnect); //читаем из БД таблицу пользователей...
                var reader = await iQuery.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    names_key.Add(reader.GetInt32(0)); //...и заносим полученные данные в списки
                    names_title.Add(reader.GetString(1));
                    names_rights.Add(reader.GetString(2));
                    names_pass.Add(reader.GetString(3));
                }
                await reader.CloseAsync();
                iQuery.Dispose();
            }
            catch (NpgsqlException e)
            {
                if (e.Message.Contains("28P01"))
                {
                    MessageBox.Show("Неверный логин/пароль БД");
                    isOpen = false;
                }
                else if (e.Message.Contains("3D000"))
                {
                    MessageBox.Show("Неверное имя базы данных");
                    isOpen = false;
                }
                else
                {
                    MessageBox.Show(e.Message);
                    isOpen = false;
                }
            }
        }

        public void login()
        {
            Window1 log1 = new(); //создаем экземляр окна
            log1.ShowDialogs(this); //вызов диалогового окна из родного класса и передача экземпляра главного окна
        }

        public bool[] loginDialogCheck(string Login, string Password)
        {
            bool[] result = { false, false, true }; //result[0] = флаг проверки логина; result[1] = флаг проверки пароля; result[2] = флаг проверки соединения
            int i = names_title.FindIndex(p => p == Login); //поиск введенного логина в списке имён
            if (i > -1)
            {
                result[0] = true; //логин нашёлся
                if (BCrypt.Verify(Password, names_pass[i])) //проверка пароля
                {
                    result[1] = true; //пароль совпал
                    iLogin = names_key[i];
                    login_text.Text = names_title[i]; //пишем имя пользователя в главном окне
                    return result; //успех, все проверки пройдены, передаём результат
                }
                else
                {
                    iLogin = -1;
                    login_text.Text = "Вход не выполнен";
                    return result; //пароль не подошёл, передаём результат
                }
            }
            else
            {
                if (isOpen)
                {
                    iLogin = -1;
                    login_text.Text = "Вход не выполнен";
                    result[1] = true; //проверки пароля не было, сообщения о неправильном пароле не должно быть
                    return result; //логина нет в списке, передаём результат
                }
                else
                {
                    iLogin = -1;
                    login_text.Text = "Вход не выполнен";
                    result[0] = true; //проверки логина не было, сообщения о несуществующем логине не должно быть
                    result[1] = true; //проверки пароля не было, сообщения о неправильном пароле не должно быть
                    result[2] = false; //соединение недоступно, вешаем флаг
                    return result;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void MainWindow1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void isDis_Checked(object sender, RoutedEventArgs e)
        {
            iConnect.Close();
        }
        private void isCon_Checked(object sender, RoutedEventArgs e)
        {
            Conn_init(iConnect);
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Conn_init(iConnect); //читаем заново таблицу имён и перелогиниваемся
            MainWindow1.Hide();
            login();
            MainWindow1.Show();
        }
    }
}
