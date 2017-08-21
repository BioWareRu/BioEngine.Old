using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BioEngine.Content.Helpers
{
    [UsedImplicitly]
    public class PatreonApiHelper
    {
        private readonly Uri _apiUrl;
        private readonly HttpClient _httpClient;

        public PatreonApiHelper(IOptions<PatreonConfig> config)
        {
            _apiUrl = config.Value.ApiUrl;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.Value.ApiKey}");
        }

        private async Task<string> GetReponseJson(string path)
        {
            var url = _apiUrl + path;
            return await _httpClient.GetStringAsync(url);
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

        public async Task<PatreonGoal> GetCurrentGoal()
        {
            var goals = await GetGoals();
            var currentGoal = goals.Where(x => x.CompletedPercentage < 100)
                .OrderByDescending(x => x.CompletedPercentage).First();
            return currentGoal;
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
        public Uri ApiUrl { get; set; }
        public string ApiKey { get; set; }
    }
}