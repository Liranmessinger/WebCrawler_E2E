using master.Contracts;
using master.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebCrawlerController : WebBaseController
    {
        public WebCrawlerController(IConfiguration config, IMemoryCache memoryCache) : base(config, memoryCache)
        {
        }

        [Route("[action]/{minutes}")]
        [HttpGet]
        public async Task<WebCrawlerwebReponse> GetCrawlerData(int minutes)
        {
            var result=await base.restManager.GetCacheData(minutes == -1 ? base.webAppSettings.GetValue<int>("CachingMinutesValue") : minutes);
            return result;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> PostCrawlerData(List<string> urls)
        {
            try
            {
                if (await base.restManager.GetReponseByUrls(urls))
                    return Ok("ok");
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
