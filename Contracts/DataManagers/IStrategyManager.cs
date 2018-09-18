using System;
using System.Collections.Generic;
using Contracts.Entities;
using Contracts.Entities.Data;
using Contracts.Enums;

namespace Contracts.DataManagers
{
    public interface IStrategyManager
    {
        //List<EnumSummary<StrategyTypes>> GetStrategyTypes();
        //List<Tick> GetTickData(BasePairs basePair, DateTime? startDate, DateTime? endDate);
        //DateTime AddTimeInterval(DateTime date, int timeUnits, TimeIntervalTypes type);

        //List<BreakoutTradeSessionSummary> GetBreakoutTradeSessions(BasePairs pair, DateTime startDate, DateTime endDate, DateTime tradeTime, TimeIntervalTypes tradeBarInterval, int referenceBarsOffset, int closeBarsOffSet, int brokerSpreadInPips);

        //List<BreakoutTradeSessionSummary> Test(BasePairs pair, DateTime startDate, DateTime endDate,
        //    DateTime tradeTime, TimeIntervalTypes tradeBarInterval, int referenceBarsOffset, int closeBarsOffSet,
        //    int brokerSpreadInPips, TimeIntervalTypes sessionInterval = TimeIntervalTypes.Days);
        CandleSummary GetReferenceValues(int pairID, DateTime referenceStartTime, DateTime referenceEndTime);


    }
}
