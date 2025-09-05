global using Microsoft.EntityFrameworkCore;
using Taskly.API.Startup;


var builder = WebApplication.CreateBuilder(args);
builder.RegisterServices();
var app = builder.Build();
app.ConfigureMiddleware();
await app.InitializeAsync(); 
app.Run();

