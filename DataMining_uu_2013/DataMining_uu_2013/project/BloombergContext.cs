using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012.project
{
	using System.Data.Entity;

	public class BloombergContext : DbContext 
	{
		public BloombergContext(string connectionString)
			: base(connectionString)
		{
			
		}

		public IDbSet<CompanyInfo> CompanyInfos { get; set; } 
	}
}
