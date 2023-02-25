using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;

namespace CryptoLibrary
{
	public class Chain
	{
		const string path = @"D:\BlockChain.txt";
		//public static string BIP39 = @"D:\english.txt";

		public static bool startTimer = false;

		static JsonSerializerSettings settings = new JsonSerializerSettings()
		{
			Formatting = Formatting.Indented
		};

		/// <summary>
		/// 
		/// Создается блокчейн с помощью указания текстового файла (или его создание - при отсутствии)
		/// 
		/// сериализация данных блокчейна и добавление в файл
		/// если файл пустой - создается генезис блок (начальный)
		/// 
		/// добавление асинхронной задачи для записи блока после поступление первой транзакции по таймеру
		/// таймер пока что выставлен на 10 секунд
		/// 
		/// </summary>
		/// <param name="pathName"></param>
		public async void Initialize(string pathName = path)
		{
			Console.WriteLine("Введите путь к словарю BIP39 в формате *.txt");
			Console.WriteLine(@"(Оставьте это поле пустым, чтобы выбрать местоположение по умолчанию: D:\english.txt): ");

			string bip39 = Console.ReadLine();

			if (!String.IsNullOrEmpty(bip39))
			{
				Address.pathForBIP39 = bip39;
			}

			Console.WriteLine("Введите путь к блокчейну в формате *.txt");
			Console.WriteLine(@"(Оставьте это поле пустым, чтобы выбрать местоположение по умолчанию: D:\BlockChain.txt): ");

			string getBlockChain = Console.ReadLine();

			if (!String.IsNullOrEmpty(getBlockChain))
			{
				pathName = getBlockChain;
			}

			Console.WriteLine();

			if (!(File.Exists(pathName)))
			{
				using (StreamReader reader = new StreamReader(File.Create(pathName)))
				{
					Console.WriteLine("Файл с блокчейном не найден!\nСоздание нового файла!");
				}
			}

			using (StreamReader reader = new StreamReader(pathName))
			{
				getBlockChain = reader.ReadToEnd();

				Console.WriteLine("В блокчейне {0} символов", getBlockChain.Length);
			}

			if (getBlockChain.Length > 0)
			{
				Console.WriteLine("Получаем блокчейн с диска!");

				try
				{
					BlockChain = JsonConvert.DeserializeObject<List<Block>>(getBlockChain, settings);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
			else
			{
				Console.WriteLine("Блоков не найдено!");
				CreateGenesisBlock();
				SaveDataToDisk(pathName);
			}

			//await CheckForNewTransactions();
			await Task.Run(() =>
			{
				while (true)
				{
					if (startTimer)
					{
						StartTimer(pathName);
					}
					else
					{
						Console.WriteLine("Ожидание транзакций...");
						Thread.Sleep(1000);
					}
				}
			});
		}

		//блокчейн 
		public static List<Block> BlockChain = new List<Block>();

		//пул транзакций блока
		public static List<Transaction> TransactionPool = new List<Transaction>();

		//получение списка транзакций
		public static List<Transaction> GetTransactions()
		{
			return TransactionPool;
		}

		/// <summary>
		/// 
		/// Добавление новой транзакции в пул транзакций
		/// после чего запускается таймер, по истичении которого создается блок со всеми добавленными транзакциями
		/// 
		/// </summary>
		/// <param name="trx">транзакция</param>
		public void AddTransactionToPool(Transaction trx)
		{
			Console.WriteLine("Adding Transaction");

			TransactionPool.Add(trx);

			if (TransactionPool.Count > 0 && startTimer == false)
			{
				startTimer = true;
			}
		}

		//add block methods

		/// <summary>
		/// Добавление блока в блокчейн через список транзакций
		/// </summary>
		/// <param name="transactions"></param>
		public void AddBlock(List<Transaction> transactions)
		{
			Block block;

			if (BlockChain.Count == 0)
			{
				Console.WriteLine("NO BLOCKS IN SYSTEM!");
				CreateGenesisBlock();
			}
			else
			{
				Block lastBlock = BlockChain.Last();
				int newHeight = lastBlock.Height + 1;
				byte[] prevHash = lastBlock.Hash;
				block = CreateNewBlock(newHeight, prevHash, transactions, "UserName");

				BlockChain.Add(block);
			}
		}

		/// <summary>
		/// Добавление блока в блокчейн через одну транзакцию
		/// </summary>
		/// <param name="transaction"></param>
		public void AddBlock(Transaction transaction)
		{
			Block block;

			if (BlockChain.Count == 0)
			{
				Console.WriteLine("NO BLOCKS IN SYSTEM!");
				CreateGenesisBlock();
			}
			else
			{
				Block lastBlock = BlockChain.Last();
				int newHeight = lastBlock.Height + 1;
				byte[] prevHash = lastBlock.Hash;

				TransactionPool.Add(transaction);
				block = CreateNewBlock(newHeight, prevHash, TransactionPool, "UserName");
				TransactionPool.Clear();

				BlockChain.Add(block);
			}
		}

		/// <summary>
		/// Добавление блока в блокчейн через пул транзакций
		/// </summary>
		public void CreateBlockFromTransactionPool()
		{
			Block lastBlock = BlockChain.Last();
			int newHeight = lastBlock.Height + 1;
			byte[] prevHash = lastBlock.Hash;

			Block block = CreateNewBlock(newHeight, prevHash, TransactionPool, "UserName");
			TransactionPool.Clear();

			BlockChain.Add(block);
		}

		//создание генезис блока
		private void CreateGenesisBlock()
		{
			List<Transaction> transactions = new List<Transaction>();

			Transaction trx = Transaction.GenerateNewTransaction("Admin", "Admin", "Genesis Block", 100000000d, 0d);

			transactions.Add(trx);

			BlockChain.Add(new Block(1, Encoding.ASCII.GetBytes(String.Empty), transactions, "Admin"));

		}
		//создание обычного блока
		private static Block CreateNewBlock(int newHeight, byte[] prevHash, List<Transaction> transactions, string address)
		{
			return new Block(newHeight, prevHash, transactions, address);
		}

		//получение баланса по адресу (перебор)
		public string GetAddressBalance(string public_key)
		{
			double balance = 0;
			double spending = 0;
			double income = 0;
			string info = "";

			foreach (Block block in BlockChain)
			{
				Transaction[] transactions = block.Transactions;

				foreach (Transaction transaction in transactions)
				{
					string sender = transaction.Sender;
					string recipient = transaction.Recipient;

					if (public_key.ToLower().Equals(sender.ToLower()))
					{
						spending += transaction.Amount + transaction.Fee;
					}
					if (public_key.ToLower().Equals(recipient.ToLower()))
					{
						income += transaction.Amount;
					}
					balance = income - spending;
				}
			}

			//info += $"Баланс адреса {public_key}: {balance}, Расходы: {spending}, Доходы: {income}\n";

			info = balance.ToString();

			return info;
		}
		//получение полной истории по адресу
		public string GetAddressFullHistory(string public_key)
		{
			string history = $"Выписка об операциях адреса {public_key} :\n\n";

			foreach (Block block in BlockChain)
			{
				Transaction[] transactions = block.Transactions;

				foreach (Transaction transaction in transactions)
				{
					string sender = transaction.Sender;
					string recipient = transaction.Recipient;

					if (public_key.ToLower().Equals(sender.ToLower()) || public_key.ToLower().Equals(recipient.ToLower()))
					{
						history += transaction.ConvertToString();
						//history += String.Format("Timestamp :{0}\n", transaction.TimeStamp);
						//history += String.Format("Sender   :{0}\n", transaction.Sender);
						//history += String.Format("Recipient :{0}\n", transaction.Recipient);
						//history += String.Format("Amount    :{0}\n", transaction.Amount);
						//history += String.Format("Fee       :{0}\n", transaction.Fee);
						//history += "--------------\n--------------\n--------------\n";
					}
				}
			}

			return history;
		}

		//вывод данных в консоль
		public string GetBlockChainExplorer()
		{
			string explorer = "====== Blockchain Explorer =====\n\n";

			foreach (Block block in BlockChain)
			{
				explorer += GetData(block);
			}

			explorer += "\n\n====== Blockchain Explorer =====\n\n";
			return explorer;
		}
		private static string GetData(Block block)
		{
			string data = String.Format("Block {0} data:\n", block.Height);

			data += block.ConvertToString();
			data += "\n--------------------\n";

			return data;

			//Console.WriteLine("Height      :{0}", block.Height);
			//Console.WriteLine("Timestamp   :{0}", block.TimeStamp.ConvertToDateTime());
			//Console.WriteLine("Prev. Hash  :{0}", block.PrevHash.ConvertToHexString());
			//Console.WriteLine("Hash        :{0}", block.Hash.ConvertToHexString());
			//Console.WriteLine("Transactins :{0}", block.Transactions.ConvertToString());
			//Console.WriteLine("Creator     :{0}", block.Creator);
			//Console.WriteLine("--------------");
		}

		//проверка соседних блоков на соответствие хеш-сумм
		public static bool CheckBlockChain()
		{
			Console.WriteLine("Запуск проверки блокчейна!");

			int length = BlockChain.Count();

			for (int i = 1; i < length; i++)
			{
				Console.WriteLine("{0}\n{1}", BlockChain[i].PrevHash.ConvertToString(), BlockChain[i - 1].Hash.ConvertToString());

				if (!String.Equals(BlockChain[i].PrevHash.ConvertToString(), BlockChain[i - 1].Hash.ConvertToString()))
				{
					Console.WriteLine("ОШИБКА! Блокчейн поврежден! Не соответствуют хеш-суммы блоков '{0}' и '{1}'", i - 1, i);
					return false;
				}

				Console.WriteLine("Хеш-суммы блоков '{0}' и '{1}' соответствуют!", i - 1, i);

			}
			Console.WriteLine("УСПЕХ! Проверка блокчейна не выявила ошибок хеш-сумм!");
			return true;
		}

		//запись данных на диск
		public void SaveDataToDisk(string filePath)
		{
			using (StreamWriter writer = new StreamWriter(filePath))
			{
				string serializedBlockChain = JsonConvert.SerializeObject(BlockChain, Formatting.Indented, settings);

				writer.Write(serializedBlockChain);
			}

			Console.WriteLine("Блокчейн сохранен на диск!");
		}

		/// <summary>
		/// Запуск таймера
		/// </summary>
		public void StartTimer(string pathName)
		{
			Console.WriteLine("Старт таймера!");

			for (int i = 0; i < 10; i++)
			{
				Thread.Sleep(1000);
				Console.WriteLine($"Прошло {i} секунд");
			}

			if (TransactionPool.Count > 0)
			{
				CreateBlockFromTransactionPool();

				if (CheckBlockChain())
				{
					SaveDataToDisk(pathName);
				}
				else
				{
					string getBlockChain;

					using (StreamReader reader = new StreamReader(pathName))
					{
						getBlockChain = reader.ReadToEnd();

						Console.WriteLine("Загружаем резервную копию!");
					}

					if (getBlockChain.Length > 0)
					{
						Console.WriteLine("Получаем блокчейн с диска!");

						try
						{
							BlockChain = JsonConvert.DeserializeObject<List<Block>>(getBlockChain, settings);
						}
						catch (Exception ex)
						{
							Console.WriteLine(ex.Message);
						}
					}
				}

				startTimer = false;
			}

		}

	}
}
