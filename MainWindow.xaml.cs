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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;
using MaterialDesignThemes.Wpf;
using System.Diagnostics;
using System.Data;

namespace Pikachu
{
	using BCrypt.Net;
	using MaterialDesignColors;
	using System.Threading;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Brush gr = new SolidColorBrush(Color.FromRgb(174, 234, 0));
		Brush rd = new SolidColorBrush(Color.FromRgb(255, 98, 80));
		Brush bl = new SolidColorBrush(Color.FromRgb(0, 0, 0));
		bool isOpen = false;
		int iLogin = -1;
		List<int> names_key = new();
		List<string> names_title = new();
		List<string> names_rights = new();
		List<string> names_pass = new();
		List<pribors> pr = new();
		NpgsqlCommand? iQuery;
		public NpgsqlConnection iConnect = new($"Server ={Properties.Settings.Default.na};" + //создаем строку подключения к БД из парамеров приложения
			$"Port={Properties.Settings.Default.np};User Id={Properties.Settings.Default.lg};" +
			$"Password={Properties.Settings.Default.ps};Database={Properties.Settings.Default.db};");
		public MainWindow()
		{
			InitializeComponent();
			Conn_init(iConnect); //вызываем метод подключения к БД и чтения таблицы names
			login(0, ""); //вызываем окно логина
			pr.Add(new pribors
			{
				pribor_num="12345",
				pribor_tip="SGOES",
				pribor_mat="Алюминий",
				pribor_gaz="Метан",
				pribor_exp="USA",
				pribor_range="0-100",
				last_date="20-20-20",
				last_status="Выпущен",
				last_name="Родионов"
			});
			lvDataBinding.ItemsSource = pr;
		}
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
		public async Task  Conn_init(NpgsqlConnection iConnect) //соединение с БД, обработка ошибок, обработка изменения состояния соединения
		{
		iConnect.StateChange += (object sender, StateChangeEventArgs e) =>
		{
			if (!(e.CurrentState == ConnectionState.Open))
			{
				isOpen = false;
				isDis.IsChecked = true;
				isDis_i.Foreground = rd;
				isDis.IsChecked = true;
			}
			else
			{
				isOpen = true;
				isDis.IsChecked = false;
				isDis_i.Foreground = bl;
				isCon.IsChecked = true;
			}
		};
		try
			{
				iConnect.Open(); //открываем соеднение с БД
				string sql = "SELECT * FROM names;";
				iQuery = new(sql, iConnect); //читаем из БД таблицу пользователей...
				var reader = await iQuery.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					names_key.Add(reader.GetInt32(0)); //...и заносим полученные данные в списки
					names_title.Add(reader.GetString(1));
					names_rights.Add(reader.GetString(2));
					names_pass.Add(reader.GetString(3));
				}
				await reader.CloseAsync();
				iQuery.Dispose();
								}
		catch (NpgsqlException e)
			{
				if (e.Message.Contains("28P01"))
				{
					MessageBox.Show("Неверный логин/пароль БД");
					isOpen = false;
				}
				else if (e.Message.Contains("3D000"))
				{
					MessageBox.Show("Неверное имя базы данных");
					isOpen = false;
				}
				else
				{
					MessageBox.Show(e.Message);
					isOpen = false;
				}
			} 
		}
		public void login(int b, string l)
		{
			Window1 log1 = new(); //создаем экземляр окна
			switch (b) //меняем цвет и подсказки в текстбоксах, если есть входящие параметры
			{
				case 1:
					HintAssist.SetHelperText(log1.PassBox, "Неверный пароль");
					log1.PassBox.Foreground = rd;
					log1.PassBox.BorderBrush = rd;
					log1.LoginBox.Text = l;
					break;

				case 2:
					HintAssist.SetHelperText(log1.LoginBox, "Неверный логин");
					log1.LoginBox.Foreground = rd;
					log1.LoginBox.BorderBrush = rd;
					log1.LoginBox.Text = l;
					break;

                case 3:
                    log1.Auth.Text=log1.Auth.Text+" невозможна\nнет подключения\nк базе данных";
					log1.Auth.Foreground = rd;
                    break;
            }
			if (log1.ShowDialog() == true) //вызов окна логина и проверка возвращенного результата
			{
				int i = names_title.FindIndex(p => p == log1.Login); //поиск введенного логина в списке имён
				if (i > -1)
				{
					if (BCrypt.Verify(log1.Password, names_pass[i])) //проверка пароля
					{
						iLogin = names_key[i];
					}
					else
					{
						iLogin = -1;
						login(1, log1.Login); //пароль не подошёл, вызываем метод login ещё раз
					}
				}
				else
				{
					if (isOpen)
					{
                        iLogin = -1;
                        login(2, log1.Login); //логина нет в списке, вызываем метод ещё раз
                    }
					else
					{
                        iLogin = -1;
                        login(3, log1.Login);
                    }
				}
			}
			else
			{
				Application.Current.Shutdown(); //окно логина было закрыто, закрываем программу
			}
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}
		private void MainWindow1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}
		private void isDis_Checked(object sender, RoutedEventArgs e)
		{
			iConnect.Close();
		}
		private void isCon_Checked(object sender, RoutedEventArgs e)
		{
			Conn_init(iConnect);
		}
	}
}
