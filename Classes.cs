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
        private List<pribors> pr = new();
        private Thread Th_N = new(() => { });
        private object locker = new();
        private object locker_O = new();
        private object locker_N = new();

        public class pribors
        {
            public string? pribor_num { get; set; }
            public string? pribor_tip { get; set; }
            public string? pribor_mod { get; set; }
            public string? pribor_mat { get; set; }
            public string? pribor_gaz { get; set; }
            public string? pribor_exp { get; set; }
            public string? pribor_range { get; set; }
            public string? last_date { get; set; }
            public string? last_status { get; set; }
            public string? last_name { get; set; }
        }

        public class archive
        {
            public DateOnly[] date { get; set; }
            public int[] status { get; set; }
            public int[] name { get; set; }
            public string[] note { get; set; }
            public archive(int count)
            {
                date = new DateOnly[count];
                status = new int[count];
                name = new int[count];
                note = new string[count];
            }
        }
        private void paint()
        {
            combo_pribors.Foreground = combo_pribors.BorderBrush; //красим элементы
            
        }
    }
}