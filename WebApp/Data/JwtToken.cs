using Newtonsoft.Json;

namespace WebApp.Data
{
    public class JwtToken
    {
        // Access token for authentication
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }

        // Expiration date of the access token
        [JsonProperty("expires_at")]
        public DateTime ExpiresAt { get; set; }
    }
}

