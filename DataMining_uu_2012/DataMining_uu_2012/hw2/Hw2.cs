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

		public string D1 { get; private set; }
		public Dictionary<string, int> D1Unigrams { get; private set; }
		public Dictionary<Tuple<string, string>, int> D1Bigrams { get; private set; }
		public Dictionary<Tuple<string, string, string>, int> D1Trigrams { get; private set; }

		public string D2 { get; private set; }
		public Dictionary<string, int> D2Unigrams { get; private set; }
		public Dictionary<Tuple<string, string>, int> D2Bigrams { get; private set; }
		public Dictionary<Tuple<string, string, string>, int> D2Trigrams { get; private set; }

		public string D3 { get; private set; }
		public Dictionary<string, int> D3Unigrams { get; private set; }
		public Dictionary<Tuple<string, string>, int> D3Bigrams { get; private set; }
		public Dictionary<Tuple<string, string, string>, int> D3Trigrams { get; private set; }

		public string D4 { get; private set; }
		public Dictionary<string, int> D4Unigrams { get; private set; }
		public Dictionary<Tuple<string, string>, int> D4Bigrams { get; private set; }
		public Dictionary<Tuple<string, string, string>, int> D4Trigrams { get; private set; }

		public Hw2()
		{
			this.D1 = ReadResource("DataMining_uu_2012.hw2.D1.txt");
			this.D2 = ReadResource("DataMining_uu_2012.hw2.D2.txt");
			this.D3 = ReadResource("DataMining_uu_2012.hw2.D3.txt");
			this.D4 = ReadResource("DataMining_uu_2012.hw2.D4.txt");

			D1Unigrams = CalculateUnigrams(this.D1);
			D1Bigrams = CalculateBigrams(this.D1);
			D1Trigrams = CalculateTrigrams(this.D1);

			D2Unigrams = CalculateUnigrams(this.D2);
			D2Bigrams = CalculateBigrams(this.D2);
			D2Trigrams = CalculateTrigrams(this.D2);

			D3Unigrams = CalculateUnigrams(this.D3);
			D3Bigrams = CalculateBigrams(this.D3);
			D3Trigrams = CalculateTrigrams(this.D3);

			D4Unigrams = CalculateUnigrams(this.D4);
			D4Bigrams = CalculateBigrams(this.D4);
			D4Trigrams = CalculateTrigrams(this.D4);
		}

		public static double JaccardUnigram(Dictionary<string, int> d1, Dictionary<string, int> d2)
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

		public static double JaccardBigram(Dictionary<Tuple<string, string>, int> d1, Dictionary<Tuple<string, string>, int> d2)
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

		public static double JaccardTrigram(Dictionary<Tuple<string, string, string>, int> d1, Dictionary<Tuple<string, string, string>, int> d2)
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

		private static Dictionary<string, int> CalculateUnigrams(string resource)
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

		private static Dictionary<Tuple<string, string>, int> CalculateBigrams(string resource)
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

		private static Dictionary<Tuple<string, string, string>, int> CalculateTrigrams(string resource)
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
