using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012.hw2
{
	// This class facilitates multiplicative hashing
	public class MultiplicativeHash
	{
		private static Random ranToUse = new Random();

		public double Frac(int k)
		{
			const double A = .83;
			var res = k * A;
			return res % 1;
		}

		public bool UnigramMinHashing(IDictionary<string, int> d1, IDictionary<string, int> d2)
		{
			var completeList = new Dictionary<string, int>();
			foreach (var item in d1.Keys.Where(item => !completeList.ContainsKey(item)))
			{
				completeList[item] = 0;
			}

			foreach (var item in d2.Keys.Where(item => !completeList.ContainsKey(item)))
			{
				completeList[item] = 0;
			}

			// add numbers into the array
			var subtractList = new List<string>();
			var addList = completeList.Keys.ToList();
			while (addList.Count > 0)
			{
				var idx = ranToUse.Next(0, addList.Count - 1);
				subtractList.Add(addList[idx]);
				addList.RemoveAt(idx);
			}

			// we have the randomly sorted elements now...
			var d1FirstElement = subtractList.FirstOrDefault(d1.ContainsKey);
			var d2FirstElement = subtractList.FirstOrDefault(d2.ContainsKey);

			return d1FirstElement == d2FirstElement && d1FirstElement != null;
		}

		public bool BigramMinHashing(IDictionary<Tuple<string, string>, int> d1, IDictionary<Tuple<string, string>, int> d2)
		{
			var completeList = new Dictionary<Tuple<string, string>, int>();
			foreach (var item in d1.Keys.Where(item => !completeList.Any(t => t.Key.Item1 == item.Item1 && t.Key.Item2 == item.Item2)))
			{
				completeList[item] = 0;
			}

			foreach (var item in d2.Keys.Where(item => !completeList.Any(t => t.Key.Item1 == item.Item1 && t.Key.Item2 == item.Item2)))
			{
				completeList[item] = 0;
			}

			// add numbers into the array
			var subtractList = new List<Tuple<string, string>>();
			var addList = completeList.Keys.ToList();
			while (addList.Count > 0)
			{
				var idx = ranToUse.Next(0, addList.Count - 1);
				subtractList.Add(addList[idx]);
				addList.RemoveAt(idx);
			}

			var d1FirstElement = subtractList.FirstOrDefault(r => d1.Any(t => t.Key.Item1 == r.Item1 && t.Key.Item2 == r.Item2));
			var d2FirstElement = subtractList.FirstOrDefault(r => d2.Any(t => t.Key.Item1 == r.Item1 && t.Key.Item2 == r.Item2));

			return d1FirstElement != null && d2FirstElement != null && d1FirstElement.Item1 == d2FirstElement.Item1 && d1FirstElement.Item2 == d2FirstElement.Item2;
		}

		public bool TrigramMinHashing(IDictionary<Tuple<string, string, string>, int> d1, IDictionary<Tuple<string, string, string>, int> d2)
		{
			var completeList = new Dictionary<Tuple<string, string, string>, int>();
			foreach (var item in d1.Keys.Where(item => !completeList.Any(t => t.Key.Item1 == item.Item1 && t.Key.Item2 == item.Item2 && t.Key.Item3 == item.Item3)))
			{
				completeList[item] = 0;
			}

			foreach (var item in d2.Keys.Where(item => !completeList.Any(t => t.Key.Item1 == item.Item1 && t.Key.Item2 == item.Item2 && t.Key.Item3 == item.Item3)))
			{
				completeList[item] = 0;
			}

			// add numbers into the array
			var subtractList = new List<Tuple<string, string, string>>();
			var addList = completeList.Keys.ToList();
			while (addList.Count > 0)
			{
				var idx = ranToUse.Next(0, addList.Count - 1);
				subtractList.Add(addList[idx]);
				addList.RemoveAt(idx);
			}

			var d1FirstElement = subtractList.FirstOrDefault(r => d1.Any(t => t.Key.Item1 == r.Item1 && t.Key.Item2 == r.Item2 && t.Key.Item3 == r.Item3));
			var d2FirstElement = subtractList.FirstOrDefault(r => d2.Any(t => t.Key.Item1 == r.Item1 && t.Key.Item2 == r.Item2 && t.Key.Item3 == r.Item3));

			return d1FirstElement != null && d2FirstElement != null && d1FirstElement.Item1 == d2FirstElement.Item1 && d1FirstElement.Item2 == d2FirstElement.Item2 && d1FirstElement.Item3 == d2FirstElement.Item3;
		}
	}
}
