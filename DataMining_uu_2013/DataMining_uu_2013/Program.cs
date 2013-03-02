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
	using DataMining_uu_2012.hw3;
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

			var sb = new StringBuilder();

			foreach (var statement in statements)
			{
				sb.AppendLine(statement);
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
			var sb = new StringBuilder();
			foreach (var statement in statements)
			{
				sb.AppendLine(statement);
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
			if (argsNotNull && args.Any(t => t == "Hw2"))
			{
				var hw2 = new Hw2();
				HowManyDistinctNgrams(hw2);
				CalculateJaccard(hw2);
				var mult = new MultiplicativeHash();
				var sb = new StringBuilder();
				for (var i = 0; i < 10; i++)
				{
					sb.AppendLine(TrigramMinHashing(mult, hw2));
				}

				var sb2 = new StringBuilder();

				var d1d2 = TrigramMinHash(mult, hw2.D1, hw2.D2, 100);
				var d1d3 = TrigramMinHash(mult, hw2.D1, hw2.D3, 100);
				var d1d4 = TrigramMinHash(mult, hw2.D1, hw2.D4, 100);
				var d2d3 = TrigramMinHash(mult, hw2.D2, hw2.D3, 100);
				var d2d4 = TrigramMinHash(mult, hw2.D2, hw2.D4, 100);
				var d3d4 = TrigramMinHash(mult, hw2.D3, hw2.D4, 100);

				sb2.AppendLine(d1d2.ToString());
				sb2.AppendLine(d1d3.ToString());
				sb2.AppendLine(d1d4.ToString());
				sb2.AppendLine(d2d3.ToString());
				sb2.AppendLine(d2d4.ToString());
				sb2.AppendLine(d3d4.ToString());

				Console.ReadLine();
			}
			else if (argsNotNull && args.Any(t => t == "Hw3"))
			{
				var hw3 = new Hw3();
				Console.ReadLine();
			}
		}

		private static string TrigramMinHashing(MultiplicativeHash mult, Hw2 hw2)
		{
			var sb = new StringBuilder();

			var stopWatch = new Stopwatch();
			decimal tenDist = 0;
			stopWatch.Start();
			tenDist = TrigramMinHash(mult, hw2.D1, hw2.D2, 10);
			stopWatch.Stop();
			sb.AppendLine("t=10: normalized-L0(a, b):" + tenDist + " in " + stopWatch.Elapsed.Milliseconds + " milliseconds");
			stopWatch.Reset();

			decimal fiftyDist = 0;
			stopWatch.Start();
			fiftyDist = TrigramMinHash(mult, hw2.D1, hw2.D2, 50);
			stopWatch.Stop();
			sb.AppendLine("t=50: normalized-L0(a, b):" + fiftyDist + " in " + stopWatch.Elapsed.Milliseconds + " milliseconds");

			decimal hundredDist = 0;
			stopWatch.Reset();
			stopWatch.Start();
			hundredDist = TrigramMinHash(mult, hw2.D1, hw2.D2, 100);
			stopWatch.Stop();
			sb.AppendLine("t=100: normalized-L0(a, b):" + hundredDist + " in " + stopWatch.Elapsed.Milliseconds + " milliseconds");

			stopWatch.Reset();
			decimal threeHundred = 0;
			stopWatch.Start();
			threeHundred = TrigramMinHash(mult, hw2.D1, hw2.D2, 300);
			stopWatch.Stop();
			sb.AppendLine("t=300: normalized-L0(a, b):" + threeHundred + " in " + stopWatch.Elapsed.Milliseconds + " milliseconds");

			decimal sixHundred = 0;
			stopWatch.Reset();
			stopWatch.Start();
			sixHundred = TrigramMinHash(mult, hw2.D1, hw2.D2, 600);
			stopWatch.Stop();
			sb.AppendLine("t=600: normalized-L0(a, b):" + sixHundred + " in " + stopWatch.Elapsed.Milliseconds + " milliseconds");

			Console.WriteLine(sb.ToString());

			return sb.ToString();
		}

		private static decimal TrigramMinHash(MultiplicativeHash mult, Document d1, Document d2, int hashFunctionCount)
		{
			return mult.TrigramCharMinHashing(d1.TrigramChars, d2.TrigramChars, hashFunctionCount);
		}
	}
}