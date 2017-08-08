using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Articles.Queries;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Files.Queries;
using BioEngine.Data.Gallery.Queries;
using BioEngine.Data.News.Queries;
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


        //[HttpGet("/{gameUrl:regex(^[[a-z0-9_]]+$)}.html", Order = 2)]
        public async Task<IActionResult> Index(string gameUrl)
        {
            var game = await Mediator.Send(new GetGameByUrlQuery(gameUrl));
            if (game == null)
                return new NotFoundResult();

            var canUserSeeUnpublishedNews = await HasRight(UserRights.News);
            var lastNews =
            (await Mediator.Send(new GetNewsQuery
            {
                WithUnPublishedNews = canUserSeeUnpublishedNews,
                Page = 1,
                PageSize = 5,
                Parent = game
            })).models;

            var canUserSeeUnpublishedArticles = await HasRight(UserRights.Articles);
            var lastArticles =
                (await Mediator.Send(new GetArticlesQuery
                {
                    WithUnPublishedArticles = canUserSeeUnpublishedArticles,
                    Page = 1,
                    PageSize = 5,
                    Parent = game
                }))
                .models;

            var lastFiles =
                (await Mediator.Send(new GetFilesQuery {Parent = game, Page = 1, PageSize = 5})).models;

            var canUserSeeUnpublishedGalleryPics = await HasRight(UserRights.Gallery);
            var lastPics =
            (await Mediator.Send(
                new GetGalleryPicsQuery
                {
                    Parent = game,
                    WithUnPublishedPictures = canUserSeeUnpublishedGalleryPics,
                    Page = 1,
                    PageSize = 5,
                    LoadPicPositions = true
                })).models;

            var view = new GamePageViewModel(ViewModelConfig, game, lastNews, lastArticles, lastFiles, lastPics);
            return View(view);
        }
    }
}