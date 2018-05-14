using System;
using System.Collections.Generic;
using Contracts.Entities;
using Contracts.Entities.Data;
using Contracts.Enums;

namespace Contracts.DataManagers
{
    public interface IStrategyManager
    {
        List<EnumSummary<StrategyTypes>> GetStrategyTypes();
        List<Tick> GetTickData(BasePairs basePair, DateTime? startDate, DateTime? endDate);
        DateTime AddTimeInterval(DateTime date, int timeUnits, TimeIntervalTypes type);

        

    }
}
