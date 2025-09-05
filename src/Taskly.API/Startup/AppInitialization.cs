namespace Taskly.API.Startup

{
    public static class AppInitialization
    {
        public static async Task InitializeAsync(this WebApplication app)
        {
            await app.InitializeDatabaseAsync();
        }
    }
}
