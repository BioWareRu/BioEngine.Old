using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Ipb;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace BioEngine.Site.Controllers
{
    public class NewsController : BaseController
    {
        private readonly ILogger<NewsController> _logger;

        public NewsController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager,
            IOptions<AppSettings> appSettingsOptions, ILogger<NewsController> logger
        ) : base(context, parentEntityProvider, urlManager, appSettingsOptions)
        {
            _logger = logger;
        }

        [HttpGet("/")]
        [HttpGet("/index.html")]
        public async Task<IActionResult> Index()
        {
            return await NewsList();
        }

        [HttpGet("/page/{page:int}.html")]
        public async Task<IActionResult> Index(int page)
        {
            return await NewsList(page);
        }

        private async Task<IActionResult> NewsList(int page = 1)
        {
            if (page < 1)
            {
                return BadRequest();
            }
            var canUserSeeUnpublishedNews = await HasRight(UserRights.News);
            var query = Context.News.AsQueryable();
            if (!canUserSeeUnpublishedNews)
            {
                query = query.Where(x => x.Pub == 1);
            }
            var news =
                await query
                    .OrderByDescending(x => x.Date)
                    .Include(x => x.Author)
                    .Include(x => x.Game)
                    .Include(x => x.Developer)
                    .Include(x => x.Topic)
                    .Skip((page - 1) * 20)
                    .Take(20)
                    .ToListAsync();
            var totalNews = await Context.News.CountAsync();

            return View(new NewsListViewModel(ViewModelConfig, news, totalNews, page));
        }

        [Route("/{year:int}.html", Order = 1)]
        public async Task<IActionResult> NewsByYear(int year)
        {
            return await NewsByDate(year, null, null);
        }

        [Route("/{year:int}/page/{page:int}.html")]
        public async Task<IActionResult> NewsByYear(int year, int page)
        {
            return await NewsByDate(year, null, null, page);
        }

        [Route("/{year:int}/{month:range(1,12)}.html")]
        public async Task<IActionResult> NewsByYearAndMonth(int year, int month)
        {
            return await NewsByDate(year, month, null);
        }

        [Route("/{year:int}/{month:range(1,12)}/page/{page:int}.html")]
        public async Task<IActionResult> NewsByYearAndMonth(int year, int month, int page)
        {
            return await NewsByDate(year, month, null, page);
        }

        [Route("/{year:int}/{month:range(1,12)}/{day:range(1,31)}.html")]
        public async Task<IActionResult> NewsByYearAndMonthAndDay(int year, int month, int day)
        {
            return await NewsByDate(year, month, day);
        }

        [Route("/{year:int}/{month:range(1,12)}/{day:range(1,31)}/page/{page:int}.html")]
        public async Task<IActionResult> NewsByYearAndMonthAndDay(int year, int month, int day, int page)
        {
            return await NewsByDate(year, month, day, page);
        }

        private async Task<IActionResult> NewsByDate(int year, int? month, int? day, int page = 1)
        {
            if (page < 1)
            {
                return BadRequest();
            }
            var monthStart = month ?? 1;
            var monthEnd = month ?? 12;
            var dayStart = day ?? 1;
            var dayEnd = day ?? DateTime.DaysInMonth(year, monthEnd);
            DateTime dateStart;
            DateTime dateEnd;
            try
            {
                dateStart = new DateTime(year, monthStart, dayStart, 0, 0, 0);
                dateEnd = new DateTime(year, monthEnd, dayEnd, 23, 59, 59);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Bad date creation: {ex.Message}");
                return new BadRequestResult();
            }
            var canUserSeeUnpublishedNews = await HasRight(UserRights.News);
            var query = Context.News.Where(x => (canUserSeeUnpublishedNews || x.Pub == 1)
                                                && x.Date >= new DateTimeOffset(dateStart).ToUnixTimeSeconds()
                                                && x.Date <= new DateTimeOffset(dateEnd).ToUnixTimeSeconds()
                )
                .OrderByDescending(x => x.Date)
                .Include(x => x.Author)
                .Include(x => x.Game)
                .Include(x => x.Developer)
                .Include(x => x.Topic);
            var news =
                await query
                    .Skip((page - 1) * 20)
                    .Take(20)
                    .ToListAsync();
            var totalNews = await query.CountAsync();

            return View("ListByDate",
                new NewsListByDateViewModel(ViewModelConfig, news, totalNews, page, year, month, day));
        }

        [HttpGet("/{parentUrl}/news.html")]
        public async Task<IActionResult> NewsList(string parentUrl)
        {
            var parent = await ParentEntityProvider.GetParenyByUrl(parentUrl);
            return parent != null ? await ParentNewsList((dynamic)parent) : Task.FromResult(StatusCode(404));
        }

        [HttpGet("/{parentUrl}/news/page/{page}.html")]
        public async Task<IActionResult> NewsList(string parentUrl, int page)
        {
            var parent = await ParentEntityProvider.GetParenyByUrl(parentUrl);
            return parent != null ? await ParentNewsList((dynamic)parent, page) : Task.FromResult(StatusCode(404));
        }

        private async Task<IActionResult> ParentNewsList(Game game, int page = 1)
        {
            if (page < 1)
            {
                return BadRequest();
            }
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

        [Route("/{year:int}/{month:range(1,12)}/{day:range(1,31)}/{url}.html")]
        public async Task<IActionResult> Show(int year, int month, int day, string url)
        {
            long dateStart;
            long dateEnd;
            try
            {
                dateStart = new DateTimeOffset(new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc)).ToUnixTimeSeconds();
                dateEnd = new DateTimeOffset(new DateTime(year, month, day, 23, 59, 59, DateTimeKind.Utc)).ToUnixTimeSeconds();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Bad date creation: {ex.Message}");
                return new BadRequestResult();
            }

            var newsQuery =
                Context.News.Include(x => x.Author)
                    .Include(x => x.Game)
                    .Include(x => x.Developer)
                    .Include(x => x.Topic)
                    .Where(n => (n.Date >= dateStart) && (n.Date <= dateEnd) && (n.Url == url));

            if (!await HasRight(UserRights.News))
            {
                newsQuery = newsQuery.Where(x => x.Pub == 1);
            }

            var news = await newsQuery.FirstOrDefaultAsync();

            if (news == null) return StatusCode(404);

            var viewModel = new OneNewsViewModel(ViewModelConfig, news);
            var parent = await ParentEntityProvider.GetModelParent(news);
            viewModel.BreadCrumbs.Add(new BreadCrumbsItem(UrlManager.News.IndexUrl(), "Новости"));
            viewModel.BreadCrumbs.Add(new BreadCrumbsItem(await UrlManager.News.ParentNewsUrl((dynamic)parent),
                parent.DisplayTitle));
            return View(viewModel);
        }

        [HttpGet("/rss.xml")]
        [HttpGet("/rss")]
        [HttpGet("/news/rss.xml")]
        [HttpGet("/news/rss")]
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

        [HttpGet("/news/update-forum-post/{newsId:int}.html")]
        public async Task<IActionResult> CreateOrUpdateNewsTopic(int newsId, [FromServices] IPBApiHelper ipbApiHelper,
            [FromServices] IConfigurationRoot configuration)
        {
            if (newsId == 0)
            {
                return new BadRequestResult();
            }
            var logProperties = new List<ILogEventEnricher>
            {
                new PropertyEnricher("BE_IPB_API_KEY", configuration["BE_IPB_API_KEY"]),
                new PropertyEnricher("BE_IPB_API_URL", configuration["BE_IPB_API_URL"]),
                new PropertyEnricher("BE_IPB_NEWS_FORUM_ID", configuration["BE_IPB_NEWS_FORUM_ID"])
            };

            var news = await Context.News.FirstOrDefaultAsync(x => x.Id == newsId);

            if (news == null)
            {
                return new NotFoundResult();
            }

            using (LogContext.PushProperties(logProperties.ToArray()))
            {
                var result = await ipbApiHelper.CreateOrUpdateNewsTopic(news);
                if (result)
                {
                    Response.StatusCode = 200;
                    return new EmptyResult();
                }

                Response.StatusCode = 500;
                return new EmptyResult();
            }
        }
    }
}