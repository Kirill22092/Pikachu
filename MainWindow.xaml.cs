using MaterialDesignThemes.Wpf;
using Npgsql;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Pikachu
{

    using BCrypt.Net;
    using System.Threading;

    public partial class MainWindow : Window
    {
        private NpgsqlCommand? iQuery;
        public NpgsqlConnection iConnect = new($"Server ={Properties.Settings.Default.na};" +
            $"Port={Properties.Settings.Default.np};User Id={Properties.Settings.Default.lg};" +
            $"Password={Properties.Settings.Default.ps};Database={Properties.Settings.Default.db};Keepalive=5;"); //создаем строку подключения к БД из параметров приложения

        public MainWindow()
        {
            InitializeComponent();
            paint();
            before_login();
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
        public bool Connect()
        {
            try
            {
                iConnect.Open();
                return true;
            }
            catch (NpgsqlException e)
            {
                if (e.Message.Contains("28P01"))
                {
                    _ = MessageBox.Show("Неверный логин/пароль БД");
                }
                else if (e.Message.Contains("3D000"))
                {
                    _ = MessageBox.Show("Неверное имя базы данных");
                }
                else
                {
                    if (e.Message == "Exception while writing to stream")
                    { _ = MessageBox.Show("Прервано подключение к БД"); }
                    else
                    {
                        _ = MessageBox.Show(e.Message);
                    }

                }
                return false;
            }
        }

        public void read_names() //Чтение таблицы names
        {
            if (My.ThreadState != ThreadState.Running && My.ThreadState != ThreadState.WaitSleepJoin)
            {
                My = new Thread(() =>
                {
                    lock (locker)
                    {
                        Debug.WriteLine(My.ThreadState.ToString());
                        if (iConnect.State == ConnectionState.Open)
                        {
                            string sql = "SELECT * FROM names;";
                            iQuery = new(sql, iConnect); //читаем из БД таблицу пользователей...
                            NpgsqlDataReader reader = iQuery.ExecuteReader();
                            if (iConnect.State == ConnectionState.Open)
                            {
                                while (reader.Read())
                                {
                                    names_key.Add(reader.GetInt32(0));
                                    names_title.Add(reader.GetString(1));
                                    names_rights.Add(reader.GetString(2));
                                    names_pass.Add(reader.GetString(3)); //...и заносим полученные данные в списки
                                }
                            }
                            reader.Close();
                            iQuery.Dispose();
                        }
                    }
                });
                My.Start();
            }
        }

        public void login()
        {
            Window1 log1 = new(this); //создаем экземляр окна логина с передачей экземпляра главного окна в виде аргумента
            _ = log1.ShowDialog(); //вызов диалогового окна логина
            combo_pribors.ItemsSource = names_title;
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
                    login_text.Text = names_title[i]; //пишем имя пользователя в главном окне
                    return result; //успех, все проверки пройдены, передаём результат
                }
                else
                {
                    login_text.Text = "Вход не выполнен";
                    return result; //пароль не подошёл, передаём результат
                }
            }
            else
            {
                if (iConnect.State == ConnectionState.Open)
                {
                    login_text.Text = "Вход не выполнен";
                    result[1] = true; //проверки пароля не было, сообщения о неправильном пароле не должно быть
                    return result; //логина нет в списке, передаём результат
                }
                else
                {
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
            WindowState = WindowState.Minimized;
        }

        private void MainWindow1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void isDis_Checked(object sender, RoutedEventArgs e)
        { //красим кнопочки
            isDis.Foreground = gr;
            isDis.BorderBrush = gr;
            isCon.Foreground = bl;
            isCon.BorderBrush = bl;
        }

        private void isCon_Checked(object sender, RoutedEventArgs e)
        {
            read_names(); //читаем таблицу names и красим кнопочки
            if (iConnect.State == ConnectionState.Open)
            {
                isDis.Foreground = bl;
                isDis.BorderBrush = bl;
                isCon.Foreground = gr;
                isCon.BorderBrush = gr;
            }
            else
            {
                isCon.IsChecked = false;
                isDis.IsChecked = true;
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            read_names(); //читаем заново таблицу names и перелогиниваемся
            MainWindow1.Hide();
            login();
            MainWindow1.Show();
        }

        private void DialogHost_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            Debug.WriteLine($"SAMPLE 1: Closing dialog with parameter: {eventArgs.Parameter ?? string.Empty}");
            Debug.WriteLine(FruitTextBox.Text);
        }

        private void DialogHost_DialogClosed(object sender, DialogClosedEventArgs eventArgs)
        {
            Debug.WriteLine($"SAMPLE 1: Closed dialog with parameter: {eventArgs.Parameter ?? string.Empty}");
            Debug.WriteLine(FruitTextBox.Text);
        }

        private void Sample2_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            Debug.WriteLine($"SAMPLE 2: Closing dialog with parameter: {eventArgs.Parameter ?? string.Empty}");
        }

        private void Sample2_DialogHost_OnDialogClosed(object sender, DialogClosedEventArgs eventArgs)
        {
            Debug.WriteLine($"SAMPLE 2: Closed dialog with parameter: {eventArgs.Parameter ?? string.Empty}");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DetailWindow w = new();
            w.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (Connect())
            {
                popup.IsTopDrawerOpen = false;
            }
        }

        private void Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            if (e.Source.GetType() == typeof(ComboBox))
            {
                lost(e.Source, e);
                e.Handled = true;
            }
            if (e.Source.GetType() == typeof(TextBox))
            {
                lost(e.Source, e, 2);
                e.Handled = true;
            }
        }

        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {
            if (e.Source.GetType() == typeof(ComboBox))
            {
                got(e.Source, e);
                e.Handled = true;
            }
            if (e.Source.GetType() == typeof(TextBox))
            {
                got(e.Source, e, 2);
                e.Handled = true;
            }
        }

        public void verify(ComboBox sender, KeyEventArgs e)
        {
            if (sender.Items.IndexOf(sender.Text) == -1)
            {
                HintAssist.SetHelperText(sender, "Неверное значение");
                sender.Foreground = rd;
                sender.BorderBrush = rd;
            }
            else
            {
                sender.Foreground = stand;
                sender.BorderBrush = stand;
            }
        }

        private void lost(object sender, RoutedEventArgs e, int i = 1)
        {
            switch (i)
            {
                case 1:
                    ComboBox s = (ComboBox)sender;
                    if ((s.Text == "") && (s.Name == "combo_pribors"))
                    {
                        HintAssist.SetHelperText(s, "Поле не может быть пустым");
                        s.Foreground = rd;
                        s.BorderBrush = rd;
                    }
                    else
                    {
                        s.Foreground = stand;
                        s.BorderBrush = stand;
                    }
                    break;
                case 2:
                    TextBox t = (TextBox)sender;
                    if ((t.Text == "") && (t.Name == "text_num"))
                    {
                        HintAssist.SetHelperText(t, "Поле не может быть пустым");
                        t.Foreground = rd;
                        t.BorderBrush = rd;
                    }
                    else
                    {
                        t.Foreground = stand;
                        t.BorderBrush = stand;
                    }
                    break;
            }
        }

        private void got(object sender, RoutedEventArgs e, int i = 1)
        {
            switch (i)
            {
                case 1:
                    ComboBox s = (ComboBox)sender;
                    HintAssist.SetHelperText(s, "");
                    s.Foreground = stand;
                    s.BorderBrush = gr;
                    break;
                case 2:
                    TextBox t = (TextBox)sender;
                    HintAssist.SetHelperText(t, "");
                    t.Foreground = stand;
                    t.BorderBrush = gr;
                    break;
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.Source.GetType());
            if (e.Source.GetType() == typeof(ComboBox))
            {
                verify((ComboBox)e.Source, e);
            }
        }
    }
}