using System.IO;

namespace SubrealTeam.Common.Extensions
{
	/// <summary>
	/// Расширения для работы с именем файла
	/// </summary>
	public static class FileNameExtensions
	{
		/// <summary>
		/// Добавить завершающий слешь к пути до файла
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
