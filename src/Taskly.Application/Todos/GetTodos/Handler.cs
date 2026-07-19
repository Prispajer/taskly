using Taskly.Application.Abstractions.Data;
using Taskly.SharedKernel.Common;
using Taskly.Application.Abstractions.Messaging;
using Taskly.Application.Todos.Common;
using Taskly.Application.Todos.Extensions;

namespace Taskly.Application.Todos.GetTodos
{
    // Handles GetTodosQuery
    public sealed class GetTodosQueryHandler(ITodoRepository repository, IQueryExecutor executor)
        : IQueryHandler<GetTodosQuery, List<TodoResponse>>
    {
        public async Task<Result<List<TodoResponse>>> Handle(GetTodosQuery query, CancellationToken cancellationToken)
        {
            // Fetch all todos and map to response DTO
            var queryable = repository.GetQueryable().ToResponse();
            var todos = await executor.ExecuteListAsync(queryable, cancellationToken);

            return Result.Success(todos);
        }
    }
}
