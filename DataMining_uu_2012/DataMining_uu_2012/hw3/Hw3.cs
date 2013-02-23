using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012.hw3
{
	using System.Text.RegularExpressions;

	using DataMining_uu_2012.utilities;

	public class Hw3
	{
		public IList<Point> C1 { get; private set; }
		public IList<Point> C2 { get; private set; }

		public Hw3()
		{
			var c1 = "DataMining_uu_2012.hw3.C1.txt".ReadResource();
			var carriageReturnSplitC1 = c1.Split('\n');
			C1 = new List<Point>();
			this.AddPointsToList(carriageReturnSplitC1, this.C1);

			var c2 = "DataMining_uu_2012.hw3.C1.txt".ReadResource();
			var carriageReturnSplitC2 = c2.Split('\n');
			C2 = new List<Point>();
			this.AddPointsToList(carriageReturnSplitC2, this.C2);
		}

		private void AddPointsToList(IEnumerable<string> carriageReturnSplitC1, ICollection<Point> list)
		{
			foreach (var point in carriageReturnSplitC1)
			{
				var splitPoint = Regex.Split(point, "\\s+");
				if (splitPoint.Length == 3)
				{
					list.Add(
						new Point { Id = splitPoint[0], X = Point.ConvertToDouble(splitPoint[1]), Y = Point.ConvertToDouble(splitPoint[2]) });
				}
			}
		}
	}
}
