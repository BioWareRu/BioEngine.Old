namespace BioEngine.API.Components.REST.Errors
{
    public class RestError : IErrorInterface
    {
        public RestError(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}