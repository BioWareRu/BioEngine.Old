using System;
using BioEngine.Common.Models;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using Microsoft.AspNetCore.Mvc;

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
        public bool ShowFull { get; }
        private UrlManager UrlManager { get; }

        public DateTimeOffset Date => DateTimeOffset.FromUnixTimeSeconds(News.Date);

        public string ParentIconUrl => UrlManager.ParentIconUrl((dynamic) News.Parent);
        public string ParentNewsUrl => UrlManager.News.ParentNewsUrl((dynamic) News.Parent);
        public string CommentsUrl => UrlManager.News.CommentsUrl(News);

        public string NewsUrl => UrlManager.News.PublicUrl(News, true);
    }
}