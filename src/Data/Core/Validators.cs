using System;
using System.Collections.Generic;
using System.Linq;
using BioEngine.Common.Interfaces;
using FluentValidation.Validators;
using JetBrains.Annotations;

namespace BioEngine.Data.Core
{
    [UsedImplicitly]
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

    [UsedImplicitly]
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
            var model = context.Instance as IChildModelCommand;
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