using Microsoft.EntityFrameworkCore;
using Taskly.SharedKernel.Common;
using Taskly.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.UpdateTodo
{
    // Handles UpdateTodoCommand
    public sealed class UpdateTodoCommandHandler(TasklyDbContext context)
        : ICommandHandler<UpdateTodoCommand>
    {
        public async Task<Result> Handle(UpdateTodoCommand command, CancellationToken cancellationToken)
        {
            // Fetch todo item by ID
            var todoItem = await context.Todos
                .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

            // Return error if not found
            if (todoItem is null)
            {
                return Result.Failure(
                    Error.NotFound("Todo.NotFound", $"The todo item with Id = {command.Id} was not found")
                );
            }

            // Update fields
            var updatedResult = todoItem.Update(command.Title, command.Description, command.Expiry);

            if (!updatedResult.IsSuccess)
            {
                return Result.Failure(updatedResult.Error);
            }

            // Save changes
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
