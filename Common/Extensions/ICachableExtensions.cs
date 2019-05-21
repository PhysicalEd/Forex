
using Contracts.Cache;

namespace Dependency.Extensions
{
	public static class ICachableExtensions
	{
		/// <summary>
		/// Assigns this identifier to the tags property of the cache item
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="type"></param>
		/// <param name="identifier"></param>
		public static void AddTag(this ICachable cache, CacheTags type, dynamic identifier)
		{
			cache.Tags.Add(FormatTag(type, identifier));
		}

		/// <summary>
		/// Formats the concatenation of the cache tag
		/// </summary>
		/// <param name="type"></param>
		/// <param name="identifier"></param>
		/// <returns></returns>
		public static string FormatTag(CacheTags type, dynamic identifier)
		{
			if (identifier == null) identifier = "null";
			string id = type.ToString() + identifier.ToString();
			return id;
		}
	}
}
