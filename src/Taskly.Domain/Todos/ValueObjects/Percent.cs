using Taskly.SharedKernel.Common;

namespace Taskly.Domain.Todos.ValueObjects;

public record Percent
{
    public int Value { get; }
    
    public static readonly Percent Zero = new(0);
    public static readonly Percent Completed = new(100);
    
    private Percent(int value) => Value = value;

    public static Result<Percent> Create(int value)
    {
        if (value is < 0 or > 100)
            return Result.Failure<Percent>(
                Error.BadRequest("Percent.OutOfRange", "Percent value must be between 0 and 100"));
        
        return Result.Success(new Percent(value));
    }
}
