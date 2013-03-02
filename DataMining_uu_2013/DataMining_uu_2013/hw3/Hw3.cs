﻿using System;
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
			AddPointsToList(carriageReturnSplitC1, this.C1);

			var c2 = "DataMining_uu_2012.hw3.C1.txt".ReadResource();
			var carriageReturnSplitC2 = c2.Split('\n');
			C2 = new List<Point>();
			AddPointsToList(carriageReturnSplitC2, this.C2);

			Links(this.C1);

			KMeans(this.C2);
		}

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