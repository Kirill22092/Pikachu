using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace Pikachu
{
    // Объявления классов, глобальных переменных и т.п.
    public partial class MainWindow : Window
    {
#pragma warning disable IDE0044 // Добавить модификатор только для чтения
        internal readonly Brush bl = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        internal readonly Brush gr = (Brush) Application.Current.FindResource("PrimaryHueMidBrush");
        internal readonly Brush rd = (Brush) Application.Current.FindResource("SecondaryHueMidBrush");
        internal readonly Brush stand = (Brush) Application.Current.FindResource("MaterialDesignTextBoxBorder");
        private List<int> names_key = new();
        private List<string> names_title = new();
        private List<string> names_rights = new();
        private List<string> names_pass = new();
        private List<int> pribor_key = new();
        private List<string> pribor_title = new();
        private List<int> material_key = new();
        private List<string> material_title = new();
        private List<int> modify_key = new();
        private List<string> modify_title = new();
        private List<int> gaz_key = new();
        private List<string> gaz_title = new();
        private List<int> range_key = new();
        private List<string> range_title = new();
        private List<int> sensor_key = new();
        private List<string> sensor_title = new();
        private List<int> status_key = new();
        private List<string> status_title = new();
        private List<string> exp = new() { "РФ", "НЕ РФ", "Точно не РФ"};
        private List<pribors> pr = new();
        private Thread Th_N = new(() => { });
        private object locker = new();
        private object locker_O = new();
        private object locker_N = new();
        private object locker_P = new();

        public class pribors
        {
            public string? pribor_num { get; set; }
            public string? pribor_tip { get; set; }
            public string? pribor_mod { get; set; }
            public string? pribor_mat { get; set; }
            public string? pribor_gaz { get; set; }
            public string? pribor_exp { get; set; }
            public string? pribor_range { get; set; }
            public DateTime? last_date { get; set; }
            public string? last_status { get; set; }
            public string? last_name { get; set; }
        }

        public class archive
        {
            public DateTime[] date { get; set; }
            public int[] status { get; set; }
            public int[] name { get; set; }
            public string[] note { get; set; }
            public archive(int count)
            {
                date = new DateTime[count];
                status = new int[count];
                name = new int[count];
                note = new string[count];
            }
        }
        private void paint() //красим элементы
        {
            combo_pribors.Foreground = combo_pribors.BorderBrush;
        }
    }
}