using System;
using System.Collections.Generic;
using Contracts.Entities;
using Contracts.Entities.Data;
using Contracts.Enums;

namespace Contracts.DataManagers
{
    public interface IPairManager
    {
        Pair GetPair(BasePair pair);
        Pair SavePair(BasePair pair);



    }
}
