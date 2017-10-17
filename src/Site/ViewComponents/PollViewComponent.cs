using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BioEngine.Data.Polls.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.ViewComponents
{
    public class PollViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public PollViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var poll = await _mediator.Send(new GetActivePollQuery());

            bool voted;
            var userId = 0;
            var sessionId = HttpContext.Session.Id;
            if (User.Identity.IsAuthenticated)
            {
                userId =
                    int.Parse(Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
                        ?.Value);
            }

            voted = await _mediator.Send(new IsPollVotedByUserQuery(poll.Id, userId,
                Request.HttpContext.Connection.RemoteIpAddress.ToString(), sessionId));

            if (voted) poll.SetVoted();
            return View(poll);
        }
    }
}