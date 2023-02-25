using System;
using System.Text;
using System.Numerics;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using NBitcoin;

namespace CryptoLibrary
{

	/// <summary>
	/// Объект криптомонеты "DotNetCoin" 
	/// (тут может быть любое название - можно переименовать)
	/// </summary>
	public class DotNetCoin
	{
		public static string name = "dotNetCoin";
		public static string abbreviation = "dNC";
		private double cash = 0d;

		/// <summary>
		/// Стандартный конструктор
		/// </summary>
		public DotNetCoin(double cash = 0d)
		{
			Cash = cash;
		}

		//Свойство получение скрытого члена cash
		public double Cash { get => cash; set => cash = value; }
		//[JsonIgnore]
		public string Name { get => name; }
		//[JsonIgnore]
		public string Abbreviation { get => abbreviation; }

		//метод получения данных по экземпляру класса монеты
		public string GetInfo()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Name: ").AppendLine(name).Append("Abbreviation: ").AppendLine(abbreviation).Append("Cash: ").AppendLine(cash.ToString());
			return sb.ToString();
		}

		public void Deposit(double cash)
		{
			this.cash += cash;
		}

		public void Withdraw(double cash)
		{
			this.cash -= cash;
		}

	}

	/// <summary>
	/// Объект адреса
	/// </summary>
	public class Address
	{
		public static string pathForBIP39 = @"D:\english.txt";

		//путь к BIT39 словарю (генерация кодовой фразы)
		private string secretPhrase;
		private const byte wordsCount = 12;

		//словарь для загрузки слов
		//[JsonIgnore]//
		private static Dictionary<int, string> BIP39 = new Dictionary<int, string>();

		private PubKey publicKey;
		private BitcoinSecret privateKey;
		private BitcoinAddress address;
		private DotNetCoin wallet;

		//Свойства
		//[JsonIgnore]//
		public DotNetCoin Wallet { get => wallet; set => wallet = value; }
		//[JsonIgnore]//[JsonProperty("public_key")]
		public string PublicKey { get => publicKey.ToHex(); set => publicKey = new PubKey(value); }
		//[JsonIgnore]//[JsonProperty("private_key")]
		public string PrivateKey { get => privateKey.ToWif(); set => privateKey = new BitcoinSecret(value, NBitcoin.Network.Main); }
		//[JsonIgnore]//[JsonProperty("address")]
		public string ThisAddress { get => address.ToString(); set => address = publicKey.GetAddress(ScriptPubKeyType.Segwit, NBitcoin.Network.Main); }
		//[JsonIgnore]//[JsonProperty("secretphrase")]
		public string SecretPhrase { get => secretPhrase; set => secretPhrase = value; }

		/// <summary>
		/// пустой конструктор
		/// </summary>
		public Address()
		{
			if (BIP39.Count < 1)
			{
				InitializeBIP39();
			}

			Wallet = new DotNetCoin();
		}

		/// <summary>
		/// конструктор с кодовой фразой
		/// </summary>
		/// <param name="secret">кодовая фраза</param>
		public Address(string secret = null)
		{
			if(BIP39.Count < 1)
			{
				InitializeBIP39();
			}

			if (String.IsNullOrEmpty(secret))
			{
				secretPhrase = GenerateCodePhrase();
			}
			else
			{
				secretPhrase = secret;

				if (!VerifyPhraseOnBIT39(secretPhrase))
				{
					throw new Exception("Фраза не соответствует словам из словаря BIT39!\nПроверьте правильность введенных данных!");
				}
			}

			Mnemonic mnemonic = new Mnemonic(secretPhrase, Wordlist.English);

			ExtKey generateKey = mnemonic.DeriveExtKey().Derive(new KeyPath("m/84'/0'/0'/0/0"));

			privateKey = generateKey.PrivateKey.GetWif(NBitcoin.Network.Main);

			publicKey = privateKey.PubKey;

			address = publicKey.GetAddress(ScriptPubKeyType.Segwit, NBitcoin.Network.Main);

			Wallet = new DotNetCoin();
		}

		public Address(DotNetCoin wallet, string secret) : this (secret)
		{
			Wallet = wallet;
		}

		[JsonConstructor]
		public Address(DotNetCoin Wallet, string PublicKey, string PrivateKey, string ThisAddress, string SecretPhrase)
		{
			//Wallet = new DotNetCoin();
			this.Wallet = Wallet;
			this.PublicKey = PublicKey;
			this.PrivateKey = PrivateKey;
			this.ThisAddress = ThisAddress;
			this.SecretPhrase = SecretPhrase;
		}
		/// <summary>
		/// Генерация приватного и публичного ключей
		/// с помощью кодовой фразы
		/// </summary>
		/// <param name="secret">кодовая фраза</param>
		//private void GenerateKeyPairs(string secret = null)
		//{
		//	PrivateKey privateKey;

		//	if (!String.IsNullOrEmpty(secret))
		//	{
		//		privateKey = new PrivateKey("secp256k1", BigInteger.Parse(secret));
		//	}
		//	else
		//	{
		//		privateKey = new PrivateKey();
		//	}

		//	BigInteger secretNumber = privateKey.secret;
		//	PublicKey publicKey = privateKey.publicKey();

		//	this.publicKey = publicKey;
		//	this.privateKey = privateKey;
		//	this.secretNumber = secretNumber;

		//	Console.WriteLine(publicKey.ConvertToString());
		//	Console.WriteLine(privateKey.secret);
		//	Console.WriteLine(secret);
		//	Console.WriteLine(secretCode);
		//	Console.WriteLine(secretNumber);
		//}

		/// <summary>
		/// Создание нового адреса
		/// </summary>
		/// <returns>Новый адрес</returns>
		public static Address GenerateNewUser()
		{
			return new Address();
		}

		/// <summary>
		/// Возврат строки публичного ключа в 16 битном виде
		/// </summary>
		/// <returns></returns>
		public string GetPubKeyHex()
		{
			return publicKey.ToHex();//UtilityClass.ConvertToString(publicKey.toString());
		}

		public string GetPrivateKeyHex()
		{
			return privateKey.ToWif();//UtilityClass.ConvertToString(privateKey.toString());
		}

		public string GetAddress()
		{
			//byte[] hash = SHA256.Create().ComputeHash(publicKey.toString());
			return address.ToString();//DotNetCoin.abbreviation + Convert.ToBase64String(hash);
		}

		//public string CreateSignature(string message)
		//{
		//	Signature signature = Ecdsa.sign(message, privateKey);
		//	return signature.toBase64();
		//}

		/// <summary>
		/// Перенос слов из словаря BIT39 в словарь программы
		/// </summary>
		/// <param name="path"></param>
		private static void InitializeBIP39()//string path = pathForBIP39)
		{
			int length = 2048;
			Dictionary<int, string> bip39 = new Dictionary<int, string>(2048);

			using (StreamReader writer = new StreamReader(pathForBIP39))
			{
				for (int i = 0; i < length; i++)
				{
					bip39.Add(i, writer.ReadLine());
				}
			}

			BIP39 = bip39;
		}

		/// <summary>
		/// Генерация кодовой фразы
		/// </summary>
		/// <param name="path">путь к файлу BIT39</param>
		/// <returns></returns>
		public static string GenerateCodePhrase()
		{
			string code;
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < wordsCount; i++)
			{
				int _random = Random();
				sb.Append(BIP39[_random]);

				if (i + 1 != wordsCount)
				{
					sb.Append(" ");
				}
			}

			code = sb.ToString();

			return code;
		}

		/// <summary>
		/// Верификация соответствия всех слов переданной фразы
		/// словам из мнемонического словаря BIT39
		/// </summary>
		/// <param name="secret"></param>
		/// <returns></returns>
		
		public static bool VerifyPhraseOnBIT39(string secret)
		{
			string[] verify = secret.Split(' ');

			for (int i = 0; i < verify.Length; i++)
			{
				string verifyWord = verify[i];

				if (BIP39.ContainsValue(verifyWord))
				{
					continue;
				}
				else
				{
					return false;
				}
			}

			return true;
		}
		/// <summary>
		/// Криптографическая генерация случайного числа
		/// </summary>
		/// <returns>случайное число</returns>
		
		private static uint GenerateRandomInt()
		{
			uint index;

			RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
			byte[] byteArray = new byte[4];
			random.GetBytes(byteArray);

			index = BitConverter.ToUInt32(byteArray, 0);

			return index;
		}
		/// <summary>
		/// Генерация числа от 0 до 2047
		/// (2048 слов в BIT39)
		/// </summary>
		/// <returns></returns>
		private static int Random()
		{
			Random random = new Random((int)GenerateRandomInt());
			int _random = random.Next(0, 2047);
			return _random;
		}

		/// <summary>
		/// Метод возвращает число типа "BigInterer" с заданной длиной в 77 символов 
		/// (для правильной работы алгоритмов генерации ключей в библиотеке "StarkbankEcdsa"
		/// создает кодовую фразу из слов BIT39 с помощью случайной генерации чисел
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		//private static BigInteger GenerateSecretNumber(string code)
		//{
		//	BigInteger bigInteger;
		//	StringBuilder sb = new StringBuilder();

		//	string[] codeArray = code.Split(' ');

		//	int arrayLength = codeArray.Length;
		//	int wordsLength = words.Count;

		//	for (int i = 0; i < arrayLength; i++)
		//	{
		//		for (int j = 0; j < wordsLength; j++)
		//		{
		//			if (words[j] == codeArray[i])
		//			{
		//				string binary = Convert.ToString(j);

		//				sb.Append(binary);

		//				break;
		//			}
		//		}
		//	}

		//	string intString = GetIntStringFromHash(sb.ToString()).Substring(0, 77);

		//	bigInteger = BigInteger.Parse(intString);

		//	return bigInteger;
		//}

		/// <summary>
		/// Получение хешированной строки
		/// </summary>
		/// <param name="intCode">число</param>
		/// <returns></returns>
		private static string GetIntStringFromHash(string intCode)
		{
			SHA512 sha = SHA512.Create();
			byte[] buffer = sha.ComputeHash(Encoding.UTF8.GetBytes(intCode));

			string secretString = BitConverter.ToString(buffer, 0, buffer.Length).Replace("-", " ");
			string[] secretArray = secretString.Split(' ');

			string intString = "";

			for (int i = 0; i < secretArray.Length; i++)
			{
				int value = Convert.ToInt32(secretArray[i], 16);
				intString += value;
			}

			return intString;
		}

		//public string GetPublicKey()
		//{
		//	return String.Format($"Public key: {BitConverter.ToString(publicKey.toString()).Replace("-", "")}");
		//}

		//public string GetPrivateKey()
		//{
		//	return String.Format($"Private key: {BitConverter.ToString(privateKey.toString()).Replace("-", "")}");
		//}

		public string GetSecret()
		{
			//return String.Format($"Secret code: {secretCode}");
			return String.Format(secretPhrase);
		}

		public double GetMoney()
		{
			return Wallet.Cash;
		}
	}

}
