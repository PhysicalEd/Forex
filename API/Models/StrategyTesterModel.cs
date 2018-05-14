using System;
using System.Collections.Generic;
using Contracts.Entities;
using Contracts.Entities.Data;
using Contracts.Enums;

namespace API.Models
{
    public class StrategyTesterModel
    {
        public List<EnumSummary<StrategyTypes>> StrategyTypes { get; set; } = new List<EnumSummary<StrategyTypes>>();
        public StrategyTesterModelFilter Filters { get; set; }
        public BreakoutStrategyModel BreakoutModel { get; set; }

    }
}
