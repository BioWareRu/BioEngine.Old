using System;
using BioEngine.Common.DB;
using BioEngine.Site.Base;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Controllers
{
    public class SearchController : BaseController
    {
        public SearchController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager,
            IOptions<AppSettings> appSettingsOptions)
            : base(context, parentEntityProvider, urlManager, appSettingsOptions)
        {
        }

        public IAsyncResult Search(string query)
        {
            throw new NotImplementedException();
        }
    }
}