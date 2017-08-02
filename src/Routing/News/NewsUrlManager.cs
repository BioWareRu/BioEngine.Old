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

        public string PublicUrl(Common.Models.News news, bool absolute = false)
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

        public string IndexUrl()
        {
            return GetUrl(NewsRoutesEnum.Index);
        }

        public string IndexUrl(int page)
        {
            return GetUrl(NewsRoutesEnum.IndexWithPage, new {page});
        }

        public string NewsByYear(int year)
        {
            return GetUrl(NewsRoutesEnum.NewsByYear, new {year});
        }

        public string NewsByYear(int year, int page)
        {
            return GetUrl(NewsRoutesEnum.NewsByYearWithPage, new {year, page});
        }

        public string NewsByMonth(int year, int? month)
        {
            return GetUrl(NewsRoutesEnum.NewsByMonth, new {year, month});
        }

        public string NewsByMonth(int year, int? month, int page)
        {
            return GetUrl(NewsRoutesEnum.NewsByMonthWithPage, new {year, month, page});
        }

        public string NewsByDay(int year, int? month, int? day)
        {
            return GetUrl(NewsRoutesEnum.NewsByDay, new {year, month, day,});
        }

        public string NewsByDay(int year, int? month, int? day, int page)
        {
            return GetUrl(NewsRoutesEnum.NewsByDayWithPage, new {year, month, day, page});
        }

        public string ParentNewsUrl(IChildModel news, int? page = null, bool absolute = false)
        {
            return ParentNewsUrl(news.Parent, page, absolute);
        }

        public string ParentNewsUrl(IParentModel parent, int? page = null, bool absolute= false)
        {
            return GetUrl(page > 0 ? NewsRoutesEnum.NewsByParentWithPage : NewsRoutesEnum.NewsByParent,
                new {parentUrl = parent.ParentUrl, page}, absolute);
        }

        public string CommentsUrl(Common.Models.News news)
        {
            return $"{Settings.IPBDomain}/topic/{news.ForumTopicId}/?do=getNewComment";
        }
    }
}