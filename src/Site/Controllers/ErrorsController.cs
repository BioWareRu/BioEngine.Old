using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using BioEngine.Site.Base;
using BioEngine.Site.Extensions;
using BioEngine.Site.ViewModels.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Controllers
{
    public class ErrorsController : BaseController
    {
        public ErrorsController(IMediator mediator, IOptions<AppSettings> appSettingsOptions,
            IContentHelperInterface contentHelper) : base(mediator,
            appSettingsOptions, contentHelper)
        {
        }

        [Route("/error/{errorCode}")]
        public IActionResult Error(int errorCode, [FromServices] ILogger<ErrorsController> logger)
        {
            if (errorCode == 404)
            {
                logger.LogError($"Page not found. Url: {HttpContext.Request.AbsoluteUrl()}");
            }
            return View("Error", new ErrorsViewModel(ViewModelConfig, errorCode));
        }
    }
}