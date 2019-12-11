using System;

namespace Funcular.DomainTools.Utilities
{
	public static class SequentialGuidGenerator
	{

		/// <summary>
		/// Generate a sequential GUID. This is a wrapper around the UuidCreateSequential 
		/// method. If it cannot generate a GUID using that, then it will fall back to generating
		/// a plain old Guid using Guid.NewGuid.
		/// This method is used to generate sequential GUIDs similar to how CRM
		/// does it.
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		[System.Runtime.InteropServices.DllImport(@"rpcrt4.dll", SetLastError = true)]
		internal static extern int UuidCreateSequential(out Guid guid);
		public static Guid GenerateSequentialGuid()
		{
			const int RPC_S_OK = 0;
			Guid guid;
			Int32 retvalue = UuidCreateSequential(out guid);
			if (retvalue != RPC_S_OK)
			{
				//If cannot generate a sequential guid, then create a standard guid
				//throw new ApplicationException("UuidCreateSequential failed: " + retvalue);
				guid = Guid.NewGuid();
			}
			return guid;
		}
		/// <summary>
		/// Generates a new sequential guid (using rpcrt4.dll's UuidCreateSequential).
		/// Use like this: var id = Guid.Empty.NewSequential();
		/// (Would be prettier if static extension methods were supported.)
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static Guid NewSequential(this Guid instance)
		{
			return SequentialGuidGenerator.GenerateSequentialGuid();
		}
	}
}

