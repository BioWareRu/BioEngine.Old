using BioEngine.Data.Core;

namespace BioEngine.Data.News.Commands
{
    public sealed class PublishNewsCommand : UpdateCommand<Common.Models.News>
    {
        public PublishNewsCommand(Common.Models.News news)
        {
            Model = news;
        }
    }
}