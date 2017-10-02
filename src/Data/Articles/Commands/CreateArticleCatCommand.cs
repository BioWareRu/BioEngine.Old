using BioEngine.Data.Core;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Articles.Commands
{
    public class CreateArticleCatCommand : CreateCommand<int>, IChildModelCommand
    {
        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? Pid { get; set; }
        public int? TopicId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Descr { get; set; }
        [CanBeNull]
        public string Content { get; set; }
        public int? Articles { get; set; }
    }
    
    [UsedImplicitly]
    internal class CreateArticleCatCommandValidator : AbstractValidator<CreateArticleCatCommand>
    {
        public CreateArticleCatCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Url).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Content).Null();
            RuleFor(x => x.Articles).Null();
            RuleFor(x => x.Descr).Null();
            RuleFor(x => x.Pid).NotNull();
            RuleFor(x => x.GameId).SetValidator(new ChildValidator(false));
            RuleFor(x => x.DeveloperId).SetValidator(new ChildValidator(false));
        }
    }
}