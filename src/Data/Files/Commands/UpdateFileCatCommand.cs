using BioEngine.Data.Core;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Files.Commands
{
    [UsedImplicitly]
    public sealed class UpdateFileCatCommand: UpdateCommand<Common.Models.FileCat>, IChildModelCommand
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Descr { get; set; }
        
        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? TopicId { get; set; }
        public int? CatId { get; set; }
        
        public UpdateFileCatCommand(Common.Models.FileCat fileCat)
        {
            Model = fileCat;
        }
    }
    
    [UsedImplicitly]
    internal class UpdateFileCatCommandValidator : AbstractValidator<UpdateFileCatCommand>
    {
        public UpdateFileCatCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Url).NotEmpty().MaximumLength(255);
            RuleFor(x => x.GameId).SetValidator(new ChildValidator(false));
            RuleFor(x => x.DeveloperId).SetValidator(new ChildValidator(false));
        }
    }
}