using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Data;

var builder = WebApplication.CreateBuilder(args);

// AddDbContext method to register ApplicationDbContext with the dependency injection container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Configure DbContext to use SQL Server with the connection string "ShirtStoreManagement".
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShirtStoreManagement"));
});


// Add services to the container.
builder.Services.AddControllers();

// Add API explorer services
builder.Services.AddEndpointsApiExplorer();

// Add Swagger generation services
builder.Services.AddSwaggerGen();


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


