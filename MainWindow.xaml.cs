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
        Window1 log1 = new(); //создаем экземляр окна
        NpgsqlConnection iConnect = new("Server=127.0.0.1;Port=5432;User Id=postgres;Password=Scorp610;Database=pikachu");
        public MainWindow()
        {
            log1.Hide();
            Conn_init(iConnect); //вызываем метод подключения к БД и чтения таблицы names
            login(0); //вызываем окно логина
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

        public void login(int b)
        {
            log1.ShowDialogs(this); //вызов диалогового окна из родного класса и передача экземпляра главного окна
        }

        public bool[] loginDialogCheck(string Login, string Password)
        {
            bool[] result = { false, false };
                int i = names_title.FindIndex(p => p == Login); //поиск введенного логина в списке имён
                if (i > -1)
                {
                result[0] = true; //логин нашёлся
                if (BCrypt.Verify(Password, names_pass[i])) //проверка пароля
                    {
                    result[1] = true; //пароль совпал
                    iLogin = names_key[i];
                    return result; //успех, все проверки пройдены, передаём результат
                }
                    else
                    {
                        iLogin = -1;
                    return result; //пароль не подошёл, передаём результат
                    }
                }
                else
                {
                    iLogin = -1;
                result[0] = false; //логин не подошёл
                result[1] = true; //проверки пароля не было, сообщения о неправильном пароле не должно быть
                return result; //логина нет в списке, передаём результат
            }
            
        }

    }
}
