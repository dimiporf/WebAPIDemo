﻿namespace WebApp.Data
{
    // Interface defining methods for executing HTTP requests to a web API
    public interface IWebApiExecuter
    {
        Task InvokeDelete(string relativeUrl);

        // Invokes an HTTP GET request to the specified relative URL of the API
        Task<T?> InvokeGet<T>(string relativeUrl);

        // Invokes an HTTP POST request to the specified relative URL of the API with the provided object as content
        Task<T?> InvokePost<T>(string relativeUrl, T obj);

        // Asynchronously sends an HTTP PUT request to the specified relative URL of the API with the provided object as content
        // and returns a Task representing the asynchronous operation
        Task InvokePut<T>(string relativeUrl, T obj);

    }
}
