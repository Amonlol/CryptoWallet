using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoLibrary;

namespace Server
{
	class Program
	{
		static void Main(string[] args)
		{
			Network.TCPServer server = new Network.TCPServer();
			server.Initialize();
			server.Listen(); 

			Console.ReadLine();
		}
	}
}
