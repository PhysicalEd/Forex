using System;
using System.Web.Http;
using Contracts.DataManagers;

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
    }


}
