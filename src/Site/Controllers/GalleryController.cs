using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Requests;
using BioEngine.Routing;
using BioEngine.Site.Base;
using BioEngine.Site.ViewModels;
using BioEngine.Site.ViewModels.Gallery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Controllers
{
    public class GalleryController : BaseController
    {
        public GalleryController(IMediator mediator,
            IOptions<AppSettings> appSettingsOptions, IContentHelperInterface contentHelper)
            : base(mediator, appSettingsOptions, contentHelper)
        {
        }


        [HttpGet("/{parentUrl}/gallery/{*url}")]
        public async Task<IActionResult> Cat(string parentUrl, string url)
        {
            var parent = await Mediator.Send(new GetParentByUrlRequest(parentUrl));
            if (parent == null)
            {
                return new NotFoundResult();
            }
            var parsed = ParseCatchAll(url, out string catUrl, out int page);
            if (!parsed)
            {
                return new NotFoundResult();
            }
            var category = await GetCat(parent, catUrl);
            if (category != null)
            {
                var breadcrumbs = new List<BreadCrumbsItem>();
                var parentCat = category.ParentCat;
                while (parentCat != null)
                {
                    breadcrumbs.Add(new BreadCrumbsItem(Url.Gallery().CatPublicUrl(parentCat),
                        parentCat.Title));
                    parentCat = parentCat.ParentCat;
                }
                breadcrumbs.Add(new BreadCrumbsItem(Url.Gallery().ParentGalleryUrl(parent), "Галерея"));
                breadcrumbs.Add(new BreadCrumbsItem(Url.Base().ParentUrl(parent), parent.DisplayTitle));

                await Context.Entry(category).Collection(x => x.Children).LoadAsync();

                var children = new List<CatsTree<GalleryCat, GalleryPic>>();
                foreach (var child in category.Children)
                {
                    children.Add(new CatsTree<GalleryCat, GalleryPic>(child, await GetPics(child, 5)));
                }

                var viewModel = new GalleryCatViewModel(ViewModelConfig, category, children,
                    await GetPics(category, page: page),
                    await Context.GalleryPics.CountAsync(x => x.CatId == category.Id),
                    page);
                breadcrumbs.Reverse();
                viewModel.BreadCrumbs.AddRange(breadcrumbs);
                return View("GalleryCat", viewModel);
            }
            return StatusCode(404);
        }

        private async Task<List<GalleryPic>> GetPics(ICat<GalleryCat> cat, int count = 24, int page = 1)
        {
            return await Context.GalleryPics.Where(x => x.CatId == cat.Id)
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * count)
                .Take(count)
                .ToListAsync();
        }

        private async Task<GalleryCat> GetCat(IParentModel parent, string catUrl)
        {
            var url = catUrl.Split('/').Last();

            var catQuery = Context.GalleryCats.Where(x => x.Url == url);
            switch (parent.Type)
            {
                case ParentType.Game:
                    catQuery = catQuery.Where(x => x.GameId == (int) parent.GetId());
                    break;
                case ParentType.Developer:
                    catQuery = catQuery.Where(x => x.DeveloperId == (int) parent.GetId());
                    break;
                case ParentType.Topic:
                    catQuery = catQuery.Where(x => x.TopicId == (int) parent.GetId());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var cat = await catQuery.FirstOrDefaultAsync();
            return cat;
        }

        [HttpGet("/{parentUrl}/gallery.html")]
        [HttpGet("/{parentUrl}/gallery")]
        [HttpGet("/gallery/{parentUrl}/")]
        public async Task<IActionResult> ParentGallery(string parentUrl)
        {
            var parent = Mediator.Send(new GetParentByUrlRequest(parentUrl));
            if (parent == null)
            {
                return new NotFoundResult();
            }

            var cats = await LoadCatsTree(parent, Context.GalleryCats, async cat => await GetPics(cat, 5));

            return View("ParentGallery", new ParentGalleryViewModel(ViewModelConfig, parent, cats));
        }
    }
}