using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Components.Url
{
    public class DeveloperUrlManager : EntityUrlManager
    {
        public DeveloperUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper,
            ParentEntityProvider parentEntityProvider)
            : base(settings, dbContext, urlHelper, parentEntityProvider)
        {
        }

        public string PublicUrl(Developer developer, bool absolute = false)
        {
            return GetUrl("NewsList", "News", new {parentUrl = developer.Url}, absolute);
        }
    }
}