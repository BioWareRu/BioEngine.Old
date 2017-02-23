using System;
using System.Collections.Generic;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Site.Base;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Site.ViewModels;
using BioEngine.Site.ViewModels.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Controllers
{
    public class FilesController : BaseController
    {
        public FilesController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager,
            IOptions<AppSettings> appSettingsOptions)
            : base(context, parentEntityProvider, urlManager, appSettingsOptions)
        {
        }

        [HttpGet("/{parentUrl}/files.html")]
        public async Task<IActionResult> ParentFiles(string parentUrl)
        {
            var parent = await ParentEntityProvider.GetParenyByUrl(parentUrl);
            if (parent == null) return StatusCode(404);

            var cats = await LoadCatsTree(parent, Context.FileCats, async cat => await GetLastFiles(cat));

            return View("ParentFiles", new ParentFilesViewModel(ViewModelConfig, parent, cats));
        }

        private async Task<List<File>> GetLastFiles(ICat<FileCat> cat, int count = 5)
        {
            return await Context.Files.Where(x => x.CatId == cat.Id)
                .OrderByDescending(x => x.Id)
                .Take(count)
                .ToListAsync();
        }

        [HttpGet("/{parentUrl}/download/{*url}")]
        public async Task<IActionResult> Download(string parentUrl, string url)
        {
            string catUrl;
            string fileUrl;
            var parent = await ParentEntityProvider.GetParenyByUrl(parentUrl);
            ParseCatchAll(url, out catUrl, out fileUrl);

            var file = await GetFile(parent, catUrl, fileUrl);
            if (file != null)
            {
                file.Count++;
                Context.Update(file);
                await Context.SaveChangesAsync();

                var breadcrumbs = new List<BreadCrumbsItem>();
                var cat = file.Cat.ParentCat;
                while (cat != null)
                {
                    breadcrumbs.Add(new BreadCrumbsItem(await UrlManager.Files.CatPublicUrl(cat), cat.Title));
                    cat = cat.ParentCat;
                }
                breadcrumbs.Add(new BreadCrumbsItem(await UrlManager.Files.CatPublicUrl(file.Cat), file.Cat.Title));
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Files.ParentFilesUrl((dynamic) file.Parent),
                    "Файлы"));
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.ParentUrl(file.Parent), file.Parent.DisplayTitle));
                var viewModel = new FileViewModel(ViewModelConfig, file);
                breadcrumbs.Reverse();
                viewModel.BreadCrumbs.AddRange(breadcrumbs);
                return View("FileDownload", viewModel);
            }

            return StatusCode(404);
        }

        [HttpGet("/{parentUrl}/files/{*url}")]
        public async Task<IActionResult> Show(string parentUrl, string url)
        {
            //so... let's try to find file
            string catUrl;
            string fileUrl;
            var parent = await ParentEntityProvider.GetParenyByUrl(parentUrl);
            ParseCatchAll(url, out catUrl, out fileUrl);

            var file = await GetFile(parent, catUrl, fileUrl);
            if (file != null)
            {
                var breadcrumbs = new List<BreadCrumbsItem>();
                var cat = file.Cat.ParentCat;
                while (cat != null)
                {
                    breadcrumbs.Add(new BreadCrumbsItem(await UrlManager.Files.CatPublicUrl(cat), cat.Title));
                    cat = cat.ParentCat;
                }
                breadcrumbs.Add(new BreadCrumbsItem(await UrlManager.Files.CatPublicUrl(file.Cat), file.Cat.Title));
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Files.ParentFilesUrl((dynamic) file.Parent),
                    "Файлы"));
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.ParentUrl(file.Parent), file.Parent.DisplayTitle));
                var viewModel = new FileViewModel(ViewModelConfig, file);
                breadcrumbs.Reverse();
                viewModel.BreadCrumbs.AddRange(breadcrumbs);
                return View("File", viewModel);
            }

            //not file... search for cat
            int page;
            ParseCatchAll(url, out catUrl, out page);
            var category = await GetCat(parent, catUrl);
            if (category != null)
            {
                var breadcrumbs = new List<BreadCrumbsItem>();
                var parentCat = category.ParentCat;
                while (parentCat != null)
                {
                    breadcrumbs.Add(new BreadCrumbsItem(await UrlManager.Files.CatPublicUrl(parentCat), parentCat.Title));
                    parentCat = parentCat.ParentCat;
                }
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Files.ParentFilesUrl((dynamic) category.Parent),
                    "Файлы"));
                breadcrumbs.Add(
                    new BreadCrumbsItem(UrlManager.ParentUrl(category.Parent), category.Parent.DisplayTitle));

                await Context.Entry(category).Collection(x => x.Children).LoadAsync();

                var children = new List<CatsTree<FileCat, File>>();
                foreach (var child in category.Children)
                {
                    children.Add(new CatsTree<FileCat, File>(child, await GetLastFiles(child)));
                }

                var viewModel = new FileCatViewModel(ViewModelConfig, category, children, await GetLastFiles(category),
                    page,
                    await Context.Files.CountAsync(x => x.CatId == category.Id));
                breadcrumbs.Reverse();
                viewModel.BreadCrumbs.AddRange(breadcrumbs);
                return View("FileCat", viewModel);
            }
            return StatusCode(404);
        }

        private async Task<FileCat> GetCat(ParentModel parent, string catUrl)
        {
            var url = catUrl.Split('/').Last();

            var catQuery = Context.FileCats.Where(x => x.Url == url);
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
            var cat = await catQuery.FirstOrDefaultAsync();
            if (cat != null)
            {
                cat.Parent = parent;
            }
            return cat;
        }

        private async Task<File> GetFile(ParentModel parent, string catUrl, string articleUrl)
        {
            if (!string.IsNullOrEmpty(catUrl) && !string.IsNullOrEmpty(articleUrl))
            {
                if (catUrl.IndexOf('/') > -1)
                {
                    catUrl = catUrl.Split('/').Last();
                }

                var query = Context.Files.Include(x => x.Cat).Include(x => x.Author).AsQueryable();
                query = query.Where(x => x.Url == articleUrl);
                switch (parent.Type)
                {
                    case ParentType.Game:
                        query = query.Where(x => x.GameId == parent.Id);
                        break;
                    case ParentType.Developer:
                        query = query.Where(x => x.DeveloperId == parent.Id);
                        break;
                    case ParentType.Topic:
                        query = query.Where(x => x.TopicId == parent.Id);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var files = await query.ToListAsync();
                if (files.Any())
                {
                    File file = null;
                    if (files.Count > 1)
                    {
                        foreach (var candidate in files)
                        {
                            if (candidate.Cat.Url != catUrl) continue;
                            file = candidate;
                            break;
                        }
                    }
                    else
                    {
                        file = files[0];
                    }
                    if (file != null)
                    {
                        file.Parent = parent;
                        return file;
                    }
                }
            }
            return null;
        }
    }
}