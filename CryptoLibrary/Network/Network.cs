using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Security.Authentication;

namespace CryptoLibrary
{
	public static class Network
	{
		/// <summary>
		/// Перечисление типов команд
		/// </summary>
		public enum Commands
		{
			Null,
			Generate,
			Check,
			Deposit,
			Withdraw,
			Transaction
		}

		//сертификат ssl
		public static string certPath = @"D:\Certificate\certificate.crt";
		public static string serverPfxPath = @"D:\Certificate\server.pfx";

		//строка для соединения с бд
		public static string database = @"(localdb)\MSSQLLocalDB";
		public static string connectionParams = String.Format(@"Persist Security Info = False; Integrated Security = true; Initial Catalog = BLOCKCHAIN; Server = {0}", database);

		//порты потока для "слушания" сообщений
		public const int clientListenOn = 8005;
		public const int serverListenOn = 8006;
		public const int bufferSize = 256;

		/// <summary>
		/// Объект клиента с реализацией общения по протоколу TCP
		/// </summary>
		public class TCPClient
		{
			public TcpClient tcpClient;
			public NetworkStream networkStream;
			public SslStream sslStream;

			public static bool ValidateServerCertificate(object sender,
																		X509Certificate certificate,
																		X509Chain chain,
																		SslPolicyErrors sslPolicyErrors)
			{

				X509Certificate2 certificate2 = new X509Certificate2(certificate);

				if (sslPolicyErrors == SslPolicyErrors.None)
				{
					return true;
				}

				throw new Exception(String.Format("Ошибка сертификата: {0}", sslPolicyErrors));
			}

			public async Task<string> SendData(string message)
			{
				string response = "";

				//X509Certificate2 certificate1 = new X509Certificate2(certPath);

				using (tcpClient = new TcpClient())
				{
					await tcpClient.ConnectAsync(IPAddress.Loopback, serverListenOn);

					sslStream = new SslStream(tcpClient.GetStream(),
													  false,
													  new RemoteCertificateValidationCallback(ValidateServerCertificate),
													  null
					);

					try
					{
						await sslStream.AuthenticateAsClientAsync("Mirea.ru");
					}
					catch (Exception ex)
					{
						Console.WriteLine("Ошибка: {0}", ex.Message);

						if (ex.InnerException != null)
						{
							Console.WriteLine("Внутренняя ошибка: {0}", ex.InnerException.Message);
						}

						Console.WriteLine("Сбой при проверке подлинности! Закрываем соединение.");

						sslStream.Close();
						tcpClient.Close();

						return "";
					}

					await WriteData(message, sslStream);

					response = await GetData(sslStream);

					await sslStream.FlushAsync();
				}
				return response;
			}

			public async Task<bool> WriteData(string message, SslStream sslStream)// NetworkStream networkStream)
			{
				byte[] binaryMessage = Encoding.UTF8.GetBytes(message);
				int count = binaryMessage.Length;
				int i = 0;

				try
				{
					while (i < count)
					{
						//await networkStream.WriteAsync(binaryMessage, i, bufferSize);
						await sslStream.WriteAsync(binaryMessage, i, bufferSize);
						i += bufferSize;
					}
				}
				catch (Exception)
				{
					//await networkStream.WriteAsync(binaryMessage, i, binaryMessage.Length);
					await sslStream.WriteAsync(binaryMessage, i, binaryMessage.Length);
				}

				//await networkStream.WriteAsync(Encoding.UTF8.GetBytes("<EOF>"), 0, "<EOF>".Length);
				await sslStream.WriteAsync(Encoding.UTF8.GetBytes("<EOF>"), 0, "<EOF>".Length);

				return true;
			}

			public async Task<string> GetData(SslStream sslStream)//NetworkStream networkStream)
			{
				byte[] buffer = new byte[256 * 4];
				int i = 0;
				string message = "";

				while (true)
				{
					//await networkStream.ReadAsync(buffer, i, bufferSize);
					await sslStream.ReadAsync(buffer, i, bufferSize);
					message += Encoding.UTF8.GetString(buffer, i, bufferSize);

					if (message.Contains("<EOF>"))
					{
						message = message.Replace("\0", String.Empty).Replace("<EOF>", String.Empty);
						break;
					}

					i += bufferSize;
				}

				return message;
			}
		}

		/// <summary>
		/// Объект сервера для связи по протоколу TCP
		/// </summary>
		public class TCPServer
		{
			public static Chain chain;
			public static SqlConnection sqlConnection;
			public static X509Certificate2 certificate;

