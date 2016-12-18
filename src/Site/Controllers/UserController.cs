using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Site.Base;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Controllers
{
    public class UserController : BaseController
    {
        public UserController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager)
            : base(context, parentEntityProvider, urlManager)
        {
        }

        public IActionResult Login([FromForm] string login, [FromForm] string password)
        {
            throw new NotImplementedException();
        }
    }
}