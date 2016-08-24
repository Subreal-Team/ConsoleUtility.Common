using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubrealTeam.Windows.Common.Extensions
{
	public static class FileNameExtensions
	{
		public static string AddTrailingSlash(this string path)
		{
			if (path.IsEmpty()) return path;

			return !path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? path + Path.DirectorySeparatorChar : path;
		}
	}
}
