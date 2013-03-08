using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012.hw4
{
	using System.Text.RegularExpressions;

	using DataMining_uu_2012.utilities;

	public class Hw4
	{
		public List<string> S1 { get; private set; }
		public Dictionary<string, int> S1Dict
		{
			get
			{
				return this.ReturnDict(this.S1);
			}
		}

		public List<string> S2 { get; private set; }

		public Dictionary<string, int> S2Dict
		{
			get
			{
				return this.ReturnDict(this.S2);
			}
		}

		public Hw4()
		{
			var s1 = "DataMining_uu_2012.hw4.S1.txt".ReadResource();
			S1 = Regex.Split(s1, "\\s+").ToList();
			var s2 = "DataMining_uu_2012.hw4.S2.txt".ReadResource();
			S2 = Regex.Split(s2, "\\s+").ToList();
		}

		private Dictionary<string, int> ReturnDict(IEnumerable<string> vals)
		{
			var returnDict = new Dictionary<string, int>();

			if (vals != null)
			{
				foreach (var item in vals)
				{
					if (returnDict.ContainsKey(item))
					{
						returnDict[item]++;
					}
					else
					{
						returnDict[item] = 1;
					}
				}
			}

			return returnDict;
		}
	}
}
