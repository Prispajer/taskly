using FluentValidation;
using Taskly.API.Infrastructure;
using Taskly.Application.Todos.UpdateTodo;
using Taskly.Application.Abstractions.Messaging;

namespace Taskly.API.Endpoints.Todos.UpdateTodo
{
    // Endpoint for updating an existing Todo item
    public sealed class UpdateTodoEndpoint : IEndpoint
    {
        // Request payload for updating a Todo
        private sealed record UpdateTodoRequest(
            string Title,
            string Description,
            string Expiry
        );

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("todos/{id:guid}", async (
                Guid id,
                UpdateTodoRequest request,
                ICommandHandler<UpdateTodoCommand> handler,
                IValidator<UpdateTodoCommand> validator,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateTodoCommand(id, request.Title, request.Description, request.Expiry);

                // Validate the command
                var validationResult = await validator.ValidateAsync(command, cancellationToken);
                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                // Execute the command
                var result = await handler.Handle(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent() // Success: no content returned
                    : ProblemResults.FromError(result.Error, StatusCodes.Status404NotFound); // Failure: return error
            })
            .WithTags("Todos")     // Swagger tag
            .WithName("UpdateTodo"); // Endpoint name
        }
    }
}
