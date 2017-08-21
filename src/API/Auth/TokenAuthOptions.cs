using Microsoft.AspNetCore.Builder;

namespace BioEngine.API.Auth
{
    public class TokenAuthOptions : AuthenticationOptions
    {
        public string ClientId { get; set; }
        public bool DevMode { get; set; }
        public string UserInformationEndpointUrl { get; set; }
    }
}