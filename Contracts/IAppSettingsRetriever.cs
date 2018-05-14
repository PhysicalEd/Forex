namespace Contracts
{
	public interface IAppSettingsRetriever
	{
		/// <summary>
		/// Gets the value using the key as the lookup.
		/// </summary>
		/// <param name="resouceKey">The key.</param>
		/// <returns></returns>
		string GetValue(string key);

		/// <summary>
		/// Gets the value using the key as the lookup. Can return a fallback default value.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="fallbackDefault">The fallback default.</param>
		/// <returns></returns>
		string GetValueOrDefault(string key, string fallbackDefault);
	}
}
