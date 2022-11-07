using MaterialDesignThemes.Wpf;
using Npgsql;
using System;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using ThreadState = System.Threading.ThreadState;

namespace Pikachu
{
    public partial class MainWindow : Window
    {
        public bool Connect()
        {
            try
            {
                iConnect.Open();
                return true;
            }
            catch (NpgsqlException e)
            {
                if (e.Message.Contains("28P01"))
                {
                    _ = MessageBox.Show("Неверный логин/пароль БД");
                }
                else if (e.Message.Contains("3D000"))
                {
                    _ = MessageBox.Show("Неверное имя базы данных");
                }
                else
                {
                    _ = e.Message == "Exception while writing to stream" ? MessageBox.Show("Прервано подключение к БД") : MessageBox.Show(e.Message);

                }
                return false;
            }
        }

        public void read_names() //Чтение таблицы names
        {
            if (Th_N.ThreadState is not ThreadState.Running and not ThreadState.WaitSleepJoin)
            {
                Th_N = new Thread(() =>
                {
                    lock (locker)
                    {
                        if (iConnect.State == ConnectionState.Open)
                        {
                            string sql = "SELECT * FROM names;";
                            iQuery = new(sql, iConnect); //читаем из БД таблицу пользователей...
                            NpgsqlDataReader reader = iQuery.ExecuteReader();
                            if (iConnect.State == ConnectionState.Open)
                            {
                                lock (locker_N)
                                {
                                    names_key.Clear();
                                    names_title.Clear();
                                    names_rights.Clear();
                                    names_pass.Clear();
                                    while (reader.Read())
                                    {
                                        names_key.Add(reader.GetInt32(0));
                                        names_title.Add(reader.GetString(1));
                                        names_rights.Add(reader.GetString(2));
                                        names_pass.Add(reader.GetString(3)); //...и заносим полученные данные в списки
                                    }
                                }
                            }
                            reader.Close();
                            iQuery.Dispose();
                        }
                    }
                });
                Th_N.Start();
            }
        }
        public void read_others(string Table) //Чтение таблицы names
        {
            Thread Th_O = new(() =>
            {
                lock (locker)
                {
                    if (iConnect.State == ConnectionState.Open)
                    {
                        string sql = $"SELECT * FROM {Table};";
                        iQuery = new(sql, iConnect); //читаем из БД таблицу...
                        NpgsqlDataReader reader = iQuery.ExecuteReader();
                        if (iConnect.State == ConnectionState.Open)
                        {
                            lock (locker_O)
                            {
                                switch (Table)
                                {
                                    case "pribor":
                                        {
                                            pribor_title.Clear();
                                            pribor_key.Clear();
                                            while (reader.Read())
                                            {
                                                pribor_key.Add(reader.GetInt32(0));
                                                pribor_title.Add(reader.GetString(1));//...и заносим полученные данные в списки
                                            }
                                            break;
                                        }
                                    case "material":
                                        {
                                            material_key.Clear();
                                            material_title.Clear();
                                            while (reader.Read())
                                            {
                                                material_key.Add(reader.GetInt32(0));
                                                material_title.Add(reader.GetString(1));//...и заносим полученные данные в списки
                                            }
                                            break;
                                        }
                                    case "gaz":
                                        {
                                            gaz_key.Clear();
                                            gaz_title.Clear();
                                            while (reader.Read())
                                            {
                                                gaz_key.Add(reader.GetInt32(0));
                                                gaz_title.Add(reader.GetString(1));//...и заносим полученные данные в списки
                                            }
                                            break;
                                        }
                                    case "sensor":
                                        {
                                            sensor_key.Clear();
                                            sensor_title.Clear();
                                            while (reader.Read())
                                            {
                                                sensor_key.Add(reader.GetInt32(0));
                                                sensor_title.Add(reader.GetString(1));//...и заносим полученные данные в списки
                                            }
                                            break;
                                        }
                                    case "range":
                                        {
                                            range_key.Clear();
                                            range_title.Clear();
                                            while (reader.Read())
                                            {
                                                range_key.Add(reader.GetInt32(0));
                                                range_title.Add(reader.GetString(1));//...и заносим полученные данные в списки
                                            }
                                            break;
                                        }
                                    case "status":
                                        {
                                            status_key.Clear();
                                            status_title.Clear();
                                            while (reader.Read())
                                            {
                                                status_key.Add(reader.GetInt32(0));
                                                status_title.Add(reader.GetString(1));//...и заносим полученные данные в списки
                                            }
                                            break;
                                        }
                                    case "modify":
                                        {
                                            modify_key.Clear();
                                            modify_title.Clear();
                                            while (reader.Read())
                                            {
                                                modify_key.Add(reader.GetInt32(0));
                                                modify_title.Add(reader.GetString(1));//...и заносим полученные данные в списки
                                            }
                                            break;
                                        }

                                }
                            }
                        }
                        reader.Close();
                        iQuery.Dispose();
                    }
                }
            });
            Th_O.Start();
        }
        private void before_login()
        {
            iConnect.StateChange += (object sender, StateChangeEventArgs e) =>          //Создаем обработчик события и метод
            {
                Debug.WriteLine(e.CurrentState.ToString());                             //на изменение состояния подключения к БД
                if (!(e.CurrentState == ConnectionState.Open))
                {
                    _ = Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ConStat.Foreground = rd;
                        ConStat.Content = new PackIcon { Kind = PackIconKind.LanDisconnect };
                        popup.IsTopDrawerOpen = true;
                    }));
                }
                else
                {
                    _ = Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ConStat.Foreground = gr;
                        ConStat.Content = new PackIcon { Kind = PackIconKind.LanConnect };
                    }));
                }
                Debug.WriteLine(iConnect.State.ToString());
            };

            _ = Connect(); //открываем соеднение с БД
            read_names(); // чтение таблицы names
        }

        private void after_login()
        {
            read_others("pribor");
            read_others("material");
            read_others("modify");
            read_others("gaz");
            read_others("range");
            read_others("sensor");
            read_others("status");
            date_snu.SelectedDate = DateTime.Now.Date;
            date_snu.DisplayDateEnd = DateTime.Now.Date;
            date_oki.SelectedDate = DateTime.Now.Date;
            date_oki.DisplayDateEnd = DateTime.Now.Date;
            date_ktx.SelectedDate = DateTime.Now.Date;
            date_ktx.DisplayDateEnd = DateTime.Now.Date;
            date_out.SelectedDate = DateTime.Now.Date;
            date_out.DisplayDateEnd = DateTime.Now.Date;
            lock (locker_O)
            {
                pribor_title.RemoveAt(0);
                pribor_key.RemoveAt(0);
                material_key.RemoveAt(0);
                material_title.RemoveAt(0);
                modify_key.RemoveAt(0);
                modify_title.RemoveAt(0);
                gaz_key.RemoveAt(0);
                gaz_title.RemoveAt(0);
                range_key.RemoveAt(0);
                range_title.RemoveAt(0);
                sensor_key.RemoveAt(0);
                sensor_title.RemoveAt(0);
                status_key.RemoveAt(0);
                status_title.RemoveAt(0);
                combo_pribors.ItemsSource = pribor_title;
                combo_gaz.ItemsSource = gaz_title;
                combo_materials.ItemsSource = material_title;
                combo_modify.ItemsSource = modify_title;
                combo_range.ItemsSource = range_title;
            }
        }

        public void read_pribors() //Чтение таблицы main
        {
                
        }
    }
}
