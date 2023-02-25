using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;

namespace CryptoLibrary
{
	class ChainDB : DbContext
	{
		public ChainDB() : base("BLOCKCHAIN")
		{

		}

		public DbSet<Block> Blocks { get; set; }
	}
}
