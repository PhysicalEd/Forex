using System.Collections.Generic;
using Contracts.Cache;

namespace Contracts.Entities
{
	public class HTMLContent : ICachable
	{

		public string CacheKey { get { return this.Url; } set { this.Url = value; } }


		private List<string> _Tags = new List<string>();
		public List<string> Tags
		{
			get { return _Tags; }
			set { _Tags = value; }
		}

		public string Url { get; set; }
		public string Html { get; set; }
	}
}
