using BioEngine.Site.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BioEngine.Site.ViewModels.News
{
    public class ShowNewsViewModel
    {
        public ShowNewsViewModel(Common.Models.News news, bool showFull, UrlManager urlManager)
        {
            News = news;
            ShowFull = showFull;
            UrlManager = urlManager;
        }

        public Common.Models.News News { get; }
        public bool ShowFull { get; } = false;
        public UrlManager UrlManager { get; }

        public DateTimeOffset Date
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(News.Date);
            }
        }

        public string NewsUrl(IUrlHelper urlHelper)
        {
            return urlHelper.Action("Show", "News", new { year = Date.Year, month = Date.Month, day = Date.Day, url = News.Url }, urlHelper.ActionContext.HttpContext.Request.Scheme);
        }

        public string ParentIconUrl
        {
            get
            {
                return UrlManager.GetParentIconUrl((dynamic)News.Parent);
            }
        }
    }
}
