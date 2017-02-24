using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Site.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Components.Url
{
    public class SearchUrlManager : EntityUrlManager
    {
        public SearchUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper,
            ParentEntityProvider parentEntityProvider) : base(settings,
            dbContext, urlHelper, parentEntityProvider)
        {
        }

        public string BlockUrl(string block, string term)
        {
            return UrlHelper.Action<SearchController>(x => x.Index(term, block));
        }
    }
}