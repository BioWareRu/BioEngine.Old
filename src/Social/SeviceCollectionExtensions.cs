using BioEngine.Social.Facebook;
using BioEngine.Social.Twitter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BioEngine.Social
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

            services.AddSingleton(new FacebookServiceConfiguration(
                configuration["BE_FACEBOOK_API_URL"],
                configuration["BE_FACEBOOK_PAGE_ID"],
                configuration["BE_FACEBOOK_ACCESS_TOKEN"]
            ));


            services.AddSingleton<ISocialService, TwitterSocialService>();
            //services.AddSingleton<ISocialService, FacebookSocialService>();
        }
    }
}