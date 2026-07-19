using Taskly.Application.Abstractions.Data;
using Taskly.SharedKernel.Common;
using Taskly.Application.Abstractions.Messaging;
using Taskly.Application.Todos.Common;
using Taskly.Application.Todos.Extensions;

namespace Taskly.Application.Todos.GetIncomingTodos
{
    // Handles GetIncomingTodosCommand
    public class GetIncomingTodoHandler(ITodoRepository repository, IQueryExecutor queryExecutor, TodoDateRangeCalculator todoDateRangeCalculator)
        : IQueryHandler<GetIncomingTodosQuery, List<TodoResponse>>
    {
        public async Task<Result<List<TodoResponse>>> Handle(GetIncomingTodosQuery query, CancellationToken cancellationToken)
        {
            // Calculate data for incoming todos
            var (from, to) = todoDateRangeCalculator.Calculate(query.Range);

            // Query todos within range provided by statement
            var queryable = repository.GetQueryable()
                .Where(x => x.Expiry.Value >= from && x.Expiry.Value < to)
                .OrderBy(x => x.Expiry.Value)
                .ToResponse(); 
            
            // Fetch todos within range and map to response DTO
            var todos = await queryExecutor.ExecuteListAsync(queryable, cancellationToken);
                
            return Result.Success(todos);
        }
    }
}
