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
        //List<Tick> GetTickData(BasePair basePair, DateTime? startDate, DateTime? endDate);
        //DateTime AddTimeInterval(DateTime date, int timeUnits, CandleTypes type);

        //List<BreakoutTradeSessionSummary> GetBreakoutTradeSessions(BasePair pair, DateTime startDate, DateTime endDate, DateTime tradeTime, CandleTypes tradeBarInterval, int referenceBarsOffset, int closeBarsOffSet, int brokerSpreadInPips);

        //List<BreakoutTradeSessionSummary> Test(BasePair pair, DateTime startDate, DateTime endDate,
        //    DateTime tradeTime, CandleTypes tradeBarInterval, int referenceBarsOffset, int closeBarsOffSet,
        //    int brokerSpreadInPips, CandleTypes sessionInterval = CandleTypes.Days);
        CandleSummary GetReferenceValues(int pairID, DateTime referenceStartTime, DateTime referenceEndTime);


    }
}
