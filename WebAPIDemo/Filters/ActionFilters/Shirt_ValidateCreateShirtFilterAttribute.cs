using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIDemo.Models;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Filters.ActionFilters
{
    // Action filter attribute to validate input for creating a new shirt
    public class Shirt_ValidateCreateShirtFilterAttribute : ActionFilterAttribute
    {
        // Override the OnActionExecuting method to perform validation before the action method is executed
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Call the base method first
            base.OnActionExecuting(context);

            // Retrieve the shirt object from the action arguments
            var shirt = context.ActionArguments["shirt"] as Shirt;

            // Check if the shirt object is null
            if (shirt == null)
            {
                // Add model error indicating that the shirt object is null
                context.ModelState.AddModelError("Shirt", "Shirt object is null.");

                // Create a validation problem details with 400 Bad Request status
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                // Set the result to BadRequestObjectResult with problem details
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                // Check if a shirt with the same properties already exists
                var existingShirt = ShirtRepository.GetShirtByProperties(shirt.Brand, shirt.Gender, shirt.Color, shirt.Size);

                if (existingShirt != null)
                {
                    // Add model error indicating that the shirt already exists
                    context.ModelState.AddModelError("Shirt", "Shirt already exists.");

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
}
