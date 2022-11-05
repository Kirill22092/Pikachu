using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace Pikachu
{
    // Объявления классов, глобальных переменных и тп
    public partial class MainWindow : Window
    {
        internal readonly Brush gr = new SolidColorBrush(Color.FromRgb(76, 175, 80));
        internal readonly Brush rd = new SolidColorBrush(Color.FromRgb(255, 98, 80));
        internal readonly Brush bl = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        internal readonly Brush stand = new SolidColorBrush(Color.FromRgb(197, 197, 197));
#pragma warning disable IDE0044 // Добавить модификатор только для чтения
        private List<int> names_key = new();
        private List<string> names_title = new();
        private List<string> names_rights = new();
        private List<string> names_pass = new();
        private List<pribors> pr = new();
        private Thread My = new(() => { });
        private object locker = new();

        public class pribors
        {
            public string? pribor_num { get; set; }
            public string? pribor_tip { get; set; }
            public string? pribor_mat { get; set; }
            public string? pribor_gaz { get; set; }
            public string? pribor_exp { get; set; }
            public string? pribor_range { get; set; }
            public string? last_date { get; set; }
            public string? last_status { get; set; }
            public string? last_name { get; set; }
        }

        private void before_login()
        {
            iConnect.StateChange += (object sender, StateChangeEventArgs e) =>          //Создаем обработчик события и метод
            {
                Debug.WriteLine(e.CurrentState.ToString());                             //на изменение состояния подключения к БД
                if (!(e.CurrentState == ConnectionState.Open))
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        isDis_i.Foreground = rd;
                        isCon.IsChecked = false;
                        popup.IsTopDrawerOpen = true;
                    }));
                }
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        isDis_i.Foreground = bl;
                        isCon.IsChecked = true;
                    }));
                }
                Debug.WriteLine(iConnect.State.ToString());
            };

            Connect(); //открываем соеднение с БД
            read_names(); // чтение таблицы names
        }
        private void paint()
        {
            combo_pribors.Foreground = combo_pribors.BorderBrush; //красим элементы
        }
    }
}