using System;
using System.Collections.Generic;
using BioEngine.Common.Base;
using BioEngine.Site.Base;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Files.Commands;
using BioEngine.Data.Files.Queries;
using BioEngine.Routing;
using BioEngine.Site.ViewModels;
using BioEngine.Site.ViewModels.Files;
using MediatR;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Controllers
{
    public class FilesController : BaseController
    {
        public FilesController(IMediator mediator, IOptions<AppSettings> appSettingsOptions,
            IContentHelperInterface contentHelper)
            : base(mediator, appSettingsOptions, contentHelper)
        {
        }

        /*[HttpGet("/{parentUrl}/files")]
        [HttpGet("/files/{parentUrl}/")]*/
        public async Task<IActionResult> ParentFiles(string parentUrl)
        {
            var parent = await Mediator.Send(new GetParentByUrlQuery(parentUrl));
            if (parent == null)
            {
                return new NotFoundResult();
            }

            var cats = await Mediator.Send(new GetFilesCategoriesQuery(parent, loadChildren: true,
                loadLastItems: 5));

            return View("ParentFiles", new ParentFilesViewModel(ViewModelConfig, parent, cats));
        }

        public async Task<IActionResult> Download(string parentUrl, string url)
        {
            var parent = await Mediator.Send(new GetParentByUrlQuery(parentUrl));
            if (parent == null)
            {
                return new NotFoundResult();
            }
            var parsed = ParseCatchAll(url, out string catUrl, out string fileUrl);
            if (!parsed)
            {
                return new NotFoundResult();
            }

            var file = await GetFile(parent, catUrl, fileUrl);
            if (file != null)
            {
                await Mediator.Publish(new FileDownloadedCommand(file));

                var breadcrumbs = new List<BreadCrumbsItem>();
                var cat = file.Cat.ParentCat;
                while (cat != null)
                {
                    breadcrumbs.Add(new BreadCrumbsItem(Url.Files().CatPublicUrl(cat), cat.Title));
                    cat = cat.ParentCat;
                }
                breadcrumbs.Add(new BreadCrumbsItem(Url.Files().CatPublicUrl(file.Cat), file.Cat.Title));
                breadcrumbs.Add(new BreadCrumbsItem(Url.Files().ParentFilesUrl(parent), "Файлы"));
                breadcrumbs.Add(new BreadCrumbsItem(Url.Base().ParentUrl(parent), parent.DisplayTitle));
                var viewModel = new FileViewModel(ViewModelConfig, file);
                breadcrumbs.Reverse();
                viewModel.BreadCrumbs.AddRange(breadcrumbs);
                return View("FileDownload", viewModel);
            }

            return StatusCode(404);
        }

        public async Task<IActionResult> Show(string parentUrl, string url)
        {
            //so... let's try to find file
            var parent = await Mediator.Send(new GetParentByUrlQuery(parentUrl));
            if (parent == null)
            {
                return new NotFoundResult();
            }
            var parsed = ParseCatchAll(url, out string catUrl, out string fileUrl);
            if (parsed)
            {
                var file = await GetFile(parent, catUrl, fileUrl);
                if (file != null)
                {
                    var breadcrumbs = new List<BreadCrumbsItem>();
                    var cat = file.Cat.ParentCat;
                    while (cat != null)
                    {
                        breadcrumbs.Add(new BreadCrumbsItem(Url.Files().CatPublicUrl(cat), cat.Title));
                        cat = cat.ParentCat;
                    }
                    breadcrumbs.Add(new BreadCrumbsItem(Url.Files().CatPublicUrl(file.Cat), file.Cat.Title));
                    breadcrumbs.Add(new BreadCrumbsItem(Url.Files().ParentFilesUrl(parent),
                        "Файлы"));
                    breadcrumbs.Add(new BreadCrumbsItem(Url.Base().ParentUrl(parent), parent.DisplayTitle));
                    var viewModel = new FileViewModel(ViewModelConfig, file);
                    breadcrumbs.Reverse();
                    viewModel.BreadCrumbs.AddRange(breadcrumbs);
                    return View("File", viewModel);
                }
            }

            //not file... search for cat
            parsed = ParseCatchAll(url, out catUrl, out int page);
            if (!parsed)
            {
                return new NotFoundResult();
            }
            var category = await GetCat(parent, catUrl, true, 5);
            if (category != null)
            {
                var breadcrumbs = new List<BreadCrumbsItem>();
                var parentCat = category.ParentCat;
                while (parentCat != null)
                {
                    breadcrumbs.Add(
                        new BreadCrumbsItem(Url.Files().CatPublicUrl(parentCat), parentCat.Title));
                    parentCat = parentCat.ParentCat;
                }
                breadcrumbs.Add(new BreadCrumbsItem(Url.Files().ParentFilesUrl(parent), "Файлы"));
                breadcrumbs.Add(new BreadCrumbsItem(Url.Base().ParentUrl(parent), parent.DisplayTitle));

                var catFiles = await Mediator.Send(new GetCategoryFilesQuery(category, page));
                category.Items = catFiles.files;
                var viewModel = new FileCatViewModel(ViewModelConfig, category, page, catFiles.count);
                breadcrumbs.Reverse();
                viewModel.BreadCrumbs.AddRange(breadcrumbs);
                return View("FileCat", viewModel);
            }
            return StatusCode(404);
        }

        private async Task<FileCat> GetCat(IParentModel parent, string catUrl, bool loadChildren = false,
            int? loadLastItems = null)
        {
            var url = catUrl.Split('/').Last();

            return await Mediator.Send(new GetFilesCategoryQuery(parent: parent, url: url,
                loadChildren: loadChildren, loadLastItems: loadLastItems));
        }

        private async Task<File> GetFile(IParentModel parent, string catUrl, string articleUrl)
        {
            if (!string.IsNullOrEmpty(catUrl) && !string.IsNullOrEmpty(articleUrl))
            {
                if (catUrl.IndexOf('/') > -1)
                {
                    catUrl = catUrl.Split('/').Last();
                }

                return await Mediator.Send(new GetFileByUrlQuery(parent, catUrl, articleUrl));
            }
            return null;
        }
    }
}