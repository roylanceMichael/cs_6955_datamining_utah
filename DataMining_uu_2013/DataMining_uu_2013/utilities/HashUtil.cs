using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012.utilities
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	public static class HashUtil
	{
		private static readonly Random Ran = new Random();

		// I want to build t number of hash functions that will make the 
		// domain of n to a number in k
		// ie, if n is 26, and k is 10, I need to randomize a subset of n to fit into k
		// I need t many of these...

		public static IEnumerable<Dictionary<int, int>> HashFunctions(int t, int k, int n)
		{
			var hashFunctions = new List<Dictionary<int, int>>();

			for (var i = 0; i < t; i++)
			{
				// we need to force n into k now
				// there may be a better way to do it...
				var mappingDictionary = new Dictionary<int, int>();

				// cycle through, set all items to the length of n
				for (var j = 0; j < n; j++)
				{
					var randomK = Ran.Next(0, k);
					mappingDictionary[j] = randomK;
				}
				hashFunctions.Add(mappingDictionary);
			}
			return hashFunctions;
		}
	}



	[TestClass]
	public class TestHash
	{
		[TestMethod]
		public void Success()
		{
			//arrange
			var res = HashUtil.HashFunctions(5, 10, 26);
			//act
			//assert
		}
	}
}
