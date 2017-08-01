using System.ComponentModel.DataAnnotations;
using BioEngine.Common.Search;
using Newtonsoft.Json;

namespace BioEngine.Common.Base
{
    public abstract class BaseModel
    {
        public abstract object GetId();
    }

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class BaseModel<TPkType> : BaseModel, ISearchModel
    {
        [Key]
        [Required]
        [JsonProperty("id")]
        public virtual TPkType Id { get; set; }

        public override object GetId()
        {
            return Id;
        }
    }
}