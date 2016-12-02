using System;
using BioEngine.Common.DB;
using BioEngine.Site.Base;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Controllers
{
    public class FilesController : BaseController
    {
        public FilesController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager)
            : base(context, parentEntityProvider, urlManager)
        {
        }

        [HttpGet("/{parentUrl}/files/{*url}")]
        public IActionResult Show(string parentUrl, string url)
        {
            throw new NotImplementedException();
        }
    }
}