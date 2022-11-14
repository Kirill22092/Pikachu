using MaterialDesignThemes.Wpf;
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
        internal readonly Style TextBoxValid = (Style)Application.Current.FindResource("MaterialDesignFloatingHintTextBox");
        internal readonly Style TextBoxNotValidEmpty = (Style)Application.Current.FindResource("EmptyNotValidTextBox");
        internal readonly Style TextBoxNotValidCheck = (Style)Application.Current.FindResource("CheckNotValidTextBox");
        internal readonly Style PassBoxValid = (Style)Application.Current.FindResource("MaterialDesignFloatingHintPasswordBox");
        internal readonly Style PassBoxNotValidEmpty = (Style)Application.Current.FindResource("EmptyNotValidPassBox");
        internal readonly Style PassBoxNotValidCheck = (Style)Application.Current.FindResource("CheckNotValidPassBox");
        internal readonly Style ComboBoxValid = (Style)Application.Current.FindResource("MaterialDesignFilledComboBox");
        internal readonly Style ComboBoxNotValidEmpty = (Style)Application.Current.FindResource("EmptyNotValidComboBox");
        internal readonly Style ComboBoxNotValidVerify = (Style)Application.Current.FindResource("VerifyNotValidComboBox");
        internal readonly Style DatePickerValid = (Style)Application.Current.FindResource("MaterialDesignFilledDatePicker");
        internal readonly Style DatePickerNotValidEmpty = (Style)Application.Current.FindResource("EmptyNotValidDatePicker");
        internal readonly Style DatePickerNotValidVerify = (Style)Application.Current.FindResource("VerifyNotValidDatePicker");
        internal readonly Style LabelConnected = (Style)Application.Current.FindResource("LabelConnected");
        internal readonly Style LabelDisconnected = (Style)Application.Current.FindResource("LabelDisconnected");
        internal readonly Style LabelStandart = (Style)Application.Current.FindResource("LabelStandart");
        internal readonly Style LabelOk = (Style)Application.Current.FindResource("LabelOk");
        internal readonly Style LabelError = (Style)Application.Current.FindResource("LabelError");
        private Thread Th_N = new(() => { });
        private object locker = new();
        private object locker_DB = new();
        private bool DarkTheme;
        private object locker_P = new();
        private DB_Data db = new();

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
            /// <summary>
            /// Список приборов
            /// </summary>
            public List<pribor> pribors = new();

            /// <summary>
            /// Класс для сохранения данных из таблицы main
            /// Каждый экземпляр класса может содержать все поля одного прибора
            /// </summary>
            public class pribor
            {
#pragma warning disable CS8618
                /// <summary>Тип прибора в числовом представлении</summary>
                string tip;
                /// <summary>Номер прибора в числовом представлении</summary>
                string num;
                /// <summary>Исполнение прибора в числовом представлении</summary>
                string exp;
                /// <summary>Модификация прибора в числовом представлении</summary>
                string mod;
#pragma warning restore CS8618
                /// <summary>
                /// Устанавливает числовые представления полей pribor_tip/num/exp/mod
                /// </summary>
                /// <param name="a">Поле pribor_tip</param>
                /// <param name="b">Поле pribor_num</param>
                /// <param name="c">Поле pribor_exp</param>
                /// <param name="d">Поле pribor_mod</param>
               /* public void SetIndex(string a, string b, string c, string d)
                {
                    tip = a;
                    num = b;
                    exp = c;
                    mod = d;
                }
                /// <summary>
                /// Возвращает список индексов pribor_tip/num/exp/mod
                /// </summary>
                /// <returns></returns>
                public List<string> GetIndex()
                {
                    List<string> l = new List<string> { tip, num, exp, mod };
                    return l;
                }*/
                /// <summary>
                /// Тип прибора (соответсвует столбцу title таблицы pribor)
                /// </summary>
                public string? pribor_tip { get; set; }
                /// <summary>
                /// Модификация прибора (соответсвует столбцу title таблицы modify)
                /// </summary>
                public string? pribor_mod { get; set; }
                /// <summary>
                /// Номер прибора
                /// </summary>
                public string? pribor_num { get; set; }
                /// <summary>
                /// Материал корпуса прибора (соответсвует столбцу title таблицы material)
                /// </summary>
                public string? pribor_mat { get; set; }
                /// <summary>
                /// 0 - прибор для РФ, 1 - для США, 2 - для Индии
                /// </summary>
                public string? pribor_exp { get; set; }
                /// <summary>
                /// Новый прибор или поступил в ремонт
                /// </summary>
                public string? pribor_rem { get; set; }
                /// <summary>
                /// Если прибор ремонтный - номер МСК
                /// </summary>
                public string? msk_num { get; set; }
                /// <summary>
                /// Газ прибора (соответсвует столбцу title таблицы gaz)
                /// </summary>
                public string? pribor_gaz { get; set; }
                /// <summary>
                /// Диапазон измерений прибора (соответсвует столбцу title таблицы range)
                /// </summary>
                public string? pribor_range { get; set; }
                /// <summary>
                /// Тип сенсора прибора (соответсвует столбцу title таблицы sensor)
                /// </summary>
                public string? pribor_sens { get; set; }
                /// <summary>
                /// Дата последней операции с прибором
                /// </summary>
                public string? last_date { get; set; }
                /// <summary>
                /// Фамилия сотрудника совершившего последнюю операцию (соответсвует столбцу title таблицы names)
                /// </summary>
                public string? last_name { get; set; }
                /// <summary>
                /// Статус последней операции с прибором
                /// </summary>
                public string? last_status { get; set; }
                /// <summary>
                /// Примечание к последней операции с прибором
                /// </summary>
                public string? last_note { get; set; }
                /// <summary>
                /// Дата последней операции на СНУ
                /// </summary>
                public string? date_snu { get; set; }
                /// <summary>
                /// Дата последней операции в КТХ
                /// </summary>
                public string? date_ktx { get; set; }
                /// <summary>
                /// Дата последней операции в ОКИ
                /// </summary>
                public string? date_oki { get; set; }
                /// <summary>
                /// Дата последней операции на УУиО
                /// </summary>
                public string? date_out { get; set; }
                /// <summary>
                /// Фамилия сотрудника на СНУ
                /// </summary>
                public string? name_snu { get; set; }
                /// <summary>
                /// Фамилия сотрудника на КТХ
                /// </summary>
                public string? name_ktx { get; set; }
                /// <summary>
                /// Фамилия сотрудника в ОКИ
                /// </summary>
                public string? name_oki { get; set; }
                /// <summary>
                /// Фамилия сотрудника на УУиО
                /// </summary>
                public string? name_out { get; set; }
                /// <summary>
                /// Организация куда отгружен прибор
                /// </summary>
                public string? name_zak { get; set; }
            }
            /// <summary>
            /// Класс для сохранения данных из таблицы archive
            /// Каждый экземпляр класса может содержать архив одного прибора.
            /// </summary>
            public class archive
            {
                /// <summary>
                /// Тип прибора (соответсвует столбцу title таблицы pribor)
                /// </summary>
#pragma warning disable CS8618
                public string pribor_tip { get; set; }
                /// <summary>
                /// Модификация прибора (соответсвует столбцу title таблицы modify)
                /// </summary>
                public string pribor_mod { get; set; }
                /// <summary>
                /// Номер прибора
                /// </summary>
                public string pribor_num { get; set; }
                /// <summary>
                /// 0 - прибор для РФ, 1 - для США, 2 - для Индии
                /// </summary>
                public string pribor_exp { get; set; }
                /// <summary>
                /// Список дат
                /// </summary>
                public DateTime date { get; set; }
                /// <summary>
                /// Список статусов
                /// </summary>
                public string status { get; set; }
                /// <summary>
                /// Список Имён
                /// </summary>
                public string name { get; set; }
                /// <summary>
                /// Список примечаний
                /// </summary>
                public string note { get; set; }
#pragma warning restore CS8618 

            }

            ///<summary>Запись данных в класс</summary>
            ///<param name="l">Список ключей</param>
            ///<param name="name">Имя таблицы из которой считаны данные</param>
            ///<returns>Возвращает false при ошибке</returns>
            public void SetData(List<int> l, string name)
            {
                if (l == null) throw new DB_DataException("Переданный список пуст");
                if (name == null) throw new DB_DataException("Имя таблицы не может быть null");
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
                    default:
                        throw new DB_DataException("Неверное имя таблицы", name);
                }
            }

            ///<summary>Запись данных в класс</summary>
            ///<param name="l">Список заголовков</param>
            ///<param name="name">Имя таблицы из которой считаны данные</param> 
            ///<param name="i">Только для таблицы names. 0 - запись заголовков (можно не указывать),
            ///1 - запись прав доступа, 2 - запись паролей</param>
            ///<returns>Возвращает false при ошибке</returns>
            public void SetData(List<string> l, string name, int i = 0)
            {
                if (l == null) throw new DB_DataException("Переданный список пуст");
                if (name == null) throw new DB_DataException("Имя таблицы не может быть null");
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
                    default:
                        throw new DB_DataException("Неверное имя таблицы", name);
                }
            }

            ///<summary>Получение данных в виде списка. Данные таблицы names в этом методе не доступны.</summary>
            ///<param name="name">Имя таблицы данные которой нужно получить</param>
            ///<returns>Возвращает List&lt;string&gt;.</returns>
            public List<string> GetData(string name)
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
                        throw new DB_DataException("Неверное имя таблицы", name);
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
            public string GetName(string index)
            {
                if (index == null) throw new DB_DataException("Параметр index не может быть null");
                if (int.Parse(index) == -1) return " ";
                return names_title[names_key.FindIndex(p => p == int.Parse(index))];
            }
            ///<summary>Возвращает имя по индексу БД</summary>
            public string GetName(int? index)
            {
                if (index == null) throw new DB_DataException("Параметр index не может быть null");
                if (index == -1) return " ";
                return names_title[names_key.FindIndex(p => p == index)];
            }

            ///<returns>Возвращает строку с паролем при совпадении. Возвращеет пустую строку при ошибке</returns>
            public string GetPass(string name)
            {
                int i = CheckName(name);
                if (i == -1)
                {
                    return "";
                }
                else
                {
                    return names_pass[i];
                }
            }

            ///<returns>Возвращает строку с правами доступа при совпадении. Возвращеет пустую строку при ошибке</returns>
            public string GetRights(string name)
            {
                int i = CheckName(name);
                if (i == -1)
                {
                    return "";
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
            public int FindKey(string table, string name)
            {
                if (table == null) throw new DB_DataException("Имя таблицы не может быть null");
                if (name == null) throw new DB_DataException("Поисковая строка не может быть null");
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
                        throw new DB_DataException("Неверное имя таблицы", table);
                }
            }

            ///<summary>Поиск заголовка по индексу</summary>
            ///<param name="table">Где ищем</param>
            ///<param name="name">Что ищем</param>
            ///<returns>Возвращает заголовок, пустой если индекс не найден</returns>
            public string FindTitle(string table, string name)
            {
                if (table == null) throw new DB_DataException("Имя таблицы не может быть null");
                if (name == null) throw new DB_DataException("Поисковая строка не может быть null");
                int i = int.Parse(name);
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
                        throw new DB_DataException("Неверное имя таблицы", table);
                }
            }
            ///<summary>Поиск заголовка по индексу</summary>
            ///<param name="table">Где ищем</param>
            ///<param name="i">Индекс того, что ищем</param>
            ///<returns>Возвращает заголовок, пустой если индекс не найден</returns>
            public string FindTitle(string table, int? i)
            {
                if (table == null) throw new DB_DataException("Имя таблицы не может быть null");
                if (i == null) throw new DB_DataException("Поисковая строка не может быть null");
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
                        throw new DB_DataException("Неверное имя таблицы", table);
                }
            }
            ///<summary>Запись прибора в класс. Заменяет индексы вспомогательных таблиц на соответствующие заголовки.</summary>
            ///<param name="l">List&lt;string&gt; полученный после чтения из БД</param>
            public void SetPribor(List<string> l)
            {
                if (l == null) throw new DB_DataException("Список не может быть null");
                pribor p = Pribor(l);
                pribors.Add(p);
            }
            ///<summary>Конвертация list&lt;string&gt; в pribor</summary>
            ///<param name="l">List&lt;string&gt; полученный после чтения из БД</param>
            ///<returns>Возвращает заполненный экземпляр pribor</returns>
            public pribor Pribor(List<string> l)
            {
                if (l == null) throw new DB_DataException("Список не может быть null");
                pribor p = new()
                {
                    pribor_tip = FindTitle("pribor", l[0]),
                    pribor_num = l[1],
                    pribor_mat = FindTitle("material", l[2]),
                    pribor_exp = exp[int.Parse(l[3])],
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
                //p.SetIndex(l[0], l[1], l[3], l[22]);
                return p;
            }
            /// <summary>
            /// Возвращает экземпляр класса преобразованную в список запись из таблицы archive
            /// </summary>
            /// <param name="pribor">List&lt;string&gt; из 4 элементов: pribor_tip, pribor_num, pribor_exp, pribor_mod в СТРОГОМ порядке</param>
            /// <param name="date">Массив дат</param>
            /// <param name="name">Массив индексов имен</param>
            /// <param name="status">Массив индексов статусов</param>
            /// <param name="note">Массив примечаний</param>
            /// <param name="count">Длинна архива</param>
            /// <returns></returns>
            public List<archive> GetArchive(List<string> pribor, DateTime[] date, int[] name, int[] status, string[] note, int count)
            {
                List<archive> l = new();
                for (int i = 0; i < count; i++)
                {
                    archive a = new()
                    {
                        pribor_tip = FindTitle("pribor", pribor[0]),
                        pribor_num = pribor[1],
                        pribor_exp = exp[int.Parse(pribor[2])],
                        pribor_mod = FindTitle("modify", pribor[3]),
                        date = date[i],
                        name = GetName(name[i]),
                        note = note[i],
                        status = FindTitle("status", status[i]),
                    };
                    l.Add(a);
                }

                return l;
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
                if (index < 0) throw new DB_DataException("Индекс не может быть отрицательным");
                return (pribors[index]);
            }

            ///<summary>Очистка сохраненных в классе приборов</summary>
            public void Clear()
            {
                pribors.Clear();
            }
            public List<string> GetIndex(int i)
            {
                List<string> l = new();// { tip, num, exp, mod };
                l.Add(FindKey("pribor", pribors[i].pribor_tip).ToString());
                l.Add(pribors[i].pribor_num);
                l.Add(exp.FindIndex(p => p == pribors[i].pribor_exp).ToString());
                l.Add(FindKey("modify", pribors[i].pribor_mod).ToString());
                return l;
            }
        }
        /// <summary>
        /// Класс иисключений для класса DB_Data
        /// </summary>
        public class DB_DataException : ArgumentException
        {
            /// <summary>
            /// Конструктор по умолчанию
            /// </summary>
            /// <param name="message">Текст сообщения об ошибке</param>
            /// <param name="value">Параметр вызвавший ошибку</param>
            public DB_DataException(string message, string value) : base(message, value)
            { }
            /// <summary>
            /// Конструктор по умолчанию
            /// </summary>
            /// <param name="message">Текст сообщения об ошибке</param>
            public DB_DataException(string message) : base(message)
            { }
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
        }
    }
}