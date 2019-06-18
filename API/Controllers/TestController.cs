using System;
using System.Web.Http;
using Contracts.DataManagers;
using Contracts.Enums;
using Logic.Candle;

namespace API.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public string Test()
        {
            Dependency.Dependency.Resolve<IStrategyManager>().GetReferenceValues(0, new DateTime(), new DateTime());
            return "OK";
        }

        [HttpGet]
        public string TestCandleGenerator()
        {
            var fromTime = new DateTime(2019, 1, 1).ToUniversalTime();
            var toTime = new DateTime(2019, 5, 1).ToUniversalTime();
            var generator = new CandleGenerator(fromTime, toTime, CandleTypes.H1, BasePair.GBPUSD);
            generator.GenerateCandles();
            return "OK";
        }

        [HttpGet]
        public void TestSettingCSVImport()
        {
            var fromTime = new DateTime(2019, 5,1).ToUniversalTime();
            var toTime = new DateTime(2019, 7, 1).ToUniversalTime();

            Dependency.Dependency.Resolve<ICandleManager>().CheckImportedCSVs(fromTime, toTime);
        }
    }


}
