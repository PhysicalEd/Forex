using Contracts.Entities.Data;
using Contracts.Repositories;
using Data.EF.Fake;

namespace Data.Initializers
{
    /// <summary>
    /// Creates stub data. At time of writing, this is more a proof-of-concept class for the IRepository and IObjectSet implementations
    /// </summary>
    public class ValidStubDataInitializer : IRepositoryInitializer
    {
        /// <summary>
        /// Opens a new fake data repository
        /// </summary>
        /// <returns></returns>
        public IRepository Create()
        {
            var fake = new FakeData();
            //fake.Person.AddObject(new Person() { FirstName = "Ben", LastName = "Liebert", PersonID = 1 });
            return fake;
        }

        /// <summary>
        /// Opens a new fake data repository
        /// </summary>
        /// <param name="callerDescription"></param>
        /// <returns></returns>
        public IRepository Create(string callerDescription) {
            return this.Create();
        }
    }
}
