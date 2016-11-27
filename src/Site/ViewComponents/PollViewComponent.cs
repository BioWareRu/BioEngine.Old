using BioEngine.Common.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BioEngine.Site.ViewComponents
{
    public class PollViewComponent : ViewComponent
    {
        private readonly BWContext dbContext;

        public PollViewComponent(BWContext context)
        {
            dbContext = context;
            //SignInManager = signInManager;
            //UserManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var poll = await dbContext.Polls.Where(x => x.OnOff == 1).OrderByDescending(x => x.PollId).FirstOrDefaultAsync();

            bool voted = false;
            if (User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(Request.HttpContext.User.Claims.Where(x => x.Type == "userId").FirstOrDefault().Value);

                voted = await dbContext.PollVotes.AnyAsync(x => x.UserId == userId && x.PollId == poll.PollId);
            }
            else
            {
                voted = await dbContext.PollVotes.AnyAsync(x => x.UserId == 0 && x.PollId == poll.PollId && x.Ip == Request.Headers["REMOTE_ADDR"].ToString());
            }

            if (voted) poll.SetVoted();
            return View(poll);
        }
    }
}
