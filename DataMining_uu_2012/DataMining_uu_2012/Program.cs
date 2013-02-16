using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012
{
	using System.Diagnostics;
	using System.IO;
	using System.Threading;

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

		static void ProcessAllCompanies(IEnumerable<string> urls, string newDirectory)
		{
			foreach (var item in urls)
			{
				Console.WriteLine("Processing " + item);
				var companyInfo = Bloomberg.GetCompanyInfo(item);

				if (companyInfo == null)
				{
					continue;
				}

				var serializedItem = JsonConvert.SerializeObject(companyInfo, Formatting.Indented);
				using (var newFile = File.CreateText(Path.Combine(newDirectory, companyInfo.CompanyHandle.CleanseFileName() + ".json")))
				{
					newFile.Write(serializedItem);
					newFile.Close();
				}
			}
		}

		static void Main(string[] args)
		{
			var argsNotNull = args != null;
			if (argsNotNull && args.Length > 0 && args.Any(t => t == "Bloomberg"))
			{
				BloombergFunc();
			}
			else if (argsNotNull && args.Any(t => t == "Hw2"))
			{
				var hw2 = new Hw2();
				HowManyDistinctNgrams(hw2);
				CalculateJaccard(hw2);
				var mult = new MultiplicativeHash();

				BigramMinHashing(mult, hw2);

				Console.ReadLine();
			}
		}

		private static void BigramMinHashing(MultiplicativeHash mult, Hw2 hw2)
		{
			var stopWatch = new Stopwatch();
			double tenDist = 0;
			stopWatch.Start();
			for (var i = 0; i < 10; i++)
			{
				tenDist = BigramMinHash(mult, hw2, tenDist);
			}
			stopWatch.Stop();
			
			tenDist = tenDist / 10;
			Console.WriteLine("10: " + tenDist + " " + stopWatch.Elapsed.Milliseconds);
			stopWatch.Reset();

			double fiftyDist = 0;
			stopWatch.Start();
			for (var i = 0; i < 50; i++)
			{
				fiftyDist = BigramMinHash(mult, hw2, fiftyDist);
			}
			stopWatch.Stop();
			fiftyDist = fiftyDist / 50;
			Console.WriteLine("50: " + fiftyDist + " " + stopWatch.Elapsed.Milliseconds);

			double hundredDist = 0;
			stopWatch.Reset();
			stopWatch.Start();

			for (var i = 0; i < 100; i++)
			{
				hundredDist = BigramMinHash(mult, hw2, hundredDist);
			}
			stopWatch.Stop();
			hundredDist = hundredDist / 100;
			Console.WriteLine("100: " + hundredDist + " " + stopWatch.Elapsed.Milliseconds);
			
			stopWatch.Reset();
			double threeHundred = 0;
			stopWatch.Start();
			for (var i = 0; i < 300; i++)
			{
				threeHundred = BigramMinHash(mult, hw2, threeHundred);
			}
			stopWatch.Stop();
			threeHundred = threeHundred / 300;
			Console.WriteLine("300: " + threeHundred + " " + stopWatch.Elapsed.Milliseconds);

			double sixHundred = 0;
			stopWatch.Reset();
			stopWatch.Start();
			for (var i = 0; i < 600; i++)
			{
				sixHundred = BigramMinHash(mult, hw2, sixHundred);
			}
			stopWatch.Stop();
			sixHundred = sixHundred / 600;
			Console.WriteLine("600: " + sixHundred + " " + stopWatch.Elapsed.Milliseconds);

			Console.WriteLine(
				"Bigram for d1 and d2 is: " + tenDist + " " + fiftyDist + " " + hundredDist + " " + threeHundred + " "
				+ sixHundred);
		}

		private static double BigramMinHash(MultiplicativeHash mult, Hw2 hw2, double dist)
		{
			var res = mult.BigramMinHashing(hw2.D1Bigrams, hw2.D2Bigrams);
			if (res)
			{
				dist++;
			}
			return dist;
		}

		private static void BloombergFunc()
		{
			Console.WriteLine("Getting all the companies...");
			var res = Bloomberg.GetAllCompanies();

			//this is going to go once every three hours
			while (true)
			{
				var fileLocation = Directory.GetCurrentDirectory();
				var newDirectory = Path.Combine(fileLocation, DateTime.Now.ToFileTimeUtc().ToString());
				Directory.CreateDirectory(newDirectory);
				ProcessAllCompanies(res, newDirectory);
				Console.WriteLine("Complete! Now going to sleep for three hours...");
				//sleep for three hours...
				Thread.Sleep(new TimeSpan(0, 3, 0, 0, 0));
			}
		}
	}
}

