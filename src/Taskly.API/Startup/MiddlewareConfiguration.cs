namespace Taskly.API.Startup
{
    // Configures middleware for the HTTP request pipeline
    public static class MiddlewareConfiguration
    {
        public static void ConfigureMiddleware(this WebApplication app)
        {
            app.UseSwagger();        // Enables Swagger JSON endpoint
            app.UseSwaggerUI();      // Enables Swagger UI for interactive docs
            app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS
        }
    }
}
