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

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.MapControllers();



app.Run();


