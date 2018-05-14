namespace Contracts.Repositories
{
    public interface IRepositoryInitializer
    {
        IRepository Create();
        IRepository Create(string callerDescription);
    }
}
