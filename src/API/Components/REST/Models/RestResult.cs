using System.Collections.Generic;

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

    public interface IErrorInterface
    {
        string Message { get; }
    }

    public class RestError : IErrorInterface
    {
        public RestError(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}