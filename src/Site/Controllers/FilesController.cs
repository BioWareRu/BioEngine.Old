using System;
using System.Collections.Generic;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Site.Base;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Site.ViewModels;
using BioEngine.Site.ViewModels.Files;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Site.Controllers
{
    public class FilesController : BaseController
    {
        public FilesController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager)
            : base(context, parentEntityProvider, urlManager)
        {
        }

        [HttpGet("/{parentUrl}/files.html")]
        public IActionResult ParentFiles(string parentUrl)
        {
            var parent = ParentEntityProvider.GetParenyByUrl(parentUrl);
            if (parent == null) return StatusCode(404);

            var cats = LoadCatsTree(parent, Context.FileCats, cat => GetLastFiles(cat));

            return View("ParentFiles", new ParentFilesViewModel(Settings, parent, cats, UrlManager));
        }

        public List<File> GetLastFiles(ICat<FileCat> cat, int count = 5)
        {
            return Context.Files.Where(x => x.CatId == cat.Id)
                .OrderByDescending(x => x.Id)
                .Take(count)
                .ToList();
        }

        [HttpGet("/{parentUrl}/download/{*url}")]
        public IActionResult Download(string parentUrl, string url)
        {
            string catUrl;
            string fileUrl;
            var parent = ParentEntityProvider.GetParenyByUrl(parentUrl);
            ParseCatchAll(url, out catUrl, out fileUrl);

            var file = GetFile(parent, catUrl, fileUrl);
            if (file != null)
            {
                file.Count++;
                Context.Update(file);
                Context.SaveChangesAsync();

                var breadcrumbs = new List<BreadCrumbsItem>();
                var cat = file.Cat.ParentCat;
                while (cat != null)
                {
                    breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Files.CatPublicUrl(cat), cat.Title));
                    cat = cat.ParentCat;
                }
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Files.CatPublicUrl(file.Cat), file.Cat.Title));
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Files.ParentFilesUrl((dynamic)file.Parent),
                    "Файлы"));
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.ParentUrl(file.Parent), file.Parent.DisplayTitle));
                var viewModel = new FileViewModel(Settings, file, UrlManager);
                breadcrumbs.Reverse();
                viewModel.BreadCrumbs.AddRange(breadcrumbs);
                return View("FileDownload", viewModel);
            }

            return StatusCode(404);
        }

        [HttpGet("/{parentUrl}/files/{*url}")]
        public IActionResult Show(string parentUrl, string url)
        {
            //so... let's try to find file
            string catUrl;
            string fileUrl;
            var parent = ParentEntityProvider.GetParenyByUrl(parentUrl);
            ParseCatchAll(url, out catUrl, out fileUrl);

            var file = GetFile(parent, catUrl, fileUrl);
            if (file != null)
            {
                var breadcrumbs = new List<BreadCrumbsItem>();
                var cat = file.Cat.ParentCat;
                while (cat != null)
                {
                    breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Files.CatPublicUrl(cat), cat.Title));
                    cat = cat.ParentCat;
                }
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Files.CatPublicUrl(file.Cat), file.Cat.Title));
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Files.ParentFilesUrl((dynamic) file.Parent),
                    "Файлы"));
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.ParentUrl(file.Parent), file.Parent.DisplayTitle));
                var viewModel = new FileViewModel(Settings, file, UrlManager);
                breadcrumbs.Reverse();
                viewModel.BreadCrumbs.AddRange(breadcrumbs);
                return View("File", viewModel);
            }

            //not file... search for cat
            int page;
            ParseCatchAll(url, out catUrl, out page);
            var category = GetCat(parent, catUrl);
            if (category != null)
            {
                var breadcrumbs = new List<BreadCrumbsItem>();
                var parentCat = category.ParentCat;
                while (parentCat != null)
                {
                    breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Files.CatPublicUrl(parentCat), parentCat.Title));
                    parentCat = parentCat.ParentCat;
                }
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Files.ParentFilesUrl((dynamic) category.Parent),
                    "Файлы"));
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.ParentUrl(category.Parent), category.Parent.DisplayTitle));

                Context.Entry(category).Collection(x => x.Children).Load();
                var children =
                    category.Children.Select(child => new CatsTree<FileCat, File>(child, GetLastFiles(child)))
                        .ToList();

                var viewModel = new FileCatViewModel(Settings, category, children, GetLastFiles(category),
                    UrlManager);
                breadcrumbs.Reverse();
                viewModel.BreadCrumbs.AddRange(breadcrumbs);
                return View("FileCat", viewModel);
            }
            return StatusCode(404);
        }

        private FileCat GetCat(ParentModel parent, string catUrl)
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
            var cat = catQuery.FirstOrDefault();
            if (cat != null)
            {
                cat.Parent = parent;
            }
            return cat;
        }

        private File GetFile(ParentModel parent, string catUrl, string articleUrl)
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

                var files = query.ToList();
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