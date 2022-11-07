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
#if DEBUG
            LoginBox.Text = "1";
            PassBox.Password = "1";
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
                    PassBox.Style = mainWindow.TextBoxNotValidCheck;
                }
                if (!result[1]) //если неверный пароль
                {
                    PassBox.Style = mainWindow.PassBoxNotValidCheck;
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
            LoginBox.Style = mainWindow.TextBoxValid;
            LoginIcon.Foreground = LoginBox.BorderBrush;
        }

        private void LoginBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "")
            {
                LoginBox.Style = mainWindow.TextBoxNotValidEmpty;
            }
            else
            {
                LoginBox.Style = mainWindow.TextBoxValid;
            }
            LoginIcon.Foreground = LoginBox.BorderBrush;
        }

        private void PassBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PassBox.Password == "")
            {
                PassBox.Style = mainWindow.PassBoxNotValidEmpty;
            }
            else
            {
                PassBox.Style = mainWindow.PassBoxValid;
            }
            Passicon.Foreground = PassBox.BorderBrush;

        }

        private void PassBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PassBox.Style = mainWindow.PassBoxValid;
            Passicon.Foreground = PassBox.BorderBrush;
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