namespace Taskly.API.Endpoints
{
    // Interface for modular endpoint registration
    public interface IEndpoint
    {
        // Maps the endpoint to the application's route builder
        void MapEndpoint(IEndpointRouteBuilder app);
    }
}
