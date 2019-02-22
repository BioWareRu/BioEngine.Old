using BioEngine.Common.Base;
using BioEngine.Site.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleMvcSitemap;
using SimpleMvcSitemap.News;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Interfaces;
using BioEngine.Data.Articles.Queries;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Files.Queries;
using BioEngine.Data.Gallery.Queries;
using BioEngine.Data.News.Queries;
using BioEngine.Routing;
using MediatR;

namespace BioEngine.Site.Controllers
{
    public class SitemapController : BaseController
    {
        public SitemapController(IMediator mediator, IOptions<AppSettings> appSettingsOptions,
            IContentHelperInterface contentHelper)
            : base(mediator, appSettingsOptions, contentHelper)
        {
        }

        [HttpGet("/sitemap.xml")]
        public ActionResult Index()
        {
            List<SitemapIndexNode> sitemapIndexNodes = new List<SitemapIndexNode>
            {
                new SitemapIndexNode(Url.Action("Main", "Sitemap")),
                new SitemapIndexNode(Url.Action("News", "Sitemap")),
                new SitemapIndexNode(Url.Action("Games", "Sitemap")),
                new SitemapIndexNode(Url.Action("Articles", "Sitemap")),
                new SitemapIndexNode(Url.Action("Files", "Sitemap")),
                new SitemapIndexNode(Url.Action("Gallery", "Sitemap")),
            };

            return new SitemapProvider().CreateSitemapIndex(new SitemapIndexModel(sitemapIndexNodes));
        }

        [HttpGet("/sitemap.main.xml")]
        public ActionResult Main()
        {
            List<SitemapNode> nodes = new List<SitemapNode>
            {
                new SitemapNode(Url.News().IndexUrl().ToString())
                {
                    ChangeFrequency = ChangeFrequency.Daily,
                    LastModificationDate = DateTime.UtcNow,
                    Priority = 1
                }
            };
            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }

        [HttpGet("/sitemap.news.xml")]
        [ResponseCache(Duration = 900)]
        public async Task<ActionResult> News()
        {
            List<SitemapNode> nodes = new List<SitemapNode>();
            var allNews = (await Mediator.Send(new GetNewsQuery())).models;
            foreach (var news in allNews)
            {
                var date = DateTimeOffset.FromUnixTimeSeconds(news.Date);
                nodes.Add(new SitemapNode(Url.News().PublicUrl(news, true).ToString())
                {
                    News = new SitemapNews(new NewsPublication(news.Title, "ru"),
                        date.UtcDateTime,
                        news.Title),
                    ChangeFrequency = ChangeFrequency.Weekly,
                    Priority = 0.9M,
                    LastModificationDate = date.UtcDateTime
                });
            }

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }

        [HttpGet("/sitemap.games.xml")]
        [ResponseCache(Duration = 1800)]
        public async Task<ActionResult> Games()
        {
            List<SitemapNode> nodes = new List<SitemapNode>();
            foreach (var game in (await Mediator.Send(new GetGamesQuery())).models)
            {
                nodes.Add(new SitemapNode(Url.Base().PublicUrl(game, true).ToString())
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTime.UtcNow,
                    Priority = 0.9M
                });
            }

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }

        [HttpGet("/sitemap.articles.xml")]
        [ResponseCache(Duration = 900)]
        public async Task<ActionResult> Articles()
        {
            List<SitemapNode> nodes = new List<SitemapNode>();
            foreach (var articleCat in (await Mediator.Send(new GetArticlesCategoriesQuery())).models)
            {
                nodes.Add(new SitemapNode(Url.Articles().CatPublicUrl(articleCat).ToString())
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    Priority = 0.9M,
                    LastModificationDate = DateTime.UtcNow
                });
            }

            foreach (var article in (await Mediator.Send(new GetArticlesQuery())).models)
            {
                nodes.Add(new SitemapNode(Url.Articles().PublicUrl(article).ToString())
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTimeOffset.FromUnixTimeSeconds(article.Date).UtcDateTime,
                    Priority = 0.9M
                });
            }

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }

        [HttpGet("/sitemap.files.xml")]
        [ResponseCache(Duration = 900)]
        public async Task<ActionResult> Files()
        {
            List<SitemapNode> nodes = new List<SitemapNode>();
            foreach (var fileCat in (await Mediator.Send(new GetFilesCategoriesQuery())).models)
            {
                nodes.Add(new SitemapNode(Url.Files().CatPublicUrl(fileCat).ToString())
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    Priority = 0.9M,
                    LastModificationDate = DateTime.UtcNow
                });
            }

            foreach (var file in (await Mediator.Send(new GetFilesQuery())).models)
            {
                nodes.Add(new SitemapNode(Url.Files().PublicUrl(file).ToString())
                {
                    ChangeFrequency = ChangeFrequency.Monthly,
                    LastModificationDate = DateTimeOffset.FromUnixTimeSeconds(file.Date).UtcDateTime,
                    Priority = 0.8M
                });
            }

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }

        [HttpGet("/sitemap.gallery.xml")]
        [ResponseCache(Duration = 900)]
        public async Task<ActionResult> Gallery()
        {
            List<SitemapNode> nodes = new List<SitemapNode>();
            foreach (var galleryCat in (await Mediator.Send(new GetGalleryCategoriesQuery())).models)
            {
                nodes.Add(new SitemapNode(Url.Gallery().CatPublicUrl(galleryCat).ToString())
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    Priority = 0.9M,
                    LastModificationDate = DateTime.UtcNow
                });
            }

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}