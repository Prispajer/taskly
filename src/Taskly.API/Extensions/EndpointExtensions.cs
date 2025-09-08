using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Taskly.API.Endpoints;

namespace Taskly.API.Extensions
{
    // Extension methods for registering and mapping modular endpoints
    public static class EndpointExtensions
    {
        // Registers all IEndpoint implementations from the current assembly
        public static void RegisterEndpoints(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
        }

        // Maps all registered endpoints to the route builder (optionally within a route group)
        public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
        {
            IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

            IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

            foreach (IEndpoint endpoint in endpoints)
            {
                endpoint.MapEndpoint(builder); // Delegate mapping logic to each endpoint
            }

            return app;
        }

        // Internal helper to scan assembly and register all non-abstract IEndpoint types
        private static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
        {
            ServiceDescriptor[] endpointServiceDescriptors = assembly.DefinedTypes
                .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoint)))
                .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
                .ToArray();

            services.TryAddEnumerable(endpointServiceDescriptors); // Avoid duplicate registrations

            return services;
        }
    }
}
