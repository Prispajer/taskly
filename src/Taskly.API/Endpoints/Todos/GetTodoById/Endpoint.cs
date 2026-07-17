using FluentValidation;
using Taskly.API.Infrastructure;
using Taskly.Application.Todos.GetTodoById;
using Taskly.Application.Abstractions.Messaging;
using Taskly.Application.Todos.Common;

namespace Taskly.API.Endpoints.Todos.GetTodoById
{
    // Endpoint for retrieving a single Todo item by its ID
    public sealed class GetTodoByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("todos/{id:guid}", async (
                Guid id,
                IQueryHandler<GetTodoByIdQuery, TodoResponse> handler,
                IValidator<GetTodoByIdQuery> validator,
                CancellationToken cancellationToken) =>
            {
                var query = new GetTodoByIdQuery(id);

                // Validate the query
                var validationResult = await validator.ValidateAsync(query, cancellationToken);
                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                // Execute the query
                var result = await handler.Handle(query, cancellationToken);
                return result.IsSuccess
                    ? Results.Ok(result.Value) // Return the todo item
                    : ProblemResults.FromError(result.Error, StatusCodes.Status404NotFound); // Todo not found
            })
            .WithTags("Todos") // Grouped under "Todos" in Swagger UI
            .WithName("GetTodoById"); // Unique name for endpoint
        }
    }
}
