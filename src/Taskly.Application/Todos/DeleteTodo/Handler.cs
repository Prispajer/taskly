using Taskly.Application.Abstractions.Data;
using Taskly.SharedKernel.Common;
using Taskly.Application.Abstractions.Messaging;
using Taskly.Domain.Todos.Entities;

namespace Taskly.Application.Todos.DeleteTodo
{
    // Handles DeleteTodoCommand
    public sealed class DeleteTodoCommandHandler(ITodoRepository repository, IQueryExecutor executor, IUnitOfWork unitOfWork)
        : ICommandHandler<DeleteTodoCommand>
    {
        public async Task<Result> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
        {
            // Fetch todo item by ID
            var queryable = repository.GetQueryable().Where(x => x.Id == command.Id);
            var todoItem = await executor.ExecuteSingleOrDefaultAsync(queryable, cancellationToken);

            // Return error if not found
            if (todoItem is null)
            {
                return Result.Failure(Error.NotFound(
                    "Todo.NotFound",
                    $"The todo item with Id = {command.Id} was not found"));
            }

            // Remove item from DB
            repository.Remove(todoItem);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
