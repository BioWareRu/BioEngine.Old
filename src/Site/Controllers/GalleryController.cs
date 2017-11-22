using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Gallery.Queries;
using BioEngine.Routing;
using BioEngine.Site.Base;
using BioEngine.Site.ViewModels;
using BioEngine.Site.ViewModels.Gallery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{parentUrl}/gallery/pic/{picId}.html")]
        public async Task<IActionResult> PicUrl(string parentUrl, int picId, [FromServices] BioUrlManager bioUrlManager)
        {
            var pic = await Mediator.Send(new GetGalleryPicByIdQuery(picId));
            if (pic == null)
            {
                return new NotFoundResult();
            }

            return new RedirectResult(bioUrlManager.Gallery.DisplayUrl(pic, true) +
                                      $"#nanogallery/nanoGallery/0/{pic.Id}");
        }


        [HttpGet("/{parentUrl}/gallery/{*url}")]
        public async Task<IActionResult> Cat(string parentUrl, string url)
        {
            var parent = await Mediator.Send(new GetParentByUrlQuery(parentUrl));
            if (parent == null)
            {
                return new NotFoundResult();
            }
            var parsed = ParseCatchAll(url, out string catUrl, out int page);
            if (!parsed)
            {
                return new NotFoundResult();
            }
            var category = await GetCat(parent, catUrl, true, 0);
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

                var catPics = await Mediator.Send(new GetGalleryPicsQuery
                {
                    Cat = category,
                    Page = page,
                    PageSize = GalleryCat.PicsOnPage
                });
                category.Items = catPics.models;

                var viewModel = new GalleryCatViewModel(ViewModelConfig, category, catPics.totalCount, page);
                breadcrumbs.Reverse();
                viewModel.BreadCrumbs.AddRange(breadcrumbs);
                return View("GalleryCat", viewModel);
            }
            return StatusCode(404);
        }

        private async Task<GalleryCat> GetCat(IParentModel parent, string catUrl, bool loadChildren = false,
            int? loadLastItems = null)
        {
            var url = catUrl.Split('/').Last();

            return await Mediator.Send(new GetGalleryCategoryQuery
            {
                Parent = parent,
                Url = url,
                LoadChildren = loadChildren,
                LoadLastItems = loadLastItems
            });
        }

        /*[HttpGet("/{parentUrl}/gallery")]
        [HttpGet("/gallery/{parentUrl}/")]*/
        public async Task<IActionResult> ParentGallery(string parentUrl)
        {
            var parent = await Mediator.Send(new GetParentByUrlQuery(parentUrl));
            if (parent == null)
            {
                return new NotFoundResult();
            }

            var cats = await Mediator.Send(new GetGalleryCategoriesQuery
            {
                Parent = parent,
                LoadChildren = true,
                LoadLastItems = 5
            });

            return View("ParentGallery", new ParentGalleryViewModel(ViewModelConfig, parent, cats.models));
        }
    }
}