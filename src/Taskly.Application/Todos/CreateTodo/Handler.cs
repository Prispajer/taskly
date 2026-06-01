using Taskly.Domain.Todos;
using Taskly.SharedKernel.Common;
using Taskly.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.CreateTodo
{
    // Handles CreateTodoCommand
    public sealed class CreateTodoCommandHandler(TasklyDbContext context)
        : ICommandHandler<CreateTodoCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
        {
            // Create todo from rich domain model
            var todoItem = Todo.Create(command.Title, command.Description, command.Expiry);

            if (!todoItem.IsSuccess)
            {
                return Result.Failure<Guid>(todoItem.Error);
            }

            // Save to database
            context.Todos.Add(todoItem.Value);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(todoItem.Value.Id);
        }
    }
}
