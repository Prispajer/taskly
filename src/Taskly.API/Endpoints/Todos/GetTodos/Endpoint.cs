using FluentValidation;
using Taskly.API.Infrastructure;
using Taskly.Application.Todos.GetTodos;
using Taskly.Application.Abstractions.Messaging;
using Taskly.Application.Todos.Common;

namespace Taskly.API.Endpoints.Todos.GetTodos
{
    // Endpoint for retrieving all Todo items
    public sealed class GetTodosEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("todos", async (
                IQueryHandler<GetTodosQuery, List<TodoResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                var query = new GetTodosQuery();

                // Execute the query
                var result = await handler.Handle(query, cancellationToken);
                return result.IsSuccess
                    ? Results.Ok(result.Value) // Return list of todos
                    : ProblemResults.FromError(result.Error, StatusCodes.Status400BadRequest); // Query failed
            })
            .WithTags("Todos") // Grouped under "Todos" in Swagger UI
            .WithName("GetTodos"); // Unique name for endpoint
        }
    }
}
