
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
        /// We want to check if we have all necessary tick data to form candles
        /// </summary>
        public void CheckImportedCSVs(DateTime fromDate, DateTime toDate)
        {
            // Set all CSVImportFromDate values first
            this.SetCSVImportFromDate();

            // Get all the CSVImports that falls in the date range...
            using (var cxt = DataStore.CreateDataStore())
            {
                var csvImports = cxt.CSVImport.Where(x => x.FromDate >= fromDate && x.FromDate <= toDate).ToList();

                int monthsApart = 0;
                var missingDates = new List<DateTime>();

                while (fromDate <= toDate)
                {
                    monthsApart++;
                    // Check if the fromDate in the CSVImports. If not, add to the missing dates
                    if (csvImports.Any(x=>x.FromDate != fromDate)) missingDates.Add(fromDate);

                    fromDate = fromDate.AddMonths(1);
                }

                var missingDatesString = string.Join(", ", missingDates);

                if (csvImports.Count < monthsApart) throw new UserException("There are missing CSVImport records. Please ensure all records have been processed. The items are " + missingDatesString);
            }
        }

        //public List<CSVImportSummary>

        public void SetCSVImportFromDate()
        {
            using (var cxt = DataStore.CreateDataStore())
            {
                var CSVImports = cxt.CSVImport.Where(x => x.FromDate == null || x.ToDate == null).ToList();
                foreach (var csv in CSVImports)
                {
                    
                    // We will assume that the CSV DateDescription will never be wrong at this point...
                    var year = Int32.Parse(csv.DateDescription.Substring(0, 4));
                    var month = Int32.Parse(csv.DateDescription.Substring(4, 2));
                    
                    csv.FromDate = new DateTime(year, month, 1).ToUniversalTime();

                    if (month == 12)
                    {
                        month = 1;
                        year += 1;
                    }

                    csv.ToDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToUniversalTime();
                    cxt.SubmitChanges();
                }
            }
        }

        

        /// <summary>
        /// Returns a range of dates depending on the candle type
        /// </summary>
        /// <returns></returns>
        public void CreateCandles(DateTime startDate, DateTime endDate, CandleTypes candleType, BasePair pair)
        {
            // We first want to check if all the ticks have been imported 
            this.CheckImportedCSVs(startDate, endDate);

            // Get the number of minutes to iterate based on the CandleType...
            var candleTypeSummary = this.GetCandleTypeSummary(candleType);
            var numberOfMinutesToAdd = candleTypeSummary.NumberOfMinutes;

            var newStartDate = startDate;

            while (newStartDate <= endDate)
            {
                // Get the starting and ending tick
                var startTick = newStartDate;
                var nextTickStart = newStartDate.AddMinutes(numberOfMinutesToAdd);

                // Check if candle has already been generated or not...
                var candle = this.GetCandle(candleType, startTick, pair);
                if (candle != null)
                {
                    newStartDate = nextTickStart;
                    continue;
                }

                // Otherwise create candle
                var generatedCandle = this.GenerateSingleCandle(candleType, startTick, nextTickStart, pair);
                if (generatedCandle == null)
                {
                    newStartDate = nextTickStart;
                    continue;
                }

                // Save resulting candle
                var candleSummary = this.SaveCandle(candleType, startTick, generatedCandle.High.TickID, generatedCandle.Low.TickID, generatedCandle.Open.TickID, generatedCandle.Close.TickID, pair);

                newStartDate = nextTickStart;

            }
        }

        public CandleSummary GetCandle(CandleTypes candleType, DateTime tickStart, BasePair pair)
        {
            using (var cxt = DataStore.CreateDataStore())
            {
                var data = cxt.Candle.FirstOrDefault(x => x.CandleTypeID == (int) candleType && x.FromTime == tickStart && x.PairID == (int) pair);
                CandleSummary result = null;

                if (data != null)
                {
                    result = (
                        from c in cxt.Candle
                        join ht in cxt.Tick on c.HighTickID equals ht.TickID
                        join lt in cxt.Tick on c.LowTickID equals lt.TickID
                        join ot in cxt.Tick on c.LowTickID equals ot.TickID
                        join ct in cxt.Tick on c.LowTickID equals ct.TickID
                        where c.CandleID == data.CandleID
                        select new CandleSummary
                        {
                            BasePairID = (int)pair,
                            CandleTypeID = (int)candleType,
                            Close = ct,
                            FromTime = tickStart,
                            High = ht,
                            Low = lt,
                            Open = ot
                        }
                    ).FirstOrDefault();
                }

                return result;
            }
        }

        public CandleSummary SaveCandle(CandleTypes candleType, DateTime fromTime, int highTickID, int lowTickID, int openTickID, int closeTickID, BasePair pair)
        {
            using (var cxt = DataStore.CreateDataStore())
            {
                // At this point, we will just assume that candle we will be saving is always new...
                var candle = cxt.GetOrCreateCandle(null);
                candle.CandleTypeID = (int) candleType;
                candle.FromTime = fromTime;
                candle.HighTickID = highTickID;
                candle.LowTickID = lowTickID;
                candle.OpenTickID = openTickID;
                candle.CloseTickID = closeTickID;
                candle.PairID = (int) pair;
                cxt.SubmitChanges();

                return this.GetCandle(candleType, candle.FromTime, pair);
            }
        }


        /// <summary>
        /// Gets a single candle of a DateTime range
        /// </summary>
        /// <param name="candleType"></param>
        /// <param name="tickStart"></param>
        /// <param name="nextTickStart"></param>
        /// <param name="pair"></param>
        /// <returns></returns>
        public CandleSummary GenerateSingleCandle(CandleTypes candleType, DateTime tickStart, DateTime nextTickStart, BasePair pair)
        {
            CandleSummary result = null;
            using (var cxt = DataStore.CreateDataStore())
            {
                var data = (
                    from t in cxt.Tick
                    join p in cxt.Pair on t.PairID equals p.PairID
                    where p.PairID == (int)pair
                          && t.TickTime >= tickStart
                          && t.TickTime < nextTickStart
                    select t
                ).OrderBy(x => x.TickTime).ToList();

                if (data.Count > 0)
                {
                    result = new CandleSummary();
                    result.High = data.Aggregate((x, y) => x.Bid > y.Bid ? x : y);
                    result.Low = data.Aggregate((x, y) => x.Bid < y.Bid ? x : y);
                    result.Open = data.FirstOrDefault();
                    result.Close = data.LastOrDefault();
                    result.FromTime = tickStart;
                    result.ToTime = nextTickStart;
                    result.BasePairID = (int)pair;
                    result.CandleTypeID = (int)candleType;
                }

                return result;
            }
        }
    }
}