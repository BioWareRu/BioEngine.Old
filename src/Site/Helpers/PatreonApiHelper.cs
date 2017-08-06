using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BioEngine.Site.Helpers
{
    public class PatreonApiHelper
    {
        private readonly Uri _apiUrl;
        private readonly HttpClient _httpClient;

        public PatreonApiHelper(PatreonConfig config)
        {
            _apiUrl = config.ApiUrl;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.ApiKey}");
        }

        private async Task<string> GetReponseJson(string path)
        {
            var url = _apiUrl + path;
            return await _httpClient.GetStringAsync(url);
        }

        private static List<T> GetIncluded<T>(string json, string type)
        {
            var jToken = JToken.Parse(json);
            var includedObjects = new List<T>();
            var included = jToken["included"].AsJEnumerable();
            foreach (var value in included)
            {
                if (value["type"].ToString() == type)
                {
                    var obj = value["attributes"].ToObject<T>();
                    if (obj != null)
                    {
                        includedObjects.Add(obj);
                    }
                }
            }

            return includedObjects;
        }

        public async Task<List<PatreonGoal>> GetGoals()
        {
            var json = await GetReponseJson("/current_user/campaigns?include-goals");

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
        public PatreonConfig(Uri apiUrl, string apiKey)
        {
            ApiUrl = apiUrl;
            ApiKey = apiKey;
        }

        public Uri ApiUrl { get; }
        public string ApiKey { get; }
    }
}