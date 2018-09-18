
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
    public class BreakoutStrategy
    {
        public int PairID { get; set; } // Param
        public DateTime WhenTestStart { get; set; } // Param
        public DateTime WhenTestEnd { get; set; } // Param

        public DateTime WhenSessionStart { get; set; } // Param
        public DateTime WhenSessionEnd { get; set; } // Param

        public DateTime WhenTradeStart => this.WhenReferenceEnd;
        public DateTime WhenTradeEnd { get; set; }

        public decimal Multiplier { get; set; } // Param

        public decimal EntryOffset { get; set; } // Param


        public DateTime WhenReferenceStart{ get; set; }
        public DateTime WhenReferenceEnd { get; set; }

        public CandleSummary ReferenceCandle { get; set; } = new CandleSummary();
        public TradingParameters TradingParameters { get; set; } = new TradingParameters();

        public decimal Spread { get; set; }


        public BreakoutStrategy(int pairID, DateTime whenSessionStart, DateTime whenSessionEnd)
        {
            this.PairID = pairID;
            this.WhenSessionStart = whenSessionStart;
            this.WhenSessionEnd = whenSessionEnd;
        }

        public void Initialize()
        {
            this.WhenReferenceStart = this.WhenSessionStart.AddHours(1);
            this.WhenReferenceEnd = this.WhenReferenceStart.AddHours(1);
        }

        public void Process()
        {
            this.GetReferenceValues();
            this.SetTradingParameters();
        }

        private void GetReferenceValues()
        {
            // Testing
            this.WhenReferenceStart = new DateTime(2018, 2, 5, 9, 0, 0);
            this.WhenReferenceEnd = new DateTime(2018, 2, 5, 10, 0, 0);

            using (var cxt = DataStore.CreateDataStore())
            {
                var data = (
                    from t in cxt.Tick
                    join p in cxt.Pair on t.PairID equals p.PairID
                    where p.PairID == this.PairID
                    && t.TickTime >= this.WhenReferenceStart
                    && t.TickTime < this.WhenReferenceEnd
                    select t
                ).OrderBy(x=>x.TickTime).ToList();

                this.ReferenceCandle.High = data.Aggregate((x, y) => x.Bid > y.Bid ? x : y);
                this.ReferenceCandle.Low = data.Aggregate((x, y) => x.Bid < y.Bid ? x : y);
                this.ReferenceCandle.Open = data.FirstOrDefault();
                this.ReferenceCandle.Close = data.LastOrDefault();

            }
        }

        private void SetTradingParameters()
        {
            this.Spread = Math.Abs(this.ReferenceCandle.High.Bid - this.ReferenceCandle.Low.Bid);
            this.TradingParameters.EntryLong = this.ReferenceCandle.High.Bid + this.ReferenceCandle.Low.Bid;
            this.TradingParameters.EntryShort = this.ReferenceCandle.Low.Bid - this.Spread;

            this.TradingParameters.StopLossLong = this.ReferenceCandle.Low.Bid - this.EntryOffset;
            this.TradingParameters.StopLossShort = this.ReferenceCandle.High.Bid + this.EntryOffset;
            this.TradingParameters.TakeProfitLong = this.ReferenceCandle.High.Bid + (this.Spread * this.Multiplier);
            this.TradingParameters.TakeProfitShort = this.ReferenceCandle.Low.Bid - (this.Spread * this.Multiplier);
        }

        private void GetOrdersForSession()
        {
            using (var cxt = DataStore.CreateDataStore())
            {
                var data = (
                    from t in cxt.Tick
                    join p in cxt.Pair on t.PairID equals p.PairID
                    where p.PairID == this.PairID
                          && (t.Bid >= this.TradingParameters.EntryLong || t.Bid <= this.TradingParameters.EntryShort)
                          //&& t.TickTime >= this.WhenSessionStart
                    select t
                ).OrderBy(x => x.TickTime).ToList();

                this.ReferenceCandle.High = data.Aggregate((x, y) => x.Bid > y.Bid ? x : y);
                this.ReferenceCandle.Low = data.Aggregate((x, y) => x.Bid < y.Bid ? x : y);
                this.ReferenceCandle.Open = data.FirstOrDefault();
                this.ReferenceCandle.Close = data.LastOrDefault();
            }
        }



    }
}
