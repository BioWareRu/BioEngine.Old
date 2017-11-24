using BioEngine.Data.Core;

namespace BioEngine.Data.News.Commands
{
    public class PublishNewsToSocialCommand : QueryBase<bool>
    {
        public PublishNewsToSocialCommand(Common.Models.News news, bool needUpdate = false)
        {
            News = news;
            NeedUpdate = needUpdate;
        }

        public Common.Models.News News { get; }
        public bool NeedUpdate { get; }
    }
}