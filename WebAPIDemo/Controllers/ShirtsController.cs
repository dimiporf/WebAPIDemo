using Microsoft.AspNetCore.Mvc;

namespace WebAPIDemo.Controllers
{
    [ApiController]
    public class ShirtsController: ControllerBase
    {
        [HttpGet]
        [Route("/shirts")]
        public string GetShirts()
        {
            return "Reading all the shirts";
        }

        [HttpGet]
        [Route("/shirts/{id}")]
        public string GetShirtById(int id)
        {
            return $"Reading shirt with ID: {id}";
        }

        [HttpPost]
        [Route("/shirts")]
        public string CreateShirt()
        {
            return $"Creating a shirt.";
        }
        [HttpPut]
        [Route("/shirts/{id}")]
        public string UpdateShirt(int id)
        {
            return $"Updating a shirt with ID: {id}";
        }
        [HttpDelete]
        [Route("/shirts/{id}")]
        public string DeleteShirt(int id)
        {
            return $"Deleting a shirt with ID: {id}";
        }

    }
}
