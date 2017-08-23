using BioEngine.Data.Core;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.News.Commands
{
    [UsedImplicitly]
    public sealed class UpdateNewsCommand : UpdateCommand<Common.Models.News>, IChildModelCommand
    {
        public string Source { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string ShortText { get; set; }
        public string AddText { get; set; }
        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? TopicId { get; set; }
        public long LastChangeDate { get; set; }

        public UpdateNewsCommand(Common.Models.News news)
        {
            Model = news;
        }
    }
    
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
            RuleFor(x => x.Model).NotEmpty();
        }
    }
}