using FluentValidation;
using Taskly.API.Infrastructure;
using Taskly.Application.Todos.DeleteTodo;
using Taskly.Application.Abstractions.Messaging;

namespace Taskly.API.Endpoints.Todos.DeleteTodo
{
    // Endpoint for deleting a Todo item by its ID
    public sealed class DeleteTodoEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("todos/{id:guid}", async (
                Guid id,
                ICommandHandler<DeleteTodoCommand> handler,
                IValidator<DeleteTodoCommand> validator,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteTodoCommand(id);

                // Validate the command
                var validationResult = await validator.ValidateAsync(command, cancellationToken);
                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                // Execute the command
                var result = await handler.Handle(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent() // Successfully deleted
                    : ProblemResults.FromError(result.Error, StatusCodes.Status404NotFound); // Todo not found
            })
            .WithTags("Todos") // Grouped under "Todos" in Swagger UI
            .WithName("DeleteTodo"); // Unique name for endpoint
        }
    }
}
