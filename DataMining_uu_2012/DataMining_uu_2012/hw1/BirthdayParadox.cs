using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012.hw1
{
	public class BirthdayParadox
	{
		/// <summary>
		/// generating random numbers of n until we have a duplicate, returning how many it takes to get it
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public int GenerateRandomNumbers(int n)
		{
			var k = 0;
			var awesomeHash = new HashSet<int>();
			var randomGen = new Random();
			var foundDup = false;
			while (!foundDup)
			{
				var num = randomGen.Next(1, n);
				if (awesomeHash.Contains(num))
				{
					foundDup = true;
				}
				else
				{
					awesomeHash.Add(num);
					k++;
				}
			}
			return k;
		}
	}
}
