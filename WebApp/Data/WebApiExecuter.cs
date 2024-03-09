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

            // Sends an HTTP GET request to the specified relative URL and deserializes the response
            return await httpClient.GetFromJsonAsync<T>(relativeUrl);
        }

        // Invokes an HTTP POST request to the specified relative URL of the API with the provided object as content
        public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
        {
            // Creates a new HttpClient instance using the HttpClientFactory
            var httpClient = httpClientFactory.CreateClient(apiName);

            // Sends an HTTP POST request to the specified relative URL with the provided object as JSON content
            var response = await httpClient.PostAsJsonAsync(relativeUrl, obj);

            // Ensures that the HTTP response indicates success (status code 2xx)
            response.EnsureSuccessStatusCode();

            // Reads the response content and deserializes it to the specified type
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
