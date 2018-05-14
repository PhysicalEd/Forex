using System;
using System.Collections.Generic;
using System.Linq;

namespace Contracts.Entities.Data
{
    public class BreakoutTradeSessionSummary
    {
        public DateTime TradeStartTime { get; set; }
        public DateTime TradeEndTime { get; set; }
        public List<Tick> TickData { get; set; }
        public DateTime ReferenceCandleTime { get; set; }
        public DateTime InactivityTradeEndTime { get; set; }

        public CandleSummary ReferenceCandle { get; set; }

    }
}
