namespace BioEngine.Social.Twitter
{
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
}