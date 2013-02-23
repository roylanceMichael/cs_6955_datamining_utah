using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012.hw3
{
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
			if (double.TryParse(val, out output))
			{
				return output;
			}
			return 0;
		}
	}
}
