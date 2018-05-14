using Contracts.Entities.Data;

namespace Contracts.DataManagers
{
    public partial interface IAccountManager
    {
        void SaveCredentials(string username, string password);
        LoginSummary SignIn(string username, string password);


    }
}
