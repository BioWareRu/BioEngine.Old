using BioEngine.Data.Core;

namespace BioEngine.Data.News.Commands
{
    public class CreateOrUpdateNewsForumTopicCommand : CommandBase
    {
        public CreateOrUpdateNewsForumTopicCommand(Common.Models.News news)
        {
            News = news;
        }

        public Common.Models.News News { get; }
    }
}