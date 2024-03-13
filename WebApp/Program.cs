using WebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services for dependency injection.
builder.Services.AddHttpClient("ShirtsApi", client =>
{
    // Configure HttpClient to use the base address of the shirts API.
    client.BaseAddress = new Uri("https://localhost:7281/api/");
    // Add an Accept header to indicate that the client expects JSON responses.
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Add services for authorization implementation.
builder.Services.AddHttpClient("AuthorityApi", client =>
{
    // Configure HttpClient to use the base address of the authority API.
    client.BaseAddress = new Uri("https://localhost:7281/");
    // Add an Accept header to indicate that the client expects JSON responses.
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Register the WebApiExecuter as a transient service for dependency injection.
builder.Services.AddTransient<IWebApiExecuter, WebApiExecuter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // Use exception handling middleware to handle errors and display error pages.
    app.UseExceptionHandler("/Home/Error");
    // Use HTTP Strict Transport Security (HSTS) middleware to enforce HTTPS and prevent man-in-the-middle attacks.
    app.UseHsts();
}

// Enable HTTPS redirection middleware to redirect HTTP requests to HTTPS.
app.UseHttpsRedirection();
// Enable serving of static files like CSS, JavaScript, and images.
app.UseStaticFiles();

app.UseRouting();

// Enable authorization middleware.
app.UseAuthorization();

// Configure controller routing.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Start the application.
app.Run();
