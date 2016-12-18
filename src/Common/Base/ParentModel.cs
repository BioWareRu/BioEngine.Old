using System;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using JetBrains.Annotations;

namespace BioEngine.Common.Base
{
    public abstract class ParentModel : BaseModel
    {
        public abstract int Id { get; set; }

        [UsedImplicitly]
        public virtual ParentType Type { get; }

        [UsedImplicitly]
        public virtual string Icon { get; }

        [UsedImplicitly]
        public virtual string ParentUrl { get; }

        [UsedImplicitly]
        public virtual string DisplayTitle { get; }

        public static ParentModel GetParent(IChildModel child)
        {
            if (child.GameId > 0)
                return child.Game;
            if (child.DeveloperId > 0)
                return child.Developer;
            if (child.TopicId > 0)
                return child.Topic;

            throw new Exception("No parent!");
        }

        public static void SetParent(IChildModel child, ParentModel parent)
        {
            switch (parent.Type)
            {
                case ParentType.Game:
                    child.Game = (Game) parent;
                    break;
                case ParentType.Developer:
                    child.Developer = (Developer) parent;
                    break;
                case ParentType.Topic:
                    child.Topic = (Topic) parent;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum ParentType
    {
        Game = 1,
        Developer = 2,
        Topic = 3
    }
}