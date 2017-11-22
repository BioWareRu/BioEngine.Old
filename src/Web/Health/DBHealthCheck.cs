using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using BioEngine.Data.DB;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BioEngine.Web.Health
{
    [UsedImplicitly]
    public class DatabaseHealthCheck : HealthCheck
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DatabaseHealthCheck(IServiceScopeFactory scopeFactory)
            : base("DatabaseCheck")
        {
            _scopeFactory = scopeFactory;
        }

        protected override async ValueTask<HealthCheckResult> CheckAsync(
            CancellationToken token = default(CancellationToken))
        {
            var context = _scopeFactory.CreateScope().ServiceProvider.GetService<BWContext>();

            // exceptions will be caught and the result will be un-healthy
            await context.Database.OpenConnectionAsync(token);

            return HealthCheckResult.Healthy();
        }
    }

}