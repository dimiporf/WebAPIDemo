using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
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
    // Report available versions
    options.ReportApiVersions = true;

    // Assume default version when not specified
    options.AssumeDefaultVersionWhenUnspecified = true;

    // Set the default API version to 1.0
    options.DefaultApiVersion = new ApiVersion(1, 0);

    // Set the API version reader to read the API version from the "X-API-Version" header
    options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
});


// Configure the API explorer to format group names as 'v' followed by the API version in a readable format
builder.Services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");


// Add Swagger generation services
builder.Services.AddSwaggerGen(c =>
{
    // Define Swagger documentation for API versions
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Web API v1", Version = "version 1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "My Web API v2", Version = "version 2" });


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
    app.UseSwaggerUI(
        options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1");
            options.SwaggerEndpoint("/swagger/v2/swagger.json", "WebAPI v2");
        });
}


app.UseHttpsRedirection();


app.MapControllers();



app.Run();


