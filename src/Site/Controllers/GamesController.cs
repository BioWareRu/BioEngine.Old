using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Articles.Requests;
using BioEngine.Data.Base.Requests;
using BioEngine.Data.Files.Requests;
using BioEngine.Data.News.Requests;
using BioEngine.Site.Base;
using BioEngine.Site.ViewModels.Games;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
            var game = await Mediator.Send(new GetGameByUrlRequest(gameUrl));
            if (game == null)
                return new NotFoundResult();

            var canUserSeeUnpublishedNews = await HasRight(UserRights.News);
            var lastNews =
                (await Mediator.Send(new GetNewsRequest(canUserSeeUnpublishedNews, 1, game) {PageSize = 5})).news;

            var canUserSeeUnpublishedArticles = await HasRight(UserRights.Articles);
            var lastArticles =
                (await Mediator.Send(new GetArticlesRequest(canUserSeeUnpublishedArticles, 1, game) {PageSize = 5}))
                .articles;

            var lastFiles =
                (await Mediator.Send(new GetFilesRequest(1, game) {PageSize = 5})).files;

            var canUserSeeUnpublishedGalleryPics = await HasRight(UserRights.Gallery);
            var lastPics =
                await (await Mediator.Send(new GetGalleryPicsRequest(canUserSeeUnpublishedGalleryPics, 1, game) { PageSize = 5 })).pics;

            var view = new GamePageViewModel(ViewModelConfig, game, lastNews, lastArticles, lastFiles, lastPics);
            return View(view);
        }
    }
}