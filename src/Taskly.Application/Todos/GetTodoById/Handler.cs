using Microsoft.EntityFrameworkCore;
using Taskly.SharedKernel;
using Tasky.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.GetTodoById
{
    // Handles GetTodoByIdQuery
    public sealed class GetTodoByIdQueryHandler(TasklyDbContext context)
        : IQueryHandler<GetTodoByIdQuery, TodoResponse>
    {
        public async Task<Result<TodoResponse>> Handle(GetTodoByIdQuery query, CancellationToken cancellationToken)
        {
            // Fetch todo item by ID and map to response DTO
            var todoItem = await context.Todos
                .Where(t => t.Id == query.Id)
                .Select(t => new TodoResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Expiry = t.Expiry,
                    PercentComplete = t.PercentComplete
                })
                .SingleOrDefaultAsync(cancellationToken);

            // Return result based on presence
            return todoItem is null
                ? Result.Failure<TodoResponse>(Error.NotFound("Todo.NotFound", $"The todo item with Id = {query.Id} was not found"))
                : Result.Success(todoItem);
        }
    }
}
