using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BioEngine.API.Components.REST.Errors
{
    public class NotFoundError : RestError
    {
        public string Message { get; }

        public NotFoundError(string message = "Not found") : base(HttpStatusCode.NotFound)
        {
            Message = message;
        }
    }
}