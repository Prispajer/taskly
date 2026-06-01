using Microsoft.EntityFrameworkCore;
using Taskly.Domain.Todos;
using Taskly.SharedKernel.Common;
using Taskly.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.DeleteTodo
{
    // Handles DeleteTodoCommand
    public sealed class DeleteTodoCommandHandler(TasklyDbContext context)
        : ICommandHandler<DeleteTodoCommand>
    {
        public async Task<Result> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
        {
            // Fetch todo item by ID
            Todo? todoItem = await context.Todos
                .SingleOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

            // Return error if not found
            if (todoItem is null)
            {
                return Result.Failure(Error.NotFound(
                    "Todo.NotFound",
                    $"The todo item with Id = {command.Id} was not found"));
            }

            // Remove item from DB
            context.Todos.Remove(todoItem);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
