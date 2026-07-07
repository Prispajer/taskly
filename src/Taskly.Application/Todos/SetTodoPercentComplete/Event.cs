using MediatR;
using Taskly.Domain.Todos.Events;

namespace Taskly.Application.Todos.SetTodoPercentComplete;

internal sealed class SetPercentCompleteDomainEventHandler : INotificationHandler<SetPercentCompleteDomainEvent>
{
    public async Task Handle(SetPercentCompleteDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}