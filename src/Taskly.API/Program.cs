global using Microsoft.EntityFrameworkCore;
using Taskly.API.Extensions;
using Taskly.API.Startup;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterEndpoints();       // Maps your API endpoints
builder.RegisterServices();        // Adds DI services (DbContext, Swagger, etc.)
var app = builder.Build();
app.ConfigureMiddleware();         // Adds Swagger, HTTPS redirection
app.MapEndpoints();                // Maps routes
await app.InitializeAsync();       // Runs DB migrations
app.Run();

public partial class Program { }