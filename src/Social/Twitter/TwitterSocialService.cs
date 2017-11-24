using System;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using Microsoft.Extensions.Logging;

namespace BioEngine.Social.Twitter
{
    public class TwitterSocialService : TwitterService, ISocialService
    {
        public TwitterSocialService(TwitterServiceConfiguration configuration, ILogger<TwitterService> logger) : base(
            configuration, logger)
        {
        }

        public Task<bool> PublishNews(News news, bool forceUpdate = false)
        {
            var result = false;
            if (news.TwitterId == 0 || forceUpdate)
            {
                if (news.TwitterId > 0)
                {
                    var deleted = DeleteTweet(news.TwitterId);
                    if (!deleted)
                    {
                        throw new Exception("Can't delete news tweet");
                    }
                }
                var text = $"{news.Title} {news.PublicUrl} #rpg";
                if (!string.IsNullOrEmpty(news.Parent.TwitterTag))
                {
                    text += $" #{news.Parent.TwitterTag}";
                }

                var newTweetId = CreateTweet(text);
                if (newTweetId > 0)
                {
                    news.TwitterId = newTweetId;
                    result = true;
                }
            }

            return Task.FromResult(result);
        }

        public Task<bool> DeleteNews(News news)
        {
            var result = false;
            if (news.TwitterId > 0)
            {
                result = DeleteTweet(news.TwitterId);
                if (result)
                {
                    news.TwitterId = 0;
                }
            }
            return Task.FromResult(result);
        }
    }
}