
using EarbudExplorer.Data;
using Microsoft.EntityFrameworkCore;

// Creates the application builder.
// This loads configuration settings and registers services.
var builder = WebApplication.CreateBuilder(args);

// Registers ASP.NET Core MVC.
// This allows our application to use Models, Views, and Controllers.
builder.Services.AddControllersWithViews();

// Registers our database context with dependency injection.
//
// UseSqlServer tells Entity Framework Core that our database
// is Microsoft SQL Server.
//
// DefaultConnection is read from appsettings.json.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Builds the application using the configured services.
var app = builder.Build();

// Configures error handling for production environments.
if (!app.Environment.IsDevelopment())
{
    // Displays the Error page when an unhandled error occurs.
    app.UseExceptionHandler("/Home/Error");

    // Enables HTTP Strict Transport Security.
    app.UseHsts();
}

// Redirects HTTP requests to HTTPS when configured.
app.UseHttpsRedirection();

// Allows the application to serve CSS, JavaScript, and images
// from the wwwroot folder.
app.UseStaticFiles();

// Enables routing so URLs can reach the correct controller.
app.UseRouting();

// Enables the authorization middleware.
// Authentication and Identity will be configured later.
app.UseAuthorization();

// Defines the default MVC route.
//
// Example:
// /Earbuds/Index
//
// If no controller or action is specified,
// the application opens HomeController's Index action.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Starts the application and listens for incoming requests.
app.Run();
