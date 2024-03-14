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
               
            };

            // Split the scopes string into an array of individual scope strings
            var scopes = app?.Scopes?.Split(",");

            // Check if the scopes array is not null and contains elements
            if (scopes != null && scopes.Length > 0)
            {
                // Iterate over each scope in the scopes array
                foreach (var scope in scopes)
                {
                    // Add a claim for each scope to the claims list, converting the scope to lowercase
                    claims.Add(new Claim(scope.ToLower(), "true"));
                }
            }


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

        public static IEnumerable<Claim>? VerifyToken(string token, string strSecretKey)
        {
            // Check if the token is null or empty
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
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

                // Check if the security token is not null
                if (securityToken != null)
                {
                    // Read the claims from the JWT token
                    var tokenObject = tokenHandler.ReadJwtToken(token);
                    return tokenObject.Claims ?? (new List<Claim>());
                }
                else
                {
                    return null;
                }
            }
            catch (SecurityTokenException)
            {
                // If there's a SecurityTokenException, return false
                return null;
            }
            catch
            {
                // If there's any other exception, rethrow it
                throw;
            }
        }

    }
}
