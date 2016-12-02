using System;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Site.Controllers;
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

        public string IndexUrl()
        {
            return UrlHelper.Action<NewsController>(x => x.Index());
        }

        public string CommentsUrl(News news)
        {
            return $"{Settings.IPBDomain}/topic/{news.ForumTopicId}/?do=getNewComment";
        }

        public string ParentNewsUrl(Developer developer)
        {
            return UrlHelper.Action<NewsController>(x => x.NewsList(developer.Url));
        }

        public string ParentNewsUrl(Game game)
        {
            return UrlHelper.Action<NewsController>(x => x.NewsList(game.Url));
        }

        public string ParentNewsUrl(Topic topic)
        {
            return UrlHelper.Action<NewsController>(x => x.NewsList(topic.Url));
        }
    }
}