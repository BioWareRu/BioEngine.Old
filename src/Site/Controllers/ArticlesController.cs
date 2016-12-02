using System;
using System.Collections.Generic;
using System.Linq;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Site.Base;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using BioEngine.Site.ViewModels;
using BioEngine.Site.ViewModels.Articles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Site.Controllers
{
    public class ArticlesController : BaseController
    {
        public ArticlesController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager)
            : base(context, parentEntityProvider, urlManager)
        {
        }

        [HttpGet("/{parentUrl}/articles/{*url}")]
        public IActionResult Show(string parentUrl, string url)
        {
            //so... let's try to find article
            string catUrl;
            string articleUrl;
            var parent = ParentEntityProvider.GetParenyByUrl(parentUrl);
            ParseCatchAll(url, out catUrl, out articleUrl);

            var article = GetArticle(parent, catUrl, articleUrl);
            if (article != null)
            {
                var breadcrumbs = new List<BreadCrumbsItem>();
                var cat = article.Cat.ParentCat;
                while (cat != null)
                {
                    breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Articles.CatPublicUrl(cat), cat.Title));
                    cat = cat.ParentCat;
                }
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Articles.CatPublicUrl(article.Cat), article.Cat.Title));
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Articles.ParentArticlesUrl((dynamic) article.Parent),
                    "Статьи"));
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.ParentUrl(article.Parent), article.Parent.DisplayTitle));
                var viewModel = new ArticleViewModel(Settings, article);
                breadcrumbs.Reverse();
                viewModel.BreadCrumbs.AddRange(breadcrumbs);
                return View("Article", viewModel);
            }

            //not article... search for cat
            int page;
            ParseCatchAll(url, out catUrl, out page);
            var category = GetCat(parent, catUrl);
            if (category != null)
            {
                var breadcrumbs = new List<BreadCrumbsItem>();
                var parentCat = category.ParentCat;
                while (parentCat != null)
                {
                    breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Articles.CatPublicUrl(parentCat), parentCat.Title));
                    parentCat = parentCat.ParentCat;
                }
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.Articles.ParentArticlesUrl((dynamic) category.Parent),
                    "Статьи"));
                breadcrumbs.Add(new BreadCrumbsItem(UrlManager.ParentUrl(category.Parent), category.Parent.DisplayTitle));

                var lastArticles =
                    Context.Articles.Where(x => x.CatId == category.Id && x.Pub == 1)
                        .OrderByDescending(x => x.Id)
                        .Take(5)
                        .ToList();

                Context.Entry(category).Collection(x => x.Children).Load();
                var children = new List<ChildCat>();
                foreach (var child in category.Children)
                {
                    var childLastArticles =
                        Context.Articles.Where(x => x.CatId == child.Id && x.Pub == 1)
                            .OrderByDescending(x => x.Id)
                            .Take(5)
                            .ToList();
                    children.Add(new ChildCat(child, childLastArticles));
                }

                var viewModel = new ArticleCatViewModel(Settings, category, children, lastArticles, UrlManager);
                breadcrumbs.Reverse();
                viewModel.BreadCrumbs.AddRange(breadcrumbs);
                return View("ArticleCat", viewModel);
            }
            throw new NotImplementedException();
        }

        private ArticleCat GetCat(ParentModel parent, string catUrl)
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
            var cat = catQuery.FirstOrDefault();
            if (cat != null)
            {
                cat.Parent = parent;
            }
            return cat;
        }

        private Article GetArticle(ParentModel parent, string catUrl, string articleUrl)
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

                var articles = query.ToList();
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
                        article.Parent = parent;
                        return article;
                    }
                }
            }
            return null;
        }

        public IActionResult ArticlesList(string parentUrl)
        {
            throw new NotImplementedException();
        }
    }
}