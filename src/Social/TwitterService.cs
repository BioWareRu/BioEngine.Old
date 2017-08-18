using System.Threading.Tasks;
using JetBrains.Annotations;
using Tweetinvi;
using Tweetinvi.Models;

namespace Social
{
    [UsedImplicitly]
    public class TwitterService
    {
        private readonly TwitterCredentials _credentials;

        public TwitterService(TwitterServiceConfiguration configuration)
        {
            _credentials = new TwitterCredentials(configuration.ConsumerKey, configuration.ConsumerSecret,
                configuration.AccessToken, configuration.AcessTokenSecret);
        }

        public async Task<long> CreateTweet(string text)
        {
            var tweet = await Auth.ExecuteOperationWithCredentials(_credentials, () => TweetAsync.PublishTweet(text));

            return tweet.Id;
        }

        public async Task<bool> DeleteTweet(long tweetId)
        {
            var result =
                await Auth.ExecuteOperationWithCredentials(_credentials, () => TweetAsync.DestroyTweet(tweetId));
            return result;
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
}