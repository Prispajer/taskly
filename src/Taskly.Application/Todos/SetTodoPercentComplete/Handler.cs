using Taskly.Application.Abstractions.Data;
using Taskly.SharedKernel.Common;
using Taskly.Application.Abstractions.Messaging;
using Taskly.Domain.Todos.ValueObjects;

namespace Taskly.Application.Todos.SetTodoPercentComplete
{
    // Handles SetTodoPercentCompleteCommand
    public sealed class SetTodoPercentCompleteCommandHandler(ITodoRepository repository, IQueryExecutor queryExecutor, IUnitOfWork unitOfWork)
        : ICommandHandler<SetTodoPercentCompleteCommand>
    {
        public async Task<Result> Handle(
            SetTodoPercentCompleteCommand command,
            CancellationToken cancellationToken)
        {
            // Fetch todo item by ID using queryable pattern
            var todoItem = await queryExecutor.ExecuteSingleOrDefaultAsync(
                repository.GetQueryable().Where(x => x.Id == command.Id),
                cancellationToken);

            // Return error if not found
            if (todoItem is null)
            {
                return Result.Failure(Error.NotFound(
                    "Todo.NotFound",
                    $"The todo item with Id = {command.Id} was not found"));
            }

            // Parse and validate percent value
            var percentResult = Percent.Create(command.PercentComplete);
            if (!percentResult.IsSuccess)
                return Result.Failure(percentResult.Error);

            // Update PercentComplete
            var updatedPercentComplete = todoItem.SetPercentComplete(percentResult.Value);
            
            if (!updatedPercentComplete.IsSuccess)
            {
                return Result.Failure(updatedPercentComplete.Error);
            }

            // Save changes
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
    }
}
