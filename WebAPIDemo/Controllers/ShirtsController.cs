using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using WebAPIDemo.Data;
using WebAPIDemo.Filters;
using WebAPIDemo.Filters.ActionFilters;
using WebAPIDemo.Filters.ExceptionFilters;
using WebAPIDemo.Models;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public IActionResult GetShirts()
        {
            // Retrieve all shirts from the database
            return Ok(db.Shirts.ToList());
        }

        // GET: api/Shirts/5
        [HttpGet("{id}")]
        [Shirt_ValidateShirtIdFilter] // Validate shirt ID
        public IActionResult GetShirtById(int id)
        {
            // Retrieve a specific shirt by ID from the repository
            return Ok(ShirtRepository.GetShirtById(id));
        }

        // POST: api/Shirts
        [HttpPost]
        [Shirt_ValidateCreateShirtFilter] // Validate input for creating a shirt
        public IActionResult CreateShirt([FromBody] Shirt shirt)
        {
            // Add a new shirt to the repository
            ShirtRepository.AddShirt(shirt);

            // Return the newly created shirt with a 201 Created status
            return CreatedAtAction(nameof(GetShirtById),
                new { id = shirt.ShirtId },
                shirt);
        }

        // PUT: api/Shirts/5
        [HttpPut("{id}")]
        [Shirt_ValidateShirtIdFilter] // Validate shirt ID
        [Shirt_ValidateUpdateShirtFilter] // Validate input for updating a shirt
        [Shirt_HandleUpdateExceptionsFilter] // Handle exceptions during shirt update
        public IActionResult UpdateShirt(int id, Shirt shirt)
        {
            // Update an existing shirt in the repository
            ShirtRepository.UpdateShirt(shirt);

            // Return 204 No Content status after successful update
            return NoContent();
        }

        // DELETE: api/Shirts/5
        [HttpDelete("{id}")]
        [Shirt_ValidateShirtIdFilter] // Validate shirt ID
        public IActionResult DeleteShirt(int id)
        {
            // Retrieve the shirt to be deleted
            var shirt = ShirtRepository.GetShirtById(id);

            // Delete the shirt from the repository
            ShirtRepository.DeleteShirt(id);

            // Return the deleted shirt with a 200 OK status
            return Ok(shirt);
        }
    }
}
