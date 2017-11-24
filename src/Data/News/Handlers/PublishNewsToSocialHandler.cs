using System;
using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.News.Commands;
using BioEngine.Social;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class PublishNewsToSocialHandler : QueryHandlerBase<PublishNewsToSocialCommand, bool>
    {
        private readonly ISocialService[] _socialServices;

        public PublishNewsToSocialHandler(HandlerContext<PublishNewsToSocialHandler> context,
            ISocialService[] socialServices) : base(context)
        {
            _socialServices = socialServices;
        }

        protected override async Task<bool> RunQueryAsync(PublishNewsToSocialCommand message)
        {
            var needSave = false;
            foreach (var socialService in _socialServices)
            {
                try
                {
                    var posted = await socialService.PublishNews(message.News);
                    if (!posted)
                    {
                        Logger.LogError($"News wasn't posted to social service {socialService.GetType()}");
                    }
                    else
                    {
                        Logger.LogInformation($"News was posted to social service {socialService.GetType()}");
                        needSave = true;
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError($"Can't post news to social service {socialService.GetType()}: {e.Message}");
                }
            }
            
            if (needSave)
            {
                DBContext.Update(message.News);
                await DBContext.SaveChangesAsync();
            }

            return true;
        }
    }
}