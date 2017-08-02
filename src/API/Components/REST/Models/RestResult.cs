using System.Collections.Generic;

namespace BioEngine.API.Components.REST.Models
{
    public abstract class RestResult
    {
        public RestResult(int code)
        {
            Code = code;
        }

        public int Code { get; }

        public IEnumerable<IErrorInterface> Errors { get; protected set; }
    }

    public interface IErrorInterface
    {
        string Message { get; }
    }
}