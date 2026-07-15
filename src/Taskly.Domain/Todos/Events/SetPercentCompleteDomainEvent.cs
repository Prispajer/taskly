using Taskly.SharedKernel.Interfaces;

namespace Taskly.Domain.Todos.Events;

public record SetPercentCompleteDomainEvent(Guid TaskId) : IDomainEvent
{
}   