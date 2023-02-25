using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CryptoLibrary
{
	public class Transaction
	{
		public const double feePercent = 0.0001;

		private long timeStamp;
		private string sender;
		private string macAddress;
		private string recipient;
		private double amount;
		private double fee;

		public long TimeStamp { get => timeStamp;}
		public string Sender { get => sender; }
		public string MacAddress { get => macAddress; }
		public string Recipient { get => recipient; }
		public double Amount { get => amount;  }
		public double Fee { get => fee; }

		public Transaction(string sender, string macAddress, string recipient, double amount, double percent = feePercent)
		{
			Console.WriteLine("Получена транзакция с диска!");
			timeStamp = DateTime.Now.Ticks;
			this.sender = sender;
			this.macAddress = macAddress;
			this.recipient = recipient;
			this.amount = amount;
			fee = amount * percent;
		}

		[JsonConstructor]
		public Transaction(long timeStamp, string sender, string macAddress, string recipient, double amount, double fee)
		{
			Console.WriteLine("Получена транзакция с диска!");
			this.timeStamp = timeStamp;
			this.sender = sender;
			this.macAddress = macAddress;
			this.recipient = recipient;
			this.amount = amount;
			this.fee = fee;
		}

		public static Transaction GenerateNewTransaction(string sender, string macAddress, string recipient, double amount, double percent = feePercent)
		{
			return new Transaction(sender, macAddress, recipient, amount, percent);
		}

		public static Transaction ReturnExistingTransaction(long timeStamp, string sender, string macAddress, string recipient, double amount, double fee)
		{
			return new Transaction(timeStamp, sender, macAddress, recipient, amount, fee);
		}

		public string GetInfo()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("Timestamp ").AppendLine(DateTime.FromBinary(TimeStamp).ToString());
			sb.Append("Sender ").AppendLine(Sender);
			sb.Append("SenderMac ").AppendLine(Sender);
			sb.Append("Recipient ").AppendLine(Recipient);
			sb.Append("Amount ").Append(Amount.ToString("n10").TrimEnd('0')).AppendLine();
			sb.Append("Fee ").Append(Fee.ToString("n10").TrimEnd('0')).AppendLine();

			return sb.ToString();
		}
	}

}