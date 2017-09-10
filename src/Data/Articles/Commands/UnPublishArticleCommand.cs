using BioEngine.Data.Articles.Commands;
using BioEngine.Data.Core;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Articles.Commands
{
    public sealed class UnPublishArticleCommand : UpdateCommand<Common.Models.Article>
    {
        public UnPublishArticleCommand(Common.Models.Article article)
        {
            Model = article;
        }
    }
}

namespace BioEngine.Data.Articles.Queries
{
    [UsedImplicitly]
    internal class UnPublishArticleCommandValidator : AbstractValidator<UnPublishArticleCommand>
    {
        public UnPublishArticleCommandValidator()
        {
            RuleFor(x => x.Model.Pub).Must(pub => pub == 1).WithMessage("Нельзя скрыть неопубликованную статью");
        }
    }
}