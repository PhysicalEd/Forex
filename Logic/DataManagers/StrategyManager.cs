
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
    public class StrategyManager : IStrategyManager
    {
        public CandleSummary GetReferenceValues(int pairID, DateTime referenceStartTime, DateTime referenceEndTime)
        {
            var strat = new BreakoutStrategy(1, new DateTime(2018, 2, 5, 9, 0, 0), new DateTime(2018, 2, 5, 10, 0, 0));
            strat.Initialize();
            strat.Process();
            return null;
        }

        //public ReferenceValuesSummary GetReferenceValues(int pairID, DateTime referenceStartTime, DateTime referenceEndTime)
        //{
        //    using (var cxt = DataStore.CreateDataStore())
        //    {
        //        var param = new List<SqlParameter>();
        //        param.Add(new SqlParameter {ParameterName = "@pairID", Value = 1});
        //        param.Add(new SqlParameter { ParameterName = "@referenceStartTime", Value = new DateTime(2018, 2, 5, 9, 0, 0) });
        //        param.Add(new SqlParameter { ParameterName = "@referenceEndTime", Value = new DateTime(2018, 2, 5, 10, 0, 0) });
        //        param.Add(new SqlParameter { ParameterName = "@high", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Precision = 19, Scale = 4});

        //        var paramArray = param.ToArray<object>();

        //        var s = cxt.ExecuteStoreQuery<object>("exec [sp_2-GetReferenceValues] @pairID, @referenceStartTime, @referenceEndTime, @high out", paramArray).ToList();

        //        return null;
        //    }
        //}

        public void Init()
        {
            
        }



        ///// <summary>
        ///// Gets all the strategy types
        ///// </summary>
        ///// <returns></returns>
        //public List<EnumSummary<StrategyTypes>> GetStrategyTypes()
        //{
        //    var result = new List<EnumSummary<StrategyTypes>>();
        //    result.Add(new EnumSummary<StrategyTypes>(StrategyTypes.Breakout, "Breakout"));
        //    return result;
        //}

        //public List<EnumSummary<BasePair>> GetBasePairs()
        //{
        //    var result = new List<EnumSummary<BasePair>>();
        //    result.Add(new EnumSummary<BasePair>(BasePair.GBPNZD, "GBP -> NZD"));
        //    result.Add(new EnumSummary<BasePair>(BasePair.GBPUSD, "GBP -> USD"));
        //    return result;
        //}

        //public List<Tick> GetTickData(BasePair basePair, DateTime? startDate, DateTime? endDate)
        //{

        //    using (var cxt = DataStore.CreateDataStore())
        //    {
        //        var pair = cxt.Pair.FirstOrDefault(x => x.PairDescription == basePair.ToString());
        //        var data = (
        //            from tick in cxt.Tick
        //            where tick.PairID == pair.PairID
        //                  && tick.TickTime >= startDate
        //                  && tick.TickTime <= endDate
        //            orderby tick.TickTime
        //            select tick
        //        ).ToList();

        //        return data;
        //    }
        //}

        //public List<ITickData> Test()
        //{
        //    using (var cxt = DataStore.CreateDataStore())
        //    {
        //        var test = cxt.CSVImport.First();

        //        //List<GBPNZD> results = cxt.ExecuteStoreQuery<GBPNZD>("SELECT * from GBPNZD", null).ToList();
        //        return null;
        //    }
        //}

        //public DateTime AddTimeInterval(DateTime date, int timeUnits, CandleTypes type)
        //{
        //    switch (type)
        //    {
        //        case (CandleTypes.Minutes):
        //            return date.AddMinutes(timeUnits);
        //        case (CandleTypes.Hours):
        //            return date.AddHours(timeUnits);
        //        case (CandleTypes.Days):
        //            return date.AddDays(timeUnits);
        //        case (CandleTypes.Weeks):
        //            return date.AddDays(timeUnits * 7);
        //        case (CandleTypes.Months):
        //            return date.AddMonths(timeUnits);
        //        default:
        //            throw new Exception("Can't add this time candle mate");
        //    }
        //}

        //private List<DateTime> GetAllSessionStartTimes(DateTime tradeStartTime, DateTime lastTickData, int timeUnits, CandleTypes intervalType)
        //{
        //    var startTimes = new List<DateTime>();
        //    while (tradeStartTime < lastTickData)
        //    {
        //        var startTime = this.AddTimeInterval(tradeStartTime, timeUnits, intervalType);
        //        startTimes.Add(startTime);
        //    }
        //    return startTimes;
        //}

        //public List<BreakoutTradeSessionSummary> GetBreakoutTradeSessions(BasePair pair, DateTime startDate, DateTime endDate, DateTime tradeTime, CandleTypes tradeBarInterval, int referenceBarsOffset, int closeBarsOffSet, int brokerSpreadInPips)
        //{
        //    using (var cxt = DataStore.CreateDataStore())
        //    {

        //        var sessions = new List<BreakoutTradeSessionSummary>();

        //        // We only want to start getting tick data when we're getting the reference candle
                
        //        // Get reference candle time
        //        var referenceCandleTime = this.AddTimeInterval(tradeTime, 2, tradeBarInterval);
        //        var firstCandleTime = referenceCandleTime;
                

        //        //while (firstCandleTime < endDate)
        //        //{
        //        //    var tradeSession = new BreakoutTradeSessionSummary();
                    
        //        //    var nextStartTimeForSession = this.AddTimeInterval(firstCandleTime, 1, CandleTypes.Days);
        //        //    var hasOrder = false;
        //        //    var hasSoldOrder = false;
        //        //    while (firstCandleTime < nextStartTimeForSession)
        //        //    {
        //        //        var nextCandleTime = this.AddTimeInterval(firstCandleTime, 1, tradeBarInterval);
        //        //        // Get all relevant tick data for the session
        //        //        var tickData = (
        //        //            from tick in cxt.Tick
        //        //            where tick.PairID == (int)pair
        //        //                  && tick.TickTime >= firstCandleTime
        //        //                  && tick.TickTime < nextCandleTime
        //        //            orderby tick.TickTime
        //        //            select tick
        //        //        ).ToList();

        //        //        // Don't waste time if there's no tick data
        //        //        if (!tickData.Any())
        //        //        {
        //        //            firstCandleTime = this.AddTimeInterval(firstCandleTime, 1, tradeBarInterval); ;
        //        //            continue;
        //        //        }

        //        //        firstCandleTime = this.AddTimeInterval(firstCandleTime, 1, tradeBarInterval); ;

        //        //        // If this tick data is reference hour, get all reference related values
        //        //        if (!tradeSession.ReferenceCandleIsSet)
        //        //        {
        //        //            tradeSession.ReferenceCandle = this.GetReferenceCandle(tickData);
        //        //            // EO TODO Double check with Laif.
        //        //            tradeSession.ReferenceCandleIsSet = true;
        //        //            continue;
        //        //        }

        //        //        // Precaution
        //        //        if (!tradeSession.ReferenceCandleIsSet) continue;
        //        //        if (!hasOrder)
        //        //        {
        //        //            // Long
        //        //            var longOrder = this.PriceComparison(tickData, tradeSession.ReferenceCandle.High.Bid, PriceComparisonTypes.GreaterThanOrEqual);
        //        //            var shortOrder = this.PriceComparison(tickData, tradeSession.ReferenceCandle.Low.Bid, PriceComparisonTypes.LesserThanOrEqual);

        //        //            if (longOrder == null && shortOrder == null) continue;


        //        //            if (longOrder != null && shortOrder != null)
        //        //            {
        //        //                tradeSession.Order.OrderPlaced = longOrder.TickTime < shortOrder.TickTime ? longOrder : shortOrder;
        //        //            }
        //        //            else if (longOrder != null)
        //        //            {
        //        //                tradeSession.Order.OrderPlaced = longOrder;
        //        //                tradeSession.Order.LongOrderSetting.TradeType = TradeTypes.Long;
        //        //            }
        //        //            else
        //        //            {
        //        //                tradeSession.Order.OrderPlaced = shortOrder;
        //        //                tradeSession.Order.ShortOrderSetting.TradeType = TradeTypes.Short;
        //        //            }

        //        //            hasOrder = tradeSession.Order.OrderPlaced != null;
        //        //        }

        //        //        // If order has been placed already, try to sell it
        //        //        if (hasOrder)
        //        //        {
        //        //            tradeSession.Order.OrderSold = this.FindSellOrder(tickData, tradeSession.LongTakeProfit, tradeSession.LongStopLoss, tradeSession.Order.OrderSetting.TradeType);

        //        //            // There is a tick found
        //        //            if (tradeSession.Order.OrderSold != null)
        //        //            {
        //        //                tradeSession.Order.IsOrderClosed = true;
        //        //                sessions.Add(tradeSession);
        //        //                firstCandleTime = nextStartTimeForSession;
        //        //                break;
        //        //            }
        //        //        }
        //        //    }
        //        //}

        //        return sessions;


        //    }
        //}

        //public List<BreakoutTradeSessionSummary> Test(BasePair pair, DateTime startDate, DateTime endDate,
        //    DateTime tradeTime, CandleTypes tradeBarInterval, int referenceBarsOffset, int closeBarsOffSet,
        //    int brokerSpreadInPips, CandleTypes sessionInterval = CandleTypes.Days)
        //{
        //    using (var cxt = DataStore.CreateDataStore())
        //    {
        //        var sessions = new List<BreakoutTradeSessionSummary>();

        //        // Init
        //        var sessionStartTime = tradeTime;
        //        while (sessionStartTime < endDate)
        //        {
        //            var oldSessionStartTime = sessionStartTime;
        //            var nextSessionStartTime = this.AddTimeInterval(oldSessionStartTime, 1, sessionInterval);
        //            sessionStartTime = nextSessionStartTime;

        //            var tradeEndTime = this.AddTimeInterval(oldSessionStartTime, 9, tradeBarInterval);
        //            var referenceCandleStartTime = this.AddTimeInterval(oldSessionStartTime, referenceBarsOffset, tradeBarInterval);
        //            var referenceCandleEndTime = this.AddTimeInterval(referenceCandleStartTime, 1, tradeBarInterval);

        //            var data = (
        //                from t in cxt.Tick
        //                where t.TickTime >= referenceCandleStartTime
        //                && t.TickTime < tradeEndTime
        //                && t.PairID == (int)pair
        //                select t
        //            ).ToList();

        //            if (data.Count == 0) continue;

        //            var session = new BreakoutTradeSessionSummary();
        //            session.TradeStartTime = oldSessionStartTime;
        //            session.TradeEndTime = tradeEndTime;
        //            session.ReferenceCandleTime = referenceCandleStartTime;
        //            session.TickData = data;
        //            session.ReferenceCandle = this.GetReferenceCandle(session.TickData.Where(x=>x.TickTime < referenceCandleEndTime).ToList());
        //            session.LongOrderSetting = this.SetLongOrderSettings(session.ReferenceCandle);
        //            session.ShortOrderSetting = this.SetShortOrderSettings(session.ReferenceCandle);

        //            sessions.Add(session);

        //        }

        //        foreach (var session in sessions)
        //        {
        //            // Attempt to get orders...
        //            session.Order = this.CreateOrder(session.TickData, session.LongOrderSetting, session.ShortOrderSetting);
        //        }
        //        return sessions;
        //    }
        //}



        //private LongOrderSetting SetLongOrderSettings(CandleSummary referenceCandle)
        //{
        //    var high = referenceCandle.High.Bid;
        //    var low = referenceCandle.Low.Bid;
        //    var marginPrice = Math.Abs(high - low);

        //    var setting = new LongOrderSetting(marginPrice, high, marginPrice + high, low);
        //    return setting;
        //}

        //private ShortOrderSetting SetShortOrderSettings(CandleSummary referenceCandle)
        //{
        //    var high = referenceCandle.High.Bid;
        //    var low = referenceCandle.Low.Bid;
        //    var marginPrice = Math.Abs(high - low);

        //    var setting = new ShortOrderSetting(marginPrice, low, low - marginPrice, high);
        //    return setting;
        //}

        //private OrderSummary CreateOrder(List<Tick> tickData, LongOrderSetting longOrderSetting, ShortOrderSetting shortOrderSetting)
        //{
        //    var order = new OrderSummary();
        //    // Get the first instance where the price is higher than the long buy price OR the short buy price
        //    order.OrderPlaced = tickData.FirstOrDefault(x=>x.Bid >= longOrderSetting.OrderPrice || x.Bid <= shortOrderSetting.OrderPrice);
        //    if (order.OrderPlaced == null) return null;
        //    order.TradeType = order.OrderPlaced.Bid >= longOrderSetting.OrderPrice ? TradeTypes.Long : TradeTypes.Short;

        //    switch (order.TradeType)
        //    {
        //        case TradeTypes.Long:
        //            order.OrderSold =
        //                tickData.FirstOrDefault(x => x.TickTime > order.OrderPlaced.TickTime &&
        //                                             (x.Bid >= longOrderSetting.TakeProfit ||
        //                                              x.Bid <= longOrderSetting.StopLoss));
        //            if (order.OrderSold == null) order.OrderSold = tickData.Last();
        //            break;
        //        case TradeTypes.Short:
        //            order.OrderSold =
        //                tickData.FirstOrDefault(x => x.TickTime > order.OrderPlaced.TickTime &&
        //                                             (x.Bid <= shortOrderSetting.TakeProfit ||
        //                                              x.Bid >= shortOrderSetting.StopLoss));
        //            if (order.OrderSold == null) order.OrderSold = tickData.Last();
        //            break;
        //        default:
        //            throw new UserException("The trade type needs to be specified");
        //    }

        //    return order;
        //}


        //private Tick PriceComparison(List<Tick> tickData, decimal pricePoint, PriceComparisonTypes comparison)
        //{
        //    switch (comparison)
        //    {
        //        case PriceComparisonTypes.GreaterThan:
        //            return tickData.FirstOrDefault(x => x.Bid > pricePoint);
        //        case PriceComparisonTypes.GreaterThanOrEqual:
        //            return tickData.FirstOrDefault(x => x.Bid >= pricePoint);
        //        case PriceComparisonTypes.LesserThan:
        //            return tickData.FirstOrDefault(x => x.Bid <= pricePoint);
        //        case PriceComparisonTypes.LesserThanOrEqual:
        //            return tickData.FirstOrDefault(x => x.Bid < pricePoint);
        //        default:
        //            return null;
        //    }
            
        //}

        //private Tick FindSellOrder(List<Tick> tickData, decimal takeProfit, decimal stopLoss, TradeTypes tradeType)
        //{
        //    if (tradeType == TradeTypes.Long)
        //    {
        //        return tickData.FirstOrDefault(x => x.Bid >= takeProfit || x.Bid <= stopLoss);
        //    }
        //    else
        //    {
        //        return tickData.FirstOrDefault(x => x.Bid <= takeProfit || x.Bid >= stopLoss);
        //    }
        //}

        ////private Tick FindNextShortOrder(List<Tick> tickData, decimal pricePoint)
        ////{
        ////    return tickData.FirstOrDefault(x => x.Bid <= pricePoint);
        ////}

        //private CandleSummary GetReferenceCandle(List<Tick> tickData)
        //{
        //    // EO TODO THIS IS WRONG
        //    var candle = new CandleSummary();
        //    candle.Open = tickData.FirstOrDefault();
        //    candle.Close = tickData.LastOrDefault();
        //    //candle.High = tickData.Max(x=>x.Bid)
        //    var orderedTickData = tickData.OrderBy(x => x.Bid).ToList();
        //    candle.High = orderedTickData.LastOrDefault();
        //    candle.Low = orderedTickData.FirstOrDefault();
        //    return candle;
        //}
    }
}