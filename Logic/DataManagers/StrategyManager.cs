
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Contracts;
using Contracts.DataManagers;
using Contracts.Entities;
using Contracts.Entities.Data;
using Contracts.Enums;
using Contracts.Exceptions;


namespace Logic.DataManagers
{
    public class StrategyManager : IStrategyManager
    {
        /// <summary>
        /// Gets all the strategy types
        /// </summary>
        /// <returns></returns>
        public List<EnumSummary<StrategyTypes>> GetStrategyTypes()
        {
            var result = new List<EnumSummary<StrategyTypes>>();
            result.Add(new EnumSummary<StrategyTypes>(StrategyTypes.Breakout, "Breakout"));
            return result;
        }

        public List<EnumSummary<BasePairs>> GetBasePairs()
        {
            var result = new List<EnumSummary<BasePairs>>();
            result.Add(new EnumSummary<BasePairs>(BasePairs.GBPNZD, "GBP -> NZD"));
            result.Add(new EnumSummary<BasePairs>(BasePairs.GBPUSD, "GBP -> USD"));
            return result;
        }

        public List<Tick> GetTickData(BasePairs basePair, DateTime? startDate, DateTime? endDate)
        {

            using (var cxt = DataStore.CreateDataStore())
            {
                var pair = cxt.Pair.FirstOrDefault(x => x.PairDescription == basePair.ToString());
                var data = (
                    from tick in cxt.Tick
                    where tick.PairID == pair.PairID
                          && tick.TickTime >= startDate
                          && tick.TickTime <= endDate
                    orderby tick.TickTime
                    select tick
                ).ToList();

                return data;
            }
        }

        public List<ITickData> Test()
        {
            using (var cxt = DataStore.CreateDataStore())
            {
                var test = cxt.CSVImport.First();

                //List<GBPNZD> results = cxt.ExecuteStoreQuery<GBPNZD>("SELECT * from GBPNZD", null).ToList();
                return null;
            }
        }

        public DateTime AddTimeInterval(DateTime date, int timeUnits, TimeIntervalTypes type)
        {
            switch (type)
            {
                case (TimeIntervalTypes.Minutes):
                    return date.AddMinutes(timeUnits);
                case (TimeIntervalTypes.Hours):
                    return date.AddHours(timeUnits);
                case (TimeIntervalTypes.Days):
                    return date.AddDays(timeUnits);
                case (TimeIntervalTypes.Weeks):
                    return date.AddDays(timeUnits * 7);
                case (TimeIntervalTypes.Months):
                    return date.AddMonths(timeUnits);
                default:
                    throw new Exception("Can't add this time interval mate");
            }
        }

        public List<BreakoutTradeSessionSummary> GetBreakoutTradeSessions(BasePairs pair, DateTime tradeStartTime,
            DateTime tradeEndTime, TimeIntervalTypes tradeBarInterval, int referenceBarsOffset, int closeBarsOffSet)
        {
            using (var cxt = DataStore.CreateDataStore())
            {



                // Get all relevant tick data
                var tickData = (
                    from tick in cxt.Tick
                    where tick.PairID == (int) pair
                          && tick.TickTime >= tradeStartTime
                          && tick.TickTime <= tradeEndTime
                    orderby tick.TickTime
                    select tick
                ).ToList();

                if (tickData.Count() == 0) throw new UserException("There is no tick data");

                var lastTickData = tickData.Last().TickTime;
                var tradeStartTimeForSession = tradeStartTime;
                while (tradeStartTimeForSession < lastTickData)
                {
                    var nextTradeTimeForSession = this.AddTimeInterval(tradeStartTimeForSession, 1, tradeBarInterval);
                    var session = new BreakoutTradeSessionSummary();

                    session.InactivityTradeEndTime =
                        this.AddTimeInterval(tradeStartTimeForSession, closeBarsOffSet, tradeBarInterval);
                    session.ReferenceCandleTime =
                        this.AddTimeInterval(tradeStartTimeForSession, referenceBarsOffset, tradeBarInterval);
                    //session.TradeEndTime = tickData.Where(x=>x.TickTime >= session.TradeStartTime && x.TickTime < nextTradeTimeForSession)
                    session.TickData = tickData
                        .Where(x => x.TickTime >= session.TradeStartTime && x.TickTime < nextTradeTimeForSession)
                        .ToList();
                    session.TradeStartTime = tickData.First().TickTime;
                    session.TradeEndTime = tickData.Last().TickTime;
                    session.ReferenceCandle = this.

                }


            }





            //var tradeSessions = new List<BreakoutTradeSessionSummary>();
            //var startTradeSession = ticks.FirstOrDefault();
            ////if (startTradeSession == null) throw new UserException("There are is no tick data for the given filters");

            //if (startTradeSession == null) throw new UserException();
            //var endTradeSession = ticks.LastOrDefault();
            //if (endTradeSession == null) throw new UserException();

            //var counter = 1;
            //while (counter == 1)
            //{
            //    var tradeSession = new BreakoutTradeSessionSummary();
            //    tradeSession.TickData = ticks.Where(x=>x.TickTime >= startTradeSession.TickTime && x.TickTime <= endTradeSession.TickTime).ToList();
            //    tradeSession.TradeStartTime = startTradeSession.TickTime;
            //    tradeSession.TradeEndTime = endTradeSession.TickTime;
            //    tradeSession.ReferenceCandleTime = AddTimeInterval(tradeSession.TradeStartTime, referenceBarsOffset, tradeBarInterval);
            //    //tra
            //}
            //return tradeSessions;

        }

        private CandleSummary GetReferenceCandle(List<Tick> tickData)
        {
            var candle = new CandleSummary();
            candle.Open = tickData.First();
            candle.Close = tickData.Last();
            candle.High = tickData.First(x => x.Ask == tickData.Max(y => y.Ask));
            candle.Low = tickData.First(x => x.Bid == tickData.Min(y => y.Bid));
            return candle;
        }
    }
}