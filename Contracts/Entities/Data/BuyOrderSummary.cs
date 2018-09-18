using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Enums;

namespace Contracts.Entities.Data
{

    public class BuyOrderSummary: OrderSummary
    {
        public TradeTypes TradeType { get; set; }
    }
}
