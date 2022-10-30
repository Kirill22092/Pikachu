using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Pikachu
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            for (int i = 1; i <= e.Args.Length; i++)
            {
                switch (e.Args[i - 1].Substring(0, 2))
                {
                    case "na":
                        Pikachu.Properties.Settings.Default.na = e.Args[i - 1].Substring(3, e.Args[i - 1].Length - 3);
                        break;

                    case "np":
                        Pikachu.Properties.Settings.Default.np = e.Args[i - 1].Substring(3, e.Args[i - 1].Length - 3);
                        break;

                    case "lg":
                        Pikachu.Properties.Settings.Default.lg = e.Args[i - 1].Substring(3, e.Args[i - 1].Length - 3);
                        break;

                    case "ps":
                        Pikachu.Properties.Settings.Default.ps = e.Args[i - 1].Substring(3, e.Args[i - 1].Length - 3);
                        break;

                    case "db":
                        Pikachu.Properties.Settings.Default.db = e.Args[i - 1].Substring(3, e.Args[i - 1].Length - 3);
                        break;

                    default:
                        MessageBox.Show("Неизвестный параметр \n\n" + e.Args[i - 1]);
                        break;
                }
            }
        }
        void Application_Exit(object sender, ExitEventArgs e)
        {
            Pikachu.Properties.Settings.Default.Save();
        }

    }
}
