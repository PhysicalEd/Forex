using System;
using System.Collections.Generic;
using Contracts.Entities.Data;
using Contracts.Enums;

namespace API.Models
{
    public abstract class BaseStrategyModelFilters
    {
        public virtual StrategyTypes StrategyType { get; set; }
        public virtual DateTime? StartTime { get; set; } = new DateTime(2018, 1, 1).ToUniversalTime();
        public virtual DateTime? EndTime { get; set; } = DateTime.UtcNow;
        public virtual BasePairs Pair { get; set; }

    }
}
