using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BioEngine.Social.Test
{
    public class FacebookTest
    {
        private readonly ServiceProvider _services;

        public FacebookTest()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets("6669c488-cfea-43d0-a0f5-a50c9ee9ca59")
                .Build();

            var services = new ServiceCollection();
            services.AddBioEngineSocial(config);
            services.AddLogging();

            _services = services.BuildServiceProvider();
        }

        [Fact]
        public async Task TestPostLink()
        {
            var facebookService = _services.GetService<FacebookService>();

            var link = new Uri("https://www.bioware.ru/");

            var postId = await facebookService.PostLink(link);

            Assert.NotEmpty(postId);

            var deleted = await facebookService.DeletePost(postId);

            Assert.True(deleted);
        }
    }
}