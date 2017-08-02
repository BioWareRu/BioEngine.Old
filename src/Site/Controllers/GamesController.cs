using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using BioEngine.Site.Base;
using BioEngine.Site.ViewModels.Games;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Controllers
{
    public class GamesController : BaseController
    {
        public GamesController(IMediator mediator,
            IOptions<AppSettings> appSettingsOptions, IContentHelperInterface contentHelper)
            : base(mediator, appSettingsOptions, contentHelper)
        {
        }


        [HttpGet("/{gameUrl:regex(^[[a-z0-9_]]+$)}.html", Order = 2)]
        public async Task<IActionResult> Index(string gameUrl)
        {
            var game = await Context.Games.Include(x => x.Developer).FirstOrDefaultAsync(x => x.Url == gameUrl);
            if (game == null)
                return new NotFoundResult();

            var lastNews =
                await Context.News.Where(x => (x.GameId == game.Id) && (x.Pub == 1))
                    .OrderByDescending(x => x.Id)
                    .Take(5)
                    .ToListAsync();
            var lastArticles =
                await Context.Articles.Where(x => (x.GameId == game.Id) && (x.Pub == 1))
                    .OrderByDescending(x => x.Id)
                    .Take(5)
                    .ToListAsync();
            var lastFiles =
                await Context.Files.Where(x => x.GameId == game.Id).OrderByDescending(x => x.Id).Take(5).ToListAsync();
            var lastPics =
                await Context.GalleryPics.Where(x => (x.GameId == game.Id) && (x.Pub == 1))
                    .OrderByDescending(x => x.Id)
                    .Take(5)
                    .ToListAsync();

            var view = new GamePageViewModel(ViewModelConfig, game, lastNews, lastArticles, lastFiles, lastPics);
            return View(view);
        }
    }
}