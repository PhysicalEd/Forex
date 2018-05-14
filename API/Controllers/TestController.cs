using System.Web.Http;

namespace API.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public string Test()
        {
            return "Worked";
        }
    }


}
