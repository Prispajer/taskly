using Microsoft.EntityFrameworkCore;
using Taskly.Domain.Todos;
using Taskly.SharedKernel.Common;
using Taskly.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.MarkTodoAsDone
{
    // Handles MarkTodoAsDoneCommand
    public sealed class MarkTodoAsDoneCommandHandler(TasklyDbContext context)
        : ICommandHandler<MarkTodoAsDoneCommand>
    {
        public async Task<Result> Handle(MarkTodoAsDoneCommand command, CancellationToken cancellationToken)
        {
            // Fetch todo item by ID
            Todo? todoItem = await context.Todos
                .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

            // Return error if not found
            if (todoItem is null)
            {
                return Result.Failure(
                    Error.NotFound("Todo.NotFound", $"The todo item with Id = {command.Id} was not found")
                );
            }

            // Mark as done
            todoItem.MarkAsDone();
            
            // Save changes
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
