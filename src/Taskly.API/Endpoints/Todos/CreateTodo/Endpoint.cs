using FluentValidation;
using Taskly.API.Infrastructure;
using Taskly.Application.Todos.CreateTodo;
using Taskly.Application.Abstractions.Messaging;

namespace Taskly.API.Endpoints.Todos.CreateTodo
{
    // Endpoint for creating a new Todo item
    public sealed class CreateTodoEndpoint : IEndpoint
    {
        // Request payload for creating a Todo
        private sealed record CreateTodoRequest(
            string Title,
            string Description,
            string Expiry
        );

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("todos", async (
                CreateTodoRequest request,
                ICommandHandler<CreateTodoCommand, Guid> handler,
                IValidator<CreateTodoCommand> validator,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateTodoCommand(request.Title, request.Description, request.Expiry);

                // Validate the command
                var validationResult = await validator.ValidateAsync(command, cancellationToken);
                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                // Execute the command
                var result = await handler.Handle(command, cancellationToken);
                return result.IsSuccess
                    ? Results.Created($"/todos/{result.Value}", new { id = result.Value }) // Return created resource
                    : ProblemResults.FromError(result.Error, StatusCodes.Status400BadRequest); // Creation failed
            })
            .WithTags("Todos") // Grouped under "Todos" in Swagger UI
            .WithName("CreateTodo"); // Unique name for endpoint
        }
    }
}
