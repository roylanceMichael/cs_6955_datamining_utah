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
															 "[G1] d1d2Bi: " +  Hw2.JaccardBigramChars(hw2.D1.BigramChars, hw2.D2.BigramChars),
															 "[G1] d1d3Bi: " +  Hw2.JaccardBigramChars(hw2.D1.BigramChars, hw2.D3.BigramChars),
															 "[G1] d1d4Bi: " + Hw2.JaccardBigramChars(hw2.D1.BigramChars, hw2.D4.BigramChars), 
															 "[G1] d2d3Bi: " + Hw2.JaccardBigramChars(hw2.D2.BigramChars, hw2.D3.BigramChars),
															 "[G1] d2d4Bi: " + Hw2.JaccardBigramChars(hw2.D2.BigramChars, hw2.D4.BigramChars),
															 "[G1] d3d4Bi: " + Hw2.JaccardBigramChars(hw2.D3.BigramChars, hw2.D4.BigramChars),

															 "[G2] d1d2Bi: " +  Hw2.JaccardTrigramChars(hw2.D1.TrigramChars, hw2.D2.TrigramChars),
															 "[G2] d1d3Bi: " +  Hw2.JaccardTrigramChars(hw2.D1.TrigramChars, hw2.D3.TrigramChars),
															 "[G2] d1d4Bi: " + Hw2.JaccardTrigramChars(hw2.D1.TrigramChars, hw2.D4.TrigramChars), 
															 "[G2] d2d3Bi: " + Hw2.JaccardTrigramChars(hw2.D2.TrigramChars, hw2.D3.TrigramChars),
															 "[G2] d2d4Bi: " + Hw2.JaccardTrigramChars(hw2.D2.TrigramChars, hw2.D4.TrigramChars),
															 "[G2] d3d4Bi: " + Hw2.JaccardTrigramChars(hw2.D3.TrigramChars, hw2.D4.TrigramChars),

															 "[G3] d1d2Tri: " +  Hw2.JaccardTrigramWords(hw2.D1.TrigramsWords, hw2.D2.TrigramsWords),
															 "[G3] d1d3Tri: " +  Hw2.JaccardTrigramWords(hw2.D1.TrigramsWords, hw2.D3.TrigramsWords),
															 "[G3] d1d4Tri: " + Hw2.JaccardTrigramWords(hw2.D1.TrigramsWords, hw2.D4.TrigramsWords), 
															 "[G3] d2d3Tri: " + Hw2.JaccardTrigramWords(hw2.D2.TrigramsWords, hw2.D3.TrigramsWords),
															 "[G3] d2d4Tri: " + Hw2.JaccardTrigramWords(hw2.D2.TrigramsWords, hw2.D4.TrigramsWords),
															 "[G3] d3d4Tri: " + Hw2.JaccardTrigramWords(hw2.D3.TrigramsWords, hw2.D4.TrigramsWords)
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
					                 "[G1] d1BiChar: " + hw2.D1.BigramChars.Keys.Count,
													 "[G2] d1TriChar: " + hw2.D1.TrigramChars.Keys.Count,
													 "[G3] d1TriWord: " + hw2.D1.TrigramsWords.Keys.Count,
													 "[G1] d2BiChar: " + hw2.D2.BigramChars.Keys.Count,
													 "[G2] d2TriChar: " + hw2.D2.TrigramChars.Keys.Count,
													 "[G3] d2TriWord: " + hw2.D2.TrigramsWords.Keys.Count,
													 "[G1] d3BiChar: " + hw2.D3.BigramChars.Keys.Count,
													 "[G2] d3TriChar: " + hw2.D3.TrigramChars.Keys.Count,
													 "[G3] d3TriWord: " + hw2.D3.TrigramsWords.Keys.Count,
													 "[G1] d4BiChar: " + hw2.D4.BigramChars.Keys.Count,
													 "[G2] d4TriChar: " + hw2.D4.TrigramChars.Keys.Count,
													 "[G3] d4TriWord: " + hw2.D4.TrigramsWords.Keys.Count
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

				for (var i = 0; i < 10; i++)
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
			var res = mult.BigramMinHashing(hw2.D1.BigramsWords, hw2.D2.BigramsWords);
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
				var res = mult.UnigramMinHashing(hw2.D1UnigramsWords, hw2.D2UnigramsWords);
				if (res)
				{
					uniTenDist++;
				}
			}

			uniTenDist = uniTenDist / 10;

			double uniFiftyDist = 0;
			for (var i = 0; i < 50; i++)
			{
				var res = mult.UnigramMinHashing(hw2.D1UnigramsWords, hw2.D2UnigramsWords);
				if (res)
				{
					uniFiftyDist++;
				}
			}
			uniFiftyDist = uniFiftyDist / 50;

			double uniHundredDist = 0;
			for (var i = 0; i < 100; i++)
			{
				var res = mult.UnigramMinHashing(hw2.D1UnigramsWords, hw2.D2UnigramsWords);
				if (res)
				{
					uniHundredDist++;
				}
			}

			uniHundredDist = uniHundredDist / 100;

			double uniThreeHundred = 0;
			for (var i = 0; i < 300; i++)
			{
				var res = mult.UnigramMinHashing(hw2.D1UnigramsWords, hw2.D2UnigramsWords);
				if (res)
				{
					uniThreeHundred++;
				}
			}

			uniThreeHundred = uniThreeHundred / 300;

			double uniSixHundred = 0;
			for (var i = 0; i < 600; i++)
			{
				var res = mult.UnigramMinHashing(hw2.D1UnigramsWords, hw2.D2UnigramsWords);
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
			var res = mult.TrigramMinHashing(hw2.D1TrigramsWords, hw2.D2TrigramsWords);
			if (res)
			{
				dis++;
			}
			return dis;
		}
 * */
