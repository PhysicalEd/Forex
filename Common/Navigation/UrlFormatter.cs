namespace Dependency.Navigation
{
	/// <summary>
	/// Ensures the given URL is correctly formatted
	/// </summary>
	public class UrlFormatter
	{

		private string Url = "";

		public UrlFormatter(string url)
		{
			this.Url = url;
		}

		/// <summary>
		/// Formats the the URL and returns
		/// </summary>
		/// <returns></returns>
		public string ToUrl()
		{
			if (string.IsNullOrEmpty(Url)) return "";
			Url = Url.Trim().ToLower();
			if (!Url.StartsWith("http")) Url = "http://" + Url;
			return Url;
		}
	}
}
