using System;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Models;

namespace BioEngine.Common.Base
{
    public class ChildModel : BaseModel
    {
        public virtual int? GameId { get; set; }
        public virtual int? DeveloperId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; }

        [ForeignKey(nameof(DeveloperId))]
        public Developer Developer { get; set; }

        [NotMapped]
        public virtual ParentModel Parent
        {
            get
            {
                if (GameId > 0)
                    return Game;
                if (DeveloperId > 0)
                    return Developer;

                throw new Exception("No parent!");
            }
            set
            {
                switch (value.Type)
                {
                    case ParentType.Game:
                        Game = (Game)value;
                        break;
                    case ParentType.Developer:
                        Developer = (Developer)value;
                        break;
                    case ParentType.Topic:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}