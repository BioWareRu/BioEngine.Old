using BioEngine.Data.Core;

namespace BioEngine.Data.News.Commands
{
    public class DeleteNewsForumTopicCommand : CommandBase
    {
        public DeleteNewsForumTopicCommand(Common.Models.News news)
        {
            News = news;
        }

        public Common.Models.News News { get; }
    }
}