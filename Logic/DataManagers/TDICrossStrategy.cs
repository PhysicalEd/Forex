
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
    public class TDICrossStrategy
    {
        public TDICrossStrategy()
        {
            // EO TODO TEST
            // Get the start and end times for test
            var startTestTime = new DateTime();
            var endTestTime = new DateTime();
            var pairID = 1;
            var period = TimeIntervalTypes.Weeks;

            // Process it using CandleManager
            var candleMgr = new CandleManager(startTestTime, endTestTime, period);
        }

        
        
        

    }
}



// EO TODO 5/2/2019 PSEUDO
// Get candles based on period
// Do a take? to avoid memory leak
// Store candle data in database

// Strategy itself is:
// Get testing timeframe range ie 3 months.
// Find take? candles and process
// Once all candles are processed do logic for RSI line and TDI lines.
// Find crossovers for candles...
// Store in database

// Little chunks
// Create new database table TestRequest
// TestRequestID
// TestStartTime
// TestEndTime
// 

    // Needed or not yet??
// Create new database table TDICrossResult
// TDICrossResultID
// CandleID
// TradeType - OpenOrder, CloseOrder
// PriceOpened, PriceClosed, Gross Profit