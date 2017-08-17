using BioEngine.Data.Core;

namespace BioEngine.Data.News.Commands
{
    public sealed class UnPublishNewsCommand : UpdateCommand<Common.Models.News>
    {
        public UnPublishNewsCommand(Common.Models.News news)
        {
            Model = news;
        }
    }
}