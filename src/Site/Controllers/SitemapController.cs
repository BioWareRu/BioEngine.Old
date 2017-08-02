using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Site.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimpleMvcSitemap;
using SimpleMvcSitemap.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Interfaces;
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
                new SitemapNode(Url.Action("Index", "News"))
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
            var allNews = await Context.News.Where(x => x.Pub == 1)
                .OrderByDescending(x => x.Date)
                .Include(x => x.Author)
                .Include(x => x.Game)
                .Include(x => x.Developer)
                .Include(x => x.Topic).ToListAsync();
            foreach (var news in allNews)
            {
                nodes.Add(new SitemapNode(UrlManager.News.PublicUrl(news, true))
                {
                    News = new SitemapNews(newsPublication: new NewsPublication(name: news.Title, language: "ru"),
                        publicationDate: DateTimeOffset.FromUnixTimeSeconds(news.Date).Date,
                        title: news.Title),
                    ChangeFrequency = ChangeFrequency.Weekly,
                    Priority = 0.9M
                });
            }
            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }

        [HttpGet("/sitemap.games.xml")]
        [ResponseCache(Duration = 1800)]
        public async Task<ActionResult> Games()
        {
            List<SitemapNode> nodes = new List<SitemapNode>();
            foreach (var game in await Context.Games.ToListAsync())
            {
                nodes.Add(new SitemapNode(Url.Base().PublicUrl(game, true))
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
            foreach (var articleCat in await Context.ArticleCats.ToListAsync())
            {
                nodes.Add(new SitemapNode(Url.Articles().CatPublicUrl(articleCat))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    Priority = 0.9M
                });
            }
            foreach (var article in await Context.Articles.Where(x => x.Pub == 1).ToListAsync())
            {
                nodes.Add(new SitemapNode(Url.Articles().PublicUrl(article))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTimeOffset.FromUnixTimeSeconds(article.Date).Date,
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
            foreach (var fileCat in await Context.FileCats.ToListAsync())
            {
                nodes.Add(new SitemapNode(Url.Files().CatPublicUrl(fileCat))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    Priority = 0.9M
                });
            }
            foreach (var file in await Context.Files.ToListAsync())
            {
                nodes.Add(new SitemapNode(Url.Files().PublicUrl(file))
                {
                    ChangeFrequency = ChangeFrequency.Monthly,
                    LastModificationDate = DateTimeOffset.FromUnixTimeSeconds(file.Date).Date,
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
            foreach (var galleryCat in await Context.GalleryCats.ToListAsync())
            {
                nodes.Add(new SitemapNode(Url.Gallery().CatPublicUrl(galleryCat))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    Priority = 0.9M
                });
            }
            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}