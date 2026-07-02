using FluentValidation;
using Taskly.Application;
using Taskly.Application.Abstractions.Data;
using Taskly.Application.Abstractions.Messaging;
using Taskly.Domain.Interfaces;
using Taskly.Domain.Providers;
using Taskly.Infrastructure.Interceptors;
using Taskly.Infrastructure.Persistence;
using Taskly.Infrastructure.Persistence.Repositories;
using Taskly.SharedKernel.Interfaces;

namespace Taskly.API.Startup
{
    // Registers application services and dependencies
    public static class DependencyInjection
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services
                .RegisterDatabase(builder.Configuration) // EF Core setup
                .RegisterSwagger()                      // Swagger setup
                .RegisterHandlers()                    // CQRS handlers + validators
                .RegisterDependencies();              // Dependencies
        }

        // Configures EF Core with PostgreSQL
        private static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TasklyDbContext>((sp, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("ConnectionString"));
                var interceptor = sp.GetRequiredService<DomainEventPublishingInterceptor>();
                options.AddInterceptors(interceptor);
            });
            return services;
        }

        // Adds Swagger services for API documentation
        private static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        // Registers CQRS handlers and validators using Scrutor
        private static IServiceCollection RegisterHandlers(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromApplicationDependencies(assembly => assembly.FullName != null && assembly.FullName.StartsWith("Taskly"))
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                    .AsImplementedInterfaces()
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                    .AsImplementedInterfaces()
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                    .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            services.AddValidatorsFromAssemblyContaining(typeof(ValidationMarker)); // FluentValidation setup
            return services;
        }

        private static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TasklyDbContext>());
            services.AddScoped<DomainEventPublishingInterceptor>();
            
            return services;
        }
    }
}
