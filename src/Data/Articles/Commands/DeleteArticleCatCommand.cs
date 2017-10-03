using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Articles.Commands
{
    public sealed class DeleteArticleCatCommand : UpdateCommand<ArticleCat>
    {
        public DeleteArticleCatCommand(ArticleCat articleCat)
        {
            Model = articleCat;
        }
    }
}