using BioEngine.Data.Core;

namespace BioEngine.Data.Articles.Commands
{
    public sealed class DeleteArticleCommand : UpdateCommand<Common.Models.Article>
    {
        public DeleteArticleCommand(Common.Models.Article article)
        {
            Model = article;
        }
    }
}