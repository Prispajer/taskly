namespace Taskly.SharedKernel.Interfaces;

public interface IDomainEvent
{
    public Guid Id { get; init; }
    public DateTime OccurredOn { get; init; }
}