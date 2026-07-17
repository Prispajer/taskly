using FluentValidation;
using Taskly.API.Infrastructure;
using Taskly.Application.Todos.SetTodoPercentComplete;
using Taskly.Application.Abstractions.Messaging;

namespace Taskly.API.Endpoints.Todos.SetTodoPercentComplete
{
    // Endpoint for updating the completion percentage of a Todo item
    public sealed class SetTodoPercentCompleteEndpoint : IEndpoint
    {
        // Request payload for setting the percent complete value
        private sealed record SetTodoPercentCompleteRequest(
            int PercentComplete
        );

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("todos/{id:guid}/percent-complete", async (
                Guid id,
                SetTodoPercentCompleteRequest request,
                ICommandHandler<SetTodoPercentCompleteCommand> handler,
                IValidator<SetTodoPercentCompleteCommand> validator,
                CancellationToken cancellationToken) =>
            {
                var command = new SetTodoPercentCompleteCommand(id, request.PercentComplete);

                // Validate the command
                var validationResult = await validator.ValidateAsync(command, cancellationToken);
                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                // Execute the command
                var result = await handler.Handle(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent() // Successfully updated
                    : ProblemResults.FromError(result.Error, StatusCodes.Status404NotFound); // Todo not found
            })
            .WithTags("Todos") // Grouped under "Todos" in Swagger UI
            .WithName("SetTodoPercentComplete"); // Unique name for endpoint
        }
    }
}
