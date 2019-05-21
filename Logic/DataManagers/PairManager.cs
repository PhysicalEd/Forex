using System.Linq;
using Contracts.DataManagers;
using Contracts.Entities.Data;
using Contracts.Enums;
using Contracts.Exceptions;

namespace Logic.DataManagers
{
    public class PairManager : IPairManager
    {
        public Pair GetPair(BasePair pair)
        {
            using (var cxt = DataStore.CreateDataStore())
            {
                var result = cxt.Pair.FirstOrDefault(x => x.PairDescription == pair.ToString());
                return result;
            }
        }

        public Pair SavePair(BasePair pair)
        {
            using (var cxt = DataStore.CreateDataStore())
            {
                var description = pair.ToString();
                var cxtPair = cxt.Pair.FirstOrDefault(x => x.PairDescription == description);
                if (cxtPair == null) cxtPair = cxt.GetOrCreatePair(null);
                cxtPair.PairDescription = description;
                cxt.SubmitChanges();
                return cxtPair;
            }
        }
    }
}


