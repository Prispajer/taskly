using Taskly.SharedKernel.Interfaces;

namespace Taskly.Domain.Todos.Events;

public record TaskCreatedEvent(Guid TaskId, DateTime CreatedAt) : IDomainEvent
{
    public Guid Id { get; init; } =  Guid.NewGuid();
    public DateTime OccurredOn { get; init; } =  DateTime.UtcNow;
}   