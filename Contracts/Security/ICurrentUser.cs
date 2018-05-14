namespace Contracts.Security
{
	public interface ICurrentUser
	{
		int? PersonID { get; }
		bool IsAuthenticated { get; }

		string UserSessionIdentifier { get; }
		string DisplayName { get; }
	}
}