			public TcpListener tcpListener;
			public TcpClient tcpClient;
			public SslStream sslStream;

			public void Initialize()
			{
				Console.WriteLine("Введите путь к сертификату в формате *.crt");
				Console.WriteLine(@"(Оставьте поле пустым, чтобы ввести значение по умолчанию: D:\Certificate\certificate.crt):");

				string crt = Console.ReadLine();

				if (!String.IsNullOrEmpty(crt))
				{
					certPath = crt;
				}

				Console.WriteLine("Введите путь к сертификату в формате *.pfx");
				Console.WriteLine(@"(Оставьте поле пустым, чтобы ввести значение по умолчанию: D:\Certificate\server.pfx):");

				string pfx = Console.ReadLine();

				if (!String.IsNullOrEmpty(pfx))
				{
					serverPfxPath = pfx;
				}

				Console.WriteLine("Введите название базы данных");
				Console.WriteLine(@"(Оставьте поле пустым, чтобы ввести значение по умолчанию: (localdb)\MSSQLLocalDB):");

				string data = Console.ReadLine();

				if (!String.IsNullOrEmpty(data))
				{
					database = data;
				}

				chain = new Chain();
				sqlConnection = new SqlConnection(connectionParams);
				certificate = new X509Certificate2(serverPfxPath, "");

				chain.Initialize();
			}

			public void Listen()
			{
				_ = Task.Run(async () =>
				{
					tcpListener = TcpListener.Create(serverListenOn);
					tcpListener.Start();

					while (true)
					{
						tcpClient = await tcpListener.AcceptTcpClientAsync();

						string sendData = "";
						string getData = "";

						sslStream = new SslStream(tcpClient.GetStream(), false);

						try
						{
							await sslStream.AuthenticateAsServerAsync(certificate, false, true);

							DisplaySecurityLevel(sslStream);
						}
						catch (Exception ex)
						{
							Console.WriteLine("Ошибка: {0}", ex.Message);

							if (ex.InnerException != null)
							{
								Console.WriteLine("Внутренняя ошибка: {0}", ex.InnerException.Message);
							}

							Console.WriteLine("Сбой при проверке подлинности!! Закрываем соединение.");

							sslStream.Close();
							tcpClient.Close();

							return;
						}

						getData = await GetData(sslStream);//networkStream);
						Console.WriteLine("Запрос:");

						try
						{
							TCPCommands command = JsonConvert.DeserializeObject<TCPCommands>(getData);
							command.ExecuteCommand(out sendData);

							Console.WriteLine(sendData);
							await WriteData(sendData, sslStream);// networkStream);
						}
						catch (Exception ex)
						{
							Console.WriteLine(ex.Message);
							//await networkStream.WriteAsync(Encoding.UTF8.GetBytes(ex.Message), 0, ex.Message.Length);
							await sslStream.WriteAsync(Encoding.UTF8.GetBytes(ex.Message), 0, ex.Message.Length);
						}
						finally
						{
							//await networkStream.FlushAsync();
							await sslStream.FlushAsync();

							sslStream.Close();
							tcpClient.Close();
						}
						//}
					}
				});
			}

			public async Task<bool> WriteData(string message, SslStream sslStream)// NetworkStream networkStream)
			{
				byte[] binaryMessage = Encoding.UTF8.GetBytes(message);
				int count = binaryMessage.Length;
				int i = 0;

				try
				{
					while (i < count)
					{
						//await networkStream.WriteAsync(binaryMessage, i, bufferSize);
						await sslStream.WriteAsync(binaryMessage, i, bufferSize);
						i += bufferSize;
					}
				}
				catch (Exception)
				{
					//await networkStream.WriteAsync(binaryMessage, i, binaryMessage.Length);
					await sslStream.WriteAsync(binaryMessage, i, binaryMessage.Length - i);
				}

				//await networkStream.WriteAsync(Encoding.UTF8.GetBytes("<EOF>"), 0, "<EOF>".Length);
				await sslStream.WriteAsync(Encoding.UTF8.GetBytes("<EOF>"), 0, "<EOF>".Length);

				return true;
			}

			public async Task<string> GetData(SslStream sslStream)//NetworkStream networkStream)
			{
				byte[] buffer = new byte[256 * 4];
				int i = 0;
				string message = "";

				while (true)
				{
					//await networkStream.ReadAsync(buffer, i, bufferSize);
					await sslStream.ReadAsync(buffer, i, bufferSize);
					message += Encoding.UTF8.GetString(buffer, i, bufferSize);

					if (message.Contains("<EOF>"))
					{
						message = message.Replace("\0", String.Empty).Replace("<EOF>", String.Empty);
						break;
					}

					i += bufferSize;
				}

				return message;
			}

