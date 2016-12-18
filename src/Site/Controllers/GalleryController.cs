using System;
using System.Collections.Generic;
using System.Linq;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Site.Base;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using BioEngine.Site.ViewModels;
using BioEngine.Site.ViewModels.Files;
using BioEngine.Site.ViewModels.Gallery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Controllers
{
    public class GalleryController : BaseController
    {
        public GalleryController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager,
            IOptions<AppSettings> appSettingsOptions)
            : base(context, parentEntityProvider, urlManager, appSettingsOptions)
        {
        }


        [HttpGet("/{parentUrl}/gallery/{*url}")]
        public IActionResult Cat(string parentUrl, string url)
        {
            string catUrl;
            int page;
            var parent = ParentEntityProvider.GetParenyByUrl(parentUrl);
            ParseCatchAll(url, out catUrl, out page);
            var category = GetCat(parent, catUrl);
            if (category != null)
            {
                var breadcrumbs = new List<BreadCrumbsItem>();
                var parentCat = category.ParentCat;
                while (parentCat != null)
                {
                    breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Gallery.CatPublicUrl(parentCat), parentCat.Title));
                    parentCat = parentCat.ParentCat;
                }
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Gallery.ParentGalleryUrl((dynamic) category.Parent),
                    "Галерея"));
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.ParentUrl(category.Parent), category.Parent.DisplayTitle));

                Context.Entry(category).Collection(x => x.Children).Load();
                var children =
                    category.Children.Select(child => new CatsTree<GalleryCat, GalleryPic>(child, GetPics(child, 5)))
                        .ToList();

                var viewModel = new GalleryCatViewModel(ViewModelConfig, category, children,
                    GetPics(category, page: page), Context.GalleryPics.Count(x => x.CatId == category.Id),
                    page);
                breadcrumbs.Reverse();
                viewModel.BreadCrumbs.AddRange(breadcrumbs);
                return View("GalleryCat", viewModel);
            }
            return StatusCode(404);
        }

        private List<GalleryPic> GetPics(ICat<GalleryCat> cat, int count = 24, int page = 0)
        {
            return Context.GalleryPics.Where(x => x.CatId == cat.Id)
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * count)
                .Take(count)
                .ToList();
        }

        private GalleryCat GetCat(ParentModel parent, string catUrl)
        {
            var url = catUrl.Split('/').Last();

            var catQuery = Context.GalleryCats.Where(x => x.Url == url);
            switch (parent.Type)
            {
                case ParentType.Game:
                    catQuery = catQuery.Where(x => x.GameId == parent.Id);
                    break;
                case ParentType.Developer:
                    catQuery = catQuery.Where(x => x.DeveloperId == parent.Id);
                    break;
                case ParentType.Topic:
                    catQuery = catQuery.Where(x => x.TopicId == parent.Id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var cat = catQuery.FirstOrDefault();
            if (cat != null)
            {
                cat.Parent = parent;
            }
            return cat;
        }

        [HttpGet("/{parentUrl}/gallery.html")]
        public IActionResult ParentGallery(string parentUrl)
        {
            var parent = ParentEntityProvider.GetParenyByUrl(parentUrl);
            if (parent == null) return StatusCode(404);

            var cats = LoadCatsTree(parent, Context.GalleryCats, cat => GetPics(cat, 5));

            return View("ParentGallery", new ParentGalleryViewModel(ViewModelConfig, parent, cats));
        }
    }
}