using BioEngine.Data.Core;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.News.Commands
{
    public sealed class UnPinNewsCommand : UpdateCommand<Common.Models.News>
    {
        public UnPinNewsCommand(Common.Models.News news)
        {
            Model = news;
        }
    }
    
    [UsedImplicitly]
    internal class UnPinNewsCommandValidator : AbstractValidator<UnPinNewsCommand>
    {
        public UnPinNewsCommandValidator()
        {
            RuleFor(x => x.Model.Sticky).Must(sticky => sticky == 1)
                .WithMessage("Нельзя открепить незакреплённую новость");
        }
    }
}