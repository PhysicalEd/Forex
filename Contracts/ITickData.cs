using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contracts
{
    public interface ITickData
    {
        int TickID { get; }
        decimal Bid { get; }
        decimal Ask { get; }
        DateTime TickTime { get; }
        int CSVImportID { get; }
        
    }
}
