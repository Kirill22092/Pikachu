using MaterialDesignThemes.Wpf;
using System;
using System.CodeDom;
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
        internal Brush bl = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        internal Brush gr = (Brush)Application.Current.FindResource("PrimaryHueMidBrush");
        internal Brush rd = (Brush)Application.Current.FindResource("SecondaryHueMidBrush");
        internal Brush stand = (Brush)Application.Current.FindResource("MaterialDesignTextBoxBorder");
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
        private List<string> exp = new() { "РФ", "НЕ РФ", "Точно не РФ" };
        private Thread Th_N = new(() => { });
        private object locker = new();
        private object locker_O = new();
        private object locker_N = new();
        private bool DarkTheme;
        private object locker_P = new();

        ///<summary>Класс для хранения и обработки полученных из базы данных</summary>
        public class DB_Data
        {
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
            private List<string> exp = new() { "РФ", "НЕ РФ", "Точно не РФ" };
            private List<pribor> pribors = new();

            public class pribor
            {
                public string? pribor_tip { get; set; }
                public string? pribor_mod { get; set; }
                public string? pribor_num { get; set; }
                public string? pribor_mat { get; set; }
                public string? pribor_exp { get; set; }
                public string? pribor_rem { get; set; }
                public string? msk_num { get; set; }
                public string? pribor_gaz { get; set; }
                public string? pribor_range { get; set; }
                public string? pribor_sens { get; set; }
                public string? last_date { get; set; }
                public string? last_name { get; set; }
                public string? last_status { get; set; }
                public string? last_note { get; set; }
                public string? date_snu { get; set; }
                public string? date_ktx { get; set; }
                public string? date_oki { get; set; }
                public string? date_out { get; set; }
                public string? name_snu { get; set; }
                public string? name_ktx { get; set; }
                public string? name_oki { get; set; }
                public string? name_out { get; set; }
                public string? name_zak { get; set; }
            }

            ///<summary>Запись данных в класс</summary>
            ///<param name="l">Список ключей</param>
            ///<param name="name">Имя таблицы из которой считаны данные</param>
            ///<returns>Возвращает false при ошибке</returns>
            public bool SetData(List<int> l, string name)
            {
                if ((l == null) || (name == null)) return false;
                l.RemoveAt(0);
                switch (name)
                {
                    case "pribor":
                        pribor_key.Clear();
                        pribor_key = l;
                        break;
                    case "material":
                        material_key.Clear();
                        material_key = l;
                        break;
                    case "modify":
                        modify_key.Clear();
                        modify_key = l;
                        break;
                    case "gaz":
                        gaz_key.Clear();
                        gaz_key = l;
                        break;
                    case "range":
                        range_key.Clear();
                        range_key = l;
                        break;
                    case "sensor":
                        sensor_key.Clear();
                        sensor_key = l;
                        break;
                    case "status":
                        status_key.Clear();
                        status_key = l;
                        break;
                    case "names":
                        names_key.Clear();
                        names_key = l;
                        break;
                    default: return false;
                }
                return true;
            }

            ///<summary>Запись данных в класс</summary>
            ///<param name="l">Список заголовков</param>
            ///<param name="name">Имя таблицы из которой считаны данные</param>            
            ///<returns>Возвращает false при ошибке</returns>
            public bool SetData(List<string> l, string name, int i = 0)
            {
                if ((l == null) || (name == null)) return false;
                l.RemoveAt(0);
                switch (name)
                {
                    case "pribor":
                        pribor_title.Clear();
                        pribor_title = l;
                        break;
                    case "material":
                        material_title.Clear();
                        material_title = l;
                        break;
                    case "modify":
                        modify_title.Clear();
                        modify_title = l;
                        break;
                    case "gaz":
                        gaz_title.Clear();
                        gaz_title = l;
                        break;
                    case "range":
                        range_title.Clear();
                        range_title = l;
                        break;
                    case "sensor":
                        sensor_title.Clear();
                        sensor_title = l;
                        break;
                    case "status":
                        status_title.Clear();
                        status_title = l;
                        break;
                    case "names":
                        if (i == 1)
                        {
                            names_rights.Clear();
                            names_rights = l;
                            break;
                        }
                        else if (i == 2)
                        {
                            names_pass.Clear();
                            names_pass = l;
                            break;
                        }
                        else
                        {
                            names_title.Clear();
                            names_title = l;
                            break;
                        }
                    default: return false;
                }
                return true;
            }

            ///<summary>Получение данных в виде списка. Данные таблицы names в этом методе не доступны.</summary>
            ///<param name="name">Имя таблицы данные которой нужно получить</param>
            ///<returns>Возвращает List&lt;string&gt;. При ошибке в name возвращает null</returns>
            public List<string>? GetData(string name)
            {
                switch (name)
                {
                    case "pribor":
                        return pribor_title;
                    case "material":
                        return material_title;
                    case "modify":
                        return modify_title;
                    case "gaz":
                        return gaz_title;
                    case "range":
                        return range_title;
                    case "sensor":
                        return sensor_title;
                    case "status":
                        return status_title;
                    default:
                        return null;
                }
            }

            ///<summary>Проверяет есть ли такое имя.</summary>
            ///<returns>Возвращает -1 при ошибке.</returns>
            public int CheckName(string name)
            {
                if (name == null) return -1;
                int i = names_title.FindIndex(p => p == name);
                return i;
            }

            ///<summary>Возвращает имя по индексу БД</summary>
            ///<returns>Возвращает null при ошибке.</returns>
            public string? GetName(string index)
            {
                if (index == null) return null;
                if (Int32.Parse(index) == -1) return " ";
                return names_title[Int32.Parse(index)];
            }

            ///<returns>Возвращает строку с паролем при совпадении. Возвращеет null при ошибке</returns>
            public string? GetPass(string name)
            {
                int i = CheckName(name);
                if (i == -1)
                {
                    return null;
                }
                else
                {
                    return names_pass[i];
                }
            }

            ///<returns>Возвращает строку с правами доступа при совпадении. Возвращеет null при ошибке</returns>
            public string? GetRights(string name)
            {
                int i = CheckName(name);
                if (i == -1)
                {
                    return null;
                }
                else
                {
                    return names_rights[i];
                }
            }

            ///<summary>Поиск индекса</summary>
            ///<param name="table">Где ищем</param>
            ///<param name="name">Что ищем</param>
            ///<returns>Возвращает индекс в БД, -1 если прибор не найден, null при ошибке</returns>
            public int? FindKey(string table, string name)
            {
                if ((table == null) || (name == null)) return null;
                int i;
                switch (table)
                {
                    case "pribor":
                        i = pribor_title.FindIndex(p => p == name);
                        if (i == -1) return -1;
                        return pribor_key[i];
                    case "material":
                        i = material_title.FindIndex(p => p == name);
                        if (i == -1) return -1;
                        return material_key[i];
                    case "modify":
                        i = modify_title.FindIndex(p => p == name);
                        if (i == -1) return -1;
                        return modify_key[i];
                    case "gaz":
                        i = gaz_title.FindIndex(p => p == name);
                        if (i == -1) return -1;
                        return gaz_key[i];
                    case "range":
                        i = range_title.FindIndex(p => p == name);
                        if (i == -1) return -1;
                        return range_key[i];
                    case "sensor":
                        i = sensor_title.FindIndex(p => p == name);
                        if (i == -1) return -1;
                        return sensor_key[i];
                    case "status":
                        i = status_title.FindIndex(p => p == name);
                        if (i == -1) return -1;
                        return status_key[i];
                    default:
                        return null;
                }
            }

            ///<summary>Поиск заголовка по индексу</summary>
            ///<param name="table">Где ищем</param>
            ///<param name="name">Что ищем</param>
            ///<returns>Возвращает заголовок, пустой если индекс не найден, null при ошибке</returns>
            public string? FindTitle(string table, string name)
            {
                if ((table == null) || (name == null)) return null;
                int i = Int32.Parse(name);
                switch (table)
                {
                    case "pribor":
                        return pribor_title[pribor_key.FindIndex(p => p == i)];
                    case "material":
                        if (i == -1) return "";
                        return material_title[material_key.FindIndex(p => p == i)];
                    case "modify":
                        if (i == -1) return "";
                        return modify_title[modify_key.FindIndex(p => p == i)];
                    case "gaz":
                        if (i == -1) return "";
                        return gaz_title[gaz_key.FindIndex(p => p == i)];
                    case "range":
                        if (i == -1) return "";
                        return range_title[range_key.FindIndex(p => p == i)];
                    case "sensor":
                        if (i == -1) return "";
                        return sensor_title[sensor_key.FindIndex(p => p == i)];
                    case "status":
                        if (i == -1) return "";
                        return status_title[status_key.FindIndex(p => p == i)];
                    default:
                        return null;
                }
            }
            ///<summary>Запись прибора в класс. Заменяет индексы вспомогательных таблиц на соответствующие заголовки.</summary>
            ///<param name="l">List&lt;string&gt; полученный после чтения из БД</param>
            ///<returns>Возвращает true, при ошибке возвращает false</returns>
            public bool SetPribor(List<string> l)
            {
#pragma warning disable CS8604
                if (l == null) return false;
                pribor? p = Pribor(l);
                if (p == null) return false;
                pribors.Add(p);
                return true;
#pragma warning restore CS8604
            }
            ///<summary>Конвертация list&lt;string&gt; в pribor</summary>
            ///<param name="l">List&lt;string&gt; полученный после чтения из БД</param>
            ///<returns>Возвращает pribor, при ошибке возвращает null</returns>
            public pribor? Pribor(List<string> l)
            {
                if (l == null) return null;
                pribor p = new()
                {
                    pribor_tip = pribor_title[Int32.Parse(l[0])],
                    pribor_num = l[1],
                    pribor_mat = FindTitle("material", l[2]),
                    pribor_exp = exp[Int32.Parse(l[3])],
                    pribor_rem = l[4],
                    msk_num = l[5],
                    pribor_gaz = FindTitle("gaz", l[6]),
                    pribor_range = FindTitle("range", l[7]),
                    pribor_sens = FindTitle("sensor", l[8]),
                    last_date = l[9],
                    last_name = GetName(l[10]),
                    last_status = FindTitle("status", l[11]),
                    last_note = l[12],
                    date_snu = l[13],
                    date_ktx = l[14],
                    date_oki = l[15],
                    date_out = l[16],
                    name_snu = GetName(l[17]),
                    name_ktx = GetName(l[18]),
                    name_oki = GetName(l[19]),
                    name_out = GetName(l[20]),
                    name_zak = l[21],
                    pribor_mod = FindTitle("modify", l[22])
                };
                return p;
            }

            ///<summary>Получение приборов из класса</summary>
            ///<returns>Возвращает список всех приборов</returns>
            public List<pribor> GetPribors()
            {
                return pribors;
            }

            ///<summary>Получение прибора из класса</summary>
            ///<param name="index">Индекс прибора</param>
            ///<returns>Возвращает один прибор</returns>
            public pribor GetPribor(int index)
            {
                return (pribors[index]);
            }

            ///<summary>Очистка сохраненных в классе приборов</summary>
            public void Clear()
            {
                pribors.Clear();
            }
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
            DarkTheme = Properties.Settings.Default.Dark;
            SetTheme(true);
        }

        ///<summary>Класс изменения темы или применения сохраненных параметров</summary>
        ///<param name="Start">Индекс прибора</param>
        public void SetTheme(bool Start)
        {
            if (!Start)
            {
                DarkTheme = !DarkTheme;
            }
            PaletteHelper paletteHelper = new();
            ITheme theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(DarkTheme ? Theme.Dark : Theme.Light);
            if (DarkTheme) theme.Paper = Color.FromRgb(40, 42, 52);
            paletteHelper.SetTheme(theme);
            Properties.Settings.Default.Dark = DarkTheme;
            Properties.Settings.Default.Save();
            gr = (Brush)Application.Current.FindResource("PrimaryHueMidBrush");
            rd = (Brush)Application.Current.FindResource("SecondaryHueMidBrush");
            stand = (Brush)Application.Current.FindResource("MaterialDesignTextBoxBorder");
        }
    }
}