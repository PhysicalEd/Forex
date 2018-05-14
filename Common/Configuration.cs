using Contracts;

namespace Common
{
    public class Configuration
	{

		#region Static Methods

		public static Configuration Current
		{
			get
			{
				return new Configuration();
			}
		}

		#endregion

#region Configuration Accessors

		private IAppSettingsRetriever _Config;

		private IAppSettingsRetriever Config
		{
			get
			{
				if (_Config == null) { _Config = Dependency.Dependency.Resolve<IAppSettingsRetriever>(); }
				return _Config;
			}
		}

		/// <summary>
		/// Returns a string representation of this application setting
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private string GetString(string key)
		{
			return Config.GetValue(key) ?? "";
		}

		/// <summary>
		/// Returns a boolean representation of this application setting
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private bool GetBool(string key, bool defaultValue = false)
		{
			string val = Config.GetValue(key);
			if (string.IsNullOrEmpty(val)) { return defaultValue; }
			return bool.Parse(val);
		}

		/// <summary>
		/// Returns an integer representation of this application setting
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private int GetInt(string key)
		{
			string val = Config.GetValue(key);
			if (string.IsNullOrEmpty(val)) { return 0; }
			return int.Parse(val);
		}

#endregion

		#region Application Settings

		/// <summary>
		/// Do we use the cache in the current system?
		/// </summary>
		public bool UseCache { get { return GetBool("UseCache"); } }

		/// <summary>
		/// The base URL of this website
		/// </summary>
		public string SiteRoot
		{
			get
			{
				var env = Dependency.Dependency.Resolve<IEnvironment>();
				return env.SiteRoot;
			}
		}

		

		#endregion

	}
}
