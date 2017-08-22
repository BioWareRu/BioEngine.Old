using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BioEngine.Common.Base
{
    public interface IBaseModel
    {
        object GetId();
    }

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class BaseModel<TPkType> : IBaseModel
    {
        [Key]
        [Required]
        [JsonProperty("id")]
        public virtual TPkType Id { get; set; }

        public object GetId()
        {
            return Id;
        }

        [NotMapped]
        [JsonProperty("publicUrl")]
        public Uri PublicUrl { get; set; }
    }
}