namespace Taskly.API.Startup

{
    public static class DependencyInjection
    {   
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            builder.RegisterDatabase();
            builder.RegisterSwagger();
        }

        private static void RegisterDatabase(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<TasklyDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionString")));
        }

        private static void RegisterSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
    }
}
