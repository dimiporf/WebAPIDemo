using System.Text.Json;

namespace WebApp.Data
{
    // Class responsible for executing HTTP requests to a web API
    public class WebApiExecuter : IWebApiExecuter
    {
        // Name of the API endpoint
        private const string apiName = "ShirtsApi";

        // Factory for creating HttpClient instances
        private readonly IHttpClientFactory httpClientFactory;

        // Constructor to initialize the WebApiExecuter with an HttpClientFactory
        public WebApiExecuter(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        // Invokes an HTTP GET request to the specified relative URL of the API
        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            // Creates a new HttpClient instance using the HttpClientFactory
            var httpClient = httpClientFactory.CreateClient(apiName);

            // Creates a new HttpRequestMessage for the GET request
            var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);

            // Sends the HTTP GET request and waits for the response
            var response = await httpClient.SendAsync(request);

            // Handle potential errors in the response
            await HandlePotentialError(response);

            // If the response is successful, reads the response content and deserializes it to the specified type
            return await response.Content.ReadFromJsonAsync<T>();
        }


        // Invokes an HTTP POST request to the specified relative URL of the API with the provided object as content
        public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
        {
            // Creates a new HttpClient instance using the HttpClientFactory
            var httpClient = httpClientFactory.CreateClient(apiName);

            // Sends an HTTP POST request to the specified relative URL with the provided object as JSON content
            var response = await httpClient.PostAsJsonAsync(relativeUrl, obj);

            // Handle potential errors in the response
            await HandlePotentialError(response);

            // If the response is successful, read the response content and deserialize it to the specified type
            return await response.Content.ReadFromJsonAsync<T>();
        }


        // Invokes an HTTP PUT request to the specified relative URL of the API with the provided object as content
        public async Task InvokePut<T>(string relativeUrl, T obj)
        {
            // Creates a new HttpClient instance using the HttpClientFactory
            var httpClient = httpClientFactory.CreateClient(apiName);

            // Sends an HTTP PUT request to the specified relative URL with the provided object as JSON content
            var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);

            // Handle potential errors in the response
            await HandlePotentialError(response);
        }

        // Asynchronously sends an HTTP DELETE request to the specified relative URL of the API
        public async Task InvokeDelete(string relativeUrl)
        {
            // Creates a new HttpClient instance using the HttpClientFactory
            var httpClient = httpClientFactory.CreateClient(apiName);

            // Sends an HTTP DELETE request to the specified relative URL
            var response = await httpClient.DeleteAsync(relativeUrl);

            // Handle potential errors in the response
            await HandlePotentialError(response);
        }

        private async Task HandlePotentialError(HttpResponseMessage httpResponse)
        {
            if(!httpResponse.IsSuccessStatusCode)
            {
                // If the response indicates an error, read the error JSON from the response content
                var errorJson = await httpResponse.Content.ReadAsStringAsync();

                // Throw a WebApiException with the error JSON
                throw new WebApiException(errorJson);
            }
        }

    }
}
