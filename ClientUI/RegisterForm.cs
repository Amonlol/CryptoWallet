using CryptoLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;
using System.Threading;
using System.Text.RegularExpressions;

namespace ClientUI
{
	/// <summary>
	/// Окно регистрации
	/// </summary>
	public partial class RegisterForm : Form
	{
		private bool proceed = false;
		private string secret;

		public RegisterForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// TO DO: actual realization
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ProceedButtonClick(object sender, EventArgs e)
		{
			if (proceed)
			{
				//инициализация нового адреса с полученной кодовой фразой
				//MainMenu.Address = new Address(secret);
				MainMenu.secret = secret;
				this.DialogResult = DialogResult.OK;
			}
			this.Dispose();
		}
		/// <summary>
		/// Асинхронный метод который посылает команду генерации нового адреса на сервер
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void GenerateButtonClick(object sender, EventArgs e)
		{
			PhoneNumberBox.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;

			if (ValidateInformation())
			{
				PhoneNumberBox.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
				char split = ';';

				StringBuilder sb = new StringBuilder();
				sb.Append(PhoneNumberBox.Text).Append(split).Append(EmailBox.Text).Append(split).Append(PasswordBox1.Text);
				
				Network.TCPCommands Command = new Network.TCPCommands(Network.Commands.Generate, sb.ToString());

				try
				{
					string serialized = JsonConvert.SerializeObject(Command);
					//асинхронная отправка команды

					secret = await MainMenu.Client.SendData(serialized);
					//асинхронное получение ответа и вывод на окно

					secret = secret.Replace("\0", String.Empty);
					codeWordBox.Text = secret;

					proceed = true;
				}
				catch (Exception)
				{
					MessageBox.Show("Произошла ошибка при подключении к серверу!");
				}
			}
		}

		private bool ValidateInformation()
		{

			if (String.IsNullOrWhiteSpace(PhoneNumberBox.Text) || PhoneNumberBox.Text.Length < 10)
			{
				MessageBox.Show("Введите номер телефона!");
				return false;
			}
			else if (String.IsNullOrWhiteSpace(EmailBox.Text))
			{
				MessageBox.Show("Введите свою почту!");
				return false;
			}
			else if (String.IsNullOrWhiteSpace(PasswordBox1.Text) || String.IsNullOrWhiteSpace(PasswordBox2.Text))
			{
				MessageBox.Show("Пароль не может быть пустым!");
				return false;
			}

			try
			{
				var mail = new System.Net.Mail.MailAddress(EmailBox.Text);
			}
			catch (Exception)
			{
				MessageBox.Show("Введенного адреса почты не существует!");
				return false;
			}

			if (!String.Equals(PasswordBox1.Text, PasswordBox2.Text))
			{
				MessageBox.Show("Пароли не совпадают!");
				return false;

			}
			else if (!CheckPasswordStrength(PasswordBox1.Text))
			{
				MessageBox.Show("Длина пароля должна быть не менее 8 символов. \nПароль должен содержать хотя бы один символ из букв латинского алфавита (A-z), арабских цифр (0-9) и специальных символов!");
				return false;
			}

			return true;
		}

		private bool CheckPasswordStrength(string password)
		{
			var check = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

			if (check.IsMatch(password))
			{
				return true;
			}

			return false;
		}
	}
}

