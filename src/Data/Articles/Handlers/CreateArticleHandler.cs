using System.Threading.Tasks;
using BioEngine.Data.Articles.Commands;
using BioEngine.Data.Core;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Articles.Handlers
{
    [UsedImplicitly]
    internal class CreateArticleHandler : RestCommandHandlerBase<CreateArticleCommand, int>
    {
        public CreateArticleHandler(HandlerContext<CreateArticleHandler> context,
            IValidator<CreateArticleCommand>[] validators)
            : base(context, validators)
        {
        }

        protected override async Task<int> ExecuteCommandAsync(CreateArticleCommand createCommand)
        {
            var article = Mapper.Map<CreateArticleCommand, Common.Models.Article>(createCommand);
            DBContext.Articles.Add(article);
            await DBContext.SaveChangesAsync();

            return article.Id;
        }
    }
}