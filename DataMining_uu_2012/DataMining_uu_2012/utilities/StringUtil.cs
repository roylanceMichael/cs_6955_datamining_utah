using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012.utilities
{
	using System.IO;
	using System.Reflection;
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


		public static string ReadResource(this string resourceLocation)
		{
			var assembly = Assembly.GetExecutingAssembly();
			using (var d1Stream = assembly.GetManifestResourceStream(resourceLocation))
			{
				if (d1Stream == null)
				{
					return string.Empty;
				}

				using (var stringReader = new StreamReader(d1Stream))
				{
					return stringReader.ReadToEnd();
				}
			}
		}
	}
}
