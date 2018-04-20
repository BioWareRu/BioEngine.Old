using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.News.Commands;
using BioEngine.Data.News.Queries;
using BioEngine.Routing;
using BioEngine.Site.Base;
using BioEngine.Site.ViewModels;
using BioEngine.Site.ViewModels.News;
using BioEngine.Web;
using cloudscribe.Syndication.Models.Rss;
using cloudscribe.Syndication.Web;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Controllers
{
    public class NewsController : BaseController
    {
        private readonly ILogger<NewsController> _logger;

        public NewsController(IMediator mediator,
            IOptions<AppSettings> appSettingsOptions, ILogger<NewsController> logger,
            IContentHelperInterface contentHelper
        ) : base(mediator, appSettingsOptions, contentHelper)
        {
            _logger = logger;
        }

        [HttpGet("/")]
        [HttpGet("/index.html")]
        public async Task<IActionResult> Index([FromServices] IMediator mediatr)
        {
            return await NewsList(mediatr);
        }

        public async Task<IActionResult> Index([FromServices] IMediator mediatr, int p)
        {
            return await NewsList(mediatr, p);
        }

        private async Task<IActionResult> NewsList(IMediator mediatr, int page = 1)
        {
            if (page < 1)
                return BadRequest();
            var canUserSeeUnpublishedNews = await HasRight(UserRights.News);

            var response =
                await mediatr.Send(new GetNewsQuery {WithUnPublishedNews = canUserSeeUnpublishedNews, Page = page});

            return View("Index", new NewsListViewModel(ViewModelConfig, response.models, response.totalCount, page));
        }

        [Route("/{year:int}.html", Order = 1)]
        public async Task<IActionResult> NewsByYear(int year)
        {
            return await NewsByDate(year, null, null);
        }

        [Route("/{year:int}/page/{p:int}.html")]
        public async Task<IActionResult> NewsByYear(int year, int p)
        {
            return await NewsByDate(year, null, null, p);
        }

        [Route("/{year:int}/{month:regex(\\d{{2}})}.html")]
        public async Task<IActionResult> NewsByYearAndMonth(int year, int month)
        {
            return await NewsByDate(year, month, null);
        }

        [Route("/{year:int}/{month:regex(\\d{{2}})}/page/{p:int}.html")]
        public async Task<IActionResult> NewsByYearAndMonth(int year, int month, int p)
        {
            return await NewsByDate(year, month, null, p);
        }

        [Route("/{year:int}/{month:regex(\\d{{2}})}/{day:regex(\\d{{2}})}.html")]
        public async Task<IActionResult> NewsByYearAndMonthAndDay(int year, int month, int day)
        {
            return await NewsByDate(year, month, day);
        }

        [Route("/{year:int}/{month:regex(\\d{{2}})}/{day:regex(\\d{{2}})}/page/{p:int}.html")]
        public async Task<IActionResult> NewsByYearAndMonthAndDay(int year, int month, int day, int p)
        {
            return await NewsByDate(year, month, day, p);
        }

        private async Task<IActionResult> NewsByDate(int year, int? month, int? day, int page = 1)
        {
            if (page < 1)
                return BadRequest();
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

            var newsResult = await Mediator.Send(new GetNewsQuery
            {
                WithUnPublishedNews = canUserSeeUnpublishedNews,
                Page = page,
                DateStart = new DateTimeOffset(dateStart).ToUnixTimeSeconds(),
                DateEnd = new DateTimeOffset(dateEnd).ToUnixTimeSeconds()
            });

            return View("ListByDate",
                new NewsListByDateViewModel(ViewModelConfig, newsResult.models, newsResult.totalCount, page, year,
                    month,
                    day));
        }

        [HttpGet("/{parentUrl}/news.html")]
        public async Task<IActionResult> ParentNews(string parentUrl)
        {
            var parent = await Mediator.Send(new GetParentByUrlQuery(parentUrl));
            return parent != null ? await ParentNewsList(parent) : StatusCode(404);
        }

        [HttpGet("/{parentUrl}/news/page/{p:int}.html")]
        public async Task<IActionResult> ParentNewsWithPage(string parentUrl, int p)
        {
            var parent = await Mediator.Send(new GetParentByUrlQuery(parentUrl));
            return parent != null ? await ParentNewsList(parent, p) : StatusCode(404);
        }

        private async Task<IActionResult> ParentNewsList(IParentModel parent, int page = 1)
        {
            if (page < 1)
                return BadRequest();

            var canUserSeeUnpublishedNews = await HasRight(UserRights.News);

            var newsResult = await Mediator.Send(new GetNewsQuery
            {
                WithUnPublishedNews = canUserSeeUnpublishedNews,
                Page = page,
                Parent = parent
            });

            return View("ParentNews",
                new ParentNewsListViewModel(ViewModelConfig, parent, newsResult.models, newsResult.totalCount, page));
        }

        [Route("/{year:int}/{month:regex(^\\d{{2}}$)}/{day:regex(^\\d{{2}}$)}/{url}.html")]
        public async Task<IActionResult> Show(int year, int month, int day, string url)
        {
            long dateStart;
            long dateEnd;
            try
            {
                dateStart = new DateTimeOffset(new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc))
                    .ToUnixTimeSeconds();
                dateEnd = new DateTimeOffset(new DateTime(year, month, day, 23, 59, 59, DateTimeKind.Utc))
                    .ToUnixTimeSeconds();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Bad date creation: {ex.Message}");
                return new BadRequestResult();
            }

            var news = await Mediator.Send(
                new GetOneNewsQuery(url)
                {
                    WithUnPublishedNews = await HasRight(UserRights.News),
                    DateStart = dateStart,
                    DateEnd = dateEnd
                });

            if (news == null) return StatusCode(404);

            var viewModel = new OneNewsViewModel(ViewModelConfig, news);
            viewModel.BreadCrumbs.Add(new BreadCrumbsItem(Url.News().IndexUrl(), "Новости"));
            viewModel.BreadCrumbs.Add(new BreadCrumbsItem(Url.News().ParentNewsUrl(news), news.Parent.DisplayTitle));
            return View(viewModel);
        }

        [Route("/{year:int}/{month:regex(^\\d{{1}}$)}/{day:regex(^\\d{{1}}$)}/{url}.html")]
        [Route("/{year:int}/{month:regex(^\\d{{2}}$)}/{day:regex(^\\d{{1}}$)}/{url}.html")]
        [Route("/{year:int}/{month:regex(^\\d{{1}}$)}/{day:regex(^\\d{{2}}$)}/{url}.html")]
        public IActionResult ShowOld(int year, int month, int day, string url)
        {
            var redirectUrl = Url.Action("show", "News",
                new
                {
                    year = year.ToString("D4"),
                    month = month.ToString("D2"),
                    day = day.ToString("D2"),
                    url
                }, Request.Scheme);
            return Redirect(redirectUrl);
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
                list.Add(new NullChannelProvider());

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
        public async Task<IActionResult> CreateOrUpdateNewsTopic(int newsId, string accessToken,
            [FromServices] IOptions<AdminAccessConfig> adminAccessConfig)
        {
            if (accessToken != adminAccessConfig.Value.AdminAccessToken)
            {
                Response.StatusCode = 403;
                return new EmptyResult();
            }
            if (newsId == 0)
                return new BadRequestResult();

            var news = await Mediator.Send(new GetNewsByIdQuery(newsId));

            if (news == null)
                return new NotFoundResult();

            await Mediator.Publish(new CreateOrUpdateNewsForumTopicCommand(news));
            if (news.ForumTopicId > 0 && news.ForumPostId > 0)
            {
                Response.StatusCode = 200;
                return new EmptyResult();
            }

            Response.StatusCode = 500;
            return new EmptyResult();
        }

        [HttpGet("/news/delete-forum-post/{newsId:int}.html")]
        public async Task<IActionResult> DeleteNewsTopic(int newsId, string accessToken,
            [FromServices] IOptions<AdminAccessConfig> adminAccessConfig)
        {
            if (accessToken != adminAccessConfig.Value.AdminAccessToken)
            {
                Response.StatusCode = 403;
                return new EmptyResult();
            }
            if (newsId == 0)
                return new BadRequestResult();
            var news = await Mediator.Send(new GetNewsByIdQuery(newsId));

            if (news == null)
                return new NotFoundResult();

            await Mediator.Publish(new DeleteNewsForumTopicCommand(news));
            if (news.ForumTopicId == 0)
            {
                Response.StatusCode = 200;
                return new EmptyResult();
            }

            Response.StatusCode = 500;
            return new EmptyResult();
        }
    }
}