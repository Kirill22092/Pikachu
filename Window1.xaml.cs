using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
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
using System.Windows.Shapes;

namespace Pikachu
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private readonly Brush stand;
        public Window1()
        {
            InitializeComponent();
            LoginIcon.Foreground = LoginBox.BorderBrush;
            Passicon.Foreground = PassBox.BorderBrush;
            LoginBox.Foreground = LoginBox.BorderBrush;
            PassBox.Foreground = PassBox.BorderBrush;
            stand = PassBox.BorderBrush;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
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
            LoginIcon.Foreground = (Brush)LoginIcon.FindResource("PrimaryHueMidBrush");
            HintAssist.SetHelperText(LoginBox, "");
            LoginBox.Foreground = stand;
            LoginBox.BorderBrush = (Brush)LoginBox.FindResource("PrimaryHueMidBrush");
        }

        private void LoginBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "")
            {
                HintAssist.SetHelperText(LoginBox, "Поле не может быть пустым");
                LoginBox.Foreground = new SolidColorBrush(Color.FromRgb(255, 98, 80));
                LoginBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 98, 80));

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
                PassBox.Foreground = new SolidColorBrush(Color.FromRgb(255, 98, 80));
                PassBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 98, 80));

            }
            else
            {
                PassBox.Foreground = stand;
                PassBox.BorderBrush = stand;
            }
            Passicon.Foreground = PassBox.BorderBrush;
        }

        private void PassBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Passicon.Foreground = (Brush)Passicon.FindResource("PrimaryHueMidBrush");
            HintAssist.SetHelperText(PassBox, "");
            PassBox.Foreground = stand;
            PassBox.BorderBrush = (Brush)PassBox.FindResource("PrimaryHueMidBrush");
        }
    }
}
