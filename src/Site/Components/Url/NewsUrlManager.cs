using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Components.Url
{
    public class NewsUrlManager : EntityUrlManager
    {
        public NewsUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper)
            : base(settings, dbContext, urlHelper)
        {
        }

        public string PublicUrl(News news, bool absolute = false)
        {
            var date = DateTimeOffset.FromUnixTimeSeconds(news.Date);
            return GetUrl("Show", "News",
                new {year = date.Year, month = date.Month, day = date.Day, url = news.Url}, absolute);
        }
    }
}