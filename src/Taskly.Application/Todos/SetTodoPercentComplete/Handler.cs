using Microsoft.EntityFrameworkCore;
using Taskly.SharedKernel.Common;
using Taskly.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.SetTodoPercentComplete
{
    // Handles SetTodoPercentCompleteCommand
    public sealed class SetTodoPercentCompleteCommandHandler(TasklyDbContext context)
        : ICommandHandler<SetTodoPercentCompleteCommand>
    {
        public async Task<Result> Handle(
            SetTodoPercentCompleteCommand command,
            CancellationToken cancellationToken)
        {
            // Fetch todo item by ID
            var todo = await context.Todos
                .SingleOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

            // Return error if not found
            if (todo is null)
            {
                return Result.Failure(Error.NotFound(
                    "Todo.NotFound",
                    $"The todo item with Id = {command.Id} was not found"));
            }
            
            // Update PercentComplete
            var updatedPercentComplete = todo.SetPercentComplete(command.PercentComplete);
            
            if (!updatedPercentComplete.IsSuccess)
            {
                return Result.Failure(updatedPercentComplete.Error);
            }

            // Save changes
            await context.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
    }
}
