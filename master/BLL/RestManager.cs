using master.Contracts;
using master.DAL;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace master.BLL
{
    public class RestManager
    {
        private RestProxy proxy;
        public CacheHandler CacheHandlerObj { get; set; }
        protected CommonHelper commonHelper;

        public RestManager(string serviceUri, IMemoryCache cache)
        {
            this.proxy = new RestProxy(serviceUri);
            this.CacheHandlerObj = new CacheHandler(cache);
            this.commonHelper = new CommonHelper();
        }

        /// <summary>
        /// run tasks by parralel
        /// </summary>
        /// <param name="urls">request data</param>
        /// <returns>web crawler response</returns>
        public async Task<bool> GetReponseByUrls(List<string> urls)
        {
            try
            {
                //check if uri are valid
                this.commonHelper.CheckUrlArrayIsValid(ref urls);
                //check if uri allready exists on cache
                this.CheckIfUriExistsOnCache(ref urls);

                List<Task<WebCrawlerNode>> tasks = new List<Task<WebCrawlerNode>>();
                for (int i = 0; i < urls.Count; i++)
                {
                    tasks.Add(proxy.GetRestCallAsyncResponse(urls[i]));
                }
                var results = await Task.WhenAll(tasks);
                this.SetResultsOnCache(results.ToList());
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<WebCrawlerwebReponse> GetCacheData(int minutes)
        {
            return await Task.Run(() =>
            {
                var memData= this.CacheHandlerObj.GetMainObjFromCache();
                var result=memData.WebCrawlerDict.Values.ToList();
                result.RemoveAll(a => a.LastUpdate.AddMinutes(minutes) <= DateTime.Now);
                return new WebCrawlerwebReponse() {  Data= result };
            });
        }

        /// <summary>
        /// set the webcrawler results on cache
        /// </summary>
        /// <param name="results"></param>
        private void SetResultsOnCache(List<WebCrawlerNode> results)
        {
            foreach(var item in results)
            {
                Uri urilocal=null;
                if (Uri.TryCreate(item.Url, UriKind.Absolute, out urilocal))
                {
                    var domain= this.commonHelper.GetDomainFromUri(urilocal);
                    this.CacheHandlerObj.AddDataToCahce(domain, item);
                }
            }
        }

        /// <summary>
        /// checking if uri exits on cache before sending rest request
        /// </summary>
        /// <param name="urls">request data before sending to crawler</param>
        private void CheckIfUriExistsOnCache(ref List<string> urls)
        {
            urls.RemoveAll(delegate (string uri)
            {
                Uri urilocal=null;
                if (Uri.TryCreate(uri, UriKind.Absolute, out urilocal))
                {
                    var domain= this.commonHelper.GetDomainFromUri(urilocal);
                    if(!string.IsNullOrEmpty(domain))
                    {
                        return this.CacheHandlerObj.CheckIfUrlExistsOnDomainCache(domain, uri);
                    }
                }
                return true;
            });
        }
    }
}
