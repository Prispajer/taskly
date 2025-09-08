namespace Taskly.API.Startup
{
    // Wraps all app-level initialization logic
    public static class AppInitialization
    {
        public static async Task InitializeAsync(this WebApplication app)
        {
            await app.InitializeDatabaseAsync(); // Triggers DB migration on startup
        }
    }
}
