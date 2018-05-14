using System;
using System.Collections.Generic;
using Contracts;
using Contracts.Entities.Data;

namespace API.Models
{
    public abstract class BaseStrategyModel
    {
        public virtual string StrategyName { get; set; }
        public virtual string StrategyDescription { get; set; }
        public virtual List<Tick> TickData { get; set; }
    }
}
