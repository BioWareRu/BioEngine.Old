using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Components.Url
{
    public class DeveloperUrlManager : EntityUrlManager
    {
        public DeveloperUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper)
            : base(settings, dbContext, urlHelper)
        {
        }

        public string PublicUrl(Developer developer)
        {
            return "#";
        }
    }
}