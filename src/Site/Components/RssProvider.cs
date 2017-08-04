using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Data.News.Queries;
using BioEngine.Routing;
using cloudscribe.Syndication.Models.Rss;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Components
{
    [UsedImplicitly]
    public class RssProvider : IChannelProvider
    {
        private readonly IMediator _mediator;
        private readonly IUrlHelper _urlHelper;

        public RssProvider(IOptions<AppSettings> options, IMediator mediator, IUrlHelper urlHelper)
        {
            _mediator = mediator;
            _urlHelper = urlHelper;
            _appSettings = options.Value;
        }

        private readonly AppSettings _appSettings;

        public Task<RssChannel> GetChannel(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.Run(async () =>
            {
                var channel = new RssChannel
                {
                    Title = _appSettings.Title,
                    Description = "Последние новости",
                    Link = new Uri(_appSettings.SiteDomain),
                    Language = CultureInfo.CurrentCulture,
                    TimeToLive = 60,
                    LastBuildDate = DateTime.Now,
                    Image =
                        new RssImage(new Uri(_appSettings.SiteDomain), _appSettings.Title,
                            new Uri(_appSettings.SocialLogo))
                };

                var newsResult = await _mediator.Send(new GetNewsQuery(page: 1), cancellationToken);
                var mostRecentPubDate = DateTime.MinValue;
                var items = new List<RssItem>();
                foreach (var news in newsResult.news)
                {
                    var newsDate = DateTimeOffset.FromUnixTimeSeconds(news.LastChangeDate).Date;
                    if (newsDate > mostRecentPubDate) mostRecentPubDate = newsDate;
                    var newsUrl = _urlHelper.News().PublicUrl(news, true);
                    var item = new RssItem
                    {
                        Title = news.Title,
                        Description = news.ShortText,
                        Link = newsUrl,
                        PublicationDate = newsDate,
                        Author = news.Author.Name,
                        Guid = new RssGuid(newsUrl.ToString(), true)
                    };

                    items.Add(item);
                }

                channel.Items = items;
                channel.PublicationDate = mostRecentPubDate;
                return channel;
            }, cancellationToken);
        }

        public string Name { get; } = "BioWare RSS";
    }
}