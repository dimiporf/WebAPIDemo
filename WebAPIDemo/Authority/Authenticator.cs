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

        public static bool VerifyToken(string token, string strSecretKey)
        {
            // Check if the token is null or empty
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            //Remove Bearer intro from Json token
            if (token.StartsWith("Bearer"))
            {
                token = token.Substring(6).Trim();
            }

            // Get the secret key as bytes
            var secretKey = Encoding.ASCII.GetBytes(strSecretKey);

            SecurityToken securityToken;

            try
            {
                // Create a new instance of JwtSecurityTokenHandler
                var tokenHandler = new JwtSecurityTokenHandler();

                // Validate the token using the provided token validation parameters
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    // Specify that the issuer signing key must be validated
                    ValidateIssuerSigningKey = true,

                    // Set the issuer signing key to the provided symmetric key
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                    // Specify that the token's lifetime must be validated
                    ValidateLifetime = true,

                    // Specify that the audience does not need to be validated
                    ValidateAudience = false,

                    // Specify that the issuer does not need to be validated
                    ValidateIssuer = false,

                    // Set the clock skew to zero to ensure the token is not expired
                    ClockSkew = TimeSpan.Zero
                },
                out securityToken);
            }
            catch (SecurityTokenException)
            {
                // If there's a SecurityTokenException, return false
                return false;
            }
            catch
            {
                // If there's any other exception, rethrow it
                throw;
            }

            // Return true if the security token is not null
            return securityToken != null;
        }

    }
}
