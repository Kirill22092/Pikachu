namespace Pikachu
{
    using BCrypt.Net;
    using MaterialDesignThemes.Wpf;
    using Npgsql;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
#pragma warning disable CS1591
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
            if (login()) //вызываем окно логина
            {
                after_login();
            }
            ListView_pribors.ItemsSource = db.pribors;
        }
        ///<summary>Метод вызывающий окно логина.</summary>
        ///<returns>Возвращает результат работы вызванного диалогового окна.</returns>
        public bool login()
        {
            Window1 log1 = new(this);
            bool? res = log1.ShowDialog();
            return res != null && (bool)res;
        }

        ///<summary>Метод проверки данных для авторизции.</summary>
        ///<param name="Login">Логин</param>
        ///<param name="Password">Пароль</param>
        ///<returns>Возвращает bool[0] = флаг проверки логина, bool[1] = флаг проверки пароля, 
        ///bool[2] = флаг проверки соединения.</returns>
        public bool[] loginDialogCheck(string Login, string Password)
        {
            lock (locker_N)
            {
                bool[] result = { false, false, true };
                int i = db.CheckName(Login); //поиск введенного логина в списке имён
                if (i > -1)
                {
                    result[0] = true; //логин нашёлся
                    if (BCrypt.Verify(Password, db.GetPass(Login))) //проверка пароля
                    {
                        result[1] = true; //пароль совпал
                        login_text.Text = Login; //пишем имя пользователя в главном окне
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
        }

        private void MainWindow1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            read_names(); //читаем заново таблицу names и перелогиниваемся
            MainWindow1.Hide();
            _ = login();
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
            /*
            Делаем запрос к БД на длинну архива прибора, чья кнопка нажата...
            ...и инициируем новый экземпляр класса archive с длинной в качестве параметра

            Делаем запрос на получение архивов даты, имени, статуса, примечания

            Записываем полученные массивы в поля класса archive

            Создаем новый экземпляр класса DetailWindow и пердаем в качестве параметров:
            экземпляр класса archive и списки names_key/title, status_key/title
             */
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Cursor c = ((Button)e.Source).Cursor;
            ((Button)e.Source).Cursor = Cursors.Wait;
            if (Connect())
            {
                popup.IsTopDrawerOpen = false;
            }
            ((Button)e.Source).Cursor = c;
        }

        private void Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            if (e.Source.GetType() == typeof(ComboBox))
            {
                lost(e.Source, e);
            }
            if (e.Source.GetType() == typeof(TextBox))
            {
                lost(e.Source, e, 2);
            }
            if (e.Source.GetType() == typeof(DatePicker))
            {
                lost(e.Source, e, 3);
            }
        }

        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {
            if (e.Source.GetType() == typeof(ComboBox))
            {
                got(e.Source, e);
            }
            if (e.Source.GetType() == typeof(TextBox))
            {
                got(e.Source, e, 2);
            }
            if (e.Source.GetType() == typeof(DatePicker))
            {
                got(e.Source, e, 3);
            }
        }

        public void verify(object sender, KeyEventArgs e, int i = 1)
        {
            switch (i)
            {
                case 1:
                    {
                        ComboBox c = (ComboBox)sender;
                        if (c.Items.IndexOf(c.Text) == -1)
                        {
                            c.Style = ComboBoxNotValidVerify;
                        }
                        else
                        {
                            c.Style = ComboBoxValid;
                        }
                        break;
                    }
                case 2:
                    {
                        DatePicker d = (DatePicker)sender;
                        Debug.WriteLine(d.SelectedDate);
                        if (d.SelectedDate > DateTime.Now.Date)
                        {
                            d.Style = DatePickerNotValidVerify;
                        }
                        else
                        {
                            d.Style = DatePickerValid;
                        }
                        break;
                    }
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
                        s.Style = ComboBoxNotValidEmpty;
                    }
                    else
                    {
                        s.Style = ComboBoxValid;
                    }
                    break;
                case 2:
                    TextBox t = (TextBox)sender;
                    if ((t.Text == "") && (t.Name == "text_num"))
                    {
                        t.Style = TextBoxNotValidEmpty;
                    }
                    else
                    {
                        t.Style = TextBoxValid;
                    }
                    break;
                case 3:
                    DatePicker d = (DatePicker)sender;
                    if (d.Text == "")
                    {
                        d.Style = DatePickerNotValidEmpty;
                    }
                    else
                    {
                        d.Style = DatePickerValid;
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
                    s.Style = ComboBoxValid;
                    break;
                case 2:
                    TextBox t = (TextBox)sender;
                    t.Style = TextBoxValid;
                    break;
                case 3:
                    DatePicker d = (DatePicker)sender;
                    d.Style = DatePickerValid;
                    break;
            }
        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Source.GetType() == typeof(ComboBox))
            {
                verify(e.Source, e);
            }
            if ((e.Source.GetType() == typeof(DatePicker)) && (e.Key == Key.Return))
            {
                verify(e.Source, e, 2);
            }
        }

        private void test_click(object sender, RoutedEventArgs e)
        {
            //Тут творится какая то хуйня... я хз что, как и зачем... Просто копипаста из инета...
            var item = VisualTreeHelper.HitTest(ListView_pribors, Mouse.GetPosition(ListView_pribors)).VisualHit;
            while (item != null && !(item is ListBoxItem))
                item = VisualTreeHelper.GetParent(item);
            int i=-1;
            if (item != null)
            {
                i = ListView_pribors.Items.IndexOf(((ListBoxItem)item).DataContext);
            }

            //А тут творится полная хуйня... 
            List<string> pr = db.GetPribor(i).GetIndex();            
            lock (locker)
            {
                int le = -1;
                string sql = $"SELECT array_length(arr_date,1) FROM archive WHERE pribor_tip={pr[0]} AND pribor_num={pr[1]} AND pribor_exp={pr[2]};";
                iQuery = new(sql, iConnect);
                NpgsqlDataReader reader = iQuery.ExecuteReader();
                if (iConnect.State == ConnectionState.Open)
                {
                    while (reader.Read())
                    {
                        le = reader.GetInt32(0);
                    }
                }
                reader.Close();
                iQuery.Dispose();

                sql = $"SELECT * FROM archive WHERE pribor_tip={pr[0]} AND pribor_num={pr[1]} AND pribor_exp={pr[2]};";
                iQuery = new(sql, iConnect);
                reader = iQuery.ExecuteReader();
                if (iConnect.State == ConnectionState.Open)
                {
                    int[] status = new int[le];
                    int[] name = new int[le];
                    string[] note = new string[le];
                    DateTime[] date = new DateTime[le];
                    while (reader.Read())
                    {
                        date = reader.GetFieldValue<DateTime[]>(3);
                        status = reader.GetFieldValue<int[]>(4);
                        name = reader.GetFieldValue<int[]>(5);
                        note = reader.GetFieldValue<string[]>(6);
                    }
                    reader.Close();
                    iQuery.Dispose();

                    DetailWindow detail = new(db.GetArchive(pr, date, name, status, note, le));
                    detail.Show();
                }
            }
        }

        private void ThemeChange(object sender, RoutedEventArgs e)
        {
            SetTheme(false);
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Minimized(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}