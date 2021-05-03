using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace master.Contracts
{

    public class WebCrawlerMemContainer
    {
        public Dictionary<string, WebCrawlerData> WebCrawlerDict { get; set; }
        public WebCrawlerMemContainer()
        {
            this.WebCrawlerDict = new Dictionary<string, WebCrawlerData>();
        }
    }

    public class WebCrawlerwebReponse
    {
        public List<WebCrawlerData> Data { get; set; }
        public WebCrawlerwebReponse()
        {
            this.Data = new List<WebCrawlerData>();
        }
    }

    public class WebCrawlerData
    {
        public string Domain { get; set; }
        public DateTime LastUpdate { get; set; }

        public List<WebCrawlerNode>  CrawlerData { get; set; }
        public WebCrawlerData()
        {
            this.CrawlerData = new List<WebCrawlerNode>();
        }
    }

    public class WebCrawlerNode
    {
        public string Title { get; set; }
        public string Url { get; set; }

    }
}
