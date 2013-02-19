using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012.hw2
{
	using System.IO;
	using System.Reflection;
	using System.Text.RegularExpressions;

	public class Hw2
	{
		public const string WhiteSpaceRegex = "\\s+";

		public IList<Document> Documents { get; private set; } 

		public Document D1
		{
			get
			{
				return Documents[0];
			}
		}

		public Document D2
		{
			get
			{
				return Documents[1];
			}
		}

		public Document D3
		{
			get
			{
				return Documents[2];
			}
		}

		public Document D4
		{
			get
			{
				return Documents[3];
			}
		}

		public Hw2()
		{
			Documents = new List<Document>();
			var d1 = new Document { D = ReadResource("DataMining_uu_2012.hw2.D1.txt") };
			var d2 = new Document { D = ReadResource("DataMining_uu_2012.hw2.D2.txt") };
			var d3 = new Document { D = ReadResource("DataMining_uu_2012.hw2.D3.txt") };
			var d4 = new Document { D = ReadResource("DataMining_uu_2012.hw2.D4.txt") };

			Documents.Add(d1);
			Documents.Add(d2);
			Documents.Add(d3);
			Documents.Add(d4);

			d1.UnigramsWords = CalculateUnigramsWords(d1.D);
			d1.BigramsWords = CalculateBigramsWords(d1.D);
			d1.TrigramsWords = CalculateTrigramsWords(d1.D);
			d1.BigramChars = CalculateBigramsChar(d1.D);
			d1.TrigramChars = CalculateTrigramsChar(d1.D);

			d2.UnigramsWords = CalculateUnigramsWords(d2.D);
			d2.BigramsWords = CalculateBigramsWords(d2.D);
			d2.TrigramsWords = CalculateTrigramsWords(d2.D);
			d2.BigramChars = CalculateBigramsChar(d2.D);
			d2.TrigramChars = CalculateTrigramsChar(d2.D);

			d3.UnigramsWords = CalculateUnigramsWords(d3.D);
			d3.BigramsWords = CalculateBigramsWords(d3.D);
			d3.TrigramsWords = CalculateTrigramsWords(d3.D);
			d3.BigramChars = CalculateBigramsChar(d3.D);
			d3.TrigramChars = CalculateTrigramsChar(d3.D);

			d4.UnigramsWords = CalculateUnigramsWords(d4.D);
			d4.BigramsWords = CalculateBigramsWords(d4.D);
			d4.TrigramsWords = CalculateTrigramsWords(d4.D);
			d4.BigramChars = CalculateBigramsChar(d4.D);
			d4.TrigramChars = CalculateTrigramsChar(d4.D);
		}

		public static double JaccardUnigramWords(Dictionary<string, int> d1, Dictionary<string, int> d2)
		{
			var intersection = Math.Abs(d1.Keys.Count(d2.ContainsKey));
			var uniqueKeys = new List<string>();

			foreach (var item in d1.Keys.Where(item => !uniqueKeys.Contains(item)))
			{
				uniqueKeys.Add(item);
			}

			foreach (var item in d2.Keys.Where(item => !uniqueKeys.Contains(item)))
			{
				uniqueKeys.Add(item);
			}

			var union = Math.Abs(uniqueKeys.Count);
			return (double)intersection / union;
		}

		public static double JaccardBigramWords(Dictionary<Tuple<string, string>, int> d1, Dictionary<Tuple<string, string>, int> d2)
		{
			var intersection = 0;
			foreach (var d1Key in d1.Keys)
			{
				var foundKey = d2.Keys.FirstOrDefault(t => t.Item1 == d1Key.Item1 && t.Item2 == d1Key.Item2);
				if (foundKey != null)
				{
					intersection++;
				}
			}

			var uniqueKeys = new List<Tuple<string, string>>();

			foreach (var item in d1.Keys.Where(item => !uniqueKeys.Any(t => t.Item1 == item.Item1 && t.Item2 == item.Item2)))
			{
				uniqueKeys.Add(item);
			}

			foreach (var item in d2.Keys.Where(item => !uniqueKeys.Any(t => t.Item1 == item.Item1 && t.Item2 == item.Item2)))
			{
				uniqueKeys.Add(item);
			}

			var union = uniqueKeys.Count;
			return (double)intersection / union;
		}

		public static double JaccardBigramChars(Dictionary<Tuple<char, char>, int> d1, Dictionary<Tuple<char, char>, int> d2)
		{
			var intersection = 0;
			foreach (var d1Key in d1.Keys)
			{
				var foundKey = d2.Keys.FirstOrDefault(t => t.Item1 == d1Key.Item1 && t.Item2 == d1Key.Item2);
				if (foundKey != null)
				{
					intersection++;
				}
			}

			var uniqueKeys = new List<Tuple<char, char>>();

			foreach (var item in d1.Keys.Where(item => !uniqueKeys.Any(t => t.Item1 == item.Item1 && t.Item2 == item.Item2)))
			{
				uniqueKeys.Add(item);
			}

			foreach (var item in d2.Keys.Where(item => !uniqueKeys.Any(t => t.Item1 == item.Item1 && t.Item2 == item.Item2)))
			{
				uniqueKeys.Add(item);
			}

			var union = uniqueKeys.Count;
			return (double)intersection / union;
		}
		
		public static double JaccardTrigramWords(Dictionary<Tuple<string, string, string>, int> d1, Dictionary<Tuple<string, string, string>, int> d2)
		{
			var intersection = 0;
			foreach (var d1Key in d1.Keys)
			{
				var foundKey = d2.Keys
					.FirstOrDefault(t => t.Item1 == d1Key.Item1 && t.Item2 == d1Key.Item2 && t.Item3 == d1Key.Item3);
				if (foundKey != null)
				{
					intersection++;
				}
			}


			var uniqueKeys = new List<Tuple<string, string, string>>();

			foreach (var item in d1.Keys.Where(item => !uniqueKeys
				.Any(t => t.Item1 == item.Item1 && t.Item2 == item.Item2 && t.Item3 == item.Item3)))
			{
				uniqueKeys.Add(item);
			}

			foreach (var item in d2.Keys.Where(item => !uniqueKeys
				.Any(t => t.Item1 == item.Item1 && t.Item2 == item.Item2 && t.Item3 == item.Item3)))
			{
				uniqueKeys.Add(item);
			}

			var union = uniqueKeys.Count;
			return (double)intersection / union;
		}

		public static double JaccardTrigramChars(Dictionary<Tuple<char, char, char>, int> d1, Dictionary<Tuple<char, char, char>, int> d2)
		{
			var intersection = 0;
			foreach (var d1Key in d1.Keys)
			{
				var foundKey = d2.Keys
					.FirstOrDefault(t => t.Item1 == d1Key.Item1 && t.Item2 == d1Key.Item2 && t.Item3 == d1Key.Item3);
				if (foundKey != null)
				{
					intersection++;
				}
			}


			var uniqueKeys = new List<Tuple<char, char, char>>();

			foreach (var item in d1.Keys.Where(item => !uniqueKeys
				.Any(t => t.Item1 == item.Item1 && t.Item2 == item.Item2 && t.Item3 == item.Item3)))
			{
				uniqueKeys.Add(item);
			}

			foreach (var item in d2.Keys.Where(item => !uniqueKeys
				.Any(t => t.Item1 == item.Item1 && t.Item2 == item.Item2 && t.Item3 == item.Item3)))
			{
				uniqueKeys.Add(item);
			}

			var union = uniqueKeys.Count;
			return (double)intersection / union;
		}

		private static Dictionary<Tuple<char, char>, int> CalculateBigramsChar(string resource)
		{
			var splitResource = resource.ToCharArray();
			var returnDict = new Dictionary<Tuple<char, char>, int>();
			for (var i = 0; i < splitResource.Length; i++)
			{
				if (i + 1 == splitResource.Length)
				{
					continue;
				}
				var bigram = new Tuple<char, char>(splitResource[i], splitResource[i + 1]);

				var foundKey = returnDict.Keys.FirstOrDefault(t => t.Item1 == bigram.Item1 && t.Item2 == bigram.Item2);
				if (foundKey == null)
				{
					returnDict[bigram] = 0;
				}
				else
				{
					returnDict[bigram]++;
				}
			}
			return returnDict;
		}

		private static Dictionary<Tuple<char, char, char>, int> CalculateTrigramsChar(string resource)
		{
			var splitResource = resource.ToCharArray();
			var returnDict = new Dictionary<Tuple<char, char, char>, int>();
			for (var i = 0; i < splitResource.Length; i++)
			{
				if (i + 1 == splitResource.Length || i + 2 >= splitResource.Length)
				{
					continue;
				}

				var trigram = new Tuple<char, char, char>(splitResource[i], splitResource[i + 1], splitResource[i + 2]);

				var foundKey = returnDict.Keys.FirstOrDefault(t => t.Item1 == trigram.Item1 && t.Item2 == trigram.Item2 && t.Item3 == trigram.Item3);
				if (foundKey == null)
				{
					returnDict[trigram] = 0;
				}
				else
				{
					returnDict[trigram]++;
				}
			}
			return returnDict;
		}

		private static Dictionary<string, int> CalculateUnigramsWords(string resource)
		{
			var splitResource = Regex.Split(resource, WhiteSpaceRegex);
			var returnDict = new Dictionary<string, int>();
			foreach (var r in splitResource)
			{
				if (returnDict.ContainsKey(r))
				{
					returnDict[r] = returnDict[r] + 1;
				}
				else
				{
					returnDict[r] = 1;
				}
			}
			return returnDict;
		}

		private static Dictionary<Tuple<string, string>, int> CalculateBigramsWords(string resource)
		{
			var splitResource = Regex.Split(resource, WhiteSpaceRegex);
			var bigrams = new Dictionary<Tuple<string, string>, int>();
			for (var i = 0; i < splitResource.Length; i++)
			{
				if (i + 1 == splitResource.Length)
				{
					continue;
				}
				var biGram = new Tuple<string, string>(splitResource[i], splitResource[i + 1]);

				var foundKey = bigrams.Keys.FirstOrDefault(t => t.Item1 == biGram.Item1 && t.Item2 == biGram.Item2);
				if (foundKey == null)
				{
					bigrams[biGram] = 1;
				}
				else
				{
					bigrams[foundKey] = bigrams[foundKey] + 1;
				}
			}
			return bigrams;
		}

		private static Dictionary<Tuple<string, string, string>, int> CalculateTrigramsWords(string resource)
		{
			var splitResource = Regex.Split(resource, WhiteSpaceRegex);
			var trigrams = new Dictionary<Tuple<string, string, string>, int>();
			for (var i = 0; i < splitResource.Length; i++)
			{
				if (i + 1 == splitResource.Length || i + 2 >= splitResource.Length)
				{
					continue;
				}
				var triGram = new Tuple<string, string, string>(splitResource[i], splitResource[i + 1], splitResource[i + 2]);

				var foundKey =
					trigrams.Keys.FirstOrDefault(t => t.Item1 == triGram.Item1 && t.Item2 == triGram.Item2 && t.Item3 == triGram.Item3);

				if (foundKey == null)
				{
					trigrams[triGram] = 1;
				}
				else
				{
					trigrams[foundKey] = trigrams[foundKey] + 1;
				}
			}
			return trigrams;
		}

		private static string ReadResource(string resourceLocation)
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
