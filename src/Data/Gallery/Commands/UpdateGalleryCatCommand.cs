using BioEngine.Data.Core;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Gallery.Commands
{
    [UsedImplicitly]
    public sealed class UpdateGalleryCatCommand: UpdateCommand<Common.Models.GalleryCat>, IChildModelCommand
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Desc { get; set; }
        
        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? TopicId { get; set; }
        public int? CatId { get; set; }
        
        public UpdateGalleryCatCommand(Common.Models.GalleryCat galleryCat)
        {
            Model = galleryCat;
        }
    }
    
    [UsedImplicitly]
    internal class UpdateGalleryCatCommandValidator : AbstractValidator<UpdateGalleryCatCommand>
    {
        public UpdateGalleryCatCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Url).NotEmpty().MaximumLength(255);
            RuleFor(x => x.GameId).SetValidator(new ChildValidator(false));
            RuleFor(x => x.DeveloperId).SetValidator(new ChildValidator(false));
        }
    }
}