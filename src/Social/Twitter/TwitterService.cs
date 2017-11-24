using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tweetinvi;
using Tweetinvi.Models;

namespace BioEngine.Social.Twitter
{
    [UsedImplicitly]
    public class TwitterService
    {
        private readonly ILogger<TwitterService> _logger;

        public TwitterService(TwitterServiceConfiguration configuration, ILogger<TwitterService> logger)
        {
            _logger = logger;
            Auth.SetCredentials(new TwitterCredentials(configuration.ConsumerKey, configuration.ConsumerSecret,
                configuration.AccessToken, configuration.AcessTokenSecret));
        }

        public long CreateTweet(string text)
        {
            var tweet = Tweet.PublishTweet(text);
            CheckExceptions();
            return tweet.Id;
        }

        public bool DeleteTweet(long tweetId)
        {
            var result = Tweet.DestroyTweet(tweetId);
            CheckExceptions("Невозможно удалить старый твит");
            return result;
        }

        private void CheckExceptions(string message = null)
        {
            var exc = ExceptionHandler.GetLastException();
            if (exc != null)
            {
                _logger.LogError(exc.TwitterDescription);
                _logger.LogError(string.Concat(exc.TwitterExceptionInfos.SelectMany(x => x.Message)));
                if (exc.TwitterExceptionInfos.Any(x => x.Message == "Status is over 140 characters."))
                {
                    throw new TooLongTweetTextException();
                }
                throw new TwitterException(!string.IsNullOrEmpty(message) ? message : exc.TwitterDescription);
            }
        }
    }
}