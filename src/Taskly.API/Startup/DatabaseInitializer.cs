namespace Taskly.API.Startup
{
    // Applies pending EF Core migrations at startup
    public static class DatabaseInitializer
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TasklyDbContext>();
            await db.Database.MigrateAsync(); // Ensures database schema is up to date
        }
    }
}
