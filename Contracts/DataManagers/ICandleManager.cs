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
        CandleSummary GenerateSingleCandle(CandleTypes candleType, DateTime tickStart, DateTime tickEnd, BasePair pair);

        void CreateCandles(DateTime startDate, DateTime endDate, CandleTypes candleType, BasePair pair);

        void CheckImportedCSVs(DateTime fromDate, DateTime toDate);


    }
}
