using System.Net;

namespace BioEngine.API.Components.REST.Errors
{
    public abstract class RestError
    {
        public HttpStatusCode Code { get; }

        protected RestError(HttpStatusCode code)
        {
            Code = code;
        }
    }
}