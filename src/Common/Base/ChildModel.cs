using System;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using Newtonsoft.Json;

namespace BioEngine.Common.Base
{
    public class ChildModel<TPkType> : BaseModel<TPkType>, IChildModel
    {
        [JsonProperty]
        public virtual int? GameId { get; set; }

        [JsonProperty]
        public virtual int? DeveloperId { get; set; }

        [JsonProperty]
        public virtual int? TopicId { get; set; }

        [ForeignKey(nameof(GameId))]
        public virtual Game Game { get; set; }

        [ForeignKey(nameof(DeveloperId))]
        public virtual Developer Developer { get; set; }

        [ForeignKey(nameof(TopicId))]
        public virtual Topic Topic { get; set; }

        public IParentModel Parent
        {
            get
            {
                if (GameId > 0 && Game != null)
                {
                    return Game;
                }
                if (DeveloperId > 0 && Developer != null)
                {
                    return Developer;
                }
                if (TopicId > 0 && Topic != null)
                {
                    return Topic;
                }
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