using master.BLL;
using master.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace master.Controllers
{
    public class WebBaseController : Controller
    {
        protected IConfiguration webAppSettings;
        protected RestManager restManager;


        public WebBaseController(IConfiguration settingsData, IMemoryCache cache)
        {
            this.webAppSettings = settingsData;
            this.restManager = new RestManager(this.webAppSettings.GetValue<string>("CrawlerUrl"), cache);
        }
    }
}
