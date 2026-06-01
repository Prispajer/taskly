using Microsoft.EntityFrameworkCore;
using Taskly.SharedKernel.Common;
using Taskly.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.GetIncomingTodos
{
    // Handles GetIncomingTodosCommand
    public class GetIncomingTodoHandler(TasklyDbContext context)
        : IQueryHandler<GetIncomingTodosQuery, List<TodoResponse>>
    {
        public async Task<Result<List<TodoResponse>>> Handle(GetIncomingTodosQuery query, CancellationToken cancellationToken)
        {
            // Normalize current date
            var now = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Unspecified);
            DateTime from;
            DateTime to;

            // Determine date range based on command
            switch (query.Range)
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

            // Fetch todos within range and map to response DTO
            List<TodoResponse> todos = await context.Todos
                .Where(t => t.Expiry >= from && t.Expiry < to)
                .OrderBy(t => t.Expiry)
                .Select(t => new TodoResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Expiry = t.Expiry,
                    PercentComplete = t.PercentComplete,
                })
                .ToListAsync(cancellationToken);

            return Result.Success(todos);
        }
    }
}
