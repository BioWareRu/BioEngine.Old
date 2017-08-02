using System.Collections.Generic;
using System.Linq;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Site.Base;
using BioEngine.Site.ViewModels;
using BioEngine.Site.ViewModels.Articles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using BioEngine.Data.Articles.Requests;
using BioEngine.Data.Base.Requests;
using BioEngine.Routing;
using BioEngine.Site.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Site.Controllers
{
    public class ArticlesController : BaseController
    {
        public ArticlesController(IMediator mediator, IOptions<AppSettings> appSettingsOptions,
            IContentHelperInterface contentHelper)
            : base(mediator, appSettingsOptions, contentHelper)
        {
        }

        [HttpGet("/{parentUrl}/articles/{*url}")]
        [HttpGet("/articles/{parentUrl}/{*url}")]
        public async Task<IActionResult> Show(string parentUrl, string url,
            [FromServices] ILogger<ArticlesController> logger)
        {
            //so... let's try to find article
            var parent = await Mediator.Send(new GetParentByUrlRequest(parentUrl));
            if (parent == null)
            {
                return new NotFoundResult();
            }
            var parsed = ParseCatchAll(url, out string catUrl, out string articleUrl);
            if (parsed)
            {
                var article = await GetArticle(parent, catUrl, articleUrl);
                if (article != null)
                {
                    var fullUrl = Url.Articles().PublicUrl(article, true);
                    if (fullUrl != HttpContext.Request.AbsoluteUrl())
                    {
                        //return new RedirectResult(fullUrl, true);
                        logger.LogWarning(
                            $"Article urls are not equal. Current url: {HttpContext.Request.AbsoluteUrl()}. Canonical: {fullUrl}");
                    }
                    var breadcrumbs = new List<BreadCrumbsItem>();
                    var cat = article.Cat.ParentCat;
                    while (cat != null)
                    {
                        breadcrumbs.Add(new BreadCrumbsItem(Url.Articles().CatPublicUrl(cat), cat.Title));
                        cat = cat.ParentCat;
                    }
                    breadcrumbs.Add(new BreadCrumbsItem(Url.Articles().CatPublicUrl(article.Cat),
                        article.Cat.Title));
                    breadcrumbs.Add(new BreadCrumbsItem(Url.Articles().ParentArticlesUrl(parent),
                        "Статьи"));
                    breadcrumbs.Add(new BreadCrumbsItem(Url.Base().ParentUrl(parent), parent.DisplayTitle));
                    var viewModel = new ArticleViewModel(ViewModelConfig, article);
                    breadcrumbs.Reverse();
                    viewModel.BreadCrumbs.AddRange(breadcrumbs);
                    return View("Article", viewModel);
                }
            }
            //not article... search for cat
            parsed = ParseCatchAll(url, out catUrl, out int _);
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
                    breadcrumbs.Add(new BreadCrumbsItem(Url.Articles().CatPublicUrl(parentCat),
                        parentCat.Title));
                    parentCat = parentCat.ParentCat;
                }
                breadcrumbs.Add(new BreadCrumbsItem(Url.Articles().ParentArticlesUrl(parent), "Статьи"));
                breadcrumbs.Add(new BreadCrumbsItem(Url.Base().ParentUrl(parent), parent.DisplayTitle));

                var catArticles = await Mediator.Send(new GetCategoryArticlesRequest(category, 0));
                category.Items = catArticles.articles;
                var viewModel = new ArticleCatViewModel(ViewModelConfig, category);
                breadcrumbs.Reverse();
                viewModel.BreadCrumbs.AddRange(breadcrumbs);
                return View("ArticleCat", viewModel);
            }
            return new NotFoundResult();
        }

        [HttpGet("/{parentUrl}/articles.html")]
        public async Task<IActionResult> ParentArticles(string parentUrl)
        {
            var parent = await Mediator.Send(new GetParentByUrlRequest(parentUrl));
            if (parent == null)
            {
                return new NotFoundResult();
            }

            var cats = await Mediator.Send(new GetArticlesCategoriesRequest(parent: parent, loadChildren: true,
                loadLastItems: 5));

            return View("ParentArticles", new ParentArticlesViewModel(ViewModelConfig, parent, cats));
        }

        public async Task<IEnumerable<Article>> GetLastArticles(ArticleCat cat, int count = 5)
        {
            return (await Mediator.Send(new GetCategoryArticlesRequest(cat, count))).articles;
        }


        private async Task<ArticleCat> GetCat(IParentModel parent, string catUrl, bool loadChildren = false,
            int? loadLastItems = null)
        {
            var url = catUrl.Split('/').Last();

            return await Mediator.Send(new GetArticlesCategoryRequest(parent: parent, url: url,
                loadChildren: loadChildren, loadLastItems: loadLastItems));
        }

        private async Task<Article> GetArticle(IParentModel parent, string catUrl, string articleUrl)
        {
            if (!string.IsNullOrEmpty(catUrl) && !string.IsNullOrEmpty(articleUrl))
            {
                if (catUrl.IndexOf('/') > -1)
                {
                    catUrl = catUrl.Split('/').Last();
                }

                return await Mediator.Send(new GetArticleByUrlRequest(parent, catUrl, articleUrl));
            }
            return null;
        }
    }
}