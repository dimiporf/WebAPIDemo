using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIDemo.Authority;

namespace WebAPIDemo.Filters.AuthFilters
{
    public class JwtTokenAuthFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
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

            // Verify the JWT token using the Authenticator class and the secret key from configuration
            if (!Authenticator.VerifyToken(token, configuration.GetValue<string>("SecretKey")))
            {
                // If the token verification fails, return an UnauthorizedResult
                context.Result = new UnauthorizedResult();
            }
        }
    }

}

