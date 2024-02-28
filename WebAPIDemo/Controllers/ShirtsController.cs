using Microsoft.AspNetCore.Mvc;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShirtsController: ControllerBase
    {
        [HttpGet]        
        public string GetShirts()
        {
            return "Reading all the shirts";
        }

        [HttpGet("{id}/{color}")]
        public string GetShirtById(int id, string color)
        {
            return $"Reading shirt with ID: {id}, color: {color}";
        }

        [HttpPost]
        public string CreateShirt([FromForm]Shirt shirt)
        {
            return $"Creating a shirt.";
        }
        [HttpPut("{id}")]        
        public string UpdateShirt(int id)
        {
            return $"Updating a shirt with ID: {id}";
        }
        [HttpDelete("{id}")]
        public string DeleteShirt(int id)
        {
            return $"Deleting a shirt with ID: {id}";
        }

    }
}
