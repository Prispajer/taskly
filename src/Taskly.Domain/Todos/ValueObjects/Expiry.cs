using Taskly.SharedKernel.Common;

namespace Taskly.Domain.Todos.ValueObjects;

public record Expiry
{
    public DateTime Value { get; }
    
    private Expiry(DateTime value) => Value = value;

    public static Expiry Create(DateTime value) => new Expiry(value);

    public static Result<Expiry> Parse(string value, DateTime now)
    {
        if (!DateTime.TryParse(value, out var parsed))
            return Result.Failure<Expiry>(
                Error.BadRequest("Expiry.InvalidFormat", "Expiry date format is invalid"));

        if (parsed < now)
            return Result.Failure<Expiry>(
                Error.BadRequest("Expiry.InPast", "Expiry date cannot be in the past"));

        return Result.Success(new Expiry(parsed));
    }
}