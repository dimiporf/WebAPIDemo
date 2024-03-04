using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIDemo.Data;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Filters.ActionFilters
{
    // Action filter attribute to validate the shirt ID
    public class Shirt_ValidateShirtIdFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        public Shirt_ValidateShirtIdFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }
        // Override the OnActionExecuting method to perform validation before the action method is executed
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Call the base method first
            base.OnActionExecuting(context);

            // Retrieve the shirt ID from the action arguments
            var shirtId = context.ActionArguments["id"] as int?;

            // Check if the shirt ID has a value
            if (shirtId.HasValue)
            {
                // Check if the shirt ID is invalid (less than or equal to 0)
                if (shirtId.Value <= 0)
                {
                    // Add model error indicating that the shirt ID is invalid
                    context.ModelState.AddModelError("ShirtId", "ShirtId is invalid.");

                    // Create a validation problem details with 400 Bad Request status
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };

                    // Set the result to BadRequestObjectResult with problem details
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                // Check if the shirt with the given ID does not exist
                else 
                {
                    var shirt = db.Shirts.Find(shirtId.Value);

                    if (shirt == null)
                    {
                        // Add model error indicating that the shirt does not exist
                        context.ModelState.AddModelError("ShirtId", "Shirt does not exist.");

                        // Create a validation problem details with 404 Not Found status
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };

                        // Set the result to NotFoundObjectResult with problem details
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        // Make the shirt object available after filtering
                        context.HttpContext.Items["shirt"] = shirt;
                    }
                   
                }
            }
        }
    }
}