			public void DisplaySecurityLevel(SslStream stream)
			{
				Console.WriteLine("Cipher: {0} strength {1}", stream.CipherAlgorithm, stream.CipherStrength);
				Console.WriteLine("Hash: {0} strength {1}", stream.HashAlgorithm, stream.HashStrength);
				Console.WriteLine("Key exchange: {0} strength {1}", stream.KeyExchangeAlgorithm, stream.KeyExchangeStrength);
				Console.WriteLine("Protocol: {0}", stream.SslProtocol);
			}
		}

		/// <summary>
		/// Объект комманд для стандартизации обрабатываемых сообщений сервером
		/// </summary>
		public class TCPCommands
		{
			public Commands command { get; set; }
			public string receivedData { get; set; }
			//private string sendData;

			[JsonConstructor]
			public TCPCommands(Commands command, string data = null)
			{
				this.command = command;
				this.receivedData = data;
			}

			public Commands GetCommand()
			{
				return command;
			}

			/// <summary>
			/// Выполнение команды через проверку типа
			/// </summary>
			/// <param name="sendData"></param>
			/// <returns></returns>
			public bool ExecuteCommand(out string sendData)//UDPCommands command)
			{
				sendData = "";

				switch (this.GetCommand())
				{
					//secret
					case Commands.Check:
						{
							Console.WriteLine("Проверка входящего сообщения!");
							//sendData = "Data checked!";

							try
							{
								using (TCPServer.sqlConnection = new SqlConnection(connectionParams))
								{
									TCPServer.sqlConnection.Open();

									Address getAddress = new Address(this.receivedData);
									Console.WriteLine(getAddress.GetPubKeyHex());
									Console.WriteLine(getAddress.GetPrivateKeyHex());

									SqlCommand select = new SqlCommand("SELECT COUNT(*) FROM Addresses WHERE public_key = @public_key AND private_key = @private_key", TCPServer.sqlConnection);
									select.Parameters.AddWithValue("@public_key", getAddress.GetPubKeyHex());
									select.Parameters.AddWithValue("@private_key", getAddress.GetPrivateKeyHex());
									int count = (int)select.ExecuteScalar();
									Console.WriteLine("Найдено аккаунтов: {0}", count);

									if (count < 1)
									{
										sendData = "ПРОВАЛ! Указанного адреса не существует!";
										throw (new Exception(sendData));
									}
									else
									{
										//getAddress.Wallet.Deposit(Convert.ToDouble(UDPServer.chain.GetAddressBalance(getAddress.GetPubKeyHex())));
										while (Chain.startTimer)
										{
											Thread.Sleep(10000);
										}

										getAddress.Wallet.Cash = Convert.ToDouble(TCPServer.chain.GetAddressBalance(getAddress.GetPubKeyHex()));
										sendData = JsonConvert.SerializeObject(getAddress); //JsonConvert.SerializeObject(getAddress.Wallet);
										Console.WriteLine("УСПЕХ! Адрес с указанной фразой зарегистрирован в системе!");
									}
								}
							}
							catch (Exception ex)
							{
								//Console.WriteLine(ex.Message);
								sendData = ex.Message;
							}

							return true;
						}
					//data = phone_number, email, password
					case Commands.Generate:
						{
							bool unique = false;

							while (!unique)
							{
								Console.WriteLine("Генерация нового кошелька!");
								Address address = new Address(null);
								using (TCPServer.sqlConnection = new SqlConnection(connectionParams))
								{
									TCPServer.sqlConnection.Open();
									Console.WriteLine("Соединение с базой данных!");

									using (SqlCommand select = new SqlCommand("SELECT COUNT(*) FROM Addresses WHERE public_key = @public_key AND private_key = @private_key", TCPServer.sqlConnection))
									{

										select.Parameters.AddWithValue("@public_key", address.GetPubKeyHex());//.Value = address.GetPubKeyHex();
										select.Parameters.AddWithValue("@private_key", address.GetPrivateKeyHex());//.Value = address.GetPrivateKeyHex();
										int count = (int)select.ExecuteScalar();
										Console.WriteLine("Найдено совпадений: {0}", count);

										if (count < 1)
										{
											string[] parsed_data = receivedData.Split(';');

											string phone_number = parsed_data[0];
											string email = parsed_data[1];
											string password = parsed_data[2];

											for (int i = 0; i < parsed_data.Length; i++)
											{
												Console.WriteLine(parsed_data[i]);
											}

											using (SqlCommand insert = new SqlCommand("INSERT INTO Addresses VALUES(@public_key,@private_key,@secret,@phone_number,@email,@password)", TCPServer.sqlConnection))
											{
												insert.Parameters.AddWithValue("@public_key", address.GetPubKeyHex());//.Value = address.GetPubKeyHex();
												insert.Parameters.AddWithValue("@private_key", address.GetPrivateKeyHex()); //System.Data.SqlDbType.NVarChar).Value = address.GetPrivateKeyHex();
												insert.Parameters.AddWithValue("@secret", address.GetSecret()); //System.Data.SqlDbType.NVarChar).Value = address.GetPrivateKeyHex();
												insert.Parameters.AddWithValue("@phone_number", phone_number);// System.Data.SqlDbType.NVarChar).Value = phone_number;
												insert.Parameters.AddWithValue("@email", email);// System.Data.SqlDbType.NVarChar).Value = email;
												insert.Parameters.AddWithValue("@password", password);// System.Data.SqlDbType.NVarChar).Value = password;

												insert.ExecuteNonQuery();
												unique = true;

												Console.WriteLine("УСПЕХ! Зарегистрирован новый аккаунт:\n\tПубличный ключ: {0},\n\tПриватный ключ: {1},\n\tФраза: {2},\n\tТелефон: {3},\n\tПочта: {4},\n\tПароль: {5}", address.GetPubKeyHex(), address.GetPrivateKeyHex(), address.GetSecret(), phone_number, email, password);
											}
										}
										else
										{
											Console.WriteLine("ПРОВАЛ! В базе данных уже есть данный адрес!\nЕще одна попытка!");
										}
									}
								}

								sendData = address.GetSecret();
								Console.WriteLine(sendData);
							}

							return true;
						}
					//data = secret, money, macAddress
					case Commands.Deposit:
						{
							Console.WriteLine("Запрос на внесение средств!");

							try
							{
								using (TCPServer.sqlConnection = new SqlConnection(connectionParams))
								{
									TCPServer.sqlConnection.Open();

									string[] parsedData = this.receivedData.Split(';');

									string secret = parsedData[0];
									double money = Convert.ToDouble(parsedData[1].Replace('.', ','));
									string macAddress = parsedData[2];

									Address getAddress = new Address(secret);
									//Console.WriteLine(getAddress.GetPubKeyHex());
									//Console.WriteLine(getAddress.GetPrivateKeyHex());

									SqlCommand select = new SqlCommand("SELECT COUNT(*) FROM Addresses WHERE public_key = @public_key AND private_key = @private_key", TCPServer.sqlConnection);
									select.Parameters.AddWithValue("@public_key", getAddress.GetPubKeyHex());
									select.Parameters.AddWithValue("@private_key", getAddress.GetPrivateKeyHex());
									int count = (int)select.ExecuteScalar();
									Console.WriteLine("Найдено аккаунтов: {0}", count);

									if (count < 1)
									{
										sendData = "ПРОВАЛ! Указанного адреса не существует!";
										throw (new Exception(sendData));
									}
									else
									{
										Console.WriteLine("УСПЕХ! Адрес с указанной фразой зарегистрирован в системе!");
										Console.WriteLine("Зачисляем средства на счет!");

										Transaction trx = new Transaction("ADMIN", macAddress, getAddress.GetPubKeyHex(), money, 0);

										TCPServer.chain.AddTransactionToPool(trx);

										Thread.Sleep(10000);

										getAddress.Wallet.Deposit(Convert.ToDouble(TCPServer.chain.GetAddressBalance(getAddress.GetPubKeyHex())));
										sendData = JsonConvert.SerializeObject(getAddress.Wallet);
									}
								}
								//sendData = Json
							}
							catch (Exception ex)
							{
								//Console.WriteLine(ex.Message);
								sendData = ex.Message;
							}

							return true;
						}
					//data = secret, money, macAddress
					case Commands.Withdraw:
						{
							Console.WriteLine("Запрос на снятие средств!");

							try
							{
								using (TCPServer.sqlConnection = new SqlConnection(connectionParams))
								{
									TCPServer.sqlConnection.Open();

									string[] parsedData = this.receivedData.Split(';');

									string secret = parsedData[0];
									double money = Convert.ToDouble(parsedData[1].Replace('.', ','));
									string macAddress = parsedData[2];

									Address getAddress = new Address(secret);
									//Console.WriteLine(getAddress.GetPubKeyHex());
									//Console.WriteLine(getAddress.GetPrivateKeyHex());

									SqlCommand select = new SqlCommand("SELECT COUNT(*) FROM Addresses WHERE public_key = @public_key AND private_key = @private_key", TCPServer.sqlConnection);
									select.Parameters.AddWithValue("@public_key", getAddress.GetPubKeyHex());
									select.Parameters.AddWithValue("@private_key", getAddress.GetPrivateKeyHex());
									int count = (int)select.ExecuteScalar();
									Console.WriteLine("Найдено аккаунтов: {0}", count);

									if (count < 1)
									{
										sendData = "ПРОВАЛ! Указанного адреса не существует!";
										throw (new Exception(sendData));
									}
									else
									{
										Console.WriteLine("УСПЕХ! Адрес с указанной фразой зарегистрирован в системе!");

										double balance = Convert.ToDouble(TCPServer.chain.GetAddressBalance(getAddress.GetPubKeyHex())) - money;

										if (balance < 0)
										{
											sendData = "ПРОВАЛ! На кошельке не хватает средств!";
											throw new Exception(sendData);
										}
										else
										{
											Transaction trx = new Transaction(getAddress.GetPubKeyHex(), macAddress, "ADMIN", money, 0);

											TCPServer.chain.AddTransactionToPool(trx);

											Thread.Sleep(10000);

											getAddress.Wallet.Withdraw(Convert.ToDouble(TCPServer.chain.GetAddressBalance(getAddress.GetPubKeyHex())));

											sendData = JsonConvert.SerializeObject(getAddress.Wallet);
										}

									}
								}
								//sendData = Json
							}
							catch (Exception ex)
							{
								//Console.WriteLine(ex.Message);
								sendData = ex.Message;
							}
							return true;
						}
					//data = secret, money, macAddress, receiverPubKey
					case Commands.Transaction:
						{
							Console.WriteLine("Запрос на перевод средств!");

							try
							{
								using (TCPServer.sqlConnection = new SqlConnection(connectionParams))
								{
									TCPServer.sqlConnection.Open();

									string[] parsedData = this.receivedData.Split(';');

									string secret = parsedData[0];
									double money = Convert.ToDouble(parsedData[1].Replace('.', ','));
									string macAddress = parsedData[2];
									string receiverPubKey = parsedData[3];

									Address getAddress = new Address(secret);
									//Console.WriteLine(getAddress.GetPubKeyHex());
									//Console.WriteLine(getAddress.GetPrivateKeyHex());

									SqlCommand select = new SqlCommand("SELECT COUNT(*) FROM Addresses WHERE public_key = @public_key AND private_key = @private_key", TCPServer.sqlConnection);
									select.Parameters.AddWithValue("@public_key", getAddress.GetPubKeyHex());
									select.Parameters.AddWithValue("@private_key", getAddress.GetPrivateKeyHex());
									int count = (int)select.ExecuteScalar();
									Console.WriteLine("Найдено аккаунтов: {0}", count);

									if (count < 1)
									{
										sendData = "ПРОВАЛ! Указанного адреса не существует!";
										throw (new Exception(sendData));
									}
									else
									{
										select = new SqlCommand("SELECT COUNT(*) FROM Addresses WHERE public_key = @public_key", TCPServer.sqlConnection);
										select.Parameters.AddWithValue("@public_key", receiverPubKey);
										count = (int)select.ExecuteScalar();
										Console.WriteLine("Найдено аккаунтов: {0}", count);

										if (count < 1)
										{
											sendData = "ПРОВАЛ! Указанного адреса не существует!";
											throw (new Exception(sendData));
										}
										else
										{
											Console.WriteLine("УСПЕХ! Адрес с указанной фразой зарегистрирован в системе!");

											double balance = Convert.ToDouble(TCPServer.chain.GetAddressBalance(getAddress.GetPubKeyHex())) - money;

											if (balance < 0)
											{
												sendData = "ПРОВАЛ! На кошельке не хватает средств!";
												throw new Exception(sendData);
											}
											else
											{
												Transaction trx = new Transaction(getAddress.GetPubKeyHex(), macAddress, receiverPubKey, money, 0);

												TCPServer.chain.AddTransactionToPool(trx);

												Thread.Sleep(10000);

												getAddress.Wallet.Withdraw(Convert.ToDouble(TCPServer.chain.GetAddressBalance(getAddress.GetPubKeyHex())));
												sendData = JsonConvert.SerializeObject(getAddress.Wallet);
											}
										}

									}
								}
								//sendData = Json
							}
							catch (Exception ex)
							{
								//Console.WriteLine(ex.Message);
								sendData = ex.Message;
							}

							return true;
						}
				}

				return false;
			}
		}

	}
}

