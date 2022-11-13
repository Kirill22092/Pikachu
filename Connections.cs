using MaterialDesignThemes.Wpf;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using ThreadState = System.Threading.ThreadState;
#pragma warning disable CS1591
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
                Thread Th = new Thread(() =>
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
                                List<int> names_key = new();
                                List<String> names_title = new();
                                List<string> names_rights = new();
                                List<string> names_pass = new();
                                while (reader.Read())
                                {
                                    names_key.Add(reader.GetInt32(0));
                                    names_title.Add(reader.GetString(1));
                                    names_rights.Add(reader.GetString(2));
                                    names_pass.Add(reader.GetString(3)); //...и заносим полученные данные в списки
                                }
                                lock (locker_DB)
                                {
                                    db.SetData(names_key, "names");
                                    db.SetData(names_title, "names");
                                    db.SetData(names_rights, "names", 1);
                                    db.SetData(names_pass, "names", 2);
                                }
                                reader.Close();
                                iQuery.Dispose();
                            }
                        }
                    }
                });
                Th.Start();
        }
        public void read_others(string Table) //Чтение таблицы names
        {
            Thread Th = new(() =>
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
                            List<int> key = new();
                            List<string> title = new();
                            lock (locker_DB)
                            {
                                while (reader.Read())
                                {
                                    key.Add(reader.GetInt32(0));
                                    title.Add(reader.GetString(1));//...и заносим полученные данные в списки
                                }
                                db.SetData(key,Table);
                                db.SetData(title, Table);
                            }
                        }
                        reader.Close();
                        iQuery.Dispose();
                    }
                }
            });
            Th.Start();
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
                        ConStat.Style = LabelDisconnected;
                        popup.IsTopDrawerOpen = true;
                    }));
                }
                else
                {
                    _ = Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ConStat.Style = LabelConnected;
                    }));
                }
                Debug.WriteLine(iConnect.State.ToString());
            };

            _= Connect(); //открываем соеднение с БД
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
            lock (locker_DB)
            {
                combo_pribors.ItemsSource = db.GetData("pribor");
                combo_gaz.ItemsSource = db.GetData("gaz");
                combo_materials.ItemsSource = db.GetData("material");
                combo_modify.ItemsSource = db.GetData("modify");
                combo_range.ItemsSource = db.GetData("range");
                combo_status.ItemsSource = db.GetData("status");
                combo_sensor.ItemsSource = db.GetData("sensor");
            }
        }

        public void read_pribors(string sql) //Чтение таблицы main
        {
            Thread Th = new(() =>
            {
                lock (locker)
                {
                    if (iConnect.State == ConnectionState.Open)
                    {
                        if (sql == "") { sql = "SELECT * FROM main ORDER BY RANDOM() LIMIT 20;"; }
                        iQuery = new(sql, iConnect); 
                        NpgsqlDataReader reader = iQuery.ExecuteReader();
                        if (iConnect.State == ConnectionState.Open)
                        {
                            lock (locker_DB)
                            {
                                while (reader.Read())
                                {
                                    List<string> pribor = new();
                                    pribor.Add(reader.GetInt32(0).ToString());
                                    pribor.Add(reader.GetInt32(1).ToString());
                                    pribor.Add(reader.GetInt32(2).ToString());
                                    pribor.Add(reader.GetInt32(3).ToString());
                                    pribor.Add(reader.GetBoolean(4).ToString());
                                    pribor.Add(reader.GetInt32(5).ToString());
                                    pribor.Add(reader.GetInt32(6).ToString());
                                    pribor.Add(reader.GetInt32(7).ToString());
                                    pribor.Add(reader.GetInt32(8).ToString());
                                    pribor.Add(reader.GetDateTime(9).ToString());
                                    pribor.Add(reader.GetInt32(10).ToString());
                                    pribor.Add(reader.GetInt32(11).ToString());
                                    pribor.Add(reader.GetString(12));
                                    if (reader.IsDBNull(13)) { pribor.Add(""); } else { pribor.Add(reader.GetDateTime(13).ToString()); }
                                    if (reader.IsDBNull(14)) { pribor.Add(""); } else { pribor.Add(reader.GetDateTime(14).ToString()); }
                                    if (reader.IsDBNull(15)) { pribor.Add(""); } else { pribor.Add(reader.GetDateTime(15).ToString()); }
                                    if (reader.IsDBNull(16)) { pribor.Add(""); } else { pribor.Add(reader.GetDateTime(16).ToString()); }                                   
                                    pribor.Add(reader.GetInt32(17).ToString());
                                    pribor.Add(reader.GetInt32(18).ToString());
                                    pribor.Add(reader.GetInt32(19).ToString());
                                    pribor.Add(reader.GetInt32(20).ToString());
                                    pribor.Add(reader.GetString(21));
                                    pribor.Add(reader.GetInt32(22).ToString());
                                    db.SetPribor(pribor);
                                }
                            }
                        }
                        reader.Close();
                        iQuery.Dispose();
                    }
                }
            });
            Th.Start();
        }
    }
}
