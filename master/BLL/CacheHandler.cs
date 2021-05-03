using master.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace master.BLL
{
    public class CacheHandler
    {
        private readonly IMemoryCache cacheObj;
        public CacheHandler(IMemoryCache cache)
        {
            this.cacheObj = cache;
        }

        public bool CheckIfUrlExistsOnDomainCache(string domain, string uri)
        {
            var cacheData=this.GetMainObjFromCache();
            if (cacheData != null)
            {
                if (cacheData.WebCrawlerDict.ContainsKey(domain))
                {
                    var crawlerData=cacheData.WebCrawlerDict[domain];
                    if (crawlerData.CrawlerData.Exists(a => a.Url == uri))
                        return true;
                }
            }
            return false;
        }

        public bool AddDataToCahce(string domain, WebCrawlerNode crawlerNode)
        {
            try
            {
                var cacheData=this.GetMainObjFromCache();
                if (cacheData == null)
                    cacheData = new WebCrawlerMemContainer();
                //check if domain exists on cache
                if (cacheData.WebCrawlerDict.ContainsKey(domain))
                {
                    var crawlerData=cacheData.WebCrawlerDict[domain];
                    //handle when uri non exists on domain
                    if (!crawlerData.CrawlerData.Exists(a => a.Url == crawlerNode.Url))
                    {
                        crawlerData.LastUpdate = DateTime.Now;
                        crawlerData.CrawlerData.Add(new WebCrawlerNode()
                        {
                            Url = crawlerNode.Url,
                            Title = crawlerNode.Title
                        });
                    }
                }
                else
                {
                    cacheData.WebCrawlerDict.Add(domain, new WebCrawlerData()
                    {
                        Domain = domain,
                        CrawlerData = { new WebCrawlerNode() { Title = crawlerNode.Title, Url = crawlerNode.Url } },
                        LastUpdate = DateTime.Now
                    });
                }
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                 .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                // Save data in cache.
                this.cacheObj.Set("crawlerCacheData", cacheData, cacheEntryOptions);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Error on AddDataToCahce() => {ex}");
                return false;
            }
        }


        public WebCrawlerMemContainer GetMainObjFromCache()
        {
            object value;
            if (this.cacheObj.TryGetValue("crawlerCacheData", out value) && value is WebCrawlerMemContainer)
                return value as WebCrawlerMemContainer;
            else
                return null;
        }
    }
}
