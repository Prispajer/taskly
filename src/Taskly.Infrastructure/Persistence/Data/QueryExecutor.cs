using Microsoft.EntityFrameworkCore;
using Taskly.Application.Abstractions.Data;

namespace Taskly.Infrastructure.Persistence.Data;

public sealed class QueryExecutor : IQueryExecutor {
    public async Task<List<T>> ExecuteListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken) => await query.ToListAsync(cancellationToken);
    public async Task<T?> ExecuteSingleOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken) => await query.SingleOrDefaultAsync(cancellationToken);
} 