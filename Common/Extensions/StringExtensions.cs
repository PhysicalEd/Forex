using System.Text.RegularExpressions;

namespace Common.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Appends the given key/value to the url
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static string AppendQueryString(this string url, string key, string value)
		{
			// Remove existing item
			var rx = new Regex(@"(?<pre>(.+)(\?|&)" + key + @"=)(?<existing>[^\?&]+)(?<post>.*)", RegexOptions.IgnoreCase);
			if (rx.IsMatch(url))
			{
				url = rx.Replace(url, "${pre}" + value + "${post}");
			}
			else
			{
				// Append
				if (url.IndexOf('?') != -1) { url += "&"; } else { url += "?"; }
				url += key + "=" + value;
			}
			// Return
			return url;
		}
	}
}
