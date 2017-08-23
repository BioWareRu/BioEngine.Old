using BioEngine.Data.Core;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.News.Commands
{
    public sealed class PublishNewsCommand : UpdateCommand<Common.Models.News>
    {
        public PublishNewsCommand(Common.Models.News news)
        {
            Model = news;
        }
    }
    
    [UsedImplicitly]
    internal class PublishNewsCommandValidator : AbstractValidator<PublishNewsCommand>
    {
        public PublishNewsCommandValidator()
        {
            RuleFor(x => x.Model.Pub).Must(pub => pub == 0).WithMessage("Новость уже опубликована");
        }
    }
}