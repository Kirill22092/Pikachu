using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        internal readonly Brush stand;
        private List<int> names_key = new();
        private List<string> names_title = new();
        private List<string> names_rights = new();
        private List<string> names_pass = new();
        private List<pribors> pr = new();
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
    }
}
