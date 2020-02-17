using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Humanizer;

namespace Funcular.DomainTools.Utilities
{
	public static class StringHelpers
	{
		private static string _additionalCollapseTokens;
	/*	private static string _preserveQuotedDelimiters;*/

		// ReSharper disable InconsistentNaming
		/// <summary>
		/// Intentionally cased differently to demonstrate the enum value's meaning
		/// </summary>
		public enum ChangeCaseTypes
		{
			camelCase,
			PascalCase,
			lowercase,
			UPPERCASE,
			unchanged
		}

		public static string AdditionalCollapseTokens
		{
			get { return _additionalCollapseTokens; }
			set { _additionalCollapseTokens = value; }
		}


		public static string EnsureEndsWith(this string value, string endWith)
		{
			return value.EndsWith(endWith) ? value : value + endWith;
		}

		public static string EnsureStartsWith(this string value, string startWith)
		{
			return value.StartsWith(startWith) ? value : startWith + value;
		}

		// ReSharper restore InconsistentNaming

		/// <summary>
		/// Create an md5 sum string of this string. Sourced from
		/// http://blog.stevex.net/index.php/c-code-snippet-creating-an-md5-hash-string/.
		/// Requires using statements for System.Text and System.Security.Cryptography;
		/// </summary>
		public static string GetMd5Sum(string str)
		{
			// First we need to convert the string into bytes, which
			// means using a text encoder.
			Encoder enc = System.Text.Encoding.Unicode.GetEncoder();

			// Create a buffer large enough to hold the string
			byte[] unicodeText = new byte[str.Length * 2];
			enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);

			// Now that we have a byte array we can ask the CSP to hash it
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] result = md5.ComputeHash(unicodeText);

