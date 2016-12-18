using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Site.Components.Url;
using BioEngine.Site.Helpers;
using cloudscribe.Syndication.Models.Rss;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Components
{
    public class RssProvider : IChannelProvider
    {
        private readonly BWContext _dbContext;
        private readonly UrlManager _urlManager;

        public RssProvider(IOptions<AppSettings> options, BWContext dbContext, UrlManager urlManager)
        {
            _dbContext = dbContext;
            _urlManager = urlManager;
            _appSettings = options.Value;
        }

        private readonly AppSettings _appSettings;

        public Task<RssChannel> GetChannel(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.Run(() =>
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
                        new RssImage(new Uri(_appSettings.SocialLogo), _appSettings.Title,
                            new Uri(_appSettings.SiteDomain))
                };

                var latestNews =
                    _dbContext.News.OrderByDescending(x => x.Sticky)
                        .ThenByDescending(x => x.Id)
                        .Where(x => x.Pub == 1)
                        .Include(x => x.Author)
                        .Take(20)
                        .ToList();
                var mostRecentPubDate = DateTime.MinValue;
                var items = new List<RssItem>();
                foreach (var news in latestNews)
                {
                    var newsDate = DateTimeOffset.FromUnixTimeSeconds(news.LastChangeDate).Date;
                    if (newsDate > mostRecentPubDate) mostRecentPubDate = newsDate;
                    var newsUrl = _urlManager.News.PublicUrl(news, true);
                    var item = new RssItem
                    {
                        Title = news.Title,
                        Description = news.ShortText,
                        Link = new Uri(newsUrl),
                        PublicationDate = newsDate,
                        Author = news.Author.Name,
                        Guid = new RssGuid(newsUrl, true)
                    };
                    var imgUrl = ContentHelper.GetImageUrl(news.ShortText);
                    if (imgUrl != null)
                    {
                        long size;
                        string mimeType;
                        ContentHelper.GetSizeAndMime(imgUrl, out size, out mimeType);
                        item.Enclosures.Add(new RssEnclosure(size, mimeType, imgUrl));
                    }

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