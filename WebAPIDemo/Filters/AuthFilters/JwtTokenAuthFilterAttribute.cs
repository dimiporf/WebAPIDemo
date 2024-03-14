using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIDemo.Attributes;
using WebAPIDemo.Authority;

namespace WebAPIDemo.Filters.AuthFilters
{
    
    // Represents an attribute used for JWT token-based authorization.    
    public class JwtTokenAuthFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
        // Executes the authorization filter asynchronously.        
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Check if the Authorization header is present in the HTTP request
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
            {
                // If the Authorization header is not present, return an UnauthorizedResult
                context.Result = new UnauthorizedResult();
                return;
            }

            // Retrieve the IConfiguration service from the request services
            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();

            // Verify the JWT token and retrieve its claims
            var claims = Authenticator.VerifyToken(token, configuration.GetValue<string>("SecretKey"));

            // If the claims are null, return UnauthorizedResult (401)
            if (claims == null)
            {
                context.Result = new UnauthorizedResult();
            }
            else
            {
                // Retrieve the required claims specified by the action method or controller
                var requiredClaims = context.ActionDescriptor.EndpointMetadata
                    .OfType<RequiredClaimAttribute>()
                    .ToList();

                // If required claims are specified and not all of them are present in the token, return StatusCodeResult (403)
                if (requiredClaims != null &&
                    !requiredClaims.All(rc => claims.Any(c => c.Type.ToLower() == rc.ClaimType.ToLower() &&
                                                               c.Value.ToLower() == rc.ClaimValue.ToLower())))
                {
                    // If any required claim is missing, return StatusCodeResult (403)
                    context.Result = new StatusCodeResult(403);
                }
            }
        }
    }


}

