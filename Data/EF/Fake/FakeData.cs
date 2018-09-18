using System.Collections.Generic;
using Contracts.Cache;
using Contracts.Repositories;

namespace Data.EF.Fake {
    
    public partial class FakeData : IRepository
    {

        public int SaveChanges() {
			var storage = Dependency.Dependency.Resolve<IPersistentStorage>();
			storage.Save(this, "validstubfakedata");
            return 1;
        }

        /// <summary>
        /// Saves the changes back to the data store
        /// </summary>
        public void SubmitChanges()
        {
            this.SaveChanges();
        }

		public static FakeData FromCache()
		{
			var storage = Dependency.Dependency.Resolve<IPersistentStorage>();
			var fake = storage.Load<FakeData>("validstubfakedata");
			return fake;
		}

        public void Dispose() {
		
        }

        public IEnumerable<RETURNTYPE> ExecuteStoreQuery<RETURNTYPE>(string commandText, object[] parameters)
        {
            return null;
        }

        public void ExecuteStoreCommand(string commandText, object[] parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}