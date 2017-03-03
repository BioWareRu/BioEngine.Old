using System;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using JetBrains.Annotations;

namespace BioEngine.Common.Base
{
    public abstract class ParentModel : BaseModel<int>, IParentModel
    {
        [UsedImplicitly]
        public virtual ParentType Type { get; }

        [UsedImplicitly]
        public virtual string Icon { get; }

        [UsedImplicitly]
        public virtual string ParentUrl { get; }

        [UsedImplicitly]
        public virtual string DisplayTitle { get; }

        public static void SetParent(IChildModel child, IParentModel parent)
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