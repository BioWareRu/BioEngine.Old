using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BioEngine.Content.Helpers
{
    [UsedImplicitly]
    public class PatreonApiHelper
    {
        private readonly ILogger<PatreonApiHelper> _logger;
        private readonly Uri _apiUrl;
        private HttpClient _httpClient;
        private readonly PatreonConfig _config;
        private string _accessToken;

        public PatreonApiHelper(IOptions<PatreonConfig> config, ILogger<PatreonApiHelper> logger)
        {
            _logger = logger;
            _apiUrl = config.Value.ApiUrl;
            _httpClient = new HttpClient();
            _config = config.Value;
            CreateHttpClient();
        }

        private async Task<string> GetReponseJsonAsync(string path)
        {
            var url = _apiUrl + path;
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                CreateHttpClient();
                return await GetReponseJsonAsync(path);
            }
            throw new Exception($"Error accessing patreon: {response.StatusCode}");
        }

        private string GetAccessToken()
        {
            var url = _apiUrl + "/token?grant_type=refresh_token"
                      + $"&refresh_token={_config.RefreshToken}"
                      + $"&client_id={_config.ClientId}"
                      + $"&client_secret={_config.ClientSecret}";
            var response = _httpClient.PostAsync(url, null).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                var tokenObj =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(
                        response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                return tokenObj["access_token"];
            }
            _logger.LogError(
                $"Can't referesh patreon token. Status code: {response.StatusCode}. Response: {response.Content.ReadAsStringAsync().GetAwaiter().GetResult()}");
            throw new Exception("Patreon refresh token error");
        }

        private HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
            return httpClient;
        }

        private void CreateHttpClient()
        {
            _accessToken = GetAccessToken();
            _httpClient = GetHttpClient();
        }

        private static List<T> GetIncluded<T>(string json, string type)
        {
            var jToken = JToken.Parse(json);
            var included = jToken["included"].AsJEnumerable();

            return (from value in included
                where value["type"].ToString() == type
                select value["attributes"].ToObject<T>()
                into obj
                where obj != null
                select obj).ToList();
        }

        public async Task<PatreonGoal> GetCurrentGoalAsync()
        {
            var goals = await GetGoalsAsync();
            var currentGoal = goals.Where(x => x.CompletedPercentage < 100)
                .OrderByDescending(x => x.CompletedPercentage).First();
            return currentGoal;
        }

        public async Task<List<PatreonGoal>> GetGoalsAsync()
        {
            var json = await GetReponseJsonAsync("/api/current_user/campaigns?include-goals");

            return GetIncluded<PatreonGoal>(json, "goal");
        }
    }

    public class PatreonGoal
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("amount_cents")]
        public int AmountCents { get; set; }

        [JsonProperty("completed_percentage")]
        public int CompletedPercentage { get; set; }

        [JsonProperty("reached_at")]
        public DateTime? ReachedAt { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        public int GetAmount()
        {
            return Amount / 100;
        }

        public int GetCompletedAmount()
        {
            return (int) Math.Ceiling((double) GetAmount() * CompletedPercentage / 100);
        }
    }

    public class PatreonConfig
    {
        public Uri ApiUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RefreshToken { get; set; }
    }
}