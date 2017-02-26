using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Site.ViewComponents
{
    public class PollViewComponent : ViewComponent
    {
        private readonly BWContext _dbContext;

        public PollViewComponent(BWContext context)
        {
            _dbContext = context;
            //SignInManager = signInManager;
            //UserManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var poll =
                await _dbContext.Polls.Where(x => x.OnOff == 1).OrderByDescending(x => x.PollId).FirstOrDefaultAsync();

            bool voted;
            var userId = 0;
            var sessionId = HttpContext.Session.Id;
            if (User.Identity.IsAuthenticated)
            {
                userId =
                    int.Parse(Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            }
            voted =
                await poll.GetIsVoted(_dbContext, userId, Request.HttpContext.Connection.RemoteIpAddress.ToString(), sessionId);

            if (voted) poll.SetVoted();
            return View(poll);
        }
    }
}