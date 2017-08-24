using System.Collections.Generic;
using BioEngine.API.Components.REST.Errors;

namespace BioEngine.API.Components.REST.Models
{
    public class RestResult
    {
        public RestResult(int code, IEnumerable<IErrorInterface> errors = null)
        {
            Code = code;
            if (errors != null)
            {
                Errors = errors;
            }
        }

        public int Code { get; }

        public IEnumerable<IErrorInterface> Errors { get; protected set; }
    }
}