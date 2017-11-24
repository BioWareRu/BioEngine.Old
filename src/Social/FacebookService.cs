using System;
using System.Threading.Tasks;
using Flurl.Http;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tweetinvi.Core.Extensions;

namespace BioEngine.Social
{
    [UsedImplicitly]
    public class FacebookService
    {
        private readonly ILogger<FacebookService> _logger;
        private readonly FacebookServiceConfiguration _configuration;

        public FacebookService(FacebookServiceConfiguration configuration, ILogger<FacebookService> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> PostLink(Uri link)
        {
            _logger.LogDebug("Post new link to facebook");
            var response =
                await $"{_configuration.ApiURL}/{_configuration.PageId}/feed"
                    .AddParameterToQuery("link", link.ToString())
                    .AddParameterToQuery("access_token", _configuration.AccessToken)
                    .WithTimeout(60)
                    .PostUrlEncodedAsync(new { });
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error while sending facebook request");
                throw new Exception($"Bad facebook response: {data}");
            }

            var postReponse = JsonConvert.DeserializeObject<FacebookNewPostResponse>(data);
            _logger.LogDebug("Link posted to facebook");
            return postReponse.Id;
        }

        public async Task<bool> DeletePost(string postId)
        {
            _logger.LogDebug("Delete post from facebook");
            var response =
                await $"{_configuration.ApiURL}/{postId}"
                    .AddParameterToQuery("access_token", _configuration.AccessToken)
                    .DeleteAsync();
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error while sending facebook request");
                throw new Exception($"Bad facebook response: {data}");
            }

            var postReponse = JsonConvert.DeserializeObject<FacebookDeleteResponse>(data);
            _logger.LogDebug($"Post deleted from facebook: {(postReponse.Success ? "Yes" : "No")}");
            return postReponse.Success;
        }
    }

    public class FacebookNewPostResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class FacebookDeleteResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }

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
    
    public enum FacebookOperationEnum
    {
        CreateOrUpdate,
        Delete
    }
}