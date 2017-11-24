using System;

namespace BioEngine.Social.Facebook
{
    public class FacebookServiceConfiguration
    {
        public FacebookServiceConfiguration(string apiUrl, string pageId, string accessToken)
        {
            ApiURL = new Uri(apiUrl);
            PageId = pageId;
            AccessToken = accessToken;
        }

        public Uri ApiURL { get; }
        public string PageId { get; }
        public string AccessToken { get; }
    }
}