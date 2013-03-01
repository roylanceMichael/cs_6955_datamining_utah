using System;

namespace DataMining_uu_2012.hw3
{
	using System.Collections.Generic;

	public class Point
	{
		public string Id { get; set; }
		public double X { get; set; }
		public double Y { get; set; }

		public static double ConvertToDouble(string val)
		{
			if (string.IsNullOrWhiteSpace(val))
			{
				return 0;
			}
			double output;
			return double.TryParse(val, out output) ? output : 0;
		}

		public static double Distance(Point a, Point b)
		{
			return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
		}

		public Point ShortestLink(IEnumerable<Point> points)
		{
			Point shortestLink = null;
			var shortestDistance = double.MaxValue;
			foreach (var point in points)
			{
				var tempDistance = Distance(this, point);
				if (!(tempDistance < shortestDistance))
				{
					continue;
				}

				shortestDistance = tempDistance;
				shortestLink = point;
			}
			return shortestLink;
		}

		public override string ToString()
		{
			return this.Id + " " + this.X + " " + this.Y;
		}
	}
}
