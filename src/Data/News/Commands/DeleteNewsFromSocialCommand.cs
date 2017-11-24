using BioEngine.Data.Core;

namespace BioEngine.Data.News.Commands
{
    public class DeleteNewsFromSocialCommand : QueryBase<bool>
    {
        public DeleteNewsFromSocialCommand(Common.Models.News news)
        {
            News = news;
        }

        public Common.Models.News News { get; }
    }
}