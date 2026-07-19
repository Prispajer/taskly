namespace Taskly.Application.Abstractions.Data;

public interface IQueryExecutor
{
    Task<List<T>> ExecuteListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken);
    Task<T?> ExecuteSingleOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken);
}