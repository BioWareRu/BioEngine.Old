using System;
using BioEngine.Routing;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.ViewModels.News
{
    public class ShowNewsViewModel
    {
        private readonly IUrlHelper _urlHelper;

        public ShowNewsViewModel(Common.Models.News news, bool showFull, IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
            News = news;
            ShowFull = showFull;
        }

        public Common.Models.News News { get; }
        public bool ShowFull { get; }

        public DateTimeOffset Date => DateTimeOffset.FromUnixTimeSeconds(News.Date);

        public string CommentsUrl => _urlHelper.News().CommentsUrl(News);

        public string NewsUrl => _urlHelper.News().PublicUrl(News, true);
    }
}