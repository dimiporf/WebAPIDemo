
namespace WebAPIDemo.Authority
{
    // Static class responsible for managing applications and authentication
    public static class AppRepository
    {
        // Static list to store application details
        private static List<Application> _applications = new List<Application>()
        {
            // Initializing with a sample application
            new Application
            {
                ApplicationId = 1,
                ApplicationName = "MVCWebApp",
                ClientId = "53D3C1E6-4587-4AD5-8C6E-A8E4BD59940E",
                Secret = "0673FC70-0514-4011-B4A3-DF9BC03201BC",
                Scopes = "read,write"
            }
        };
        
        // Method to retrieve application details based on provided clientId
        public static Application? GetApplicationByClientId(string clientId)
        {
            // Returning the first application that matches the provided clientId
            return _applications.FirstOrDefault(x => x.ClientId == clientId);
        }
    }
}
    
