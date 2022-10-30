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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isOpen = false;
        int iLogin = -1;
        List<int> names_key = new();
        List<string> names_title = new();
        List<string> names_rights = new();
        List<string> names_pass = new();
        NpgsqlCommand? iQuery;
        NpgsqlConnection iConnect = new("Server=127.0.0.1;Port=5432;User Id=postgres;Password=Scorp610;Database=pikachu");
        public MainWindow()
        {
            Conn_init(iConnect); //вызываем метод подключения к БД и чтения таблицы names
            login(0, ""); //вызываем окно логина
            InitializeComponent();
        }
        private async void Conn_init(NpgsqlConnection iConnect) //соединение с БД, обработка ошибок, обработка изменения состояния соединения
        {
            iConnect.StateChange += (object sender, StateChangeEventArgs e) =>
            {
                if (!(e.CurrentState == ConnectionState.Open))
                {
                    isOpen = false;
                }
                else { isOpen = true; }
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

        public void login(int b, string l)
        {
            if (!isOpen) { return; } //проверка есть ли соединение (недоделано)
            Window1 log1 = new(); //создаем экземляр окна
            switch (b) //меняем цвет и подсказки в текстбоксах, если есть входящие параметры
            {
                case 1:
                HintAssist.SetHelperText(log1.PassBox, "Неверный пароль");
                log1.PassBox.Foreground = new SolidColorBrush(Color.FromRgb(255, 98, 80));
                log1.PassBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 98, 80));
                    log1.LoginBox.Text = l;
                    break;

                case 2:
                HintAssist.SetHelperText(log1.LoginBox, "Неверный логин");
                log1.LoginBox.Foreground = new SolidColorBrush(Color.FromRgb(255, 98, 80));
                log1.LoginBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 98, 80));
                    log1.LoginBox.Text = l;
                    break;
            }
            if (log1.ShowDialog() == true) //вызов окна логина и проверка возвращенного результата
            {                
                int i=names_title.FindIndex(p => p == log1.Login); //поиск введенного логина в списке имён
                if (i > -1)
                {
                    if (BCrypt.Verify(log1.Password, names_pass[i])) //проверка пароля
                    {
                        iLogin = names_key[i];
                    } else
                    { 
                        iLogin = -1;
                        login(1,log1.Login); //пароль не подошёл, вызываем метод login ещё раз
                    }
                } 
                else
                {
                    iLogin = -1;
                    login(2, log1.Login); //логина нет в списке, вызываем метод ещё раз
                }
            }
            else
            {
                Application.Current.Shutdown(); //окно логина было закрыто, закрываем программу
            }
        }

    }
}
