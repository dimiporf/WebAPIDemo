using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WebAPIDemo.Authority;

namespace WebAPIDemo.Controllers
{
    [ApiController]
    public class AuthorityController : ControllerBase
    {
        // POST: /auth
        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] AppCredential credential)
        {
            // Authenticate the application using provided credentials
            if (AppRepository.Authenticate(credential.ClientId, credential.Secret))
            {
                // If authentication is successful, return an access token with expiration time
                return Ok(new
                {
                    access_token = CreateToken(credential.ClientId),
                    expires_at = DateTime.UtcNow.AddMinutes(10)
                });
            }
            else
            {
                // If authentication fails, return unauthorized status with error details
                ModelState.AddModelError("Unauthorized", "You are not authorized.");

                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status401Unauthorized
                };
                return new UnauthorizedObjectResult(problemDetails);
            }
        }

        // Helper method to create a token for the authenticated application
        private string CreateToken(string clientId)
        {
            // Implement token generation logic here
            return string.Empty;
        }
    }
}
