using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using API.Models;
using Contracts;
using Contracts.DataManagers;
using Contracts.Enums;


namespace API.Controllers
{
    public class StrategyController : ApiController
    {
        public string CSVBasePath { get; set; } = @"D:\Google Drive\Projects\Forex\";
        public void LoadCSVToDB()
        {
            // EO TODO This should be dynamic...
            var csvName = @"Test.csv";
            var csvFullPath = this.CSVBasePath + csvName;
            if (File.Exists(csvFullPath))
            {
                // Load CSV here...

            }
        }

        //[HttpPost]
        //[HttpGet]
        //public StrategyTesterModel StrategyTester([FromUri] StrategyTesterModelFilter filters)
        //{
        //    var model = new StrategyTesterModel();
        //    model.Filters = filters ?? new StrategyTesterModelFilter();
        //    model.StrategyTypes = Dependency.Dependency.Resolve<IStrategyManager>().GetStrategyTypes();
        //    return model;
        //}

        //[HttpPost]
        //[HttpGet]
        //public BreakoutStrategyModel BreakoutTester([FromUri] BreakoutStrategyModelFilter filters)
        //{
        //    var model = new BreakoutStrategyModel();
        //    model.Filters = filters ?? new BreakoutStrategyModelFilter();
        //    var mgr = Dependency.Dependency.Resolve<IStrategyManager>();
            
        //    model.TickData = mgr.GetTickData(model.Filters.Pair, model.Filters.StartTime, model.Filters.EndTime);

        //    return model;
        //}

        [HttpPost]
        [HttpGet]
        public BreakoutStrategyModel Test()
        {
            var model = new BreakoutStrategyModel();
            model.Filters = new BreakoutStrategyModelFilter();
            // Hard code testing...
            model.Filters.Pair = BasePair.GBPNZD;
            model.Filters.StartTime = new DateTime(2018,2,1).ToUniversalTime();
            model.Filters.EndTime = new DateTime(2018, 2,20).ToUniversalTime();
            model.Filters.TradeTime = new DateTime(2018, 2,1,8,0,0);
            model.Filters.CandleType = CandleTypes.Hours;

            var mgr = Dependency.Dependency.Resolve<IStrategyManager>();

            //// Get all tick data and put into memory...
            //model.TickData = mgr.GetTickData(model.Filters.Pair, model.Filters.StartTime, model.Filters.EndTime);

            //var tradeHour = model.Filters.TradeTime.TimeOfDay.Hours;
            //var tradeMinute = model.Filters.TradeTime.TimeOfDay.Minutes;


            //var firstTradeTime = model.TickData.FindAll(x=>x.TickTime.TimeOfDay.Hours == tradeHour && x.TickTime.TimeOfDay.Minutes == tradeMinute);
            //model.TradeSessions = mgr.Test(model.Filters.Pair, model.Filters.StartTime,
            //    model.Filters.EndTime.Value, model.Filters.TradeTime, model.Filters.CandleType, model.Filters.ReferenceOffset,
            //    model.Filters.CloseOffset, 5);

            //var referenceCandle = 

            return model;
        }
    }


}
