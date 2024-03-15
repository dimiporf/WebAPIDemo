using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIDemo.Models;

namespace WebAPIDemo.Filters.ActionFilters.V2
{
    // Action filter attribute to ensure that the description is present in a shirt object.
    public class Shirt_EnsureDescriptionIsPresentFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Retrieve the shirt object from the action arguments
            var shirt = context.ActionArguments["shirt"] as Shirt;
            
            if (shirt != null && !shirt.ValidateDescription())
            {
                // Add a model error indicating that the description is required
                context.ModelState.AddModelError("Shirt", "Description is required.");

                // Create a validation problem details with 400 Bad Request status
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                // Set the result to BadRequestObjectResult with problem details
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}
