using Newtonsoft.Json;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace WebApp.Data
{
    // Class responsible for executing HTTP requests to a web API
    public class WebApiExecuter : IWebApiExecuter
    {
        // Name of the API endpoint
        private const string apiName = "ShirtsApi";

        // Name of the Authority API
        private const string authApiName = "AuthorityApi";

        // Factory for creating HttpClient instances
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        // Constructor to initialize the WebApiExecuter with an HttpClientFactory
        public WebApiExecuter(IHttpClientFactory httpClientFactory,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
        }

        // Invokes an HTTP GET request to the specified relative URL of the API
        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            // Creates a new HttpClient instance using the HttpClientFactory
            var httpClient = httpClientFactory.CreateClient(apiName);

            // Add JWT to the header of the HttpClient for authentication
            await AddJwtToHeader(httpClient);

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

            // Add JWT to the header of the HttpClient for authentication
            await AddJwtToHeader(httpClient);

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

            // Add JWT to the header of the HttpClient for authentication
            await AddJwtToHeader(httpClient);

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

            // Add JWT to the header of the HttpClient for authentication
            await AddJwtToHeader(httpClient);

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

        private async Task AddJwtToHeader(HttpClient httpClient)
        {
            // Check if the access token is already stored in the session
            JwtToken? token = null;
            string? strToken = httpContextAccessor.HttpContext?.Session.GetString("access_token");
            if (!string.IsNullOrWhiteSpace(strToken))
            {
                // Deserialize the token from the stored string
                token = JsonConvert.DeserializeObject<JwtToken>(strToken);
            }

            // If the token is null or expired, authenticate and obtain a new token
            if (token == null || token.ExpiresAt <= DateTime.UtcNow)
            {
                var clientId = configuration.GetValue<string>("ClientId");
                var secret = configuration.GetValue<string>("Secret");

                // Authenticate 
                var authoClient = httpClientFactory.CreateClient(authApiName);
                var response = await authoClient.PostAsJsonAsync("auth", new AppCredential
                {
                    ClientId = clientId,
                    Secret = secret
                });
                response.EnsureSuccessStatusCode();

                // Get the JWT 
                strToken = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<JwtToken>(strToken);

                // Store the new token in the session
                httpContextAccessor.HttpContext?.Session.SetString("access_token", strToken);
            }

            // Pass the JWT to endpoints through the http headers
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
        }


    }
}
