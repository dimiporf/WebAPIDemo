using Microsoft.AspNetCore.Mvc;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShirtsController: ControllerBase
    {
        private List<Shirt> shirts = new List<Shirt>() 
        {
            new Shirt { ShirtId = 1, Brand = "My Brand", Color = "Blue", Gender = "Men", Price = 30, Size = 10},
            new Shirt { ShirtId = 2, Brand = "My Brand1", Color = "Black", Gender = "Men", Price = 35, Size = 12},
            new Shirt { ShirtId = 3, Brand = "My Brand2", Color = "Pink", Gender = "Women", Price = 20, Size = 8},
            new Shirt { ShirtId = 4, Brand = "My Brand3", Color = "Yellow", Gender = "Women", Price = 28, Size = 9}

        };

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
