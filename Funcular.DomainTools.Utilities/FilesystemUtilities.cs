using System.IO;

namespace Funcular.DomainTools.Utilities
{
    public static class FilesystemUtilities
    {
        public static DirectoryInfo DirectoryInfo { get; set; }
        /// <summary>
        /// Create the directory if it doesn't exist
        /// </summary>
        /// <param name="directory"></param>
        public static void EnsureExists(this DirectoryInfo directory)
        {
            if (!Directory.Exists(directory.FullName))
                Directory.CreateDirectory(directory.FullName);
        }
    }
}
