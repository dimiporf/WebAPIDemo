using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Drawing;
using System.Reflection;
using WebAPIDemo.Data;
using WebAPIDemo.Models;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Filters.ActionFilters
{
    // Action filter attribute to validate input for creating a new shirt
    public class Shirt_ValidateCreateShirtFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        public Shirt_ValidateCreateShirtFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }
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
                var existingShirt = db.Shirts.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(shirt.Brand) &&
                !string.IsNullOrWhiteSpace(x.Brand) &&
                x.Brand.ToLower() == shirt.Brand.ToLower() &&
                !string.IsNullOrWhiteSpace(shirt.Gender) &&
                !string.IsNullOrWhiteSpace(x.Gender) &&
                x.Gender.ToLower() == shirt.Gender.ToLower() &&
                !string.IsNullOrWhiteSpace(shirt.Color) &&
                !string.IsNullOrWhiteSpace(x.Color) &&
                x.Color.ToLower() == shirt.Color.ToLower() &&
                shirt.Size.HasValue &&
                x.Size.HasValue &&
                shirt.Size.Value == x.Size.Value);
          
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
