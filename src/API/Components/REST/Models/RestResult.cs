using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BioEngine.API.Components.REST.Models
{
    public abstract class RestResult
    {
        public int Code { get; }

        public IEnumerable<ErrorInterface> Errors { get; protected set; }

        public RestResult(int code)
        {
            Code = code;
        }
    }

    public interface ErrorInterface
    {
        string Message { get; }
    }
}
