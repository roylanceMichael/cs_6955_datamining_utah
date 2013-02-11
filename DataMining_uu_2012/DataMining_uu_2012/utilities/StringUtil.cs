using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012.utilities
{
	using System.Text.RegularExpressions;

	public static class StringUtil
	{
		public static string CleanseFileName(this string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				return string.Empty;
			}

			return Regex.Replace(fileName, "[^0-9A-Za-z]", "_");
		}
	}
}
