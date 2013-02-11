using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012
{
	using System.IO;

	using DataMining_uu_2012.hw1;
	using DataMining_uu_2012.hw2;
	using DataMining_uu_2012.project;
	using DataMining_uu_2012.utilities;

	using Newtonsoft.Json;

	class Program
	{
		private static void CalculateJaccard(Hw2 hw2)
		{
			var statements = new List<string>
				                     {
					                     "d1d2Uni: " + Hw2.JaccardUnigram(hw2.D1Unigrams, hw2.D2Unigrams),
															 "d1d3Uni: " + Hw2.JaccardUnigram(hw2.D1Unigrams, hw2.D3Unigrams),
															 "d1d4Uni: " + Hw2.JaccardUnigram(hw2.D1Unigrams, hw2.D4Unigrams),
															 "d2d3Uni: " + Hw2.JaccardUnigram(hw2.D2Unigrams, hw2.D3Unigrams),
															 "d2d4Uni: " + Hw2.JaccardUnigram(hw2.D2Unigrams, hw2.D4Unigrams),
															 "d3d4Uni: " + Hw2.JaccardUnigram(hw2.D3Unigrams, hw2.D4Unigrams),
															 "d1d2Bi: " + Hw2.JaccardBigram(hw2.D1Bigrams, hw2.D2Bigrams),
															 "d1d3Bi: " + Hw2.JaccardBigram(hw2.D1Bigrams, hw2.D3Bigrams),
															 "d1d4Bi: " + Hw2.JaccardBigram(hw2.D1Bigrams, hw2.D4Bigrams),
															 "d2d3Bi: " + Hw2.JaccardBigram(hw2.D2Bigrams, hw2.D3Bigrams),
															 "d2d4Bi: " + Hw2.JaccardBigram(hw2.D2Bigrams, hw2.D4Bigrams),
															 "d3d4Bi: " + Hw2.JaccardBigram(hw2.D3Bigrams, hw2.D4Bigrams),
															 "d1d2Tri: " +  Hw2.JaccardTrigram(hw2.D1Trigrams, hw2.D2Trigrams),
															 "d1d3Tri: " +  Hw2.JaccardTrigram(hw2.D1Trigrams, hw2.D3Trigrams),
															 "d1d4Tri: " + Hw2.JaccardTrigram(hw2.D1Trigrams, hw2.D4Trigrams), 
															 "d2d3Tri: " + Hw2.JaccardTrigram(hw2.D2Trigrams, hw2.D3Trigrams),
															 "d2d4Tri: " + Hw2.JaccardTrigram(hw2.D2Trigrams, hw2.D4Trigrams),
															 "d3d4Tri: " + Hw2.JaccardTrigram(hw2.D3Trigrams, hw2.D4Trigrams)
				                     };

			foreach (var statement in statements)
			{
				Console.WriteLine(statement);
			}
		}

		private static void HowManyDistinctNgrams(Hw2 hw2)
		{
			var statements = new List<string>
				                 {
					                 "d1Uni: " + hw2.D1Unigrams.Keys.Count,
													 "d1Bi: " + hw2.D1Bigrams.Keys.Count,
													 "d1Tri: " + hw2.D1Trigrams.Keys.Count,
													 "d2Uni: " + hw2.D2Unigrams.Keys.Count,
													 "d2Bi: " + hw2.D2Bigrams.Keys.Count,
													 "d2Tri: " + hw2.D2Trigrams.Keys.Count,
													 "d3Uni: " + hw2.D3Unigrams.Keys.Count,
													 "d3Bi: " + hw2.D3Bigrams.Keys.Count,
													 "d3Tri: " + hw2.D3Trigrams.Keys.Count,
													 "d4Uni: " + hw2.D4Unigrams.Keys.Count,
													 "d4Bi: " + hw2.D4Bigrams.Keys.Count,
													 "d4Tri: " + hw2.D4Trigrams.Keys.Count
				                 };
			foreach (var statement in statements)
			{
				Console.WriteLine(statement);
			}
		}

		static void Main(string[] args)
		{
			if (args != null && args.Length > 0 && args.Any(t => t == "Bloomberg"))
			{
				var fileLocation = Directory.GetCurrentDirectory();

				var newDirectory = Path.Combine(fileLocation,  DateTime.Now.ToFileTimeUtc().ToString());

				Directory.CreateDirectory(newDirectory);
				
				var res = Bloomberg.GetAllCompanies();
				foreach (var item in res)
				{
					var companyInfo = Bloomberg.GetCompanyInfo(item);
					var serializedItem = JsonConvert.SerializeObject(companyInfo, Formatting.Indented);
					using (var newFile = File.CreateText(Path.Combine(newDirectory, companyInfo.CompanyHandle.CleanseFileName() + ".json")))
					{
						newFile.Write(serializedItem);
						newFile.Close();
					}
				}

				return;
			}

			var hw2 = new Hw2();
			HowManyDistinctNgrams(hw2);
			CalculateJaccard(hw2);
			Console.ReadLine();
			//var bdp = new BirthdayParadox();
			////let's average it out a hundred times
			//var nums = new List<int>();

			//for (var i = 0; i < 200; i++)
			//{
			//	var random = new Random();
			//	var num = bdp.GenerateRandomNumbers(1000, random); 
			//	nums.Add(num);
			//	Console.WriteLine(num);	
			//}
			//Console.WriteLine("Average amount was: " + (nums.Sum() / nums.Count));
			//Console.ReadKey();
		}
	}
}
