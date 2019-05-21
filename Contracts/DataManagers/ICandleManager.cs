using System;
using System.Collections.Generic;
using Contracts.Entities;
using Contracts.Entities.Candle;
using Contracts.Entities.Data;
using Contracts.Enums;

namespace Contracts.DataManagers
{
    public interface ICandleManager
    {
        CandleSummary GetCandleOfTimeRange(DateTime tickStart, DateTime tickEnd, BasePair pair);

        List<DateTimeRangeSummary> GetDateRangesForCandleType(DateTime startDate, DateTime endDate, CandleTypeSummary candleType);

    }
}
