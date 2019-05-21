using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Contracts;
using Contracts.DataManagers;
using Contracts.Enums;
using Contracts.Exceptions;
using HtmlAgilityPack;

namespace Logic
{
    public class FileDownloader : IFileDownloader
    {
        // Static variables
        public string BaseUrl { get; set; } = "https://www.histdata.com/";

        public string FullUrl { get; set; }

        public string DownloadUrl => this.BaseUrl + "get.php";

        public BasePair Pair { get; set; }
        public DateTime FromDate { get; set; } = new DateTime();
        public DateTime ToDate { get; set; } = new DateTime();
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
        public string BaseDownloadPath { get; set; } = "D:\\Projects\\Forex\\Downloads\\";

        public string DownloadFilePath { get; set; }
        public void Initialize(DateTime fromDate, DateTime toDate, BasePair pair)
        {
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.Pair = pair;
            this.DownloadFilePath = this.BaseDownloadPath + "Zip\\";
        }

        public async Task UnzipFiles()
        {

            var unzippedFiles = Dependency.Dependency.Resolve<IFileDownloadManager>().GetallUnzippedFiles();
            foreach (var file in unzippedFiles)
            {
                var filePath = this.DownloadFilePath + file.FileName;
                this.UnzipFile(filePath);
                Dependency.Dependency.Resolve<IFileDownloadManager>().SetFileDownloadStatusToUnzipped(file.FileDownloadStatusID);
            }
        }

        public async Task ProcessFiles()
        {
            
            // Iterate through each URL and attempt to download each page to get meta information
            while (this.FromDate <= this.ToDate)
            {
                var dateDescription = string.Format("{0}{1}", this.FromDate.Year, this.FromDate.Month.ToString("d2"));

                // Attempt to get the file record. 
                var fileDownloadStatus = Dependency.Dependency.Resolve<IFileDownloadManager>().GetFileDownloadStatusUsingDateDescriptionAndBasePair(dateDescription, this.Pair);

                var filePath = "";
                
                // If the file download has been completed, we skip.
                if (fileDownloadStatus != null && fileDownloadStatus.IsCompleted.GetValueOrDefault(false))
                {

                    // Iterate to the next possible date
                    this.FromDate = this.FromDate.AddMonths(1);
                    continue;
                }
                
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
                    client.DefaultRequestHeaders.Add("Origin", this.BaseUrl);
                    client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                    client.DefaultRequestHeaders.Add("Referer", this.FullUrl);
                    client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
                    client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));

                    var response = await client.SendAsync(request);
                    var fileName = "";
                    if (response.IsSuccessStatusCode)
                    {
                        fileName = response.Content.Headers.ContentDisposition.FileName;
                    }

                    // Record file in the database
                    fileDownloadStatus = Dependency.Dependency.Resolve<IFileDownloadManager>().SaveFileDownloadStatus(fileDownloadStatus?.FileDownloadStatusID, this.Pair, fileName, null, dateDescription, response.IsSuccessStatusCode, fileDownloadStatus?.IsUnzipped);

                    // Create the file and download it
                    filePath = this.DownloadFilePath + fileName;
                    using (var file = System.IO.File.Create(filePath))
                    {
                        var contentStream = await response.Content.ReadAsStreamAsync();
                        contentStream.CopyTo(file);
                        Dependency.Dependency.Resolve<IFileDownloadManager>().SetFileDownloadStatusToComplete(fileDownloadStatus.FileDownloadStatusID);
                    }

                    // Iterate to the next possible date
                    this.FromDate = this.FromDate.AddMonths(1);
                }

            }
        }

        private void UnzipFile(string zipPath)
        {
            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                        {
                            // Gets the full path to ensure that relative segments are removed.

                            string destinationPath = Path.GetFullPath(Path.Combine(this.BaseDownloadPath, entry.FullName));

                            entry.ExtractToFile(destinationPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            //System.IO.Compression.ZipFile.CreateFromDirectory(this.DownloadFilePath, zipPath);
        }

        private async Task<Dictionary<string, string>> GetRequestParamsAsync(int year, int month)
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


    }
}


