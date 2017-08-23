using BioEngine.Data.Core;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.News.Commands
{
    public sealed class UnPublishNewsCommand : UpdateCommand<Common.Models.News>
    {
        public UnPublishNewsCommand(Common.Models.News news)
        {
            Model = news;
        }
    }
    
    [UsedImplicitly]
    internal class UnPublishNewsCommandValidator : AbstractValidator<UnPublishNewsCommand>
    {
        public UnPublishNewsCommandValidator()
        {
            RuleFor(x => x.Model.Pub).Must(pub => pub == 1).WithMessage("Нельзя скрыть неопубликованную новость");
        }
    }
}