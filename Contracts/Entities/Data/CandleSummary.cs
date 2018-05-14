using System;
using System.Collections.Generic;
using System.Linq;

namespace Contracts.Entities.Data
{
    public class CandleSummary
    {
        public Tick High { get; set; }
        public Tick Low { get; set; }
        public Tick Open { get; set; }
        public Tick Close { get; set; }

    }
}
