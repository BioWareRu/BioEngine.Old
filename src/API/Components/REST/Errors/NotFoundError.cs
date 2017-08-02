using System.Net;

namespace BioEngine.API.Components.REST.Errors
{
    public class NotFoundError : RestError
    {
        public NotFoundError(string message = "Not found") : base(HttpStatusCode.NotFound)
        {
            Message = message;
        }

        public string Message { get; }
    }
}