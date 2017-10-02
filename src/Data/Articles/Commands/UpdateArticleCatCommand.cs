﻿using BioEngine.Data.Core;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Articles.Commands
{
    [UsedImplicitly]
    public sealed class UpdateArticleCatCommand: UpdateCommand<Common.Models.ArticleCat>, IChildModelCommand
    {
        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? Pid { get; set; }
        public int? TopicId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Descr { get; set; }
        public string Content { get; set; }
        public int? Articles { get; set; }
        
        public UpdateArticleCatCommand(Common.Models.ArticleCat articleCat)
        {
            Model = articleCat;
        }
    }
    
    [UsedImplicitly]
    internal class UpdateArticleCatCommandValidator : AbstractValidator<UpdateArticleCatCommand>
    {
        public UpdateArticleCatCommandValidator()
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