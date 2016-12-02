using System;
using BioEngine.Common.DB;
using BioEngine.Site.Base;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Controllers
{
    public class ArticlesController : BaseController
    {
        public ArticlesController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager)
            : base(context, parentEntityProvider, urlManager)
        {
        }

        [HttpGet("/{parentUrl}/articles/{catUrl:regex(^[[a-z0-9_\\/]]+$)}/{articleUrl}.html")]
        public IActionResult Show(string parentUrl, string catUrl, string articleUrl)
        {
            throw new NotImplementedException();
        }
    }
}