using System;
using BioEngine.Common.Models;
using BioEngine.Site.Components;
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

        public string ParentIconUrl => UrlManager.News.ParentIconUrl((dynamic) News.Parent);

        public string NewsUrl => UrlManager.News.PublicUrl(News, true);
    }
}