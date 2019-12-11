using System;
using System.Text;

namespace Funcular.DomainTools.Utilities
{
    /// <summary>
    /// A class to help display the Id's components 
    /// </summary>
    public struct Base36Id
    {
        public string Id;
        public string Reserved;
        public string HostHash;
        public long Microseconds;
        public DateTime InService;
        public DateTime CreatedRoughly;

        public string ShowFields()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("          Id:\t{0}\r\n", this.Id);
            sb.AppendFormat("   InService:\t{0}\r\n", this.InService);
            sb.AppendFormat("Microseconds:\t{0}\r\n", this.Microseconds);
            sb.AppendFormat("    HostHash:\t{0}\r\n", this.HostHash);
            sb.AppendFormat("    Reserved:\t{0}\r\n", this.Reserved);
            sb.AppendFormat(" EstCreated~:\t{0}\r\n", this.CreatedRoughly);
            return sb.ToString();
        }
        public static Base36Id NewBase36Id()
        {
            return Base36IdGenerator.NewBase36();
        }
    }
}