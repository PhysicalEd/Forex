using System;
using System.Collections.Generic;
using System.Dynamic;
using Contracts.Entities.Data;
using Contracts.Enums;

namespace API.Models
{
    public class BreakoutStrategyModelFilter : BaseStrategyModelFilters
    {
        // EO TODO Setup London open etc on this...
        public DateTime TradeTime { get; set; }

        public DateTime ReferenceTime
        {
            get { return this.TradeTime.AddHours(this.ReferenceOffset); }
        }

        public int ReferenceOffset { get; set; } = 2;
        public int CloseOffset { get; set; } = 6;

        public TimeIntervalTypes TimeIntervalType { get; set; }

        //public 
    }
}
