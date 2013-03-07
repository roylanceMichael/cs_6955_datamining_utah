using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012.hw3
{
	using System.Text.RegularExpressions;

	using DataMining_uu_2012.models;
	using DataMining_uu_2012.utilities;

	using ZedGraph;

	public class Hw3
	{
		private static readonly Random Random = new Random();

		public IList<Point> C1 { get; private set; }
		public IList<Point> C2 { get; private set; }

		public GraphModel KMeansPlusPlusGraphModel { get; set; }
		public GraphModel LloydsGraphModel { get; set; }

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

			//Links(this.C1);
			var clusters = new List<Cluster>
				               {
					               new Cluster(),
												 new Cluster(),
												 new Cluster()
				               };
			clusters[0].AddPoint(this.C2[0]);
			clusters[1].AddPoint(this.C2[1]);
			clusters[2].AddPoint(this.C2[2]);

			Lloyds(this.C2, clusters);

			var gonzalezResult = Gonzales3Center(this.C2);
			clusters = gonzalezResult.Item3;
			Lloyds(this.C2, clusters);

			var normalizedGonzalez = Regex.Replace(gonzalezResult.Item2, "\\s+", string.Empty);

			//var iter = 0;
			//while (!dict.Values.Any(t => t > 0))
			//{
			//	var res = KMeansPlusPlus(this.C2);
			//	if (dict.ContainsKey(res))
			//	{
			//		dict[res]++;
			//	}
			//	else
			//	{
			//		dict[res] = 0;
			//	}
			//	iter++;
			//}
			//Console.WriteLine(iter);

			var gonzalezCount = 0;
			var lloydsCount = 0;
			var gonzalezDict = new Dictionary<double, int>();
			var lloydsDict = new Dictionary<double, int>();
			for (var i = 0; i < 300; i++)
			{
				var res = KMeansPlusPlus(this.C2);

				var normalizedResult = Regex.Replace(res.Item2, "\\s+", string.Empty);
				var lloyds = Lloyds(this.C2, res.Item3);
				var normalizedLloyds = Regex.Replace(lloyds.Item2, "\\s+", string.Empty);
				if (normalizedResult == normalizedGonzalez)
				{
					gonzalezCount++;
				}
				if (normalizedResult == normalizedLloyds)
				{
					lloydsCount++;
				}

				if (gonzalezDict.ContainsKey(res.Item1))
				{
					gonzalezDict[res.Item1]++;
				}
				else
				{
					gonzalezDict[res.Item1] = 1;
				}

				if (lloydsDict.ContainsKey(lloyds.Item1))
				{
					lloydsDict[lloyds.Item1]++;
				}
				else
				{
					lloydsDict[lloyds.Item1] = 1;
				}
			}

			var total = gonzalezDict.Values.Sum();
			var runningTotal = (double)0;
			var pointList = new PointPairList();
			foreach (var key in gonzalezDict.OrderBy(t => t.Key).Select(t => t.Key))
			{
				runningTotal += gonzalezDict[key];
				for (var i = 0; i < gonzalezDict[key]; i++)
				{
					pointList.Add(key, runningTotal / total);
				}
			}
			this.KMeansPlusPlusGraphModel = new GraphModel
																				{
																					TitleText = "KMeans++",
																					Values = pointList,
																					XTitleText = "3 Means Cost"
																				};

			var ltotal = gonzalezDict.Values.Sum();
			var lrunningTotal = (double)0;
			var lpointList = new PointPairList();
			foreach (var key in lloydsDict.OrderBy(t => t.Key).Select(t => t.Key))
			{
				lrunningTotal += lloydsDict[key];
				for (var i = 0; i < lloydsDict[key]; i++)
				{
					lpointList.Add(key, lrunningTotal / ltotal);
				}
			}
			this.LloydsGraphModel = new GraphModel
			{
				TitleText = "Lloyd's",
				Values = lpointList,
				XTitleText = "3 Means Cost"
			};

			foreach (var d in D)
			{
				var l1NormRes = L1Norm(100, d);
			}

			foreach (var d in D)
			{
				var l2NormRes = L2Norm(100, d);
			}

			foreach (var d in D)
			{
				var numTimes = LpDistance(100, d);
			}
		}

		private static List<int> D = new List<int> { 1, 2, 3, 5, 10, 50, 100 };
		#region box-muller
		private static double L1Norm(int tVal, int dVal)
		{
			double sum = 0.0;
			for (var t = 0; t < tVal; t++)
			{
				double i;
				if (t % 2 == 0)
				{
					i = Math.Sqrt(-2.0 * Math.Log(Random.NextDouble())) * Math.Cos(2.0 * Math.PI * Random.NextDouble());
				}
				else
				{
					i = Math.Sqrt(-2.0 * Math.Log(Random.NextDouble())) * Math.Sin(2.0 * Math.PI * Random.NextDouble());
				}
				for (var d = 0; d < dVal; d++)
				{
					double j;
					if (d % 2 == 0)
					{
						j = Math.Sqrt(-2.0 * Math.Log(Random.NextDouble())) * Math.Cos(2.0 * Math.PI * Random.NextDouble());
					}
					else
					{
						j = Math.Sqrt(-2.0 * Math.Log(Random.NextDouble())) * Math.Sin(2.0 * Math.PI * Random.NextDouble());
					}

					sum += Math.Sqrt(Math.Pow(i - j, 2));
				}
			}

			return sum;
		}

		private static double L2Norm(int tVal, int dVal)
		{
			double sum = 0.0;
			for (var t = 0; t < tVal; t++)
			{
				double i;
				if (t % 2 == 0)
				{
					i = Math.Sqrt(-2.0 * Math.Log(Random.NextDouble())) * Math.Cos(2.0 * Math.PI * Random.NextDouble());
				}
				else
				{
					i = Math.Sqrt(-2.0 * Math.Log(Random.NextDouble())) * Math.Sin(2.0 * Math.PI * Random.NextDouble());
				}
				for (var d = 0; d < dVal; d++)
				{
					double j;
					if (d % 2 == 0)
					{
						j = Math.Sqrt(-2.0 * Math.Log(Random.NextDouble())) * Math.Cos(2.0 * Math.PI * Random.NextDouble());
					}
					else
					{
						j = Math.Sqrt(-2.0 * Math.Log(Random.NextDouble())) * Math.Sin(2.0 * Math.PI * Random.NextDouble());
					}

					sum += Math.Abs(i - j);
				}
			}
			return sum / tVal;
		}

		private static double LpDistance(int tVal, int dVal)
		{
			var numberOfTimesDistanceIsLessThan1 = 0.0;
			for (var t = 0; t < tVal; t++)
			{
				for (var d = 0; d < dVal; d++)
				{
					var y1 = Random.NextDouble() * 2 - 1;
					var y2 = Random.NextDouble() * 2 - 1;
					if (Math.Abs(y1 + y2) > 1)
					{
						numberOfTimesDistanceIsLessThan1++;
					}
				}
				numberOfTimesDistanceIsLessThan1 = numberOfTimesDistanceIsLessThan1 / dVal;
			}
			return numberOfTimesDistanceIsLessThan1 / tVal;
		}
		#endregion

		#region Lloyds
		private static Tuple<double, string> Lloyds(IList<Point> points, IList<Cluster> clusters)
		{
			// choose k points for clusters arbitrarily?
			// repeat
			// for all point in points, find phiC(x) (closest center c in C to x)
			// for i in j (points.count?), let ci = average of the points located in it...
			// until the set C is unchanged...
			//lets just add the rest into the first cluster...
			for (var i = 3; i < points.Count; i++)
			{
				clusters[0].AddPoint(points[i]);
			}

			var cost = 0.0;
			var movements = 1;
			while (movements > 0)
			{
				movements = 0;

				foreach (var cluster in clusters)
				{
					for (var i = 0; i < cluster.Points.Count; i++)
					{
						var point = cluster.Points[i];
						var nearestClusterIndex = FindClosestCluster(point, clusters);
						if (nearestClusterIndex.Item1 != clusters.IndexOf(cluster))
						{
							if (cluster.Points.Count > 1 && clusters[nearestClusterIndex.Item1].Points.All(t => t.Id != point.Id))
							{
								cluster.RemovePoint(point.Id);
								clusters[nearestClusterIndex.Item1].AddPoint(point);
								movements++;
								cost += nearestClusterIndex.Item2;
							}
						}
					}
				}
			}

			var sb = new StringBuilder();
			foreach (var cluster in clusters)
			{
				sb.Append(cluster);
				sb.AppendLine();
			}

			return new Tuple<double, string>(cost, sb.ToString());
		}
		#endregion

		#region KMeans++

		private static Tuple<double, string, List<Cluster>> KMeansPlusPlus(IList<Point> points)
		{
			var phi = new int[points.Count];
			//choose c1 in X arbitrarily
			var clusters = new Point[3];
			clusters[0] = points[0];
			//get a random
			//select 1 random points
			var threeCenterCost = 0.0;
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
				var nextCluster = Random.NextDouble();

				var runningTotal = (double)0;
				foreach (var tuple in probTuples)
				{
					runningTotal += tuple.Item2 / total;
					if (nextCluster <= runningTotal)
					{
						clusters[i] = tuple.Item1;
						threeCenterCost += tuple.Item2;
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

			var sb = new StringBuilder();
			for (var i = 0; i < clusters.Length; i++)
			{
				for (var j = 0; j < points.Count; j++)
				{
					if (phi[j] == i)
					{
						sb.AppendLine(points[j].ToString());
					}
				}
				sb.AppendLine();
			}
			var c1 = new Cluster();
			var c2 = new Cluster();
			var c3 = new Cluster();
			c1.AddPoint(clusters[0]);
			c2.AddPoint(clusters[1]);
			c3.AddPoint(clusters[2]);

			return new Tuple<double, string, List<Cluster>>(
				threeCenterCost, sb.ToString(), new List<Cluster> { c1, c2, c3 });
		}
		#endregion

		#region Helper Functions
		private static Tuple<int, double> FindClosestCluster(Point point, IList<Cluster> clusters)
		{
			var minimumDistance = double.MaxValue;
			var nearestNeighborIndex = -1;

			for (var i = 0; i < clusters.Count; i++)
			{
				var dist = Point.Distance(point, clusters[i].Centroid);
				dist = dist * dist;
				if (minimumDistance > dist)
				{
					minimumDistance = dist;
					nearestNeighborIndex = i;
				}
			}

			return new Tuple<int, double>(nearestNeighborIndex, minimumDistance);
		}
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
		private static Tuple<double, string, List<Cluster>> Gonzales3Center(IList<Point> points)
		{
			var phi = new int[points.Count];
			var cost = 0.0;
			for (var i = 0; i < points.Count; i++)
			{
				phi[i] = 0;
			}

			var cPhi = new Point[3];

			cPhi[0] = points[0];

			var m1 = (double)0;
			cPhi[1] = points[0];
			// find the furthest point from A 
			foreach (var point in points)
			{
				var dist = Point.Distance(point, cPhi[0]);
				if (dist > m1)
				{
					m1 = dist;
					cPhi[1] = point;
				}
			}
			cost += m1;
			// update all the clusters now to be in either 1 or 2
			for (var i = 0; i < points.Count; i++)
			{
				var point = points[i];
				var dist1 = Point.Distance(point, cPhi[0]);
				var dist2 = Point.Distance(point, cPhi[1]);
				if (dist1 > dist2)
				{
					phi[i] = 1;
				}
			}

			var tempCenter = new Point { X = (cPhi[0].X + cPhi[1].X) / 2, Y = (cPhi[0].Y + cPhi[1].Y) / 2 };

			var furthestCluster = GonzalezFurthestCluster(points, phi, tempCenter);

			cPhi[2] = points[furthestCluster.Item1];

			//var furthestCluster1 = GonzalezFurthestCluster(points, phi, cPhi[0]);
			//var furthestCluster2 = GonzalezFurthestCluster(points, phi, cPhi[1]);

			//if (furthestCluster1.Item2 > furthestCluster2.Item2)
			//{
			//	cPhi[2] = points[furthestCluster1.Item1];
			//}
			//else
			//{
			//	cPhi[2] = points[furthestCluster2.Item1];
			//}

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

			cost += m1;

			var sb = new StringBuilder();
			for (var i = 0; i < cPhi.Length; i++)
			{
				for (var j = 0; j < points.Count; j++)
				{
					if (phi[j] == i)
					{
						sb.AppendLine(points[j].ToString());
					}
				}
				sb.AppendLine();
			}

			var c1 = new Cluster();
			c1.AddPoint(cPhi[0]);
			var c2 = new Cluster();
			c2.AddPoint(cPhi[1]);
			var c3 = new Cluster();
			c3.AddPoint(cPhi[2]);

			return new Tuple<double, string, List<Cluster>>(cost, sb.ToString(), new List<Cluster> { c1, c2, c3 });
		}

		private static Tuple<int, double> GonzalezFurthestCluster(IList<Point> points, int[] phi, Point tempCenter)
		{
			var furthestCluster = new Tuple<int, double>(0, double.MaxValue);

			for (var i = 0; i < phi.Length; i++)
			{
				var dist = Point.Distance(points[i], tempCenter);
				if (dist < .000001)
				{
					continue;
				}

				if (dist < furthestCluster.Item2)
				{
					furthestCluster = new Tuple<int, double>(i, dist);
				}
			}
			return furthestCluster;
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
