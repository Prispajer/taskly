using Taskly.Application.Abstractions.Data;
using Taskly.SharedKernel.Common;
using Taskly.Application.Abstractions.Messaging;
using Taskly.Application.Todos.Common;
using Taskly.Application.Todos.Extensions;

namespace Taskly.Application.Todos.GetTodoById
{
    // Handles GetTodoByIdQuery
    public sealed class GetTodoByIdQueryHandler(ITodoRepository repository, IQueryExecutor queryExecutor)
        : IQueryHandler<GetTodoByIdQuery, TodoResponse>
    {
        public async Task<Result<TodoResponse>> Handle(GetTodoByIdQuery query, CancellationToken cancellationToken)
        {
            // Fetch todo item by ID with EF Core projection to DTO
            var todoItem = await queryExecutor.ExecuteSingleOrDefaultAsync(
                repository.GetQueryable().Where(x => x.Id == query.Id).ToResponse(),
                cancellationToken);

            // Return result based on presence
            return todoItem is null
                ? Result.Failure<TodoResponse>(Error.NotFound("Todo.NotFound", $"The todo item with Id = {query.Id} was not found"))
                : Result.Success(todoItem);
        }
    }
}
