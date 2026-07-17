using FluentValidation;
using Taskly.API.Infrastructure;
using Taskly.Application.Todos.MarkTodoAsDone;
using Taskly.Application.Abstractions.Messaging;

namespace Taskly.API.Endpoints.Todos.MarkTodoAsDone
{
    // Endpoint for marking a Todo item as completed
    public sealed class MarkTodoAsDoneEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("todos/{id:guid}/completed", async (
                Guid id,
                ICommandHandler<MarkTodoAsDoneCommand> handler,
                IValidator<MarkTodoAsDoneCommand> validator,
                CancellationToken cancellationToken) =>
            {
                var command = new MarkTodoAsDoneCommand(id);

                // Validate the command
                var validationResult = await validator.ValidateAsync(command, cancellationToken);
                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                // Execute the command
                var result = await handler.Handle(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent() // Successfully marked as completed
                    : ProblemResults.FromError(result.Error, StatusCodes.Status404NotFound); // Todo not found
            })
            .WithTags("Todos") // Grouped under "Todos" in Swagger UI
            .WithName("MarkTodoAsDone"); // Unique name for endpoint
        }
    }
}
