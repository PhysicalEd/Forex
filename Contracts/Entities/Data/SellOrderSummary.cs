using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Enums;

namespace Contracts.Entities.Data
{

    public class SellOrderSummary: OrderSummary
    {
        public NetProfitTypes NetProfitType { get; set; }
        public decimal NetProfitValue { get; set; }

    }
}
