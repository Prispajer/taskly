using FluentValidation;
using Taskly.API.Infrastructure;
using Taskly.Application.Todos.GetIncomingTodo;
using Tasky.Application.Abstractions.Messaging;

namespace Taskly.API.Endpoints.Todos.GetIncomingTodos
{
    // Endpoint for retrieving upcoming Todo items within a specified range
    public class GetIncomingTodosEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/todos/incoming/{range}", async (
               IncomingTodoRange range,
               IQueryHandler<GetIncomingTodosQuery, List<TodoResponse>> handler,
               IValidator<GetIncomingTodosQuery> validator,
               CancellationToken cancellationToken) =>
            {
                var command = new GetIncomingTodosQuery(range);

                // Validate the query
                var validationResult = await validator.ValidateAsync(command, cancellationToken);
                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                // Execute the query
                var result = await handler.Handle(command, cancellationToken);
                return result.IsSuccess
                    ? Results.Ok(result.Value) // Return filtered list of upcoming todos
                    : ProblemResults.FromError(result.Error, StatusCodes.Status400BadRequest); // Query failed
            })
           .WithTags("Todos") // Grouped under "Todos" in Swagger UI
           .WithName("GetIncomingTodos"); // Unique name for endpoint
        }
    }
}
