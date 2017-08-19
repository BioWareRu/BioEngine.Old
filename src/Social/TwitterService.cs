using System.Linq;
using BioEngine.Common.Base;
using JetBrains.Annotations;
using Tweetinvi;
using Tweetinvi.Models;

namespace BioEngine.Social
{
    [UsedImplicitly]
    public class TwitterService
    {
        public TwitterService(TwitterServiceConfiguration configuration)
        {
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
                if (exc.TwitterExceptionInfos.Any(x => x.Message == "Status is over 140 characters."))
                {
                    throw new TooLongTweetTextException();
                }
                throw new TwitterException(!string.IsNullOrEmpty(message) ? message : exc.TwitterDescription);
            }
        }
    }

    public class TwitterServiceConfiguration
    {
        public TwitterServiceConfiguration(string consumerKey, string consumerSecret, string accessToken,
            string acessTokenSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            AccessToken = accessToken;
            AcessTokenSecret = acessTokenSecret;
        }

        public string ConsumerKey { get; }
        public string ConsumerSecret { get; }
        public string AccessToken { get; }
        public string AcessTokenSecret { get; }
    }

    public enum TwitterOperationEnum
    {
        CreateOrUpdate,
        Delete
    }

    public class TwitterException : UserException
    {
        public TwitterException(string message) : base(message)
        {
        }
    }

    public class TooLongTweetTextException : TwitterException
    {
        public TooLongTweetTextException() : base("Текст твита больше 140 символов")
        {
        }
    }
}