using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Funcular.DomainTools.Utilities;

namespace Funcular.DomainTools.Utilities
{
	public static class Base36Converter
	{
		private static string _charList = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		/// <summary>
		/// The character set for encoding. Defaults to upper-case alphanumerics 0-9, A-Z.
		/// </summary>
		public static string CharList { get { return _charList; } set { _charList = value; } }
		public static string FromHex(string hex)
		{
			BaseConverter.CharList = _charList;
			return BaseConverter.Convert(hex, 16, 36);
		}
		public static string FromGuid(Guid guid)
		{
			BaseConverter.CharList = _charList; 
			return BaseConverter.Convert(guid.ToString("N"), 16, 36);
		}
		public static string FromInt32(long int32)
		{
			BaseConverter.CharList = _charList; 
			return BaseConverter.Convert(int32.ToString(CultureInfo.InvariantCulture), 10, 36);
		}
		public static string FromInt64(long int64)
		{
			BaseConverter.CharList = _charList;
			return BaseConverter.Convert(number: int64.ToString(CultureInfo.InvariantCulture), fromBase: 10, toBase: 36);
		}
		/// <summary>
		/// Encode the given number into a Base36 string
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static String Encode(long input)
		{
			if (input < 0) throw new ArgumentOutOfRangeException("input", input, "input cannot be negative");
			char[] clistarr = CharList.ToCharArray();
			var result = new Stack<char>();
			while (input != 0)
			{
				result.Push(clistarr[input % 36]);
				input /= 36;
			}
			return new string(result.ToArray());
		}

		/// <summary>
		/// Decode the Base36 Encoded string into a number
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static Int64 Decode(string input)
		{
			var reversed = input.ToUpper().Reverse();
			long result = 0;
			int pos = 0;
			foreach (char c in reversed)
			{
				result += CharList.IndexOf(c) * (long)Math.Pow(36, pos);
				pos++;
			}
			return result;
		}
	}
}