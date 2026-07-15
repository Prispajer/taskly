using Taskly.SharedKernel.Interfaces;

namespace Taskly.Domain.Todos.Events;

public record UpdateTaskDomainEvent(Guid TaskId, DateTime Expiry) : IDomainEvent
{
}