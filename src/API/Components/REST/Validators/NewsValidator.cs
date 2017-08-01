using System;
using System.Collections.Generic;
using System.Linq;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using FluentValidation;
using FluentValidation.Validators;

namespace BioEngine.API.Components.REST.Validators
{
    public class NewsValidator : AbstractValidator<News>
    {
        public NewsValidator()
        {
            RuleFor(x => x.Source).NotEmpty().MaximumLength(255).SetValidator(new UrlValidator());
            RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Url).NotEmpty().MaximumLength(255);
            RuleFor(x => x.ShortText).NotEmpty();
            RuleFor(x => x.LastChangeDate).NotEmpty();
            RuleFor(x => x.AuthorId).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.GameId).SetValidator(new ChildValidator(true));
            RuleFor(x => x.DeveloperId).SetValidator(new ChildValidator(true));
            RuleFor(x => x.TopicId).SetValidator(new ChildValidator(true));
        }
    }

    public class UrlValidator : PropertyValidator
    {
        public UrlValidator() : base("{PropertyName} не является URL")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue is string value)
            {
                try
                {
                    var unused = new Uri(value);
                }
                catch (Exception)
                {
                    context.MessageFormatter.AppendArgument("PropertyName", context.PropertyName);
                    return false;
                }
            }

            return true;
        }
    }

    public class ChildValidator : PropertyValidator
    {
        private readonly bool _checkTopic;

        public ChildValidator(bool checkTopic)
            : base("{Message}")
        {
            _checkTopic = checkTopic;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var model = context.Instance as IChildModel;
            if (model == null) return false;

            var properties = new Dictionary<string, bool>
            {
                {"GameId", model.GameId != null},
                {"DeveloperId", model.DeveloperId != null}
            };

            if (_checkTopic)
            {
                properties.Add("TopicId", model.TopicId != null);
            }

            var parentsCount = properties.Count(x => x.Value);
            if (parentsCount > 1)
            {
                context.MessageFormatter.AppendArgument("Message", "Выбрано больше одного раздела");
                return false;
            }

            if (parentsCount == 0)
            {
                context.MessageFormatter.AppendArgument("Message", "Должен быть выбран раздел");
                return false;
            }

            return true;
        }
    }
}