
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using Contracts;
using Contracts.DataManagers;
using Contracts.Entities;
using Contracts.Entities.Data;
using Contracts.Enums;
using Contracts.Exceptions;


namespace Logic.DataManagers
{
    public class CandleManager
    {
        public int PairID { get; set; }
        public DateTime TickStart { get; set; }
        public DateTime TickEnd { get; set; }

        public TimeIntervalTypes TimeInterval { get; set; }
        public List<CandleSummary> Result { get; set; }


        public CandleManager(DateTime tickStart, DateTime tickEnd, TimeIntervalTypes timeInterval)
        {
            this.TickStart = tickStart;
            this.TickEnd = tickEnd;
            this.TimeInterval = timeInterval;
        }
        public void GenerateCandles()
        {

        }

        public CandleSummary GetCandle(DateTime tickStart, DateTime tickEnd)
        {
            var result = new CandleSummary();
            using (var cxt = DataStore.CreateDataStore())
            {
                var data = (
                    from t in cxt.Tick
                    join p in cxt.Pair on t.PairID equals p.PairID
                    where p.PairID == this.PairID
                          && t.TickTime >= tickStart
                          && t.TickTime < tickEnd
                    select t
                ).OrderBy(x => x.TickTime).ToList();

                result.High = data.Aggregate((x, y) => x.Bid > y.Bid ? x : y);
                result.Low = data.Aggregate((x, y) => x.Bid < y.Bid ? x : y);
                result.Open = data.FirstOrDefault();
                result.Close = data.LastOrDefault();

                return result;
            }
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