			// Build the final string by converting each byte
			// into hex and appending it to a StringBuilder
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < result.Length; i++)
			{
				sb.Append(result[i].ToString("X2"));
			}

			// And return it
			return sb.ToString();
		}
		public static string FromBase64(string input)
		{
			return Encoding.ASCII.GetString(Convert.FromBase64String(input));
		}
		public static string ToBase64(this string input)
		{
			return Convert.ToBase64String(Encoding.ASCII.GetBytes(input));
		}
		private static string fixAtom(int ordinal, string atom, ChangeCaseTypes newCase, bool useStrictSuffixConformity)
		{
			if (ordinal < 0)
				throw new IndexOutOfRangeException("ordinal must be >= 0");
			switch (newCase)
			{
				case ChangeCaseTypes.PascalCase:
			        var s = atom.Pascalize();
			        if (s.EndsWith(value: "ID", comparisonType: StringComparison.Ordinal))
			            s = s.Substring(0, atom.LastIndexOf("ID", StringComparison.Ordinal)) + "Id";
			        return s;// PascalCase(atom, useStrictSuffixConformity);
				case ChangeCaseTypes.lowercase:
					return atom.ToLower();
				case ChangeCaseTypes.unchanged:
					return atom;
				case ChangeCaseTypes.UPPERCASE:
					return atom.ToUpper();
				case ChangeCaseTypes.camelCase:
			        return atom.Camelize();
                    //return (useStrictSuffixConformity ?
                    //    (ordinal < 1 ? atom.ToLower() : PascalCase(atom, useStrictSuffixConformity)) :
                    //    (ordinal < 1 ? atom.Substring(0, 1).ToLower() + atom.Substring(1) : PascalCase(atom, useStrictSuffixConformity)));
				default:
					return atom;
			}
		}
		public static string Collapse(this string originalString, ChangeCaseTypes newCase, bool useStrictSuffixConformity, params string[] delimiterStrings)
		{
			if (string.IsNullOrEmpty(originalString))
				return originalString;
			string[] temp = originalString.Split(delimiterStrings, StringSplitOptions.RemoveEmptyEntries);
			string atoms = "";
			for (int i = 0; i < temp.Length; i++)
			{
				string s = temp[i];
				atoms += fixAtom(i, s, newCase, false);
			}
			return atoms;
		}
		public static string Collapse(this string originalString, ChangeCaseTypes newCase, bool useStrictSuffixConformity, params char[] delimiterChars)
		{
			if (string.IsNullOrEmpty(originalString))
				return originalString;
			string[] temp = originalString.Split(delimiterChars);
			string atoms = "";
			for (int i = 0; i < temp.Length; i++)
			{
				string s = temp[i];
				atoms += fixAtom(i, s, newCase, false);
			}
			return atoms;
		}
		public static string Collapse(this string originalString, ChangeCaseTypes newCase, string delimiterCharacters)
		{
			if (string.IsNullOrEmpty(originalString))
				return originalString;
			return Collapse(originalString, newCase, false, delimiterCharacters.ToCharArray());
		}
		/// <summary>
		/// Returns originalString with first letter lower-cased, i.e. CamelCase becomes camelCase
		/// </summary>
		/// <param name="originalString"></param>
		/// <returns>originalString with first letter in lower case</returns>
		public static string CamelCase(this string originalString)
		{
			if (string.IsNullOrEmpty(originalString))
				return originalString;
			return originalString.Substring(0, 1).ToLower() + originalString.Substring(1);
		}
		public static string PascalCase(this string originalString, bool useStrictSuffixConformity = false)
		{
			if (string.IsNullOrEmpty(originalString))
				return originalString;
			if (useStrictSuffixConformity)
				return originalString.Substring(0, 1).ToUpper() + originalString.Substring(1).ToLower();
			else
				return originalString.Substring(0, 1).ToUpper() + originalString.Substring(1);
		}
		public static string LeftOfFirst(this string originalString, string ofString)
		{
			if (string.IsNullOrEmpty(originalString))
				return originalString;
			int idx = originalString.IndexOf(ofString, System.StringComparison.Ordinal);
			return (idx >= 0 ? originalString.Substring(0, idx) : "");
		}
		public static string RightOfLast(this string originalString, string ofString)
		{
			if (string.IsNullOrEmpty(originalString))
				return originalString;
			int idx = originalString.LastIndexOf(ofString, System.StringComparison.Ordinal);
			return (idx < 0 ?
			"" :
			originalString.Substring(idx + ofString.Length, originalString.Length - (idx + ofString.Length)));
		}
        public static string LeftOfLast(this string originalString, string ofString)
        {
            if (string.IsNullOrEmpty(originalString))
                return originalString;
            int idx = originalString.LastIndexOf(ofString, System.StringComparison.Ordinal);
            return (idx < 0 ?
                "" :
                originalString.Substring(0, idx + 1));
        }
		public static string RightOfFirst(this string originalString, string ofString)
		{
			if (string.IsNullOrEmpty(originalString))
				return originalString;
			int idx = originalString.IndexOf(ofString, System.StringComparison.Ordinal);
			return (idx < 0 ?
			"" :
			originalString.Substring(idx + 1, originalString.Length - (idx + 1)));
		}
		/// <summary>
		/// Remove all occurences of any string in <paramref name="stringsToRemove"/>
		/// </summary>
		/// <param name="originalString"></param>
		/// <param name="stringsToRemove"></param>
		/// <returns></returns>
		public static string RemoveAll(this string originalString, string[] stringsToRemove)
		{
			if (string.IsNullOrEmpty(originalString) || !stringsToRemove.HasContents())
				return originalString;
			string tmp = originalString;
			foreach (string s in stringsToRemove)
			{
				tmp = tmp.Replace(s, "");
			}
			return tmp;
		}

		/// <summary>
		/// Remove all occurences of any char in <paramref name="charsToRemove"/>
		/// </summary>
		/// <param name="originalString"></param>
		/// <param name="charsToRemove"></param>
		/// <returns></returns>
		public static string RemoveAll(this string originalString, char[] charsToRemove)
		{
			if (string.IsNullOrEmpty(originalString) || charsToRemove.Length < 1)
				return originalString;
			int pos = originalString.IndexOfAny(charsToRemove);
			string tmp = originalString;
			while (pos >= 0)
			{
				tmp = tmp.Remove(pos, 1);
				pos = tmp.IndexOfAny(charsToRemove);
			}
			return tmp;
		}
		/// <summary>
		/// If originalString starts with stripString, returns originalString with stripString removed from beginning. 
		/// </summary>
		/// <param name="originalString"></param>
		/// <param name="stripString"></param>
		/// <returns></returns>
		public static string StripLeft(this string originalString, string stripString)
		{
			if (string.IsNullOrEmpty(originalString))
				return originalString;
			if (originalString.StartsWith(stripString, StringComparison.CurrentCultureIgnoreCase))
			{
				return originalString.Substring(stripString.Length);
			}
			return originalString;
		}
		/// <summary>
		/// If originalString ends with stripString, returns originalString with stripString removed from end. 
		/// </summary>
		/// <param name="originalString"></param>
		/// <param name="stripString"></param>
		/// <returns></returns>
		public static string StripRight(this string originalString, string stripString)
		{
			if (string.IsNullOrEmpty(originalString))
				return originalString;
			if (originalString.EndsWith(stripString, StringComparison.CurrentCultureIgnoreCase))
			{
				return originalString.Substring(0, originalString.Length - stripString.Length);
			}
			return originalString;
		}

		/// <summary>
		/// Truncates the string if it exceeds the max length
		/// </summary>
		/// <param name="value"></param>
		/// <param name="maxLength"></param>
		/// <returns></returns>
		public static string Truncate(this string value, int maxLength)
		{
			return value.Length <= maxLength ? value : value.Substring(0, maxLength);
		}

		/// <summary>
		/// If value ends with digits, returns the integral numeric 
		/// portion of the string's end; otherwise, returns -1.
		/// </summary>
		public static int GetIntegerSuffix(this string value)
		{
			int ret = -1;
			if (!string.IsNullOrWhiteSpace(value))
			{
				StringBuilder sb = new StringBuilder();
				var chars = value.ToCharArray();
				for (int i = chars.Length - 1; i >= 0; i--)
				{
					if (char.IsDigit(chars[i]))
						sb.Insert(0, chars[i]);
					else
						break;
				}
				int parsed;
				if (int.TryParse(sb.ToString(), out parsed))
					ret = parsed;
			}
			return ret;
		}

		/// <summary>
		/// If value is all digits, returns true, otherwise false.
		/// </summary>
		public static bool IsInteger(this string value)
		{
			bool ret = false;
			if (!string.IsNullOrWhiteSpace(value))
			{
				var chars = value.ToCharArray();
				ret = !chars.Any(c => !Char.IsDigit(c));
			}
			return ret;
		}

		public static bool HasValue(this string value, bool nonWhitespaceOnly = false)
		{
		    return nonWhitespaceOnly ? string.IsNullOrWhiteSpace(value) == false : string.IsNullOrEmpty(value) == false;
		}
	}
}
