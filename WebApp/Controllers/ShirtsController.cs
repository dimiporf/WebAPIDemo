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

        // Retrieves the details of a shirt with the specified ID from the API and displays the details in a view for updating
        public async Task<IActionResult> UpdateShirt(int shirtId)
        {
            // Invoke the API to get the details of the shirt with the specified ID
            var shirt = await webApiExecuter.InvokeGet<Shirt>($"shirts/{shirtId}");

            // If the shirt details are found, display them in the view for updating
            if (shirt != null)
            {
                return View(shirt);
            }

            // If the shirt with the specified ID is not found, return a 404 Not Found response
            return NotFound();
        }

        // Handles the POST request to update the shirt details
        [HttpPost]
        public async Task<IActionResult> UpdateShirt(Shirt shirt)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Invoke the API to update the shirt details
                await webApiExecuter.InvokePut($"shirts/{shirt.ShirtId}", shirt);

                // Redirect to the index action after successful update
                return RedirectToAction(nameof(Index));
            }

            // If the model state is not valid, return the view with the shirt model to display validation errors
            return View(shirt);
        }

        // Asynchronously deletes a shirt with the specified ID from the API and redirects to the index action
        public async Task<IActionResult> DeleteShirt(int shirtId)
        {
            // Invoke the API to delete the shirt with the specified ID
            await webApiExecuter.InvokeDelete($"shirts/{shirtId}");

            // Redirect to the index action after successful deletion
            return RedirectToAction(nameof(Index));
        }

    }
}
