
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using Contracts;
using Contracts.DataManagers;
using Contracts.Entities;
using Contracts.Entities.Candle;
using Contracts.Entities.Data;
using Contracts.Enums;
using Contracts.Exceptions;


namespace Logic.DataManagers
{
    public class CandleManager : ICandleManager
    {
        /// <summary>
        /// Gets information about the candle type from the database
        /// </summary>
        /// <param name="candleType"></param>
        /// <returns></returns>
        public CandleTypeSummary GetCandleTypeSummary(CandleTypes candleType)
        {
            using (var cxt = DataStore.CreateDataStore())
            {
                var data = (
                        from ct in cxt.CandleType
                        where ct.CandleTypeCode == candleType.ToString()
                        select new CandleTypeSummary
                        {
                            CandleType = candleType,
                            Description = ct.Description,
                            NumberOfMinutes = ct.NumberOfMinutes
                        }
                    ).FirstOrDefault();

                return data;
            }
        }

        /// <summary>
        /// Returns a range of dates depending on the candle type
        /// </summary>
        /// <returns></returns>
        public List<DateTimeRangeSummary> GetDateRangesForCandleType(DateTime startDate, DateTime endDate, CandleTypeSummary candleType)
        {
            var result = new List<DateTimeRangeSummary>();
            int setMinutes = 0;

            switch (candleType.CandleType)
            {
                    case CandleTypes.M1:
                        startDate = startDate.AddMinutes(1);
                        break;
                    case CandleTypes.M5:
                        setMinutes = startDate.Minute + (5 - (startDate.Minute % 5));
                        startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, setMinutes, 0);
                    break;
                    case CandleTypes.M15:
                        setMinutes = startDate.Minute + (15 - (startDate.Minute % 15));
                        startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, setMinutes, 0);
                        break;
                    case CandleTypes.M30:
                        setMinutes = startDate.Minute + (30 - (startDate.Minute % 30));
                        startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, setMinutes, 0);
                        break;
                    case CandleTypes.H1:
                        if (startDate.Minute != 0) startDate = startDate.AddHours(1);
                        break;
                    default:
                    throw new UserException("Please provide a candleType");

            }



            return result;


            // If the startDate doesn't start on the hour, let's make it the next hour for simplicity...
            //if (startDate.Minute != 0) startDate.Add
        }

        /// <summary>
        /// Gets a single candle of a DateTime range
        /// </summary>
        /// <param name="tickStart"></param>
        /// <param name="tickEnd"></param>
        /// <param name="pair"></param>
        /// <returns></returns>
        public CandleSummary GetCandleOfTimeRange(DateTime tickStart, DateTime tickEnd, BasePair pair)
        {
            var result = new CandleSummary();
            using (var cxt = DataStore.CreateDataStore())
            {
                var data = (
                    from t in cxt.Tick
                    join p in cxt.Pair on t.PairID equals p.PairID
                    where p.PairID == (int)pair
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