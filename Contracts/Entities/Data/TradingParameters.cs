using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Contracts.Enums;

namespace Contracts.Entities.Data
{
    public class TradingParameters
    {
        public decimal EntryLong { get; set; }
        public decimal EntryShort { get; set; }
        public decimal StopLossLong { get; set; }
        public decimal TakeProfitLong { get; set; }
        public decimal StopLossShort { get; set; }
        public decimal TakeProfitShort { get; set; }
    }
}

