using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

#pragma warning disable CS1591
namespace Pikachu
{
    /// <summary>
    /// Логика взаимодействия для DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        public DetailWindow(List<MainWindow.DB_Data.archive> a)
        {
            InitializeComponent();
            ListView_archive.ItemsSource = a;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DialogWindow1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
