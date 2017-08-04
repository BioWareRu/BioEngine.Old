using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Requests;
using BioEngine.Data.Gallery.Requests;
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

                var catPics = await Mediator.Send(new GetGalleryPicsRequest(cat: category, page: page));
                category.Items = catPics.pics;

                var viewModel = new GalleryCatViewModel(ViewModelConfig, category, catPics.count, page);
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

            return await Mediator.Send(new GetGalleryCategoryRequest(parent, url: url, loadChildren: loadChildren,
                loadLastItems: loadLastItems));
        }

        /*[HttpGet("/{parentUrl}/gallery")]
        [HttpGet("/gallery/{parentUrl}/")]*/
        public async Task<IActionResult> ParentGallery(string parentUrl)
        {
            var parent = await Mediator.Send(new GetParentByUrlRequest(parentUrl));
            if (parent == null)
            {
                return new NotFoundResult();
            }

            var cats = await Mediator.Send(new GetGalleryCategoriesRequest(parent, loadChildren: true,
                loadLastItems: 5));

            return View("ParentGallery", new ParentGalleryViewModel(ViewModelConfig, parent, cats));
        }
    }
}