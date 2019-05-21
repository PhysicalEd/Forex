using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using API.Models;
using Contracts;
using Contracts.DataManagers;
using Contracts.Enums;
using System.Runtime.InteropServices;

namespace API.Controllers
{
    public class FileDownloadController : ApiController
    {
        [HttpPost]
        [HttpGet]
        public string DownloadAll()
        {
            var mgr = Dependency.Dependency.Resolve<IFileDownloader>();

            mgr.Initialize(new DateTime(2019, 4, 1), new DateTime(2019, 5, 1), BasePair.GBPNZD);
            try
            {
                Task t = Task.Run(() => mgr.ProcessFiles());
                t.Wait();
            }
            catch (Exception ex)
            {
                
            }
            // Unzip everything...
            try
            {
                Task unzip = Task.Run(() => mgr.UnzipFiles());
                unzip.Wait();
            }
            catch (Exception ex)
            {

            }

            var result = "";
            return result;
        }
    }


}
