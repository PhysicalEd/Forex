using Contracts.Repositories;

namespace Logic
{
    public class DataStore
    {
        /// <summary>
        /// Creates a connection to the database or a test data store, depending on our unity configuration
        /// </summary>
        /// <returns></returns>
		public static IRepository CreateDataStore(string activityOfDescription = "")
        {
            var initializer = Dependency.Dependency.Resolve<IRepositoryInitializer>();
            return initializer.Create(activityOfDescription);
        }
    }
}
