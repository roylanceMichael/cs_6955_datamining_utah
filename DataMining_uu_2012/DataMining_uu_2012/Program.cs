using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012
{
	using DataMining_uu_2012.hw1;

	class Program
	{
		static void Main(string[] args)
		{
			var bdp = new BirthdayParadox();
			//let's average it out a hundred times
			var nums = new List<int>();

			for (var i = 0; i < 200; i++)
			{
				var random = new Random();
				var num = bdp.GenerateRandomNumbers(1000, random); 
				nums.Add(num);
				Console.WriteLine(num);	
			}
			Console.WriteLine("Average amount was: " + (nums.Sum() / nums.Count));
			Console.ReadKey();
		}
	}
}
