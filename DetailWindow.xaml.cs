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
      //  private MainWindow.archive archive;

       /* public DetailWindow(MainWindow.archive a, List<string> n, List<int> nk, List<string> s, List<int> sk)
        {
            InitializeComponent();
        }*/

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
