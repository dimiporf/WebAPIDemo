﻿namespace WebAPIDemo.Authority
{
    // Represents credentials for authenticating an application
    public class AppCredential
    {
        // Client ID associated with the application
        public string ClientId { get; set; } = string.Empty;

        // Secret key associated with the application
        public string Secret { get; set; } = string.Empty;
    }
}
