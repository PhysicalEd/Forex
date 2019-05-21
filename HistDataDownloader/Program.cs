using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Contracts.Enums;
using Contracts.Exceptions;
using Flurl;
using Flurl.Http;
using HtmlAgilityPack;
using Tiny.RestClient;
using System.Configuration;
using System.Runtime.CompilerServices;
using Contracts.DataManagers;

namespace HistDataDownloader
{
    public class HistDataDownloader
    {

        public static void Main(string[] args)
        {
            var processor = new Processor();
            processor.SetConfiguration();
            Task.Run(() => processor.ProcessFiles());
            Console.Read();
        }

        public class Processor
        {
            //public string BaseUrl { get; set; } = "https://www.histdata.com/download-free-forex-historical-data/?/ascii/tick-data-quotes/";
            public string BaseUrl { get; set; } = "https://www.histdata.com/";

            public string FullUrl { get; set; }

            public string DownloadUrl => this.BaseUrl + "get.php";

            public BasePair Pair { get; set; }
            public DateTime FromDate { get; set; } = new DateTime();
            public DateTime ToDate { get; set; } = new DateTime();
            public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
            public string DownloadFilePath { get; set; } = ConfigurationManager.AppSettings["DownloadPath"];

            public async Task GetFileDetailFromServer()
            {

            }



            public async Task ProcessFiles()
            {
                // Iterate through each URL and attempt to download each page to get meta information
                while (this.FromDate <= this.ToDate)
                {
                    var dateDescription = "";
                    // Attempt to get the file record
                    var fileDownloadStatus = Dependency.Dependency.Resolve<IFileDownloadManager>().GetFileDownloadStatusUsingDateDescriptionAndBasePair(dateDescription);

                    //if (fileDownloadStatus == null) Dependency.Dependency.Resolve<IFileDownloadManager>().

                    // Get request parameters
                    this.Parameters = await this.GetRequestParamsAsync(this.FromDate.Year, this.FromDate.Month);

                    using (var client = new HttpClient())
                    {
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, this.DownloadUrl);

                        // Populate content and content headers
                        request.Content = new FormUrlEncodedContent(this.Parameters);
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                        // Populate request headers
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                        client.DefaultRequestHeaders.Add("Host", "www.histdata.com");
                        client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                        client.DefaultRequestHeaders.Add("Pragma", "no-cache");
                        client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                        client.DefaultRequestHeaders.Add("Origin", "https://www.histdata.com");
                        client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                        client.DefaultRequestHeaders.Add("Referer", "https://www.histdata.com/download-free-forex-historical-data/?/ascii/tick-data-quotes/gbpusd/2019/3");
                        client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
                        client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));

                        var response = await client.SendAsync(request);
                        var fileName = response.Content.Headers.ContentDisposition.FileName;

                        // Record file in the database

                        // Create the file and download it
                        var filePath = this.DownloadFilePath + fileName;
                        using (var file = System.IO.File.Create(filePath))
                        {
                            var contentStream = response.Content.ReadAsStreamAsync().Result;
                            contentStream.CopyTo(file);
                        }
                    }
                }
            }

            public void SetConfiguration()
            {
                // Set the date range of the desired downloads...
                this.FromDate = new DateTime(2019, 1, 1);
                this.ToDate = new DateTime(2019, 2, 1);

                // Set Pair...
                this.Pair = BasePair.GBPUSD;
            }

            public async Task<Dictionary<string, string>> GetRequestParamsAsync(int year, int month)
            {
                // Create a dictionary container for the return value
                var result = new Dictionary<string, string>();

                // Set full url
                this.FullUrl = string.Format("{0}{1}/{2}/{3}/{4}", this.BaseUrl, "download-free-forex-historical-data/?/ascii/tick-data-quotes", this.Pair, year, month);

                string html;
                // Get the html content for the url
                using (var client = new WebClient())
                {
                    html = client.DownloadString(this.FullUrl);
                }

                // Turn into HtmlDocument using Html Agility
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);


                // File_down is the ID for the file download click handler
                var form = doc.GetElementbyId("file_down");

                // Iterate through each input field to get the parameters for the file download
                foreach (HtmlNode node in form.ChildNodes.Where(x => x.Name == "input"))
                {
                    result.Add(node.Id, node.GetAttributeValue("value", ""));
                }

                if (result.Count != 6) throw new UserException("The result did not contain the correct number of parameters");

                return result;
            }



            // For reference, this is the content of the request
            //Accept: text / html,application / xhtml + xml,application / xml; q = 0.9,image / webp,image / apng,*/*;q=0.8,application/signed-exchange;v=b3
            //Accept-Encoding: gzip, deflate, br
            //Accept-Language: en-US,en;q=0.9
            //Cache-Control: no-cache
            //Connection: keep-alive
            //Content-Length: 103
            //Content-Type: application/x-www-form-urlencoded
            //Cookie: _ga=GA1.2.72554178.1550479371; __gads=ID=c37502f7e147b895:T=1550479374:S=ALNI_MbFZ_MSj2DiFlALsvSynhlg2oHQtA; complianceCookie=on; _gid=GA1.2.454641251.1554661739; _gat_gtag_UA_115229803_1=1; _gat_gtag_UA_136162586_1=1; bfp_sn_rf_2a9d14d67e59728e1b5b2c86cb4ac6c4=Direct; bfp_sn_rt_2a9d14d67e59728e1b5b2c86cb4ac6c4=1554696347487; bfp_sn_pl=1554696347_366158981436; mnet_session_depth=2%7C1554696347154
            //Host: www.histdata.com
            //Origin: https://www.histdata.com
            //Pragma: no-cache
            //Referer: https://www.histdata.com/download-free-forex-historical-data/?/ascii/tick-data-quotes/gbpusd/2019/3
            //Upgrade-Insecure-Requests: 1
            //User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.86 Safari/537.36
            public async Task DownloadFile()
            {
                // Set the parameters for the request
                Dictionary<string, string> postParams = new Dictionary<string, string>();

                postParams.Add("tk", "b21f60d984a98efb085a0c4e2f9ae587");
                postParams.Add("date", "2019");
                postParams.Add("datemonth", "201903");
                postParams.Add("platform", "ASCII");
                postParams.Add("timeframe", "T");
                postParams.Add("fxpair", "GBPUSD");

                var requestContent = new FormUrlEncodedContent(postParams);

                //var url = "https://www.histdata.com/download-free-forex-historical-data/?/ascii/tick-data-quotes/gbpusd/2019/3";
                var url = "https://www.histdata.com/get.php";

                var client = new HttpClient();

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

                request.Content = requestContent;
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                client.DefaultRequestHeaders.Add("Host", "www.histdata.com");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Pragma", "no-cache");
                client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                client.DefaultRequestHeaders.Add("Origin", "https://www.histdata.com");
                client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                client.DefaultRequestHeaders.Add("Referer", "https://www.histdata.com/download-free-forex-historical-data/?/ascii/tick-data-quotes/gbpusd/2019/3");
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));

                var response = client.SendAsync(request).Result;

                using (var file = System.IO.File.Create("C:\\Users\\Ed\\Desktop\\test.zip"))
                {
                    // create a new file to write to
                    var contentStream = response.Content.ReadAsStreamAsync().Result; // get the actual content stream
                    contentStream.CopyTo(file); // copy that stream to the file stream
                }
            }
        }
    }
}
