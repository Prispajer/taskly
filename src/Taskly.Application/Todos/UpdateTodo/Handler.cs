using Taskly.Application.Abstractions.Data;
using Taskly.SharedKernel.Common;
using Taskly.Application.Abstractions.Messaging;
using Taskly.Domain.Interfaces;
using Taskly.Domain.Todos.ValueObjects;

namespace Taskly.Application.Todos.UpdateTodo
{
    // Handles UpdateTodoCommand
    public sealed class UpdateTodoCommandHandler(ITodoRepository repository, IQueryExecutor queryExecutor, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        : ICommandHandler<UpdateTodoCommand>
    {
        public async Task<Result> Handle(UpdateTodoCommand command, CancellationToken cancellationToken)
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

            // Parse and validate expiry
            var expiryResult = Expiry.Parse(command.Expiry, dateTimeProvider.UtcNow);
            if (!expiryResult.IsSuccess)
                return Result.Failure(expiryResult.Error);

            // Update fields
            var updatedResult = todoItem.Update(command.Title, command.Description, expiryResult.Value);

            if (!updatedResult.IsSuccess)
            {
                return Result.Failure(updatedResult.Error);
            }

            // Save changes
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
