using Microsoft.AspNetCore.Authentication;

namespace BioEngine.API.Auth
{
    public class TokenAuthOptions : AuthenticationSchemeOptions
    {
        public string ClientId { get; set; }
        public bool DevMode { get; set; }
        public string UserInformationEndpointUrl { get; set; }
    }
}