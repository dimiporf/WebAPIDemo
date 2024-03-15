using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebAPIDemo.Data;
using WebAPIDemo.Filters.OperationFilters;

var builder = WebApplication.CreateBuilder(args);

// AddDbContext method to register ApplicationDbContext with the dependency injection container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Configure DbContext to use SQL Server with the connection string "ShirtStoreManagement".
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShirtStoreManagement"));
});


// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    // Assume default version when not specified
    options.AssumeDefaultVersionWhenUnspecified = true;

    // Set the default API version to 1.0
    options.DefaultApiVersion = new ApiVersion(1, 0);
});



// Add API explorer services
builder.Services.AddEndpointsApiExplorer();

// Add Swagger generation services
builder.Services.AddSwaggerGen(c =>
{
    // Apply the AuthorizationHeaderOperationFilter to add Bearer token authorization to all Swagger operations
    c.OperationFilter<AuthorizationHeaderOperationFilter>();

    // Define the security definition for Bearer token authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        // Specify the authentication scheme as Bearer
        Scheme = "Bearer",

        // Set the type of security scheme to HTTP
        Type = SecuritySchemeType.Http,

        // Specify the format of the Bearer token as JWT (JSON Web Token)
        BearerFormat = "JWT",

        // Define the location of the Bearer token, which is in the header of the HTTP request
        In = ParameterLocation.Header
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    // Enable Swagger middleware to serve the generated Swagger JSON document
    app.UseSwagger();

    // Enable Swagger UI middleware to serve the Swagger UI interactive documentation
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();


app.MapControllers();



app.Run();


