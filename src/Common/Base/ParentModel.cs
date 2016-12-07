using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;

namespace BioEngine.Common.Base
{
    public abstract class ParentModel : BaseModel
    {
        public abstract int Id { get; set; }

        public virtual ParentType Type { get; }

        public virtual string NewsUrl { get; }

        public virtual string Icon { get; }
        public virtual string ParentUrl { get; }

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