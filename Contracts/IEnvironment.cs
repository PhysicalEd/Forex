namespace Contracts
{
	public interface IEnvironment
	{
		string SiteRoot { get; }
		string CurrentIPAddress { get; }
		string BrowserSummary { get; }
		string LastUrlReferrer { get; }
		string GetFullUrl(string partUrl);
	}
}
