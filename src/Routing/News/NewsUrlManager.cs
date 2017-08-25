using System;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Routing.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BioEngine.Routing.News
{
    public class NewsUrlManager : UrlManager<NewsRoutesEnum>
    {
        public NewsUrlManager(IUrlHelper urlHelper, IOptions<AppSettings> appSettings) : base(urlHelper, appSettings)
        {
        }

        public Uri PublicUrl(Common.Models.News news, bool absolute = false)
        {
            var date = DateTimeOffset.FromUnixTimeSeconds(news.Date);
            return GetUrl(NewsRoutesEnum.NewsPage,
                new
                {
                    year = date.Year.ToString("D4"),
                    month = date.Month.ToString("D2"),
                    day = date.Day.ToString("D2"),
                    url = news.Url
                }, absolute);
        }

        public Uri IndexUrl()
        {
            return GetUrl(NewsRoutesEnum.Index);
        }

        public Uri IndexUrl(int page)
        {
            return GetUrl(NewsRoutesEnum.IndexWithPage, new {page});
        }

        public Uri NewsByYear(int year)
        {
            return GetUrl(NewsRoutesEnum.NewsByYear, new {year});
        }

        public Uri NewsByYear(int year, int page)
        {
            return GetUrl(NewsRoutesEnum.NewsByYearWithPage, new {year, page});
        }

        public Uri NewsByMonth(int year, int? month)
        {
            return GetUrl(NewsRoutesEnum.NewsByMonth, new {year, month});
        }

        public Uri NewsByMonth(int year, int? month, int page)
        {
            return GetUrl(NewsRoutesEnum.NewsByMonthWithPage, new {year, month, page});
        }

        public Uri NewsByDay(int year, int? month, int? day)
        {
            return GetUrl(NewsRoutesEnum.NewsByDay, new {year, month, day,});
        }

        public Uri NewsByDay(int year, int? month, int? day, int page)
        {
            return GetUrl(NewsRoutesEnum.NewsByDayWithPage, new {year, month, day, page});
        }

        public Uri ParentNewsUrl(IChildModel news, int? page = null, bool absolute = false)
        {
            return ParentNewsUrl(news.Parent, page, absolute);
        }

        public Uri ParentNewsUrl(IParentModel parent, int? page = null, bool absolute = false)
        {
            return GetUrl(page > 0 ? NewsRoutesEnum.NewsByParentWithPage : NewsRoutesEnum.NewsByParent,
                new {parentUrl = parent.ParentUrl, page}, absolute);
        }

        public Uri CommentsUrl(Common.Models.News news)
        {
            return news.ForumTopicId != null
                ? new Uri($"{Settings.IPBDomain}/topic/{news.ForumTopicId}/?do=getNewComment")
                : null;
        }
    }
}