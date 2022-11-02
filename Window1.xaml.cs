#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.

using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Media;

namespace Pikachu
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        MainWindow mainWindow;
        private readonly Brush stand;
        readonly Brush rd = new SolidColorBrush(Color.FromRgb(255, 98, 80));
        readonly Brush gr = new SolidColorBrush(Color.FromRgb(76, 175, 80));

        public Window1()

        {
            InitializeComponent();
            LoginIcon.Foreground = LoginBox.BorderBrush;
            Passicon.Foreground = PassBox.BorderBrush;
            LoginBox.Foreground = LoginBox.BorderBrush;
            PassBox.Foreground = PassBox.BorderBrush;
            stand = PassBox.BorderBrush;
#if DEBUG
            LoginBox.Text = "XXX";
            PassBox.Password = "2";
#endif
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool[] result; //создаем массив
            result = mainWindow.loginDialogCheck(Login, Password); //получаем результаты проверки из главного окна
            if (result != null)
            {
                if (result[0] == false) //если неверный логин
                {
                    HintAssist.SetHelperText(LoginBox, "Неверный логин");
                    LoginBox.Foreground = rd;
                    LoginBox.BorderBrush = rd;
                }
                if (result[1] == false) //если неверный пароль
                {
                    HintAssist.SetHelperText(PassBox, "Неверный пароль");
                    PassBox.Foreground = rd;
                    PassBox.BorderBrush = rd;
                }
                if (result[2] == false) //если нет соединения
                {
                    AuthFailConnect.Visibility = Visibility.Visible;
                }
                else AuthFailConnect.Visibility = Visibility.Collapsed;
                if (result[0] && result[1] && result[2]) //если всё правильно, диалоговое окно закрывается, управление переходит главному окну
                {
                    DialogResult = true;
                }
            }
        }
        public string Password
        {
            get { return PassBox.Password; }
        }
        public string Login
        {
            get { return LoginBox.Text; }
        }
        private void LoginBox_GotFocus(object sender, RoutedEventArgs e)
        {
            LoginIcon.Foreground = gr;
            HintAssist.SetHelperText(LoginBox, "");
            LoginBox.Foreground = stand;
            LoginBox.BorderBrush = gr;
        }

        private void LoginBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "")
            {
                HintAssist.SetHelperText(LoginBox, "Поле не может быть пустым");
                LoginBox.Foreground = rd;
                LoginBox.BorderBrush = rd;
            }
            else
            {
                LoginBox.Foreground = stand;
                LoginBox.BorderBrush = stand;
            }
            LoginIcon.Foreground = LoginBox.BorderBrush;
        }

        private void PassBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PassBox.Password == "")
            {
                HintAssist.SetHelperText(PassBox, "Поле не может быть пустым");
                PassBox.Foreground = rd;
                PassBox.BorderBrush = rd;
            }
            else
            {
                PassBox.Foreground = stand;
                PassBox.BorderBrush = stand;
            }
            Passicon.Foreground = PassBox.BorderBrush;
        }

        public void ShowDialogs(MainWindow some)
        {
            mainWindow = some; //Получаем экземпляр главного окна и присваеваем его к глобальной переменной
            ShowDialog(); //Показываем данное окно как диалоговое
        }

        private void PassBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Passicon.Foreground = gr;
            HintAssist.SetHelperText(PassBox, "");
            PassBox.Foreground = stand;
            PassBox.BorderBrush = gr;
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}