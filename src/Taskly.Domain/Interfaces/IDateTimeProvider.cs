namespace Taskly.Domain.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}