using MediatR;
using Taskly.Domain.Todos.Events;

namespace Taskly.Application.Todos.UpdateTodo;

internal sealed class UpdateTaskDomainEventHandler : INotificationHandler<UpdateTaskDomainEvent>
{
    public async Task Handle(UpdateTaskDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}