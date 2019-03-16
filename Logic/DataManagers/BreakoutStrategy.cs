
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
        
        /// <summary>
        /// Test is the start/end of data to be used for the simulation
        /// </summary>
        public DateTime WhenTestStart { get; set; } // Param
        public DateTime WhenTestEnd { get; set; } // Param

        /// <summary>
        /// Session is the start/end of a possible trade including finding the reference candle. eg. London open times  of 8 - 4 etc...
        /// </summary>
        public DateTime WhenSessionStart { get; set; } // Param
        public DateTime WhenSessionEnd { get; set; } // Param

        /// <summary>
        /// The official start/end of trading excluding getting the reference candle
        /// </summary>
        public DateTime WhenTradeStart => this.WhenReferenceEnd;
        public DateTime WhenTradeEnd { get; set; }

        /// <summary>
        /// The value used for leverage
        /// </summary>
        public decimal Multiplier { get; set; } // Param

        /// <summary>
        /// Offset amount used for entry of trade
        /// </summary>
        public decimal EntryOffset { get; set; } // Param

        public DateTime WhenReferenceStart{ get; set; }
        public DateTime WhenReferenceEnd { get; set; }

        public CandleSummary ReferenceCandle { get; set; } = new CandleSummary();

        /// <summary>
        /// List of all Orders done in a session
        /// </summary>
        public List<OrderSummary> OrdersCreated { get; set; } = new List<OrderSummary>();

        /// <summary>
        /// Class that summarises the different trading parameters such as TP and SL etc...
        /// </summary>
        public TradingParameters TradingParameters { get; set; } = new TradingParameters();

        public decimal Spread { get; set; }
        /// <summary>
        /// This is a marker used to simulate at which point we are in the simulation in relation to time
        /// </summary>
        public Tick CurrentTickMarker { get; set; }

        public OrderSummary CurrentOrder { get; set; }

        public BreakoutStrategy(int pairID, DateTime whenSessionStart, DateTime whenSessionEnd)
        {
            this.PairID = pairID;
            this.WhenSessionStart = whenSessionStart;
            this.WhenSessionEnd = whenSessionEnd;
            //this.CurrentTickMarker = this.CurrentTickMarker.TickID > 0 ? this.CurrentTickMarker : new Tick
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
            //this.GetNextOrder();
        }

        /// <summary>
        /// Gets the reference candle values
        /// </summary>
        public void GetReferenceValues()
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
        /// <summary>
        /// Gets the values for trading stops and loseses
        /// </summary>
        public void SetTradingParameters()
        {
            this.Spread = Math.Abs(this.ReferenceCandle.High.Bid - this.ReferenceCandle.Low.Bid);
            this.TradingParameters.EntryLong = this.ReferenceCandle.High.Bid + this.ReferenceCandle.Low.Bid;
            this.TradingParameters.EntryShort = this.ReferenceCandle.Low.Bid - this.Spread;

            this.TradingParameters.StopLossLong = this.ReferenceCandle.Low.Bid - this.EntryOffset;
            this.TradingParameters.StopLossShort = this.ReferenceCandle.High.Bid + this.EntryOffset;
            this.TradingParameters.TakeProfitLong = this.ReferenceCandle.High.Bid + (this.Spread * this.Multiplier);
            this.TradingParameters.TakeProfitShort = this.ReferenceCandle.Low.Bid - (this.Spread * this.Multiplier);
        }


        //public void GetNextOrder()
        //{
        //    using (var cxt = DataStore.CreateDataStore())
        //    {
        //        var result = (
        //            from t in cxt.Tick
        //            join p in cxt.Pair on t.PairID equals p.PairID
        //            where p.PairID == this.PairID
        //                  && (t.Bid >= this.TradingParameters.EntryLong || t.Bid <= this.TradingParameters.EntryShort)
        //                  && (t.TickTime >= CurrentTickMarker.TickTime)
        //            select t
        //        ).FirstOrDefault();

        //        if (result != null)
        //        {
        //            this.CurrentOrder = result;
        //            this.OpenOrder(this.CurrentOrder);
        //        }

        //    }

        //}


        /// <summary>
        /// Opens an order based a tick value
        /// </summary>
        public void OpenOrder(Tick data)
        {
            OrderSummary order = null;
            // If the price is gte the entry long, do a "buy" order
            if (data.Bid >= this.TradingParameters.EntryLong)
            {
                order = new BuyOrderSummary();
                order.TickID = data.TickID;
                order.Ask = data.Ask;
                order.Bid = data.Bid;
                order.PairID = data.PairID;
                order.TickDateTime = data.TickTime;
            }
            // If the price is lte the entry short, do a "short" order
            if (data.Bid <= this.TradingParameters.EntryShort)
            {
                order = new SellOrderSummary();
                order.TickID = data.TickID;
                order.Ask = data.Ask;
                order.Bid = data.Bid;
                order.PairID = data.PairID;
                order.TickDateTime = data.TickTime;
            }

            if (order != null)
            {
                this.OrdersCreated.Add(order);
            }
            else
            {
                throw new UserException("The order is null");
            }
        }

        /// <summary>
        /// Closes an order based on a tick value
        /// </summary>
        public void CloseOrder(Tick data)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openOrder"></param>
        /// <param name="closeOrder"></param>
        public void CalculateProfit(OrderSummary openOrder, OrderSummary closeOrder)
        {
            
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