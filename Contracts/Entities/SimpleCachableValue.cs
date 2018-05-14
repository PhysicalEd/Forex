using System.Collections.Generic;
using Contracts.Cache;

namespace Contracts.Entities
{
	public class SimpleCachableValue<T> : ICachable
	{
		public T Value { get; set; }
		public string CacheKey { get; set; }
		

		private List<string> _Tags = new List<string>();
		public List<string> Tags
		{
			get { return _Tags; }
			set { _Tags = value; }
		}
	}
}
