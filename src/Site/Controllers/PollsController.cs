using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Data.Polls.Commands;
using BioEngine.Data.Polls.Requests;
using BioEngine.Site.Base;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Controllers
{
    public class PollsController : BaseController
    {
        public PollsController(IMediator mediator, IOptions<AppSettings> appSettingsOptions,
            IContentHelperInterface contentHelper) : base(mediator,
            appSettingsOptions, contentHelper)
        {
        }

        /*[HttpPost("polls/vote.html")]
        public async Task<IActionResult> Vote([FromForm] Dictionary<int, int> votes)
        {
            return null;
        }*/

        [HttpPost("polls/{pollId}/vote.html")]
        public async Task<IActionResult> Vote(int pollId, [FromForm] int vote)
        {
            var poll = await Mediator.Send(new GetPollByIdRequest(pollId));
            if (poll == null)
            {
                return new NotFoundResult();
            }
            var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var returnUrl = Request.Headers["REFERER"].ToString();
            var userId = 0;
            var userLogin = "guest";
            var sessionId = HttpContext.Session.Id;
            if (User.Identity.IsAuthenticated)
            {
                userId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
                userLogin = User.Identity.Name;
            }
            if (await Mediator.Send(new IsPollVotedByUserRequest(poll.Id, userId, ip, sessionId)))
            {
                return new RedirectResult(returnUrl);
            }

            await Mediator.Publish(new PollVoteCommand(poll.Id, vote, ip, sessionId, userLogin, userId));

            await Mediator.Publish(new PollRecountCommand(poll));
            HttpContext.Session.SetInt32("voted", poll.Id);
            return new RedirectResult(returnUrl);
        }
    }
}