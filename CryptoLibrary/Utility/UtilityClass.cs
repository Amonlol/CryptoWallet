using System;
using System.Text;
using System.Text.Unicode;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace CryptoLibrary
{
	/// <summary>
	/// Библиотека методов поддержки
	/// </summary>
	public static class UtilityClass
	{
		//задание свойств сериализации
		private static JsonSerializerOptions options = new JsonSerializerOptions
		{
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
			WriteIndented = true,
		};

		//конвертирование строки в массив байтов
		public static byte[] ConvertToByte(this string arg)
		{
			return Encoding.UTF8.GetBytes(arg);
		}
		//конвертирование массива транзакций в массив байтов
		public static byte[] ConvertToByte(this Transaction[] lsTrx)
		{
			string transactionsString = JsonSerializer.Serialize(lsTrx);
			return transactionsString.ConvertToByte();
		}
		//конвертирование байтов в строку
		public static string ConvertToString(this byte[] ba)
		{
			StringBuilder hex = new StringBuilder(ba.Length * 2);
			foreach (byte b in ba)
			{
				hex.AppendFormat("{0:x2}", b);
			}
			return hex.ToString();
		}
		//конвертирование тиков времени в строку даты
		public static string ConvertToString(this Int64 timestamp)
		{

			DateTime myDate = new DateTime(timestamp);
			var strDate = myDate.ToString("dd MMM yyyy hh:mm:ss");
			return strDate;

		}
		//сериализация указаного массива в строку
		public static string ConvertToString<T>(this T[] o)
		{
			return JsonSerializer.Serialize(o, options);
		}
		//сериализация указанного объекта в строку
		public static string ConvertToString<T>(this T o)
		{
			return JsonSerializer.Serialize(o, options);
		}

	}

}