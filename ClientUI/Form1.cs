using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using CryptoLibrary;
using Newtonsoft.Json;
using System.Threading;
using Microsoft.VisualBasic;

namespace ClientUI
{
	public partial class MainMenu : Form
	{
		//предопределение адреса, клиента и ip адреса для корректной работы всех модулей
		public static Address Address;
		public static Network.TCPClient Client = new Network.TCPClient();
		public static IPAddress IPAddress = IPAddress.Loopback;
		public static string secret = "";
		private static string received = "";

		public MainMenu()
		{
			InitializeComponent();
		}

		//нажатие на кнопку логина - открывается новое окно логина
		private async void LoginButtonClick(object sender, EventArgs e)
		{
			Network.TCPCommands command = new Network.TCPCommands(Network.Commands.Check, codeWordBox.Text);
			string reply = "";

			try
			{
				string serialized = JsonConvert.SerializeObject(command);
				//асинхронная отправка команды
				reply = await Client.SendData(serialized);

				Address = JsonConvert.DeserializeObject<Address>(reply);

				MessageBox.Show("На ваш номер поступило сообщение с шестизначным кодом\nДля продолжения введите его в поле ниже");

				Form auth = new AuthenticationForm();
				auth.ShowDialog();

				Form logged = new LoggedForm();
				logged.ShowDialog();

				if (logged.DialogResult == DialogResult.Retry)
				{
					LoginButtonClick(this, e);
				}

			}
			catch (Exception)
			{
				MessageBox.Show(reply);
			}

		}

		//нажатие на кнопку регистрации - открывается новое окно регистрации
		private void RegisterButtonClick(object sender, EventArgs e)
		{
			Form registerForm = new RegisterForm();

			registerForm.ShowDialog();

			if (registerForm.DialogResult == DialogResult.OK)
			{
				codeWordBox.Text = secret;
			}
		}
	}
}
