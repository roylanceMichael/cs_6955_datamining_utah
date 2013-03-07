namespace GraphingUi
{
	using ZedGraph;

	partial class GraphHolder
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.zdc = new ZedGraph.ZedGraphControl();
			this.SuspendLayout();
			// 
			// zdc
			// 
			this.zdc.AutoSize = true;
			this.zdc.Location = new System.Drawing.Point(43, 13);
			this.zdc.Name = "zdc";
			this.zdc.ScrollGrace = 0D;
			this.zdc.ScrollMaxX = 0D;
			this.zdc.ScrollMaxY = 0D;
			this.zdc.ScrollMaxY2 = 0D;
			this.zdc.ScrollMinX = 0D;
			this.zdc.ScrollMinY = 0D;
			this.zdc.ScrollMinY2 = 0D;
			this.zdc.Size = new System.Drawing.Size(658, 446);
			this.zdc.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(737, 506);
			this.Controls.Add(this.zdc);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ZedGraphControl zdc;
	}
}

