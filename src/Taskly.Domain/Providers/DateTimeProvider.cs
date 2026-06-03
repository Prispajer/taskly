using Taskly.Domain.Interfaces;

namespace Taskly.Domain.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}