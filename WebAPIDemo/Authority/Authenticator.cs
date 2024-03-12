using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace WebAPIDemo.Authority
{
    public static class Authenticator
    {
        // Method to authenticate an application based on provided clientId and secret
        public static bool Authenticate(string clientId, string secret)
        {
            // Retrieve application details based on client ID
            var app = AppRepository.GetApplicationByClientId(clientId);
            if (app == null)
            {
                return false;
            }

            // Check if provided credentials match the application's clientId and secret
            return (app.ClientId == clientId && app.Secret == secret);
        }

        // Helper method to create a token for the authenticated application
        public static string CreateToken(string clientId, DateTime expiresAt, string strSecretKey)
        {
            // Retrieve application details based on client ID
            var app = AppRepository.GetApplicationByClientId(clientId);

            // Define claims for the token payload
            var claims = new List<Claim>
            {
                new Claim("AppName", app?.ApplicationName ?? string.Empty),
                new Claim("Read", (app?.Scopes ?? string.Empty).Contains("read") ? "true" : "false"),
                new Claim("Write", (app?.Scopes ?? string.Empty).Contains("write") ? "true" : "false")
            };

            // Get the secret key from configuration
            var secretKey = Encoding.ASCII.GetBytes(strSecretKey);

            // Create the JWT token with claims, expiration, and signing credentials
            var jwt = new JwtSecurityToken(
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature),
                claims: claims,
                expires: expiresAt,
                notBefore: DateTime.UtcNow
            );

            // Write the token as a string
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
