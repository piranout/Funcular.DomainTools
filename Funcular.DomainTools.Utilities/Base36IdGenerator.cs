using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Funcular.DomainTools.Utilities
{
    public static class Base36IdGenerator
	{

		/// <summary>
		/// This value should be hardcoded; edit it to reflect the in-service date for your platform:
		/// </summary>
		private static readonly DateTime _inService = DateTime.Parse("2013-01-01 00:00:00.000");
		public static DateTime InServiceDate { get { return _inService; } }
		private static readonly DateTime _lastInitialized = DateTime.Now;
		private static readonly Stopwatch _sw = Stopwatch.StartNew();
		private static readonly MD5 _md5 = MD5.Create();

		private static readonly object _lockObj = new object();
		private static long _lastMicroseconds;
		private static long? _hostSum;
		private static string _hostHash = _hostHash ?? ComputeHostHash();
		// reserved byte, start at the max Base36 value, can decrement 
		// up to 35 times when values are exhausted (every ~115 years),
		// or repurpose as a discriminator if desired:
		private static int _reserved = 35;
		private static bool _shouldCheckReserved;

		public static string HostHash
		{
			get { return _hostHash; }
			// set { IdGenerator._hostHash = value; }
		}
		private static string _reservedHash;
		public static string Reserved { get { return _reservedHash ?? (_reservedHash = Base36Converter.FromInt32(_reserved)); } }

		/// <summary>
		/// Generates a new Base36 identifier with the 11th and 12th bits
		/// replaced by the first two characters of the value supplied for 
		/// <paramref name="overrideHostString"/>.
		/// 
		/// If <paramref name="overrideReservedByte"/> is supplied, the 
		/// reserved 13th bit (normally 'Z') will be replaced by it.
		/// 
		/// Examples of overriding the host bytes might include re-purposing 
		/// those characters for entity type identification, or for synchronizing 
		/// a group of servers to issue ids in the same range. (This last example 
		/// somewhat increases the possibility of a collision by about 1.5432 * 10⁻³). 
		/// </summary>
		/// <param name="overrideHostString"></param>
		/// <returns></returns>
		public static Base36Id NewBase36(string overrideHostString = null, byte? overrideReservedByte = null)
		{
			string tmp = null;
			if (string.IsNullOrEmpty(overrideHostString) && null == overrideReservedByte)
				tmp = NewBase36String();
			byte[] origBase = getBytes(NewBase36String());
			byte[] overrideHostBytes = getBytes(overrideHostString);
			if (overrideHostBytes.Length > 0)
				origBase[10] = overrideHostBytes[0];
			if (overrideHostBytes.Length > 1)
				origBase[11] = overrideHostBytes[1];
			if (overrideReservedByte != null && overrideReservedByte.HasValue)
				origBase[12] = overrideReservedByte.Value;
			tmp = getString(origBase);
			return Parse(tmp);
			//return getString(origBase);
		}

		/// <summary>
		/// Generates a unique, sequential, Base36 string, 16 characters long.
		/// The first 10 characters are the microseconds elapsed since the InService DateTime 
		///		(constant field you hardcode in this file).
		/// The next 2 characters are a compressed checksum of the MD5 of this host. 
		/// The next 1 character is a reserved constant of 36 ('Z' in Base36).
		/// The last 3 characters are random number less than 46655 additional for additional uniqueness.
		/// </summary>
		/// <returns>Returns a unique, sequential, 16-character Base36 string</returns>
		public static string NewBase36String()
		{
			return NewBase36String(null);
		}

		/// <summary>
		/// Generates a unique, sequential, Base36 string, 16 characters long.
		/// The first 10 characters are the microseconds elapsed since the InService DateTime 
		///		(constant field you hardcode in this file).
		/// The next 2 characters are a compressed checksum of the MD5 of this host. 
		/// The next 1 character is a reserved constant of 36 ('Z' in Base36).
		/// The last 3 characters are random number less than 46655 additional for additional uniqueness.
		/// </summary>
		/// <param name="delimiter">If provided, formats the ID as four groups of
		/// 4 characters separated by the delimiter.</param>
		/// <returns>Returns a unique, sequential, 16-character Base36 string</returns>
		public static string NewBase36String(string delimiter)
		{
			// For 10 chars from timestamp, microseconds since InService, uses Stopwatch...
			long microseconds = initStaticsReturnMicroseconds();

			// For 2 chars from host id MD5...
			string hostHash = (_hostHash ?? (_hostHash = ComputeHostHash()));

			// 1 reserved char; static largest base36 digit.
			// If the same ID system, scheme and sequence is still in use 
			// more than 115.85 years after in-service date, decrements
			// 'reserved' by 1 for each whole multiple of 115 years
			// elapsed, up to 35 times max. If the same system, scheme
			// and sequence is still in service 3,850 years from the
			// initial go-live, you probably have bigger problems than 
			// ID collisions...
			int reserved = _reserved;

			// 3 chars random in Base36 = 46656 units
			string rndHexStr = getRandomDigits();
			StringBuilder sb = new StringBuilder();
			sb.Append(Base36Converter.FromInt64(microseconds).PadLeft(10, '0'));
			sb.Length = 10;
			sb.Append(hostHash.PadLeft(2, '0'));
			sb.Length = 12;
			sb.Append(Base36Converter.FromInt32(reserved));
			sb.Length = 13;
			sb.Append(Base36Converter.FromHex(rndHexStr).PadLeft(3, '0'));
			sb.Length = 16;
			if (!string.IsNullOrEmpty(delimiter))
			{
				sb.Insert(12, delimiter);
				sb.Insert(8, delimiter);
				sb.Insert(4, delimiter);
			}
			return sb.ToString();
		}

		private static readonly Regex _regex = new Regex("[^0-9a-zA-Z]", RegexOptions.Compiled);
		/// <summary>
		/// Returns value with all non base 36 characters removed.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Base36Id Parse(string value)
		{
			if (string.IsNullOrEmpty(value))
				throw new ArgumentNullException(paramName: "value");
			string ret = _regex.Replace(value, "").ToUpper();
			if (ret.Length != 16)
				throw new ArgumentException("Value was not a valid Base36 id", "value");

			Base36Id fields = new Base36Id
				{
					Id = ret,
					HostHash = ret.Substring(10, 2),
					Reserved = ret.Substring(12, 1),
					InService = _inService
				};
			int pos = Base36Converter.CharList.IndexOf(fields.Reserved, System.StringComparison.Ordinal);
			int centuryCycles = Math.Max(0, ((35 - pos) - 1));
			// some precision is lost, as chars beyond 10 weren't originally from the timestamp 
			// component of the ID:
			int probableEncodedLength =
				Base36Converter.FromInt64(InServiceDate.AddDays(365.25 * 115.89 * centuryCycles).Ticks / 10).Length;
			if (centuryCycles == 0)
				probableEncodedLength = 10;
			string encodedMs = ret.Substring(0, probableEncodedLength);
			long ms = Base36Converter.Decode(encodedMs);
			fields.Microseconds = ms;
			fields.CreatedRoughly = _inService.AddTicks(ms * 10); //.AddTicks((3656158440062976 * 10 + 9) * centuryCycles);
			return fields;
		}
		private static long initStaticsReturnMicroseconds()
		{
			long microseconds = GetMicroseconds();
			// init server name checksum...
			_hostHash = (_hostHash ?? (_hostHash = ComputeHostHash()));
			// decrement reserved byte every 115 years...
			_reserved = _shouldCheckReserved ? 35 - Convert.ToInt32(microseconds / 3656158440062975) : _reserved;
			return microseconds;
		}

		/// <summary>
		/// Return the elapsed microseconds since the in-service DateTime; will never
		/// return the same value twice. Uses a high-resolution Stopwatch (not DateTime.Now)
		/// to measure durations, and uses shared memory to ensure uniqueness.
		/// </summary>
		/// <returns></returns>
		public static long GetMicroseconds()
		{
			long microseconds;
			lock (_lockObj)
			{
				do
				{
					microseconds = (_lastInitialized.Subtract(_inService).Add(_sw.Elapsed).Ticks) / 10;
				} while (microseconds <= _lastMicroseconds + 9); // max 1 per microsecond
				_lastMicroseconds = microseconds;
			}
			_shouldCheckReserved = _lastMicroseconds > TimeSpan.FromDays(365.25 * 115).Ticks / 10;
			return microseconds;
		}

		static readonly Random _rnd = new Random();
		/// <summary>
		/// Returns a random Base36 number 3 characters long. 
		/// </summary>
		/// <returns></returns>
		private static string getRandomDigits()
		{
			Int32 rndInt;
			lock (_lockObj)
			{
				rndInt = _rnd.Next(46655);
			}
			return Base36Converter.Encode(rndInt);
		}

		/// <summary>
		/// Compresses the MD5 of this server's hostname by summing the bytes until the sum
		/// fits into two Base36 characters (<= 36^2, or 1296):
		/// </summary>
		/// <returns>2 character Base36 checksum of MD5 of hostname</returns>
		public static string ComputeHostHash()
		{
			string hash = Base36Converter.Encode(
					(_hostSum.HasValue ? _hostSum.Value : (
							_hostSum = new long?(getBytesSum(computeMd5(getHostBytes()), 36, 2))).Value));
			return hash;
		}
		/// <summary>
		/// For internal use; use the parameterless overload for effeciency.
		/// </summary>
		/// <param name="host"></param>
		/// <returns></returns>
		public static string ComputeHostHash(string host)
		{
			long sum = new long?(getBytesSum(computeMd5(host), 36, 2)).Value;
			string hash = Base36Converter.Encode(sum);
			return hash;
		}

		private static byte[] computeMd5(string value)
		{
			return _md5.ComputeHash(getBytes(value));
		}

		private static byte[] computeMd5(byte[] value)
		{
			return _md5.ComputeHash(value);
		}

		/// <summary>
		/// Recursively sums a byte array until the value will fit within
		/// <paramref name="maxChars"/> characters in the base specified by
		/// <paramref name="forBase"/>.
		/// </summary>
		/// <param name="val">The byte array to checksum</param>
		/// <param name="maxChars">The max number of characters to hold the checksum</param>
		/// <param name="forBase">The base in which the checksum will be expressed</param>
		/// <returns></returns>
		private static long getBytesSum(byte[] val, int forBase, int maxChars)
		{
			long maxVal = Convert.ToInt64(Math.Pow(forBase, maxChars));
			byte[] arr = val;
			long tmp;
			do
			{
				tmp = 0;
				foreach (byte t in arr)
				{
					tmp += t;
				}
				arr = BitConverter.GetBytes(tmp);
			} while (tmp > maxVal);
			return tmp; // arr.Where(b => b != 0).ToArray();
		}

		/// <summary>
		/// Returns a byte array containing the HostName if available, or
		/// else the mac address of the fastest non-loopback, non-tunnel 
		/// NIC available. If neither can be determined, will default to an 
		/// array of 6 bytes all set to 35.
		/// </summary>
		/// <returns></returns>
		private static byte[] getHostBytes()
		{
			const int MIN_MAC_ADDR_LENGTH = 6;
			byte[] defaultBytes = new byte[] { 35, 35, 35, 35, 35, 35 };
			byte[] macBytes = new byte[] { 35, 35, 35, 35, 35, 35 };
			byte[] retBytes = new byte[] { 0 };
			byte[] hostnameBytes = getBytes(System.Net.Dns.GetHostName());
			if (hostnameBytes.Length > 3)
			{
				retBytes = hostnameBytes;
			}
			else
			{
				try
				{
					List<NetworkInterface> candidateInterfaces =
						NetworkInterface.GetAllNetworkInterfaces().ToList().Where(nw =>
							nw.NetworkInterfaceType != NetworkInterfaceType.Loopback
							&& nw.NetworkInterfaceType != NetworkInterfaceType.Tunnel
							&& nw.OperationalStatus == OperationalStatus.Up
							&& nw.GetPhysicalAddress().GetAddressBytes().Length >= MIN_MAC_ADDR_LENGTH)
						.ToList();
					macBytes = candidateInterfaces.Max(nw => nw.GetPhysicalAddress().GetAddressBytes());
					if (macBytes.Length >= 6)
						retBytes = macBytes;
				}
				catch (Exception ex)
				{ // back to default
					Trace.WriteLine(ex);
					macBytes = defaultBytes;
					retBytes = macBytes;
				}
			}
			if ((retBytes ?? (retBytes = defaultBytes)).Length < 6)
				retBytes = retBytes.Union(defaultBytes).ToArray();
			return retBytes;
		}
		/// <summary>
		/// Shorthand for Encoding.Default.GetBytes
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private static byte[] getBytes(string str)
		{
			return Encoding.Default.GetBytes(str);
		}
		/// <summary>
		/// Shorthand for Encoding.Default.GetString
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		private static string getString(byte[] bytes)
		{
			return Encoding.Default.GetString(bytes);
		}
	}

}

