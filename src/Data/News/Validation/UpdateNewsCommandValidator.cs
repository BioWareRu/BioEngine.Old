using BioEngine.Data.Core;
using BioEngine.Data.News.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.News.Validation
{
    [UsedImplicitly]
    internal class UpdateNewsCommandValidator : AbstractValidator<UpdateNewsCommand>
    {
        public UpdateNewsCommandValidator()
        {
            RuleFor(x => x.Source).NotEmpty().MaximumLength(255).SetValidator(new UrlValidator());
            RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Url).NotEmpty().MaximumLength(255);
            RuleFor(x => x.ShortText).NotEmpty();
            RuleFor(x => x.GameId).SetValidator(new ChildValidator(true));
            RuleFor(x => x.DeveloperId).SetValidator(new ChildValidator(true));
            RuleFor(x => x.TopicId).SetValidator(new ChildValidator(true));
            RuleFor(x => x.LastChangeDate).NotEmpty();
            RuleFor(x => x.Model).NotEmpty();
        }
    }
}