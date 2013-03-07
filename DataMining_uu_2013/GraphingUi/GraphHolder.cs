using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphingUi
{
	using DataMining_uu_2012.hw3;
	using DataMining_uu_2012.models;

	using ZedGraph;

	public partial class GraphHolder : Form
	{
		public GraphHolder()
		{
			InitializeComponent();
			var hw3 = new Hw3();
			CreateGraph(this.zdc, hw3.LloydsGraphModel);
		}

		private static void CreateGraph(ZedGraphControl zgc, GraphModel graphModel)
		{
			// get a reference to the GraphPane
			var myPane = zgc.GraphPane;

			// Set the Titles
			myPane.Title.Text = graphModel.TitleText;
			myPane.XAxis.Title.Text = graphModel.XTitleText;
			myPane.YAxis.Title.Text = graphModel.YTitleText;

			// Generate a red curve with diamond
			// symbols, and "Porsche" in the legend
			myPane.AddCurve(string.Empty, graphModel.Values, Color.Red, SymbolType.Circle);

			// Tell ZedGraph to refigure the
			// axes since the data have changed
			zgc.AxisChange();
		}
	}
}
