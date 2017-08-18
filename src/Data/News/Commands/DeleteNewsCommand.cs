using BioEngine.Data.Core;

namespace BioEngine.Data.News.Commands
{
    public sealed class DeleteNewsCommand : UpdateCommand<Common.Models.News>
    {
        public DeleteNewsCommand(Common.Models.News news)
        {
            Model = news;
        }
    }
}