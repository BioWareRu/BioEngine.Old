using System.Collections.Generic;
using BioEngine.Common.Base;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BioEngine.API.Components.REST.Models
{
    public class ListResult<T> : RestResult where T : BaseModel
    {
        public ListResult(IEnumerable<T> data, int totalitem) : base(StatusCodes.Status200OK)
        {
            Data = data;
            TotalItems = totalitem;
        }

        [JsonProperty]
        public IEnumerable<T> Data { get; }

        [JsonProperty]
        public int TotalItems { get; }
    }
}