using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace CryptoLibrary
{
	/// <summary>
	/// Объект Block для хранения данных об
	/// id - height (высота)
	/// timeStamp - тики текущей даты (там программно уже реализовано)
	/// hash - хеш блока
	/// prevHash - хеш предыдущего блока
	/// transactions - массив транзакций в этом блок
	/// creator - создатель (метка)
	/// </summary>
	public class Block
	{
		private int height;
		private long timeStamp;
		private byte[] hash;
		private byte[] prevHash;
		private Transaction[] transactions;
		private string creator;

		public int Height { get => height; }
		public long TimeStamp { get => timeStamp; }
		public byte[] Hash { get => hash; }
		public byte[] PrevHash { get => prevHash; }
		public Transaction[] Transactions { get => transactions; }
		public string Creator { get => creator; }

		public Block(int height, byte[] prevHash, List<Transaction> transactions, string creator)
		{
			this.height = height;
			this.prevHash = prevHash;
			this.transactions = transactions.ToArray();
			this.creator = creator;
			timeStamp = DateTime.Now.Ticks;
			hash = GenerateHash();
		}

		//JsonConstructor - атрибут для работы json библиотеки newtonsoft.json
		[JsonConstructor]
		public Block(int height, long timeStamp, byte[] hash, byte[] prevHash, List<Transaction> transactions, string creator)
		{
			this.height = height;
			this.prevHash = prevHash;
			this.transactions = transactions.ToArray();
			this.creator = creator;
			this.timeStamp = timeStamp;
			this.hash = hash;
		}

		//private string GetData()
		//{
		//	string result = "";

		//	for (int i = 0; i < Transactions.Length; i++)
		//	{
		//		result += Transactions[i].TimeStamp;
		//		result += Transactions[i].Sender;
		//		result += Transactions[i].Recipient;
		//		result += Transactions[i].Amount;
		//		result += Transactions[i].Fee;
		//	}

		//	return result;
		//}

		/// <summary>
		/// Алгоритм генерации хеша
		/// </summary>
		/// <returns></returns>
		private byte[] GenerateHash()
		{
			var sha = SHA256.Create();
			byte[] timeStamp = BitConverter.GetBytes(TimeStamp);

			byte[] transactionHash = Transactions.ConvertToByte();

			byte[] headerBytes = new byte[timeStamp.Length + PrevHash.Length + transactionHash.Length];

			Buffer.BlockCopy(timeStamp, 0, headerBytes, 0, timeStamp.Length);
			Buffer.BlockCopy(PrevHash, 0, headerBytes, timeStamp.Length, PrevHash.Length);
			Buffer.BlockCopy(transactionHash, 0, headerBytes, timeStamp.Length + PrevHash.Length, transactionHash.Length);

			byte[] hash = sha.ComputeHash(headerBytes);

			//Console.WriteLine($"Хеш блока {this.Height} = {UtilityClass.ConvertToString(hash)}");

			return hash;
		}
	}
}