/*
 * private static void UnigramMinHashing(MultiplicativeHash mult, Hw2 hw2)
		{
			double uniTenDist = 0;
			for (var i = 0; i < 10; i++)
			{
				var res = mult.UnigramMinHashing(hw2.D1Unigrams, hw2.D2Unigrams);
				if (res)
				{
					uniTenDist++;
				}
			}

			uniTenDist = uniTenDist / 10;

			double uniFiftyDist = 0;
			for (var i = 0; i < 50; i++)
			{
				var res = mult.UnigramMinHashing(hw2.D1Unigrams, hw2.D2Unigrams);
				if (res)
				{
					uniFiftyDist++;
				}
			}
			uniFiftyDist = uniFiftyDist / 50;

			double uniHundredDist = 0;
			for (var i = 0; i < 100; i++)
			{
				var res = mult.UnigramMinHashing(hw2.D1Unigrams, hw2.D2Unigrams);
				if (res)
				{
					uniHundredDist++;
				}
			}

			uniHundredDist = uniHundredDist / 100;

			double uniThreeHundred = 0;
			for (var i = 0; i < 300; i++)
			{
				var res = mult.UnigramMinHashing(hw2.D1Unigrams, hw2.D2Unigrams);
				if (res)
				{
					uniThreeHundred++;
				}
			}

			uniThreeHundred = uniThreeHundred / 300;

			double uniSixHundred = 0;
			for (var i = 0; i < 600; i++)
			{
				var res = mult.UnigramMinHashing(hw2.D1Unigrams, hw2.D2Unigrams);
				if (res)
				{
					uniSixHundred++;
				}
			}
			uniSixHundred = uniSixHundred / 600;

			Console.WriteLine(
				"Unigram for d1 and d2 is: " + uniTenDist + " " + uniFiftyDist + " " + uniHundredDist + " " + uniThreeHundred + " "
				+ uniSixHundred);
		}

		private static void TrigramMinHashing(MultiplicativeHash mult, Hw2 hw2)
		{
			double tenDist = 0;
			for (var i = 0; i < 10; i++)
			{
				tenDist = TrigramMinHash(mult, hw2, tenDist);
			}

			tenDist = tenDist / 10;

			double fiftyDist = 0;
			for (var i = 0; i < 50; i++)
			{
				fiftyDist = TrigramMinHash(mult, hw2, fiftyDist);
			}
			fiftyDist = fiftyDist / 50;

			double hundredDist = 0;
			for (var i = 0; i < 100; i++)
			{
				hundredDist = TrigramMinHash(mult, hw2, hundredDist);
			}

			hundredDist = hundredDist / 100;

			double threeHundred = 0;
			for (var i = 0; i < 300; i++)
			{
				threeHundred = TrigramMinHash(mult, hw2, threeHundred);
			}

			threeHundred = threeHundred / 300;

			double sixHundred = 0;
			for (var i = 0; i < 600; i++)
			{
				sixHundred = TrigramMinHash(mult, hw2, sixHundred);
			}
			sixHundred = sixHundred / 600;

			Console.WriteLine(
				"Trigram for d1 and d2 is: " + tenDist + " " + fiftyDist + " " + hundredDist + " " + threeHundred + " "
				+ sixHundred);
		}

		private static double TrigramMinHash(MultiplicativeHash mult, Hw2 hw2, double dis)
		{
			var res = mult.TrigramMinHashing(hw2.D1Trigrams, hw2.D2Trigrams);
			if (res)
			{
				dis++;
			}
			return dis;
		}
 * */
