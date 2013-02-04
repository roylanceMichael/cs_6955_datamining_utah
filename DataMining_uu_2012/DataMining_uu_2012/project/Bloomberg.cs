using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012.project
{
	using System.Net;

	using HtmlAgilityPack;

	public static class Bloomberg
	{
		const string BloombergUrl = "http://www.bloomberg.com";
		static readonly List<string> CompanyUrls = new List<string>
				                           {
																		 BloombergUrl + "/markets/companies/a-z/0-9/",
					                           BloombergUrl + "/markets/companies/a-z/a/",
																		 BloombergUrl + "/markets/companies/a-z/b/",
																		 BloombergUrl + "/markets/companies/a-z/c/",
																		 BloombergUrl + "/markets/companies/a-z/d/",
																		 BloombergUrl + "/markets/companies/a-z/e/",
																		 BloombergUrl + "/markets/companies/a-z/f/",
																		 BloombergUrl + "/markets/companies/a-z/g/",
																		 BloombergUrl + "/markets/companies/a-z/h/",
																		 BloombergUrl + "/markets/companies/a-z/i/",
																		 BloombergUrl + "/markets/companies/a-z/j/",
																		 BloombergUrl + "/markets/companies/a-z/k/",
																		 BloombergUrl + "/markets/companies/a-z/l/",
																		 BloombergUrl + "/markets/companies/a-z/m/",
																		 BloombergUrl + "/markets/companies/a-z/n/",
																		 BloombergUrl + "/markets/companies/a-z/o/",
																		 BloombergUrl + "/markets/companies/a-z/p/",
																		 BloombergUrl + "/markets/companies/a-z/q/",
																		 BloombergUrl + "/markets/companies/a-z/r/",
																		 BloombergUrl + "/markets/companies/a-z/s/",
																		 BloombergUrl + "/markets/companies/a-z/t/",
																		 BloombergUrl + "/markets/companies/a-z/u/",
																		 BloombergUrl + "/markets/companies/a-z/v/",
																		 BloombergUrl + "/markets/companies/a-z/w/",
																		 BloombergUrl + "/markets/companies/a-z/x/",
																		 BloombergUrl + "/markets/companies/a-z/y/",
																		 BloombergUrl + "/markets/companies/a-z/z/"
				                           };

		public static List<string> GetAllCompanies()
		{
			using (var webClient = new WebClient())
			{
				var individualLinks = new List<string>();
				foreach (var webAddress in CompanyUrls)
				{
					var webPage = webClient.DownloadString(webAddress);
					var doc = new HtmlDocument();
					doc.LoadHtml(webPage);
					var res = doc.DocumentNode.SelectSingleNode("//table[@class='ticker_data']//tr");
					var nextSibling = res;

					if (res == null)
					{
						continue;
					}

					while (nextSibling != null)
					{
						var firstTd = nextSibling.SelectSingleNode("td[@class='name']/a");
						if (firstTd != null && firstTd.Attributes.Any() && firstTd.Attributes.First().Name == "href")
						{
							individualLinks.Add(BloombergUrl + firstTd.Attributes.First().Value);
						}
						nextSibling = nextSibling.NextSibling;
					}
				}

				return individualLinks;
			}
		}
	}
}
