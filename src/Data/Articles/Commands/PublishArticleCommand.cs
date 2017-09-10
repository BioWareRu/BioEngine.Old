using BioEngine.Data.Articles.Commands;
using BioEngine.Data.Core;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Articles.Commands
{
    public sealed class PublishArticleCommand : UpdateCommand<Common.Models.Article>
    {
        public PublishArticleCommand(Common.Models.Article article)
        {
            Model = article;
        }
    }
}

namespace BioEngine.Data.Articles.Queries
{
    [UsedImplicitly]
    internal class PublishArticleCommandValidator : AbstractValidator<PublishArticleCommand>
    {
        public PublishArticleCommandValidator()
        {
            RuleFor(x => x.Model.Pub).Must(pub => pub == 0).WithMessage("Статья уже опубликована");
        }
    }
}