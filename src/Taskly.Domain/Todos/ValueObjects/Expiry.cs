using Taskly.Domain.Exceptions;

namespace Taskly.Domain.Todos.ValueObjects;

public record Expiry
{
    public DateTime Value { get; }
    
    private Expiry(DateTime value) => Value = value;

    public static Expiry Create(DateTime value) => new Expiry(value);

    public static Expiry Parse(string value, DateTime now)
    {
        if (!DateTime.TryParse(value, out var parsed))
            throw new DomainException("Expiry date format is invalid");

        if (parsed < now)
            throw new DomainException("Expiry date cannot be in the past");

        return new Expiry(parsed);
    }
}