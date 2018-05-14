using Contracts;

namespace Logic
{

	/// <summary>
	/// Returns values from the application configuration file
	/// </summary>
	public class AppSettingsRetriever : IAppSettingsRetriever
	{
		public AppSettingsRetriever()
		{ }


		public string GetValue(string key)
		{
			return System.Configuration.ConfigurationManager.AppSettings[key];
		}

		public string GetValueOrDefault(string key, string fallbackDefault)
		{
			return GetValue(key) ?? fallbackDefault;
		}
	}
}
