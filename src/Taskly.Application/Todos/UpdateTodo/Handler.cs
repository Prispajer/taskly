using Microsoft.EntityFrameworkCore;
using Taskly.SharedKernel;
using Tasky.Application.Abstractions.Messaging;

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
            todoItem.Title = command.Title;
            todoItem.Description = command.Description;
            todoItem.Expiry = DateTime.Parse(command.Expiry);

            // Save changes
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
