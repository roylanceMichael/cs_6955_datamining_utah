using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012.hw3
{
	public class Cluster
	{
		public Cluster()
		{
			this.Points = new List<Point>();
		}

		public IList<Point> Points { get; set; } 

		public void AddPoint(Point p)
		{
			this.Points.Add(p);
		}

		public void MergeClusters(Cluster c)
		{
			foreach (var cPoint in c.Points)
			{
				this.AddPoint(cPoint);
			}
		}

		public double SingleLink(Cluster c)
		{
			// I want to find the shortest distance between this cluster and the cluster given
			var shortestDistance = double.MaxValue;
			foreach (var thisPoint in this.Points)
			{
				foreach (var thatPoint in c.Points)
				{
					var tmpDist = Point.Distance(thisPoint, thatPoint);
					if (tmpDist < shortestDistance)
					{
						shortestDistance = tmpDist;
					}
				}
			}

			return shortestDistance;
		}

		public double CompleteLink(Cluster c)
		{
			// I want to find the greatest distance between this cluster and the cluster given
			var longestDistance = double.MinValue;
			foreach (var thisPoint in this.Points)
			{
				foreach (var thatPoint in c.Points)
				{
					var tmpDist = Point.Distance(thisPoint, thatPoint);
					if (tmpDist > longestDistance)
					{
						longestDistance = tmpDist;
					}
				}
			}

			return longestDistance;
		}

		public double MeanLink(Cluster c)
		{
			var thisMeanPoint = new Point { X = 0, Y = 0 };
			foreach (var thisPoint in this.Points)
			{
				thisMeanPoint.X = thisMeanPoint.X + thisPoint.X;
				thisMeanPoint.Y = thisMeanPoint.Y + thisPoint.Y;
			}
			thisMeanPoint.X = thisMeanPoint.X / this.Points.Count;
			thisMeanPoint.Y = thisMeanPoint.Y / this.Points.Count;

			var thatMeanPoint = new Point { X = 0, Y = 0 };
			foreach (var thatPoint in c.Points)
			{
				thatMeanPoint.X = thatMeanPoint.X + thatPoint.X;
				thatMeanPoint.Y = thatMeanPoint.Y + thatPoint.Y;
			}
			thatMeanPoint.X = thatMeanPoint.X / c.Points.Count;
			thatMeanPoint.Y = thatMeanPoint.Y / c.Points.Count;

			return Point.Distance(thisMeanPoint, thatMeanPoint);
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			foreach (var point in this.Points)
			{
				sb.AppendLine(point.ToString());
			}
			return sb.ToString();
		}
	}
}
