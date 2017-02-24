using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Components.Url
{
    public class TopicUrlManager : EntityUrlManager
    {
        public TopicUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper,
            ParentEntityProvider parentEntityProvider)
            : base(settings, dbContext, urlHelper, parentEntityProvider)
        {
        }

        public string PublicUrl(Topic topic)
        {
            return "#";
        }
    }
}