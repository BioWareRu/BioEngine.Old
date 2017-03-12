using Microsoft.AspNetCore.Builder;

namespace BioEngine.API.Auth
{
    public class TokenAuthOptions : AuthenticationOptions
    {
        public string ClientId { get; set; }
    }
}