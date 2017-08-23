using BioEngine.Data.Core;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.News.Commands
{
    public sealed class PinNewsCommand : UpdateCommand<Common.Models.News>
    {
        public PinNewsCommand(Common.Models.News news)
        {
            Model = news;
        }
    }
    
    [UsedImplicitly]
    internal class PinNewsCommandValidator : AbstractValidator<PinNewsCommand>
    {
        public PinNewsCommandValidator()
        {
            RuleFor(x => x.Model.Sticky).Must(sticky => sticky == 0)
                .WithMessage("Нельзя закрепить уже закреплённую новость");
        }
    }
}