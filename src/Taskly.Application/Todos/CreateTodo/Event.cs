using MediatR;
using Taskly.Domain.Todos.Events;

namespace Taskly.Application.Todos.CreateTodo;

internal sealed class TaskCreatedDomainEventHandler : INotificationHandler<TaskCreatedDomainEvent>
{
    public async Task Handle(TaskCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}