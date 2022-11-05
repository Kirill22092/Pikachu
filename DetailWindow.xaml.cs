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
using System.Windows.Shapes;

namespace Pikachu
{
    /// <summary>
    /// Логика взаимодействия для DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        private MainWindow.archive archive;
        private List<string> names_title;
        private List<int> names_key;
        private List<string> strings_title;
        private List<int> strings_key;
        public DetailWindow(MainWindow.archive a, List<string> n, List<int> nk, List<string> s, List<int> sk)
        {
            InitializeComponent();
            archive = a;
            names_title = n;
            names_key = nk;
            strings_title = s;
            strings_key = sk;
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
