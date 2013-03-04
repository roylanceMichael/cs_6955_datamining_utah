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
		private static Random random = new Random();

		public IList<Point> C1 { get; private set; }
		public IList<Point> C2 { get; private set; }
		public IList<Point> Test { get; private set; }

		public Hw3()
		{
			var c1 = "DataMining_uu_2012.hw3.C1.txt".ReadResource();
			var carriageReturnSplitC1 = c1.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			C1 = new List<Point>();
			AddPointsToList(carriageReturnSplitC1, this.C1);

			var c2 = "DataMining_uu_2012.hw3.C2.txt".ReadResource();
			var carriageReturnSplitC2 = c2.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			C2 = new List<Point>();
			AddPointsToList(carriageReturnSplitC2, this.C2);

			var test = "DataMining_uu_2012.hw3.Test.txt".ReadResource();
			var carriageResturnSplitTest = test.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			Test = new List<Point>();
			AddPointsToList(carriageResturnSplitTest, this.Test);

			KMeansPlusPlus(this.C2);

			//KMeans(this.C2);

			//Gonzales3Center(this.C2);

			//Gonzales3Means(this.C2);

			//Links(this.C1);

			//KMeans(this.C2);
		}

		private static void Lloyds(IList<Point> points)
		{
			// choose k points for clusters arbitrarily?
			// repeat
				// for all point in points, find phiC(x) (closest center c in C to x)
				// for i in j (points.count?), let ci = average of the points located in it...
			// until the set C is unchanged...
		}

		#region KMeans++
		private static void KMeansPlusPlus(IList<Point> points)
		{
			var phi = new int[points.Count];
			//choose c1 in X arbitrarily
			var clusters = new Point[3];
			clusters[0] = points[0];
			//get a random
			//select 1 random points
			for (var i = 1; i < 3; i++)
			{
				var prevCluster = clusters[i - 1];
				//choose ci from X with a probability proportional to d(x, phic-1(x))^2
				var probTuples = new List<Tuple<Point, double>>();
				for (var j = 0; j < points.Count; j++)
				{
					var dist = Point.Distance(points[j], prevCluster);
					probTuples.Add(new Tuple<Point, double>(points[j], dist * dist));
				}
				var total = probTuples.Sum(t => t.Item2);
				var nextCluster = random.NextDouble();

				var runningTotal = (double)0;
				foreach (var tuple in probTuples)
				{
					runningTotal += tuple.Item2 / total;
					if (nextCluster <= runningTotal)
					{
						clusters[i] = tuple.Item1;
						break;
					}
				}
			}

			for (var i = 0; i < points.Count; i++)
			{
				if (clusters.Any(t => t.Id == points[i].Id))
				{
					continue;
				}
				var m = double.MaxValue;
				for (var j = 0; j < clusters.Length; j++)
				{
					var dist = Point.Distance(points[i], clusters[j]);
					if (dist < m)
					{
						phi[i] = j;
						m = dist;
					}
				}
			}
		}
		#endregion

		#region Helper Functions
		// C(x) = arg minc2C d(x; c)
		private static Point PhiC(Point x, IEnumerable<Point> clusters)
		{
			Point returnPoint = null;
			var m = double.MaxValue;

			foreach (var cluster in clusters)
			{
				var dist = Point.Distance(x, cluster);
				if (dist < m)
				{
					m = dist;
					returnPoint = cluster;
				}
			}
			return returnPoint;
		}
		#endregion

		#region Gonzales 3 Functions
		private static void Gonzales3Means(IList<Point> points)
		{
			// find the point as the center of the first cluster
			var cPhi = new Point[3];
			cPhi[0] = points[0];
			var minimumDist = double.MaxValue;
			foreach (var point in points.Where(t => t != cPhi[0]))
			{
				var dist = Point.Distance(point, cPhi[0]);
				var res = dist * dist;
				if (res < minimumDist)
				{
					minimumDist = res;
					cPhi[1] = point;
				}
			}
		}

		private static void Gonzales3Center(IList<Point> points)
		{
			var phi = new int[points.Count];
			for (var i = 0; i < points.Count; i++)
			{
				phi[i] = 0;
			}

			var cPhi = new Point[3];

			cPhi[0] = points[0];

			for (var i = 1; i < 3; i++)
			{
				var m = (double)0;
				cPhi[i] = points[0];
				var prevCluster = cPhi[i - 1];
				//just show me the cluster that has the highest distance to i-1...
				foreach (var point in points)
				{
					var dist = Point.Distance(point, prevCluster);
					if (dist > m)
					{
						m = dist;
						cPhi[i] = point;
					}
				}
			}

			for (var j = 0; j < points.Count; j++)
			{
				var m = double.MaxValue;
				for (var i = 0; i < 3; i++)
				{
					var dist = Point.Distance(points[j], cPhi[i]);
					if (dist < m)
					{
						m = dist;
						phi[j] = i;
					}
				}
			}
		}
		#endregion

		#region KMeans
		private static void KMeans(ICollection<Point> points)
		{
			// initially choose k points that are likely to be in different clusters
			// to avoid too much variation in the results, choose c1 as the point a
			var p1 = points.First(t => t.Id == "a");
			Point p2 = null;
			Point p3 = null;

			// find the point that has the largest distance from c1

			var largestTuple = FindLargestDistanceFromPointTuple(points, p1);

			if (largestTuple.Item1 == null)
			{
				return;
			}

			p2 = largestTuple.Item1;

			var tempPoint = new Point
												{
													X = (p1.X + p2.X) / 2,
													Y = (p1.Y + p2.Y) / 2,
												};

			largestTuple = FindLargestDistanceFromPointTuple(points, tempPoint);

			if (largestTuple.Item1 == null)
			{
				return;
			}

			p3 = largestTuple.Item1;

			var clusters = new List<Cluster>();
			var initClusters = new[] { p1, p2, p3 };
			foreach (var point in initClusters)
			{
				var cluster = new Cluster();
				cluster.AddPoint(point);
				clusters.Add(cluster);
			}

			// copy to new array, this is a waste I know..

			foreach (var point in points)
			{
				if (initClusters.Any(t => t.Id == point.Id))
				{
					continue;
				}

				var closestCluster = new Tuple<Cluster, double>(null, double.MaxValue);

				foreach (var cluster in clusters)
				{
					var dist = Point.Distance(cluster.Centroid, point);
					if (dist < closestCluster.Item2)
					{
						closestCluster = new Tuple<Cluster, double>(cluster, dist);
					}
				}

				if (closestCluster.Item1 != null)
				{
					closestCluster.Item1.AddPoint(point);
				}
			}

			PrintClusters(clusters);
		}
		#endregion
		
		private static void PrintClusters(IEnumerable<Cluster> clusters)
		{
			var sb = new StringBuilder();
			foreach (var cluster in clusters)
			{
				sb.AppendLine(cluster.ToString());
				sb.AppendLine();
			}
			Console.WriteLine(sb);
		}

		private static Tuple<Point, double> FindLargestDistanceFromPointTuple(IEnumerable<Point> points, Point c1)
		{
			var largestTuple = new Tuple<Point, double>(null, double.MinValue);
			foreach (var point in points.Where(t => t.Id != c1.Id))
			{
				var dist = Point.Distance(c1, point);
				if (dist > largestTuple.Item2)
				{
					largestTuple = new Tuple<Point, double>(point, dist);
				}
			}
			return largestTuple;
		}

		private static void Links(IEnumerable<Point> points)
		{
			var singleLinkClusters = new List<Cluster>();
			var completeLinkClusters = new List<Cluster>();
			var meanLinkClusters = new List<Cluster>();

			foreach (var point in points)
			{
				var cluster = new Cluster();
				cluster.AddPoint(point);
				singleLinkClusters.Add(cluster);
				var cCluster = new Cluster();
				cCluster.AddPoint(new Point
														{
															Id = point.Id,
															X = point.X,
															Y = point.Y
														});

				completeLinkClusters.Add(cCluster);

				var mCluster = new Cluster();
				mCluster.AddPoint(new Point
				{
					Id = point.Id,
					X = point.X,
					Y = point.Y
				});

				meanLinkClusters.Add(mCluster);
			}

			//find the smallest distance between all clusters
			SingleLinkCluster(singleLinkClusters);

			CompleLinkCluster(completeLinkClusters);

			MeanLinkCluster(meanLinkClusters);
		}

		public static void MeanLinkCluster(ICollection<Cluster> meanLinkClusters)
		{
			var dictionary = new Dictionary<Tuple<Cluster, Cluster>, double>();

			while (meanLinkClusters.Count > 3)
			{
				foreach (var cluster in meanLinkClusters)
				{
					var closureCluster = cluster;
					foreach (var anotherCluster in meanLinkClusters.Where(t => t != closureCluster))
					{
						var newTuple = new Tuple<Cluster, Cluster>(cluster, anotherCluster);
						dictionary[newTuple] = cluster.MeanLink(anotherCluster);
					}
				}

				var firstRes = dictionary.OrderBy(t => t.Value).First();
				var mergeWith = firstRes.Key.Item1;
				var mergeTo = firstRes.Key.Item2;
				mergeWith.MergeClusters(mergeTo);
				meanLinkClusters.Remove(mergeTo);
				dictionary.Clear();
			}

			PrintClusters(meanLinkClusters);
		}

		private static void CompleLinkCluster(ICollection<Cluster> completeLinkClusters)
		{
			var dictionary = new Dictionary<Tuple<Cluster, Cluster>, double>();

			while (completeLinkClusters.Count > 3)
			{
				foreach (var cluster in completeLinkClusters)
				{
					var closureCluster = cluster;
					foreach (var anotherCluster in completeLinkClusters.Where(t => t != closureCluster))
					{
						var newTuple = new Tuple<Cluster, Cluster>(cluster, anotherCluster);
						dictionary[newTuple] = cluster.CompleteLink(anotherCluster);
					}
				}

				var firstRes = dictionary.OrderBy(t => t.Value).First();
				var mergeWith = firstRes.Key.Item1;
				var mergeTo = firstRes.Key.Item2;
				mergeWith.MergeClusters(mergeTo);
				completeLinkClusters.Remove(mergeTo);
				dictionary.Clear();
			}

			PrintClusters(completeLinkClusters);
		}

		private static void SingleLinkCluster(ICollection<Cluster> singleLinkClusters)
		{
			var dictionary = new Dictionary<Tuple<Cluster, Cluster>, double>();

			while (singleLinkClusters.Count > 3)
			{
				foreach (var cluster in singleLinkClusters)
				{
					var closureCluster = cluster;
					foreach (var anotherCluster in singleLinkClusters.Where(t => t != closureCluster))
					{
						var newTuple = new Tuple<Cluster, Cluster>(cluster, anotherCluster);
						dictionary[newTuple] = cluster.SingleLink(anotherCluster);
					}
				}

				var firstRes = dictionary.OrderBy(t => t.Value).First();
				var mergeWith = firstRes.Key.Item1;
				var mergeTo = firstRes.Key.Item2;
				mergeWith.MergeClusters(mergeTo);
				singleLinkClusters.Remove(mergeTo);
				dictionary.Clear();
			}

			PrintClusters(singleLinkClusters);
		}

		private static void AddPointsToList(IEnumerable<string> carriageReturnSplitC1, ICollection<Point> list)
		{
			foreach (var splitPoint in carriageReturnSplitC1.Select(point => Regex.Split(point, "\\s+")).Where(splitPoint => splitPoint.Length == 3))
			{
				list.Add(new Point { Id = splitPoint[0], X = Point.ConvertToDouble(splitPoint[1]), Y = Point.ConvertToDouble(splitPoint[2]) });
			}
		}
	}
}
