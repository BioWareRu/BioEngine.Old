using System;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Articles.Commands
{
    public class CreateArticleCommand : CreateCommand<int>, IChildModelCommand
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public int? CatId { get; set; }
        public string Announce { get; set; }
        public string Text { get; set; }

        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? TopicId { get; set; }
        public int AuthorId { get; }
        public long Date { get; }

        public CreateArticleCommand(User user)
        {
            AuthorId = user.Id;
            Date = DateTimeOffset.Now.ToUnixTimeSeconds();
        }
    }

    [UsedImplicitly]
    internal class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
    {
        public CreateArticleCommandValidator()
        {
            RuleFor(x => x.Source).NotEmpty().MaximumLength(255).SetValidator(new UrlValidator());
            RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Url).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Announce).NotEmpty();
            RuleFor(x => x.Text).NotEmpty();
            RuleFor(x => x.GameId).SetValidator(new ChildValidator(true));
            RuleFor(x => x.DeveloperId).SetValidator(new ChildValidator(true));
            RuleFor(x => x.TopicId).SetValidator(new ChildValidator(true));
            RuleFor(x => x.AuthorId).NotEmpty();
            RuleFor(x => x.CatId).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
        }
    }
}