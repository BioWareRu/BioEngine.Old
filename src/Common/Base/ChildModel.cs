using System;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Models;

namespace BioEngine.Common.Base
{
    public class ChildModel : BaseModel
    {
        public virtual int? GameId { get; set; }
        public virtual int? DeveloperId { get; set; }
        [NotMapped]
        public virtual int? TopicId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; }

        [ForeignKey(nameof(DeveloperId))]
        public Developer Developer { get; set; }


        [ForeignKey(nameof(TopicId))]
        [NotMapped]
        public Topic Topic { get; set; }

        [NotMapped]
        public virtual ParentModel Parent
        {
            get
            {
                if (GameId > 0)
                    return Game;
                if (DeveloperId > 0)
                    return Developer;
                if (TopicId > 0)
                    return Topic;

                throw new Exception("No parent!");
            }
            set
            {
                switch (value.Type)
                {
                    case ParentType.Game:
                        Game = (Game) value;
                        break;
                    case ParentType.Developer:
                        Developer = (Developer) value;
                        break;
                    case ParentType.Topic:
                        Topic = (Topic) value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}