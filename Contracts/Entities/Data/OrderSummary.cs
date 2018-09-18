using System;
using System.Collections.Generic;
using System.Linq;

namespace Contracts.Entities.Data
{
    public abstract class OrderSummary
    {
        public int TickID { get; set; }
        public int PairID { get; set; }
        public DateTime TickDateTime { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
    }
}
