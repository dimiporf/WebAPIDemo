using System.Text.Json;

namespace WebApp.Data
{
    // Represents an exception that occurs during communication with a web API
    public class WebApiException : Exception
    {
        // Gets or sets the error response received from the API
        public ErrorResponse? ErrorResponse { get; set; }

        // Constructor that initializes a new instance of the WebApiException class with the specified error JSON
        public WebApiException(string errorJson)
        {
            // Deserialize the error JSON into an ErrorResponse object
            ErrorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorJson);
        }
    }

}
