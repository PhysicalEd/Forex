using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.DataManagers;
using Contracts.Entities.Data;
using Contracts.Enums;

namespace Logic.Candle
{
    public class CandleGenerator
    {
        public BasePair Pair { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public CandleTypes CandleType { get; set; }
        

        public CandleGenerator(DateTime dateStart, DateTime dateEnd, CandleTypes candleType)
        {
            this.DateStart = dateStart;
            this.DateEnd = dateEnd;
            this.CandleType = candleType;
        }
        public void GenerateCandles()
        {
            // Iterate through with the first possible hour to start with
            // Split the date range into iterations of the candle type
            var tickRange = 

            Dependency.Dependency.Resolve<ICandleManager>().GetCandleOfTimeRange()


        }


    }
}


// Commit Session range onto memory
// Create Order summary:
// Tick When created, tick when completed, profit or loss, Reason for ending order.
// Find When order is created
// Find When order is completed
// If there is order completed, go next available session.
// If there is no order completed, go next session with the same parameters until order is completed
// Commit created and completed and write to CSV file.