using MaterialDesignThemes.Wpf;
using System.Windows;

namespace Pikachu
{

    public partial class Window1 : Window
    {
        private MainWindow mainWindow;

        public Window1(MainWindow window)
        {
            InitializeComponent();
            mainWindow = window;
            LoginIcon.Foreground = LoginBox.BorderBrush;
            Passicon.Foreground = PassBox.BorderBrush;
            LoginBox.Foreground = LoginBox.BorderBrush;
            PassBox.Foreground = PassBox.BorderBrush;
#if DEBUG
            LoginBox.Text = "XXX";
            PassBox.Password = "2";
#endif
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool[] result; //создаем массив
            result = mainWindow.loginDialogCheck(LoginBox.Text, PassBox.Password); //получаем результаты проверки из главного окна
            if (result != null)
            {
                if (!result[0]) //если неверный логин
                {
                    HintAssist.SetHelperText(LoginBox, "Неверный логин");
                    LoginBox.Foreground = mainWindow.rd;
                    LoginBox.BorderBrush = mainWindow.rd;
                    LoginIcon.Foreground = mainWindow.rd;
                }
                if (!result[1]) //если неверный пароль
                {
                    HintAssist.SetHelperText(PassBox, "Неверный пароль");
                    PassBox.Foreground = mainWindow.rd;
                    PassBox.BorderBrush = mainWindow.rd;
                    Passicon.Foreground = mainWindow.rd;
                }
                AuthFailConnect.Visibility = !result[2] ? Visibility.Visible : Visibility.Collapsed;
                if (result[0] && result[1] && result[2]) //если всё правильно, диалоговое окно закрывается, управление переходит главному окну
                {
                    DialogResult = true;
                }
            }
        }

        private void LoginBox_GotFocus(object sender, RoutedEventArgs e)
        {
            LoginIcon.Foreground = mainWindow.gr;
            HintAssist.SetHelperText(LoginBox, "");
            LoginBox.Foreground = mainWindow.stand;
            LoginBox.BorderBrush = mainWindow.gr;
        }

        private void LoginBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "")
            {
                HintAssist.SetHelperText(LoginBox, "Поле не может быть пустым");
                LoginBox.Foreground = mainWindow.rd;
                LoginBox.BorderBrush = mainWindow.rd;
            }
            else
            {
                LoginBox.Foreground = mainWindow.stand;
                LoginBox.BorderBrush = mainWindow.stand;
            }
            LoginIcon.Foreground = LoginBox.BorderBrush;
        }

        private void PassBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PassBox.Password == "")
            {
                HintAssist.SetHelperText(PassBox, "Поле не может быть пустым");
                PassBox.Foreground = mainWindow.rd;
                PassBox.BorderBrush = mainWindow.rd;
            }
            else
            {
                PassBox.Foreground = mainWindow.stand;
                PassBox.BorderBrush = mainWindow.stand;
            }
            Passicon.Foreground = PassBox.BorderBrush;
        }

        private void PassBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Passicon.Foreground = mainWindow.gr;
            HintAssist.SetHelperText(PassBox, "");
            PassBox.Foreground = mainWindow.stand;
            PassBox.BorderBrush = mainWindow.gr;
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