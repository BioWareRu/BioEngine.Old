using System;
using System.Linq;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Site.Base;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using BioEngine.Site.ViewModels;
using BioEngine.Site.ViewModels.News;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Site.Controllers
{
    public class NewsController : BaseController
    {
        public NewsController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager)
            : base(context, parentEntityProvider, urlManager)
        {
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return NewsList();
        }

        [HttpGet("/page/{page}.html")]
        public IActionResult Index(int page)
        {
            return NewsList(page);
        }

        private IActionResult NewsList(int page = 1)
        {
            var news =
                Context.News.Where(x => x.Pub == 1).OrderByDescending(x => x.Date)
                    .Include(x => x.Author)
                    .Include(x => x.Game)
                    .Include(x => x.Developer)
                    .Include(x => x.Topic)
                    .Skip((page - 1)*20)
                    .Take(20)
                    .ToList();
            var totalNews = Context.News.Count();

            return View(new NewsListViewModel(Settings, news, totalNews, page) {Title = "Новости"});
        }

        [HttpGet("/{parentUrl}/news.html")]
        public IActionResult NewsList(string parentUrl)
        {
            var parent = ParentEntityProvider.GetParenyByUrl(parentUrl);
            if (parent != null)
            {
                return ParentNewsList((dynamic) parent);
            }
            return StatusCode(404);
        }

        [HttpGet("/{parentUrl}/news/page/{page}.html")]
        public IActionResult NewsList(string parentUrl, int page)
        {
            var parent = ParentEntityProvider.GetParenyByUrl(parentUrl);
            if (parent != null)
            {
                return ParentNewsList((dynamic) parent, page);
            }
            return StatusCode(404);
        }

        private IActionResult ParentNewsList(Game game, int page = 1)
        {
            var query = Context.News.Where(x => x.Pub == 1 && x.GameId == game.Id).AsQueryable();
            var totalNews = query.Count();
            var news = query
                .OrderByDescending(x => x.Date)
                .Include(x => x.Author)
                .Include(x => x.Game)
                .Skip((page - 1)*20)
                .Take(20)
                .ToList();

            return View("ParentNews",
                new ParentNewsListViewModel(Settings, game, news, totalNews, page));
        }

        private IActionResult ParentNewsList(Developer developer, int page = 1)
        {
            var query = Context.News.Where(x => x.Pub == 1 && x.DeveloperId == developer.Id).AsQueryable();
            var totalNews = query.Count();
            var news = query
                .OrderByDescending(x => x.Date)
                .Include(x => x.Author)
                .Include(x => x.Developer)
                .Skip((page - 1)*20)
                .Take(20)
                .ToList();

            return View("ParentNews",
                new ParentNewsListViewModel(Settings, developer, news, totalNews, page));
        }

        private IActionResult ParentNewsList(Topic topic, int page = 1)
        {
            var query = Context.News.Where(x => x.Pub == 1 && x.TopicId == topic.Id).AsQueryable();
            var totalNews = query.Count();
            var news = query
                .OrderByDescending(x => x.Date)
                .Include(x => x.Author)
                .Include(x => x.Topic)
                .Skip((page - 1)*20)
                .Take(20)
                .ToList();

            return View("ParentNews",
                new ParentNewsListViewModel(Settings, topic, news, totalNews, page));
        }

        [Route("/{year}/{month}/{day}/{url}.html")]
        public IActionResult Show(int year, int month, int day, string url)
        {
            var dateStart =
                new DateTimeOffset(new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc)).ToUnixTimeSeconds();
            var dateEnd =
                new DateTimeOffset(new DateTime(year, month, day, 23, 59, 59, DateTimeKind.Utc)).ToUnixTimeSeconds();

            var news =
                Context.News.Include(x => x.Author)
                    .Include(x => x.Game)
                    .Include(x => x.Developer)
                    .Include(x => x.Topic).FirstOrDefault(
                        n => (n.Pub == 1) && (n.Date >= dateStart) && (n.Date <= dateEnd) && (n.Url == url));

            if (news == null) return StatusCode(404);

            var viewModel = new OneNewsViewModel(news, Settings);

            viewModel.BreadCrumbs.Add(new BreadCrumbsItem(UrlManager.News.IndexUrl(), "Новости"));
            viewModel.BreadCrumbs.Add(new BreadCrumbsItem(UrlManager.News.ParentNewsUrl((dynamic) news.Parent),
                news.Parent.DisplayTitle));
            return View(viewModel);
        }
    }
}