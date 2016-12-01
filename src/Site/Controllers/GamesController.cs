using System.Linq;
using BioEngine.Common.DB;
using BioEngine.Site.Base;
using BioEngine.Site.ViewModels.Games;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Site.Controllers
{
    public class GamesController : BaseController
    {
        public GamesController(BWContext context) : base(context)
        {
        }

        [HttpGet("/{gameUrl:regex(^[[a-z0-9_]]+$)}.html")]
        public IActionResult Index(string gameUrl)
        {
            var game = Context.Games.Include(x => x.Developer).FirstOrDefault(x => x.Url == gameUrl);
            if (game == null)
            {
                return new NotFoundResult();
            }

            var lastNews =
                Context.News.Where(x => x.GameId == game.Id && x.Pub == 1).OrderByDescending(x => x.Id).Take(5).ToList();
            var lastArticles =
                Context.Articles.Where(x => x.GameId == game.Id && x.Pub == 1).OrderByDescending(x => x.Id).Take(5).ToList();
            var lastFiles =
                Context.Files.Where(x => x.GameId == game.Id).OrderByDescending(x => x.Id).Take(5).ToList();
            var lastPics =
                Context.GalleryPics.Where(x => x.GameId == game.Id && x.Pub == 1).OrderByDescending(x => x.Id).Take(5).ToList();

            var view = new GamePageViewModel(Settings, game, lastNews, lastArticles, lastFiles, lastPics);
            return View(view);
        }
    }
}