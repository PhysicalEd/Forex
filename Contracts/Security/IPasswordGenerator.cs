namespace Contracts.Security
{
	public interface IPasswordGenerator
	{
		/// <summary>
		/// Generates this password info
		/// </summary>
		/// <returns></returns>
		string Generate();
	}
}