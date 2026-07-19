using Taskly.Application.Abstractions.Data;
using Taskly.SharedKernel.Common;
using Taskly.Application.Abstractions.Messaging;
using Taskly.Domain.Interfaces;
using Taskly.Domain.Todos.Entities;
using Taskly.Domain.Todos.ValueObjects;

namespace Taskly.Application.Todos.CreateTodo
{
    // Handles CreateTodoCommand
    public sealed class CreateTodoCommandHandler(ITodoRepository repository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        : ICommandHandler<CreateTodoCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
        {
            // Get current data time
            var now = dateTimeProvider.UtcNow;
            
            // Check expiry correctness
            var expiryResult = Expiry.Parse(command.Expiry, now);
            if (!expiryResult.IsSuccess)
                return Result.Failure<Guid>(expiryResult.Error);
            
            // Create todo from rich domain model
            var todoItem = Todo.Create(command.Title, command.Description, expiryResult.Value);

            if (!todoItem.IsSuccess)
            {
                return Result.Failure<Guid>(todoItem.Error);
            }

            // Save to database
            repository.Add(todoItem.Value);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Success(todoItem.Value.Id);
        }
    }
}
