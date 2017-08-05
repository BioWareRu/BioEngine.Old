using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Site.Base;
using BioEngine.Site.ViewModels.Errors;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
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
                var feature = HttpContext.Features.Get<IHttpRequestFeature>();
                logger.LogError($"Page not found. Url: {feature.RawTarget}");
            }
            return View("Error", new ErrorsViewModel(ViewModelConfig, errorCode));
        }
    }
}