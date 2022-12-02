using CryptoLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientUI
{
	/// <summary>
	/// Окно логина
	/// </summary>
	public partial class LoggedForm : Form
	{
		private readonly static string abbreviation = MainMenu.Address.Wallet.Abbreviation;
		private static double money = 0d; 
		private static string macAddress;
		private static string received = "" ;

		public LoggedForm()
		{
			InitializeComponent();

			UpdateMoney();

			macAddress = GetMacAddress();
		}

		private void ExitButton_Click(object sender, EventArgs e)
		{
			MainMenu.Address = null;
			this.Dispose();
		}

		private async void Button_Click(object sender, EventArgs e)
		{
			char split = ';';
			string reply = "";

			StringBuilder sb = new StringBuilder();
			Network.TCPCommands command = new Network.TCPCommands(Network.Commands.Null);

			if (sender.Equals(DepositButton))
			{
				sb.Append(MainMenu.Address.GetSecret()).Append(split).Append(DepositBox.Text).Append(split).Append(macAddress);

				command = new Network.TCPCommands(Network.Commands.Deposit, sb.ToString());
			}
			else if (sender.Equals(WithdrawButton))
			{
				sb.Append(MainMenu.Address.GetSecret()).Append(split).Append(WithdrawBox.Text).Append(split).Append(macAddress);

				command = new Network.TCPCommands(Network.Commands.Withdraw, sb.ToString());
			}
			else if (sender.Equals(TransferButton))
			{
				sb.Append(MainMenu.Address.GetSecret()).Append(split).Append(TransferBox.Text).Append(split).Append(macAddress).Append(split).Append(AddressBox.Text);

				command = new Network.TCPCommands(Network.Commands.Transaction, sb.ToString());
			}

			try
			{
				string serialized = JsonConvert.SerializeObject(command);
				//асинхронная отправка команды

				reply = await MainMenu.Client.SendData(serialized);
				//асинхронное получение ответа и вывод на окно

				MessageBox.Show("Ожидайте, транзакция поставлена в очередь на исполнение!\nДанный этап может занимать до 20 секунд!");

				MainMenu.Address.Wallet = JsonConvert.DeserializeObject<DotNetCoin>(reply);

				MessageBox.Show(String.Format("На ваш счет успешно зачислено: {0} {1}\nПерезайдите в аккаунт!", DepositBox.Text, abbreviation));

			}
			catch (Exception)
			{
				MessageBox.Show(reply);
			}
			finally
			{
				UpdateMoney();

				this.DialogResult = DialogResult.Retry;

				this.Dispose();
			}
		}

		private void BoxEnter(object sender, EventArgs e)
		{
			if (sender.Equals(DepositBox))
			{
				DepositBox.Text = "";
			}

			if (sender.Equals(WithdrawBox))
			{
				WithdrawBox.Text = "";
			}

			if (sender.Equals(AddressBox))
			{
				AddressBox.Text = "";
			}

			if (sender.Equals(TransferBox))
			{
				TransferBox.Text = "";
			}
		}

		private void BoxExit(object sender, EventArgs e)
		{
			if (sender.Equals(DepositBox) && String.IsNullOrWhiteSpace(DepositBox.Text))
			{
				DepositBox.Text = "Укажите сумму внесения";
			}

			if (sender.Equals(WithdrawBox) && String.IsNullOrWhiteSpace(WithdrawBox.Text))
			{
				WithdrawBox.Text = "Укажите сумму снятия";
			}

			if (sender.Equals(AddressBox) && String.IsNullOrWhiteSpace(AddressBox.Text))
			{
				AddressBox.Text = "Укажите адрес получателя";
			}

			if (sender.Equals(TransferBox) && String.IsNullOrWhiteSpace(TransferBox.Text))
			{
				TransferBox.Text = "Укажите сумму перевода";
			}
		}

		private void ExitFromAllBoxes(EventArgs e)
		{
			BoxExit(DepositBox, e);
			BoxExit(WithdrawBox, e);
			BoxExit(AddressBox, e);
			BoxExit(TransferBox, e);
		}

		private void LoggedForm_Click(object sender, EventArgs e)
		{
			this.ActiveControl = null;
		}

		private static string GetMacAddress()
		{
			string mac = "";
			foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
			{

				if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && 
																						!nic.Description.Contains("Pseudo") &&
																						!nic.Description.Contains("Tap") && 
																						!nic.Name.Contains("Virtual") && 
																						!nic.Name.Contains("Pseudo") &&
																						!nic.Name.Contains("Tap")))
				{
					if (nic.GetPhysicalAddress().ToString() != "")
					{
						mac = nic.GetPhysicalAddress().ToString();
					}
				}
			}
			//MessageBox.Show(mac);
			return mac;
		}
		private void UpdateMoney()
		{
			money = MainMenu.Address.GetMoney();

			StringBuilder sb = new StringBuilder();
			sb.Append(money.ToString("n10"));
			sb.Append(" ");
			sb.Append(abbreviation);

			BalanceBox.Text = sb.ToString();
		}

		private void MaskInput(object sender, EventArgs e)
		{
			Regex checkMoney = new Regex("^[0-9.,]*");
			Regex checkKey = new Regex("^[a-b0-9]*");

			string text;

			if (sender.Equals(DepositBox))
			{
				if (!checkMoney.IsMatch(DepositBox.Text))
				{
					text = DepositBox.Text;

					foreach (Match Match in checkMoney.Matches(text))
					{
						text.Replace(Match.Value, String.Empty);
					}

					DepositBox.Text = text;
				}
			}
			if (sender.Equals(WithdrawBox))
			{
				if (!checkMoney.IsMatch(WithdrawBox.Text))
				{
					text = WithdrawBox.Text;

					foreach (Match Match in checkMoney.Matches(text))
					{
						text.Replace(Match.Value, String.Empty);
					}

					WithdrawBox.Text = text;
				}
			}
			if (sender.Equals(TransferBox))
			{
				if (!checkMoney.IsMatch(TransferBox.Text))
				{
					text = TransferBox.Text;

					foreach (Match Match in checkMoney.Matches(text))
					{
						text.Replace(Match.Value, String.Empty);
					}

					TransferBox.Text = text;
				}
			}
			if (sender.Equals(AddressBox))
			{
				if (!checkKey.IsMatch(AddressBox.Text))
				{
					text = AddressBox.Text;

					foreach (Match Match in checkKey.Matches(text))
					{
						text.Replace(Match.Value, String.Empty);
					}

					AddressBox.Text = text;
				}
			}
		}
	}
}
