using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    public class ShirtsController : Controller
    {
        private readonly IWebApiExecuter webApiExecuter;

        public ShirtsController(IWebApiExecuter webApiExecuter)
        {
            this.webApiExecuter = webApiExecuter;
        }

        // GET: Shirts
        public async Task<IActionResult> Index()
        {
            // Retrieves a list of shirts from the API and returns them to the view
            return View(await webApiExecuter.InvokeGet<List<Shirt>>("shirts"));
        }

        // GET: Shirts/Create
        public IActionResult CreateShirt()
        {
            // Returns the view for creating a new shirt
            return View();
        }

        // POST: Shirts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShirt(Shirt shirt)
        {
            // Checks if the model state is valid
            if (ModelState.IsValid)
            {
                // Invokes the API to create a new shirt
                var response = await webApiExecuter.InvokePost("shirts", shirt);
                if (response != null)
                {
                    // Redirects to the index action if the shirt creation is successful
                    return RedirectToAction("Index");
                }
            }

            // If model state is not valid or shirt creation fails, returns to the create shirt view
            return View(shirt);
        }

        public async Task<IActionResult> UpdateShirt(int shirtId)
        {
            var shirt = await webApiExecuter.InvokeGet<Shirt>($"shirts/{shirtId}");

            if (shirt != null)
            {
                return View(shirt);
            }
            return NotFound();
        }
    }
}
