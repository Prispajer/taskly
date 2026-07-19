using Taskly.Application.Todos.GetIncomingTodos;
using Taskly.Domain.Interfaces;

namespace Taskly.Application.Todos.Extensions;

public sealed class TodoDateRangeCalculator(IDateTimeProvider dateTimeProvider) 
{
    public (DateTime from, DateTime to) Calculate(IncomingTodoRange range)
    {
        // Get current date starting on new day (00:00:00)
        var now = dateTimeProvider.UtcNow.Date;
        DateTime from;
        DateTime to;

        // Determine date range based on command
        switch (range)
        {
            case IncomingTodoRange.Today:
                from = now;
                to = now.AddDays(1);
                break;
            case IncomingTodoRange.NextDay:
                from = now.AddDays(1);
                to = now.AddDays(2);
                break;
            case IncomingTodoRange.CurrentWeek:
                from = now;
                to = now.AddDays(((int)DayOfWeek.Sunday - (int)now.DayOfWeek + 7) % 7);
                break;
            default:
                from = now;
                to = now.AddDays(1);
                break;
        }
        
        return (from, to);
    }
}