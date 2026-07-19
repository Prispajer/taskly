using Taskly.Application.Abstractions.Data;
using Taskly.SharedKernel.Common;
using Taskly.Application.Abstractions.Messaging;
using Taskly.Domain.Todos.Entities;

namespace Taskly.Application.Todos.MarkTodoAsDone
{
    // Handles MarkTodoAsDoneCommand
    public sealed class MarkTodoAsDoneCommandHandler(ITodoRepository repository, IQueryExecutor queryExecutor, IUnitOfWork unitOfWork)
        : ICommandHandler<MarkTodoAsDoneCommand>
    {
        public async Task<Result> Handle(MarkTodoAsDoneCommand command, CancellationToken cancellationToken)
        {
            // Fetch todo item by ID using queryable pattern
            var todoItem = await queryExecutor.ExecuteSingleOrDefaultAsync(
                repository.GetQueryable().Where(x => x.Id == command.Id),
                cancellationToken);

            // Return error if not found
            if (todoItem is null)
            {
                return Result.Failure(
                    Error.NotFound("Todo.NotFound", $"The todo item with Id = {command.Id} was not found")
                );
            }

            // Mark as done - check if domain logic allows it
            var markResult = todoItem.MarkAsDone();
            if (!markResult.IsSuccess)
            {
                return Result.Failure(markResult.Error);
            }

            // Save changes
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
