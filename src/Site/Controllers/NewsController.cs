using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Site.Base;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using BioEngine.Site.ViewModels;
using BioEngine.Site.ViewModels.News;
using cloudscribe.Syndication.Models.Rss;
using cloudscribe.Syndication.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Controllers
{
    public class NewsController : BaseController
    {
        public NewsController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager,
            IOptions<AppSettings> appSettingsOptions
        ) : base(context, parentEntityProvider, urlManager, appSettingsOptions)
        {
        }

        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            return await NewsList();
        }

        [HttpGet("/page/{page}.html")]
        public async Task<IActionResult> Index(int page)
        {
            return await NewsList(page);
        }

        private async Task<IActionResult> NewsList(int page = 1)
        {
            var news =
                await Context.News.Where(x => x.Pub == 1)
                    .OrderByDescending(x => x.Date)
                    .Include(x => x.Author)
                    .Include(x => x.Game)
                    .Include(x => x.Developer)
                    .Include(x => x.Topic)
                    .Skip((page - 1) * 20)
                    .Take(20)
                    .ToListAsync();
            var totalNews = await Context.News.CountAsync();

            return View(new NewsListViewModel(ViewModelConfig, news, totalNews, page) {Title = "Новости"});
        }

        [HttpGet("/{parentUrl}/news.html")]
        public async Task<IActionResult> NewsList(string parentUrl)
        {
            var parent = await ParentEntityProvider.GetParenyByUrl(parentUrl);
            return parent != null ? ParentNewsList((dynamic) parent) : StatusCode(404);
        }

        [HttpGet("/{parentUrl}/news/page/{page}.html")]
        public async Task<IActionResult> NewsList(string parentUrl, int page)
        {
            var parent = await ParentEntityProvider.GetParenyByUrl(parentUrl);
            return parent != null ? ParentNewsList((dynamic) parent, page) : StatusCode(404);
        }

        private async Task<IActionResult> ParentNewsList(Game game, int page = 1)
        {
            var query = Context.News.Where(x => x.Pub == 1 && x.GameId == game.Id).AsQueryable();
            var totalNews = await query.CountAsync();
            var news = await query
                .OrderByDescending(x => x.Date)
                .Include(x => x.Author)
                .Include(x => x.Game)
                .Skip((page - 1) * 20)
                .Take(20)
                .ToListAsync();

            return View("ParentNews",
                new ParentNewsListViewModel(ViewModelConfig, game, news, totalNews, page));
        }

        private async Task<IActionResult> ParentNewsList(Developer developer, int page = 1)
        {
            var query = Context.News.Where(x => x.Pub == 1 && x.DeveloperId == developer.Id).AsQueryable();
            var totalNews = await query.CountAsync();
            var news = await query
                .OrderByDescending(x => x.Date)
                .Include(x => x.Author)
                .Include(x => x.Developer)
                .Skip((page - 1) * 20)
                .Take(20)
                .ToListAsync();

            return View("ParentNews",
                new ParentNewsListViewModel(ViewModelConfig, developer, news, totalNews, page));
        }

        private async Task<IActionResult> ParentNewsList(Topic topic, int page = 1)
        {
            var query = Context.News.Where(x => x.Pub == 1 && x.TopicId == topic.Id).AsQueryable();
            var totalNews = await query.CountAsync();
            var news = await query
                .OrderByDescending(x => x.Date)
                .Include(x => x.Author)
                .Include(x => x.Topic)
                .Skip((page - 1) * 20)
                .Take(20)
                .ToListAsync();

            return View("ParentNews",
                new ParentNewsListViewModel(ViewModelConfig, topic, news, totalNews, page));
        }

        [Route("/{year}/{month}/{day}/{url}.html")]
        public async Task<IActionResult> Show(int year, int month, int day, string url)
        {
            var dateStart =
                new DateTimeOffset(new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc)).ToUnixTimeSeconds();
            var dateEnd =
                new DateTimeOffset(new DateTime(year, month, day, 23, 59, 59, DateTimeKind.Utc)).ToUnixTimeSeconds();

            var newsQuery =
                Context.News.Include(x => x.Author)
                    .Include(x => x.Game)
                    .Include(x => x.Developer)
                    .Include(x => x.Topic)
                    .Where(n => (n.Date >= dateStart) && (n.Date <= dateEnd) && (n.Url == url));

            if (User.Identity.IsAuthenticated)
            {
                if (User.HasClaim(x => x.Type == "siteTeam"))
                {
                    if (!User.HasClaim(x => x.Type == UserRights.PubNews.ToString()))
                    {
                        newsQuery =
                            newsQuery.Where(
                                x =>
                                    x.Pub == 1 |
                                    x.AuthorId == int.Parse(User.Claims.Where(c => c.Type == "userId").ToString()));
                    }
                }
                else
                {
                    if (!User.HasClaim(x => x.Type == "admin"))
                    {
                        newsQuery = newsQuery.Where(x => x.Pub == 1);
                    }
                }
            }
            else
            {
                newsQuery = newsQuery.Where(x => x.Pub == 1);
            }

            var news = await newsQuery.FirstOrDefaultAsync();

            if (news == null) return StatusCode(404);

            var viewModel = new OneNewsViewModel(ViewModelConfig, news);

            viewModel.BreadCrumbs.Add(new BreadCrumbsItem(UrlManager.News.IndexUrl(), "Новости"));
            viewModel.BreadCrumbs.Add(new BreadCrumbsItem(UrlManager.News.ParentNewsUrl((dynamic) news.Parent),
                news.Parent.DisplayTitle));
            return View(viewModel);
        }

        [HttpGet("/rss.xml")]
        public async Task<IActionResult> Rss([FromServices] IEnumerable<IChannelProvider> channelProviders = null)
        {
            channelProviders = channelProviders ?? new List<IChannelProvider>();
            var list = channelProviders as List<IChannelProvider>;
            if (list?.Count == 0)
            {
                list.Add(new NullChannelProvider());
            }

            var channelResolver = new DefaultChannelProviderResolver();
            var xmlFormatter = new DefaultXmlFormatter();

            var currentChannelProvider = channelResolver.GetCurrentChannelProvider(channelProviders);

            if (currentChannelProvider == null)
            {
                Response.StatusCode = 404;
                return new EmptyResult();
            }

            var currentChannel = await currentChannelProvider.GetChannel();

            if (currentChannel == null)
            {
                Response.StatusCode = 404;
                return new EmptyResult();
            }

            var xml = xmlFormatter.BuildXml(currentChannel);

            return new XmlResult(xml);
        }
    }
}