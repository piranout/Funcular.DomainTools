using System;
using System.Collections.Generic;
using System.Linq;

namespace Funcular.DomainTools.Utilities
{
	/// <summary>
	/// A Base36 De- and Encoder
	/// </summary>
	/// <remarks>
	/// Portions of this adapted from the base36 encoder at
	/// http://www.stum.de/2008/10/20/base36-encoderdecoder-in-c/
	/// </remarks>
	public static class BaseConverter
	{
		private static string _charList; // "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		/// <summary>
		/// The character set for encoding. Defaults to upper-case alphanumerics 0-9, A-Z.
		/// </summary>
		public static string CharList { get { return _charList; } set { _charList = value; } }

		public static string Convert(string number, int fromBase, int toBase)
		{
			if (string.IsNullOrEmpty(_charList))
				throw new FormatException("You must populate .CharList before calling Convert().");
			// var digits = "0123456789abcdefghijklmnopqrstuvwxyz";
			int length = number.Length;
			string result = string.Empty;
			List<int> nibbles = number.Select(c => CharList.IndexOf(c)).ToList();
			int newlen;
			do
			{
				int value = 0;
				newlen = 0;
				for (var i = 0; i < length; ++i)
				{
					value = value * fromBase + nibbles[i];
					if (value >= toBase)
					{
						if (newlen == nibbles.Count)
						{
							nibbles.Add(0);
						}
						nibbles[newlen++] = value / toBase;
						value %= toBase;
					}
					else if (newlen > 0)
					{
						if (newlen == nibbles.Count)
						{
							nibbles.Add(0);
						}
						nibbles[newlen++] = 0;
					}
				}
				length = newlen;
				result = CharList[value] + result; //
			}
			while (newlen != 0);
			return result;
		}
	}
}
