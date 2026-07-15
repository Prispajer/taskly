using Taskly.SharedKernel.Interfaces;

namespace Taskly.Domain.Todos.Events;

public record TaskCreatedDomainEvent(Guid TaskId, DateTime CreatedAt) : IDomainEvent
{
}   