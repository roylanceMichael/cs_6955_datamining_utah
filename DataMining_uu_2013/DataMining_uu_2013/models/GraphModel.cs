using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012.models
{
	using ZedGraph;

	public class GraphModel
	{
		public string TitleText { get; set; }
		public string XTitleText { get; set; }
		public string YTitleText { get; set; }
		public PointPairList Values { get; set; }
	}
}
