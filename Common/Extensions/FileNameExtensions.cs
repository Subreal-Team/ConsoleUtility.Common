using System.IO;

namespace SubRealTeam.ConsoleUtility.Common.Extensions
{
    /// <summary>
    /// File Name Extensions
    /// </summary>
    public static class FileNameExtensions
    {
        /// <summary>
        /// Add trailing to the file path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string AddTrailingSlash(this string path)
        {
            if (path.IsEmpty()) return path;

            return !path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? path + Path.DirectorySeparatorChar : path;
        }
    }
}
