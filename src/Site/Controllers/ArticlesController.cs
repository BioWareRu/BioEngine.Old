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
using BioEngine.Site.ViewModels.Articles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BioEngine.Site.Controllers
{
    public class ArticlesController : BaseController
    {
        public ArticlesController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager,
            IOptions<AppSettings> appSettingsOptions)
            : base(context, parentEntityProvider, urlManager, appSettingsOptions)
        {
        }

        [HttpGet("/{parentUrl}/articles/{*url}")]
        [HttpGet("/articles/{parentUrl}/{*url}")]
        public async Task<IActionResult> Show(string parentUrl, string url)
        {
            //so... let's try to find article
            var parent = await ParentEntityProvider.GetParenyByUrl(parentUrl);
            if (parent == null)
            {
                return new NotFoundResult();
            }
            ParseCatchAll(url, out string catUrl, out string articleUrl);

            var article = await GetArticle(parent, catUrl, articleUrl);
            if (article != null)
            {
                /*var fullUrl = await UrlManager.Articles.PublicUrl(article, true);
                if (fullUrl != HttpContext.Request.AbsoluteUrl())
                {
                    return new RedirectResult(fullUrl, true);
                }*/
                var breadcrumbs = new List<BreadCrumbsItem>();
                var cat = article.Cat.ParentCat;
                while (cat != null)
                {
                    breadcrumbs.Add(new BreadCrumbsItem(await UrlManager.Articles.CatPublicUrl(cat), cat.Title));
                    cat = cat.ParentCat;
                }
                breadcrumbs.Add(new BreadCrumbsItem(await UrlManager.Articles.CatPublicUrl(article.Cat),
                    article.Cat.Title));
                breadcrumbs.Add(new BreadCrumbsItem(await UrlManager.Articles.ParentArticlesUrl((dynamic)parent), "Статьи"));
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.ParentUrl(parent), parent.DisplayTitle));
                var viewModel = new ArticleViewModel(ViewModelConfig, article);
                breadcrumbs.Reverse();
                viewModel.BreadCrumbs.AddRange(breadcrumbs);
                return View("Article", viewModel);
            }

            //not article... search for cat
            var parsed = ParseCatchAll(url, out catUrl, out int _);
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
                    breadcrumbs.Add(new BreadCrumbsItem(await UrlManager.Articles.CatPublicUrl(parentCat),
                        parentCat.Title));
                    parentCat = parentCat.ParentCat;
                }
                breadcrumbs.Add(new BreadCrumbsItem(await UrlManager.Articles.ParentArticlesUrl((dynamic)parent), "Статьи"));
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.ParentUrl(parent), parent.DisplayTitle));

                await Context.Entry(category).Collection(x => x.Children).LoadAsync();
                var children = new List<CatsTree<ArticleCat, Article>>();
                foreach (var child in category.Children)
                {
                    children.Add(new CatsTree<ArticleCat, Article>(child, await GetLastArticles(child)));
                }

                var viewModel = new ArticleCatViewModel(ViewModelConfig, category, children,
                    await GetLastArticles(category));
                breadcrumbs.Reverse();
                viewModel.BreadCrumbs.AddRange(breadcrumbs);
                return View("ArticleCat", viewModel);
            }
            return new NotFoundResult();
        }

        [HttpGet("/{parentUrl}/articles.html")]
        public async Task<IActionResult> ParentArticles(string parentUrl)
        {
            var parent = await ParentEntityProvider.GetParenyByUrl(parentUrl);
            if (parent == null)
            {
                return new NotFoundResult();
            }

            var cats = await LoadCatsTree(parent, Context.ArticleCats,
                async (cat) => await GetLastArticles(cat));

            return View("ParentArticles", new ParentArticlesViewModel(ViewModelConfig, parent, cats));
        }

        public async Task<List<Article>> GetLastArticles(ICat<ArticleCat> cat, int count = 5)
        {
            return await Context.Articles.Where(x => x.CatId == cat.Id && x.Pub == 1)
                .OrderByDescending(x => x.Id)
                .Take(count)
                .ToListAsync();
        }


        private async Task<ArticleCat> GetCat(IParentModel parent, string catUrl)
        {
            var url = catUrl.Split('/').Last();

            var catQuery = Context.ArticleCats.Where(x => x.Url == url);
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
            return await catQuery.FirstOrDefaultAsync();
        }

        private async Task<Article> GetArticle(IParentModel parent, string catUrl, string articleUrl)
        {
            if (!string.IsNullOrEmpty(catUrl) && !string.IsNullOrEmpty(articleUrl))
            {
                if (catUrl.IndexOf('/') > -1)
                {
                    catUrl = catUrl.Split('/').Last();
                }

                var query = Context.Articles.Include(x => x.Cat).Include(x => x.Author).AsQueryable();
                query = query.Where(x => x.Pub == 1 && x.Url == articleUrl);
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

                var articles = await query.ToListAsync();
                if (articles.Any())
                {
                    Article article = null;
                    if (articles.Count > 1)
                    {
                        foreach (var candidate in articles)
                        {
                            if (candidate.Cat.Url != catUrl) continue;
                            article = candidate;
                            break;
                        }
                    }
                    else
                    {
                        article = articles[0];
                    }
                    if (article != null)
                    {
                        return article;
                    }
                }
            }
            return null;
        }
    }
}