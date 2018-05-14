using System;
using System.Collections.Generic;
using Contracts.Entities.Data;
using Contracts.Enums;

namespace API.Models
{
    public class StrategyTesterModelFilter
    {
        public StrategyTypes Strategy { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}
