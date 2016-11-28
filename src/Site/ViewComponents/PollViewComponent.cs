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
            if (User.Identity.IsAuthenticated)
            {
                var userId =
                    int.Parse(Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId").Value);

                voted = await _dbContext.PollVotes.AnyAsync(x => (x.UserId == userId) && (x.PollId == poll.PollId));
            }
            else
            {
                voted =
                    await
                        _dbContext.PollVotes.AnyAsync(
                            x =>
                                (x.UserId == 0) && (x.PollId == poll.PollId) &&
                                (x.Ip == Request.Headers["REMOTE_ADDR"].ToString()));
            }

            if (voted) poll.SetVoted();
            return View(poll);
        }
    }
}