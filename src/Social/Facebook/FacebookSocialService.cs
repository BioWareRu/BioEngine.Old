using System;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using Microsoft.Extensions.Logging;

namespace BioEngine.Social.Facebook
{
    public class FacebookSocialService : FacebookService, ISocialService
    {
        public FacebookSocialService(FacebookServiceConfiguration configuration, ILogger<FacebookService> logger) :
            base(configuration, logger)
        {
        }

        public async Task<bool> PublishNews(News news, bool forceUpdate = false)
        {
            var result = false;
            if (string.IsNullOrEmpty(news.FacebookId) || forceUpdate)
            {
                if (!string.IsNullOrEmpty(news.FacebookId))
                {
                    var deleted = await DeletePost(news.FacebookId);
                    if (!deleted)
                    {
                        throw new Exception("Can't delete news facebook post");
                    }
                }

                var newPostId = await PostLink(news.PublicUrl);
                if (!string.IsNullOrEmpty(newPostId))
                {
                    news.FacebookId = newPostId;
                    result = true;
                }
            }

            return result;
        }

        public async Task<bool> DeleteNews(News news)
        {
            var result = false;
            if (!string.IsNullOrEmpty(news.FacebookId))
            {
                result = await DeletePost(news.FacebookId);
                if (result)
                {
                    news.FacebookId = null;
                }
            }
            return result;
        }
    }
}