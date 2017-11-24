using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.News.Commands;
using BioEngine.Social;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class DeleteNewsFromSocialHandler : QueryHandlerBase<DeleteNewsFromSocialCommand, bool>
    {
        private readonly List<SocialServiceInterface> _socialServices;

        public DeleteNewsFromSocialHandler(HandlerContext<DeleteNewsFromSocialHandler> context,
            List<SocialServiceInterface> socialServices) : base(context)
        {
            _socialServices = socialServices;
        }

        protected override async Task<bool> RunQueryAsync(DeleteNewsFromSocialCommand message)
        {
            var needSave = false;
            foreach (var socialService in _socialServices)
            {
                try
                {
                    var posted = await socialService.DeleteNews(message.News);
                    if (!posted)
                    {
                        Logger.LogError($"News wasn't deleted from social service {socialService.GetType()}");
                    }
                    else
                    {
                        Logger.LogInformation($"News was deleted from social service {socialService.GetType()}");
                        needSave = true;
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError($"Can't delete news from social service {socialService.GetType()}: {e.Message}");
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