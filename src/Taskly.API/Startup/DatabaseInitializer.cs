namespace Taskly.API.Startup

{
    public static class DatabaseInitializer
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TasklyDbContext>();
            await db.Database.MigrateAsync();
        }
    }
}