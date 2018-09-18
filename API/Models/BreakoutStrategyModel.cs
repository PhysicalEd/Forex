using System;
using System.Collections.Generic;
using Contracts;
using Contracts.Entities.Data;

namespace API.Models
{
    public class BreakoutStrategyModel : BaseStrategyModel
    {
        public override string StrategyName => "Breakout strategy";
        public override string StrategyDescription => "Blah blah";

        public BreakoutStrategyModelFilter Filters { get; set; }

        public DateTime TradeTime { get; set; }

        //public List<BreakoutTradeSessionSummary> TradeSessions { get; set; } = new List<BreakoutTradeSessionSummary>();

        //public List<>

    }
}
