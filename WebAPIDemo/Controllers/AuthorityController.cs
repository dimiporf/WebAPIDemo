using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebAPIDemo.Authority;

namespace WebAPIDemo.Controllers
{
    [ApiController]
    public class AuthorityController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AuthorityController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // POST: /auth
        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] AppCredential credential)
        {
            // Authenticate the application using provided credentials
            if (Authenticator.Authenticate(credential.ClientId, credential.Secret))
            {
                var expiresAt = DateTime.UtcNow.AddMinutes(10);

                // If authentication is successful, return an access token with expiration time
                return Ok(new
                {
                    access_token = Authenticator.CreateToken(credential.ClientId, expiresAt, configuration.GetValue<string>("SecretKey")),
                    expires_at = expiresAt
                });
            }
            else
            {
                // If authentication fails, add an error to the ModelState indicating unauthorized access
                ModelState.AddModelError("Unauthorized", "You are not authorized.");

                // Create a ValidationProblemDetails object with the unauthorized status code
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status401Unauthorized
                };

                // Return an UnauthorizedObjectResult with the problem details
                return new UnauthorizedObjectResult(problemDetails);
            }

        }
    }
}
