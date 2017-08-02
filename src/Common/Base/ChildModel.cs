using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
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

        public IParentModel Parent()
        {
            if (GameId > 0)
            {
                return Game;
            }
            if (DeveloperId > 0)
            {
                return Developer;
            }
            if (TopicId > 0)
            {
                return Topic;
            }
            throw new Exception("No parent!");
        }
    }
}