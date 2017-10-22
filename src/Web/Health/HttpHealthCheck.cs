using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace BioEngine.Web.Health
{
    public abstract class HttpHealthCheck : HealthCheck
    {
        private readonly HttpClient _httpClient;
        protected abstract Uri Uri { get; }
        protected abstract Func<string, bool> Check { get; }

        protected HttpHealthCheck(string name) : base(name)
        {
            _httpClient = new HttpClient();
        }

        protected override async ValueTask<HealthCheckResult> CheckAsync(
            CancellationToken token = default(CancellationToken))
        {
            var result = await _httpClient.GetStringAsync(Uri);
            return Check(result) ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
        }
    }

    [UsedImplicitly]
    public class GraylogHealthCheck : HttpHealthCheck
    {
        protected override Uri Uri { get; }
        protected override Func<string, bool> Check { get; } = result => result == "ALIVE";

        public GraylogHealthCheck(IOptions<GraylogHealthCheckOptions> options) : base("Graylog is alive")
        {
            Uri = options.Value.Uri;
        }
    }

    public class GraylogHealthCheckOptions
    {
        public Uri Uri { get; set; }
    }

}