using BioEngine.Data.Core;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Files.Commands
{
    public class CreateFileCatCommand : CreateCommand<int>, IChildModelCommand
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Descr { get; set; }
        
        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? Pid { get; set; }
        public int? TopicId { get; set; }
    }
    
    [UsedImplicitly]
    internal class CreateFileCatCommandValidator : AbstractValidator<CreateFileCatCommand>
    {
        public CreateFileCatCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Url).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Descr).Empty();
            RuleFor(x => x.GameId).SetValidator(new ChildValidator(false));
            RuleFor(x => x.DeveloperId).SetValidator(new ChildValidator(false));
            RuleFor(x => x.Pid).Empty();
        }
    }
}