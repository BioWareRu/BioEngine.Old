using BioEngine.Data.Core;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Articles.Commands
{
    public class CreateArticleCatCommand : CreateCommand<int>, IChildModelCommand
    {
        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? CatId { get; set; }
        public int? TopicId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Descr { get; set; }
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
            RuleFor(x => x.GameId).SetValidator(new ChildValidator(true));
            RuleFor(x => x.DeveloperId).SetValidator(new ChildValidator(true));
            RuleFor(x => x.TopicId).SetValidator(new ChildValidator(true));
        }
    }
}