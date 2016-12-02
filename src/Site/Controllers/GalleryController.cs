using System;
using BioEngine.Common.DB;
using BioEngine.Site.Base;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Controllers
{
    public class GalleryController : BaseController
    {
        public GalleryController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager)
            : base(context, parentEntityProvider, urlManager)
        {
        }


        [HttpGet("/{parentUrl}/gallery/{catUrl}.html")]
        public IActionResult Cat(string parentUrl, string catUrl, int page)
        {
            throw new NotImplementedException();
        }
    }
}