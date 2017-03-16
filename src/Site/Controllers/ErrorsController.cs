using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Site.Base;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using BioEngine.Site.ViewModels.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Controllers
{
    public class ErrorsController : BaseController
    {
        public ErrorsController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager,
            IOptions<AppSettings> appSettingsOptions) : base(context, parentEntityProvider, urlManager,
            appSettingsOptions)
        {
        }

        [Route("/error/{errorCode}")]
        public IActionResult Error(int errorCode)
        {
            return View("Error", new ErrorsViewModel(ViewModelConfig, errorCode));
        }
    }
}