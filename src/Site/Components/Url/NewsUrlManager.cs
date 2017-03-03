using System;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Site.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Components.Url
{
    public class NewsUrlManager : EntityUrlManager
    {
        public NewsUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper,
            ParentEntityProvider parentEntityProvider)
            : base(settings, dbContext, urlHelper, parentEntityProvider)
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

        public async Task<string> ParentNewsUrl(News news)
        {
            var parent = await ParentEntityProvider.GetModelParent(news);
            return ParentNewsUrl((dynamic) parent);
        }

        public async Task<string> ParentNewsUrl<T>(T parentModel) where T : IParentModel
        {
            return await Task.FromResult(UrlHelper.Action<NewsController>(x => x.NewsList(parentModel.ParentUrl)));
        }
    }
}