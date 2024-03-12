namespace WebAPIDemo.Authority
{
    // Represents an application entity in the system
    public class Application
    {
        // Unique identifier for the application
        public int ApplicationId { get; set; }

        // Name of the application
        public string? ApplicationName { get; set; }

        // Client identifier for the application
        public string? ClientId { get; set; }

        // Secret key associated with the application
        public string? Secret { get; set; }

        // Scopes or permissions associated with the application
        public string? Scopes { get; set; }
    }
}
