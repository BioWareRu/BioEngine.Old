using System;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BioEngine.Content.Helpers
{
    [UsedImplicitly]
    public class PatreonApiHelper
    {
        private readonly ILogger<PatreonApiHelper> _logger;
        private readonly PatreonConfig _config;

        public PatreonApiHelper(IOptions<PatreonConfig> config, ILogger<PatreonApiHelper> logger)
        {
            _logger = logger;
            _config = config.Value;
        }

        private async Task<T> GetAsync<T>(string path)
        {
            var builder = new UriBuilder(_config.ServiceUrl) {Path = path};
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(builder.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(json);
                }
                _logger.LogError($"Error accessing patreon: {response.StatusCode}");
                throw new Exception($"Error accessing patreon: {response.StatusCode}");
            }
        }

        public async Task<PatreonGoal> GetCurrentGoalAsync()
        {
            try
            {
                var goal = await GetAsync<PatreonGoal>("/v1/goals/current");
                return goal ?? new PatreonGoal();
            }
            catch (Exception)
            {
                return new PatreonGoal();
            }
        }
    }

    public class PatreonGoal
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("current_amount")]
        public int CurrentAmount { get; set; }

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
    }

    public class PatreonConfig
    {
        public Uri ServiceUrl { get; set; }
    }
}