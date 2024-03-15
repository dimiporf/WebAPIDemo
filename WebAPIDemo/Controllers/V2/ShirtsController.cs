using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using WebAPIDemo.Attributes;
using WebAPIDemo.Data;
using WebAPIDemo.Filters;
using WebAPIDemo.Filters.ActionFilters;
using WebAPIDemo.Filters.ActionFilters.V2;
using WebAPIDemo.Filters.AuthFilters;
using WebAPIDemo.Filters.ExceptionFilters;
using WebAPIDemo.Models;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Controllers.V2
{
    [ApiController]
    [Route("api/[controller]")]
    [JwtTokenAuthFilter]
    public class ShirtsController : ControllerBase
    {
        // Define a private field to hold an instance of the ApplicationDbContext.
        private readonly ApplicationDbContext db;

        // Constructor for the ShirtsController class, which injects an instance of ApplicationDbContext.
        public ShirtsController(ApplicationDbContext db)
        {
            // Initialize the ApplicationDbContext field with the injected instance.
            this.db = db;
        }


        // GET: api/Shirts
        [HttpGet]
        [RequiredClaim("read","true")]
        public IActionResult GetShirts()
        {
            // Retrieve all shirts from the database
            return Ok(db.Shirts.ToList());
        }

        // GET: api/Shirts/5
        [HttpGet("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))] // Validate shirt ID
        public IActionResult GetShirtById(int id)
        {
            // Retrieve a specific shirt by ID from the database (after filtering)
            return Ok(HttpContext.Items["shirt"]);
        }

        // POST: api/Shirts
        [HttpPost]
        [TypeFilter(typeof(Shirt_ValidateCreateShirtFilterAttribute))] // Validate input for creating a shirt
        [RequiredClaim("write", "true")]
        [Shirt_EnsureDescriptionIsPresentFilter]
        public IActionResult CreateShirt([FromBody] Shirt shirt)
        {
            // Add a new shirt to the database
            this.db.Shirts.Add(shirt);
            this.db.SaveChanges();

            // Return the newly created shirt with a 201 Created status
            return CreatedAtAction(nameof(GetShirtById),
                new { id = shirt.ShirtId },
                shirt);
        }

        // PUT: api/Shirts/5
        [HttpPut("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))] // Validate shirt ID
        [Shirt_ValidateUpdateShirtFilter] // Validate input for updating a shirt
        [TypeFilter(typeof(Shirt_HandleUpdateExceptionsFilterAttribute))] //Validate exceptions during shirt update
        [RequiredClaim("write", "true")]
        [Shirt_EnsureDescriptionIsPresentFilter]
        public IActionResult UpdateShirt(int id, Shirt shirt)
        {
            // Update an existing shirt in the repository
            var shirtToUpdate = HttpContext.Items["shirt"] as Shirt;

            shirtToUpdate.Brand = shirt.Brand;
            shirtToUpdate.Price = shirt.Price;
            shirtToUpdate.Size = shirt.Size;
            shirtToUpdate.Color = shirt.Color;
            shirtToUpdate.Gender = shirt.Gender;

            db.SaveChanges();

            // Return 204 No Content status after successful update
            return NoContent();
        }

        // DELETE: api/Shirts/5
        [HttpDelete("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))] // Validate shirt ID
        [RequiredClaim("delete", "true")]
        public IActionResult DeleteShirt(int id)
        {
            // Retrieve the shirt to be deleted
            var shirtToDelete = HttpContext.Items["shirt"] as Shirt;

            // Delete the shirt from the database
            db.Shirts.Remove(shirtToDelete);
            db.SaveChanges();

            // Return the deleted shirt with a 200 OK status
            return Ok(shirtToDelete);
        }
    }
}
