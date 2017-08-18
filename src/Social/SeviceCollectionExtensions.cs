using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Social
{
    public static class SeviceCollectionExtensions
    {
        public static void AddBioEngineSocial(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new TwitterServiceConfiguration(
                configuration["BE_TWITTER_CONSUMER_KEY"],
                configuration["BE_TWITTER_CONSUMER_SECRET"],
                configuration["BE_TWITTER_ACCESS_TOKEN"],
                configuration["BE_TWITTER_ACCESS_TOKEN_SECRET"]
            ));

            services.AddSingleton<TwitterService>();
        }
    }
}