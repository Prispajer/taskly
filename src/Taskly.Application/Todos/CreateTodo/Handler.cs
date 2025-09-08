using Taskly.Domain.Todos;
using Taskly.SharedKernel;
using Tasky.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.CreateTodo
{
    // Handles CreateTodoCommand
    public sealed class CreateTodoCommandHandler(TasklyDbContext context)
        : ICommandHandler<CreateTodoCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
        {
            // Parse expiry date
            if (!DateTime.TryParse(command.Expiry, out var expiryDate))
                return Result.Failure<Guid>(Error.BadRequest("Todo.BadRequest", "Expiry date format is invalid"));

            // Validate expiry is not in the past
            if (expiryDate < DateTime.UtcNow)
                return Result.Failure<Guid>(Error.BadRequest("Todo.BadRequest", "Expiry date cannot be in the past"));

            // Create new todo item
            var todoItem = new Todo
            {
                Title = command.Title,
                Description = command.Description,
                Expiry = expiryDate,
            };

            // Save to database
            context.Todos.Add(todoItem);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(todoItem.Id);
        }
    }
}
