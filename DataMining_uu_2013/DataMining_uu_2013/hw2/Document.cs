using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012.hw2
{
	public class Document
	{
		public string D { get; set; }
		public Dictionary<string, int> UnigramsWords { get; set; }
		public Dictionary<Tuple<string, string>, int> BigramsWords { get; set; }
		public Dictionary<Tuple<string, string, string>, int> TrigramsWords { get; set; }
		public Dictionary<Tuple<char, char>, int> BigramChars { get; set; }
		public Dictionary<Tuple<char, char, char>, int> TrigramChars { get; set; } 
	}
}
