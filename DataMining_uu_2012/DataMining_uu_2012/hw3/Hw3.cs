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
			AddPointsToList(carriageReturnSplitC1, this.C1);

			var c2 = "DataMining_uu_2012.hw3.C1.txt".ReadResource();
			var carriageReturnSplitC2 = c2.Split('\n');
			C2 = new List<Point>();
			AddPointsToList(carriageReturnSplitC2, this.C2);

			this.SingleLink(this.C1);
		}

		private void SingleLink(IEnumerable<Point> points)
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

			var res = 0;
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

			var sb = new StringBuilder();
			foreach (var cluster in meanLinkClusters)
			{
				sb.AppendLine(cluster.ToString());
				sb.AppendLine();
			}
			Console.WriteLine(sb);
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

			var sb = new StringBuilder();
			foreach (var cluster in completeLinkClusters)
			{
				sb.AppendLine(cluster.ToString());
				sb.AppendLine();
			}
			Console.WriteLine(sb);
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

			var sb = new StringBuilder();
			foreach (var cluster in singleLinkClusters)
			{
				sb.AppendLine(cluster.ToString());
				sb.AppendLine();
			}
			Console.WriteLine(sb);
			return;
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
