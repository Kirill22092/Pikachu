using System.Windows;

namespace Pikachu
{

    public partial class Window1 : Window
    {
        private readonly MainWindow mainWindow;

        ///<summary>Класс окна логина.</summary>
        ///<param name="window">Экземпляр главного окна, в метод которого должен быть переданы данные для авторизации.</param>
        public Window1(MainWindow window)
        {
            InitializeComponent();
            mainWindow = window;
#if DEBUG
            LoginBox.Text = "1";
            PassBox.Password = "1";
#endif
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PassBox.Password) || string.IsNullOrWhiteSpace(LoginBox.Text)) { return; }
            bool[] result = mainWindow.loginDialogCheck(LoginBox.Text, PassBox.Password); //получаем результаты проверки из главного окна
            if (result != null)
            {
                if (!result[0]) //если неверный логин
                {
                    LoginBox.Style = mainWindow.TextBoxNotValidCheck;
                    LoginIcon.Style = mainWindow.LabelError;
                }
                if (!result[1]) //если неверный пароль
                {
                    PassBox.Style = mainWindow.PassBoxNotValidCheck;
                    Passicon.Style = mainWindow.LabelError;
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
            LoginIcon.Style = mainWindow.LabelOk;
        }

        private void LoginBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginBox.Text))
            {
                LoginBox.Style = mainWindow.TextBoxNotValidEmpty;
                LoginIcon.Style = mainWindow.LabelError;
            }
            else
            {
                LoginBox.Style = mainWindow.TextBoxValid;
                LoginIcon.Style = mainWindow.LabelStandart;
            }
        }

        private void PassBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PassBox.Password))
            {
                PassBox.Style = mainWindow.PassBoxNotValidEmpty;
                Passicon.Style = mainWindow.LabelError;
            }
            else
            {
                PassBox.Style = mainWindow.PassBoxValid;
                Passicon.Style = mainWindow.LabelStandart;
            }
        }

        private void PassBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PassBox.Style = mainWindow.PassBoxValid;
            Passicon.Style = mainWindow.LabelOk;
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ThemeChange(object sender, RoutedEventArgs e)
        {
            mainWindow.SetTheme(false);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}