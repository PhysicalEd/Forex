using System.Web;
using Contracts;

namespace API.AppCode
{
	public class WebEnvironment : IEnvironment
	{

		/// <summary>
		/// Returns the path to the full website
		/// </summary>
		/// <param name="partUrl"></param>
		/// <returns></returns>
		public string GetFullUrl(string partUrl)
		{
			if (partUrl.ToLower().StartsWith("http")) { return partUrl; }
			if (partUrl.StartsWith("~")) { partUrl = partUrl.Substring(1); }
			if (partUrl.StartsWith("/")) { partUrl = partUrl.Substring(1); }
			return SiteRoot + partUrl;
		}

		public string SiteRoot
		{
			get
			{
				string s = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
				if (!s.EndsWith("/")) { s += "/"; }

				string appPath = HttpContext.Current.Request.ApplicationPath;
				if (!appPath.EndsWith("/")) { appPath += "/"; }
				if (appPath == "" || appPath == "/") { return s; }
				if (appPath.StartsWith("/")) { appPath = appPath.Substring(1); }
				
				s += appPath;
				if (!s.EndsWith("/")) { s += "/"; }

				return s.ToLower();
			}
		}

		public string CurrentIPAddress
		{
			get { return HttpContext.Current.Request.UserHostAddress; }
		}

		public string BrowserSummary
		{
			get { return ""; }
		}

		public string LastUrlReferrer
		{
			get
			{
				if (HttpContext.Current.Request.UrlReferrer == null) {
					return "";
				}else
				{
					return HttpContext.Current.Request.UrlReferrer.ToString();
				}
			}
		}
	}
}
