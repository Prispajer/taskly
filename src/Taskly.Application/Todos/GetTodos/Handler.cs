using Microsoft.EntityFrameworkCore;
using Taskly.SharedKernel.Common;
using Taskly.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.GetTodos
{
    // Handles GetTodosQuery
    public sealed class GetTodosQueryHandler(TasklyDbContext context)
        : IQueryHandler<GetTodosQuery, List<TodoResponse>>
    {
        public async Task<Result<List<TodoResponse>>> Handle(GetTodosQuery query, CancellationToken cancellationToken)
        {
            // Fetch all todos and map to response DTO
            List<TodoResponse> todos = await context.Todos.Select(t => new TodoResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Expiry = t.Expiry,
                PercentComplete = t.PercentComplete,
            }).ToListAsync(cancellationToken);

            return Result.Success(todos);
        }
    }
}
