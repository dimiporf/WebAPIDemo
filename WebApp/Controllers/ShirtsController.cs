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


        // Handles HTTP POST requests to create a new shirt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShirt(Shirt shirt)
        {
            // Checks if the model state is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Invokes the API to create a new shirt
                    var response = await webApiExecuter.InvokePost("shirts", shirt);

                    // Checks if the response is not null, indicating successful creation
                    if (response != null)
                    {
                        // Redirects to the index action if the shirt creation is successful
                        return RedirectToAction("Index");
                    }
                }
                // Catches a WebApiException if an error occurs during API invocation
                catch (WebApiException ex)
                {
                    HandleWebApiException(ex);
                }
            }

            // If model state is not valid or shirt creation fails, returns to the create shirt view
            return View(shirt);
        }


        // Retrieves the details of a shirt with the specified ID from the API and displays the details in a view for updating
        public async Task<IActionResult> UpdateShirt(int shirtId)
        {
            try
            {
                // Invoke the API to get the details of the shirt with the specified ID
                var shirt = await webApiExecuter.InvokeGet<Shirt>($"shirts/{shirtId}");

                // If the shirt details are found, display them in the view for updating
                if (shirt != null)
                {
                    return View(shirt);
                }
            }
            catch(WebApiException ex)
            {
                HandleWebApiException(ex);
                return View();
            }

            // If the shirt with the specified ID is not found, return a 404 Not Found response
            return NotFound();
        }


        // Handles the POST request to update the shirt details
        [HttpPost]
        public async Task<IActionResult> UpdateShirt(Shirt shirt)
        {
            try
            {
                // Check if the model state is valid
                if (ModelState.IsValid)
                {
                    // Invoke the API to update the shirt details
                    await webApiExecuter.InvokePut($"shirts/{shirt.ShirtId}", shirt);

                    // Redirect to the index action after successful update
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (WebApiException ex)
            {
                HandleWebApiException (ex);
            }            

            // If the model state is not valid, return the view with the shirt model to display validation errors
            return View(shirt);
        }


        // Asynchronously deletes a shirt with the specified ID from the API and redirects to the index action
        public async Task<IActionResult> DeleteShirt(int shirtId)
        {
            try
            {
                // Invoke the API to delete the shirt with the specified ID
                await webApiExecuter.InvokeDelete($"shirts/{shirtId}");

                // Redirect to the index action after successful deletion
                return RedirectToAction(nameof(Index));
            }
            // Catches a WebApiException if an error occurs during API invocation
            catch (WebApiException ex)
            {
                // Handle the WebApiException
                HandleWebApiException(ex);

                // Return to the index view, displaying the updated list of shirts
                return View(nameof(Index),
                    await webApiExecuter.InvokeGet<List<Shirt>>("shirts"));
            }
        }



        private void HandleWebApiException(WebApiException ex)
        {
            // Checks if the exception contains error response details
            if (ex.ErrorResponse != null &&
                ex.ErrorResponse.Errors != null &&
                ex.ErrorResponse.Errors.Count > 0)
            {
                // Adds model errors based on the error response received from the API
                foreach (var error in ex.ErrorResponse.Errors)
                {
                    ModelState.AddModelError(error.Key, string.Join("; ", error.Value));
                }
            }
            // Check if the error response is not null
            else if (ex.ErrorResponse != null)
            {
                // Add model error with the error response title
                ModelState.AddModelError("Error", ex.ErrorResponse.Title);
            }
            else
            {
                // Add model error with the exception message
                ModelState.AddModelError("Error", ex.Message);
            }
        }
    }
}
